using System.Reflection;
using System.ComponentModel;

namespace SuperStarTrek
{
    internal static class CommandExtensions
    {
        internal static string GetDescription(this Command command) =>
            typeof(Command)
                .GetField(command.ToString())
                .GetCustomAttribute<DescriptionAttribute>()
                .Description;
    }
}
