using System.Text;

const int maxWidth = 70;
const int maxHeight = 24;

Console.WriteLine("ENTER YOUR PATTERN:");
var pattern = ReadPattern(limitHeight: maxHeight).ToArray();

var (minX, minY) = FindTopLeftCorner(pattern);
var maxX = maxHeight;
var maxY = maxWidth;

var matrix = new Matrix(height: maxHeight, width: maxWidth);
var simulation = InitializeSimulation(pattern, matrix);

PrintHeader();
ProcessSimulation();

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

        // In the original version, BASIC would trim the spaces in the beginning of an input, so the original
        // game allowed you to input an '.' before the spaces to circumvent this limitation. This behavior was
        // kept for compatibility.
        if (input.StartsWith('.'))
            yield return input.Substring(1, input.Length - 2);

        yield return input;
    }
}

(int minX, int minY) FindTopLeftCorner(IEnumerable<string> patternLines)
{
    var longestInput = patternLines
        .Select((value, index) => (index, value))
        .OrderByDescending(input => input.value.Length)
        .First();
    var centerX = (11 - longestInput.index / 2) - 1;
    var centerY = (33 - longestInput.value.Length / 2) - 1;
    return (centerX, centerY);
}

void PrintHeader()
{
    void PrintCentered(string text)
    {
        const int pageWidth = 64;

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

Simulation InitializeSimulation(IReadOnlyList<string> inputPattern, Matrix matrixToInitialize) {
    var newSimulation = new Simulation();

    // translates the pattern to the middle of the simulation and counts initial population
    for (var x = 0; x < inputPattern.Count; x++)
    {
        for (var y = 0; y < inputPattern[x].Length; y++)
        {
            if (inputPattern[x][y] == ' ') continue;
            
            matrixToInitialize[minX + x, minY + y] = CellState.Stable;
            newSimulation.IncreasePopulation();
        }
    }

    return newSimulation;
}

TimeSpan GetPauseBetweenIterations()
{
    if (args.Length == 2)
    {
        var parameter = args[0].ToLower();
        if (parameter.Contains("wait"))
        {
            var value = args[1];
            if (int.TryParse(value, out var sleepMilliseconds))
                return TimeSpan.FromMilliseconds(sleepMilliseconds);
        }
    }

    return TimeSpan.Zero;
}

void ProcessSimulation()
{
    var pauseBetweenIterations = GetPauseBetweenIterations();
    var isInvalid = false;
    
    while (true)
    {
        if (pauseBetweenIterations > TimeSpan.Zero)
            Thread.Sleep(pauseBetweenIterations);
        
        Console.WriteLine($"GENERATION: {simulation.Generation}\tPOPULATION: {simulation.Population}");
        if (isInvalid)
            Console.WriteLine("INVALID!");
        
        simulation.StartNewGeneration();

        var nextMinX = maxHeight - 1; 
        var nextMinY = maxWidth - 1;
        var nextMaxX = 0; 
        var nextMaxY = 0; 

        // prints the empty lines before search area
        for (var x = 0; x < minX; x++)
        {
            Console.WriteLine();
        }

        // refreshes the matrix and updates search area 
        for (var x = minX; x < maxX; x++)
        {
            var printedLine = Enumerable.Repeat(' ', maxWidth).ToList();
            for (var y = minY; y < maxY; y++)
            {
                if (matrix[x, y] == CellState.Dying)
                {
                    matrix[x, y] = CellState.Empty;
                    continue;
                }
                if (matrix[x, y] == CellState.New)
                {
                    matrix[x, y] = CellState.Stable;
                }
                else if (matrix[x, y] != CellState.Stable)
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

        // prints empty lines after search area
        for (var x = maxX + 1; x < maxHeight; x++)
        {
            Console.WriteLine();
        }
        Console.WriteLine();

        void UpdateSearchArea()
        {
            minX = nextMinX;
            maxX = nextMaxX;
            minY = nextMinY;
            maxY = nextMaxY;

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
        }
        UpdateSearchArea();

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
                            if (matrix[i, j] == CellState.Stable || matrix[i, j] == CellState.Dying)
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
                        matrix[x, y] = CellState.New;
                        simulation.IncreasePopulation();
                    }
                }
                else if (neighbors is < 3 or > 4)
                {
                    matrix[x, y] = CellState.Dying;
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

/// <summary>
/// Indicates the state of a given cell in the simulation.
/// </summary>
internal enum CellState
{
    Empty = 0,
    Stable = 1,
    Dying = 2,
    New = 3
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

/// <summary>
/// This class was created to aid debugging, through the implementation of the ToString() method.
/// </summary>
class Matrix
{
    private readonly CellState[,] _matrix;

    public Matrix(int height, int width)
    {
        _matrix = new CellState[height, width];
    }

    public CellState this[int x, int y]
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
                var character = _matrix[x, y] == 0 ? " ": ((int)_matrix[x, y]).ToString();
                stringBuilder.Append(character);
            }

            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }
}