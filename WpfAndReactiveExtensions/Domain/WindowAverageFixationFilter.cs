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
        private readonly IEnumerable<Point> _currentWindow;
        private readonly IEnumerable<Point> _previousWindow;
        private Point? _fixatedOn;

        public WindowAverageFixationFilter(int windowSize, double tolerance)
        {
            _history = new LimitedLengthList<Point>(windowSize*2);            
            _previousWindow = _history.Take(windowSize);
            _currentWindow = _history.Skip(windowSize);
            _tolerance = tolerance;
        }

        public Point? CurrentFixationPosition(Point currentPosition)
        {
            _history.Push(currentPosition);

            var currentViewPoint = _currentWindow.AveragePoint();
            if (_fixatedOn.HasValue && WithinTolerance(_fixatedOn.Value, currentViewPoint))
            {
                return _fixatedOn;
            }

            var previousWindowViewPoint = _previousWindow.AveragePoint();
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