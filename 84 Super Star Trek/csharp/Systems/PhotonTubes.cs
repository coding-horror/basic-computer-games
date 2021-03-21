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
            if (!_input.TryGetCourse("Photon torpedo course", "Ensign Chekov", out var course))
            {
                return CommandResult.Ok;
            }

            TorpedoCount -= 1;

            var isHit = false;
            _output.WriteLine("Torpedo track:");
            foreach (var sector in course.GetSectorsFrom(_enterprise.SectorCoordinates))
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
