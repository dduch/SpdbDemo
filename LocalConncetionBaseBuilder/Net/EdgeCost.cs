using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation.Graph;

namespace LocalConncetionBaseBuilder.Net
{
    class EdgeCost : IEdgeCost
    {
        private double value;

        public EdgeCost(double value)
        {
            this.value = value;
        }

        public double Value()
        {
            return value;
        }
    }
}
