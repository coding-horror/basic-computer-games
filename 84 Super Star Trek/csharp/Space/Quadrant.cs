using System;
using System.Collections.Generic;
using System.Linq;
using SuperStarTrek.Objects;

namespace SuperStarTrek.Space
{
    internal class Quadrant
    {
        private readonly QuadrantInfo _info;
        private readonly Random _random;
        private readonly Dictionary<Coordinates, object> _sectors;
        private readonly Coordinates _enterpriseSector;
        private readonly Coordinates _starbaseSector;

        public Quadrant(QuadrantInfo info, Enterprise enterprise)
        {
            _info = info;
            _random = new Random();

            _enterpriseSector = enterprise.Sector;
            _sectors = new Dictionary<Coordinates, object> { [enterprise.Sector] = enterprise };
            PositionObject(() => new Klingon(), _info.KlingonCount);
            if (_info.HasStarbase)
            {
                _starbaseSector = PositionObject(() => new Starbase());
            }
            PositionObject(() => new Star(), _info.StarCount);
        }

        public Coordinates Coordinates => _info.Coordinates;
        public bool HasKlingons => _info.KlingonCount > 0;
        public bool HasStarbase => _info.HasStarbase;
        public bool EnterpriseIsNextToStarbase =>
            _info.HasStarbase &&
            Math.Abs(_enterpriseSector.X - _starbaseSector.X) <= 1 &&
            Math.Abs(_enterpriseSector.Y - _starbaseSector.Y) <= 1;

        public override string ToString() => _info.Name;

        private Coordinates PositionObject(Func<object> objectFactory)
        {
            var sector = GetRandomEmptySector();
            _sectors[sector] = objectFactory.Invoke();
            return sector;
        }

        private void PositionObject(Func<object> objectFactory, int count)
        {
            for (int i = 0; i < count; i++)
            {
                PositionObject(objectFactory);
            }
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

        public IEnumerable<string> GetDisplayLines() => Enumerable.Range(1, 8).Select(x => GetDisplayLine(x));

        private string GetDisplayLine(int x)
            => string.Join(
                " ",
                Enumerable
                    .Range(1, 8)
                    .Select(y => new Coordinates(x, y))
                    .Select(c => _sectors.GetValueOrDefault(c))
                    .Select(o => o?.ToString() ?? "   "));
    }
}
