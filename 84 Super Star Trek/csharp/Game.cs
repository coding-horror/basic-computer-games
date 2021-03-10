using System;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;
using SuperStarTrek.Systems;
using SuperStarTrek.Systems.ComputerFunctions;
using static System.StringComparison;

namespace SuperStarTrek
{
    internal class Game
    {
        private readonly Output _output;
        private readonly Input _input;

        private int _initialStardate;
        private int _finalStarDate;
        private float _currentStardate;
        private Coordinates _currentQuadrant;
        private Galaxy _galaxy;
        private int _initialKlingonCount;
        private Enterprise _enterprise;

        public Game()
        {
            _output = new Output();
            _input = new Input(_output);
        }

        public float Stardate => _currentStardate;
        public float StardatesRemaining => _finalStarDate - _currentStardate;

        public void DoIntroduction()
        {
            _output.Write(Strings.Title);

            if (_input.GetYesNo("Do you need instructions", Input.YesNoMode.FalseOnN))
            {
                _output.Write(Strings.Instructions);

                _input.WaitForAnyKeyButEnter("to continue");
            }
        }

        public void Play()
        {
            Initialise();
            var gameOver = false;

            while (!gameOver)
            {
                var command = _input.GetCommand();

                var result = _enterprise.Execute(command);

                gameOver = result.IsGameOver || CheckIfStranded();
                _currentStardate += result.TimeElapsed;
            }

            if (_galaxy.KlingonCount > 0)
            {
                _output.Write(Strings.EndOfMission, _currentStardate, _galaxy.KlingonCount);
            }
            else
            {
                _output.Write(Strings.Congratulations, GetEfficiency());
            }
        }

        private void Initialise()
        {
            var random = new Random();

            _currentStardate = _initialStardate = random.GetInt(20, 40) * 100;
            _finalStarDate = _initialStardate + random.GetInt(25, 35);

            _currentQuadrant = random.GetCoordinate();

            _galaxy = new Galaxy();
            _initialKlingonCount = _galaxy.KlingonCount;

            _enterprise = new Enterprise(3000, random.GetCoordinate(), _output, random);
            _enterprise
                .Add(new ShortRangeSensors(_enterprise, _galaxy, this, _output))
                .Add(new LongRangeSensors(_galaxy, _output))
                .Add(new PhotonTubes(10, _enterprise, _output, _input))
                .Add(new ShieldControl(_enterprise, _output, _input))
                .Add(new DamageControl(_enterprise, _output))
                .Add(new LibraryComputer(
                    _output,
                    _input,
                    new CumulativeGalacticRecord(_output, _galaxy),
                    new StatusReport(this, _galaxy, _enterprise, _output),
                    new TorpedoDataCalculator(_enterprise, _output),
                    new StarbaseDataCalculator(_enterprise, _output),
                    new DirectionDistanceCalculator(_enterprise, _output, _input),
                    new GalaxyRegionMap(_output, _galaxy)));

            _output.Write(Strings.Enterprise);
            _output.Write(
                Strings.Orders,
                _galaxy.KlingonCount,
                _finalStarDate,
                _finalStarDate - _initialStardate,
                _galaxy.StarbaseCount > 1 ? "are" : "is",
                _galaxy.StarbaseCount,
                _galaxy.StarbaseCount > 1 ? "s" : "");

            _input.WaitForAnyKeyButEnter("when ready to accept command");

            var quadrant = _galaxy[_currentQuadrant].BuildQuadrant(_enterprise, random, _galaxy, _input, _output);
            _enterprise.Enter(quadrant, Strings.StartText);
        }

        public bool Replay() => _galaxy.StarbaseCount > 0 && _input.GetString(Strings.ReplayPrompt, "Aye");

        private bool CheckIfStranded()
        {
            if (_enterprise.IsStranded) { _output.Write(Strings.Stranded); }
            return _enterprise.IsStranded;
        }

        private float GetEfficiency() =>
            1000 * (float)Math.Pow(_initialKlingonCount / (_currentStardate - _initialStardate), 2);
    }
}
