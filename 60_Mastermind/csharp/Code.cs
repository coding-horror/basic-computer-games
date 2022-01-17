using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    /// <summary>
    /// Represents a secret code in the game.
    /// </summary>
    public class Code
    {
        private readonly int[] m_colors;

        /// <summary>
        /// Initializes a new instance of the Code class from the given set
        /// of positions.
        /// </summary>
        /// <param name="colors">
        /// Contains the color for each position.
        /// </param>
        public Code(IEnumerable<int> colors)
        {
            m_colors = colors.ToArray();
            if (m_colors.Length == 0)
                throw new ArgumentException("A code must contain at least one position");
        }

        /// <summary>
        /// Compares this code with the given code.
        /// </summary>
        /// <param name="other">
        /// The code to compare.
        /// </param>
        /// <returns>
        /// A number of black pegs and a number of white pegs.  The number
        /// of black pegs is the number of positions that contain the same
        /// color in both codes.  The number of white pegs is the number of
        /// colors that appear in both codes, but in the wrong positions.
        /// </returns>
        public (int blacks, int whites) Compare(Code other)
        {
            // What follows is the O(N^2) from the original BASIC program
            // (where N is the number of positions in the code).  Note that
            // there is an O(N) algorithm.  (Finding it is left as an
            // exercise for the reader.)
            if (other.m_colors.Length != m_colors.Length)
                throw new ArgumentException("Only codes of the same length can be compared");

            // Keeps track of which positions in the other code have already
            // been marked as exact or close matches.
            var consumed = new bool[m_colors.Length];

            var blacks = 0;
            var whites = 0;

            for (var i = 0; i < m_colors.Length; ++i)
            {
                if (m_colors[i] == other.m_colors[i])
                {
                    ++blacks;
                    consumed[i] = true;
                }
                else
                {
                    // Check if the current color appears elsewhere in the
                    // other code.  We must be careful not to consider
                    // positions that are also exact matches.
                    for (var j = 0; j < m_colors.Length; ++j)
                    {
                        if (!consumed[j] &&
                            m_colors[i] == other.m_colors[j] &&
                            m_colors[j] != other.m_colors[j])
                        {
                            ++whites;
                            consumed[j] = true;
                            break;
                        }
                    }
                }
            }

            return (blacks, whites);
        }

        /// <summary>
        /// Gets a string representation of the code.
        /// </summary>
        public override string ToString() =>
            new (m_colors.Select(index => Colors.List[index].ShortName).ToArray());
    }
}
