using System.Collections.Generic;

namespace INavigation
{
    public interface IRoute
    {
        IEnumerable<Point> GetPoints();

        double GetLength();

        IRoute Append(IRoute toAppend);
    }
}
