using System.Linq;
using SuperStarTrek.Commands;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal class LongRangeSensors : Subsystem
    {
        private readonly Galaxy _galaxy;
        private readonly Output _output;

        internal LongRangeSensors(Galaxy galaxy, Output output)
            : base("Long Range Sensors", Command.LRS, output)
        {
            _galaxy = galaxy;
            _output = output;
        }

        protected override bool CanExecuteCommand() => IsOperational("{name} are inoperable");

        protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
        {
            _output.WriteLine($"Long range scan for quadrant {quadrant.Coordinates}");
            _output.WriteLine("-------------------");
            foreach (var quadrants in _galaxy.GetNeighborhood(quadrant))
            {
                _output.WriteLine(": " + string.Join(" : ", quadrants.Select(q => q?.Scan() ?? "***")) + " :");
                _output.WriteLine("-------------------");
            }

            return CommandResult.Ok;
        }
    }
}
