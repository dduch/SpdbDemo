using System.Collections.Generic;
using NavigationResolver.Types;

namespace NavigationResolver.Interfaces
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
