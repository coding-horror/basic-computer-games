using Games.Common.Randomness;

namespace Target
{
    internal class FiringRange
    {
        private readonly IRandom _random;
        private Point _targetPosition;

        public FiringRange(IRandom random)
        {
            _random = random;
        }

        public Point NextTarget() =>  _targetPosition = _random.NextPosition();

        public Explosion Fire(Angle angleFromX, Angle angleFromZ, float distance)
        {
            var explosionPosition = new Point(angleFromX, angleFromZ, distance);
            var targetOffset = explosionPosition - _targetPosition;
            return new (explosionPosition, targetOffset);
        }
    }
}
