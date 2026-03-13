using System;
using System.Collections.Generic;
using System.Text;

using Scanner.Core.Models;
using OpenCvSharp;
using System.Formats.Asn1;

namespace Scanner.Core.Geometry
{
    public static class ContourUtils
    {
        public static void GetBestDocumentContour(Point[][] contours, double workArea)
        {
            /*
            foreach (Point[] contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area < 0.10 * workArea) { continue; }


            }
            */
        }
    }
}
