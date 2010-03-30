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

            Observable.Context = SynchronizationContexts.CurrentDispatcher;//Automatic marshaling of all events to correct context
            var eyePositions = Observable.Interval(TimeSpan.FromMilliseconds(1)).Select(_ => _lastEyePosition);

            #endregion

            eyePositions.Subscribe(point => currentLocation.Content = point);

            var fixtationFilter = new WindowAverageFixationFilter(windowSize: 8, tolerance: 30);            
            var movements = eyePositions.Movements();
            var distance = movements.TotalMovementDistance();

            var fixationPositions = eyePositions.Select(fixtationFilter.CurrentFixationPosition);

            fixationPositions.Subscribe(point => currentFixationPosition.Content = point);
            movements.Subscribe(move => lastMovement.Content = move);
            distance.Subscribe(travelled => distanceTravelled.Content = travelled);
        }

     
    }
}