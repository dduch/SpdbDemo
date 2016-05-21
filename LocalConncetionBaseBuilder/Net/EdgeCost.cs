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

        public EdgeCost(Arch arch)
        {
            switch (arch.Type)
            {
                case ArchType.BICYCLE:
                    value = arch.Content.GetLength() * 1.0;
                    break;
                case ArchType.MOTOR:
                    value = arch.Content.GetLength() * 1.2;
                    break;
                case ArchType.FOOT:
                    value = arch.Content.GetLength() * 1.3;
                    break;
                default:
                    value = arch.Content.GetLength() * 1.4;
                    break;
            }
        }

        public double Value()
        {
            return value;
        }
    }
}
