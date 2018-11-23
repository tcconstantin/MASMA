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
using System.Threading.Tasks;

namespace MASMA
{
    public class ParallelAlgorithm
    {
        public static void SortMergeInnerPar(int[] src, Int32 l, Int32 r, int[] dst, bool srcToDst = true)
        {
            if (r == l)
            {
                if (srcToDst) dst[l] = src[l];
                return;
            }

            if ((r - l) <= Utils.Threshold)
            {
                Array.Sort(src, l, r - l + 1);
                if (srcToDst)
                    for (int i = l; i <= r; i++)
                        dst[i] = src[i];
                return;
            }

            int m = ((r + l) / 2);

            Parallel.Invoke(
                () => { SortMergeInnerPar(src, l, m, dst, !srcToDst); },
                () => { SortMergeInnerPar(src, m + 1, r, dst, !srcToDst); }
            );

            if (srcToDst) MergeInnerPar(src, l, m, m + 1, r, dst, l);
            else MergeInnerPar(dst, l, m, m + 1, r, src, l);
        }

        public static void MergeInnerPar(int[] src, int p1, int r1, int p2, int r2, int[] dst, int p3)
        {
            int length1 = r1 - p1 + 1;
            int length2 = r2 - p2 + 1;

            if (length1 < length2)
            {
                Exchange(ref p1, ref p2);
                Exchange(ref r1, ref r2);
                Exchange(ref length1, ref length2);
            }

            if (length1 == 0) return;

            int q1 = (p1 + r1) / 2;
            int q2 = BinarySearch(src[q1], src, p2, r2);
            int q3 = p3 + (q1 - p1) + (q2 - p2);
            dst[q3] = src[q1];

            MergeInnerPar(src, p1, q1 - 1, p2, q2 - 1, dst, p3);
            MergeInnerPar(src, q1 + 1, r1, q2, r2, dst, q3 + 1);

            //Parallel.Invoke(
            //    () => { MergeInnerPar(src, p1, q1 - 1, p2, q2 - 1, dst, p3); },
            //    () => { MergeInnerPar(src, q1 + 1, r1, q2, r2, dst, q3 + 1); }
            //);
        }

        /************************************************/
        // BinarySearch

        public static int BinarySearch(int value, int[] a, int left, int right)
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

        /************************************************/
        // Utils

        public static void Exchange(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        /************************************************/
    }
}
