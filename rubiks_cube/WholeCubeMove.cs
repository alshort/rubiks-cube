namespace rubiks_cube
{
    public class WholeCubeMove : Move
    {
        public Axis Axis { get; private set; }

        public WholeCubeMove(Axis axis, Rotation rotation) : base(rotation)
        {
            Axis = axis;
        }
    }
}
