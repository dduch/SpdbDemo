
namespace INavigation
{
    public interface INavigationResolver
    {
        IRoute GetBestRoute(Point source, Point destination, double avgSpeed = 15.0);
    }
}
