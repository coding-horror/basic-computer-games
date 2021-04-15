using SuperStarTrek.Objects;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems.ComputerFunctions
{
    internal class DirectionDistanceCalculator : NavigationCalculator
    {
        private readonly Enterprise _enterprise;
        private readonly Input _input;

        internal DirectionDistanceCalculator(Enterprise enterprise, Output output, Input input)
            : base("Starbase nav data", output)
        {
            _enterprise = enterprise;
            _input = input;
        }

        internal override void Execute(Quadrant quadrant)
        {
            Output.WriteLine("Direction/distance calculator:")
                .Write($"You are at quadrant {_enterprise.QuadrantCoordinates}")
                .WriteLine($" sector {_enterprise.SectorCoordinates}")
                .WriteLine("Please enter");

            WriteDirectionAndDistance(
                _input.GetCoordinates("  Initial coordinates"),
                _input.GetCoordinates("  Final coordinates"));
        }
    }
}