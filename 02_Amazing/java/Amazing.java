import java.util.ArrayList;
import java.util.Arrays;
import java.util.Random;
import java.util.Scanner;
import static java.lang.System.in;
import static java.lang.System.out;

/**
 * Core algorithm copied from amazing.py
 */
public class Amazing {

    final static int FIRST_COL = 0;
    final static int FIRST_ROW = 0;
    final static int EXIT_UNSET = 0;
    final static int EXIT_DOWN = 1;
    final static int EXIT_RIGHT = 2;
    private final Scanner kbScanner;
    public Amazing() {
        kbScanner = new Scanner(in);
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

    public static int random(int min, int max) {
        Random random = new Random();
        return random.nextInt(max - min) + min;
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

        Grid grid = new Grid(length, width);
        int enterCol = grid.setupEntrance();

        int totalWalls = width * length + 1;
        int count = 2;
        Cell cell = grid.startingCell();

        while (count != totalWalls) {
            ArrayList<Direction> possibleDirs = getPossibleDirs(grid, cell);

            if (possibleDirs.size() != 0) {
                cell = setCellExit(grid, cell, possibleDirs);
                cell.count = count++;
            } else {
                cell = grid.getFirstUnset(cell);
            }
        }
        grid.setupExit();

        writeMaze(width, grid, enterCol);
    }

    private Cell setCellExit(Grid grid, Cell cell, ArrayList<Direction> possibleDirs) {
        Direction direction = possibleDirs.get(random(0, possibleDirs.size()));
        if (direction == Direction.GO_LEFT) {
            cell = grid.getPrevCol(cell);
            cell.exitType = EXIT_RIGHT;
        } else if (direction == Direction.GO_UP) {
            cell = grid.getPrevRow(cell);
            cell.exitType = EXIT_DOWN;
        } else if (direction == Direction.GO_RIGHT) {
            cell.exitType = cell.exitType + EXIT_RIGHT;
            cell = grid.getNextCol(cell);
        } else if (direction == Direction.GO_DOWN) {
            cell.exitType = cell.exitType + EXIT_DOWN;
            cell = grid.getNextRow(cell);
        }
        return cell;
    }

    private void writeMaze(int width, Grid grid, int enterCol) {
        // top line
        for (int i = 0; i < width; i++) {
            if (i == enterCol) {
                out.print(".  ");
            } else {
                out.print(".--");
            }
        }
        out.println('.');

        for (Cell[] rows : grid.cells) {
            out.print("I");
            for (Cell cell : rows) {
                if (cell.exitType == EXIT_UNSET || cell.exitType == EXIT_DOWN) {
                    out.print("  I");
                } else {
                    out.print("   ");
                }
            }
            out.println();
            for (Cell cell : rows) {
                if (cell.exitType == EXIT_UNSET || cell.exitType == EXIT_RIGHT) {
                    out.print(":--");
                } else {
                    out.print(":  ");
                }
            }
            out.println(".");
        }
    }

    private ArrayList<Direction> getPossibleDirs(Grid grid, Cell cell) {
        ArrayList<Direction> possibleDirs = new ArrayList<>(Arrays.asList(Direction.values()));

        if (cell.col == FIRST_COL || grid.isPrevColSet(cell)) {
            possibleDirs.remove(Direction.GO_LEFT);
        }
        if (cell.row == FIRST_ROW || grid.isPrevRowSet(cell)) {
            possibleDirs.remove(Direction.GO_UP);
        }
        if (cell.col == grid.lastCol || grid.isNextColSet(cell)) {
            possibleDirs.remove(Direction.GO_RIGHT);
        }
        if (cell.row == grid.lastRow || grid.isNextRowSet(cell)) {
            possibleDirs.remove(Direction.GO_DOWN);
        }
        return possibleDirs;
    }

    private String displayTextAndGetInput(String text) {
        out.print(text);
        return kbScanner.next();
    }

    enum Direction {
        GO_LEFT,
        GO_UP,
        GO_RIGHT,
        GO_DOWN,
    }

    public static class Cell {
        int exitType = EXIT_UNSET;
        int count = 0;

        int col;
        int row;

        public Cell(int row, int col) {
            this.row = row;
            this.col = col;
        }
    }

    public static class Grid {
        Cell[][] cells;

        int lastCol;
        int lastRow;

        int width;
        int enterCol;

        public Grid(int length, int width) {
            this.lastCol = width - 1;
            this.lastRow = length - 1;
            this.width = width;

            this.cells = new Cell[length][width];
            for (int i = 0; i < length; i++) {
                this.cells[i] = new Cell[width];
                for (int j = 0; j < width; j++) {
                    this.cells[i][j] = new Cell(i, j);
                }
            }
        }

        public int setupEntrance() {
            this.enterCol = random(0, this.width);
            cells[0][this.enterCol].count = 1;
            return this.enterCol;
        }

        public void setupExit() {
            int exit = random(0, width - 1);
            cells[lastRow][exit].exitType += 1;
        }

        public Cell startingCell() {
            return cells[0][enterCol];
        }

        public boolean isPrevColSet(Cell cell) {
            return 0 != cells[cell.row][cell.col - 1].count;
        }

        public boolean isPrevRowSet(Cell cell) {
            return 0 != cells[cell.row - 1][cell.col].count;
        }

        public boolean isNextColSet(Cell cell) {
            return 0 != cells[cell.row][cell.col + 1].count;
        }

        public boolean isNextRowSet(Cell cell) {
            return 0 != cells[cell.row + 1][cell.col].count;
        }

        public Cell getPrevCol(Cell cell) {
            return cells[cell.row][cell.col - 1];
        }

        public Cell getPrevRow(Cell cell) {
            return cells[cell.row - 1][cell.col];
        }

        public Cell getNextCol(Cell cell) {
            return cells[cell.row][cell.col + 1];
        }

        public Cell getNextRow(Cell cell) {
            return cells[cell.row + 1][cell.col];
        }

        public Cell getFirstUnset(Cell cell) {
            int col = cell.col;
            int row = cell.row;
            Cell newCell;
            do {
                if (col != this.lastCol) {
                    col++;
                } else if (row != this.lastRow) {
                    row++;
                    col = 0;
                } else {
                    row = 0;
                    col = 0;
                }
            } while ((newCell = cells[row][col]).count == 0);
            return newCell;
        }
    }
}