using System;
using System.Linq;

namespace WpfAndReactiveExtensions.Linq
{
    public static class Observables
    {
        public static IObservable<T> RemoveConsecutiveDuplicates<T>(this IObservable<T> me)
        {
            return me.ConsecutivePairs((current, old) => new { current, old })
                .Where(pair => !pair.old.Equals(pair.current))
                .Select(pair => pair.current);
        }

        public static IObservable<TSelected> ConsecutivePairs<TSource, TSelected>(this IObservable<TSource> positions, Func<TSource, TSource, TSelected> selector)
        {
            return positions.Zip(positions.Skip(1), selector);
        }
    }
}