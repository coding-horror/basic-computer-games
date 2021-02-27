using SuperStarTrek.Space;

namespace SuperStarTrek
{
    internal class Random
    {
        private static readonly System.Random _random = new();

        public Coordinates GetCoordinate() => new Coordinates(Get1To8Inclusive(), Get1To8Inclusive());

        // Duplicates the algorithm used in the original code to get an integer value from 1 to 8, inclusive:
        //     475 DEF FNR(R)=INT(RND(R)*7.98+1.01)
        // Returns a value from 1 to 8, inclusive.
        // Note there's a slight bias away from the extreme values, 1 and 8.
        public int Get1To8Inclusive() => (int)(_random.NextDouble() * 7.98 + 1.01);

        public int GetInt(int inclusiveMinValue, int exclusiveMaxValue) =>
            _random.Next(inclusiveMinValue, exclusiveMaxValue);

        public double GetDouble() => _random.NextDouble();

        public double GetDouble(double inclusiveMinValue, double exclusiveMaxValue)
            => _random.NextDouble() * (exclusiveMaxValue - inclusiveMinValue) + inclusiveMinValue;
    }
}
