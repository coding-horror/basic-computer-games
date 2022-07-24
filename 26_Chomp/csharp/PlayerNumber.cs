namespace Chomp;

internal class PlayerNumber
{
    private readonly float _playerCount;
    private int _counter;
    private float _number;

    // The original code does not constrain playerCount to be an integer
    public PlayerNumber(float playerCount)
    {
        _playerCount = playerCount;
        _number = 0;
        Increment();
    }

    public static PlayerNumber operator ++(PlayerNumber number) => number.Increment();

    private PlayerNumber Increment()
    {
		if (_playerCount == 0) { throw new DivideByZeroException(); }

        // The increment logic here is the same as the original program, and exhibits
        // interesting behaviour when _playerCount is not an integer.
        _counter++;
        _number = _counter - (float)Math.Floor(_counter / _playerCount) * _playerCount;
        if (_number == 0) { _number = _playerCount; }
        return this;
    }

    public override string ToString() => (_number >= 0 ? " " : "") + _number.ToString();
}