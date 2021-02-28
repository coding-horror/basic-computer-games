namespace SuperStarTrek.Objects
{
    internal class Klingon
    {
        private double _energy;

        public Klingon()
        {
            _energy = new Random().GetDouble(100, 300);
        }

        public override string ToString() => "+K+";
    }
}
