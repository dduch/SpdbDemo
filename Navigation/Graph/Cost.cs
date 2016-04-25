using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Graph
{
    struct Cost
    {
        public double Major;
        public double Minor;

        public Cost(double major, double minor)
        {
            Major = major;
            Minor = minor;
        }

        private static readonly double projectionFactor = 1000000.0;

        public double ToDouble()
        {
            return Major * projectionFactor + Minor;
        }

    }
}
