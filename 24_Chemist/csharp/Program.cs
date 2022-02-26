using System;
const int maxLives = 9;

WriteCentred("Chemist");
WriteCentred("Creative Computing, Morristown, New Jersey");
Console.WriteLine(@"


The fictitious chemical kryptocyanic acid can only be
diluted by the ratio of 7 parts water to 3 parts acid.
If any other ratio is attempted, the acid becomes unstable
and soon explodes.  Given the amount of acid, you must
decide who much water to add for dilution.  If you miss
you face the consequences.
");

var random = new Random();
int livesUsed = 0;
while (livesUsed < maxLives)
{
    int krypto = random.Next(1, 50);
    double water = krypto * 7.0 / 3.0;

    Console.WriteLine($"{krypto} Liters of kryptocyanic acid.  How much water?");
    double answer = double.Parse(Console.ReadLine());

    double diff = Math.Abs(answer - water);
    if (diff <= water / 20)
    {
        Console.WriteLine("Good job! You may breathe now, but don't inhale the fumes"!);
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("Sizzle!  You have just been desalinated into a blob\nof quivering protoplasm!");
        Console.WriteLine();
        livesUsed++;

        if (livesUsed < maxLives)
            Console.WriteLine("However, you may try again with another life.");
    }
}
Console.WriteLine($"Your {maxLives} lives are used, but you will be long remembered for\nyour contributions to the field of comic book chemistry.");

static void WriteCentred(string text)
{
    int indent = (Console.WindowWidth + text.Length) / 2;
    Console.WriteLine($"{{0,{indent}}}", text);
}
