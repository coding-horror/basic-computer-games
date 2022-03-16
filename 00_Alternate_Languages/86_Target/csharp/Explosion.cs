namespace Target
{
    internal class Explosion
    {
        private readonly Point _position;

        public Explosion(Point position, Offset targetOffset)
        {
            _position = position;
            FromTarget = targetOffset;
            DistanceToTarget = targetOffset.Distance;
        }

        public Point Position => _position;
        public Offset FromTarget { get; }
        public float DistanceToTarget { get; }
        public string GetBearing() => _position.GetBearing();

        public bool IsHit => DistanceToTarget <= 20;
        public bool IsTooClose => _position.Distance < 20;
    }
}
