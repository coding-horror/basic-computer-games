using System;
using System.Collections.Generic;
using System.Linq;
using static Hexapawn.Pawn;
using static Hexapawn.Cell;

namespace Hexapawn
{
    /// <summary>
    /// Encapsulates the logic of the computer player.
    /// </summary>
    internal class Computer : IPlayer
    {
        private readonly Random _random = new();
        private readonly Dictionary<Board, List<Move>> _potentialMoves;
        private (List<Move>, Move) _lastMove;

        public Computer()
        {
            // This dictionary implements the data in the original code, which encodes board positions for which the
            // computer has a legal move, and the list of possible moves for each position:
            //   900 DATA -1,-1,-1,1,0,0,0,1,1,-1,-1,-1,0,1,0,1,0,1
            //   905 DATA -1,0,-1,-1,1,0,0,0,1,0,-1,-1,1,-1,0,0,0,1
            //   910 DATA -1,0,-1,1,1,0,0,1,0,-1,-1,0,1,0,1,0,0,1
            //   915 DATA 0,-1,-1,0,-1,1,1,0,0,0,-1,-1,-1,1,1,1,0,0
            //   920 DATA -1,0,-1,-1,0,1,0,1,0,0,-1,-1,0,1,0,0,0,1
            //   925 DATA 0,-1,-1,0,1,0,1,0,0,-1,0,-1,1,0,0,0,0,1
            //   930 DATA 0,0,-1,-1,-1,1,0,0,0,-1,0,0,1,1,1,0,0,0
            //   935 DATA 0,-1,0,-1,1,1,0,0,0,-1,0,0,-1,-1,1,0,0,0
            //   940 DATA 0,0,-1,-1,1,0,0,0,0,0,-1,0,1,-1,0,0,0,0
            //   945 DATA -1,0,0,-1,1,0,0,0,0
            //   950 DATA 24,25,36,0,14,15,36,0,15,35,36,47,36,58,59,0
            //   955 DATA 15,35,36,0,24,25,26,0,26,57,58,0
            //   960 DATA 26,35,0,0,47,48,0,0,35,36,0,0,35,36,0,0
            //   965 DATA 36,0,0,0,47,58,0,0,15,0,0,0
            //   970 DATA 26,47,0,0,47,58,0,0,35,36,47,0,28,58,0,0,15,47,0,0
            //
            // The original code loaded this data into two arrays.
            //   40 FOR I=1 TO 19: FOR J=1 TO 9: READ B(I,J): NEXT J: NEXT I
            //   45 FOR I=1 TO 19: FOR J=1 TO 4: READ M(I,J): NEXT J: NEXT I
            //
            // When finding moves for the computer the first array was searched for the current board position, or the
            // reflection of it, and the resulting index was used in the second array to get the possible moves.
            // With this dictionary we can just use the current board as the index, and retrieve a list of moves for
            // consideration by the computer.
            _potentialMoves = new()
            {
                [new(Black, Black, Black, White, None,  None,  None,  White, White)] = Moves((2, 4), (2, 5), (3, 6)),
                [new(Black, Black, Black, None,  White, None,  White, None,  White)] = Moves((1, 4), (1, 5), (3, 6)),
                [new(Black, None,  Black, Black, White, None,  None,  None,  White)] = Moves((1, 5), (3, 5), (3, 6), (4, 7)),
                [new(None,  Black, Black, White, Black, None,  None,  None,  White)] = Moves((3, 6), (5, 8), (5, 9)),
                [new(Black, None,  Black, White, White, None,  None,  White, None)]  = Moves((1, 5), (3, 5), (3, 6)),
                [new(Black, Black, None,  White, None,  White, None,  None,  White)] = Moves((2, 4), (2, 5), (2, 6)),
                [new(None,  Black, Black, None,  Black, White, White, None,  None)]  = Moves((2, 6), (5, 7), (5, 8)),
                [new(None,  Black, Black, Black, White, White, White, None,  None)]  = Moves((2, 6), (3, 5)),
                [new(Black, None,  Black, Black, None,  White, None,  White, None)]  = Moves((4, 7), (4, 8)),
                [new(None,  Black, Black, None,  White, None,  None,  None,  White)] = Moves((3, 5), (3, 6)),
                [new(None,  Black, Black, None,  White, None,  White, None,  None)]  = Moves((3, 5), (3, 6)),
                [new(Black, None,  Black, White, None,  None,  None,  None,  White)] = Moves((3, 6)),
                [new(None,  None,  Black, Black, Black, White, None,  None,  None)]  = Moves((4, 7), (5, 8)),
                [new(Black, None,  None,  White, White, White, None,  None,  None)]  = Moves((1, 5)),
                [new(None,  Black, None,  Black, White, White, None,  None,  None)]  = Moves((2, 6), (4, 7)),
                [new(Black, None,  None,  Black, Black, White, None,  None,  None)]  = Moves((4, 7), (5, 8)),
                [new(None,  None,  Black, Black, White, None,  None,  None,  None)]  = Moves((3, 5), (3, 6), (4, 7)),
                [new(None,  Black, None,  White, Black, None,  None,  None,  None)]  = Moves((2, 8), (5, 8)),
                [new(Black, None,  None,  Black, White, None,  None,  None,  None)]  = Moves((1, 5), (4, 7))
            };
        }

        public int Wins { get; private set; }

        public void AddWin() => Wins++;

        // Try to make a move. We first try to find a legal move for the current board position.
        public bool TryMove(Board board)
        {
            if (TryGetMoves(board, out var moves, out var reflected) &&
                TrySelectMove(moves, out var move))
            {
                // We've found a move, so we record it as the last move made, and then announce and make the move.
                _lastMove = (moves, move);

                // If we found the move from a reflacted match of the board we need to make the reflected move.
                if (reflected) { move = move.Reflected; }

                Console.WriteLine($"I move {move}");
                move.Execute(board);
                return true;
            }

            // We haven't found a move for this board position, so remove the previous move that led to this board
            // position from future consideration. We don't want to make that move again, because we now know it's a
            // non-winning move.
            ExcludeLastMoveFromFuturePlay();

            return false;
        }

        // Looks up the given board and its reflection in the potential moves dictionary. If it's found then we have a
        // list of potential moves. If the board is not found in the dictionary then the computer has no legal moves,
        // and the human player wins.
        private bool TryGetMoves(Board board, out List<Move> moves, out bool reflected)
        {
            if (_potentialMoves.TryGetValue(board, out moves))
            {
                reflected = false;
                return true;
            }

            if (_potentialMoves.TryGetValue(board.Reflected, out moves))
            {
                reflected = true;
                return true;
            }

            reflected = default;
            return false;
        }

        // Get a random move from the list. If the list is empty, then we've previously eliminated all the moves for
        // this board position as being non-winning moves. We therefore resign the game.
        private bool TrySelectMove(List<Move> moves, out Move move)
        {
            if (moves.Any())
            {
                move = moves[_random.Next(moves.Count)];
                return true;
            }

            Console.Write("I resign.");
            move = null;
            return false;
        }

        private void ExcludeLastMoveFromFuturePlay()
        {
            var (moves, move) = _lastMove;
            moves.Remove(move);
        }

        private static List<Move> Moves(params Move[] moves) => moves.ToList();

        public bool IsFullyAdvanced(Board board) =>
            board[9] == Black || board[8] == Black || board[7] == Black;
    }
}
