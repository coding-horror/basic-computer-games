using SuperStarTrek.Commands;
using SuperStarTrek.Objects;
using SuperStarTrek.Space;

namespace SuperStarTrek.Systems
{
    internal class PhotonTubes : Subsystem
    {
        private readonly int _tubeCount;
        private readonly Enterprise _enterprise;
        private readonly Output _output;
        private readonly Input _input;

        public PhotonTubes(int tubeCount, Enterprise enterprise, Output output, Input input)
            : base("Photon Tubes", Command.TOR, output)
        {
            TorpedoCount = _tubeCount = tubeCount;
            _enterprise = enterprise;
            _output = output;
            _input = input;
        }

        public int TorpedoCount { get; private set; }

        protected override bool CanExecuteCommand() => HasTorpedoes() && IsOperational("{name} are not operational");

        private bool HasTorpedoes()
        {
            if (TorpedoCount > 0) { return true; }

            _output.WriteLine("All photon torpedoes expended");
            return false;
        }

        protected override CommandResult ExecuteCommandCore(Quadrant quadrant)
        {
            if (!_input.TryGetNumber("Photon torpedo course", 1, 9, out var direction))
            {
                _output.WriteLine("Ensign Chekov reports, 'Incorrect course data, sir!'");
                return CommandResult.Ok;
            }

            var isHit = false;
            _output.WriteLine("Torpedo track:");
            foreach (var sector in new Course(direction).GetSectorsFrom(_enterprise.Sector))
            {
                _output.WriteLine($"                {sector}");

                if (quadrant.TorpedoCollisionAt(sector, out var message, out var gameOver))
                {
                    _output.WriteLine(message);
                    isHit = true;
                    if (gameOver) { return CommandResult.GameOver; }
                    break;
                }
            }

            if (!isHit) { _output.WriteLine("Torpedo missed!"); }

            return quadrant.KlingonsFireOnEnterprise();
        }
    }
}
