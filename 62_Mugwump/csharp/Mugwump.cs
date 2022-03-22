namespace Mugwump;

internal class Mugwump
{
    private readonly int _id;
    private readonly Position _position;

    public Mugwump(int id, Position position)
    {
        _id = id;
        _position = position;
    }

    public (bool, Distance) FindFrom(Position guess) => (guess == _position, guess - _position);

    public string Reveal() => $"{this} is at {_position}";

    public override string ToString() => $"Mugwump {_id}";
}
