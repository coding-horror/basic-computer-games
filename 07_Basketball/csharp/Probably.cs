using Games.Common.Randomness;

namespace Basketball;

/// <summary>
/// Supports a chain of actions to be performed based on various probabilities. The original game code gets a new
/// random number for each probability check. Evaluating a set of probabilities against a single random number is
/// much simpler, but yield a very different outcome distribution. The purpose of this class is to simplify the code
/// to for the original probabilistic branch decisions.
/// </summary>
internal struct Probably
{
    private readonly float _defenseFactor;
    private readonly IRandom _random;
    private readonly bool _done;

    internal Probably(float defenseFactor, IRandom random, bool done = false)
    {
        _defenseFactor = defenseFactor;
        _random = random;
        _done = done;
    }

    public Probably Do(float probability, Action action)
    {
        if (!_done && _random.NextFloat() <= probability * _defenseFactor)
        {
            action.Invoke();
            return new Probably(_defenseFactor, _random, true);
        }

        return this;
    }

    public Probably Or(float probability, Action action) => Do(probability, action);

    public Probably Or(Action action) => Do(1f, action);
}
