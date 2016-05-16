using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;

namespace LocalConncetionBaseBuilder.Net
{
    class Node
    {
        public Point Position { get; }
        public List<Arch> Archs { get; }

        public Node(Point position)
        {
            Position = position;
            Archs = new List<Arch>();
        }

        public void AddArch(int toNode, ArchType type)
        {
            int existingId = Archs.FindIndex(arch => arch.To == toNode);
            if(existingId >= 0)
            {
                if (Archs[existingId].Type != ArchType.BICYCLE)
                    Archs[existingId].Type = type;
            }
            else
            {
                Archs.Add(new Arch(toNode, type));
            }
        }
    }
}
