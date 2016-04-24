using System.Collections.Generic;

namespace INavigation
{
    public interface IRoute
    {
        IEnumerable<Point> GetPoints();

        double GetLength();

        // This method will require reconsideration - is it really needed?
        double GetCycleLenght();

        IRoute Append(IRoute toAppend);
    }
}
