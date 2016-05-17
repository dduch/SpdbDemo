using System;
using System.Collections.Generic;
using System.Linq;
using INavigation;

namespace Navigation.DataModels
{
    // Model of route consisting of sequence of points in space
    // In this model empty routes (with no elements) are allowed
    public class Route : IRoute
    {
        private List<Point> points;
        private double routeLength;

        public Route(List<Point> points)
        {
            this.points = points;

            routeLength = 0;
            var last = points.FirstOrDefault();
            foreach(var next in points)
            {
                routeLength += last.GetDistanceTo(next);
                last = next;
            }
        }

        public Route(List<Point> points, double routeLength)
        {
            this.points = points;
            this.routeLength = routeLength;
        }



        public IRoute Append(IRoute toAppend)
        {
            var pointsToAppend = toAppend.GetPoints() as List<Point>;
            // 1) Appending to empty route
            if (points.Count == 0)
            {
                points = pointsToAppend;
                routeLength = toAppend.GetLength();
            }
            // 2) Appending non empty route to non empty route
            else if (pointsToAppend.Count > 0)
            {
                var fstLast = points.Last();
                var sndFirst = pointsToAppend.First();

                // 2.1) Starting point of appended list is the same as ending of existing
                if(fstLast.Equals(sndFirst))
                    points.RemoveAt(points.Count - 1);
                // 2.2) Otherwise
                else
                    routeLength += fstLast.GetDistanceTo(sndFirst);

                points.AddRange(pointsToAppend);
                routeLength += toAppend.GetLength();
            }
            // 3) Appending empty route to non empty route -> do nothing

            return this; 
        }

        public double GetLength()
        {
            return routeLength;
        }

        public IEnumerable<Point> GetPoints()
        {
            return points;
        }

        public float[] Serialize()
        {
            float[] data = new float[points.Count*2 + 1];
            int i;
            for(i = 0; i < points.Count; ++i)
            {
                data[2 * i] = (float)points[i].Latitude;
                data[2 * i + 1] = (float)points[i].Longitude;
            }
            data[2 * i] = (float)routeLength;
            return data;
        }

        public Route(float[] data)
        {
            int n = (data.Length - 1) / 2;
            points = new List<Point>(n);
            for (int i = 0; i < n; ++i)
            {
                points.Add(new Point(data[2 * i], data[2 * i + 1]));
            }
            routeLength = data[data.Length - 1];
        }
    }
}
