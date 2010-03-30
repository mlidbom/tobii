using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using NUnit.Framework;
using WpfAndReactiveExtensions.Domain;

namespace WpfAndReactiveExtensions.Tests
{
    [TestFixture]
    public class PerformanceFixture
    {
        [Test]
        public void ShouldBeBloodyPerformant()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            const int hours = 24; //use 24 hours and show memoryconsumption during work.;
            const int herz = 60;

            var anHoursWorthOfGazeData = GenerateRandomGazePointWalk(hertz: herz, hours: hours);

            var filter = new WindowAverageFixationFilter(windowSize: 10, tolerance: 20);
            var fixations = anHoursWorthOfGazeData.Fixations(filter);

            var fixationCount = fixations.Count();



            Console.WriteLine(@"Took {0} to parse {1} hours worth of gaze data at {2} herz. Found {3} fixations",
                              stopwatch.Elapsed, hours, herz, fixationCount);
        }

        private static IEnumerable<Point> GenerateRandomGazePointWalk(int hertz, int hours)
        {
            var random = new Random(23487);
            var points = hertz*60*60*hours;

            var current = new Point(300, 300);
            for (var i = 0; i < points; i++)
            {
                yield return current += new Vector(GetStep(random), GetStep(random));
            }
        }

        private static double GetStep(Random random)
        {
            return (random.NextDouble() - 0.5)*3;
        }
    }
}