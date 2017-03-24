using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rubiks_cube
{
    public class FaceRelationship
    {
        public Face Up { get; private set; }
        public Face Left { get; private set; }
        public Face Right { get; private set; }
        public Face Down { get; private set; }

        public FaceRelationship(Face up, Face left, Face right, Face down)
        {
            Up = up;
            Left = left;
            Right = right;
            Down = down;
        }

        public Side QueryRelation(Face caller)
        {
            if (caller.Equals(Up)) return Side.Up;
            else if (caller.Equals(Left)) return Side.Left;
            else if (caller.Equals(Right)) return Side.Right;
            else if (caller.Equals(Down)) return Side.Down;
            else
            {
                throw new Exception("Not related");
            }
        }

        public override bool Equals(object obj)
        {
            FaceRelationship relationship = obj as FaceRelationship;
            if (relationship == null) return false;

            return Up == relationship.Up &&
                Left == relationship.Left &&
                Right == relationship.Right &&
                Down == relationship.Down;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = 31 * hash + Up.GetHashCode();
            hash = 31 * hash + Left.GetHashCode();
            hash = 31 * hash + Down.GetHashCode();
            hash = 31 * hash + Right.GetHashCode();
            return hash;
        }
    }
}
