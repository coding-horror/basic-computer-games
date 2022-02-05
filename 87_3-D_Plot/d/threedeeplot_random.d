@safe: // Make @safe the default for this file, enforcing memory-safety.
import std.stdio, std.string, std.math, std.range, std.conv, std.random, std.algorithm;

void main()
{
    enum width = 80;
    writeln(center("3D Plot", width));
    writeln(center("(After Creative Computing  Morristown, New Jersey)\n\n", width));

    enum functions = ["30.0 * exp(-z * z / 100.0)",
                      "sqrt(900.01 - z * z) * .9 - 2",
                      "30 * (cos(z / 16.0) + .5)",
                      "30 - 30 * sin(z / 18.0)",
                      "30 * exp(-cos(z / 16.0)) - 30",
                      "30 * sin(z / 10.0)"];

    size_t index = uniform(0, functions.length);
    writeln(center("f(z) = " ~ functions[index], width), "\n");

    float fna(float z)
    {
        final switch (index)
        {
            static foreach (i, f; functions)
                case i:
                    mixin("return " ~ f ~ ";");
        }
    }

    char[] row;

    for (float x = -30.0; x <= 30.0; x += 1.5)
    {
        size_t max_z = 0L;
        auto y1 = 5 * lrint((sqrt(900 - x * x)) / 5.0);
        for (float y = y1; y >= -y1; y -= 5)
        {
            auto z = to!size_t(max(0, lrint(25 + fna(sqrt(x * x + y * y)) - .7 * y)));
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
