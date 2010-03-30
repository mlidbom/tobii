using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfAndReactiveExtensions.Collections;
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

        public static Point AveragePointInRange(this LimitedLengthList<Point> points, int startIndex, int endIndex)
        {
            double xSum = 0.0; 
            double ySum=0.0;
            int items = endIndex - startIndex;

            for (int i = startIndex; i < endIndex; i++)
            {
                var point = points[i];
                xSum += point.X;
                ySum += point.Y;
            }
            return new Point((int)(xSum / items),
                             (int)(ySum / items));
        }
    }
}