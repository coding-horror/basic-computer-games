using System;

Console.WriteLine(Tab(30) + "Sine Wave");
Console.WriteLine(Tab(15) + "Creative Computing Morristown, New Jersey\n\n\n\n\n");

bool isCreative = true;
for (double t = 0.0; t <= 40.0; t += 0.25)
{
    int a = (int)(26 + 25 * Math.Sin(t));
    string word = isCreative ? "Creative" : "Computing";
    Console.WriteLine($"{Tab(a)}{word}");
    isCreative = !isCreative;
}

static string Tab(int n) => new string(' ', n);