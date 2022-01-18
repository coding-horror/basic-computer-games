using System.Text;

namespace ThreeDTicTacToe
{
    /// <summary>
    /// Qubic is a 3D Tic-Tac-Toe game played on a 4x4x4 cube. This code allows
    ///  a player to compete against a deterministic AI that is surprisingly
    ///  difficult to beat.
    /// </summary>
    internal class Qubic
    {
        // The Y variable in the original BASIC.
        private static readonly int[] CornersAndCenters = QubicData.CornersAndCenters;
        // The M variable in the original BASIC.
        private static readonly int[,] RowsByPlane = QubicData.RowsByPlane;

        // Board spaces are filled in with numeric values. A space could be:
        //
        //  - EMPTY: no one has moved here yet.
        //  - PLAYER: the player moved here.
        //  - MACHINE: the machine moved here.
        //  - POTENTIAL: the machine, in the middle of its move,
        //      might fill a space with a potential move marker, which
        //      prioritizes the space once it finally chooses where to move.
        //
        // The numeric values allow the program to determine what moves have
        //  been made in a row by summing the values in a row. In theory, the
        //  individual values could be any positive numbers that satisfy the
        //  following:
        //
        //  - EMPTY = 0
        //  - POTENTIAL * 4 < PLAYER
        //  - PLAYER * 4 < MACHINE
        private const double PLAYER = 1.0;
        private const double MACHINE = 5.0;
        private const double POTENTIAL = 0.125;
        private const double EMPTY = 0.0;
        
        // The X variable in the original BASIC. This is the Qubic board,
        //  flattened into a 1D array.
        private readonly double[] Board = new double[64];

        // The L variable in the original BASIC. There are 76 unique winning rows
        //  in the board, so each gets an entry in RowSums. A row sum can be used
        //  to check what moves have been made to that row in the board.
        //
        // Example: if RowSums[i] == PLAYER * 4, the player won with row i!
        private readonly double[] RowSums = new double[76];

        public Qubic() { }

        /// <summary>
        /// Run the Qubic game.
        /// 
        /// Show the title, prompt for instructions, then begin the game loop.
        /// </summary>
        public void Run()
        {
            Title();
            Instructions();
            Loop();
        }

        /***********************************************************************
        /* Terminal Text/Prompts
        /**********************************************************************/
        #region TerminalText

        /// <summary>
        /// Display title and attribution.
        /// 
        /// Original BASIC: 50-120
        /// </summary>
        private static void Title()
        {
            Console.WriteLine(
                "\n" +
                "                                 QUBIC\n\n" +
                "               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n"
            );
        }

        /// <summary>
        /// Prompt user for game instructions.
        /// 
        /// Original BASIC: 210-313
        /// </summary>
        private static void Instructions()
        {
            Console.Write("DO YOU WANT INSTRUCTIONS? ");
            var yes = ReadYesNo();

            if (yes)
            {
                Console.WriteLine(
                    "\n" +
                    "THE GAME IS TIC-TAC-TOE IN A 4 X 4 X 4 CUBE.\n" +
                    "EACH MOVE IS INDICATED BY A 3 DIGIT NUMBER, WITH EACH\n" +
                    "DIGIT BETWEEN 1 AND 4 INCLUSIVE.  THE DIGITS INDICATE THE\n" +
                    "LEVEL, ROW, AND COLUMN, RESPECTIVELY, OF THE OCCUPIED\n" +
                    "PLACE.\n" +
                    "\n" +
                    "TO PRINT THE PLAYING BOARD, TYPE 0 (ZERO) AS YOUR MOVE.\n" +
                    "THE PROGRAM WILL PRINT THE BOARD WITH YOUR MOVES INDI-\n" +
                    "CATED WITH A (Y), THE MACHINE'S MOVES WITH AN (M), AND\n" +
                    "UNUSED SQUARES WITH A ( ).  OUTPUT IS ON PAPER.\n" +
                    "\n" +
                    "TO STOP THE PROGRAM RUN, TYPE 1 AS YOUR MOVE.\n\n"
                );
            }
        }

        /// <summary>
        /// Prompt player for whether they would like to move first, or allow
        ///  the machine to make the first move.
        ///  
        /// Original BASIC: 440-490
        /// </summary>
        /// <returns>true if the player wants to move first</returns>
        private static bool PlayerMovePreference()
        {
            Console.Write("DO YOU WANT TO MOVE FIRST? ");
            var result = ReadYesNo();
            Console.WriteLine();
            return result;
        }

