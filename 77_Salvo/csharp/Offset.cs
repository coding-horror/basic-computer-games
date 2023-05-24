namespace Salvo;

internal record struct Offset(int X, int Y)
{
    public static readonly Offset Zero = 0;

    public static Offset operator *(Offset offset, int scale) => new(offset.X * scale, offset.Y * scale);

    public static implicit operator Offset(int value) => new(value, value);

    public static IEnumerable<Offset> Units
    {
        get
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var offset = new Offset(x, y);
                    if (offset != Zero) { yield return offset; }
                }
            }
        }
    }
}
