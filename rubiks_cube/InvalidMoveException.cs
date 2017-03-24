using System;

namespace rubiks_cube
{
    public class InvalidMoveException : Exception
    {
        public InvalidMoveException() { }

        public InvalidMoveException(string message) : base(message) { }
    }
}