        /// <summary>
        /// Run the Qubic program loop.
        /// </summary>
        private void Loop()
        {
            // The "retry" loop; ends if player quits or chooses not to retry
            // after game ends.
            while (true)
            {
                ClearBoard();
                var playerNext = PlayerMovePreference();

                // The "game" loop; ends if player quits, player/machine wins,
                // or game ends in draw.
                while (true)
                {
                    if (playerNext)
                    {
                        // Player makes a move.
                        var playerAction = PlayerMove();
                        if (playerAction == PlayerAction.Move)
                        {
                            playerNext = !playerNext;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        // Check for wins, if any.
                        RefreshRowSums();
                        if (CheckPlayerWin() || CheckMachineWin())
                        {
                            break;
                        }

                        // Machine makes a move.
                        var machineAction = MachineMove();
                        if (machineAction == MachineAction.Move)
                        {
                            playerNext = !playerNext;
                        }
                        else if (machineAction == MachineAction.End)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("unreachable; machine should always move or end game in game loop");
                        }
                    }
                }

                var retry = RetryPrompt();

                if (!retry)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Prompt the user to try another game.
        /// 
        /// Original BASIC: 1490-1560
        /// </summary>
        /// <returns>true if the user wants to play again</returns>
        private static bool RetryPrompt()
        {
            Console.Write("DO YOU WANT TO TRY ANOTHER GAME? ");
            return ReadYesNo();
        }

        /// <summary>
        /// Read a yes/no from the terminal. This method accepts anything that
        ///  starts with N/n as no and Y/y as yes.
        /// </summary>
        /// <returns>true if the player answered yes</returns>
        private static bool ReadYesNo()
        {
            while (true)
            {
                var response = Console.ReadLine() ?? " ";
                if (response.ToLower().StartsWith("y"))
                {
                    return true;
                }
                else if (response.ToLower().StartsWith("n"))
                {
                    return false;
                }
                else
                {
                    Console.Write("INCORRECT ANSWER.  PLEASE TYPE 'YES' OR 'NO'. ");
                }
            }
        }

        #endregion

        /***********************************************************************
        /* Player Move
        /**********************************************************************/
        #region PlayerMove

        /// <summary>
        /// Possible actions player has taken after ending their move. This
        ///  replaces the `GOTO` logic that allowed the player to jump out of
        ///  the game loop and quit.
        /// </summary>
        private enum PlayerAction
        {
            /// <summary>
            /// The player ends the game prematurely.
            /// </summary>
            Quit,
            /// <summary>
            /// The player makes a move on the board.
            /// </summary>
            Move,
        }

        /// <summary>
        /// Make the player's move based on their input.
        /// 
        /// Original BASIC: 500-620
        /// </summary>
        /// <returns>Whether the player moved or quit the program.</returns>
        private PlayerAction PlayerMove()
        {
            // Loop until a valid move is inputted.
            while (true)
            {
                var move = ReadMove();
                if (move == 1)
                {
                    return PlayerAction.Quit;
                }
                else if (move == 0)
                {
                    ShowBoard();
                }
                else
                {
                    ClearPotentialMoves();
                    if (TryCoordToIndex(move, out int moveIndex))
                    {
                        if (Board[moveIndex] == EMPTY)
                        {
                            Board[moveIndex] = PLAYER;
                            return PlayerAction.Move;
                        }
                        else
                        {
                            Console.WriteLine("THAT SQUARE IS USED, TRY AGAIN.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("INCORRECT MOVE, TRY AGAIN.");
                    }
                }
            }
        }

        /// <summary>
        /// Read a player move from the terminal. Move can be any integer.
        /// 
        /// Original BASIC: 510-520
        /// </summary>
        /// <returns>the move inputted</returns>
        private static int ReadMove()
        {
            Console.Write("YOUR MOVE? ");
            return ReadInteger();
        }

        /// <summary>
        /// Read an integer from the terminal.
        /// 
        /// Original BASIC: 520
        /// 
        /// Unlike the basic, this code will not accept any string that starts
        ///  with a number; only full number strings are allowed.
        /// </summary>
        /// <returns>the integer inputted</returns>
        private static int ReadInteger()
        {
            while (true)
            {
                var response = Console.ReadLine() ?? " ";

                if (int.TryParse(response, out var move))
                {
                    return move;

                }
                else
                {
                    Console.Write("!NUMBER EXPECTED - RETRY INPUT LINE--? ");
                }
            }
        }

        /// <summary>
        /// Display the board to the player. Spaces taken by the player are
        ///  marked with "Y", while machine spaces are marked with "M".
        ///  
        /// Original BASIC: 2550-2740
        /// </summary>
        private void ShowBoard()
        {
            var s = new StringBuilder(new string('\n', 9));

            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 4; j++)
                {
                    s.Append(' ', 3 * (j + 1));
                    for (int k = 1; k <= 4; k++)
                    {
                        int q = (16 * i) + (4 * j) + k - 21;
                        s.Append(Board[q] switch
                        {
                            EMPTY or POTENTIAL => "( )      ",
                            PLAYER => "(Y)      ",
                            MACHINE => "(M)      ",
                            _ => throw new Exception($"invalid space value {Board[q]}"),
                        });
                    }
                    s.Append("\n\n");
                }
                s.Append("\n\n");
            }

            Console.WriteLine(s.ToString());
        }

        #endregion

        /***********************************************************************
        /* Machine Move
        /**********************************************************************/
        #region MachineMove

        /// <summary>
        /// Check all rows for a player win.
        /// 
        /// A row indicates a player win if its sum = PLAYER * 4.
        /// 
        /// Original BASIC: 720-780
        /// </summary>
        /// <returns>whether the player won in any row</returns>
        private bool CheckPlayerWin()
        {
            for (int row = 0; row < 76; row++)
            {
                if (RowSums[row] == (PLAYER * 4))
                {
                    // Found player win!
                    Console.WriteLine("YOU WIN AS FOLLOWS");
                    DisplayRow(row);
                    return true;
                }
            }

            // No player win found.
            return false;
        }

        /// <summary>
        /// Check all rows for a row that the machine could move to to win
        ///  immediately.
        /// 
        /// A row indicates a player could win immediately if it has three
        ///  machine moves already; that is, sum = MACHINE * 3.
        /// 
        /// Original Basic: 790-920
        /// </summary>
        /// <returns></returns>
        private bool CheckMachineWin()
        {
            for (int row = 0; row < 76; row++)
            {
                if (RowSums[row] == (MACHINE * 3))
                {
                    // Found a winning row!
                    for (int space = 0; space < 4; space++)
                    {
                        int move = RowsByPlane[row, space];
                        if (Board[move] == EMPTY)
                        {
                            // Found empty space in winning row; move there.
                            Board[move] = MACHINE;
                            Console.WriteLine($"MACHINE MOVES TO {IndexToCoord(move)} , AND WINS AS FOLLOWS");
                            DisplayRow(row);
                            return true;
                        }
                    }
                }
            }

            // No winning row available.
            return false;
        }

        /// <summary>
        /// Display the coordinates of a winning row.
        /// </summary>
        /// <param name="row">index into RowsByPlane data</param>
        private void DisplayRow(int row)
        {
            for (int space = 0; space < 4; space++)
            {
                Console.Write($" {IndexToCoord(RowsByPlane[row, space])} ");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Possible actions machine can take in a move. This helps replace the
        ///  complex GOTO logic from the original BASIC, which allowed the
        ///  program to jump from the machine's action to the end of the game.
        /// </summary>
        private enum MachineAction
        {
            /// <summary>
            /// Machine did not take any action.
            /// </summary>
            None,
            /// <summary>
            /// Machine made a move.
            /// </summary>
            Move,
            /// <summary>
            /// Machine either won, conceded, or found a draw.
            /// </summary>
            End,
        }

        /// <summary>
        /// Machine decides where to move on the board, and ends the game if
        ///  appropriate.
        ///  
        /// The machine's AI tries to take the following actions (in order):
        /// 
        ///  1. If the player has a row that will get them the win on their
        ///     next turn, block that row.
        ///  2. If the machine can trap the player (create two different rows
        ///     with three machine moves each that cannot be blocked by only a
        ///     single player move, create such a trap.
        ///  3. If the player can create a similar trap for the machine on
        ///     their next move, block the space where that trap would be
        ///     created.
        ///  4. Find a plane in the board that is well-populated by player
        ///     moves, and take a space in the first such plane.
        ///  5. Find the first open corner or center and move there.
        ///  6. Find the first open space and move there.
        /// 
        /// If none of these actions are possible, then the board is entirely
        ///  full, and the game results in a draw.
        ///  
        /// Original BASIC: start at 930
        /// </summary>
        /// <returns>the action the machine took</returns>
        private MachineAction MachineMove()
        {
            // The actions the machine attempts to take, in order.
            var actions = new Func<MachineAction>[]
            {
                BlockPlayer,
                MakePlayerTrap,
                BlockMachineTrap,
                MoveByPlane,
                MoveCornerOrCenter,
                MoveAnyOpenSpace,
            };

            foreach (var action in actions)
            {
                // Try each action, moving to the next if nothing happens.
                var actionResult = action();
                if (actionResult != MachineAction.None)
                {
                    // Not in original BASIC: check for draw after each machine
                    // move.
                    if (CheckDraw())
                    {
                        return DrawGame();
                    }
                    return actionResult;
                }
            }

            // If we got here, all spaces are taken. Draw the game.
            return DrawGame();
        }

        /// <summary>
        /// Block a row with three spaces already taken by the player.
        /// 
        /// Original BASIC: 930-1010
        /// </summary>
        /// <returns>
        /// Move if the machine blocked,
        /// None otherwise
        /// </returns>
        private MachineAction BlockPlayer()
        {
            for (int row = 0; row < 76; row++)
            {
                if (RowSums[row] == (PLAYER * 3))
                {
                    // Found a row to block on!
                    for (int space = 0; space < 4; space++)
                    {
                        if (Board[RowsByPlane[row, space]] == EMPTY)
                        {
                            // Take the remaining empty space.
                            Board[RowsByPlane[row, space]] = MACHINE;
                            Console.WriteLine($"NICE TRY. MACHINE MOVES TO {IndexToCoord(RowsByPlane[row, space])}");
                            return MachineAction.Move;
                        }
                    }
                }
            }

            // Didn't find a row to block on.
            return MachineAction.None;
        }

        /// <summary>
        /// Create a trap for the player if possible. A trap can be created if
        ///  moving to a space on the board results in two different rows having
        ///  three MACHINE spaces, with the remaining space not shared between
        ///  the two rows. The player can only block one of these traps, so the
        ///  machine will win.
        ///  
        /// If a player trap is not possible, but a row is found that is
        ///  particularly advantageous for the machine to move to, the machine
        ///  will try and move to a plane edge in that row.
        ///  
        /// Original BASIC: 1300-1480
        /// 
        /// Lines 1440/50 of the BASIC call 2360 (MovePlaneEdge). Because it
        ///  goes to this code only after it has found an open space marked as
        ///  potential, it cannot reach line 2440 of that code, as that is only
        ///  reached if an open space failed to be found in the row on which
        ///  that code was called.
        /// </summary>
        /// <returns>
        /// Move if a trap was created,
        /// End if the machine conceded,
        /// None otherwise
        /// </returns>
        private MachineAction MakePlayerTrap()
        {
            for (int row = 0; row < 76; row++)
            {
                // Refresh row sum, since new POTENTIALs might have changed it.
                var rowSum = RefreshRowSum(row);

                // Machine has moved in this row twice, and player has not moved
                // in this row.
                if (rowSum >= (MACHINE * 2) && rowSum < (MACHINE * 2) + 1)
                {
                    // Machine has no potential moves yet in this row.
                    if (rowSum == (MACHINE * 2))
                    {
                        for (int space = 0; space < 4; space++)
                        {
                            // Empty space can potentially be used to create a
                            // trap.
                            if (Board[RowsByPlane[row, space]] == EMPTY)
                            {
                                Board[RowsByPlane[row, space]] = POTENTIAL;
                            }
                        }
                    }
                    // Machine has already found a potential move in this row,
                    // so a trap can be created with another row.
                    else
                    {
                        return MakeOrBlockTrap(row);
                    }
                }
            }

            // No player traps can be made.
            RefreshRowSums();

            for (int row = 0; row < 76; row++)
            {
                // A row may be particularly advantageous for the machine to
                // move to at this point; this is the case if a row is entirely
                // filled with POTENTIAL or has one MACHINE and others
                // POTENTIAL. Such rows may help set up trapping opportunities.
                if (RowSums[row] == (POTENTIAL * 4) || RowSums[row] == MACHINE + (POTENTIAL * 3))
                {
                    // Try moving to a plane edge in an advantageous row.
                    return MovePlaneEdge(row, POTENTIAL);
                }
            }

            // No spaces found that are particularly advantageous to machine.
            ClearPotentialMoves();
            return MachineAction.None;
        }

        /// <summary>
        /// Block a trap that the player could create for the machine on their
        ///  next turn.
        ///  
        /// If there are no player traps to block, but a row is found that is
        ///  particularly advantageous for the player to move to, the machine
        ///  will try and move to a plane edge in that row.
        ///  
        /// Original BASIC: 1030-1190
        /// 
        /// Lines 1160/1170 of the BASIC call 2360 (MovePlaneEdge). As with
        ///  MakePlayerTrap, because it goes to this code only after it has
        ///  found an open space marked as potential, it cannot reach line 2440
        ///  of that code, as that is only reached if an open space failed to be
        ///  found in the row on which that code was called.
        /// </summary>
        /// <returns>
        /// Move if a trap was created,
        /// End if the machine conceded,
        /// None otherwise
        /// </returns>
        private MachineAction BlockMachineTrap()
        {
            for (int i = 0; i < 76; i++)
            {
                // Refresh row sum, since new POTENTIALs might have changed it.
                var rowSum = RefreshRowSum(i);

                // Player has moved in this row twice, and machine has not moved
                // in this row.
                if (rowSum >= (PLAYER * 2) && rowSum < (PLAYER * 2) + 1)
                {
                    // Machine has no potential moves yet in this row.
                    if (rowSum == (PLAYER * 2))
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (Board[RowsByPlane[i, j]] == EMPTY)
                            {
                                Board[RowsByPlane[i, j]] = POTENTIAL;
                            }
                        }
                    }
                    // Machine has already found a potential move in this row,
                    // so a trap can be created with another row by the player.
                    // Move to block.
                    else
                    {
                        return MakeOrBlockTrap(i);
                    }
                }
            }

            // No player traps to block found.
            RefreshRowSums();

            for (int row = 0; row < 76; row++)
            {
                // A row may be particularly advantageous for the player to move
                // to at this point, indicated by a row containing all POTENTIAL
                // moves or one PLAYER and rest POTENTIAL. Such rows may aid in
                // in the later creation of traps.
                if (RowSums[row] == (POTENTIAL * 4) || RowSums[row] == PLAYER + (POTENTIAL * 3))
                {
                    // Try moving to a plane edge in an advantageous row.
                    return MovePlaneEdge(row, POTENTIAL);
                }
            }

            // No spaces found that are particularly advantageous to the player.
            return MachineAction.None;
        }

        /// <summary>
        /// Either make a trap for the player or block a trap the player could
        ///  create on their next turn.
        ///  
        /// Unclear how this method could possibly end with a concession; it
        ///  seems it can only be called if the row contains a potential move.
        ///  
        /// Original BASIC: 2230-2350
        /// </summary>
        /// <param name="row">the row containing the space to move to</param>
        /// <returns>
        /// Move if the machine moved,
        /// End if the machine conceded
        /// </returns>
        private MachineAction MakeOrBlockTrap(int row)
        {
            for (int space = 0; space < 4; space++)
            {
                if (Board[RowsByPlane[row, space]] == POTENTIAL)
                {
                    Board[RowsByPlane[row, space]] = MACHINE;

                    // Row sum indicates we're blocking a player trap.
                    if (RowSums[row] < MACHINE)
                    {
                        Console.Write("YOU FOX.  JUST IN THE NICK OF TIME, ");
                    }
                    // Row sum indicates we're completing a machine trap.
                    else
                    {
                        Console.Write("LET'S SEE YOU GET OUT OF THIS:  ");
                    }

                    Console.WriteLine($"MACHINE MOVES TO {IndexToCoord(RowsByPlane[row, space])}");

                    return MachineAction.Move;
                }
            }

            // Unclear how this can be reached.
            Console.WriteLine("MACHINE CONCEDES THIS GAME.");
            return MachineAction.End;
        }

        /// <summary>
        /// Find a satisfactory plane on the board and move to one if that
        ///  plane's plane edges.
        ///  
        /// A plane on the board is satisfactory if it meets the following
        ///  conditions:
        ///     1. Player has made exactly 4 moves on the plane.
        ///     2. Machine has made either 0 or one moves on the plane.
        ///  Such a plane is one that the player could likely use to form traps.
        /// 
        /// Original BASIC: 1830-2020
        /// 
        /// Line 1990 of the original basic calls 2370 (MovePlaneEdge). Only on
        ///  this call to MovePlaneEdge can line 2440 of that method be reached,
        ///  which surves to help this method iterate through the rows of a
        ///  plane.
        /// </summary>
        /// <returns>
        /// Move if a move in a plane was found,
        /// None otherwise
        /// </returns>
        private MachineAction MoveByPlane()
        {
            // For each plane in the cube...
            for (int plane = 1; plane <= 18; plane++)
            {
                double planeSum = PlaneSum(plane);

                // Check that plane sum satisfies condition.
                const double P4 = PLAYER * 4;
                const double P4_M1 = (PLAYER * 4) + MACHINE;
                if (
                    (planeSum >= P4 && planeSum < P4 + 1) || 
                    (planeSum >= P4_M1 && planeSum < P4_M1 + 1)
                )
                {
                    // Try to move to plane edges in each row of plane
                    // First, check for plane edges marked as POTENTIAL.
                    for (int row = (4 * plane) - 4; row < (4 * plane); row++)
                    {
                        var moveResult = MovePlaneEdge(row, POTENTIAL);
                        if (moveResult != MachineAction.None)
                        {
                            return moveResult;
                        }
                    }

                    // If no POTENTIAL plane edge found, look for an EMPTY one.
                    for (int row = (4 * plane) - 4; row < (4 * plane); row++)
                    {
                        var moveResult = MovePlaneEdge(row, EMPTY);
                        if (moveResult != MachineAction.None)
                        {
                            return moveResult;
                        }
                    }
                }
            }

            // No satisfactory planes with open plane edges found.
            ClearPotentialMoves();
            return MachineAction.None;
        }

        /// <summary>
        /// Given a row, move to the first space in that row that:
        ///  1. is a plane edge, and
        ///  2. has the given value in Board
        ///  
        /// Plane edges are any spaces on a plane with one face exposed. The AI
        ///  prefers to move to these spaces before others, presumably
        ///  because they are powerful moves: a plane edge is contained on 3-4
        ///  winning rows of the cube.
        ///  
        /// Original BASIC: 2360-2490
        /// 
        /// In the original BASIC, this code is pointed to from three different
        ///  locations by GOTOs: 
        ///  - 1440/50, or MakePlayerTrap; 
        ///  - 1160/70, or BlockMachineTrap; and
        ///  - 1990, or MoveByPlane. 
        /// At line 2440, this code jumps back to line 2000, which is in
        ///  MoveByPlane. This makes it appear as though calling MakePlayerTrap
        ///  or BlockPlayerTrap in the BASIC could jump into the middle of the
        ///  MoveByPlane method; were this to happen, not all of MoveByPlane's
        ///  variables would be defined! However, the program logic prevents
        ///  this from ever occurring; see each method's description for why
        ///  this is the case.
        /// </summary>
        /// <param name="row">the row to try to move to</param>
        /// <param name="spaceValue">
        /// what value the space to move to should have in Board
        /// </param>
        /// <returns>
        /// Move if a plane edge piece in the row with the given spaceValue was
        /// found,
        /// None otherwise
        /// </returns>
        private MachineAction MovePlaneEdge(int row, double spaceValue)
        {
            // Given a row, we want to find the plane edge pieces in that row.
            // We know that each row is part of a plane, and that the first
            // and last rows of the plane are on the plane edge, while the
            // other two rows are in the middle. If we know whether a row is an
            // edge or middle, we can determine which spaces in that row are
            // plane edges.
            //
            // Below is a birds-eye view of a plane in the cube, with rows
            // oriented horizontally:
            //
            //   row 0: ( ) (1) (2) ( )
            //   row 1: (0) ( ) ( ) (3)
            //   row 2: (0) ( ) ( ) (3)
            //   row 3: ( ) (1) (2) ( )
            //
            // The plane edge pieces have their row indices marked. The pattern
            // above shows that:
            // 
            //  if row == 0 | 3, plane edge spaces = [1, 2]
            //  if row == 1 | 2, plane edge spaces = [0, 3]

            // The below condition replaces the following BASIC code (2370):
            //  
            //  I-(INT(I/4)*4)>1
            //
            // which in C# would be:
            //
            //
            // int a = i - (i / 4) * 4 <= 1)
            //     ? 1
            //     : 2;
            //
            // In the above, i is the one-indexed row in RowsByPlane.
            //
            // This condition selects a different a value based on whether the
            // given row is on the edge or middle of its plane.
            int a = (row % 4) switch
            {
                0 or 3 => 1,  // row is on edge of plane
                1 or 2 => 2,  // row is in middle of plane
                _ => throw new Exception($"unreachable ({row % 4})"),
            };

            // Iterate through plane edge pieces of the row.
            //
            //  if a = 1 (row is edge), iterate through [0, 3]
            //  if a = 2 (row is middle), iterate through [1, 2]
            for (int space = a - 1; space <= 4 - a; space += 5 - (2 * a))
            {
                if (Board[RowsByPlane[row, space]] == spaceValue)
                {
                    // Found a plane edge to take!
                    Board[RowsByPlane[row, space]] = MACHINE;
                    Console.WriteLine($"MACHINE TAKES {IndexToCoord(RowsByPlane[row, space])}");
                    return MachineAction.Move;
                }
            }

            // No valid corner edge to take.
            return MachineAction.None;
        }

        /// <summary>
        /// Find the first open corner or center in the board and move there.
        /// 
        /// Original BASIC: 1200-1290
        /// 
        /// This is the only place where the Z variable from the BASIC code is
        ///  used; here it is implied in the for loop.
        /// </summary>
        /// <returns>
        /// Move if an open corner/center was found and moved to,
        /// None otherwise
        /// </returns>
        private MachineAction MoveCornerOrCenter()
        {
            foreach (int space in CornersAndCenters)
            {
                if (Board[space] == EMPTY)
                {
                    Board[space] = MACHINE;
                    Console.WriteLine($"MACHINE MOVES TO {IndexToCoord(space)}");
                    return MachineAction.Move;
                }
            }

            return MachineAction.None;
        }

        /// <summary>
        /// Find the first open space in the board and move there.
        ///
        /// Original BASIC: 1720-1800
        /// </summary>
        /// <returns>
        /// Move if an open space was found and moved to,
        /// None otherwise
        /// </returns>
        private MachineAction MoveAnyOpenSpace()
        {
            for (int row = 0; row < 64; row++)
            {
                if (Board[row] == EMPTY)
                {
                    Board[row] = MACHINE;
                    Console.WriteLine($"MACHINE LIKES {IndexToCoord(row)}");
                    return MachineAction.Move;
                }
            }
            return MachineAction.None;
        }

        /// <summary>
        /// Draw the game in the event that there are no open spaces.
        /// 
        /// Original BASIC: 1810-1820
        /// </summary>
        /// <returns>End</returns>
        private MachineAction DrawGame()
        {
            Console.WriteLine("THIS GAME IS A DRAW.");
            return MachineAction.End;
        }

        #endregion

        /***********************************************************************
        /* Helpers
        /**********************************************************************/
        #region Helpers

        /// <summary>
        /// Attempt to transform a cube coordinate to an index into Board.
        /// 
        /// A valid cube coordinate is a three-digit number, where each digit
        ///  of the number X satisfies 1 <= X <= 4.
        ///  
        /// Examples:
        ///  111 -> 0
        ///  444 -> 63
        ///  232 -> 35
        ///  
        /// If the coord provided is not valid, the transformation fails.
        /// 
        /// The conversion from coordinate to index is essentially a conversion
        ///  between base 4 and base 10.
        ///  
        /// Original BASIC: 525-580
        /// 
        /// This method fixes a bug in the original BASIC (525-526), which only
        ///  checked whether the given coord satisfied 111 <= coord <= 444. This
        ///  allows invalid coordinates such as 199 and 437, whose individual
        ///  digits are out of range.
        /// </summary>
        /// <param name="coord">cube coordinate (e.g. "111", "342")</param>
        /// <param name="index">trasnformation output</param>
        /// <returns>
        /// true if the transformation was successful, false otherwise
        /// </returns>
        private static bool TryCoordToIndex(int coord, out int index)
        {
            // parse individual digits, subtract 1 to get base 4 number
            var hundreds = (coord / 100) - 1;
            var tens = ((coord % 100) / 10) - 1;
            var ones = (coord % 10) - 1;

            // bounds check for each digit
            foreach (int digit in new int[] { hundreds, tens, ones })
            {
                if (digit < 0 || digit > 3)
                {
                    index = -1;
                    return false;
                }
            }

            // conversion from base 4 to base 10
            index = (16 * hundreds) + (4 * tens) + ones;
            return true;
        }

        /// <summary>
        /// Transform a Board index into a valid cube coordinate.
        ///  
        /// Examples:
        ///  0 -> 111
        ///  63 -> 444
        ///  35 -> 232
        /// 
        /// The conversion from index to coordinate is essentially a conversion
        ///  between base 10 and base 4.
        ///  
        /// Original BASIC: 1570-1610
        /// </summary>
        /// <param name="index">Board index</param>
        /// <returns>the corresponding cube coordinate</returns>
        private static int IndexToCoord(int index)
        {
            // check that index is valid
            if (index < 0 || index > 63)
            {
                // runtime exception; all uses of this method are with
                // indices provided by the program, so this should never fail
                throw new Exception($"index {index} is out of range");
            }

            // convert to base 4, add 1 to get cube coordinate
            var hundreds = (index / 16) + 1;
            var tens = ((index % 16) / 4) + 1;
            var ones = (index % 4) + 1;

            // concatenate digits
            int coord = (hundreds * 100) + (tens * 10) + ones;
            return coord;
        }

        /// <summary>
        /// Refresh the values in RowSums to account for any changes.
        /// 
        /// Original BASIC: 1640-1710
        /// </summary>
        private void RefreshRowSums()
        {
            for (var row = 0; row < 76; row++)
            {
                RefreshRowSum(row);
            }
        }

        /// <summary>
        /// Refresh a row in RowSums to reflect changes.
        /// </summary>
        /// <param name="row">row in RowSums to refresh</param>
        /// <returns>row sum after refresh</returns>
        private double RefreshRowSum(int row)
        {
            double rowSum = 0;
            for (int space = 0; space < 4; space++)
            {
                rowSum += Board[RowsByPlane[row, space]];
            }
            RowSums[row] = rowSum;
            return rowSum;
        }

        /// <summary>
        /// Calculate the sum of spaces in one of the 18 cube planes in RowSums.
        /// 
        /// Original BASIC: 1840-1890
        /// </summary>
        /// <param name="plane">the desired plane</param>
        /// <returns>sum of spaces in plane</returns>
        private double PlaneSum(int plane)
        {
            double planeSum = 0;
            for (int row = (4 * (plane - 1)); row < (4 * plane); row++)
            {
                for (int space = 0; space < 4; space++)
                {
                    planeSum += Board[RowsByPlane[row, space]];
                }
            }
            return planeSum;
        }

        /// <summary>
        /// Check whether the board is in a draw state, that is all spaces are
        ///  full and neither the player nor the machine has won.
        ///  
        /// The original BASIC contains a bug that if the player moves first, a
        ///  draw will go undetected. An example series of player inputs
        ///  resulting in such a draw (assuming player goes first):
        ///  
        ///  114, 414, 144, 444, 122, 221, 112, 121,
        ///  424, 332, 324, 421, 231, 232, 244, 311, 
        ///  333, 423, 331, 134, 241, 243, 143, 413, 
        ///  142, 212, 314, 341, 432, 412, 431, 442
        /// </summary>
        /// <returns>whether the game is a draw</returns>
        private bool CheckDraw()
        {
            for (var i = 0; i < 64; i++)
            {
                if (Board[i] != PLAYER && Board[i] != MACHINE)
                {
                    return false;
                }
            }

            RefreshRowSums();

            for (int row = 0; row < 76; row++)
            {
                var rowSum = RowSums[row];
                if (rowSum == PLAYER * 4 || rowSum == MACHINE * 4)
                {
                    return false;
                }
            }


            return true;
        }

        /// <summary>
        /// Reset POTENTIAL spaces in Board to EMPTY.
        /// 
        /// Original BASIC: 2500-2540
        /// </summary>
        private void ClearPotentialMoves()
        {
            for (var i = 0; i < 64; i++)
            {
                if (Board[i] == POTENTIAL)
                {
                    Board[i] = EMPTY;
                }
            }
        }

        /// <summary>
        /// Reset all spaces in Board to EMPTY.
        /// 
        /// Original BASIC: 400-420
        /// </summary>
        private void ClearBoard()
        {
            for (var i = 0; i < 64; i++)
            {
                Board[i] = EMPTY;
            }
        }

        #endregion
    }
}
