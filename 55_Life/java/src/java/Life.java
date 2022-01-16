import java.util.ArrayList;
import java.util.List;
import java.util.Scanner;

record Transition(int y, int x, byte newState) { }

public class Life {

    private static final byte DEAD  = 0;
    private static final byte ALIVE = 1;

    private final byte[][] matrix = new byte[21][67];
    private int generation = 0;
    private int population = 0;
    boolean invalid = false;

    private void start() throws Exception {
        Scanner s = new Scanner(System.in);
        printGameHeader();
        readPattern();
        while (true) {
            printPattern();
            advanceToNextGeneration();
            s.nextLine();
//            Thread.sleep(1000);
        }
    }

    private void advanceToNextGeneration() {
        List<Transition> transitions = new ArrayList<>();
        for (int y = 0; y < matrix.length; y++) {
            for (int x = 0; x < matrix[y].length; x++) {
                int neighbours = countNeighbours(y, x);
                if (matrix[y][x] == DEAD) {
                    if (neighbours == 3) {
                        transitions.add(new Transition(y, x, ALIVE));
                        population++;
                    }
                } else {
                    // cell is alive
                    if (neighbours < 2 || neighbours > 3) {
                        transitions.add(new Transition(y, x, DEAD));
                        population--;
                    }
                }
            }
        }
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
        Scanner s = new Scanner(System.in);
        List<String> lines = new ArrayList<>();
        String line;
        int maxLineLength = 0;
        boolean reading = true;
        while (reading) {
            System.out.print("? ");
            line = s.nextLine();
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
        System.out.println("lines=" + lines.size() + " columns=" + maxLineLength + " yMin=" + yMin + " xMin=" + xMin);
        for (int y = 0; y < lines.size(); y++) {
            String line = lines.get(y);
            for (int x = 1; x <= line.length(); x++) {
                if (line.charAt(x-1) == '*') {
                    matrix[round(yMin + y)][round(xMin + x)] = ALIVE;
                    population++;
                }
            }
        }
    }

    private int round(float f) {
        return (int) Math.floor(f);
    }

    private void printGameHeader() {
        System.out.println(" ".repeat(34) + "LIFE");
        System.out.println(" ".repeat(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println("\n\n\n");
    }

    private void printPattern() {
        System.out.println("GENERATION: " + generation + "          POPULATION: " + population);
        for (int y = 0; y < matrix.length; y++) {
            for (int x = 0; x < matrix[y].length; x++) {
                System.out.print(matrix[y][x] == 1 ? "*" : " ");
            }
            System.out.println();
        }
    }

    public static void main(String[] args) throws Exception {
        new Life().start();
    }

}
