using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Graph
{
    public interface IEdgeCost
    {
        double Value();
    }
}
