using System;

namespace Target
{
    internal class Game
    {
        private readonly FiringRange _firingRange;
        private int _shotCount;

        private Game(FiringRange firingRange)
        {
            _firingRange = firingRange;
        }

        public static void Play(FiringRange firingRange) => new Game(firingRange).Play();

        private void Play()
        {
            var target = _firingRange.TargetPosition;
            Console.WriteLine(target.GetBearing());
            Console.WriteLine($"Target sighted: approximate coordinates:  {target}");

            while (true)
            {
                Console.WriteLine($"     Estimated distance: {target.EstimateDistance()}");
                Console.WriteLine();

                var explosion = Shoot();

                if (explosion.IsTooClose)
                {
                    Console.WriteLine("You blew yourself up!!");
                    return;
                }

                Console.WriteLine(explosion.GetBearing());

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
            var input = Input.ReadNumbers("Input angle deviation from X, angle deviation from Z, distance", 3);
            _shotCount++;
            Console.WriteLine();

            return _firingRange.Fire(Angle.InDegrees(input[0]), Angle.InDegrees(input[1]), input[2]);
        }

        private void ReportHit(float distance)
        {
            Console.WriteLine();
            Console.WriteLine($" * * * HIT * * *   Target is non-functional");
            Console.WriteLine();
            Console.WriteLine($"Distance of explosion from target was {distance} kilometers.");
            Console.WriteLine();
            Console.WriteLine($"Mission accomplished in {_shotCount} shots.");
        }

        private void ReportMiss(Explosion explosion)
        {
            ReportMiss(explosion.FromTarget);
            Console.WriteLine($"Approx position of explosion:  {explosion.Position}");
            Console.WriteLine($"     Distance from target = {explosion.DistanceToTarget}");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

        private void ReportMiss(Offset targetOffset)
        {
            ReportMiss(targetOffset.DeltaX, "in front of", "behind");
            ReportMiss(targetOffset.DeltaY, "to left of", "to right of");
            ReportMiss(targetOffset.DeltaZ, "above", "below");
        }

        private void ReportMiss(float delta, string positiveText, string negativeText) =>
            Console.WriteLine(delta >= 0 ? GetOffsetText(positiveText, delta) : GetOffsetText(negativeText, -delta));

        private static string GetOffsetText(string text, float distance) => $"Shot {text} target {distance} kilometers.";
    }
}
