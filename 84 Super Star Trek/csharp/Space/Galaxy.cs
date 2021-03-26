using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;

using static System.StringSplitOptions;

namespace SuperStarTrek.Space
{
    internal class Galaxy
    {
        private static readonly string[] _regionNames;
        private static readonly string[] _subRegionIdentifiers;
        private readonly QuadrantInfo[][] _quadrants;
        private readonly Random _random;

        static Galaxy()
        {
            _regionNames = Strings.RegionNames.Split(new[] { ' ', '\n' }, RemoveEmptyEntries | TrimEntries);
            _subRegionIdentifiers = new[] { "I", "II", "III", "IV" };
        }

        public Galaxy(Random random)
        {
            _random = random;

            _quadrants = Enumerable
                .Range(0, 8)
                .Select(x => Enumerable
                    .Range(0, 8)
                    .Select(y => new Coordinates(x, y))
                    .Select(c => QuadrantInfo.Create(c, GetQuadrantName(c)))
                    .ToArray())
                .ToArray();

            if (StarbaseCount == 0)
            {
                var randomQuadrant = this[random.GetCoordinate()];
                randomQuadrant.AddStarbase();

                if (randomQuadrant.KlingonCount < 2)
                {
                    randomQuadrant.AddKlingon();
                }
            }
        }

        public QuadrantInfo this[Coordinates coordinate] => _quadrants[coordinate.X][coordinate.Y];

        public int KlingonCount => _quadrants.SelectMany(q => q).Sum(q => q.KlingonCount);
        public int StarbaseCount => _quadrants.SelectMany(q => q).Count(q => q.HasStarbase);
        public IEnumerable<IEnumerable<QuadrantInfo>> Quadrants => _quadrants;

        private static string GetQuadrantName(Coordinates coordinates) =>
            $"{_regionNames[coordinates.RegionIndex]} {_subRegionIdentifiers[coordinates.SubRegionIndex]}";

        public IEnumerable<IEnumerable<QuadrantInfo>> GetNeighborhood(Quadrant quadrant) =>
            Enumerable.Range(-1, 3)
                .Select(dx => dx + quadrant.Coordinates.X)
                .Select(x => GetNeighborhoodRow(quadrant, x));
        private IEnumerable<QuadrantInfo> GetNeighborhoodRow(Quadrant quadrant, int x) =>
            Enumerable.Range(-1, 3)
                .Select(dy => dy + quadrant.Coordinates.Y)
                .Select(y => y < 0 || y > 7 || x < 0 || x > 7 ? null : _quadrants[x][y]);
    }
}
