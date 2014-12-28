using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLoc
{
    public class GeoLocation
    {
        public Int64 ID { get; private set; }
        public Double Longitude { get; private set; }
        public Double Latitude { get; private set; }
        public int Accuracy { get; private set; } 


        public GeoLocation(Double longitude, Double latitude, int accuracy)
        {
            Longitude = longitude;
            Latitude = latitude;
            Accuracy = accuracy;
        }


        public GeoLocation(Int64 id, Double longitude, Double latitude, int accuracy)
        {
            ID = id;
            Longitude = longitude;
            Latitude = latitude;
            Accuracy = accuracy;
        }


        /// <summary>
        /// Returns the distance of this location to another one in meters.
        /// </summary>
        /// <param name="location">The second GeoLocation to which the distance is calculated.</param>
        /// <returns>The distance between both locations in meters.</returns>
        public double Distance(GeoLocation location)
        {
            double r = 6378.137;
            double dLat = (location.Latitude - this.Latitude) * Math.PI / 180;
            double dLong = (location.Longitude - this.Longitude) * Math.PI / 180;
            double a = Math.Pow(Math.Sin(dLat / 2), 2)
                        + Math.Cos(this.Latitude * Math.PI / 180)
                        * Math.Cos(location.Latitude * Math.PI / 180)
                        * Math.Pow(Math.Sin(dLong / 2), 2);
            return 1000 * r * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        }


        public override String ToString()
        {
            return string.Format("GeoLocation: [ID={0}, Longitude={1}, Latitude={2}, Accuracy={3}]", ID, Longitude, Latitude, Accuracy);
        }
    }
}
