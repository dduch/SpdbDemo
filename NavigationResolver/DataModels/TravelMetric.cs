using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.DataModels
{
    class TravelMetric
    {
        // Cyclist average velocity in [m/s]
        public double Velocity { get; }

        // Average time of changing bike in [s]
        public double ChangeTime { get; }

        // Free of charge riding time in [s]
        public double FreeOfChargeTime { get; }

        // Returns fee value in [PLN] basing on riding time in [s] 
        public double CostFunction(double time)
        {
            if (time < 20 * 60)
                return 0;

            if (time < 60 * 60)
                return 1;

            if (time < 2 * 60 * 60)
                return 1 + 3;

            if (time < 3 * 60 * 60)
                return 1 + 3 + 5;

            return (1 + 3 + 5) + Math.Ceiling((time - 3.0 * 60.0 * 60.0) / (60.0 * 60.0)) + ((time > 12 * 60 * 60) ? 200 : 0);
        }

        public TravelMetric()
        {
            Velocity = 15.0 * 1000.0 / 3600.0;
            ChangeTime = 2 * 60;
            FreeOfChargeTime = 20 * 60;
        }

        public TravelMetric(double velocity, double changeTime = 2 * 60)
        {
            Velocity = velocity;
            ChangeTime = changeTime;
            FreeOfChargeTime = 20 * 60;
        }
    }
}
