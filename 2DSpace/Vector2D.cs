namespace Tobii.Points
{
    public class Vector2D<TCoordinate> : IVector2D<TCoordinate>
    {
        public TCoordinate X { get; private set; }
        public TCoordinate Y { get; private set; }

        public Vector2D(TCoordinate x, TCoordinate y)
        {
            X = x;
            Y = y;
        }
    }
}