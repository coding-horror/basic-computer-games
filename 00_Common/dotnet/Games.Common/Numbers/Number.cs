namespace Games.Common.Numbers;

/// <summary>
/// A single-precision floating-point number with string formatting equivalent to the BASIC interpreter.
/// </summary>
public struct Number
{
    private readonly float _value;

    public Number (float value)
    {
        _value = value;
    }

    public static implicit operator float(Number value) => value._value;

    public static implicit operator Number(float value) => new Number(value);

    public override string ToString() => _value < 0 ? $"{_value} " : $" {_value} ";
}
