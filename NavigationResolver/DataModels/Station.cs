﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;

namespace Navigation.DataModels
{
    public class Station
    {
        public string Name { get; }
        public int Id { get; }
        public Point Position { get; }

        public Station(string name, int id, double lat, double lon)
        {
            Name = name;
            Id = id;
            Position = new Point(lat, lon);
        }
    }
}
