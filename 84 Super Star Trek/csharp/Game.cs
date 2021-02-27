using System;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;
using SuperStarTrek.Systems;
using static System.StringComparison;

namespace SuperStarTrek
{
    internal class Game
    {
        private readonly Output _output;
        private readonly Input _input;

        private int _initialStardate;
        private int _finalStarDate;
        private double _currentStardate;
        private Coordinates _currentQuadrant;
        private Coordinates _currentSector;
        private Galaxy _galaxy;
        private int _initialKlingonCount;
        private Enterprise _enterprise;

        public Game()
        {
            _output = new Output();
            _input = new Input(_output);
        }

        public double Stardate => _currentStardate;

        public void DoIntroduction()
        {
            _output.Write(Strings.Title);

            _output.Write("Do you need instructions (Y/N)? ");
            var response = Console.ReadLine();
            _output.WriteLine();

            if (!response.Equals("N", InvariantCultureIgnoreCase))
            {
                _output.Write(Strings.Instructions);

                _input.WaitForAnyKeyButEnter("to continue");
            }
        }

        public void Play()
        {
            var quadrant = Initialise();
            var gameOver = false;

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

            _enterprise.Enter(quadrant, Strings.StartText);

            while (!gameOver)
            {
                var command = _input.GetCommand();

                gameOver = command == Command.XXX || _enterprise.Execute(command);
            }
        }

        private Quadrant Initialise()
        {
            var random = new Random();

            _currentStardate = _initialStardate = random.GetInt(20, 40) * 100;
            _finalStarDate = _initialStardate + random.GetInt(25, 35);

            _currentQuadrant = random.GetCoordinate();
            _currentSector = random.GetCoordinate();

            _galaxy = new Galaxy();
            _initialKlingonCount = _galaxy.KlingonCount;

            _enterprise = new Enterprise(3000, random.GetCoordinate());
            _enterprise.Add(new ShortRangeSensors(_enterprise, _galaxy, this, _output))
                .Add(new ShieldControl(_enterprise, _output, _input));

            return new Quadrant(_galaxy[_currentQuadrant], _enterprise);
        }

        public bool Replay() => _galaxy.StarbaseCount > 0 && _input.GetString(Strings.ReplayPrompt, "Aye");
    }
}
