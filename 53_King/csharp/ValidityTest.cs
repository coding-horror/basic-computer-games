namespace King;

internal class ValidityTest
{
    private readonly Predicate<float> _isValid;
    private readonly Func<string> _getError;

    public ValidityTest(Predicate<float> isValid, string error)
        : this(isValid, () => error)
    {
    }

    public ValidityTest(Predicate<float> isValid, Func<string> getError)
    {
        _isValid = isValid;
        _getError = getError;
    }

    public bool IsValid(float value, IReadWrite io)
    {
        if (_isValid(value)) { return true; }
        
        io.Write(_getError());
        return false;
    }
}