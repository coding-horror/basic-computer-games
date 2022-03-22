using System;
using Games.Common.IO;
using Games.Common.Randomness;
using SuperStarTrek.Objects;
using SuperStarTrek.Resources;
using SuperStarTrek.Space;
using SuperStarTrek.Systems;
using SuperStarTrek.Systems.ComputerFunctions;

namespace SuperStarTrek;

internal class Game
{
    private readonly TextIO _io;
    private readonly IRandom _random;

    private int _initialStardate;
    private int _finalStarDate;
    private float _currentStardate;
    private Coordinates _currentQuadrant;
    private Galaxy _galaxy;
    private int _initialKlingonCount;
    private Enterprise _enterprise;

    internal Game(TextIO io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    internal float Stardate => _currentStardate;

    internal float StardatesRemaining => _finalStarDate - _currentStardate;

    internal void DoIntroduction()
    {
        _io.Write(Strings.Title);

        if (_io.GetYesNo("Do you need instructions", IReadWriteExtensions.YesNoMode.FalseOnN))
        {
            _io.Write(Strings.Instructions);

            _io.WaitForAnyKeyButEnter("to continue");
        }
    }

    internal void Play()
    {
        Initialise();
        var gameOver = false;

        while (!gameOver)
        {
            var command = _io.ReadCommand();

            var result = _enterprise.Execute(command);

            gameOver = result.IsGameOver || CheckIfStranded();
            _currentStardate += result.TimeElapsed;
            gameOver |= _currentStardate > _finalStarDate;
        }

        if (_galaxy.KlingonCount > 0)
        {
            _io.Write(Strings.EndOfMission, _currentStardate, _galaxy.KlingonCount);
        }
        else
        {
            _io.Write(Strings.Congratulations, CalculateEfficiency());
        }
    }

    private void Initialise()
    {
        _currentStardate = _initialStardate = _random.Next(20, 40) * 100;
        _finalStarDate = _initialStardate + _random.Next(25, 35);

        _currentQuadrant = _random.NextCoordinate();

        _galaxy = new Galaxy(_random);
        _initialKlingonCount = _galaxy.KlingonCount;

        _enterprise = new Enterprise(3000, _random.NextCoordinate(), _io, _random);
        _enterprise
            .Add(new WarpEngines(_enterprise, _io))
            .Add(new ShortRangeSensors(_enterprise, _galaxy, this, _io))
            .Add(new LongRangeSensors(_galaxy, _io))
            .Add(new PhaserControl(_enterprise, _io, _random))
            .Add(new PhotonTubes(10, _enterprise, _io))
            .Add(new ShieldControl(_enterprise, _io))
            .Add(new DamageControl(_enterprise, _io))
            .Add(new LibraryComputer(
                _io,
                new CumulativeGalacticRecord(_io, _galaxy),
                new StatusReport(this, _galaxy, _enterprise, _io),
                new TorpedoDataCalculator(_enterprise, _io),
                new StarbaseDataCalculator(_enterprise, _io),
                new DirectionDistanceCalculator(_enterprise, _io),
                new GalaxyRegionMap(_io, _galaxy)));

        _io.Write(Strings.Enterprise);
        _io.Write(
            Strings.Orders,
            _galaxy.KlingonCount,
            _finalStarDate,
            _finalStarDate - _initialStardate,
            _galaxy.StarbaseCount > 1 ? "are" : "is",
            _galaxy.StarbaseCount,
            _galaxy.StarbaseCount > 1 ? "s" : "");

        _io.WaitForAnyKeyButEnter("when ready to accept command");

        _enterprise.StartIn(BuildCurrentQuadrant());
    }

    private Quadrant BuildCurrentQuadrant() => new(_galaxy[_currentQuadrant], _enterprise, _random, _galaxy, _io);

    internal bool Replay() => _galaxy.StarbaseCount > 0 && _io.ReadExpectedString(Strings.ReplayPrompt, "Aye");

    private bool CheckIfStranded()
    {
        if (_enterprise.IsStranded) { _io.Write(Strings.Stranded); }
        return _enterprise.IsStranded;
    }

    private float CalculateEfficiency() =>
        1000 * (float)Math.Pow(_initialKlingonCount / (_currentStardate - _initialStardate), 2);
}
