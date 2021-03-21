using System.Linq;
using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal class PhaserControl : Subsystem
    {
        private readonly Enterprise _enterprise;
        private readonly Output _output;
        private readonly Input _input;
        private readonly Random _random;

        public PhaserControl(Enterprise enterprise, Output output, Input input, Random random)
            : base("Phaser Control", Command.PHA, output)
        {
            _enterprise = enterprise;
            _output = output;
            _input = input;
            _random = random;
        }

        protected override bool CanExecuteCommand() => IsOperational("Phasers inoperative");

        protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
        {
            if (!quadrant.HasKlingons)
            {
                _output.WriteLine(Strings.NoEnemyShips);
                return CommandResult.Ok;
            }

            if (_enterprise.Computer.IsDamaged)
            {
                _output.WriteLine("Computer failure hampers accuracy");
            }

            _output.Write($"Phasers locked on target;  ");

            var phaserStrength = GetPhaserStrength();
            if (phaserStrength < 0) { return CommandResult.Ok; }

            _enterprise.UseEnergy(phaserStrength);

            var perEnemyStrength = GetPerTargetPhaserStrength(phaserStrength, quadrant.KlingonCount);

            foreach (var klingon in quadrant.Klingons.ToList())
            {
                ResolveHitOn(klingon, perEnemyStrength, quadrant);
            }

            return quadrant.KlingonsFireOnEnterprise();
        }

        private float GetPhaserStrength()
        {
            while (true)
            {
                _output.WriteLine($"Energy available = {_enterprise.Energy} units");
                var phaserStrength = _input.GetNumber("Number of units to fire");

                if (phaserStrength <= _enterprise.Energy) { return phaserStrength; }
            }
        }

        private float GetPerTargetPhaserStrength(float phaserStrength, int targetCount)
        {
            if (_enterprise.Computer.IsDamaged)
            {
                phaserStrength *= _random.GetFloat();
            }

            return phaserStrength / targetCount;
        }

        private void ResolveHitOn(Klingon klingon, float perEnemyStrength, Quadrant quadrant)
        {
            var distance = _enterprise.SectorCoordinates.GetDistanceTo(klingon.Sector);
            var hitStrength = (int)(perEnemyStrength / distance * (2 + _random.GetFloat()));

            if (klingon.TakeHit(hitStrength))
            {
                _output.WriteLine($"{hitStrength} unit hit on Klingon at sector {klingon.Sector}");
                _output.WriteLine(
                    klingon.Energy <= 0
                        ? quadrant.Remove(klingon)
                        : $"   (sensors show {klingon.Energy} units remaining)");
            }
            else
            {
                _output.WriteLine($"Sensors show no damage to enemy at {klingon.Sector}");
            }
        }
    }
}
