using Tobii.Points;
using Tobii.Time;

namespace Tobii.SpaceTime
{
    public class PixelPositionEvent : IPixelPositionEvent
    {
        public IPixelPosition Position { get; private set; }
        public ITime Time { get; private set; }

        public PixelPositionEvent(IPixelPosition position, ITime time)
        {
            Position = position;
            Time = time;
        }
    }

    public static class PixelPositionEventExtensions
    {
           
    }
}