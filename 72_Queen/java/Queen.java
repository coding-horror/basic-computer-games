import java.util.List;
import java.util.Optional;
import java.util.Scanner;

/**
 * QUEEN
 * <p>
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 */
public class Queen {

    public static final int WINNING_POSITION = 158;
    public static final int FORFEIT_MOVE = 0;

    // @formatter:off
    private static final int[] S = {
             81,  71,  61,  51,  41,  31, 21, 11,
             92,  82,  72,  62,  52,  42, 32, 22,
            103,  93,  83,  73,  63,  53, 43, 33,
            114, 104,  94,  84,  74,  64, 54, 44,
            125, 115, 105,  95,  85,  75, 65, 55,
            136, 126, 116, 106,  96,  86, 76, 66,
            147, 137, 127, 117, 107,  97, 87, 77,
            158, 148, 138, 128, 118, 108, 98, 88
    };
    // @formatter:on

    private static final Scanner scanner = new Scanner(System.in);


    public static void main(String[] args) {
        printWithTab("QUEEN", 33);
        printWithTab("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", 15);
        System.out.println("\n\n");

        askAndShowInstructions();

        boolean anotherGame;
        do {
            printBoard();

            Move firstMove = getUserFirstMove();
            if (firstMove.move == 0) {
                printWonByForfeit();
            }

            if (isTopmostRowOrRightmostColumn(firstMove)) {
                playOneGame(firstMove);
            }

            anotherGame = askForAnotherGame();

        } while (anotherGame);


        System.out.println("\nOK --- THANKS AGAIN.");
    }

    /**
     * Play one game starting with the first move from the user
     */
    private static void playOneGame(Move firstMove) {
        boolean gameInProgress = true;
        Move userMove = firstMove;

        while (gameInProgress) {

            if (userMove.move == WINNING_POSITION) {
                //players wins
                printCongratulatoryMessage();
                gameInProgress = false;
            } else {

                ComputerMove computerMove = getComputerMove(userMove);

                System.out.printf("COMPUTER MOVES TO SQUARE %d\n", computerMove.move);

                if (computerMove.move == WINNING_POSITION) {
                    printComputerWins();
                    gameInProgress = false;
                } else {
                    userMove = getValidUserMove(computerMove);

                    if (userMove.move == FORFEIT_MOVE) {
                        printWonByForfeit();
                        gameInProgress = false;
                    } else if (userMove.move == WINNING_POSITION) {
                        printCongratulatoryMessage();
                        gameInProgress = false;
                    }
                }
            }
        }
    }

    /**
     * Get the user's first move
     */
    private static Move getUserFirstMove() {
        boolean validMove;
        Move move;
        do {
            System.out.print("WHERE WOULD YOU LIKE TO START? ");
            int movePosition = scanner.nextInt();
            move = new Move(movePosition);
            validMove = false;
            if (!isTopmostRowOrRightmostColumn(move)) {
                System.out.println("PLEASE READ THE DIRECTIONS AGAIN.");
                System.out.println("YOU HAVE BEGUN ILLEGALLY.");
                System.out.println();
            } else {
                validMove = true;
            }
            scanner.nextLine();
        } while (!validMove);
        return move;
    }

    /**
     * Prompt and get a valid move from the user. Uses the computer's latest move to validate the next move.
     */
    private static Move getValidUserMove(ComputerMove latestComputerMove) {
        boolean validUserMove = false;
        Move userMove = null;
        while (!validUserMove) {
            userMove = getUserMove();
            if (!validateUserMove(userMove, latestComputerMove)) {
                System.out.println("\nY O U   C H E A T . . .  TRY AGAIN");
            } else {
                validUserMove = true;
            }
        }
        return userMove;
    }

    private static void printWonByForfeit() {
        System.out.println("\nIT LOOKS LIKE I HAVE WON BY FORFEIT.\n");
    }

    private static boolean validateUserMove(Move userMove, ComputerMove computerMove) {
        if (userMove.move <= computerMove.move) {
            return false;
        }

        if (userMove.move == FORFEIT_MOVE || userMove.move == WINNING_POSITION) {
            return true;
        }

        int tensValueUser = userMove.move / 10;
        int unitsValueUser = userMove.move - (tensValueUser * 10);
        int unitsValueComputer = computerMove.u;
        int tensValueComputer = computerMove.t;
        int p = unitsValueUser - unitsValueComputer;
        if (p != 0) {
            if ((tensValueUser - tensValueComputer) != p) {
                return (tensValueUser - tensValueComputer) == 2 * p;
            } else {
                return true;
            }
        } else {
            int l = tensValueUser - tensValueComputer;
            return l > 0;
        }
    }

    private static Move getUserMove() {
        System.out.print("WHAT IS YOUR MOVE? ");
        int movePosition = scanner.nextInt();
        scanner.nextLine();
        return new Move(movePosition);
    }

    private static void printComputerWins() {
        System.out.println("\nNICE TRY, BUT IT LOOKS LIKE I HAVE WON.");
        System.out.println("THANKS FOR PLAYING.\n");
    }

    private static boolean askForAnotherGame() {
        System.out.print("ANYONE ELSE CARE TO TRY? ");
        do {
            String response = Queen.scanner.nextLine();
            if (response.equals("NO")) {
                return false;
            } else if (response.equals("YES")) {
                return true;
            } else {
                System.out.println("PLEASE ANSWER 'YES' OR 'NO'.");
            }
        } while (true);
    }

    private static boolean isTopmostRowOrRightmostColumn(Move move) {
        return move.unitsPlaceValue == 1 || move.unitsPlaceValue == move.tensPlaceValue;
    }

