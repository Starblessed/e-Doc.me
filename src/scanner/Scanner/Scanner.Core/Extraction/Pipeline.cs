using System;
using System.Collections.Generic;
using System.Text;

using OpenCvSharp;
using Scanner.Core.Models;
using Scanner.Core.Extraction;

namespace Scanner.Core.Extraction
{
    public static class Pipeline
    {
        private static Mat LoadImage(string path)
        {
            // todo: load an image into memory
            return new Mat();
        }

        private static void SaveImage(string path, Mat image)
        {
            // todo: save an image to a path
        }

        public static string AutoExtract(string imagePath, string saveToPath)
        {
            Mat image = LoadImage(imagePath);
            Quadrilateral? documentContour = AutoExtractor.AutoExtract(image);

            if (documentContour == null)
            {
                throw new ArgumentNullException(nameof(documentContour),"Could not find any documents in the image.");
            }

            SaveImage(saveToPath, image);

            return saveToPath;
            
        }
    }
}
