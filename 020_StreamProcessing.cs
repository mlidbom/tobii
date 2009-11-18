using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Void;
using Void.Linq;

namespace Tobii
{
    [TestFixture]
    public class StreamProcessing
    {
        [Test]
        public void UsingLinqStreamsToProcessInComingData()
        {
            ///Use blocking iterator + eventpushing thread + spliting of streams to 
            /// implement multiple threads watching different filters and aggregations
            /// of the generated point data.
            /// 
            /// Emit fixed points using the fixedpoint algorithm
            /// 
            /// Have separate stream for out of range data
            /// 
            /// separate stream for aggregating hotspot areas
            /// 
            /// Make the data timestamped so that it can be lazily evaluated later.
        }

        [Test]
        public void UsingStreamsToImportExportData()
        {
        }



        [Test]
        public void TestStabilityWithStatefulFunction()
        {
            GetCyclicPathPoints(500000)
                .Where(IsStabilized())
                .Count()
                .Do(Console.WriteLine);
        }

        [Test]
        public void TestStabilityViaChunking()
        {
            GetCyclicPathPoints(500000)
                .ChopIntoSizesOf(10)
                .Where(ChunkIsStable)
                .SelectMany(me => me)
                .Count()
                .Do(Console.WriteLine);
        }

        protected bool ChunkIsStable(IEnumerable<Zipping.Pair<int, int>> points)
        {
            const int maxVariance = 50;
            var xVariance = points.Max(p => p.First) - points.Min(p => p.First);
            var yVariance = points.Max(p => p.Second) - points.Min(p => p.Second);
            return xVariance < maxVariance && yVariance < maxVariance;
        }


        static Func<Zipping.Pair<int, int>, bool> IsStabilized()
        {
            const int maxVariance = 50;
            var last10 = new LinkedList<Zipping.Pair<int, int>>(new Zipping.Pair<int, int>(0, 0).Repeat(10));
            return currentPosition =>
            {
                last10.RemoveLast();
                last10.AddFirst(currentPosition);
                var xVariance = last10.Max(p => p.First) - last10.Min(p => p.First);
                var yVariance = last10.Max(p => p.Second) - last10.Min(p => p.Second);
                return xVariance < maxVariance && yVariance < maxVariance;
            };
        }

        #region Very very ugly

        private IEnumerable<Zipping.Pair<int, int>> GetCyclicPathPoints(int numberOfPoints)
        {
            return 1.Through(numberOfPoints)
                .Select(me => Math.Abs((int)(1280 * Math.Sin(me / 100D))))
                .Select(me => new Zipping.Pair<int, int>(me, me));
        }

        #endregion
    }
}