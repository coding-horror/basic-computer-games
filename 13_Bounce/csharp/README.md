# Bounce

Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Microsoft C#](https://docs.microsoft.com/en-us/dotnet/csharp/)

## Conversion notes

### Mode of Operation

This conversion performs the same function as the original, and provides the same experience, but does it in a different
way.

The original BASIC code builds the graph as it writes to the screen, scanning each line for points that need to be
plotted.

This conversion steps through time, calculating the position of the ball at each instant, building the graph in memory.
It then writes the graph to the output in one go.

### Failure Modes

The original BASIC code performs no validation of the input parameters. Some combinations of parameters produce no
output, others crash the program.

In the spirit of the original this conversion also performs no validation of the parameters, but it does not attempt to
replicate the original's failure modes. It fails quite happily in its own way.
