using System;
using System.Collections.Generic;
using System.Linq;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;

namespace SuperStarTrek.Space
{
    internal class Quadrant
    {
        private readonly QuadrantInfo _info;
        private readonly Random _random;
        private readonly Dictionary<Coordinates, object> _sectors;
        private readonly Enterprise _enterprise;
        private readonly Output _output;
        private bool _displayed = false;

        internal Quadrant(
            QuadrantInfo info,
            Enterprise enterprise,
            Random random,
            Galaxy galaxy,
            Input input,
            Output output)
        {
            _info = info;
            _random = random;
            _output = output;
            Galaxy = galaxy;

            info.MarkAsKnown();
            _sectors = new() { [enterprise.SectorCoordinates] = _enterprise = enterprise };
            PositionObject(sector => new Klingon(sector, _random), _info.KlingonCount);
            if (_info.HasStarbase)
            {
                Starbase = PositionObject(sector => new Starbase(sector, _random, input, output));
            }
            PositionObject(_ => new Star(), _info.StarCount);
        }

        internal Coordinates Coordinates => _info.Coordinates;

        internal bool HasKlingons => _info.KlingonCount > 0;

        internal int KlingonCount => _info.KlingonCount;

        internal bool HasStarbase => _info.HasStarbase;

        internal Starbase Starbase { get; }

        internal Galaxy Galaxy { get; }

        internal bool EnterpriseIsNextToStarbase =>
            _info.HasStarbase &&
            Math.Abs(_enterprise.SectorCoordinates.X - Starbase.Sector.X) <= 1 &&
            Math.Abs(_enterprise.SectorCoordinates.Y - Starbase.Sector.Y) <= 1;

        internal IEnumerable<Klingon> Klingons => _sectors.Values.OfType<Klingon>();

        public override string ToString() => _info.Name;

        private T PositionObject<T>(Func<Coordinates, T> objectFactory)
        {
            var sector = GetRandomEmptySector();
            _sectors[sector] = objectFactory.Invoke(sector);
            return (T)_sectors[sector];
        }

        private void PositionObject(Func<Coordinates, object> objectFactory, int count)
        {
            for (int i = 0; i < count; i++)
            {
                PositionObject(objectFactory);
            }
        }

        internal void Display(string textFormat)
        {
            if (_displayed) { return; }

            _output.Write(textFormat, this);

            if (_info.KlingonCount > 0)
            {
                _output.Write(Strings.CombatArea);
                if (_enterprise.ShieldControl.ShieldEnergy <= 200) { _output.Write(Strings.LowShields); }
            }

            _enterprise.Execute(Command.SRS);

            _displayed = true;
        }

        internal bool HasObjectAt(Coordinates coordinates) => _sectors.ContainsKey(coordinates);

        internal bool TorpedoCollisionAt(Coordinates coordinates, out string message, out bool gameOver)
        {
            gameOver = false;
            message = default;

            switch (_sectors.GetValueOrDefault(coordinates))
            {
                case Klingon klingon:
                    message = Remove(klingon);
                    gameOver = Galaxy.KlingonCount == 0;
                    return true;

                case Star _:
                    message = $"Star at {coordinates} absorbed torpedo energy.";
                    return true;

                case Starbase _:
                    _sectors.Remove(coordinates);
                    _info.RemoveStarbase();
                    message = "*** Starbase destroyed ***" +
                        (Galaxy.StarbaseCount > 0 ? Strings.CourtMartial : Strings.RelievedOfCommand);
                    gameOver = Galaxy.StarbaseCount == 0;
                    return true;

                default:
                    return false;
            }
        }

        internal string Remove(Klingon klingon)
        {
            _sectors.Remove(klingon.Sector);
            _info.RemoveKlingon();
            return "*** Klingon destroyed ***";
        }

        internal CommandResult KlingonsMoveAndFire()
        {
            foreach (var klingon in Klingons.ToList())
            {
                var newSector = GetRandomEmptySector();
                _sectors.Remove(klingon.Sector);
                _sectors[newSector] = klingon;
                klingon.MoveTo(newSector);
            }

            return KlingonsFireOnEnterprise();
        }

        internal CommandResult KlingonsFireOnEnterprise()
        {
            if (EnterpriseIsNextToStarbase && Klingons.Any())
            {
                Starbase.ProtectEnterprise();
                return CommandResult.Ok;
            }

            foreach (var klingon in Klingons)
            {
                var result = klingon.FireOn(_enterprise);
                if (result.IsGameOver) { return result; }
            }

            return CommandResult.Ok;
        }

        private Coordinates GetRandomEmptySector()
        {
            while (true)
            {
                var sector = _random.GetCoordinate();
                if (!_sectors.ContainsKey(sector))
                {
                    return sector;
                }
            }
        }

        internal IEnumerable<string> GetDisplayLines() => Enumerable.Range(0, 8).Select(x => GetDisplayLine(x));

        private string GetDisplayLine(int x) =>
            string.Join(
                " ",
                Enumerable
                    .Range(0, 8)
                    .Select(y => new Coordinates(x, y))
                    .Select(c => _sectors.GetValueOrDefault(c))
                    .Select(o => o?.ToString() ?? "   "));

        internal void SetEnterpriseSector(Coordinates sector)
        {
            _sectors.Remove(_enterprise.SectorCoordinates);
            _sectors[sector] = _enterprise;
        }
    }
}
