using System.Linq;

namespace SuperStarTrek.Space
{
    internal class Galaxy
    {
        private readonly QuadrantInfo[][] _quadrants;

        public Galaxy()
        {
            var random = new Random();

            _quadrants = Enumerable.Range(1, 8).Select(x =>
                Enumerable.Range(1, 8).Select(y => QuadrantInfo.Create(new Coordinates(x, y), "")).ToArray())
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
    }
}
