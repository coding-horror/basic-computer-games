using System;
using System.Collections.Immutable;

namespace Game.Extensions
{
    /// <summary>
    /// Provides additional methods for the <see cref="ImmutableArray{T}"/> class.
    /// </summary>
    public static class ImmutableArrayExtensions
    {
        /// <summary>
        /// Maps each element in an immutable array to a new value.
        /// </summary>
        /// <typeparam name="TSource">
        /// The type of elements in the source array.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type of elements in the resulting array.
        /// </typeparam>
        /// <param name="source">
        /// The source array.
        /// </param>
        /// <param name="selector">
        /// Function which receives an element from the source array and its
        /// index and returns the resulting element.
        /// </param>
        public static ImmutableArray<TResult> Map<TSource, TResult>(this ImmutableArray<TSource> source, Func<TSource, int, TResult> selector)
        {
            var builder = ImmutableArray.CreateBuilder<TResult>(source.Length);

            for (var i = 0; i < source.Length; ++i)
                builder.Add(selector(source[i], i));

            return builder.MoveToImmutable();
        }
    }
}
