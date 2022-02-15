using System;

namespace Games.Common.IO;

/// <summary>
/// An implementation of <see cref="IReadWrite" /> with input begin read for STDIN and output being written to
/// STDOUT.
/// </summary>
public sealed class ConsoleIO : TextIO
{
    public ConsoleIO()
        : base(Console.In, Console.Out)
    {
    }
}
