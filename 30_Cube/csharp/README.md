Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Microsoft C#](https://docs.microsoft.com/en-us/dotnet/csharp/)

#### Execution

As noted in the main Readme file, the randomization code in the BASIC program has a switch (the variable `X`) that
allows the game to be run in a deterministic (non-random) mode.

Running the C# port without command-line parameters will play the game with random mine locations.

Running the port with a `-d` command-line switch will run the game with non-random mine locations.
