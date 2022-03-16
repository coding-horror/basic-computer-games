using System;
using Games.Common.IO;

namespace Target
{
    internal class Game
    {
        private readonly IReadWrite _io;
        private readonly FiringRange _firingRange;
        private int _shotCount;

        public Game(IReadWrite io, FiringRange firingRange)
        {
            _io = io;
            _firingRange = firingRange;
        }

        public void Play()
        {
            _shotCount = 0;
            var target = _firingRange.NextTarget();
            _io.WriteLine(target.GetBearing());
            _io.WriteLine($"Target sighted: approximate coordinates:  {target}");

            while (true)
            {
                _io.WriteLine($"     Estimated distance: {target.EstimateDistance()}");
                _io.WriteLine();

                var explosion = Shoot();

                if (explosion.IsTooClose)
                {
                    _io.WriteLine("You blew yourself up!!");
                    return;
                }

                _io.WriteLine(explosion.GetBearing());

                if (explosion.IsHit)
                {
                    ReportHit(explosion.DistanceToTarget);
                    return;
                }

                ReportMiss(explosion);
            }
        }

        private Explosion Shoot()
        {
            var (xDeviation, zDeviation, distance) = _io.Read3Numbers(
                "Input angle deviation from X, angle deviation from Z, distance");
            _shotCount++;
            _io.WriteLine();

            return _firingRange.Fire(Angle.InDegrees(xDeviation), Angle.InDegrees(zDeviation), distance);
        }

        private void ReportHit(float distance)
        {
            _io.WriteLine();
            _io.WriteLine($" * * * HIT * * *   Target is non-functional");
            _io.WriteLine();
            _io.WriteLine($"Distance of explosion from target was {distance} kilometers.");
            _io.WriteLine();
            _io.WriteLine($"Mission accomplished in {_shotCount} shots.");
        }

        private void ReportMiss(Explosion explosion)
        {
            ReportMiss(explosion.FromTarget);
            _io.WriteLine($"Approx position of explosion:  {explosion.Position}");
            _io.WriteLine($"     Distance from target = {explosion.DistanceToTarget}");
            _io.WriteLine();
            _io.WriteLine();
            _io.WriteLine();
        }

        private void ReportMiss(Offset targetOffset)
        {
            ReportMiss(targetOffset.DeltaX, "in front of", "behind");
            ReportMiss(targetOffset.DeltaY, "to left of", "to right of");
            ReportMiss(targetOffset.DeltaZ, "above", "below");
        }

        private void ReportMiss(float delta, string positiveText, string negativeText) =>
            _io.WriteLine(delta >= 0 ? GetOffsetText(positiveText, delta) : GetOffsetText(negativeText, -delta));

        private static string GetOffsetText(string text, float distance) => $"Shot {text} target {distance} kilometers.";
    }
}
