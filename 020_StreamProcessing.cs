using NUnit.Framework;

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
    }
}