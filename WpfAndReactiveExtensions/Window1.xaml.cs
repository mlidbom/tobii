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
        private readonly SynchronizationContext context;
        private Point _position = new Point(0, 0);

        static double AddAbsolutes(double x, double y)
        {
            return Math.Abs(x) + Math.Abs(y);
        }

        static Point SubtractVectors(Point subtractFrom, Point subtract)
        {
            return new Point
                   {
                       X = subtractFrom.X - subtract.X,
                       Y = subtractFrom.Y - subtract.Y
                   };
        }

        //Domainspecific name improves readability, and declaring it as a Func 
        //makes typeinference together with algorithms work!
        static readonly Func<Point, Point, Point> MovementBetweenPoints = SubtractVectors;

        static Point AddMovementDistance(Point movement1, Point movement2)
        {
            return new Point
                   {
                       X = AddAbsolutes(movement1.X, movement2.X),
                       Y = AddAbsolutes(movement1.Y, movement2.Y)
                   };
        }

        static bool MovementIsNotZeroLength(Point move)
        {
            return move.X != 0 || move.Y != 0;
        }

        public Window1()
        {
            #region scaffolding

            InitializeComponent();
            MouseMove += (_, args) => _position = args.GetPosition(this);

            context = Observable.Context = SynchronizationContexts.CurrentDispatcher;

            var eyePositions = Observable.Interval(1).Select(_ => new Point(_position.X, _position.Y));

            #endregion

            eyePositions.Subscribe(point => currentObservable.Content = point.ToString());

            var fixations = eyePositions.Where(IsFixated());
           
            var movements = eyePositions
                .Let(ep => ep.Zip(ep.Skip(1), MovementBetweenPoints))
                .Where(MovementIsNotZeroLength);

            var distance = movements.Scan(new Point(0, 0), AddMovementDistance);

            fixations.Subscribe(point => lastFixatedObservable.Content = point.ToString());
            movements.Subscribe(move => movementObservable.Content = move);
            distance.Subscribe(travelled => distanceObservable.Content = travelled);

            
            StartEnumerableThreads(eyePositions);
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

        #region reimplement using Enumerable to illustrate increased difficulty and worse structure

        private void StartEnumerableThreads(IObservable<Point> postions)
        {
            var eyePositions = postions.ToEnumerable();

            var fixations = eyePositions.Where(IsFixated());

            //I had to create my own let and zip here......
            var movements = eyePositions

                .Let(ep => ep.Zip(ep.Skip(0), MovementBetweenPoints))
                .Where(MovementIsNotZeroLength);

            ThreadPool.QueueUserWorkItem(__ => eyePositions.ForEach(point => context.Post(_ => currentEnumerable.Content = point, null)));
            ThreadPool.QueueUserWorkItem(__ => fixations.ForEach(point => context.Post(_ => lastFixatedEnumerable.Content = point, null)));
            ThreadPool.QueueUserWorkItem(__ => movements.ForEach(point => context.Post(_ => movementEnumerable.Content = point, null)));

            //Yikes, that's a handful!
            ThreadPool.QueueUserWorkItem(
                __ => movements.Aggregate(
                          new Point(0, 0), (accumulator, movement) =>
                                           {
                                               var dist = AddMovementDistance(accumulator, movement);
                                               context.Post(_ => distanceEnumerable.Content = dist, null);
                                               return dist;
                                           }));
        }

        #endregion
    }
}