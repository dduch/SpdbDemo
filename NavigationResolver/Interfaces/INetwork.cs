using NavigationResolver.Types;

namespace NavigationResolver.Interfaces
{
    public interface INetwork
    {
        IRoute GetBestRoute(Point source, Point destination);
    }
}
