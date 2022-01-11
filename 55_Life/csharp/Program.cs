// See https://aka.ms/new-console-template for more information

using System.Xml;

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


PrintHeader();

Console.WriteLine("ENTER YOUR PATTERN:");
// var pattern = ReadPattern(limitHeight: MaxHeight).ToArray();
var pattern = new[]
{
    "     ",
    "  *  ", 
    "  *  ", 
    "  *  ", 
    "     "
}; // FOR DEBUGGING PURPOSES
var (index, value) = GetLongestInput(pattern);
Console.WriteLine("" + index + ", " + value);

// B = pattern


const int MaxWidth = 70; // Y2
const int MaxHeight = 24; // X2

var matrix = new int[24, 70]; // TODO understand it
int population = 0; 
var isInvalid = false; // TODO understand


int x1 = (11 - index / 2) - 1;              // middle x
int y1 = (33 - value.Length / 2) - 1;       // middle y
for (var x = 0; x < pattern.Length; x++)
{
    for (var y = 0; y < pattern[x].Length; y++)
    {
        if (pattern[x][y] != ' ')
        {
            matrix[x1 + x, y1 + y] = 1; // copy the pattern to the middle of the simulation
            population++; // increments the population
        }
    }
}

PrintMatrix(matrix);
void PrintMatrix(int[,] matrix)
{
    Console.WriteLine("Matrix:");
    for (int x = 0; x < matrix.GetLength(0); x++)
    {
        for (int y = 0; y < matrix.GetLength(1); y++)
        {
            Console.Write(matrix[x, y].ToString());            
        }
        Console.WriteLine();
    }
}



int generation = 0;

void ProcessGeneration()
{
    generation++;
    void PrintPopulation(int generation, int population)
    {
        Console.WriteLine($"GENERATION: {generation}\tPOPULATION: {population}");
        if (isInvalid)
            Console.WriteLine("INVALID!");
    }
    PrintPopulation(generation, population);

    int nextMaxX = MaxHeight, nextMaxY = MaxWidth;
    int nextMinX = 1, nextMinY = 1;
    
    for (int x = 0; x < x1; x++)
    {
        Console.WriteLine();
    }

    for (var x = x1; x < MaxHeight; x++)
    {
        Console.WriteLine();
        for (var y = y1; y < MaxWidth; y++)
        {
            if (matrix[x, y] == 2)
            {
                matrix[x, y] = 0; 
                continue;
            }

            if (matrix[x, y] == 3)
            {
                matrix[x, y] = 1; // birth?
                Console.WriteLine(new string(' ', y+1) + "*");
                continue;
            }

            nextMinX = Math.Min(x, nextMinX);
            nextMaxX = Math.Max(x, nextMaxX);
            nextMinY = Math.Min(y, nextMinY);
            nextMaxY = Math.Max(y, nextMaxY);
        }
    }

    var x2 = MaxHeight;
    for (int x = x2 + 1; x < MaxHeight; x++) // TODO test +1
    {
        Console.WriteLine();
    }

    x1 = nextMaxX;
    x2 = nextMinX;
    y1 = nextMaxY;
    var y2 = nextMinY;
    
    // TODO boundaries? review
    if (x1 < 3)
    {
        x1 = 3;
        isInvalid = true;
    }

    if (x2 > 22)
    {
        x2 = 22;
        isInvalid = true;
    }

    if (y1 < 3)
    {
        y1 = 3;
        isInvalid = true;
    }

    if (y2 > 68)
    {
        y2 = 68;
        isInvalid = true;
    }

    ProcessPopulation();
    // TODO line 635

    void ProcessPopulation()
    {
        var population = 0;
        for (int x = x1 - 1; x < x2 + 1; x++) // TODO review indices
        {
            for (int y = y1 - 1; y < y2 + 1; y++) // TODO review indices
            {
                var counter = 0;
                for (int i = x - 1; i < x + 1; i++)
                {
                    for (int j = y - 1; j < y + 1; j++)
                    {
                        if (matrix[i, j] == 1 || matrix[i, j] == 2)
                            counter++;
                    }
                }

                if (matrix[x, y] == 0)
                {
                    if (counter == 3)
                    {
                        matrix[x, y] = 2;
                        population++;
                    }
                }
                else if (counter is < 3 or > 4)
                {
                    matrix[x, y] = 2;
                }
                else
                {
                    population++;
                }
            }
        }
    }
    PrintMatrix(matrix);
    ProcessGeneration();
}



Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
ProcessGeneration();






// int x1 = 1, y1 = 1;
// int x2 = 24, y2 = 70;

// var b = new string[24];





