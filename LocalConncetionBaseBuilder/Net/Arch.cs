using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation.DataModels;

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
        public int To { get; set; }
        public ArchType Type { get; set; }
        public Route Content { get; set; }
        public Arch(int to, ArchType type, Route content)
        {
            To = to;
            Type = type;
            Content = content;
        }
    }
}
