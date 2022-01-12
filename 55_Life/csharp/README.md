# Life

An implementation of John Conway's popular cellular automaton, also know as **Conway's Game of Life**. The original source was downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html).

Ported by Dyego Alekssander Maas.

## How to run

This program requires you to install [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0). After installed, you just need to run `dotnet run` from this directory in the terminal.

## Know more about Conway's Game of Life

You can find more about Conway's Game of Life on this page of the [Cornell Math Explorers' Club](http://pi.math.cornell.edu/~lipa/mec/lesson6.html), alongside many examples of patterns you can try. 

### Optional parameters

Optionally, you can run this program with the `--wait 1000` argument, the number being the time in milliseconds
that the application will pause between each iteration. This is enables you to watch the simulation unfolding. By default, there is no pause between iterations.

The complete command would be `dotnet run --wait 1000`.

## Entering patterns

Once running the game, you are expected to enter a pattern. This pattern consists of multiple lines of text with either **spaces** or **some character**, usually an asterisk (`*`).

Spaces represent empty cells. Asterisks represent alive cells.

After entering the pattern, you need to enter the word "DONE". It is not case sensitive. An example of pattern would be:

```
 *
***
DONE
```

### Some patterns you could try

```
 *
***
```

```
*
***
```

```
**
**
```

```
  *
 *
*
```

This one is known as **glider**:

```
***
*
 *
```

## Instructions to the port

Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Microsoft C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
