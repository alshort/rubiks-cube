using System;
using System.Collections.Generic;

namespace rubiks_cube
{
    public class Cube
    {
        private const int CubeSides = 6;


        private int width = 3;
        private int height = 3;
        private int depth = 3;

        private Face[] faces;
        private Dictionary<Side, Face> orientation;

        public Cube()
        {
            // Instantiate cubes
            faces = new Face[CubeSides];
            for (int i = 0; i < CubeSides; i++)
            {
                faces[i] = new Face(i);
            }

            // Define their initial positions
            orientation = new Dictionary<Side, Face>();
            orientation.Add(Side.Front, faces[0]);
            orientation.Add(Side.Left, faces[1]);
            orientation.Add(Side.Up, faces[2]);
            orientation.Add(Side.Right, faces[3]);
            orientation.Add(Side.Down, faces[4]);
            orientation.Add(Side.Back, faces[5]);

            // Define relationships between them
            faces[0].SetSide(Side.Front);
            faces[1].SetSide(Side.Left);
            faces[2].SetSide(Side.Up);
            faces[3].SetSide(Side.Right);
            faces[4].SetSide(Side.Down);
            faces[5].SetSide(Side.Back);

            SetRelationships();
        }

        private void SetRelationships()
        {
            orientation[Side.Front].SetFaceRelationship(
                new FaceRelationship(orientation[Side.Up], orientation[Side.Left], orientation[Side.Right], orientation[Side.Down]));
            orientation[Side.Left].SetFaceRelationship(
                new FaceRelationship(orientation[Side.Up], orientation[Side.Back], orientation[Side.Front], orientation[Side.Down]));
            orientation[Side.Up].SetFaceRelationship(
                new FaceRelationship(orientation[Side.Back], orientation[Side.Left], orientation[Side.Right], orientation[Side.Front]));
            orientation[Side.Right].SetFaceRelationship(
                new FaceRelationship(orientation[Side.Up], orientation[Side.Front], orientation[Side.Back], orientation[Side.Down]));
            orientation[Side.Down].SetFaceRelationship(
                new FaceRelationship(orientation[Side.Front], orientation[Side.Left], orientation[Side.Right], orientation[Side.Back]));
            orientation[Side.Back].SetFaceRelationship(
                new FaceRelationship(orientation[Side.Down], orientation[Side.Left], orientation[Side.Right], orientation[Side.Up]));
        }

        public void Scramble(string sequence = null)
        {
            if (sequence != null)
            {
                // Parse all the steps and scramble/move accordingly
                string[] moves = sequence.Split(' ');
                foreach (string moveStr in moves)
                {
                    Move move = Move.Parse(moveStr);
                    PerformMove(move);
                }
            }
            else
            {
                // Random sequence of random length
                Random random = new Random();

            }
        }

