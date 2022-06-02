using Poker.Resources;
using static System.StringComparison;

namespace Poker;

internal class Game
{
    private readonly IReadWrite _io;
    private readonly IRandom _random;

    private Hand _playerHand;
    private Hand _computerHand;

    private bool _hasWatch;
    private int _computerBalance;
    private int _playerBalance;
    private int _pot;

    private int _playerBet;
    private int _playerTotalBet;
    private int I;
    private int Z;
    private int _keepMask;
    private int _computerTotalBet;
    private int V;
    private bool _playerFolds;
    private bool _computerFolds;

    public Game(IReadWrite io, IRandom random)
    {
        _io = io;
        _random = random;
    }

    private int Get0To9() => _random.Next(10);

    internal void Play()
    {
        var deck = new Deck();

        _io.Write(Resource.Streams.Title);
        _io.Write(Resource.Streams.Instructions);

        _hasWatch = true;
        _computerBalance = 200;
        _playerBalance = 200;

        do
        {
            deck.Shuffle(_random);
        } while (PlayHand(deck));
    }

    internal bool PlayHand(Deck deck)
    {
        _playerFolds = _computerFolds = false;
        _pot=0;
        while(true)
        {
            _io.WriteLine();
            if (_computerBalance<=5)
            {
                CongratulatePlayer();
                return false;
            }
            _io.WriteLine("The ante is $5.  I will deal:");
            _io.WriteLine();
            if (_playerBalance <= 5 && PlayerCantRaiseFunds()) { return false; }
            _pot += 10;

            _playerBalance -= 5;
            _computerBalance -= 5;
            _playerHand = deck.DealHand();
            _computerHand = deck.DealHand();

            _io.WriteLine("Your hand:");
            _io.Write(_playerHand);
            (_keepMask, I) = _computerHand.Analyze(2);
            _io.WriteLine();
_330:       if (I!=6) { goto _470; }
_340:       if (Get0To9()<=7) { goto _370; }
_350:       _keepMask = 0b11100;
_360:       goto _420;
_370:       if (Get0To9()<=7) { goto _400; }
_380:       _keepMask = 0b11110;
_390:       goto _420;
_400:       if (Get0To9()>=1) { goto _450; }
_410:       _keepMask = 0b11111;
_420:       I=7;
_430:       Z=23;
_440:       goto _580;
_450:       Z=1;
_460:       goto _510;
_470:       if (_computerHand.Rank >= 13) { goto _540; }
_480:       if (Get0To9()>=2) { goto _500; }
_490:       goto _420;
_500:       Z=0;
_510:       _computerTotalBet = 0;
_520:       _io.WriteLine("I check.");
_530:       goto _620;
_540:       Z = _computerHand.Rank <= 16 || Get0To9() < 1 ? 35 : 2;
_580:       V=Z+Get0To9();
_590:       if (ComputerCantContinue()) { return false; }
_600:       _io.WriteLine($"I'll open with ${V}");
_610:       _computerTotalBet = V;
            _playerTotalBet = 0;
_620:       if (GetWager()) { return false; }
_630:       if (IsThereAWinner() is {} response) { return response; }

            _io.WriteLine();
            var playerDrawCount = _io.ReadNumber(
                "Now we draw -- How many cards do you want",
                3,
                "You can't draw more than three cards.");
            if (playerDrawCount != 0)
            {
                Z=10;
                _io.WriteLine("What are their numbers:");
                for (var i = 1; i <= playerDrawCount; i++)
                {
                    _playerHand = _playerHand.Replace((int)_io.ReadNumber(), deck.DealCard());
                }
                _io.WriteLine("Your new hand:");
                _io.Write(_playerHand);
            }
            var computerDrawCount = 0;
            for (var i = 1; i <= 5; i++)
            {
                if ((_keepMask & (1 << (i - 1))) == 0)
                {
                    _computerHand = _computerHand.Replace(i, deck.DealCard());
                    computerDrawCount++;
                }
            }
            _io.WriteLine();
            _io.Write($"I am taking {computerDrawCount} card");
            if (computerDrawCount != 1)
            {
                _io.WriteLine("s");
            }
            _io.WriteLine();
            V=I;
            (_, I) = _computerHand.Analyze(1);
            if (V == 7)
            {
                Z = 28;
            }
            else if (I == 6)
            {
                Z = 1;
            }
            else if (_computerHand.Rank < 13)
            {
                Z = Get0To9() == 6 ? 19 : 2;
            }
            else
            {
                if (_computerHand.Rank >= 16)
                {
                    Z = 2;
                }
                else
                {
                    Z = Get0To9() == 8 ? 11 : 19;
                }
            }
_1330:      _computerTotalBet = 0;
            _playerTotalBet = 0;
_1340:      if (GetWager()) { return false; }
_1350:      if (_playerBet != 0) { goto _1450; }
_1360:      if (V==7) { goto _1400; }
_1370:      if (I!=6) { goto _1400; }
_1380:      _io.WriteLine("I'll check");
_1390:      goto _1460;
_1400:      V=Z+Get0To9();
_1410:      if (ComputerCantContinue()) { return false; }
_1420:      _io.WriteLine($"I'll bet ${V}");
_1430:      _computerTotalBet = V;
_1440:      if (GetWager()) { return false; }
_1450:      if (IsThereAWinner() is {} response2) { return response2; }
_1460:      _io.WriteLine();
            _io.WriteLine("Now we compare hands:");
            _io.WriteLine("My hand:");
            _io.Write(_computerHand);
            _playerHand.Analyze(0);
            _io.WriteLine();
            _io.Write($"You have {_playerHand.Name}");
            _io.Write($"and I have {_computerHand.Name}");
            if (_computerHand > _playerHand) { return ComputerWins(); }
            if (_playerHand > _computerHand) { return PlayerWins(); }
            _io.WriteLine("The hand is drawn.");
            _io.WriteLine($"All $ {_pot} remains in the pot.");
        }

        bool? IsThereAWinner()
        {
            if (_playerFolds)
            {
                _io.WriteLine();
                return ComputerWins();
            }
            else if (_computerFolds)
            {
                _io.WriteLine();
                return PlayerWins();
            }
            else
            {
                return null;
            }
        }

        bool ComputerWins()
        {
            _io.WriteLine("I win.");
            _computerBalance += _pot;
            return ShouldContinue();
        }

        bool ShouldContinue()
        {
            _io.WriteLine($"Now I have ${_computerBalance}and you have ${_playerBalance}");
            return _io.ReadYesNo("Do you wish to continue");
        }

        bool PlayerWins()
        {
            _io.WriteLine("You win.");
            _playerBalance += _pot;
            return ShouldContinue();
        }

        bool GetWager()
        {
            while(true)
            {
                if (_io.ReadPlayerAction(_computerTotalBet == 0 && _playerTotalBet == 0) is Bet bet)
                {
                    if (_playerTotalBet + bet.Amount < _computerTotalBet)
                    {
                        _io.WriteLine("If you can't see my bet, then fold.");
                        continue;
                    }
                    if (_playerBalance - _playerTotalBet - bet.Amount >= 0)
                    {
                        _playerBet = bet.Amount;
                        _playerTotalBet += bet.Amount;
                        break;
                    }
                    if (PlayerCantRaiseFunds()) { return true; }
                    continue;
                }
                else
                {
                    _playerFolds = true;
                    return UpdatePot();
                }
            }
            if (_playerTotalBet == _computerTotalBet) { return UpdatePot(); }
            if (Z == 1)
            {
                if (_playerTotalBet > 5)
                {
                    _computerFolds = true;
                    _io.WriteLine("I fold.");
                    return false;
                }
                V = 5;
            }
            return Line_3420();
        }

        bool Line_3350()
        {
            if (Z==2) { return Line_3430(); }
            return Line_3360();
        }

        bool Line_3360()
        {
            _io.WriteLine("I'll see you.");
            _computerTotalBet = _playerTotalBet;
            return UpdatePot();
        }

        bool UpdatePot()
        {
            _playerBalance -= _playerTotalBet;
            _computerBalance -= _computerTotalBet;
            _pot += _playerTotalBet + _computerTotalBet;
            return false;
        }

        bool Line_3420()
        {
            if (_playerTotalBet>3*Z) { return Line_3350(); }
            return Line_3430();
        }

        bool Line_3430()
        {
            V = _playerTotalBet - _computerTotalBet + Get0To9();
            if (ComputerCantContinue()) { return true; }
            _io.WriteLine($"I'll see you, and raise you{V}");
            _computerTotalBet = _playerTotalBet + V;
            return GetWager();
        }

        bool ComputerCantContinue()
        {
            if (_computerBalance - _playerTotalBet - V >= 0) { return false; }
            if (_playerTotalBet == 0)
            {
                V = _computerBalance;
            }
            else if (_computerBalance - _playerTotalBet >= 0)
            {
                return Line_3360();
            }
            else if (!_hasWatch)
            {
                var response = _io.ReadString("Would you like to buy back your watch for $50");
                if (!response.StartsWith("N", InvariantCultureIgnoreCase))
                {
                    // The original code does not deduct $50 from the player
                    _computerBalance += 50;
                    _hasWatch = true;
                    return false;
                }
            }
            return CongratulatePlayer();
        }

        bool CongratulatePlayer()
        {
            _io.WriteLine("I'm busted.  Congratulations!");
            return true;
        }

        bool PlayerCantRaiseFunds()
        {
            _io.WriteLine();
            _io.WriteLine("You can't bet with what you haven't got.");

            if (_hasWatch)
            {
                var response = _io.ReadString("Would you like to sell your watch");
                if (!response.StartsWith("N", InvariantCultureIgnoreCase))
                {
                    if (Get0To9() < 7)
                    {
                        _io.WriteLine("I'll give you $75 for it.");
                        _playerBalance += 75;
                    }
                    else
                    {
                        _io.WriteLine("That's a pretty crummy watch - I'll give you $25.");
                        _playerBalance += 25;
                    }
                    _hasWatch = false;
                    return false;
                }
            }

            // The original program had some code about selling a tie tack, but due to a fault
            // in the logic the code was unreachable. I've omitted it in this port.

            _io.WriteLine("Your wad is shot.  So long, sucker!");
            return true;
        }
    }
}

internal interface IAction { }
internal record Fold : IAction;
internal record Bet (int Amount) : IAction
{
    public Bet(float amount)
        : this((int)amount)
    {
    }
}
