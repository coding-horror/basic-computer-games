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
    private readonly bool? _result;

    internal Probably(float defenseFactor, IRandom random, bool? result = false)
    {
        _defenseFactor = defenseFactor;
        _random = random;
        _result = result;
    }

    public Probably Do(float probability, Func<bool?> action) =>
        ShouldResolveAction(probability)
            ? new Probably(_defenseFactor, _random, _result | Resolve(action))
            : this;

    public Probably Do(float probability, Action action) =>
        ShouldResolveAction(probability)
            ? new Probably(_defenseFactor, _random, _result | Resolve(action))
            : this;

    public Probably Or(float probability, Action action) => Do(probability, action);

    public Probably Or(float probability, Func<bool?> action) => Do(probability, action);

    public bool Or(Action action) => _result | Resolve(action) ?? false;

    private bool? Resolve(Action action)
    {
        action.Invoke();
        return _result;
    }

    private bool? Resolve(Func<bool?> action) => action.Invoke();

    private readonly bool ShouldResolveAction(float probability)
    {
        return _result is null && _random.NextFloat() <= probability * _defenseFactor;
    }
}
