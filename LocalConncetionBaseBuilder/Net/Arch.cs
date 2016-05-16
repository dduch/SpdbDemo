using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalConncetionBaseBuilder.Net
{
    enum ArchType
    {
        FOOT,
        BICYCLE,
        MOTOR,
        OTHER
    }

    class Arch
    {
        public int To { get; }
        public ArchType Type { get; set; }
        public Arch(int to, ArchType type)
        {
            To = to;
            Type = type;
        }
    }
}
