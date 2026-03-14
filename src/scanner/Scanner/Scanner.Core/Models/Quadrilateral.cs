using System;
using System.Collections.Generic;
using System.Text;

using OpenCvSharp;

namespace Scanner.Core.Models
{
    public class Quadrilateral
    {
        public Point2f TopLeft { get; set; }
        public Point2f TopRight { get; set; }
        public Point2f BottomRight { get; set; }
        public Point2f BottomLeft { get; set; }

        private Quadrilateral(Point tl, Point tr, Point br, Point bl)
        {
            TopLeft = tl;
            TopRight = tr;
            BottomRight = br;
            BottomLeft = bl;
        }

        public static Quadrilateral FromPoints(Point[] points)
        {
            Point[] orderedPoints = Geometry.ContourUtils.ReorderContour(points);

            return new Quadrilateral(
                orderedPoints[0],
                orderedPoints[1],
                orderedPoints[2],
                orderedPoints[3]
            );
        }
    }
}
