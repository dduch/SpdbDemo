using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;

namespace NavigationResolver.DataModels
{
    public class Route : IRoute
    {
        public IRoute Append(IRoute toAppend)
        {
            throw new NotImplementedException();
        }

        public double GetCycleLenght()
        {
            throw new NotImplementedException();
        }

        public double GetLength()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Point> GetPoints()
        {
            throw new NotImplementedException();
        }
    }
}
