using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    /// <summary>
    /// Provides additional methods for the <see cref="IEnumerable{T}"/>
    /// interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Cycles through the integer values in the range [0, count).
        /// </summary>
        /// <param name="start">
        /// The first value to return.
        /// </param>
        /// <param name="count">
        /// The number of values to return.
        /// </param>
        public static IEnumerable<int> Cycle(int start, int count)
        {
            if (count < 1)
                throw new ArgumentException("count must be at least 1");

            if (start < 0 || start >= count)
                throw new ArgumentException("start must be in the range [0, count)");

            for (var i = start; i < count; ++i)
                yield return i;

            for (var i = 0; i < start; ++i)
                yield return i;
        }

        /// <summary>
        /// Finds the index of the first item in the given sequence that
        /// satisfies the given predicate.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the sequence.
        /// </typeparam>
        /// <param name="source">
        /// The source sequence.
        /// </param>
        /// <param name="predicate">
        /// The predicate function.
        /// </param>
        /// <returns>
        /// The index of the first element in the source sequence for which
        /// predicate(element) is true.  If there is no such element, return
        /// is null.
        /// </returns>
        public static int? FindFirstIndex<T>(this IEnumerable<T> source, Func<T, bool> predicate) =>
            source.Select((element, index) => predicate(element) ? index : default(int?))
                .FirstOrDefault(index => index.HasValue);

        /// <summary>
        /// Returns the first item in the given sequence that matches the
        /// given predicate.
        /// </summary>
        /// <typeparam name="T">
        /// The type of elements in the sequence.
        /// </typeparam>
        /// <param name="source">
        /// The source sequence.
        /// </param>
        /// <param name="predicate">
        /// The predicate to check against each element.
        /// </param>
        /// <param name="defaultValue">
        /// The value to return if no elements match the predicate.
        /// </param>
        /// <returns>
        /// The first item in the source sequence that matches the given
        /// predicate, or the provided default value if none do.
        /// </returns>
        public static T FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T defaultValue)
        {
            foreach (var element in source)
                if (predicate(element))
                    return element;

            return defaultValue;
        }
    }
}
