import java.io.PrintStream;
import java.util.HashMap;
import java.util.Map;
import java.util.Scanner;

public class HighIQ {

    private Map<Integer, Boolean> board;
    private PrintStream out;
    private Scanner scanner;

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

    public void play() {
        do {
            while (!move()) {
                System.out.println("ILLEGAL MOVE, TRY AGAIN...");
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

    public boolean move() {
        System.out.println("MOVE WHICH PIECE");
        int from = scanner.nextInt();

        //using the getOrDefault, which will make the statement false if it is an invalid position
        if (!board.getOrDefault(from, false)) {
            return false;
        }

        System.out.println("TO WHERE");
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

        return true;
    }

//     private boolean playAgain() {
//         out.println("PLAY AGAIN (YES OR NO)");
//         return scanner.nextLine().toLowerCase().equals("yes");
//     }

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
