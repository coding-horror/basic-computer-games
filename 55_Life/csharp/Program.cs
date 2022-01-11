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

(int index, string value) GetLongestInput(IEnumerable<string> strings)
{
    return strings
        .Select((value, index) => (index, value))
        .OrderByDescending(input => input.value.Length)
        .First();
}

try
{



PrintHeader();

Console.WriteLine("ENTER YOUR PATTERN:");
// var pattern = ReadPattern(limitHeight: MaxHeight).ToArray();
var pattern = new[]
{
    "*", 
    "*", 
    "*" 
}; // FOR DEBUGGING PURPOSES
var (index, value) = GetLongestInput(pattern);
Console.WriteLine("" + index + ", " + value);

// B = pattern


const int MaxWidth = 70; // Y2
const int MaxHeight = 24; // X2

// var matrix = new int[24, 70]; // TODO understand it
var matrixSpace = new MatrixSpace(height: 24, width: 70);
// int population = 0; 
var isInvalid = false; // TODO understand



int minX = (11 - index / 2) - 1;              // middle x
int minY = (33 - value.Length / 2) - 1;       // middle y
int maxX = MaxHeight;
int maxY = MaxWidth;
var simulation = InitializeSimulation(pattern, matrixSpace);



Simulation InitializeSimulation(string[] inputPattern, MatrixSpace matrixToInitialize) {
    var newSimulation = new Simulation();

    for (var x = 0; x < inputPattern.Length; x++)
    {
        for (var y = 0; y < inputPattern[x].Length; y++)
        {
            if (inputPattern[x][y] != ' ')
            {
                matrixToInitialize.Matrix[minX + x, minY + y] = 1; // copy the pattern to the middle of the simulation
                // population++; // increments the population
                newSimulation.IncreasePopulation();
            }
        }
    }

    return newSimulation;
}


// PrintMatrix(matrixSpace.Matrix);
void PrintMatrix(int[,] matrix)
{
    Console.WriteLine("Matrix:");
    for (int x = 0; x < matrix.GetLength(0); x++)
    {
        for (int y = 0; y < matrix.GetLength(1); y++)
        {
            var character = matrix[x, y] == 0 ? ' ' : '*';
            Console.Write(character);
        }
        Console.WriteLine();
    }
}

    void ProcessGeneration()
    {
        var matrix = matrixSpace.Matrix; // TODO refactor
        
        // generation++;
        
        void PrintPopulation(int generation, int population)
        {
            Console.WriteLine($"GENERATION: {generation}\tPOPULATION: {population}");
            if (isInvalid)
                Console.WriteLine("INVALID!");
        }
        PrintPopulation(simulation.Generation, simulation.Population);

        simulation.StartNewGeneration();

        // LINE 215
        // x3 = 24 = MaxHeight
        // y3 = 70 = MaxWidth
        // x4 = 1
        // y4 = 1
        // g = g + 1

        int nextMinX = MaxHeight - 1; // x4
        int nextMinY = MaxWidth - 1; // y4
        int nextMaxX = 0; // x3
        int nextMaxY = 0; // y3
        
        
        // prints lines before
        for (int x = 0; x < minX; x++)
        {
            Console.WriteLine();
        }
        
        // prints matrix and 
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
                nextMaxX = Math.Max(x+1, nextMaxX);
                nextMinY = Math.Min(y, nextMinY);
                nextMaxY = Math.Max(y+1, nextMaxY);
            }
            Console.WriteLine(string.Join(separator: null, values: printedLine));
        }

        // prints lines after
        for (int x = maxX + 1; x < MaxHeight; x++) // TODO test +1
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

        // LINE 309
        ProcessPopulation();

        void ProcessPopulation()
        {
            // var population = 0;
            for (int x = minX - 1; x < maxX + 2; x++) // TODO review indices
            {
                for (int y = minY - 1; y < maxY + 2; y++) // TODO review indices
                {
                    var neighbors = 0;
                    for (int i = x - 1; i < x + 2; i++) // TODO review indices
                    {
                        for (int j = y - 1; j < y + 2; j++) // TODO review indices
                        {
                            if (matrix[i, j] == 1 || matrix[i, j] == 2)
                                neighbors++;
                        }
                    }
                    // PrintMatrix(matrix);
                    if (matrix[x, y] == 0)
                    {
                        if (neighbors == 3)
                        {
                            matrix[x, y] = 3;
                            // population++;
                            simulation.IncreasePopulation();
                        }
                    }
                    else if (neighbors is < 3 or > 4)
                    {
                        matrix[x, y] = 2;
                    }
                    else
                    {
                        // population++;
                        simulation.IncreasePopulation();
                    }
                }
            }

            // LINE 635
            minX--;
            minY--;
            maxX++;
            maxY++;
        }
        // PrintMatrix(matrix);
        ProcessGeneration();
    }



    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    ProcessGeneration();
}
catch (Exception e)
{
    Console.WriteLine(e);
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


// int x1 = 1, y1 = 1;
// int x2 = 24, y2 = 70;

// var b = new string[24];





public class MatrixSpace
{
    public int[,] Matrix { get; }

    public MatrixSpace(int height, int width)
    {
        Matrix = new int[height, width];
    }

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        for (var x = 0; x < Matrix.GetLength(0); x++)
        {
            for (var y = 0; y < Matrix.GetLength(1); y++)
            {
                var character = Matrix[x, y] == 0 ? " ": Matrix[x, y].ToString();
                stringBuilder.Append(character);
            }

            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }
}
