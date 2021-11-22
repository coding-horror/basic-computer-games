using System;
using System.Collections.Generic;

namespace Amazing
{
    class AmazingGame
    {
        private const int FIRST_COL = 0;
        private const int FIRST_ROW = 0;
        private const int EXIT_UNSET = 0;
        private const int EXIT_DOWN = 1;
        private const int EXIT_RIGHT = 2;

        private static int GetDelimitedValue(String text, int pos)
        {
            String[] tokens = text.Split(",");

            int val;
            if (Int32.TryParse(tokens[pos], out val))
            {
                return val;
            }
            return 0;
        }

        private static String Tab(int spaces)
        {
            return new String(' ', spaces);
        }

        public static int Random(int min, int max)
        {
            Random random = new Random();
            return random.Next(max - min) + min;
        }

        public void Play()
        {
            Console.WriteLine(Tab(28) + "AMAZING PROGRAM");
            Console.WriteLine(Tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
            Console.WriteLine();

            int width = 0;
            int length = 0;

            do
            {
                String range = DisplayTextAndGetInput("WHAT ARE YOUR WIDTH AND LENGTH");
                if (range.IndexOf(",") > 0)
                {
                    width = GetDelimitedValue(range, 0);
                    length = GetDelimitedValue(range, 1);
                }
            }
            while (width < 1 || length < 1);

            Grid grid = new Grid(length, width);
            int enterCol = grid.SetupEntrance();

            int totalWalls = width * length + 1;
            int count = 2;
            Cell cell = grid.StartingCell();

            while (count != totalWalls)
            {
                List<Direction> possibleDirs = GetPossibleDirs(grid, cell);

                if (possibleDirs.Count != 0)
                {
                    cell = SetCellExit(grid, cell, possibleDirs);
                    cell.Count = count++;
                }
                else
                {
                    cell = grid.GetFirstUnset(cell);
                }
            }
            grid.SetupExit();

            WriteMaze(width, grid, enterCol);
        }

        private Cell SetCellExit(Grid grid, Cell cell, List<Direction> possibleDirs)
        {
            Direction direction = possibleDirs[Random(0, possibleDirs.Count)];
            if (direction == Direction.GO_LEFT)
            {
                cell = grid.GetPrevCol(cell);
                cell.ExitType = EXIT_RIGHT;
            }
            else if (direction == Direction.GO_UP)
            {
                cell = grid.GetPrevRow(cell);
                cell.ExitType = EXIT_DOWN;
            }
            else if (direction == Direction.GO_RIGHT)
            {
                cell.ExitType = cell.ExitType + EXIT_RIGHT;
                cell = grid.GetNextCol(cell);
            }
            else if (direction == Direction.GO_DOWN)
            {
                cell.ExitType = cell.ExitType + EXIT_DOWN;
                cell = grid.GetNextRow(cell);
            }
            return cell;
        }

        private void WriteMaze(int width, Grid grid, int enterCol)
        {
            // top line
            for (int i = 0; i < width; i++)
            {
                if (i == enterCol) Console.Write(".  ");
                else Console.Write(".--");
            }
            Console.WriteLine(".");

            for (int i = 0; i < grid.Length; i++)
            {
                Console.Write("I");
                for (int j = 0; j < grid.Width; j++)
                {
                    if (grid.Cells[i,j].ExitType == EXIT_UNSET || grid.Cells[i, j].ExitType == EXIT_DOWN)
                        Console.Write("  I");
                    else Console.Write("   ");
                }
                Console.WriteLine();
                for (int j = 0; j < grid.Width; j++)
                {
                    if (grid.Cells[i,j].ExitType == EXIT_UNSET || grid.Cells[i, j].ExitType == EXIT_RIGHT)
                        Console.Write(":--");
                    else Console.Write(":  ");
                }
                Console.WriteLine(".");
            }
        }

        private List<Direction> GetPossibleDirs(Grid grid, Cell cell)
        {
            var possibleDirs = new List<Direction>();
            foreach (var val in Enum.GetValues(typeof(Direction)))
            {
                possibleDirs.Add((Direction)val);
            }

            if (cell.Col == FIRST_COL || grid.IsPrevColSet(cell))
            {
                possibleDirs.Remove(Direction.GO_LEFT);
            }
            if (cell.Row == FIRST_ROW || grid.IsPrevRowSet(cell))
            {
                possibleDirs.Remove(Direction.GO_UP);
            }
            if (cell.Col == grid.LastCol || grid.IsNextColSet(cell))
            {
                possibleDirs.Remove(Direction.GO_RIGHT);
            }
            if (cell.Row == grid.LastRow || grid.IsNextRowSet(cell))
            {
                possibleDirs.Remove(Direction.GO_DOWN);
            }
            return possibleDirs;
        }

        private String DisplayTextAndGetInput(String text)
        {
            Console.WriteLine(text);
            return Console.ReadLine();
        }


        private enum Direction
        {
            GO_LEFT,
            GO_UP,
            GO_RIGHT,
            GO_DOWN,
        }

        public class Cell
        {
            public int ExitType { get; set; }
            public int Count { get; set; }

            public int Col { get; set; }
            public int Row { get; set; }

            public Cell(int row, int col)
            {
                ExitType = EXIT_UNSET;
                Row = row;
                Col = col;
            }
        }


        public class Grid
        {
            public Cell[,] Cells { get; private set; }

            public int LastCol { get; set; }
            public int LastRow { get; set; }

            public int Width { get; private set; }
            public int Length { get; private set; }

            private int enterCol;

            public Grid(int length, int width)
            {
                LastCol = width - 1;
                LastRow = length - 1;
                Width = width;
                Length = length;

                Cells = new Cell[length,width];
                for (int i = 0; i < length; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        this.Cells[i,j] = new Cell(i, j);
                    }
                }
            }

            public int SetupEntrance()
            {
                this.enterCol = Random(0, Width);
                Cells[0, enterCol].Count = 1;
                return this.enterCol;
            }

            public void SetupExit()
            {
                int exit = Random(0, Width - 1);
                Cells[LastRow, exit].ExitType += 1;
            }

            public Cell StartingCell()
            {
                return Cells[0, enterCol];
            }

            public bool IsPrevColSet(Cell cell)
            {
                return 0 != Cells[cell.Row, cell.Col - 1].Count;
            }

            public bool IsPrevRowSet(Cell cell)
            {
                return 0 != Cells[cell.Row - 1, cell.Col].Count;
            }

            public bool IsNextColSet(Cell cell)
            {
                return 0 != Cells[cell.Row, cell.Col + 1].Count;
            }

            public bool IsNextRowSet(Cell cell)
            {
                return 0 != Cells[cell.Row + 1, cell.Col].Count;
            }

            public Cell GetPrevCol(Cell cell)
            {
                return Cells[cell.Row, cell.Col - 1];
            }

            public Cell GetPrevRow(Cell cell)
            {
                return Cells[cell.Row - 1, cell.Col];
            }

            public Cell GetNextCol(Cell cell)
            {
                return Cells[cell.Row, cell.Col + 1];
            }

            public Cell GetNextRow(Cell cell)
            {
                return Cells[cell.Row + 1, cell.Col];
            }

            public Cell GetFirstUnset(Cell cell)
            {
                int col = cell.Col;
                int row = cell.Row;
                Cell newCell;
                do
                {
                    if (col != this.LastCol)
                    {
                        col++;
                    }
                    else if (row != this.LastRow)
                    {
                        row++;
                        col = 0;
                    }
                    else
                    {
                        row = 0;
                        col = 0;
                    }
                }
                while ((newCell = Cells[row, col]).Count == 0);
                return newCell;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            new AmazingGame().Play();
        }
    }
}
