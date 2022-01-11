// See https://aka.ms/new-console-template for more information

const int MaxWidth = 70;
const int MaxHeight = 24;

PrintHeader();

int x1 = 1, y1 = 1;
int x2 = 24, y2 = 70;
var a = new float[24, 70];
var b = new string[24];
var c = 1;

Console.WriteLine("ENTER YOUR PATTERN:");
b[c] = Console.ReadLine();
if (b[c] == "DONE")
    b[c] = ""


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

