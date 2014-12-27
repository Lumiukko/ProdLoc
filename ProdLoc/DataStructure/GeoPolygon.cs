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
            List<GeoLocation> locList = new List<GeoLocation>();
            locList.Add(Vertices.Last());
            locList.AddRange(Vertices);
            locList.Add(Vertices.First());
            for (int i = 1; i < locList.Count - 1; i++) 
            {
                double dLeft = locList.ElementAt(i).Distance(locList.ElementAt(i - 1));
                double dRight = locList.ElementAt(i).Distance(locList.ElementAt(i + 1));
                double dCenter = locList.ElementAt(i - 1).Distance(locList.ElementAt(i + 1));
                double angle = Math.Acos((dLeft * dLeft + dRight * dRight - dCenter * dCenter) / (2 * dLeft * dRight));
                angle *= (180 / Math.PI);
                //TODO: Something is FUBAR here, the angles are completely messed up...
                Console.WriteLine(string.Format("Angle: {0}\n\tdLeft: {1}\n\tdRight: {2}\n\tdCenter: {3}", angle, dLeft, dRight, dCenter));
                if (angle > 180)
                {
                    //return false;  // If an internal angle is larger than 180°, this cannot be a convex polygon.
                }
            }
            return true;
        }


        /// <summary>
        /// Returns the circumference of the polygon in meters, or all distances between the vertices in a cycle.
        /// </summary>
        /// <returns>The circumference in meters.</returns>
        public double Circumference()
        {
            double circumference = 0;
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                circumference += Vertices.ElementAt(i).Distance(Vertices.ElementAt(i+1));                
            }
            circumference += Vertices.First().Distance(Vertices.Last());
            return circumference;
        }


        /// <summary>
        /// Returns the area of the polygon (assuming it is a simple polygon, meaning it does not intersect itself) in square meters.
        /// </summary>
        /// <returns>The area of the polygon in square meters.</returns>
        public double Area()
        {
            double acc = 0;
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                acc += Vertices.ElementAt(i).Longitude * Vertices.ElementAt(i + 1).Latitude - Vertices.ElementAt(i + 1).Longitude * Vertices.ElementAt(i).Latitude;
            }
            acc += Vertices.Last().Longitude * Vertices.First().Latitude - Vertices.First().Longitude * Vertices.Last().Latitude;
            return 110 * 1000 * 1000 * Math.Abs(acc) / 2; // Aproximation of longitude/latitude degrees to meters in the central european area.
        }


        public override String ToString()
        {
            return string.Format("GeoPolygon: [({0})]", string.Join(", ", Vertices));
        }
    }

    
}
