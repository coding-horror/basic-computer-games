@safe: // Make @safe the default for this file, enforcing memory-safety.
import std.stdio, std.string, std.math, std.range, std.conv, std.algorithm;

void main()
{
    enum width = 80;
    writeln(center("3D Plot", width));
    writeln(center("(After Creative Computing  Morristown, New Jersey)\n\n\n", width));

    static float fna(float z)
    {
        return 30.0 * exp(-z * z / 100.0);
    }

    char[] row;

    for (float x = -30.0; x <= 30.0; x += 1.5)
    {
        size_t max_z = 0L;
        auto y1 = 5 * floor((sqrt(900 - x * x)) / 5.0);
        for (float y = y1; y >= -y1; y -= 5)
        {
            auto z = to!size_t(max(0, floor(25 + fna(sqrt(x * x + y * y)) - .7 * y)));
            if (z > max_z) // Visible
            {
                max_z = z;
                if (z + 1 > row.length) // row needs to grow
                    row ~= ' '.repeat(z + 1 - row.length).array;
                row[z] = '*';
            }
        }
        writeln(row);
        row = null;
    }
}
