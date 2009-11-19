using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Void;
using Void.Linq;

namespace Tobii
{
    /// <summary>
    /// As a general rule Linq logic performs spectacularly well.
    /// This is why:
    /// </summary>
    [TestFixture]
    public class LazynessMakeTheImpossiblePossible
    {
        [Test]
        public void UseIntMaxvalueSquaredIntegersToFindTheFirst100NumbersDivisibleBy5()
        {
            Console.WriteLine("Facebook has just over 1.5 petabytes of users' photos stored,\ntranslating into roughly 10 billion photos\n");

            Func<double, long> toPetaBytes = i => (long)(i / Math.Pow(1000, 5));
            Math.Pow(int.MaxValue, 2)
                .Transform(numberOfIntegers => numberOfIntegers * 4)
                .Transform(toPetaBytes)
                .Do(me => Console.WriteLine("Generating {0} petabytes of lazy data...\n", me));


            #region start timing

            var watch = new Stopwatch();
            watch.Start();

            #endregion

            var intMaxValueSquaredInLength = 1.Through(int.MaxValue)
                .SelectMany(num => 1.Through(int.MaxValue));

            #region print: Creating the data took...

            Console.WriteLine("Creating the data took {0} milliseconds\n", watch.ElapsedMilliseconds);
            watch.Reset();
            watch.Start();

            #endregion

            Console.WriteLine("Searching...\n");

            intMaxValueSquaredInLength
                .Where(i => i % 5 == 0)
                .Take(10)
                .ForEach(Console.WriteLine);

            #region print: Searching the data took...

            Console.WriteLine("\nSearching the data took {0} milliseconds\n", watch.ElapsedMilliseconds);

            #endregion

            //JUST DON'T CALLY ANY OPERATORS THAT FORCE ITERATION: 
            //var theReallyLongWait = intMaxValueSquaredInLength.Count(); //Don't do this. It will take "some" time
        }
    }
}