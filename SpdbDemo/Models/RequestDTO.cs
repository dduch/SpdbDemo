using NavigationResolver.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpdbDemo.Models
{
    public class RequestDTO
    {
        public Point StartPosition { get; set; }

        public Point DestinationPosition { get; set; }
    }
}