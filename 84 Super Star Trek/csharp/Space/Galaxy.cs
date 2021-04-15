using System.Collections.Generic;
using System.Linq;
using SuperStarTrek.Resources;

using static System.StringSplitOptions;

namespace SuperStarTrek.Space
{
    internal class Galaxy
    {
        private static readonly string[] _regionNames;
        private static readonly string[] _subRegionIdentifiers;
        private readonly QuadrantInfo[][] _quadrants;

        static Galaxy()
        {
            _regionNames = Strings.RegionNames.Split(new[] { ' ', '\n' }, RemoveEmptyEntries | TrimEntries);
            _subRegionIdentifiers = new[] { "I", "II", "III", "IV" };
        }

        internal Galaxy(Random random)
        {
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

        internal QuadrantInfo this[Coordinates coordinate] => _quadrants[coordinate.X][coordinate.Y];

        internal int KlingonCount => _quadrants.SelectMany(q => q).Sum(q => q.KlingonCount);
        internal int StarbaseCount => _quadrants.SelectMany(q => q).Count(q => q.HasStarbase);
        internal IEnumerable<IEnumerable<QuadrantInfo>> Quadrants => _quadrants;

        private static string GetQuadrantName(Coordinates coordinates) =>
            $"{_regionNames[coordinates.RegionIndex]} {_subRegionIdentifiers[coordinates.SubRegionIndex]}";

        internal IEnumerable<IEnumerable<QuadrantInfo>> GetNeighborhood(Quadrant quadrant) =>
            Enumerable.Range(-1, 3)
                .Select(dx => dx + quadrant.Coordinates.X)
                .Select(x => GetNeighborhoodRow(quadrant, x));
        private IEnumerable<QuadrantInfo> GetNeighborhoodRow(Quadrant quadrant, int x) =>
            Enumerable.Range(-1, 3)
                .Select(dy => dy + quadrant.Coordinates.Y)
                .Select(y => y < 0 || y > 7 || x < 0 || x > 7 ? null : _quadrants[x][y]);
    }
}
