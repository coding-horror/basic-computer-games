using System;
using System.Collections.Generic;
using System.Linq;

using Animal;

Console.WriteLine(new string(' ', 32) + "ANIMAL");
Console.WriteLine(new string(' ', 15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
Console.WriteLine();
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("PLAY 'GUESS THE ANIMAL'");
Console.WriteLine();
Console.WriteLine("THINK OF AN ANIMAL AND THE COMPUTER WILL TRY TO GUESS IT.");
Console.WriteLine();

// Root of the question and answer tree
Branch rootBranch = new Branch
{
    Text = "DOES IT SWIM",
    Yes = new Branch { Text = "FISH" },
    No = new Branch { Text = "BIRD" }
};

string[] TRUE_INPUTS = { "Y", "YES", "T", "TRUE" };
string[] FALSE_INPUTS = { "N", "NO", "F", "FALSE" };


while (true)
{
    MainGameLoop();
}

void MainGameLoop()
{
    // Wait fora YES or LIST command
    string input = null;
    while (true)
    {
        input = GetInput("ARE YOU THINKING OF AN ANIMAL");
        if (IsInputListCommand(input))
        {
            ListKnownAnimals(rootBranch);
        }
        else if (IsInputYes(input))
        {
            break;
        }
    }

    // Walk through the tree following the YES and NO
    // branches based on user input.
    Branch currentBranch = rootBranch;
    while (!currentBranch.IsEnd)
    {
        while (true)
        {
            input = GetInput(currentBranch.Text);
            if (IsInputYes(input))
            {
                currentBranch = currentBranch.Yes;
                break;
            }
            else if (IsInputNo(input))
            {
                currentBranch = currentBranch.No;
                break;
            }
        }
    }

    // Was the answer correct?
    input = GetInput($"IS IT A {currentBranch.Text}");
    if (IsInputYes(input))
    {
        Console.WriteLine("WHY NOT TRY ANOTHER ANIMAL?");
        return;
    }

    // Interview the user to add a new question and answer
    // branch to the tree
    string newAnimal = GetInput("THE ANIMAL YOU WERE THINKING OF WAS A");
    string newQuestion = GetInput($"PLEASE TYPE IN A QUESTION THAT WOULD DISTINGUISH A {newAnimal} FROM A {currentBranch.Text}");
    string newAnswer = null;
    while (true)
    {
        newAnswer = GetInput($"FOR A {newAnimal} THE ANSWER WOULD BE");
        if (IsInputNo(newAnswer))
        {
            currentBranch.No = new Branch { Text = newAnimal };
            currentBranch.Yes = new Branch { Text = currentBranch.Text };
            currentBranch.Text = newQuestion;
            break;
        }
        else if (IsInputYes(newAnswer))
        {
            currentBranch.Yes = new Branch { Text = newAnimal };
            currentBranch.No = new Branch { Text = currentBranch.Text };
            currentBranch.Text = newQuestion;
            break;
        }
    }
}

string GetInput(string prompt)
{
    Console.Write($"{prompt}? ");
    string result = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(result))
    {
        return GetInput(prompt);
    }

    return result.Trim().ToUpper();
}

bool IsInputYes(string input) => TRUE_INPUTS.Contains(input.ToUpperInvariant().Trim());

bool IsInputNo(string input) => FALSE_INPUTS.Contains(input.ToUpperInvariant().Trim());

bool IsInputListCommand(string input) => input.ToUpperInvariant().Trim() == "LIST";

string[] GetKnownAnimals(Branch branch)
{
    List<string> result = new List<string>();
    if (branch.IsEnd)
    {
        return new[] { branch.Text };
    }
    else
    {
        result.AddRange(GetKnownAnimals(branch.Yes));
        result.AddRange(GetKnownAnimals(branch.No));
        return result.ToArray();
    }
}

void ListKnownAnimals(Branch branch)
{
    string[] animals = GetKnownAnimals(branch);
    for (int x = 0; x < animals.Length; x++)
    {
        int column = (x % 4);
        if (column == 0)
        {
            Console.WriteLine();
        }

        Console.Write(new string(' ', column == 0 ? 0 : 15) + animals[x]);
    }
    Console.WriteLine();
}
