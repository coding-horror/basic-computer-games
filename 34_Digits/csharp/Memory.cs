namespace Digits;

public class Memory
{
    private readonly Matrix[] _matrices;

    public Memory()
    {
        _matrices = new[] 
        {
            new Matrix(27, 3, (_, _) => 1),
            new Matrix(9, 1, (i, j) => i == 4 * j ? 2 : 3),
            new Matrix(3, 0, (_, _) => 9)
        };
    }

    public int GetWeightedSum(int row) => _matrices.Select(m => m.GetWeightedValue(row)).Sum();

    public void ObserveDigit(int digit)
    {
        for (int i = 0; i < 3; i++)
        {
            _matrices[i].IncrementValue(digit);
        }

        _matrices[0].Index = _matrices[0].Index % 9 * 3 + digit;
        _matrices[1].Index = _matrices[0].Index % 9;
        _matrices[2].Index = digit;
    }
}