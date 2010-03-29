using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Void.Linq;

namespace WpfAndReactiveExtensions.Domain
{
    public class WindowAverageFixationFilter : IFixationFilter
    {
        private readonly double _tolerance;
        private readonly LinkedList<Point> _history;
        private readonly IEnumerable<Point> _currentWindow;
        private readonly IEnumerable<Point> _previousWindow;
        private Point? _fixatedOn;

        public WindowAverageFixationFilter(int windowSize, double tolerance)
        {
            _history = new LinkedList<Point>(new Point(0, 0).Repeat(windowSize*2));
            _currentWindow = _history.Skip(windowSize);
            _previousWindow = _history.Take(windowSize);
            _tolerance = tolerance;
        }

        public Point? CurrentFixationPosition(Point currentPosition)
        {
            _history.RemoveFirst();
            _history.AddLast(currentPosition);

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