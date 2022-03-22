using System;
using System.Linq;
using Games.Common.IO;
using static Hexapawn.Cell;
using static Hexapawn.Move;
using static Hexapawn.Pawn;

namespace Hexapawn;

internal class Human
{
    private readonly TextIO _io;

    public Human(TextIO io)
    {
        _io = io;
    }

    public void Move(Board board)
    {
        while (true)
        {
            var move = _io.ReadMove("Your move");

            if (TryExecute(board, move)) { return; }

            _io.WriteLine("Illegal move.");
        }
    }

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
