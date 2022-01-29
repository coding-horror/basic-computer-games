using System;
using System.Collections.Generic;

namespace Game.Extensions
{
    /// <summary>
    /// Provides additional methods for the <see cref="Random"/> class.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Generates an infinite sequence of random numbers.
        /// </summary>
        /// <param name="random">
        /// The random number generator.
        /// </param>
        /// <param name="min">
        /// The inclusive lower bound of the range to generate.
        /// </param>
        /// <param name="max">
        /// The exclusive upper bound of the range to generate.
        /// </param>
        /// <returns>
        /// An infinite sequence of random integers in the range [min, max).
        /// </returns>
        /// <remarks>
        /// <para>
        /// We use an exclusive upper bound, even though it's a little
        /// confusing, for the sake of consistency with Random.Next.
        /// </para>
        /// <para>
        /// Since the sequence is infinite, a typical usage would be to cap
        /// the results with a function like Enumerable.Take.  For example,
        /// to sum the results of rolling three six sided dice, we could do:
        /// </para>
        /// <code>
        /// random.Integers(1, 7).Take(3).Sum()
        /// </code>
        /// </remarks>
        public static IEnumerable<int> Integers(this Random random, int min, int max)
        {
            while (true)
                yield return random.Next(min, max);
        }
    }
}