        public void PerformMove(Move move)
        {
            if (move is SingleLayerMove)
            {
                SingleLayerMove singleLayerMove = move as SingleLayerMove;

                switch (singleLayerMove.Rotation)
                {
                    case Rotation.Clockwise:
                        orientation[singleLayerMove.Side].RotateClockwise();
                        break;
                    case Rotation.Anticlockwise:
                        orientation[singleLayerMove.Side].RotateAnticlockwise();
                        break;
                    case Rotation.HalfTurn:
                        orientation[singleLayerMove.Side].RotateClockwise();
                        orientation[singleLayerMove.Side].RotateClockwise();
                        break;
                }
            }
            else if (move is DoubleLayerMove)
            {
                DoubleLayerMove doubleLayerMove = move as DoubleLayerMove;

                // Rotate the cube and twist the opposite layer in the opposite direction
                // (Hey, I'm a programmer, I'm lazy...)
                Axis axis = Axis.X;
                Rotation singleSliceRotation = Rotation.Clockwise;
                Rotation cubeRotation = Rotation.Clockwise;

                switch (doubleLayerMove.Side)
                {
                    case Side.Front:
                        axis = Axis.Z;
                        cubeRotation = Rotation.Clockwise;
                        singleSliceRotation = Rotation.Clockwise;
                        break;
                    case Side.Left:
                        axis = Axis.X;
                        cubeRotation = Rotation.Anticlockwise;
                        singleSliceRotation = Rotation.Clockwise;
                        break;
                    case Side.Up:
                        axis = Axis.Y;
                        cubeRotation = Rotation.Clockwise;
                        singleSliceRotation = Rotation.Clockwise;
                        break;
                    case Side.Right:
                        axis = Axis.X;
                        cubeRotation = Rotation.Clockwise;
                        singleSliceRotation = Rotation.Clockwise;
                        break;
                    case Side.Down:
                        axis = Axis.Y;
                        cubeRotation = Rotation.Anticlockwise;
                        singleSliceRotation = Rotation.Clockwise;
                        break;
                    case Side.Back:
                        axis = Axis.Z;
                        cubeRotation = Rotation.Anticlockwise;
                        singleSliceRotation = Rotation.Clockwise;
                        break;
                }

                if (doubleLayerMove.Rotation == Rotation.Anticlockwise)
                {
                    cubeRotation = GetOppositeRotation(cubeRotation);
                    singleSliceRotation = GetOppositeRotation(singleSliceRotation);
                }

                RotateCubeAboutAxis(axis, cubeRotation);
                PerformMove(new SingleLayerMove(GetOppositeSide(doubleLayerMove.Side), singleSliceRotation));
            }
            else if (move is WholeCubeMove)
            {
                WholeCubeMove wholeCubeMove = move as WholeCubeMove;
                RotateCubeAboutAxis(wholeCubeMove.Axis, wholeCubeMove.Rotation);
            }
            // TODO: Middle slice moves
            else
            {
                throw new Exception("Invalid actual type of move");
            }
        }

