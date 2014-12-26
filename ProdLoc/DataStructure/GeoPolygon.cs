using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdLoc
{
    public class GeoPolygon
    {
        public List<GeoLocation> Vertices { get; private set; }

        public GeoPolygon(List<GeoLocation> vertices)
        {
            Vertices = vertices;
        }

        /// <summary>
        /// Returns whether or not the polygon is a convex polygon.
        /// This is a requirement for some clipping algorithms in order to use the polygon as a clipping window.
        /// </summary>
        /// <returns>True if the polygon is convex, or False if it is not.</returns>
        public bool isConvex()
        {
            throw new NotImplementedException();
        }
    }

    
}
