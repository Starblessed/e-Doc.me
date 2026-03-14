using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Scanner.Core.Models
{
    public readonly struct Angle
    {
        double Radians { get; }

        private Angle(double radians)
        {
            Radians = radians;
        }

        public static Angle FromPoints(Point[] a, Point[] b, Point[] c)
        {
            // todo: implement cosine formula for vectors
            return new Angle(0);
        }
    }

    public interface IPolygonAngles { int VertexCount { get; } }
    public readonly record struct QuadrilateralAngles
        (
        double TopLeft,
        double TopRight,
        double BottomRight,
        double BottomLeft
        ) : IPolygonAngles
    {
        public int VertexCount => 4;
        
        public static QuadrilateralAngles FromQuadrilateral(Quadrilateral quad)
        {
            // todo: call angle struct to fill in the angles
            return new QuadrilateralAngles(0, 0, 0, 0);
        }
    }
}
