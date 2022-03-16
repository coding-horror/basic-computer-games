using SuperStarTrek.Space;
using SuperStarTrek.Utils;

namespace SuperStarTrek.Systems.ComputerFunctions
{
    internal abstract class NavigationCalculator : ComputerFunction
    {
        protected NavigationCalculator(string description, Output output)
            : base(description, output)
        {
        }

        protected void WriteDirectionAndDistance(Coordinates from, Coordinates to)
        {
            var (direction, distance) = from.GetDirectionAndDistanceTo(to);
            Write(direction, distance);
        }

        protected void WriteDirectionAndDistance((float X, float Y) from, (float X, float Y) to)
        {
            var (direction, distance) = DirectionAndDistance.From(from.X, from.Y).To(to.X, to.Y);
            Write(direction, distance);
        }

        private void Write(float direction, float distance) =>
            Output.WriteLine($"Direction = {direction}")
                .WriteLine($"Distance = {distance}");
    }
}
