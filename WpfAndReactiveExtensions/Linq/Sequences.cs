using System;
using System.Collections.Generic;
using System.Linq;
using Zipping = Void.Linq.Zipping;

namespace WpfAndReactiveExtensions
{
    public static class Sequences
    {
        public static IEnumerable<T> RemoveConsecutiveDuplicates<T>(this IEnumerable<T> me)
        {
            return me.ConsecutivePairs((current, old) => new {current, old})
                .Where(pair => !pair.old.Equals(pair.current))
                .Select(pair => pair.current);
        }

        public static IEnumerable<TSelected> ConsecutivePairs<TSource, TSelected>(this IEnumerable<TSource> positions,
                                                                                  Func<TSource, TSource, TSelected> selector)
        {
            return Zipping.Zip(positions, positions.Skip(1), selector);
        }
    }
}