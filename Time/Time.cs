namespace Tobii.Time
{
    public class Time : ITime
    {
        public Time(long milliSeconds)
        {
            MilliSeconds = milliSeconds;
        }

        public long MilliSeconds { get; private set; }
    }
}