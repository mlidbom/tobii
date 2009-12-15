using Tobii.Points;

namespace Tobii.Time
{
    public static class PixelPositionExtensions
    {
        public static IPixelPosition Move(this IPixelPosition me, IVector2D<uint> movement)
        {
            return new PixelPosition(me.X + movement.X, me.Y + movement.Y);
        }
    }
}