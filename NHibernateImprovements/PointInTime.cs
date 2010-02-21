using System;

namespace NHibernateImprovements
{
    public struct PointInTime
    {
        private readonly long _microSec;
        private PointInTime(long microSec) : this() { _microSec = microSec; }
        public long TotalMicroseconds { get { return _microSec; } }
        public static PointInTime FromMicroseconds(long microseconds) { return new PointInTime(microseconds); }

        public override string ToString()
        {
            return TimeSpan.FromMilliseconds(_microSec*10).ToString();
        }
    }
}