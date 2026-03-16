using System;
using System.Collections.Generic;
using System.Text;

namespace Scanner.Core
{
    public class Commons
    {
        public static int ArgMax(double[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                throw new ArgumentException("Array cannot be null nor empty.", nameof(arr));
            }

            double maxValue = arr[0];
            int maxValueIndex = 0;

            for (int i = 1; i < arr.Length; ++i)
            {
                if (arr[i] > maxValue)
                {
                    maxValue = arr[i];
                    maxValueIndex = i;
                }
            }

            return maxValueIndex;
            
        }
        public static int ArgMin(double[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                throw new ArgumentException("Array cannot be null nor empty.", nameof(arr));
            }

            double minValue = arr[0];
            int minValueIndex = 0;

            for (int i = 1; i < arr.Length; ++i)
            {
                if (arr[i] < minValue)
                {
                    minValue = arr[i];
                    minValueIndex = i;
                }
            }

            return minValueIndex;
            
        }
    }
}
