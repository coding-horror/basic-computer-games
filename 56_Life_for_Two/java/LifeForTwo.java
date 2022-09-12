import java.util.*;
import java.util.stream.IntStream;

/**
 * Life for Two
 * <p>
 * The original BASIC program uses a grid with an extras border of cells all around,
 * probably to simplify calculations and manipulations. This java program has the exact
 * grid size and instead uses boundary check conditions in the logic.
 * <p>
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 */
public class LifeForTwo {

    final static int GRID_SIZE = 5;

    //Pair of offset which when added to the current cell's coordinates,
    // give the coordinates of the neighbours
    final static int[] neighbourCellOffsets = {
            -1, 0,
            1, 0,
            0, -1,
            0, 1,
            -1, -1,
            1, -1,
            -1, 1,
            1, 1
    };

    //The best term that I could come with to describe these numbers was 'masks'
    //They act like indicators to decide which player won the cell. The value is the score of the cell after all the
    // generation calculations.
    final static List<Integer> maskPlayer1 = List.of(3, 102, 103, 120, 130, 121, 112, 111, 12);
    final static List<Integer> maskPlayer2 = List.of(21, 30, 1020, 1030, 1011, 1021, 1003, 1002, 1012);

    public static void main(String[] args) {
        printIntro();
        Scanner scan = new Scanner(System.in);
        scan.useDelimiter("\\D");

        int[][] grid = new int[GRID_SIZE][GRID_SIZE];

        initializeGrid(grid);

        //Read the initial 3 moves for each player
        for (int b = 1; b <= 2; b++) {
            System.out.printf("\nPLAYER %d - 3 LIVE PIECES.%n", b);
            for (int k1 = 1; k1 <= 3; k1++) {
                var player1Coordinates = readUntilValidCoordinates(scan, grid);
                grid[player1Coordinates.x - 1][player1Coordinates.y - 1] = (b == 1 ? 3 : 30);
            }
        }

        printGrid(grid);

        calculatePlayersScore(grid); //Convert 3, 30 to 100, 1000

        resetGridForNextGen(grid);
        computeCellScoresForOneGen(grid);

        var playerScores = calculatePlayersScore(grid);
        resetGridForNextGen(grid);

        boolean gameOver = false;
        while (!gameOver) {
            printGrid(grid);
            if (playerScores.getPlayer1Score() == 0 && playerScores.getPlayer2Score() == 0) {
                System.out.println("\nA DRAW");
                gameOver = true;
            } else if (playerScores.getPlayer2Score() == 0) {
                System.out.println("\nPLAYER 1 IS THE WINNER");
                gameOver = true;
            } else if (playerScores.getPlayer1Score() == 0) {
                System.out.println("\nPLAYER 2 IS THE WINNER");
                gameOver = true;
            } else {
                System.out.print("PLAYER 1 ");
                Coordinate player1Move = readCoordinate(scan);
                System.out.print("PLAYER 2 ");
                Coordinate player2Move = readCoordinate(scan);
                if (!player1Move.equals(player2Move)) {
                    grid[player1Move.x - 1][player1Move.y - 1] = 100;
                    grid[player2Move.x - 1][player2Move.y - 1] = 1000;
                }
                //In the original, B is assigned 99 when both players choose the same cell
                //and that is used to control the flow
                computeCellScoresForOneGen(grid);
                playerScores = calculatePlayersScore(grid);
                resetGridForNextGen(grid);
            }
        }

    }

    private static void initializeGrid(int[][] grid) {
        for (int[] row : grid) {
            Arrays.fill(row, 0);
        }
    }

    private static void computeCellScoresForOneGen(int[][] grid) {
        for (int i = 0; i < GRID_SIZE; i++) {
            for (int j = 0; j < GRID_SIZE; j++) {
                if (grid[i][j] >= 100) {
                    calculateScoreForOccupiedCell(grid, i, j);
                }
            }
        }
    }

    private static Scores calculatePlayersScore(int[][] grid) {
        int m2 = 0;
        int m3 = 0;
        for (int i = 0; i < GRID_SIZE; i++) {
            for (int j = 0; j < GRID_SIZE; j++) {
                if (grid[i][j] < 3) {
                    grid[i][j] = 0;
                } else {
                    if (maskPlayer1.contains(grid[i][j])) {
                        m2++;
                    } else if (maskPlayer2.contains(grid[i][j])) {
                        m3++;
                    }
                }
            }
        }
        return new Scores(m2, m3);
    }

