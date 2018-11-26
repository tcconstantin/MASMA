/**************************************************************************
 *                                                                        *
 *  File:        Utils.cs                                                 *
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

namespace MASMA
{
    public class Utils
    {
        public static int Length = 100;
        public static int Threshold = 5;
        public static int[] Source = new int[Length];
        public static int[] Destination = new int[Length];

        public static bool SkipNext = false;
        public static int NoAgents = 10;

        public static void Assert()
        {
            for (int i = 0; i < Utils.Length - 1; ++i)
            {
                if (Destination[i] > Destination[i + 1])
                {
                    Console.WriteLine("FAIL");
                    break;
                }
            }
        }

        public static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }
}