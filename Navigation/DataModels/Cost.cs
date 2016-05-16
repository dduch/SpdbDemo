using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation.Graph;

namespace Navigation.DataModels
{
    class Cost : IEdgeCost
    {
        public double Major;
        public double Minor;

        public Cost(double major, double minor)
        {
            Major = major;
            Minor = minor;
        }

        private static readonly double projectionFactor = 1000000.0;

        public double Value()
        {
            return Major * projectionFactor + Minor;
        }

    }
}
