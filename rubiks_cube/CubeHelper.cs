using System;
using System.Collections.Generic;

namespace rubiks_cube
{
    public static class CubeHelper
    {
        public static ConsoleColor GetCubieColor(int face)
        {
            Dictionary<int, ConsoleColor> colors = new Dictionary<int, ConsoleColor>()
            {
                { 0, ConsoleColor.White },
                { 1, ConsoleColor.Red },
                { 2, ConsoleColor.Green },
                { 3, ConsoleColor.Magenta },
                { 4, ConsoleColor.Blue },
                { 5, ConsoleColor.Yellow }
            };

            return colors[face];
        }
    }
}
