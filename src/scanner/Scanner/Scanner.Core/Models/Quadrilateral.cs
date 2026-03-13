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
    }
}
