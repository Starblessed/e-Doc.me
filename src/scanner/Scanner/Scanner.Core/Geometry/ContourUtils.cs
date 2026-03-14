using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Scanner.Core.Models;
using OpenCvSharp;
using System.Formats.Asn1;

namespace Scanner.Core.Geometry
{
    public static class ContourUtils
    {
        public static void GetBestDocumentContour(Point[][] contours, double workArea)
        {
            Quadrilateral bestContour;
            double bestScore = -1;


            foreach (Point[] contour in contours)
            {
                double area = Cv2.ContourArea(contour);

                if (area < 0.10 * workArea) { continue; }

                double perimeter = Cv2.ArcLength(contour, true);
                Point[] approx = Cv2.ApproxPolyDP(contour, 0.02 * perimeter, true);

                if (approx.Length != 4) { continue; }
                if (!Cv2.IsContourConvex(approx)) { continue; }

                Mat reshapedPoints = InputArray.Create(approx).GetMat().Reshape(4, 2);
                Rect contourRectangle = Cv2.BoundingRect(approx);

                double width = contourRectangle.Width;
                double height = contourRectangle.Height;

                if ((width < 40) | (height < 40)) { continue; }

                double aspectRatio = Math.Max(width, height) / Math.Max(1, Math.Min(width, height));

                if (aspectRatio > 8.0) { continue; }

                double rectangleArea = width * height;
                double fillRatio = rectangleArea > 0 ? area / rectangleArea : 0;

                if (fillRatio < 0.45) { continue; }




            }

        }

        public static Point[] ReorderContour(Point[] contour)
        {

            if (contour == null || contour.Length == 0)
            {
                throw new ArgumentException("Contour cannot be null nor empty.", nameof(contour));
            }

            double[] sum = contour.Select(point => GetPointSum(point)).ToArray();
            double[] difference = contour.Select(point => GetPointDifference(point)).ToArray();

            Point[] orderedContour = new Point[4];

            orderedContour[0] = contour[Commons.ArgMin(sum)];
            orderedContour[2] = contour[Commons.ArgMax(sum)];
            orderedContour[1] = contour[Commons.ArgMin(difference)];
            orderedContour[3] = contour[Commons.ArgMax(difference)];

            return orderedContour;

        }

        public static double GetPointSum(Point point)
        {
            return point.X + point.Y;
        }
        public static double GetPointDifference(Point point)
        {
            return point.Y - point.X;
        }
    }
}
