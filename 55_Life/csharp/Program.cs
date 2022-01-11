// See https://aka.ms/new-console-template for more information

using System.Text;

IEnumerable<string> ReadPattern(int limitHeight)
{
    for (var i = 0; i < limitHeight; i++)
    {
        var input = Console.ReadLine();
        if (input.ToUpper() == "DONE")
        {
            yield return string.Empty;
            break;
        }

        // kept for compatibility
        if (input.StartsWith('.'))
            yield return input.Substring(1, input.Length - 2);

        yield return input;
    }
}

void PrintHeader()
{
    const int pageWidth = 64;

    void PrintCentered(string text)
    {
        var spaceCount = (pageWidth - text.Length) / 2;
        Console.Write(new string(' ', spaceCount));
        Console.WriteLine(text);
    }
    
    PrintCentered("LIFE");
    PrintCentered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();    
}






Console.WriteLine("ENTER YOUR PATTERN:");
// var pattern = ReadPattern(limitHeight: MaxHeight).ToArray();
var pattern = new[]
{
    "*", 
    "*", 
    "*" 
}; // FOR DEBUGGING PURPOSES
// var pattern = new[]
// {
//     "**", 
//     "**", 
// }; // FOR DEBUGGING PURPOSES
// var pattern = new[]
// {
//     "***", 
//     "*", 
// }; // FOR DEBUGGING PURPOSES


const int MaxWidth = 70;
const int MaxHeight = 24;

var isInvalid = false;

var (index, value) = GetLongestInput(pattern);
int minX = (11 - index / 2) - 1;              // middle x
int minY = (33 - value.Length / 2) - 1;       // middle y
int maxX = MaxHeight;
int maxY = MaxWidth;
var matrix = new Matrix(height: MaxHeight, width: MaxWidth);
var simulation = InitializeSimulation(pattern, matrix);

PrintHeader();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
ProcessGeneration();


Simulation InitializeSimulation(IReadOnlyList<string> inputPattern, Matrix matrixToInitialize) {
    var newSimulation = new Simulation();

    for (var x = 0; x < inputPattern.Count; x++)
    {
        for (var y = 0; y < inputPattern[x].Length; y++)
        {
            if (inputPattern[x][y] == ' ') continue;
            
            matrixToInitialize[minX + x, minY + y] = 1; // copy the pattern to the middle of the simulation
            newSimulation.IncreasePopulation();
        }
    }

    return newSimulation;
}

void ProcessGeneration()
{
    while (true)
    {
        void PrintPopulation(int generation, int population)
        {
            Console.WriteLine($"GENERATION: {generation}\tPOPULATION: {population}");
            if (isInvalid)
                Console.WriteLine("INVALID!");
        }

        PrintPopulation(simulation.Generation, simulation.Population);

        simulation.StartNewGeneration();

        // LINE 215
        var nextMinX = MaxHeight - 1; 
        var nextMinY = MaxWidth - 1;
        var nextMaxX = 0; 
        var nextMaxY = 0; 

        // prints empty lines before alive cells
        for (var x = 0; x < minX; x++)
        {
            Console.WriteLine();
        }

        // refreshes the matrix and updates search area 
        for (var x = minX; x < maxX; x++)
        {
            var printedLine = Enumerable.Repeat(' ', MaxWidth).ToList();
            for (var y = minY; y < maxY; y++)
            {
                if (matrix[x, y] == 2)
                {
                    matrix[x, y] = 0;
                    continue;
                }
                if (matrix[x, y] == 3)
                {
                    matrix[x, y] = 1;
                }
                else if (matrix[x, y] != 1)
                {
                    continue;
                }

                printedLine[y] = '*';

                nextMinX = Math.Min(x, nextMinX);
                nextMaxX = Math.Max(x + 1, nextMaxX);
                nextMinY = Math.Min(y, nextMinY);
                nextMaxY = Math.Max(y + 1, nextMaxY);
            }

            Console.WriteLine(string.Join(separator: null, values: printedLine));
        }

        // prints empty lines after alive cells
        for (int x = maxX + 1; x < MaxHeight; x++)
        {
            Console.WriteLine();
        }
        Console.WriteLine();

        minX = nextMinX;
        maxX = nextMaxX;
        minY = nextMinY;
        maxY = nextMaxY;

        // TODO boundaries? review
        if (minX < 3)
        {
            minX = 3;
            isInvalid = true;
        }

        if (maxX > 22)
        {
            maxX = 22;
            isInvalid = true;
        }

        if (minY < 3)
        {
            minY = 3;
            isInvalid = true;
        }

        if (maxY > 68)
        {
            maxY = 68;
            isInvalid = true;
        }

        for (var x = minX - 1; x < maxX + 2; x++)
        {
            for (var y = minY - 1; y < maxY + 2; y++)
            {
                int CountNeighbors()
                {
                    var neighbors = 0;
                    for (var i = x - 1; i < x + 2; i++)
                    {
                        for (var j = y - 1; j < y + 2; j++)
                        {
                            if (matrix[i, j] == 1 || matrix[i, j] == 2)
                                neighbors++;
                        }
                    }

                    return neighbors;
                }

                var neighbors = CountNeighbors();
                if (matrix[x, y] == 0)
                {
                    if (neighbors == 3)
                    {
                        matrix[x, y] = 3;
                        simulation.IncreasePopulation();
                    }
                }
                else if (neighbors is < 3 or > 4)
                {
                    matrix[x, y] = 2;
                }
                else
                {
                    simulation.IncreasePopulation();
                }
            }
        }

        // expands search area to accommodate new cells 
        minX--;
        minY--;
        maxX++;
        maxY++;
    }
}

(int index, string value) GetLongestInput(IEnumerable<string> strings)
{
    return strings
        .Select((value, index) => (index, value))
        .OrderByDescending(input => input.value.Length)
        .First();
}

public class Simulation
{
    public int Generation { get; private set; }

    public int Population { get; private set; }

    public void StartNewGeneration()
    {
        Generation++;
        Population = 0;
    }

    public void IncreasePopulation()
    {
        Population++;
    }
}

class Matrix
{
    private readonly int[,] _matrix;

    public Matrix(int height, int width)
    {
        _matrix = new int[height, width];
    }

    public int this[int x, int y]
    {
        get => _matrix[x, y];
        set => _matrix[x, y] = value;
    }
    
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        for (var x = 0; x < _matrix.GetLength(0); x++)
        {
            for (var y = 0; y < _matrix.GetLength(1); y++)
            {
                var character = _matrix[x, y] == 0 ? " ": _matrix[x, y].ToString();
                stringBuilder.Append(character);
            }

            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }
}