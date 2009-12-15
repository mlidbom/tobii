namespace Tobii.Points
{
    public class PixelPosition : IPixelPosition
    {
        public uint X { get; private set; }
        public uint Y { get; private set; }

        public PixelPosition(uint x, uint y)
        {
            X = x;
            Y = y;
        }
    }
}