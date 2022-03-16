using SuperStarTrek.Space;

namespace SuperStarTrek
{
    internal class Random
    {
        private readonly System.Random _random = new();

        internal Coordinates GetCoordinate() => new Coordinates(Get1To8Inclusive() - 1, Get1To8Inclusive() - 1);

        // Duplicates the algorithm used in the original code to get an integer value from 1 to 8, inclusive:
        //     475 DEF FNR(R)=INT(RND(R)*7.98+1.01)
        // Returns a value from 1 to 8, inclusive.
        // Note there's a slight bias away from the extreme values, 1 and 8.
        internal int Get1To8Inclusive() => (int)(GetFloat() * 7.98 + 1.01);

        internal int GetInt(int inclusiveMinValue, int exclusiveMaxValue) =>
            _random.Next(inclusiveMinValue, exclusiveMaxValue);

        internal float GetFloat() => (float)_random.NextDouble();

        internal float GetFloat(float inclusiveMinValue, float exclusiveMaxValue)
            => GetFloat() * (exclusiveMaxValue - inclusiveMinValue) + inclusiveMinValue;
    }
}
