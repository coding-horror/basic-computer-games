using SuperStarTrek.Commands;
using SuperStarTrek.Space;

namespace SuperStarTrek.Objects
{
    internal class Klingon
    {
        private double _energy;
        private Coordinates _sector;
        private readonly Random _random;

        public Klingon(Coordinates sector, Random random)
        {
            _sector = sector;
            _random = random;
            _energy = _random.GetDouble(100, 300);
        }

        public override string ToString() => "+K+";

        public CommandResult FireOn(Enterprise enterprise)
        {
            var attackStrength = _random.GetDouble();
            var hitStrength = (int)(_energy * (2 + attackStrength) / _sector.GetDistanceTo(enterprise.Sector));
            _energy /= 3 + attackStrength;

            return enterprise.TakeHit(_sector, hitStrength);
        }
    }
}
