using SuperStarTrek.Commands;
using SuperStarTrek.Space;

namespace SuperStarTrek.Objects
{
    internal class Klingon
    {
        private readonly Random _random;

        internal Klingon(Coordinates sector, Random random)
        {
            Sector = sector;
            _random = random;
            Energy = _random.GetFloat(100, 300);
        }

        internal float Energy { get; private set; }
        internal Coordinates Sector { get; private set; }

        public override string ToString() => "+K+";

        internal CommandResult FireOn(Enterprise enterprise)
        {
            var attackStrength = _random.GetFloat();
            var distanceToEnterprise = Sector.GetDistanceTo(enterprise.SectorCoordinates);
            var hitStrength = (int)(Energy * (2 + attackStrength) / distanceToEnterprise);
            Energy /= 3 + attackStrength;

            return enterprise.TakeHit(Sector, hitStrength);
        }

        internal bool TakeHit(int hitStrength)
        {
            if (hitStrength < 0.15 * Energy) { return false; }

            Energy -= hitStrength;
            return true;
        }

        internal void MoveTo(Coordinates newSector) => Sector = newSector;
    }
}
