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
        public float Accuracy { get; private set; }

        public GeoLocation(Double longitude, Double latitude, float accuracy)
        {
            Longitude = longitude;
            Latitude = latitude;
            Accuracy = accuracy;
        }

        public GeoLocation(Int64 id, Double longitude, Double latitude, float accuracy)
        {
            ID = id;
            Longitude = longitude;
            Latitude = latitude;
            Accuracy = accuracy;
        }
    }
}
