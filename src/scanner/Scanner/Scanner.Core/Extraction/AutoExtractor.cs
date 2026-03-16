using System;
using System.Collections.Generic;
using System.Text;
using Scanner.Core.Models;

using OpenCvSharp;
using System.Security.Cryptography.X509Certificates;

namespace Scanner.Core.Extraction
{
    public static class AutoExtractor
    {
        public static void AutoExtract(Mat image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Image cannot be null.");

            } else if (image.Size().Width == 0 | image.Size().Height == 0)
            {
                throw new ArgumentException("Image cannot have 0 width nor height.", nameof(image));
            }
            
            // Adjusts dimensions to perform operations
            int originalWidth = image.Size().Width;
            int originalHeight = image.Size().Height;

            int targetHeight = 1024; // Fixed unless parameters can be changed by the user

            double scale = originalHeight > targetHeight ? targetHeight / originalHeight : 1.0f;

            int newWidth = (int)(originalWidth * scale);
            int newHeight = (int)(originalHeight * scale);

            int workingArea = newWidth * newHeight;

            Size newDimensions = new Size(newWidth, newHeight);

            // Rescales image
            using Mat scaledImage = new Mat();
            Cv2.Resize(image, scaledImage, newDimensions);

            // Converts image to grayscale
            using Mat grayScaleImage = new Mat();
            Cv2.CvtColor(scaledImage, grayScaleImage, ColorConversionCodes.BGR2GRAY);

            // Blurs the image
            using Mat blurredImage = new Mat();
            Size blurKernelSize = new Size(5, 5);
            Cv2.GaussianBlur(grayScaleImage, blurredImage, blurKernelSize, 0);

            // Extracts edges with the Canny operation
            using Mat edgeImage = new Mat();
            int thresholdLow = 50 , thresholdHigh = 150;
            Cv2.Canny(blurredImage, edgeImage, thresholdLow, thresholdHigh);

            // Closes the edges with morphologic transformations (close, close, dilate, erode)
            using Mat closedEdgeImage = new Mat();
            Size closeEdgeKernelSize = new Size(5, 5);
            Mat closeEdgeElement = Cv2.GetStructuringElement(MorphShapes.Rect, closeEdgeKernelSize);
            Cv2.MorphologyEx(edgeImage, closedEdgeImage, MorphTypes.Close, closeEdgeElement, iterations: 2);
            Cv2.Dilate(closedEdgeImage, closedEdgeImage, closeEdgeElement, iterations: 1);
            Cv2.Erode(closedEdgeImage, closedEdgeImage, closeEdgeElement, iterations: 1);

            // Gets contours
            Point[][] contours; // Holds contour points
            HierarchyIndex[] hierarchy; // Not used
            Cv2.FindContours(
                closedEdgeImage,
                out contours,
                out hierarchy,
                mode: RetrievalModes.List,
                method: ContourApproximationModes.ApproxSimple
            );

            // todo: call contour utils to get best contour and return it then
            //       convert method to return a new Quadrilateral instance
        }
    }
}
