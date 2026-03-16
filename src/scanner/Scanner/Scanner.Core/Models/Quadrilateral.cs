using System;
using System.Collections.Generic;
using System.Text;

using OpenCvSharp;

namespace Scanner.Core.Models
{
    public struct Quadrilateral
    {
        public Point TopLeft { get; set; }
        public Point TopRight { get; set; }
        public Point BottomRight { get; set; }
        public Point BottomLeft { get; set; }

        private Quadrilateral(Point tl, Point tr, Point br, Point bl)
        {
            TopLeft = tl;
            TopRight = tr;
            BottomRight = br;
            BottomLeft = bl;

        }

        public void Scale(double factor)
        {
            // todo: implement scaling logic for quadrilaterals
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
