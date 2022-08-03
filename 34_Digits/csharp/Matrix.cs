namespace Digits;

internal class Matrix
{
    private readonly int _weight;
    private readonly int[,] _values;

    public Matrix(int width, int weight, Func<int, int, int> seedFactory)
    {
        _weight = weight;
        _values = new int[width, 3];
        
        for (int i = 0; i < width; i++)
        for (int j = 0; j < 3; j++)
        {
            _values[i, j] = seedFactory.Invoke(i, j);
        }

        Index = width - 1;
    }

    public int Index { get; set; }

    public int GetWeightedValue(int row) => _weight * _values[Index, row];

    public int IncrementValue(int row) => _values[Index, row]++;
}