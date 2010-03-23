using System;
using System.Linq;
using System.Threading;
using System.Windows;
using WpfAndReactiveExtensions.Domain;

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

            var eyePositions = Observable.Interval(TimeSpan.FromMilliseconds(1)).Select(_ => _lastEyePosition);

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
}