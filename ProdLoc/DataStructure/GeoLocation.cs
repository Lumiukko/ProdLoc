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

        public override String ToString()
        {
            return string.Format("GeoLocation: [ID={0}, Longitude=\"{1}\", Latitude={2}, Accuracy={3}]", ID, Longitude, Latitude, Accuracy);
        }
    }
}
