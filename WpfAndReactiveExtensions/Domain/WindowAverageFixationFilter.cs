using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfAndReactiveExtensions.Collections;

namespace WpfAndReactiveExtensions.Domain
{
    public class WindowAverageFixationFilter : IFixationFilter
    {
        private readonly double _tolerance;
        private readonly LimitedLengthList<Point> _history;
        private Point? _fixatedOn;
        private readonly int _windowSize;

        public WindowAverageFixationFilter(int windowSize, double tolerance)
        {
            _history = new LimitedLengthList<Point>(windowSize*2);
            _windowSize = windowSize;
            _tolerance = tolerance;
        }

        public Point? CurrentFixationPosition(Point currentPosition)
        {
            _history.Push(currentPosition);

            var currentViewPoint = _history.AveragePointInRange(_windowSize, _windowSize * 2);
            if (_fixatedOn.HasValue && WithinTolerance(_fixatedOn.Value, currentViewPoint))
            {
                return _fixatedOn;
            }

            var previousWindowViewPoint = _history.AveragePointInRange(0, _windowSize);
            return _fixatedOn = WithinTolerance(previousWindowViewPoint, currentViewPoint)
                                    ? previousWindowViewPoint
                                    : (Point?) null;
        }

        private bool WithinTolerance(Point candidate, Point currentViewPoint)
        {
            return (candidate - currentViewPoint).Length < _tolerance;
        }
    }
}