using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;

namespace NavigationResolver.DataModels
{
    public class Network : INetwork
    {
        public IRoute GetBestRoute(Point source, Point destination)
        {
            throw new NotImplementedException();
        }
    }
}
