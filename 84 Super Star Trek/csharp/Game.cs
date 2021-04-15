using System;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;
using SuperStarTrek.Systems;
using SuperStarTrek.Systems.ComputerFunctions;

namespace SuperStarTrek
{
    internal class Game
    {
        private readonly Output _output;
        private readonly Input _input;
        private readonly Random _random;

        private int _initialStardate;
        private int _finalStarDate;
        private float _currentStardate;
        private Coordinates _currentQuadrant;
        private Galaxy _galaxy;
        private int _initialKlingonCount;
        private Enterprise _enterprise;

        internal Game(Output output, Input input, Random random)
        {
            _output = output;
            _input = input;
            _random = random;
        }

        internal float Stardate => _currentStardate;
        internal float StardatesRemaining => _finalStarDate - _currentStardate;

        internal void DoIntroduction()
        {
            _output.Write(Strings.Title);

            if (_input.GetYesNo("Do you need instructions", Input.YesNoMode.FalseOnN))
            {
                _output.Write(Strings.Instructions);

                _input.WaitForAnyKeyButEnter("to continue");
            }
        }

        internal void Play()
        {
            Initialise();
            var gameOver = false;

            while (!gameOver)
            {
                _enterprise.Quadrant.Display(Strings.NowEntering);

                var command = _input.GetCommand();

                var result = _enterprise.Execute(command);

                gameOver = result.IsGameOver || CheckIfStranded();
                _currentStardate += result.TimeElapsed;
                gameOver |= _currentStardate > _finalStarDate;
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
            _currentStardate = _initialStardate = _random.GetInt(20, 40) * 100;
            _finalStarDate = _initialStardate + _random.GetInt(25, 35);

            _currentQuadrant = _random.GetCoordinate();

            _galaxy = new Galaxy(_random);
            _initialKlingonCount = _galaxy.KlingonCount;

            _enterprise = new Enterprise(3000, _random.GetCoordinate(), _output, _random, _input);
            _enterprise
                .Add(new WarpEngines(_enterprise, _output, _input))
                .Add(new ShortRangeSensors(_enterprise, _galaxy, this, _output))
                .Add(new LongRangeSensors(_galaxy, _output))
                .Add(new PhaserControl(_enterprise, _output, _input, _random))
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

            _enterprise.StartIn(BuildCurrentQuadrant());
        }

        private Quadrant BuildCurrentQuadrant() =>
           new Quadrant(_galaxy[_currentQuadrant], _enterprise, _random, _galaxy, _input, _output);

        internal bool Replay() => _galaxy.StarbaseCount > 0 && _input.GetString(Strings.ReplayPrompt, "Aye");

        private bool CheckIfStranded()
        {
            if (_enterprise.IsStranded) { _output.Write(Strings.Stranded); }
            return _enterprise.IsStranded;
        }

        private float GetEfficiency() =>
            1000 * (float)Math.Pow(_initialKlingonCount / (_currentStardate - _initialStardate), 2);
    }
}
