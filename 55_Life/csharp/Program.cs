// See https://aka.ms/new-console-template for more information

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
var pattern = new string[] { "a", "bdc", "", "pattern" }; // FOR DEBUGGING PURPOSES
var (index, value) = GetLongestInput(pattern);
Console.WriteLine("" + index + ", " + value);

// B = pattern

int x1 = (11 - index / 2) - 1;
int y1 = (33 - value.Length / 2) - 1;
const int MaxWidth = 70; // Y2
const int MaxHeight = 24; // X2

var a = new int[24, 70]; // TODO understand it
int population = 0; 

// count initial population?
for (var x = 0; x < pattern.Length; x++)
{
    for (var y = 0; y < pattern[x].Length; y++)
    {
        if (pattern[x][y] != ' ')
        {
            a[x1 + x, y1 + y] = 1;
            population++;
        }
    }
}

void ProcessGeneration()
{
    void PrintPopulation(int generation, int population)
    {
        Console.WriteLine($"GENERATION: {generation}\tPOPULATION: {population}");
        var i9 = false; // TODO understand
        if (i9)
            Console.WriteLine("INVALID!");
    }
    int generation = 1;
    PrintPopulation(generation, population);

    int x3 = MaxHeight, y3 = MaxWidth;
    int x4 = 1, y4 = 1;
    
    for (int x = 0; x < x1; x++)
    {
        Console.WriteLine();
    }

    for (var x = x1; x < MaxHeight; x++)
    {
        Console.WriteLine();
        for (var y = y1; y < MaxWidth; y++)
        {
            if (a[x, y] == 2)
            {
                a[x, y] = 0; // birth?
                continue;
            }

            if (a[x, y] == 3)
            {
                a[x, y] = 1;
                Console.WriteLine(new string('\t', y+1) + "*");
                continue;
            }

            // TODO understand what it does
            if (x < x3) x3 = x;
            if (x > x4) x4 = x;
            if (y < y3) y3 = y;
            if (y < y4) y4 = y;
        }
    }
    
}

PrintMatrix(a);
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

Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
ProcessGeneration();






// int x1 = 1, y1 = 1;
// int x2 = 24, y2 = 70;

// var b = new string[24];





