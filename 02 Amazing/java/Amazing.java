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


        Integer[][] used = new Integer[length][width];
        for (int i=0; i < length; i++) {
            used[i] = new Integer[width];
            for (int j = 0; j < width; j++) {
                used[i][j] = 0;
            }
        }

        Integer[][] walls = new Integer[length][width];
        for (int i=0; i < length; i++) {
            walls[i] = new Integer[width];
            for (int j = 0; j < width; j++) {
                walls[i][j] = 0;
            }
        }

        int EXIT_DOWN = 1;
        int EXIT_RIGHT = 2;

        int enterCol = random(0, width);
        int col = enterCol;
        int row = 0;
        int count = 1;
        int totalWalls = width * length + 1;

        // set up entrance
        used[row][col] = count;
        count++;

        while (count != totalWalls) {
            ArrayList<Direction> possibleDirs = new ArrayList<>(Arrays.asList(Direction.values()));

            if (col == 0 || used[row][col - 1] != 0) {
                possibleDirs.remove(Direction.GO_LEFT);
            }
            if (row == 0 || used[row - 1][col] != 0) {
                possibleDirs.remove(Direction.GO_UP);
            }
            if (col == width - 1 || used[row][col + 1] != 0) {
                possibleDirs.remove(Direction.GO_RIGHT);
            }
            if (row == length - 1 || used[row + 1][col] != 0) {
                possibleDirs.remove(Direction.GO_DOWN);
            }

            if (possibleDirs.size() != 0) {
                Direction direction = possibleDirs.get(random(0, possibleDirs.size()));
                if (direction == Direction.GO_LEFT) {
                    col = col - 1;
                    walls[row][col] = EXIT_RIGHT;
                } else if (direction == Direction.GO_UP) {
                    row = row - 1;
                    walls[row][col] = EXIT_DOWN;
                } else if (direction == Direction.GO_RIGHT) {
                    walls[row][col] = walls[row][col] + EXIT_RIGHT;
                    col = col + 1;
                } else if (direction == Direction.GO_DOWN) {
                    walls[row][col] = walls[row][col] + EXIT_DOWN;
                    row = row + 1;
                }
                used[row][col] = count;
                count++;
            } else {
                do {
                    if (col != width - 1) {
                        col++;
                    } else if (row != length - 1) {
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
                if (walls[i][j] < 2) {
                    out.print("  I");
                } else {
                    out.print("   ");
                }
            }
            out.println();
            for (int j = 0; j < width; j++) {
                if (walls[i][j] == 0 || walls[i][j] == 2) {
                    out.print(":--");
                } else {
                    out.print(":  ");
                }
            }
            out.println(".");
        }
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