        public void RotateCubeAboutAxis(Axis axis, Rotation rotation)
        {
            // Need to be conscious of the update order!
            switch (axis)
            {
                case Axis.X:
                    switch (rotation)
                    {
                        case Rotation.Clockwise:
                            Face up = orientation[Side.Up];
                            AssignFaceToSide(orientation[Side.Front], Side.Up);
                            AssignFaceToSide(orientation[Side.Down], Side.Front);
                            AssignFaceToSide(orientation[Side.Back], Side.Down);
                            AssignFaceToSide(up, Side.Back);

                            // Rotate left and right faces
                            orientation[Side.Left] = Face.CopyAndRotate(orientation[Side.Left], GetOppositeRotation(rotation));
                            orientation[Side.Right] = Face.CopyAndRotate(orientation[Side.Right], rotation);
                            break;

                        case Rotation.Anticlockwise:
                            Face back = orientation[Side.Back];
                            AssignFaceToSide(orientation[Side.Down], Side.Back);
                            AssignFaceToSide(orientation[Side.Front], Side.Down);
                            AssignFaceToSide(orientation[Side.Up], Side.Front);
                            AssignFaceToSide(back, Side.Up);

                            // Rotate left and right faces
                            orientation[Side.Left] = Face.CopyAndRotate(orientation[Side.Left], GetOppositeRotation(rotation));
                            orientation[Side.Right] = Face.CopyAndRotate(orientation[Side.Right], rotation);
                            break;

                        case Rotation.HalfTurn:
                            Face temp3 = orientation[Side.Up];
                            AssignFaceToSide(orientation[Side.Down], Side.Up);
                            AssignFaceToSide(temp3, Side.Down);

                            temp3 = orientation[Side.Front];
                            AssignFaceToSide(orientation[Side.Back], Side.Front);
                            AssignFaceToSide(temp3, Side.Back);

                            // Rotate left and right faces
                            orientation[Side.Left] = Face.CopyAndRotate(orientation[Side.Left], rotation);
                            orientation[Side.Right] = Face.CopyAndRotate(orientation[Side.Right], rotation);
                            break;
                    }
                    break;

                case Axis.Y:
                    switch (rotation)
                    {
                        case Rotation.Clockwise:
                            Face temp4 = orientation[Side.Left];
                            AssignFaceToSide(orientation[Side.Front], Side.Left);
                            AssignFaceToSide(orientation[Side.Right], Side.Front);
                            AssignFaceToSide(orientation[Side.Back], Side.Right);
                            AssignFaceToSide(temp4, Side.Back);

                            // Account for projection
                            orientation[Side.Right] = Face.CopyAndRotate(orientation[Side.Right], Rotation.HalfTurn);
                            orientation[Side.Back] = Face.CopyAndRotate(orientation[Side.Back], Rotation.HalfTurn);

                            // Rotate up and down faces
                            orientation[Side.Up] = Face.CopyAndRotate(orientation[Side.Up], rotation);
                            orientation[Side.Down] = Face.CopyAndRotate(orientation[Side.Down], GetOppositeRotation(rotation));
                            break;

                        case Rotation.Anticlockwise:
                            Face temp5 = orientation[Side.Right];
                            AssignFaceToSide(orientation[Side.Front], Side.Right);
                            AssignFaceToSide(orientation[Side.Left], Side.Front);
                            AssignFaceToSide(orientation[Side.Back], Side.Left);
                            AssignFaceToSide(temp5, Side.Back);

                            // Account for projection
                            orientation[Side.Left] = Face.CopyAndRotate(orientation[Side.Left], Rotation.HalfTurn);
                            orientation[Side.Back] = Face.CopyAndRotate(orientation[Side.Back], Rotation.HalfTurn);

                            // Rotate up and down faces
                            orientation[Side.Up] = Face.CopyAndRotate(orientation[Side.Up], rotation);
                            orientation[Side.Down] = Face.CopyAndRotate(orientation[Side.Down], GetOppositeRotation(rotation));
                            break;

                        case Rotation.HalfTurn:
                            Face temp6 = orientation[Side.Left];
                            AssignFaceToSide(orientation[Side.Right], Side.Left);
                            AssignFaceToSide(temp6, Side.Right);

                            temp6 = orientation[Side.Front];
                            AssignFaceToSide(orientation[Side.Back], Side.Front);
                            AssignFaceToSide(temp6, Side.Back);

                            // Account for projection
                            orientation[Side.Front] = Face.CopyAndRotate(orientation[Side.Front], Rotation.HalfTurn);
                            orientation[Side.Back] = Face.CopyAndRotate(orientation[Side.Back], Rotation.HalfTurn);

                            // Rotate up and down faces
                            orientation[Side.Up] = Face.CopyAndRotate(orientation[Side.Up], rotation);
                            orientation[Side.Down] = Face.CopyAndRotate(orientation[Side.Down], rotation);
                            break;
                    }
                    break;

                case Axis.Z:
                    switch (rotation)
                    {
                        case Rotation.Clockwise:
                            Face temp7 = orientation[Side.Left];
                            AssignFaceToSide(orientation[Side.Down], Side.Left);
                            AssignFaceToSide(orientation[Side.Right], Side.Down);
                            AssignFaceToSide(orientation[Side.Up], Side.Right);
                            AssignFaceToSide(temp7, Side.Up);

                            // Account for projection
                            orientation[Side.Left] = Face.CopyAndRotate(orientation[Side.Left], rotation);
                            orientation[Side.Right] = Face.CopyAndRotate(orientation[Side.Right], rotation);
                            orientation[Side.Front] = Face.CopyAndRotate(orientation[Side.Front], rotation);
                            orientation[Side.Back] = Face.CopyAndRotate(orientation[Side.Back], GetOppositeRotation(rotation));
                            orientation[Side.Up] = Face.CopyAndRotate(orientation[Side.Up], rotation);
                            orientation[Side.Down] = Face.CopyAndRotate(orientation[Side.Down], rotation);
                            break;

                        case Rotation.Anticlockwise:
                            Face temp8 = orientation[Side.Left];
                            AssignFaceToSide(orientation[Side.Up], Side.Left);
                            AssignFaceToSide(orientation[Side.Right], Side.Up);
                            AssignFaceToSide(orientation[Side.Down], Side.Right);
                            AssignFaceToSide(temp8, Side.Down);

                            // Rotate front and back faces
                            orientation[Side.Left] = Face.CopyAndRotate(orientation[Side.Left], rotation);
                            orientation[Side.Right] = Face.CopyAndRotate(orientation[Side.Right], rotation);
                            orientation[Side.Front] = Face.CopyAndRotate(orientation[Side.Front], rotation);
                            orientation[Side.Back] = Face.CopyAndRotate(orientation[Side.Back], GetOppositeRotation(rotation));
                            orientation[Side.Up] = Face.CopyAndRotate(orientation[Side.Up], rotation);
                            orientation[Side.Down] = Face.CopyAndRotate(orientation[Side.Down], rotation);
                            break;

                        case Rotation.HalfTurn:
                            Face temp9 = orientation[Side.Left];
                            AssignFaceToSide(orientation[Side.Right], Side.Left);
                            AssignFaceToSide(temp9, Side.Right);

                            temp9 = orientation[Side.Up];
                            AssignFaceToSide(orientation[Side.Down], Side.Up);
                            AssignFaceToSide(temp9, Side.Down);

                            // Rotate front and back faces
                            orientation[Side.Left] = Face.CopyAndRotate(orientation[Side.Left], rotation);
                            orientation[Side.Right] = Face.CopyAndRotate(orientation[Side.Right], rotation);
                            orientation[Side.Front] = Face.CopyAndRotate(orientation[Side.Front], rotation);
                            orientation[Side.Back] = Face.CopyAndRotate(orientation[Side.Back], rotation);
                            orientation[Side.Up] = Face.CopyAndRotate(orientation[Side.Up], rotation);
                            orientation[Side.Down] = Face.CopyAndRotate(orientation[Side.Down], rotation);
                            break;
                    }
                    break;
            }

            SetRelationships();
        }

