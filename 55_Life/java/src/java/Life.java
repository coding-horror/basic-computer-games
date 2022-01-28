import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

/**
 * The Game of Life class.<br>
 * <br>
 * Mimics the behaviour of the BASIC version, however the Java code does not have much in common with the original.
 * <br>
 * Differences in behaviour:
 * <ul>
 *     <li>Input supports the "." character, but it's optional.</li>
 *     <li>Input regarding the "DONE" string is case insensitive.</li>
 * </ul>
 */
public class Life {

    private static final byte DEAD  = 0;
    private static final byte ALIVE = 1;
    private static final String NEWLINE = "\n";

    private final Scanner consoleReader = new Scanner(System.in);

    private final byte[][] matrix = new byte[21][67];
    private int generation = 0;
    private int population = 0;
    boolean stopAfterGen = false;
    boolean invalid = false;


    public Life(String[] args) {
        parse(args);
    }

    private void parse(String[] args) {
        for (String arg : args) {
            if ("-s".equals(arg)) {
                stopAfterGen = true;
                break;
            }
        }
    }

    private void start() {
        printGameHeader();
        readPattern();
        while (true) {
            printGeneration();
            advanceToNextGeneration();
            if (stopAfterGen) {
                consoleReader.nextLine();
            }
        }
    }

    private void advanceToNextGeneration() {
        // store all cell transitions in a list, i.e. if a dead cell becomes alive, or a living cell dies
        List<Transition> transitions = new ArrayList<>();
        // there's still room for optimization: instead of iterating over all cells in the matrix,
        // we could consider only the section containing the pattern(s), as in the BASIC version
        for (int y = 0; y < matrix.length; y++) {
            for (int x = 0; x < matrix[y].length; x++) {
                int neighbours = countNeighbours(y, x);
                if (matrix[y][x] == ALIVE) {
                    if (neighbours < 2 || neighbours > 3) {
                        transitions.add(new Transition(y, x, DEAD));
                        population--;
                    }
                } else { // cell is dead
                    if (neighbours == 3) {
                        if (x < 2 || x > 67 || y < 2 || y > 21) {
                            invalid = true;
                        }
                        transitions.add(new Transition(y, x, ALIVE));
                        population++;
                    }
                }
            }
        }
        // apply all transitions to the matrix
        transitions.forEach(t -> matrix[t.y()][t.x()] = t.newState());
        generation++;
    }

    private int countNeighbours(int y, int x) {
        int neighbours = 0;
        for (int row = Math.max(y - 1, 0); row <= Math.min(y + 1, matrix.length - 1); row++) {
            for (int col = Math.max(x - 1, 0); col <= Math.min(x + 1, matrix[row].length - 1); col++) {
                if (row == y && col == x) {
                    continue;
                }
                if (matrix[row][col] == ALIVE) {
                    neighbours++;
                }
            }
        }
        return neighbours;
    }

    private void readPattern() {
        System.out.println("ENTER YOUR PATTERN:");
        List<String> lines = new ArrayList<>();
        String line;
        int maxLineLength = 0;
        boolean reading = true;
        while (reading) {
            System.out.print("? ");
            line = consoleReader.nextLine();
            if (line.equalsIgnoreCase("done")) {
                reading = false;
            } else {
                // optional support for the '.' that is needed in the BASIC version
                lines.add(line.replace('.', ' '));
                maxLineLength = Math.max(maxLineLength, line.length());
            }
        }
        fillMatrix(lines, maxLineLength);
    }

    private void fillMatrix(List<String> lines, int maxLineLength) {
        float xMin = 33 - maxLineLength / 2f;
        float yMin = 11 - lines.size() / 2f;
        for (int y = 0; y < lines.size(); y++) {
            String line = lines.get(y);
            for (int x = 1; x <= line.length(); x++) {
                if (line.charAt(x-1) == '*') {
                    matrix[floor(yMin + y)][floor(xMin + x)] = ALIVE;
                    population++;
                }
            }
        }
    }

    private int floor(float f) {
        return (int) Math.floor(f);
    }

    private void printGameHeader() {
        printIndented(34, "LIFE");
        printIndented(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println(NEWLINE.repeat(3));
    }

    private void printIndented(int spaces, String str) {
        System.out.println(" ".repeat(spaces) + str);
    }

    private void printGeneration() {
        printGenerationHeader();
        for (int y = 0; y < matrix.length; y++) {
            for (int x = 0; x < matrix[y].length; x++) {
                System.out.print(matrix[y][x] == 1 ? "*" : " ");
            }
            System.out.println();
        }
    }

    private void printGenerationHeader() {
        String invalidText = invalid ? "INVALID!" : "";
        System.out.printf("GENERATION: %-13d POPULATION: %d %s\n", generation, population, invalidText);
    }

    /**
     * Main method that starts the program.
     *
     * @param args the command line arguments.
     * @throws Exception if something goes wrong.
     */
    public static void main(String[] args) throws Exception {
        new Life(args).start();
    }

}

/**
 * Represents a state change for a single cell within the matrix.
 *
 * @param y the y coordinate (row) of the cell
 * @param x the x coordinate (column) of the cell
 * @param newState the new state of the cell (either DEAD or ALIVE)
 */
record Transition(int y, int x, byte newState) { }
