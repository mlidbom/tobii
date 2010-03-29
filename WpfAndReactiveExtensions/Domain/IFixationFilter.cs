using System.Windows;

namespace WpfAndReactiveExtensions.Domain
{
    public interface IFixationFilter
    {
        Point? CurrentFixationPosition(Point currentPosition);
    }
}