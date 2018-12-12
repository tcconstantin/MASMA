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
using System.Collections.Generic;
using System.Text;

namespace MASMA
{
    public class Utils
    {
        // Common
        public static int Length = 50;
        public static int[] Source = new int[Length];
        public static int[] Destination = new int[Length];
        public static Random Rand = new Random();
        public static ActressMas.Environment Env;

        // MergeSort
        public static int Threshold = 5;

        // Enumeration
        public static bool SkipNext = false;
        public static int NoAgents = 10;

        // OddEven
        public static bool IsSorted = false;
        public static bool OrderType = true;

        public static int NrAgent;
        public static int slaveElem;
        public static List<String> AgentPool = new List<string>();
        public static List<string> ValidagentPool = new List<string>();
        public static Dictionary<String, int[]> destination = new Dictionary<string, int[]>();
        public static List<int> lsort = new List<int>();
        public static Boolean finalSplit = false;

        // Tests
        public static void Assert(int[] array)
        {
            Program.Log.Warn("Array: ");

            StringBuilder text = new StringBuilder();

            for (int i = 0; i < array.Length - 1; ++i)
            {
                if (array[i] > array[i + 1])
                {
                    Console.WriteLine("FAIL");
                    break;
                }

                if(array[i] != 0)
                    text.Append($"{array[i]} ");
            }

            Program.Log.Warn(text.ToString());
        }

        public static void Init(Case @case)
        {
            switch (@case)
            {
                case Case.WORST:
                    for (int i = 0; i < Length; i++)
                    {
                        Source[i] = Length - i;
                    }
                    break;
                case Case.AVERAGE:
                    for (int i = 0; i < Length; i++)
                    {
                        Source[i] = Rand.Next(Length);
                    }
                    break;
                case Case.BEST:
                    for (int i = 0; i < Length; i++)
                    {
                        Source[i] = i;
                    }
                    break;
            }
        }
    }

    public enum Case
    {
        BEST = 0,
        WORST,
        AVERAGE
    }
}