    private static ComputerMove getComputerMove(Move userMove) {
        int unitsValueUser = userMove.unitsPlaceValue;
        int tensValueUser = userMove.tensPlaceValue;

        List<Integer> moveRandomlyFromPositions = List.of(41, 44, 73, 75, 126, 127);

        if (moveRandomlyFromPositions.contains(userMove.move)) {
            //random move
            return getMovePositionInRandomDirection(unitsValueUser, tensValueUser);
        } else {

            for (int k = 7; k >= 1; k--) {
                // consider same row first, left-most position
                Optional<ComputerMove> move = evaluatePositionAndGetMove(unitsValueUser, tensValueUser + k);
                if (move.isPresent()) return move.get();

                //try same column, bottom-most
                move = evaluatePositionAndGetMove(unitsValueUser + k, tensValueUser + k);
                if (move.isPresent()) return move.get();

                //now left-most at the bottom-most
                move = evaluatePositionAndGetMove(unitsValueUser + k, tensValueUser + k + k);
                if (move.isPresent()) return move.get();
            }

            //else move one step in a random direction
            return getMovePositionInRandomDirection(unitsValueUser, tensValueUser);
        }

    }

    private static Optional<ComputerMove> evaluatePositionAndGetMove(int newUnitsValue, int newTensValue) {
        EvaluationResult result = evaluateComputerMove(newUnitsValue, newTensValue);
        if (result.favourablePosition) {
            return Optional.of(new ComputerMove(result.move));
        }
        return Optional.empty();
    }

    private static EvaluationResult evaluateComputerMove(int u, int t) {
        int m = 10 * t + u;
        if (m == 158 || m == 127 || m == 126 || m == 75 || m == 73) {
            return new EvaluationResult(m, true);
        } else {
            return new EvaluationResult(m, false);
        }
    }

    private static void printCongratulatoryMessage() {
        System.out.println();
        System.out.println("C O N G R A T U L A T I O N S . . .");
        System.out.println();
        System.out.println("YOU HAVE WON--VERY WELL PLAYED.");
        System.out.println("IT LOOKS LIKE I HAVE MET MY MATCH.");
        System.out.println("THANKS FOR PLAYING---I CAN'T WIN ALL THE TIME.");
        System.out.println();

    }

    private static ComputerMove getMovePositionInRandomDirection(int u1, int t1) {
        double randomValue = Math.random();

        if (randomValue > 0.6) {
            // Move directly down
            return new ComputerMove(calculateMove(u1, t1, 1, 1));
        } else if (randomValue > 0.3) {
            // Move down left
            return new ComputerMove(calculateMove(u1, t1, 1, 2));
        } else {
            // Move left
            return new ComputerMove(calculateMove(u1, t1, 0, 1));
        }
    }

    private static int calculateMove(int u, int t, int uChange, int tChange) {
        int newU = u + uChange;
        int newT = t + tChange;
        return 10 * newT + newU; // Combine units and tens to corresponding position value
    }


    private static void printBoard() {
        System.out.println();
        for (int A = 0; A <= 7; A++) {
            for (int B = 0; B <= 7; B++) {
                int I = 8 * A + B;
                System.out.printf(" %d ", S[I]);
            }
            System.out.println();
            System.out.println();
            System.out.println();
        }
        System.out.println();
    }

    private static void askAndShowInstructions() {
        do {
            System.out.print("DO YOU WANT INSTRUCTIONS? ");
            String wish = scanner.nextLine();
            if (wish.equals("NO")) {
                break;
            } else if (wish.equals("YES")) {
                showInstructions();
                break;
            } else {
                System.out.println("PLEASE ANSWER 'YES' OR 'NO'.");
            }
        } while (true);
    }

    private static void printWithTab(String message, int tab) {
        for (int i = 0; i < tab; i++) {
            System.out.print(" ");
        }
        System.out.println(message);
    }


    private static void showInstructions() {
        System.out.println("WE ARE GOING TO PLAY A GAME BASED ON ONE OF THE CHESS");
        System.out.println("MOVES.  OUR QUEEN WILL BE ABLE TO MOVE ONLY TO THE LEFT,");
        System.out.println("DOWN, OR DIAGONALLY DOWN AND TO THE LEFT.");
        System.out.println();
        System.out.println("THE OBJECT OF THE GAME IS TO PLACE THE QUEEN IN THE LOWER");
        System.out.println("LEFT HAND SQUARE BY ALTERNATING MOVES BETWEEN YOU AND THE");
        System.out.println("COMPUTER.  THE FIRST ONE TO PLACE THE QUEEN THERE WINS.");
        System.out.println();
        System.out.println("YOU GO FIRST AND PLACE THE QUEEN IN ANY ONE OF THE SQUARES");
        System.out.println("ON THE TOP ROW OR RIGHT HAND COLUMN.");
        System.out.println("THAT WILL BE YOUR FIRST MOVE.");
        System.out.println("WE ALTERNATE MOVES.");
        System.out.println("YOU MAY FORFEIT BY TYPING '0' AS YOUR MOVE.");
        System.out.println("BE SURE TO PRESS THE RETURN KEY AFTER EACH RESPONSE.");
        System.out.println();
    }


    private static class ComputerMove {
        private final int move;
        private final int u;
        private final int t;

        private ComputerMove(int move) {
            this.move = move;
            this.t = move / 10;
            this.u = move - (t * 10);
        }

    }

    private static class EvaluationResult {
        private final int move;
        private final boolean favourablePosition;

        public EvaluationResult(int move, boolean favourablePosition) {
            this.move = move;
            this.favourablePosition = favourablePosition;
        }


    }

    private static class Move {
        private final int move;
        private final int unitsPlaceValue;
        private final int tensPlaceValue;

        private Move(int move) {
            this.move = move;
            this.tensPlaceValue = move / 10;
            this.unitsPlaceValue = move % 10;
        }

    }
}
