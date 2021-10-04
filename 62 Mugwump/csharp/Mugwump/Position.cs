namespace Mugwump
{
    internal record Position(float X, float Y)
    {
        public override string ToString() => $"( {X} , {Y} )";

        public static Distance operator -(Position p1, Position p2) => new(p1.X - p2.X, p1.Y - p2.Y);
    }
}
