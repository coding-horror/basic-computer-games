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
        private readonly Coordinates _starbaseSector;
        private readonly Galaxy _galaxy;

        public Quadrant(QuadrantInfo info, Enterprise enterprise, Random random, Galaxy galaxy)
        {
            _info = info;
            _random = random;
            _galaxy = galaxy;

            _sectors = new() { [enterprise.Sector] = _enterprise = enterprise };
            PositionObject(sector => new Klingon(sector, _random), _info.KlingonCount);
            if (_info.HasStarbase)
            {
                _starbaseSector = PositionObject(_ => new Starbase(_random, new Input(new Output()), new Output()));
            }
            PositionObject(_ => new Star(), _info.StarCount);
        }

        public object this[Coordinates coordinates] => _sectors.GetValueOrDefault(coordinates);
        public Coordinates Coordinates => _info.Coordinates;
        public bool HasKlingons => _info.KlingonCount > 0;
        public bool HasStarbase => _info.HasStarbase;
        public Starbase Starbase => HasStarbase ? (Starbase)_sectors[_starbaseSector] : null;
        public bool EnterpriseIsNextToStarbase =>
            _info.HasStarbase &&
            Math.Abs(_enterprise.Sector.X - _starbaseSector.X) <= 1 &&
            Math.Abs(_enterprise.Sector.Y - _starbaseSector.Y) <= 1;

        private IEnumerable<Klingon> Klingons => _sectors.Values.OfType<Klingon>();

        public override string ToString() => _info.Name;

        private Coordinates PositionObject(Func<Coordinates, object> objectFactory)
        {
            var sector = GetRandomEmptySector();
            _sectors[sector] = objectFactory.Invoke(sector);
            return sector;
        }

        private void PositionObject(Func<Coordinates, object> objectFactory, int count)
        {
            for (int i = 0; i < count; i++)
            {
                PositionObject(objectFactory);
            }
        }

        internal bool TorpedoCollisionAt(Coordinates coordinates, out string message, out bool gameOver)
        {
            gameOver = false;
            message = default;

            switch (_sectors.GetValueOrDefault(coordinates))
            {
                case Klingon _:
                    _sectors.Remove(coordinates);
                    _info.RemoveKlingon();
                    message = "*** Klingon destroyed ***";
                    gameOver = _galaxy.KlingonCount == 0;
                    return true;

                case Star _:
                    message = $"Star at {coordinates} absorbed torpedo energy.";
                    return true;

                case Starbase _:
                    _sectors.Remove(coordinates);
                    _info.RemoveStarbase();
                    message = "*** Starbase destroyed ***" +
                        (_galaxy.StarbaseCount > 0 ? Strings.CourtMartial : Strings.RelievedOfCommand);
                    gameOver = _galaxy.StarbaseCount == 0;
                    return true;

                default:
                    return false;
            }
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

        public IEnumerable<string> GetDisplayLines() => Enumerable.Range(0, 8).Select(x => GetDisplayLine(x));

        private string GetDisplayLine(int x)
            => string.Join(
                " ",
                Enumerable
                    .Range(0, 8)
                    .Select(y => new Coordinates(x, y))
                    .Select(c => _sectors.GetValueOrDefault(c))
                    .Select(o => o?.ToString() ?? "   "));
    }
}
