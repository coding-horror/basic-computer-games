using System;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal class WarpEngines : Subsystem
    {
        private readonly Enterprise _enterprise;
        private readonly Output _output;
        private readonly Input _input;

        public WarpEngines(Enterprise enterprise, Output output, Input input)
            : base("Warp Engines", Command.NAV, output)
        {
            _enterprise = enterprise;
            _output = output;
            _input = input;
        }

        protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
        {
            if (_input.TryGetCourse("Course", "   Lt. Sulu", out var course) &&
                TryGetWarpFactor(out var warpFactor) &&
                TryGetDistanceToMove(warpFactor, out var distanceToMove))
            {
                var result = quadrant.KlingonsMoveAndFire();
                if (result.IsGameOver) { return result; }

                _enterprise.RepairSystems(warpFactor);
                _enterprise.VaryConditionOfRandomSystem();
                var timeElapsed = _enterprise.Move(course, warpFactor, distanceToMove);

                if (_enterprise.IsDocked)
                {
                    _enterprise.ShieldControl.DropShields();
                    _enterprise.Refuel();
                    _enterprise.PhotonTubes.ReplenishTorpedoes();
                }

                return CommandResult.Elapsed(timeElapsed);
            }

            return CommandResult.Ok;
        }

        private bool TryGetWarpFactor(out float warpFactor)
        {
            var maximumWarp = IsDamaged ? 0.2f : 8;
            if (_input.TryGetNumber("Warp Factor", 0, maximumWarp, out warpFactor))
            {
                return warpFactor > 0;
            }

            _output.WriteLine(
                IsDamaged && warpFactor > maximumWarp
                    ? "Warp engines are damaged.  Maximum speed = warp 0.2"
                    : $"  Chief Engineer Scott reports, 'The engines won't take warp {warpFactor} !'");

            return false;
        }

        private bool TryGetDistanceToMove(float warpFactor, out int distanceToTravel)
        {
            distanceToTravel = (int)Math.Round(warpFactor * 8, MidpointRounding.AwayFromZero);
            if (distanceToTravel <= _enterprise.Energy) { return true; }

            _output.WriteLine("Engineering reports, 'Insufficient energy available")
                .WriteLine($"                      for maneuvering at warp {warpFactor} !'");

            if (distanceToTravel <= _enterprise.TotalEnergy && !_enterprise.ShieldControl.IsDamaged)
            {
                _output.Write($"Deflector control room acknowledges {_enterprise.ShieldControl.ShieldEnergy} ")
                    .WriteLine("units of energy")
                    .WriteLine("                         presently deployed to shields.");
            }

            return false;
        }
    }
}