        private void AssignFaceToSide(Face face, Side side)
        {
            orientation[side] = face;
            face.SetSide(side);
        }

        private Rotation GetOppositeRotation(Rotation rotation)
        {
            if (rotation == Rotation.Clockwise) return Rotation.Anticlockwise;
            else if (rotation == Rotation.Anticlockwise) return Rotation.Clockwise;
            else return rotation;
        }

        private Side GetOppositeSide(Side side)
        {
            Side oppositeSide = Side.Front;

            switch (side)
            {
                case Side.Front:
                    oppositeSide = Side.Back;
                    break;
                case Side.Left:
                    oppositeSide = Side.Right;
                    break;
                case Side.Up:
                    oppositeSide = Side.Down;
                    break;
                case Side.Right:
                    oppositeSide = Side.Left;
                    break;
                case Side.Down:
                    oppositeSide = Side.Up;
                    break;
                case Side.Back:
                    oppositeSide = Side.Front;
                    break;
            }

            return oppositeSide;
        }

        /// <summary>
        /// Rotates target side 90 degrees clockwise (with respect to that side).
        /// </summary>
        /// <param name="side">Target side to rotate.</param>
        public void RotateClockwise(Side side)
        {
            orientation[side].RotateClockwise();
        }

        /// <summary>
        /// Rotates target side 90 degrees anti-clockwise (with respect to that side).
        /// </summary>
        /// <param name="side">Target side to rotate.</param>
        public void RotateAnticlockwise(Side side)
        {
            orientation[side].RotateAnticlockwise();
        }

        public void Render()
        {
            for (int y = 0; y < depth + height + depth + height; y++)
            {
                for (int x = 0; x < depth + width + depth; x++)
                {
                    if ((x < depth || x >= depth + width) &&
                        (y < depth || y >= depth + height))
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.Write(' ');
                    }
                    else
                    {
                        Side faceDir = Side.Right;

                        if (x < depth)
                        {
                            faceDir = Side.Left;
                        }
                        else if (x < width + depth)
                        {
                            if (y < depth)
                            {
                                faceDir = Side.Up;
                            }
                            else if (y >= depth && y < depth + height)
                            {
                                faceDir = Side.Front;
                            }
                            else if (y >= depth + height && y < depth + height + depth)
                            {
                                faceDir = Side.Down;
                            }
                            else
                            {
                                faceDir = Side.Back;
                            }
                        }

                        orientation[faceDir].Render(x % 3, y % 3);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}