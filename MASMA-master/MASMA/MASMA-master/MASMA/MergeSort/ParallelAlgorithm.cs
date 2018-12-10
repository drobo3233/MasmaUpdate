/**************************************************************************
 *                                                                        *
 *  File:        ParallelAlgorithm.cs                                     *
 *  Description: Merge Sort multi-agent                                   *
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/
  
using System;

namespace MASMA.MergeSort
{
    public class ParallelAlgorithm
    {
        public static void MergeInnerPar(int[] src, int p1, int r1, int p2, int r2, int[] dst, int p3)
        {
            int length1 = r1 - p1 + 1;
            int length2 = r2 - p2 + 1;

            if (length1 < length2)
            {
                Utils.Swap(ref p1, ref p2);
                Utils.Swap(ref r1, ref r2);
                Utils.Swap(ref length1, ref length2);
            }

            if (length1 == 0) return;

            int q1 = (p1 + r1) / 2;
            int q2 = BinarySearch(src[q1], src, p2, r2);
            int q3 = p3 + (q1 - p1) + (q2 - p2);
            dst[q3] = src[q1];

            MergeInnerPar(src, p1, q1 - 1, p2, q2 - 1, dst, p3);
            MergeInnerPar(src, q1 + 1, r1, q2, r2, dst, q3 + 1);
        }

        /************************************************/
        // BinarySearch

        private static int BinarySearch(int value, int[] a, int left, int right)
        {
            int low = left;
            int high = Math.Max(left, right + 1);

            while (low < high)
            {
                int mid = (low + high) / 2;
                if (value <= a[mid])
                    high = mid;
                else low = mid + 1;
            }

            return high;
        }
    }
}
