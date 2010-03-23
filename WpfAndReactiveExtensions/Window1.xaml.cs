using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using Void.Linq;

namespace WpfAndReactiveExtensions
{
    public partial class Window1 : Window
    {
        private Point _lastEyePosition;

        public Window1()
        {
            #region scaffolding

            InitializeComponent();
            MouseMove += (_, args) => _lastEyePosition = args.GetPosition(this);

            Observable.Context = SynchronizationContexts.CurrentDispatcher;

            var eyePositions = Observable.Interval(1).Select(_ => _lastEyePosition);

            #endregion

            eyePositions.Subscribe(point => currentLocation.Content = point);

            var fixations = eyePositions.Fixations();
            var movements = eyePositions.MovementsBetweenPositions();
            var distance = movements.TotalMovementDistance();

            fixations.Subscribe(point => lastFixation.Content = point);
            movements.Subscribe(move => lastMovement.Content = move);
            distance.Subscribe(travelled => distanceTravelled.Content = travelled);
        }

     
    }

    public static class EyeMovement
    {
        public static IObservable<Vector> MovementsBetweenPositions(this IObservable<Point> positions)
        {
            return positions
                .ConsecutivePairs((startPoint, destinationPoint) => destinationPoint - startPoint)
                .Where(movement => movement.Length > 0);
        }

        public static IObservable<double> TotalMovementDistance(this IObservable<Vector> movements)
        {
            return movements.Scan(0.0, (travelled, lastMovement) => travelled + lastMovement.Length);
        }

        public static IObservable<Point> Fixations(this IObservable<Point> eyeMovements)
        {
            return eyeMovements.Where(IsFixated());
        }

        private static Func<Point, bool> IsFixated()
        {
            const int maxVariance = 10;
            var last10 = new LinkedList<Point>(new Point(0, 0).Repeat(10));
            return currentPosition =>
            {
                last10.RemoveLast();
                last10.AddFirst(currentPosition);
                var xVariance = last10.Max(p => p.X) - last10.Min(p => p.X);
                var yVariance = last10.Max(p => p.Y) - last10.Min(p => p.Y);
                return xVariance < maxVariance && yVariance < maxVariance;
            };
        }
    }

    public static class Sequences
    {
        public static IObservable<TSelected> ConsecutivePairs<TSource, TSelected>(this IObservable<TSource> positions, Func<TSource, TSource, TSelected> selector)
        {
            return positions.Zip(positions.Skip(1), selector);
        }
    }
}