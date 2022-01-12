Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Microsoft C#](https://docs.microsoft.com/en-us/dotnet/csharp/)

Conversion Notes

- There are minor spacing issues which have been preserved in this port.
- This implementation uses switch expressions to concisely place the dice pips in the right place.
- Random() is only pseudo-random but perfectly adequate for the purposes of simulating dice rolls.
- Console width is assumed to be 120 chars for the purposes of centrally aligned the intro text.