using System;

namespace rubiks_cube
{
    class Program
    {
        /* Example scrambles:
         * B U' R' U' D' F D2 R2 D2 F' L2 R B U2 L R D F' D F' D B2 U2 L R2
         */

        static void Main(string[] args)
        {
            Cube cube = new Cube();
            //cube.Scramble("B U' R' U' D' F D2 R2 D2 F' L2 R B U2 L R D F' D F' D B2 U2 L R2");
            cube.Render();
            //Console.ReadKey();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                string input = Console.ReadLine();
                Move inputMove = Move.Parse(input);

                Console.Clear();
                cube.PerformMove(inputMove);
                cube.Render();
            }
        }
    }
}
