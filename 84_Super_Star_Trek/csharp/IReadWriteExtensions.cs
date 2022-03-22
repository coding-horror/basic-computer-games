using System;
using System.Linq;
using Games.Common.IO;
using SuperStarTrek.Commands;
using SuperStarTrek.Space;
using static System.StringComparison;

namespace SuperStarTrek;

internal static class IReadWriteExtensions
{
    internal static void WaitForAnyKeyButEnter(this IReadWrite io, string prompt)
    {
        io.Write($"Hit any key but Enter {prompt} ");
        while (io.ReadCharacter() == '\r');
    }

    internal static (float X, float Y) GetCoordinates(this IReadWrite io, string prompt) =>
        io.Read2Numbers($"{prompt} (X,Y)");

    internal static bool TryReadNumberInRange(
        this IReadWrite io,
        string prompt,
        float minValue,
        float maxValue,
        out float value)
    {
        value = io.ReadNumber($"{prompt} ({minValue}-{maxValue})");

        return value >= minValue && value <= maxValue;
    }

    internal static bool ReadExpectedString(this IReadWrite io, string prompt, string trueValue) =>
        io.ReadString(prompt).Equals(trueValue, InvariantCultureIgnoreCase);

    internal static Command ReadCommand(this IReadWrite io)
    {
        while(true)
        {
            var response = io.ReadString("Command");

            if (response.Length >= 3 &&
                Enum.TryParse(response.Substring(0, 3), ignoreCase: true, out Command parsedCommand))
            {
                return parsedCommand;
            }

            io.WriteLine("Enter one of the following:");
            foreach (var command in Enum.GetValues(typeof(Command)).OfType<Command>())
            {
                io.WriteLine($"  {command}  ({command.GetDescription()})");
            }
            io.WriteLine();
        }
    }

    internal static bool TryReadCourse(this IReadWrite io, string prompt, string officer, out Course course)
    {
        if (!io.TryReadNumberInRange(prompt, 1, 9, out var direction))
        {
            io.WriteLine($"{officer} reports, 'Incorrect course data, sir!'");
            course = default;
            return false;
        }

        course = new Course(direction);
        return true;
    }

    internal static bool GetYesNo(this IReadWrite io, string prompt, YesNoMode mode)
    {
        var response = io.ReadString($"{prompt} (Y/N)").ToUpperInvariant();

        return (mode, response) switch
        {
            (YesNoMode.FalseOnN, "N") => false,
            (YesNoMode.FalseOnN, _) => true,
            (YesNoMode.TrueOnY, "Y") => true,
            (YesNoMode.TrueOnY, _) => false,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, "Invalid value")
        };
    }

    internal enum YesNoMode
    {
        TrueOnY,
        FalseOnN
    }
}
