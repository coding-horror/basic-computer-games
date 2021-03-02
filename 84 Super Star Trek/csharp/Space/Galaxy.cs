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

        public Galaxy()
        {
            var random = new Random();

            _quadrants = Enumerable
                .Range(1, 8)
                .Select(x => Enumerable
                    .Range(1, 8)
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

        public QuadrantInfo this[Coordinates coordinate] => _quadrants[coordinate.X - 1][coordinate.Y - 1];

        public int KlingonCount => _quadrants.SelectMany(q => q).Sum(q => q.KlingonCount);
        public int StarbaseCount => _quadrants.SelectMany(q => q).Count(q => q.HasStarbase);

        private static string GetQuadrantName(Coordinates coordinates) =>
            $"{_regionNames[coordinates.RegionIndex]} {_subRegionIdentifiers[coordinates.SubRegionIndex]}";
    }
}
