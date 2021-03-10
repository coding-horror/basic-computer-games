using SuperStarTrek.Commands;
using SuperStarTrek.Space;

namespace SuperStarTrek.Objects
{
    internal class Klingon
    {
        private float _energy;
        private readonly Random _random;

        public Klingon(Coordinates sector, Random random)
        {
            Sector = sector;
            _random = random;
            _energy = _random.GetFloat(100, 300);
        }

        public Coordinates Sector { get; private set; }

        public override string ToString() => "+K+";

        public CommandResult FireOn(Enterprise enterprise)
        {
            var attackStrength = _random.GetFloat();
            var (_, distanceToEnterprise) = Sector.GetDirectionAndDistanceTo(enterprise.Sector);
            var hitStrength = (int)(_energy * (2 + attackStrength) / distanceToEnterprise);
            _energy /= 3 + attackStrength;

            return enterprise.TakeHit(Sector, hitStrength);
        }
    }
}
