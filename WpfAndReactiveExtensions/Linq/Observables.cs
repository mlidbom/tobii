using System;
using System.Linq;

namespace WpfAndReactiveExtensions.Linq
{
    public static class Observables
    {
        public static IObservable<TSelected> ConsecutivePairs<TSource, TSelected>(this IObservable<TSource> positions,
                                                                                  Func<TSource, TSource, TSelected> selector)
        {
            return positions.Zip(positions.Skip(1), selector);
        }
    }
}