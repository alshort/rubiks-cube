using System;
using System.Linq;

namespace rubiks_cube
{
    public class Face
    {
        private int i;

        private int[,] face;

        private Side side;
        private FaceRelationship relationship;

        public Face(int i)
        {
            this.i = i;

            face = new int[3, 3]
            { 
                { i, i, i }, 
                { i, i, i }, 
                { i, i, i }
            };
        }

        public void SetSide(Side side)
        {
            this.side = side;
        }

        public int[] GetRow(int index)
        {
            int[] row = new int[face.GetLength(1)];
            for (int i = 0; i < face.GetLength(1); i++)
            {
                row[i] = face[index, i];
            }
            return row;
        }

        public int[] GetColumn(int index)
        {
            int[] col = new int[face.GetLength(0)];
            for (int i = 0; i < face.GetLength(0); i++)
            {
                col[i] = face[i, index];
            }
            return col;
        }

        public void SetRow(int index, int[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                face[index, i] = data[i];
            }
        }

        public void SetColumn(int index, int[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                face[i, index] = data[i];
            }
        }

        public void SetFaceRelationship(FaceRelationship relationship)
        {
            this.relationship = relationship;
        }


        public void RotateAnticlockwise()
        {
            RotateClockwise();
            RotateClockwise();
            RotateClockwise();
        }

        public void RotateClockwise()
        {
            int temp = 0;

            // Twist corners
            temp = face[0, 0]; // Store upper-left corner
            face[0, 0] = face[2, 0];
            face[2, 0] = face[2, 2];
            face[2, 2] = face[0, 2];
            face[0, 2] = temp;

            // Twist edges
            temp = face[0, 1]; // Store upper-center edge
            face[0, 1] = face[1, 0];
            face[1, 0] = face[2, 1];
            face[2, 1] = face[1, 2];
            face[1, 2] = temp;

            // Rotate pieces on corresponding neighbours
            int[] tempArr;


            // Where am I in relation to other faces?
            Side dir = relationship.Up.relationship.QueryRelation(this);

            switch (dir)
            {
                case Side.Left:
                    tempArr = relationship.Up.GetColumn(0);
                    relationship.Up.SetColumn(0, relationship.Left.GetColumn(0));
                    relationship.Left.SetColumn(0, relationship.Down.GetColumn(0));
                    relationship.Down.SetColumn(0, relationship.Right.GetColumn(0));
                    relationship.Right.SetColumn(0, tempArr);
                    break;

                case Side.Right:
                    tempArr = relationship.Up.GetColumn(2);
                    relationship.Up.SetColumn(2, relationship.Left.GetColumn(2));
                    relationship.Left.SetColumn(2, relationship.Down.GetColumn(2));
                    relationship.Down.SetColumn(2, relationship.Right.GetColumn(2));
                    relationship.Right.SetColumn(2, tempArr);
                    break;

                case Side.Down:
                    // Cases for top and down to come...
                    if (side == Side.Front)
                    {
                        tempArr = relationship.Up.GetRow(2);
                        relationship.Up.SetRow(2, relationship.Left.GetColumn(2).Reverse().ToArray());
                        relationship.Left.SetColumn(2, relationship.Down.GetRow(0));
                        relationship.Down.SetRow(0, relationship.Right.GetColumn(0).Reverse().ToArray());
                        relationship.Right.SetColumn(0, tempArr);
                    }
                    else if (side == Side.Down)
                    {
                        tempArr = relationship.Up.GetRow(2);
                        relationship.Up.SetRow(2, relationship.Left.GetRow(2));
                        relationship.Left.SetRow(2, relationship.Down.GetRow(0).Reverse().ToArray());
                        relationship.Down.SetRow(0, relationship.Right.GetRow(2).Reverse().ToArray());
                        relationship.Right.SetRow(2, tempArr);
                    }
                    else if (side == Side.Back)
                    {
                        tempArr = relationship.Up.GetRow(2).Reverse().ToArray();
                        relationship.Up.SetRow(2, relationship.Left.GetColumn(0));
                        relationship.Left.SetColumn(0, relationship.Down.GetRow(0).Reverse().ToArray());
                        relationship.Down.SetRow(0, relationship.Right.GetColumn(2));
                        relationship.Right.SetColumn(2, tempArr);
                    }
                    else
                    {
                        // Up
                        tempArr = relationship.Up.GetRow(2).Reverse().ToArray();
                        relationship.Up.SetRow(2, relationship.Left.GetRow(0).Reverse().ToArray());
                        relationship.Left.SetRow(0, relationship.Down.GetRow(0));
                        relationship.Down.SetRow(0, relationship.Right.GetRow(0));
                        relationship.Right.SetRow(0, tempArr);
                    }
                    break;
                default:
                    throw new Exception("Cannot get pieces");
            }
        }

        public void Render(int x, int y)
        {
            Console.BackgroundColor = CubeHelper.GetCubieColor(face[y, x]);
            Console.Write(' ');
        }

        public static Face CopyAndRotate(Face faceToCopy, Rotation rotation)
        {
            Face face = new Face(faceToCopy.i);
            face.SetFaceRelationship(faceToCopy.relationship);
            face.face = faceToCopy.face;
            face.side = faceToCopy.side;

            int temp = 0;
            switch (rotation)
            {
                case Rotation.Clockwise:
                    // Corners
                    temp = face.face[0, 0];
                    face.face[0, 0] = face.face[2, 0];
                    face.face[2, 0] = face.face[2, 2];
                    face.face[2, 2] = face.face[0, 2];
                    face.face[0, 2] = temp;

                    // Edges
                    temp = face.face[0, 1];
                    face.face[0, 1] = face.face[1, 0];
                    face.face[1, 0] = face.face[2, 1];
                    face.face[2, 1] = face.face[1, 2];
                    face.face[1, 2] = temp;
                    break;

                case Rotation.Anticlockwise:
                    // Corners
                    temp = face.face[0, 0];
                    face.face[0, 0] = face.face[0, 2];
                    face.face[0, 2] = face.face[2, 2];
                    face.face[2, 2] = face.face[2, 0];
                    face.face[2, 0] = temp;

                    // Edges
                    temp = face.face[0, 1];
                    face.face[0, 1] = face.face[1, 2];
                    face.face[1, 2] = face.face[2, 1];
                    face.face[2, 1] = face.face[1, 0];
                    face.face[1, 0] = temp;
                    break;

                case Rotation.HalfTurn:
                    // Corners
                    temp = face.face[0, 0];
                    face.face[0, 0] = face.face[2, 2];
                    face.face[2, 2] = temp;

                    temp = face.face[2, 0];
                    face.face[2, 0] = face.face[0, 2];
                    face.face[0, 2] = temp;

                    // Edges
                    temp = face.face[0, 1];
                    face.face[0, 1] = face.face[2, 1];
                    face.face[2, 1] = temp;

                    temp = face.face[1, 0];
                    face.face[1, 0] = face.face[1, 2];
                    face.face[1, 2] = temp;
                    break;
            }

            return face;
        }

        public override bool Equals(object obj)
        {
            Face otherFace = obj as Face;
            if (otherFace == null) return false;

            return i == otherFace.i && 
                side.Equals(otherFace.side) &&
                ArrayHelper.Are2DArraysEqual(face, otherFace.face) && 
                relationship.Equals(otherFace.relationship);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = 31 * hash + i.GetHashCode();
            hash = 31 * hash + side.GetHashCode();
            hash = 31 * hash + face.GetHashCode();
            return hash;
        }
    }
}
