using Tobii.Points;
using Tobii.Time;

namespace Tobii.SpaceTime
{
    public interface IPixelPositionEvent 
    {
        IPixelPosition Position{ get;}
        ITime Time { get; }
    }
}