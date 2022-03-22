using System;
using Games.Common.IO;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal class WarpEngines : Subsystem
    {
        private readonly Enterprise _enterprise;
        private readonly IReadWrite _io;

        internal WarpEngines(Enterprise enterprise, IReadWrite io)
            : base("Warp Engines", Command.NAV, io)
        {
            _enterprise = enterprise;
            _io = io;
        }

        protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
        {
            if (_io.TryReadCourse("Course", "   Lt. Sulu", out var course) &&
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

                _enterprise.Quadrant.Display(Strings.NowEntering);

                return CommandResult.Elapsed(timeElapsed);
            }

            return CommandResult.Ok;
        }

        private bool TryGetWarpFactor(out float warpFactor)
        {
            var maximumWarp = IsDamaged ? 0.2f : 8;
            if (_io.TryReadNumberInRange("Warp Factor", 0, maximumWarp, out warpFactor))
            {
                return warpFactor > 0;
            }

            _io.WriteLine(
                IsDamaged && warpFactor > maximumWarp
                    ? "Warp engines are damaged.  Maximum speed = warp 0.2"
                    : $"  Chief Engineer Scott reports, 'The engines won't take warp {warpFactor} !'");

            return false;
        }

        private bool TryGetDistanceToMove(float warpFactor, out int distanceToTravel)
        {
            distanceToTravel = (int)Math.Round(warpFactor * 8, MidpointRounding.AwayFromZero);
            if (distanceToTravel <= _enterprise.Energy) { return true; }

            _io.WriteLine("Engineering reports, 'Insufficient energy available");
            _io.WriteLine($"                      for maneuvering at warp {warpFactor} !'");

            if (distanceToTravel <= _enterprise.TotalEnergy && !_enterprise.ShieldControl.IsDamaged)
            {
                _io.Write($"Deflector control room acknowledges {_enterprise.ShieldControl.ShieldEnergy} ");
                _io.WriteLine("units of energy");
                _io.WriteLine("                         presently deployed to shields.");
            }

            return false;
        }
    }
}
