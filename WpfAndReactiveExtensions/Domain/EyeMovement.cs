using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfAndReactiveExtensions.Linq;

namespace WpfAndReactiveExtensions.Domain
{
    public static class EyeMovement
    {
        public static IObservable<Vector> Movements(this IObservable<Point> positions)
        {
            return positions
                .ConsecutivePairs((startPoint, destinationPoint) => destinationPoint - startPoint)
                .Where(movement => movement.Length > 0);
        }

        public static IObservable<double> TotalMovementDistance(this IObservable<Vector> movements)
        {
            return movements.Scan(0.0, (travelled, lastMovement) => travelled + lastMovement.Length);
        }

        public static Point AveragePoint(this IEnumerable<Point> points)
        {
            return new Point((int)points.Average(point => point.X),
                             (int)points.Average(point => point.Y));
        }
    }
}