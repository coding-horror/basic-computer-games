using System.Text;

const int maxWidth = 70;
const int maxHeight = 24;

Console.WriteLine("ENTER YOUR PATTERN:");
var pattern = new Pattern(ReadPattern(limitHeight: maxHeight).ToArray());

var minX = 10 - pattern.Height / 2;
var minY = 34 - pattern.Width / 2;
var maxX = maxHeight - 1;
var maxY = maxWidth - 1;

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
            break;
        }

        // In the original version, BASIC would trim the spaces in the beginning of an input, so the original
        // game allowed you to input an '.' before the spaces to circumvent this limitation. This behavior was
        // kept for compatibility.
        if (input.StartsWith('.'))
            yield return input.Substring(1, input.Length - 1);

        yield return input;
    }
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

Simulation InitializeSimulation(Pattern pattern, Matrix matrixToInitialize) {
    var newSimulation = new Simulation();

    // transcribes the pattern to the middle of the simulation and counts initial population
    for (var x = 0; x < pattern.Height; x++)
    {
        for (var y = 0; y < pattern.Width; y++)
        {
            if (pattern.Content[x][y] == ' ')
                continue;

            matrixToInitialize[minX + x, minY + y] = CellState.Stable;
            newSimulation.IncreasePopulation();
        }
    }

    return newSimulation;
}

TimeSpan GetPauseBetweenIterations()
{
    if (args.Length != 2) return TimeSpan.Zero;

    var parameter = args[0].ToLower();
    if (parameter.Contains("wait"))
    {
        var value = args[1];
        if (int.TryParse(value, out var sleepMilliseconds))
            return TimeSpan.FromMilliseconds(sleepMilliseconds);
    }

    return TimeSpan.Zero;
}

void ProcessSimulation()
{
    var pauseBetweenIterations = GetPauseBetweenIterations();
    var isInvalid = false;

    while (true)
    {
        var invalidText = isInvalid ? "INVALID!" : "";
        Console.WriteLine($"GENERATION: {simulation.Generation}\tPOPULATION: {simulation.Population} {invalidText}");

        simulation.StartNewGeneration();

        var nextMinX = maxHeight - 1;
        var nextMinY = maxWidth - 1;
        var nextMaxX = 0;
        var nextMaxY = 0;

        var matrixOutput = new StringBuilder();

        // prints the empty lines before search area
        for (var x = 0; x < minX; x++)
        {
            matrixOutput.AppendLine();
        }

        // refreshes the matrix and updates search area
        for (var x = minX; x <= maxX; x++)
        {
            var printedLine = Enumerable.Repeat(' ', maxWidth).ToList();
            for (var y = minY; y <= maxY; y++)
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
                nextMaxX = Math.Max(x, nextMaxX);
                nextMinY = Math.Min(y, nextMinY);
                nextMaxY = Math.Max(y, nextMaxY);
            }

            matrixOutput.AppendLine(string.Join(separator: null, values: printedLine));
        }

        // prints empty lines after search area
        for (var x = maxX + 1; x < maxHeight; x++)
        {
            matrixOutput.AppendLine();
        }
        Console.Write(matrixOutput);

        void UpdateSearchArea()
        {
            minX = nextMinX;
            maxX = nextMaxX;
            minY = nextMinY;
            maxY = nextMaxY;

            const int limitX = 21;
            const int limitY = 67;

            if (minX < 2)
            {
                minX = 2;
                isInvalid = true;
            }

            if (maxX > limitX)
            {
                maxX = limitX;
                isInvalid = true;
            }

            if (minY < 2)
            {
                minY = 2;
                isInvalid = true;
            }

            if (maxY > limitY)
            {
                maxY = limitY;
                isInvalid = true;
            }
        }
        UpdateSearchArea();

        for (var x = minX - 1; x <= maxX + 1; x++)
        {
            for (var y = minY - 1; y <= maxY + 1; y++)
            {
                int CountNeighbors()
                {
                    var neighbors = 0;
                    for (var i = x - 1; i <= x + 1; i++)
                    {
                        for (var j = y - 1; j <= y + 1; j++)
                        {
                            if (matrix[i, j] == CellState.Stable || matrix[i, j] == CellState.Dying)
                                neighbors++;
                        }
                    }

                    return neighbors;
                }

                var neighbors = CountNeighbors();
                if (matrix[x, y] == CellState.Empty)
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

        if (pauseBetweenIterations > TimeSpan.Zero)
            Thread.Sleep(pauseBetweenIterations);
    }
}

public class Pattern
{
    public string[] Content { get; }
    public int Height { get; }
    public int Width { get; }

    public Pattern(IReadOnlyCollection<string> patternLines)
    {
        Height = patternLines.Count;
        Width = patternLines.Max(x => x.Length);
        Content = NormalizeWidth(patternLines);
    }

    private string[] NormalizeWidth(IReadOnlyCollection<string> patternLines)
    {
        return patternLines
            .Select(x => x.PadRight(Width, ' '))
            .ToArray();
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
