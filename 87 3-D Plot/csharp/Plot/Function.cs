using System;
using System.Collections.Generic;

namespace Plot
{
    internal static class Function
    {
        internal static IEnumerable<IEnumerable<int>> GetRows()
        {
            for (var x = -30f; x <= 30f; x += 1.5f)
            {
                yield return GetValues(x);
            }
        }

        private static IEnumerable<int> GetValues(float x)
        {
            var zPrevious = 0;
            var yLimit = 5 * (int)(Math.Sqrt(900 - x * x) / 5);

            for (var y = yLimit; y >= -yLimit; y -= 5)
            {
                var z = GetValue(x, y);

                if (z > zPrevious)
                {
                    zPrevious = z;
                    yield return z;
                }
            }
        }

        private static int GetValue(float x, float y)
        {
            var r = (float)Math.Sqrt(x * x + y * y);
            return (int)(25 + 30 * Math.Exp(-r * r / 100) - 0.7f * y);
        }
    }
}
