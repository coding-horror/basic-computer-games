namespace Bounce;

/// <summary>
/// Represents the bounce of the ball, calculating duration, height and position in time.
/// </summary>
/// <remarks>
/// All calculations are derived from the equation for projectile motion: s = vt + 0.5at^2
/// </remarks>
internal class Bounce
{
    private const float _acceleration = -32; // feet/s^2

    private readonly float _velocity;

    internal Bounce(float velocity)
    {
        _velocity = velocity;
    }

    public float Duration => -2 * _velocity / _acceleration;

    public float MaxHeight =>
        (float)Math.Round(-_velocity * _velocity / 2 / _acceleration, MidpointRounding.AwayFromZero);

    public float Plot(Graph graph, float startTime)
    {
        var time = 0f;
        for (; time <= Duration; time += graph.TimeIncrement)
        {
            var height = _velocity * time + _acceleration * time * time / 2;
            graph.Plot(startTime + time, height);
        }

        return startTime + time;
    }

    public Bounce Next(float elasticity) => new Bounce(_velocity * elasticity);
}
