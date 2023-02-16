namespace Roulette;

internal class Croupier
{
    private const int _initialHouse = 100_000;
    private const int _initialPlayer = 1_000;

    private int _house = _initialHouse;
    private int _player = _initialPlayer;

    public string Totals => Strings.Totals(_house, _player);
    public bool PlayerIsBroke => _player <= 0;
    public bool HouseIsBroke => _house <= 0;

    internal string Pay(Bet bet)
    {
        _house -= bet.Payout;
        _player += bet.Payout;

        if (_house <= 0)
        {
            _player = _initialHouse + _initialPlayer;
        }

        return Strings.Win(bet);
    }

    internal string Take(Bet bet)
    {
        _house += bet.Wager;
        _player -= bet.Wager;

        return Strings.Lose(bet);
    }

    public void CutCheck(IReadWrite io, IRandom random)
    {
        var name = io.ReadString(Prompts.Check);
        io.Write(Strings.Check(random, name, _player));
    }
}
