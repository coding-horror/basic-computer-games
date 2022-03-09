# Games.Common

This is the common library for C# and VB.Net ports of the games.

## Overview

### Game Input/Output

* `TextIO` is the main class which manages text input and output for a game. It take a `TextReader` and a `TextWriter` in
its constructor so it can be wired up in unit tests to test gameplay scenarios.
* `ConsoleIO` derives from `TextIO` and binds it to `System.Console.In` and `System.Console.Out`.
* `IReadWrite` is an interface implemented by `TextIO` which may be useful in some test scenarios.

```csharp
public interface IReadWrite
{
    // Reads a float value from input.
    float ReadNumber(string prompt);

    // Reads 2 float values from input.
    (float, float) Read2Numbers(string prompt);

    // Reads 3 float values from input.
    (float, float, float) Read3Numbers(string prompt);

    // Reads 4 float values from input.
    (float, float, float, float) Read4Numbers(string prompt);

    // Read numbers from input to fill an array.
    void ReadNumbers(string prompt, float[] values);

    // Reads a string value from input.
    string ReadString(string prompt);

    // Reads 2 string values from input.
    (string, string) Read2Strings(string prompt);

    // Writes a string to output.
    void Write(string message);

    // Writes a string to output, followed by a new-line.
    void WriteLine(string message = "");

    // Writes a float to output, formatted per the BASIC interpreter, with leading and trailing spaces.
    void Write(float value);

    // Writes a float to output, formatted per the BASIC interpreter, with leading and trailing spaces,
    // followed by a new-line.
    void WriteLine(float value);

    // Writes the contents of a Stream to output.
    void Write(Stream stream);}
```

### Random Number Generation

* `IRandom` is an interface that provides basic methods that parallel the 3 uses of BASIC's `RND(float)` function.
* `RandomNumberGenerator` is an implementation of `IRandom` built around `System.Random`.
* `IRandomExtensions` provides convenience extension methods for obtaining random numbers as `int` and also within a
  given range.

```csharp
public interface IRandom
{
    // Like RND(1), gets a random float such that 0 <= n < 1.
    float NextFloat();

    // Like RND(0), Gets the float returned by the previous call to NextFloat.
    float PreviousFloat();

    // Like RND(-x), Reseeds the random number generator.
    void Reseed(int seed);
}
```

Extension methods on `IRandom`:

```csharp
// Gets a random float such that 0 <= n < exclusiveMaximum.
float NextFloat(this IRandom random, float exclusiveMaximum);

// Gets a random float such that inclusiveMinimum <= n < exclusiveMaximum.
float NextFloat(this IRandom random, float inclusiveMinimum, float exclusiveMaximum);

// Gets a random int such that 0 <= n < exclusiveMaximum.
int Next(this IRandom random, int exclusiveMaximum);

// Gets a random int such that inclusiveMinimum <= n < exclusiveMaximum.
int Next(this IRandom random, int inclusiveMinimum, int exclusiveMaximum);

// Gets the previous unscaled float (between 0 and 1) scaled to a new range:
// 0 <= x < exclusiveMaximum.
float PreviousFloat(this IRandom random, float exclusiveMaximum);

// Gets the previous unscaled float (between 0 and 1) scaled to a new range:
// inclusiveMinimum <= n < exclusiveMaximum.
float PreviousFloat(this IRandom random, float inclusiveMinimum, float exclusiveMaximum);

// Gets the previous unscaled float (between 0 and 1) scaled to an int in a new range:
// 0 <= n < exclusiveMaximum.
int Previous(this IRandom random, int exclusiveMaximum);

// Gets the previous unscaled float (between 0 and 1) scaled to an int in a new range:
// inclusiveMinimum <= n < exclusiveMaximum.
int Previous(this IRandom random, int inclusiveMinimum, int exclusiveMaximum);
```

## C\# Usage

### Add Project Reference

Add the `Games.Common` project as a reference to the game project. For example, here's the reference from the C\# port
of `86_Tower`:

```xml
<ItemGroup>
  <ProjectReference Include="..\..\00_Common\dotnet\Games.Common\Games.Common.csproj" />
</ItemGroup>
```

### C# Game Input/Output usage

A game can be encapsulated in a class which takes a `TextIO` instance in it's constructor:

```csharp
public class Game
{
    private readonly TextIO _io;

    public Game(TextIO io) => _io = io;

    public void Play()
    {
        var name = _io.ReadString("What is your name");
        var (cats, dogs) = _io.Read2Number($"Hello, {name}, how many pets do you have (cats, dogs)");
        _io.WriteLine($"So, {cats + dogs} pets in total, huh?");
    }
}
```

Then the entry point of the game program would look something like:

```csharp
var game = new Game(new ConsoleIO());
game.Play();
```

### C# Random Number Generator usage

```csharp
var io = new ConsoleIO();
var rng = new RandomNumberGenerator();
io.WriteLine(rng.NextFloat());           // 0.1234, for example
io.WriteLine(rng.NextFloat());           // 0.6, for example
io.WriteLine(rng.PreviousFloat());       // 0.6, repeats previous
io.WriteLine(rng.PreviousFloat(0, 10));  // 6,   repeats previous value, but scaled to new range
```

### C# Unit Test usage

`TextIO` can be initialised with a `StringReader` and `StringWriter` to enable testing. For example, given the `Game`
class above:

```csharp
var reader = new StringReader("Joe Bloggs\r\n4\n\r5");
var writer = new StringWriter();
var game = new Game(new TextIO(reader, writer))

game.Play();

writer.ToString().Should().BeEquivalentTo(
    "What is your name? Hello, Joe Bloggs, how many pets do you have (cats, dogs)? ?? So, 9 pets in total, huh?");
```

Note the lack of line breaks in the expected output, because during game play the line breaks come from the text input.

Of course, `IReadWrite` can also be mocked for simple test scenarios.

## VB.Net Usage

*To be provided*
