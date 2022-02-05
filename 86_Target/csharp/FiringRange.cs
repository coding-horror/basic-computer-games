using System;

namespace Target
{
    internal class FiringRange
    {
        private readonly Random random;

        public FiringRange()
        {
            random = new Random();
            NextTarget();
        }

        public Point TargetPosition { get; private set; }

        public void NextTarget() =>  TargetPosition = random.NextPosition();

        public Explosion Fire(Angle angleFromX, Angle angleFromZ, float distance)
        {
            var explosionPosition = new Point(angleFromX, angleFromZ, distance);
            var targetOffset = explosionPosition - TargetPosition;
            return new (explosionPosition, targetOffset);
        }
    }
}
