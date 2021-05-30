namespace Hexapawn
{
    // Represents the contents of a cell on the board
    internal class Pawn
    {
        public static readonly Pawn Black = new('X');
        public static readonly Pawn White = new('O');
        public static readonly Pawn None = new('.');

        private readonly char _symbol;

        private Pawn(char symbol)
        {
            _symbol = symbol;
        }

        public override string ToString() => _symbol.ToString();
    }
}
