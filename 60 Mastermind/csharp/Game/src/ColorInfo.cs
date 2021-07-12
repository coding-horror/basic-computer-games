using System;

namespace Game
{
    /// <summary>
    /// Stores information about a color.
    /// </summary>
    public record ColorInfo
    {
        /// <summary>
        /// Gets a single character that represents the color.
        /// </summary>
        public char ShortName { get; init; }

        /// <summary>
        /// Gets the color's full name.
        /// </summary>
        public string LongName { get; init; } = String.Empty;
    }
}
