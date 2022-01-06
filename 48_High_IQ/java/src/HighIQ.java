import java.io.PrintStream;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

/**
 * Game of HighIQ
 * <p>
 * Based on the Basic Game of HighIQ Here:
 * https://github.com/coding-horror/basic-computer-games/blob/main/48_High_IQ/highiq.bas
 *
 * No additional functionality has been added
 */
public class HighIQ {

    //Game board, as a map of position numbers to their values
    private final Map<Integer, Boolean> board;

    //Output stream
    private final PrintStream out;

    //Input scanner to use
    private final Scanner scanner;


    public HighIQ(Scanner scanner) {
        out = System.out;
        this.scanner = scanner;
        board = new HashMap<>();

        //Set of all locations to put initial pegs on
        int[] locations = new int[]{
                13, 14, 15, 22, 23, 24, 29, 30, 31, 32, 33, 34, 35, 38, 39, 40, 42, 43, 44, 47, 48, 49, 50, 51, 52, 53, 58, 59, 60, 67, 68, 69
        };

        for (int i : locations) {
            board.put(i, true);
        }

        board.put(41, false);
    }

    /**
     * Plays the actual game, from start to finish.
     */
    public void play() {
        do {
            printBoard();
            while (!move()) {
                out.println("ILLEGAL MOVE, TRY AGAIN...");
            }
        } while (!isGameFinished());

        int pegCount = 0;
        for (Integer key : board.keySet()) {
            if (board.getOrDefault(key, false)) {
                pegCount++;
            }
        }

        out.println("YOU HAD " + pegCount + " PEGS REMAINING");

        if (pegCount == 1) {
            out.println("BRAVO!  YOU MADE A PERFECT SCORE!");
            out.println("SAVE THIS PAPER AS A RECORD OF YOUR ACCOMPLISHMENT!");
        }
    }

    /**
     * Makes an individual move
     * @return True if the move was valid, false if the user made an error and the move is invalid
     */
    public boolean move() {
        out.println("MOVE WHICH PIECE");
        int from = scanner.nextInt();

        //using the getOrDefault, which will make the statement false if it is an invalid position
        if (!board.getOrDefault(from, false)) {
            return false;
        }

        out.println("TO WHERE");
        int to = scanner.nextInt();

        if (board.getOrDefault(to, true)) {
            return false;
        }

        //Do nothing if they are the same
        if (from == to) {
            return true;
        }

        //using the difference to check if the relative locations are valid
        int difference = Math.abs(to - from);
        if (difference != 2 && difference != 18) {
            return false;
        }

        //check if there is a peg between from and to
        if (!board.getOrDefault((to + from) / 2, false)) {
            return false;
        }

        //Actually move
        board.put(from,false);
        board.put(to,true);
        board.put((from + to) / 2, false);

        return true;
    }

    /**
     * Checks if the game is finished
     * @return True if there are no more moves, False otherwise
     */
    public boolean isGameFinished() {
        for (Integer key : board.keySet()) {
            if (board.get(key)) {
                //Spacing is either 1 or 9
                //Looking to the right and down from every point, checking for both directions of movement
                for (int space : new int[]{1, 9}) {
                    Boolean nextToPeg = board.getOrDefault(key + space, false);
                    Boolean hasMovableSpace = !board.getOrDefault(key - space, true) || !board.getOrDefault(key + space * 2, true);
                    if (nextToPeg && hasMovableSpace) {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void printBoard() {
        for (int i = 0; i < 7; i++) {
            for (int j = 11; j < 18; j++) {
                out.print(getChar(j + 9 * i));
            }
            out.println();
        }
    }

    private char getChar(int position) {
        Boolean value = board.get(position);
        if (value == null) {
            return ' ';
        } else if (value) {
            return '!';
        } else {
            return 'O';
        }
    }
}
