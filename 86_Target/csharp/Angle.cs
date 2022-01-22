namespace Target
{
    internal class Angle
    {
        // Use same precision for constants as original code
        private const float PI = 3.14159f;
        private const float DegreesPerRadian = 57.296f;

        private readonly float _radians;

        private Angle(float radians) => _radians = radians;

        public static Angle InDegrees(float degrees) => new (degrees / DegreesPerRadian);
        public static Angle InRotations(float rotations) => new (2 * PI * rotations);

        public static implicit operator float(Angle angle) => angle._radians;
    }
}
