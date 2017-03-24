using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rubiks_cube
{
    public class ArrayHelper
    {
        public static bool Are2DArraysEqual(int[,] a1, int[,] a2)
        {
            if (a1.GetLength(0) != a2.GetLength(0)) return false;
            if (a1.GetLength(1) != a2.GetLength(1)) return false;

            for (int i = 0; i < a1.GetLength(0); i++)
            {
                for (int j = 0; j < a1.GetLength(1); j++)
                {
                    if (a1[i, j] != a2[i, j]) return false;
                }
            }

            return true;
        }
    }
}
