using System;
using System.Collections.Generic;
using System.Linq;

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

                // Calculating the cross product to figure out whether or not the angle is larger than 180°
                double vLeftX = locList.ElementAt(i).Latitude - locList.ElementAt(i - 1).Latitude;
                double vLeftY = locList.ElementAt(i).Longitude - locList.ElementAt(i - 1).Longitude;
                double vRightX = locList.ElementAt(i).Latitude - locList.ElementAt(i + 1).Latitude;
                double vRightY = locList.ElementAt(i).Longitude - locList.ElementAt(i + 1).Longitude;
                double cross = vLeftX * vRightY - vRightX * vLeftY;
                double sineA = cross / dLeft / dRight;

                if (sineA < 0)
                {
                    return false;
                }

                // We only need the following lines if we want to know the exact angle.
                /*
                double dCenter = locList.ElementAt(i - 1).Distance(locList.ElementAt(i + 1));
                double angle = (180 / Math.PI) * Math.Acos((dLeft * dLeft + dRight * dRight - dCenter * dCenter) / (2 * dLeft * dRight));
                if (sineA < 0)
                {
                    angle += 180;
                }
                // Console.WriteLine(string.Format("Angle: {0}\n\tdLeft: {1}\n\tdRight: {2}\n\tdCenter: {3}\n\tsin(a): {4}", angle, dLeft, dRight, dCenter, sineA));
                if (angle > 180)
                {
                    return false;  // If an internal angle is larger than 180°, this cannot be a convex polygon.
                }
                */
            }
            return true;
        }


        /// <summary>
        /// Returns the circumference of the polygon in meters, or all distances between the vertices in a cycle.
        /// </summary>
        /// <returns>The circumference in meters.</returns>
        public UInt64 Circumference()
        {
            UInt64 circumference = 0;
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                circumference += (UInt64) Vertices.ElementAt(i).Distance(Vertices.ElementAt(i+1));                
            }
            circumference += (UInt64) Vertices.First().Distance(Vertices.Last());
            return circumference;
        }


        /// <summary>
        /// Returns the area of the polygon (assuming it is a simple polygon, meaning it does not intersect itself) in square meters.
        /// </summary>
        /// <returns>The area of the polygon in square meters.</returns>
        public UInt64 Area()
        {
            double acc = 0;
            for (int i = 0; i < Vertices.Count - 1; i++)
            {
                acc += Vertices.ElementAt(i).Longitude * Vertices.ElementAt(i + 1).Latitude - Vertices.ElementAt(i + 1).Longitude * Vertices.ElementAt(i).Latitude;
            }
            acc += Vertices.Last().Longitude * Vertices.First().Latitude - Vertices.First().Longitude * Vertices.Last().Latitude;
            return (UInt64) (10.5892 * 1000 * 1000 * 1000 *  Math.Abs(acc) / 2); // Aproximation of longitude/latitude degrees to meters in the central european area.
        }


        public override String ToString()
        {
            return string.Format("GeoPolygon: [({0})]", string.Join(", ", Vertices));
        }
    }

    
}
