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

namespace MASMA
{
    public class Utils
    {

        //env
        public static ActressMas.Environment env;
        // Common
        public static int Length = 16;
        public static int[] Source = new int[Length];
        public static int[] Destination = new int[Length];
        public static Random Rand = new Random();

        // MergeSort
        public static int Threshold = 1;

        // Enumeration
        public static bool SkipNext = false;
        public static int NoAgents = 10;

        // OddEven
        public static bool IsSorted = false;
        public static bool OrderType = true;

        public static int numAg;
        public static int slaveElem;
        public static List<String> agentPool = new List<string>();
        public static List<String> validagentPool = new List<string>();
        public static Dictionary<String, int[]> destination = new Dictionary<string, int[]>();
        public static List<int> lsort = new List<int>();

        public static Boolean finalSplit = false;

        // Tests
        public static void Assert(int[] array)
        {
            for (int i = 0; i < Length - 1; ++i)
            {
                if (array[i] > array[i + 1])
                {
                    Console.WriteLine("FAIL");
                    break;
                }
            }
        }
        public static string Str(object p1, object p2)
        {
            return string.Format("{0} {1}", p1, p2);
        }
        public static void ParseMessage(string content, out string action, out string parameters)
        {
            string[] t = content.Split();

            action = t[0];

            parameters = "";

            if (t.Length > 1)
            {
                for (int i = 1; i < t.Length - 1; i++)
                    parameters += t[i] + " ";
                parameters += t[t.Length - 1];
            }
        }
        public static void ParseMessage(string content, out string action, out List<string> parameters)
        {
            string[] t = content.Split();

            action = t[0];

            parameters = new List<string>();
            for (int i = 1; i < t.Length; i++)
                parameters.Add(t[i]);
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

        // Utils
        public static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
    }

    public enum Case
    {
        BEST = 0,
        WORST,
        AVERAGE
    }
}