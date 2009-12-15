namespace Tobii.Points
{
    /// <summary>
    /// Represent a pixel point on a screen artifact represented as the offset down from the top left corner
    /// of the screen artifact. 
    /// 
    /// when X = 10 and Y = 10 you are describing the point 10 pixels down and 10 pixels 
    /// to the right of the top left corner.
    /// </summary>
    public interface IPixelPosition : IVector2D<uint>
    {
    }
}