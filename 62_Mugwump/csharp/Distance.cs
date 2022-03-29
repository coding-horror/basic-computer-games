namespace Mugwump;

internal struct Distance
{
    private readonly float _value;

    public Distance(float deltaX, float deltaY)
    {
        _value = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }

    public override string ToString() => _value.ToString("0.0");
}