    private static void resetGridForNextGen(int[][] grid) {
        for (int i = 0; i < GRID_SIZE; i++) {
            for (int j = 0; j < GRID_SIZE; j++) {
                if (grid[i][j] < 3) {
                    grid[i][j] = 0;
                } else {
                    if (maskPlayer1.contains(grid[i][j])) {
                        grid[i][j] = 100;
                    } else if (maskPlayer2.contains(grid[i][j])) {
                        grid[i][j] = 1000;
                    } else {
                        grid[i][j] = 0;
                    }
                }
            }
        }
    }

    private static void calculateScoreForOccupiedCell(int[][] grid, int i, int j) {
        var b = 1;
        if (grid[i][j] > 999) {
            b = 10;
        }
        for (int k = 0; k < 15; k += 2) {
            //check bounds
            var neighbourX = i + neighbourCellOffsets[k];
            var neighbourY = j + neighbourCellOffsets[k + 1];
            if (neighbourX >= 0 && neighbourX < GRID_SIZE &&
                    neighbourY >= 0 && neighbourY < GRID_SIZE) {
                grid[neighbourX][neighbourY] = grid[neighbourX][neighbourY] + b;
            }

        }
    }

    private static void printGrid(int[][] grid) {
        System.out.println();
        printRowEdge();
        System.out.println();
        for (int i = 0; i < grid.length; i++) {
            System.out.printf("%d ", i + 1);
            for (int j = 0; j < grid[i].length; j++) {
                System.out.printf(" %c ", mapChar(grid[i][j]));
            }
            System.out.printf(" %d", i + 1);
            System.out.println();
        }
        printRowEdge();
        System.out.println();
    }

    private static void printRowEdge() {
        System.out.print("0 ");
        IntStream.range(1, GRID_SIZE + 1).forEach(i -> System.out.printf(" %s ", i));
        System.out.print(" 0");
    }

    private static char mapChar(int i) {
        if (i == 3 || i == 100) {
            return '*';
        }
        if (i == 30 || i == 1000) {
            return '#';
        }
        return ' ';
    }

    private static Coordinate readUntilValidCoordinates(Scanner scanner, int[][] grid) {
        boolean coordinateInRange = false;
        Coordinate coordinate = null;
        while (!coordinateInRange) {
            coordinate = readCoordinate(scanner);
            if (coordinate.x <= 0 || coordinate.x > GRID_SIZE
                    || coordinate.y <= 0 || coordinate.y > GRID_SIZE
                    || grid[coordinate.x - 1][coordinate.y - 1] != 0) {
                System.out.println("ILLEGAL COORDS. RETYPE");
            } else {
                coordinateInRange = true;
            }
        }
        return coordinate;
    }

    private static Coordinate readCoordinate(Scanner scanner) {
        Coordinate coordinate = null;
        int x, y;
        boolean valid = false;

        System.out.println("X,Y");
        System.out.print("XXXXXX\r");
        System.out.print("$$$$$$\r");
        System.out.print("&&&&&&\r");

        while (!valid) {
            try {
                System.out.print("? ");
                y = scanner.nextInt();
                x = scanner.nextInt();
                valid = true;
                coordinate = new Coordinate(x, y);
            } catch (InputMismatchException e) {
                System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE");
                valid = false;
            } finally {
                scanner.nextLine();
            }
        }
        return coordinate;
    }

    private static void printIntro() {
        System.out.println("                                LIFE2");
        System.out.println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println("\n\n");

        System.out.println("\tU.B. LIFE GAME");
    }

    private static class Coordinate {
        private final int x, y;

        public Coordinate(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public int getX() {
            return x;
        }

        public int getY() {
            return y;
        }

        @Override
        public String toString() {
            return "Coordinate{" +
                    "x=" + x +
                    ", y=" + y +
                    '}';
        }

        @Override
        public boolean equals(Object o) {
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;
            Coordinate that = (Coordinate) o;
            return x == that.x && y == that.y;
        }

        @Override
        public int hashCode() {
            return Objects.hash(x, y);
        }
    }

    private static class Scores {
        private final int player1Score;
        private final int player2Score;

        public Scores(int player1Score, int player2Score) {
            this.player1Score = player1Score;
            this.player2Score = player2Score;
        }

        public int getPlayer1Score() {
            return player1Score;
        }

        public int getPlayer2Score() {
            return player2Score;
        }
    }


}
