using System;

namespace rubiks_cube
{
    public abstract class Move
    {
        public Rotation Rotation { get; private set; }

        protected Move(Rotation rotation)
        {
            Rotation = rotation;
        }

        public static Move Parse(string move)
        {
            char[] moveChars = move.ToCharArray();

            Move desiredMove = new SingleLayerMove(Side.Front, Rotation.Clockwise);

            Side side = Side.Front;
            Rotation rotation = Rotation.Clockwise;

            if (moveChars.Length == 1)
            {
                if (moveChars[0] == 'x' || moveChars[0] == 'y' || moveChars[0] == 'z')
                {
                    desiredMove = new WholeCubeMove(GetReferencedAxis(moveChars[0]), rotation);
                }
                else
                {
                    side = GetReferencedSide(moveChars[0]);

                    // TODO: try-catch error

                    if (char.IsUpper(moveChars[0]))
                    {
                        desiredMove = new SingleLayerMove(side, rotation);
                    }
                    else if (char.IsLower(moveChars[0]))
                    {
                        desiredMove = new DoubleLayerMove(side, rotation);
                    }
                    else
                    {
                        // TODO: Throw error
                    }
                }
            }
            else if (moveChars.Length == 2)
            {
                if (moveChars[1] == '\'')
                {
                    rotation = Rotation.Anticlockwise;
                }
                else if (moveChars[1] == '2')
                {
                    rotation = Rotation.HalfTurn;
                }
                else
                {
                    throw new InvalidMoveException();
                }


                if (moveChars[0] == 'x' || moveChars[0] == 'y' || moveChars[0] == 'z')
                {
                    desiredMove = new WholeCubeMove(GetReferencedAxis(moveChars[0]), rotation);
                }
                else
                {
                    side = GetReferencedSide(moveChars[0]);

                    // TODO: try-catch error

                    if (char.IsUpper(moveChars[0]))
                    {
                        desiredMove = new SingleLayerMove(side, rotation);
                    }
                    else if (char.IsLower(moveChars[0]))
                    {
                        desiredMove = new DoubleLayerMove(side, rotation);
                    }
                    else
                    {
                        // TODO: Throw error
                    }
                }
            }
            else
            {
                throw new InvalidMoveException();
            }

            return desiredMove;
        }

        private static Axis GetReferencedAxis(char c)
        {
            switch (c)
            {
                case 'x': return Axis.X;
                case 'y': return Axis.Y;
                case 'z': return Axis.Z;
                default:
                    throw new Exception("Unrecognised side: " + c);
            }
        }

        private static Side GetReferencedSide(char c)
        {
            switch (c)
            {
                case 'F':
                case 'f':
                    return Side.Front;
                case 'L':
                case 'l':
                    return Side.Left;
                case 'U':
                case 'u':
                    return Side.Up;
                case 'R':
                case 'r':
                    return Side.Right;
                case 'D':
                case 'd':
                    return Side.Down;
                case 'B':
                case 'b':
                    return Side.Back;
                default:
                    throw new Exception("Unrecognised side: " + c);
            }
        }
    }
}
