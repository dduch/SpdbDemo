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
        private static readonly double bikePreferenceFactor = 0.8;

        public EdgeCost(double value)
        {
            this.value = value;
        }

        public EdgeCost(Arch arch)
        {
            if (arch.Type == ArchType.BICYCLE)
                this.value = arch.Content.GetLength() * bikePreferenceFactor;
            else
                this.value = arch.Content.GetLength();
        }

        public double Value()
        {
            return value;
        }
    }
}
