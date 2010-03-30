using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfAndReactiveExtensions.Linq;

namespace WpfAndReactiveExtensions.Domain
{
    public static class FixationReader
    {
        public static IEnumerable<Point> Fixations(this IEnumerable<Point> gazePoints, IFixationFilter filter)
        {
            return gazePoints.
                Select(filter.CurrentFixationPosition)
                .Where(fixation => fixation.HasValue)
                .Select(fixation => fixation.Value)
                .RemoveConsecutiveDuplicates();
        }

        public static IObservable<Point> Fixations(this IObservable<Point> gazePoints, IFixationFilter filter)
        {
            return gazePoints.
                Select(filter.CurrentFixationPosition)
                .Where(fixation => fixation.HasValue)
                .Select(fixation => fixation.Value)
                .RemoveConsecutiveDuplicates();
        }
    }
}