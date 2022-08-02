namespace Digits;

internal class Guesser
{
    private readonly IReadOnlyList<int> _weights = new List<int> { 0, 1, 3 }.AsReadOnly();
    private readonly int[][,] _matrices = new[] { new int[3, 3], new int[9, 3], new int[27, 3] };
    private readonly int[] _indices = new[] { 2, 8, 26 };
    private readonly IRandom _random;

    public Guesser(IRandom random)
    {
        _random = random;

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++) { _matrices[0][i, j] = 9; }
            for (int i = 0; i < 9; i++) { _matrices[1][i, j] = i == 4 * j ? 2 : 3; }
            for (int i = 0; i < 27; i++) { _matrices[2][i, j] = 1; }
        }
    }

    public int GuessNextDigit()
    {
        var currentSum = 0;
        var guess = 0;

        for (int j = 0; j < 3; j++)
        {
            var sum = Enumerable.Range(0, 3).Aggregate((s, i) => s + GetWeightedValue(i, j));
            if (sum > currentSum || _random.NextFloat() >= 0.5)
            {
                currentSum = sum;
                guess = j;
            }
        }

        return guess;
    }

    public void ObserveActualDigit(int digit)
    {
        for (int i = 0; i < 3; i++)
        {
            _matrices[i][_indices[i], digit]++;
        }
        _indices[2] = _indices[2] % 9 * 3 + digit;
        _indices[1] = _indices[2] % 9;
        _indices[0] = digit;
    }

    private int GetWeightedValue(int matrix, int row) => _weights[matrix] * _matrices[matrix][_indices[matrix], row];
}