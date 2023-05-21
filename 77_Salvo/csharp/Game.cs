using Salvo.Targetting;

namespace Salvo;

internal class Game 
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    internal void Play()
    {
        _io.Write(Streams.Title);

        var damagedShips = new List<(int Turn, Ship Ship)>();
        var temp = new Position[13];
        
        var computerFleet = new Fleet(_random);
        var humanFleet = new Fleet(_io);
        var startResponse = _io.ReadString(Prompts.Start);
        while (startResponse == Strings.WhereAreYourShips)
        {
            foreach (var ship in computerFleet.Ships)
            {
                _io.WriteLine(ship);
            }
            startResponse = _io.ReadString(Prompts.Start);
        }
L1890:  var turnNumber=0;
        var humanShotSelector = new HumanShotSelector(humanFleet, _io);
        var computerShotSelector = new ComputerShotSelector(computerFleet, _random, _io);
L1920:  _io.WriteLine();
L1930:  if (startResponse != "YES") { goto L2620; }
L1950:  if (startResponse != "YES") { goto L1990; }
L1960:  turnNumber++;
L1980:  _io.Write(Strings.Turn(turnNumber));
L1990:  var numberOfShots = humanShotSelector.NumberOfShots;
L2220:  _io.Write(Strings.YouHaveShots(numberOfShots));
        if (numberOfShots == 0) { goto L2270; }
L2230:  if (humanShotSelector.CanTargetAllRemainingSquares) 
        { 
            _io.WriteLine(Streams.YouHaveMoreShotsThanSquares);
L2250:      goto L2890;
        }
        computerFleet.ReceiveShots(humanShotSelector.GetShots(turnNumber), ship => _io.Write(Strings.YouHit(ship.Name)));
L2620:  if (startResponse == "YES") { goto L2670; }
L2640:  turnNumber++;
L2660:  _io.Write(Strings.Turn(turnNumber));
L2670:  numberOfShots = computerShotSelector.NumberOfShots;
L2840:  _io.Write(Strings.IHaveShots(numberOfShots));
L2850:  if (!computerShotSelector.CanTargetAllRemainingSquares) { goto L2880; }
L2860:  _io.Write(Streams.IHaveMoreShotsThanSquares);
L2270:  _io.Write(Streams.IWon);
        return;
L2880:  if (numberOfShots > 0) { goto L2960; }
L2890:  _io.Write(Streams.YouWon);
L2900:  return;

L2960:  humanFleet.ReceiveShots(
            computerShotSelector.GetShots(turnNumber),
            ship =>
            { 
                _io.Write(Strings.IHit(ship.Name));
                computerShotSelector.RecordHit(ship, turnNumber);
            });
        goto L1950;
    }
}

internal class DataRandom : IRandom
{
    private readonly Queue<float> _values = 
        new(File.ReadAllLines("data.txt").Select(l => float.Parse(l) / 1000000));
    private float _previous;

    public float NextFloat() => _previous = _values.Dequeue();

    public float PreviousFloat() => _previous;

    public void Reseed(int seed)
    {
        throw new NotImplementedException();
    }
}

