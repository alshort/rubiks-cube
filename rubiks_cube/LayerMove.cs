namespace rubiks_cube
{
    public abstract class LayerMove : Move
    {
        public Side Side { get; private set; }

        public LayerMove(Side side, Rotation rotation) : base(rotation)
        {
            Side = side;
        }
    }
}
