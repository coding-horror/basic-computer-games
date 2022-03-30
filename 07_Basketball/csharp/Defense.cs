namespace Basketball;

internal class Defense
{
    private float _value;

    public Defense(float value) => Set(value);

    public void Set(float value) => _value = value;

    public static implicit operator float(Defense defense) => defense._value;
}
