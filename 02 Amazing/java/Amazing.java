import java.util.ArrayList;
import java.util.Arrays;
import java.util.Random;
import java.util.Scanner;
import static java.lang.System.in;
import static java.lang.System.out;

public class Amazing {

    private final Scanner kbScanner;

    public Amazing() {
        kbScanner = new Scanner(in);
    }

    enum Direction {
        GO_LEFT,
        GO_UP,
        GO_RIGHT,
        GO_DOWN,
    }

    final static int FIRST_COL = 0;
    final static int FIRST_ROW = 0;

    final static int EXIT_UNSET = 0;
    final static int EXIT_DOWN = 1;
    final static int EXIT_RIGHT = 2;

    public void play() {
        out.println(tab(28) + "AMAZING PROGRAM");
        out.println(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        out.println();

        int width = 0;
        int length = 0;

        do {
            String range = displayTextAndGetInput("WHAT ARE YOUR WIDTH AND LENGTH");
            if (range.indexOf(",") > 0) {
                width = getDelimitedValue(range, 0);
                length = getDelimitedValue(range, 1);
            }
        } while (width < 1 || length < 1);


        Integer[][] walls = buildGrid(width, length);
        Integer[][] used = buildGrid(width, length);

        int lastCol = width - 1;
        int lastRow = length - 1;

        int enterCol = random(0, width);
        int col = enterCol;
        int row = 0;
        int count = 1;
        int totalWalls = width * length + 1;

        // set up entrance
        used[row][col] = count;
        count++;

        while (count != totalWalls) {
            ArrayList<Direction> possibleDirs = getPossibleDirs(used, lastCol, lastRow, col, row);

            if (possibleDirs.size() != 0) {
                Direction direction = possibleDirs.get(random(0, possibleDirs.size()));
                if (direction == Direction.GO_LEFT) {
                    col--;
                    walls[row][col] = EXIT_RIGHT;
                } else if (direction == Direction.GO_UP) {
                    row--;
                    walls[row][col] = EXIT_DOWN;
                } else if (direction == Direction.GO_RIGHT) {
                    walls[row][col] = walls[row][col] + EXIT_RIGHT;
                    col++;
                } else if (direction == Direction.GO_DOWN) {
                    walls[row][col] = walls[row][col] + EXIT_DOWN;
                    row++;
                }
                used[row][col] = count;
                count++;
            } else {
                do {
                    if (col != lastCol) {
                        col++;
                    } else if (row != lastRow) {
                        row++;
                        col = 0;
                    } else {
                        row = 0;
                        col = 0;
                    }
                } while (used[row][col] == 0);
            }
        }

        col = random(0, width - 1);
        row = length - 1;
        walls[row][col] = walls[row][col] + 1;

        // top line
        for (int i=0; i < width; i++) {
            if (i == enterCol) {
                out.print(".  ");
            } else {
                out.print(".--");
            }
        }
        out.println('.');

        for (int i=0; i < length; i++) {
            out.print("I");
            for (int j = 0; j < width; j++) {
                if (walls[i][j] == EXIT_UNSET || walls[i][j] == EXIT_DOWN) {
                    out.print("  I");
                } else {
                    out.print("   ");
                }
            }
            out.println();
            for (int j = 0; j < width; j++) {
                if (walls[i][j] == EXIT_UNSET || walls[i][j] == EXIT_RIGHT) {
                    out.print(":--");
                } else {
                    out.print(":  ");
                }
            }
            out.println(".");
        }
    }

    private Integer[][] buildGrid(int width, int length) {
        Integer[][] grid = new Integer[length][width];
        for (int i=0; i < length; i++) {
            grid[i] = new Integer[width];
            for (int j = 0; j < width; j++) {
                grid[i][j] = EXIT_UNSET;
            }
        }
        return grid;
    }

    private ArrayList<Direction> getPossibleDirs(Integer[][] used, int lastCol, int lastRow, int col, int row) {
        ArrayList<Direction> possibleDirs = new ArrayList<>(Arrays.asList(Direction.values()));

        if (col == FIRST_COL || 0 != used[row][col - 1]) {
            possibleDirs.remove(Direction.GO_LEFT);
        }
        if (row == FIRST_ROW || 0 != used[row - 1][col]) {
            possibleDirs.remove(Direction.GO_UP);
        }
        if (col == lastCol || 0 != used[row][col + 1]) {
            possibleDirs.remove(Direction.GO_RIGHT);
        }
        if (row == lastRow || 0 != used[row + 1][col]) {
            possibleDirs.remove(Direction.GO_DOWN);
        }
        return possibleDirs;
    }

    private String displayTextAndGetInput(String text) {
        out.print(text);
        return kbScanner.next();
    }

    private static int getDelimitedValue(String text, int pos) {
        String[] tokens = text.split(",");
        try {
            return Integer.parseInt(tokens[pos]);
        } catch (Exception ex) {
            return 0;
        }
    }

    private static String tab(int spaces) {
        char[] spacesTemp = new char[spaces];
        Arrays.fill(spacesTemp, ' ');
        return new String(spacesTemp);
    }

    public static boolean random() {
        Random random = new Random();
        return random.nextBoolean();
    }

    public static int random(int min, int max) {
        Random random = new Random();
        return random.nextInt(max - min) + min;
    }
}

