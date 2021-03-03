
using System;
using System.Collections.Generic;
using System.Linq;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal class LongRangeSensors : Subsystem
    {
        private readonly Galaxy _galaxy;
        private readonly Output _output;

        public LongRangeSensors(Galaxy galaxy, Output output)
            : base("Long Range Sensors", Command.LRS)
        {
            _galaxy = galaxy;
            _output = output;
        }

        public override CommandResult ExecuteCommand(Quadrant quadrant)
        {
            if (Condition < 0)
            {
                _output.WriteLine("Long Range Sensors are inoperable");
            }
            else
            {
                _output.WriteLine($"Long range scan for quadrant {quadrant.Coordinates}");
                _output.WriteLine("-------------------");
                foreach (var quadrants in _galaxy.GetNeighborhood(quadrant))
                {
                    _output.WriteLine(": " + string.Join(" : ", quadrants.Select(q => q?.Scan() ?? "***")) + " :");
                    _output.WriteLine("-------------------");
                }
            }

            return CommandResult.Ok;
        }
    }
}
