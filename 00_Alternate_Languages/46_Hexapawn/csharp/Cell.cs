using System;
using System.Collections.Generic;

namespace Hexapawn
{
    // Represents a cell on the board, numbered 1 to 9, with support for finding the reflection of the reference around
    // the middle column of the board.
    internal class Cell
    {
        private static readonly Cell[] _cells = new Cell[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private static readonly Cell[] _reflected = new Cell[] { 3, 2, 1, 6, 5, 4, 9, 8, 7 };

        private readonly int _number;

        private Cell(int number)
        {
            if (number < 1 || number > 9)
            {
                throw new ArgumentOutOfRangeException(nameof(number), number, "Must be from 1 to 9");
            }

            _number = number;
        }

        // Facilitates enumerating all the cells.
        public static IEnumerable<Cell> AllCells => _cells;

        // Takes a value input by the user and attempts to create a Cell reference
        public static bool TryCreate(float input, out Cell cell)
        {
            if (IsInteger(input) && input >= 1 && input <= 9)
            {
                cell = (int)input;
                return true;
            }

            cell = default;
            return false;

            static bool IsInteger(float value) => value - (int)value == 0;
        }

        // Returns the reflection of the cell reference about the middle column of the board.
        public Cell Reflected => _reflected[_number - 1];

        // Allows the cell reference to be used where an int is expected, such as the indexer in Board.
        public static implicit operator int(Cell c) => c._number;

        public static implicit operator Cell(int number) => new(number);

        public override string ToString() => _number.ToString();
    }
}
