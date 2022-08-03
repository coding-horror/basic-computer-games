namespace Digits;

internal class Guesser
{
    private readonly Memory _matrices = new();
    private readonly IRandom _random;

    public Guesser(IRandom random)
    {
        _random = random;
    }

    public int GuessNextDigit()
    {
        var currentSum = 0;
        var guess = 0;

        for (int i = 0; i < 3; i++)
        {
            var sum = _matrices.GetWeightedSum(i);
            if (sum > currentSum || _random.NextFloat() >= 0.5)
            {
                currentSum = sum;
                guess = i;
            }
        }

        return guess;
    }

    public void ObserveActualDigit(int digit) => _matrices.ObserveDigit(digit);
}
