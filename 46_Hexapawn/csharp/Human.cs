using System;
using System.Linq;
using static Hexapawn.Cell;
using static Hexapawn.Move;
using static Hexapawn.Pawn;

namespace Hexapawn
{
    internal class Human : IPlayer
    {
        public int Wins { get; private set; }

        public void Move(Board board)
        {
            while (true)
            {
                var move = Input.GetMove("Your move");

                if (TryExecute(board, move)) { return; }

                Console.WriteLine("Illegal move.");
            }
        }

        public void AddWin() => Wins++;

        public bool HasLegalMove(Board board)
        {
            foreach (var from in AllCells.Where(c => c > 3))
            {
                if (board[from] != White) { continue; }

                if (HasLegalMove(board, from))
                {
                    return true;
                }
            }

            return false;
        }

        private bool HasLegalMove(Board board, Cell from) =>
            Right(from).IsRightDiagonalToCapture(board) ||
            Straight(from).IsStraightMoveToEmptySpace(board) ||
            from > 4 && Left(from).IsLeftDiagonalToCapture(board);

        public bool HasNoPawns(Board board) => board.All(c => c != White);

        public bool TryExecute(Board board, Move move)
        {
            if (board[move.From] != White) { return false; }

            if (move.IsStraightMoveToEmptySpace(board) ||
                move.IsLeftDiagonalToCapture(board) ||
                move.IsRightDiagonalToCapture(board))
            {
                move.Execute(board);
                return true;
            }

            return false;
        }
    }
}
