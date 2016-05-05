
namespace INavigation
{
    public interface INavigationResolver
    {
        NavigationResult GetBestRoute(Point source, Point destination, double avgSpeed = 15.0);
    }
}
