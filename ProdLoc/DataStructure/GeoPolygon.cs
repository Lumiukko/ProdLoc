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
        public bool IsConvex()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Returns the circumference of the polygon in meters, or all distances between the vertices in a cycle.
        /// </summary>
        /// <returns>The circumference in meters.</returns>
        public double Circumference()
        {
            double circumference = 0;
            for (int i = 0; i < Vertices.Count-1; i++)
            {
                circumference += Vertices.ElementAt(i).Distance(Vertices.ElementAt(i+1));                
            }
            circumference += Vertices.First().Distance(Vertices.Last());
            return circumference;
        }


        public override String ToString()
        {
            return string.Format("GeoPolygon: [({0})]", string.Join(", ", Vertices));
        }
    }

    
}
