using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Extensions
{
    /// <summary>
    /// Provides additional methods for the <see cref="IEnumerable{T}"/>
    /// interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Simultaneously projects each element of a sequence and applies
        /// the result of the previous projection.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of elements in the source sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of elements in the result sequence.
        /// </typeparam>
        /// <param name="source">
        /// The source sequence.
        /// </param>
        /// <param name="seed">
        /// The seed value for the aggregation component.  This value is
        /// passed to the first call to <paramref name="selector"/>.
        /// </param>
        /// <param name="selector">
        /// The projection function.  This function is supplied with a value
        /// from the source sequence and the result of the projection on the
        /// previous value in the source sequence.
        /// </param>
        /// <returns>
        /// The resulting sequence.
        /// </returns>
        public static IEnumerable<TResult> SelectAndAggregate<TSource, TResult>(
            this IEnumerable<TSource> source,
            TResult seed,
            Func<TSource, TResult, TResult> selector)
        {
            foreach (var element in source)
            {
                seed = selector(element, seed);
                yield return seed;
            }
        }

        /// <summary>
        /// Combines the results of three distinct sequences into a single
        /// sequence.
        /// </summary>
        /// <typeparam name="T1">
        /// The element type of the first sequence.
        /// </typeparam>
        /// <typeparam name="T2">
        /// The element type of the second sequence.
        /// </typeparam>
        /// <typeparam name="T3">
        /// The element type of the third sequence.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The element type of the resulting sequence.
        /// </typeparam>
        /// <param name="first">
        /// The first source sequence.
        /// </param>
        /// <param name="second">
        /// The second source sequence.
        /// </param>
        /// <param name="third">
        /// The third source sequence.
        /// </param>
        /// <param name="resultSelector">
        /// Function that combines results from each source sequence into a
        /// final result.
        /// </param>
        /// <returns>
        /// A sequence of combined values.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This function works identically to Enumerable.Zip except that it
        /// combines three sequences instead of two.
        /// </para>
        /// <para>
        /// We have defined this as an extension method for consistency with
        /// the similar LINQ methods in the <see cref="Enumerable"/> class.
        /// However, since there is nothing special about the first sequence,
        /// it is often more clear to call this as a regular function.  For
        /// example:
        /// </para>
        /// <code>
        /// EnumerableExtensions.Zip(
        ///     sequence1,
        ///     sequence2,
        ///     sequence3,
        ///     (a, b, c) => GetResult (a, b, c));
        /// </code>
        /// </remarks>
        public static IEnumerable<TResult> Zip<T1, T2, T3, TResult>(
            this IEnumerable<T1> first,
            IEnumerable<T2> second,
            IEnumerable<T3> third,
            Func<T1, T2, T3, TResult> resultSelector)
        {
            using var enumerator1 = first.GetEnumerator();
            using var enumerator2 = second.GetEnumerator();
            using var enumerator3 = third.GetEnumerator();

            while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext())
                yield return resultSelector(enumerator1.Current, enumerator2.Current, enumerator3.Current);
        }
    }
}
