using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using static Hexapawn.Pawn;

namespace Hexapawn
{
    internal class Board : IEnumerable<Pawn>, IEquatable<Board>
    {
        private readonly Pawn[] _cells;

        public Board()
        {
            _cells = new[]
            {
                Black, Black, Black,
                None,  None,  None,
                White, White, White
            };
        }

        public Board(params Pawn[] cells)
        {
            _cells = cells;
        }

        public Pawn this[int index]
        {
            get => _cells[index - 1];
            set => _cells[index - 1] = value;
        }

        public Board Reflected => new(Cell.AllCells.Select(c => this[c.Reflected]).ToArray());

        public IEnumerator<Pawn> GetEnumerator() => _cells.OfType<Pawn>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public override string ToString()
        {
            var builder = new StringBuilder().AppendLine();
            for (int row = 0; row < 3; row++)
            {
                builder.Append("          ");
                for (int col = 0; col < 3; col++)
                {
                    builder.Append(_cells[row * 3 + col]);
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }

        public bool Equals(Board other) => other?.Zip(this).All(x => x.First == x.Second) ?? false;

        public override bool Equals(object obj) => Equals(obj as Board);

        public override int GetHashCode()
        {
            var hash = 19;

            for (int i = 0; i < 9; i++)
            {
                hash = hash * 53 + _cells[i].GetHashCode();
            }

            return hash;
        }
    }
}
