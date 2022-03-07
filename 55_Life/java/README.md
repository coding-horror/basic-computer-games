# Game of Life - Java version

Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Oracle Java](https://openjdk.java.net/)

## Requirements

* Requires Java 17 (or later)

## Notes

The Java version of Game of Life tries to mimics the behaviour of the BASIC version.
However, the Java code does not have much in common with the original.

**Differences in behaviour:**
* Input supports the ```.``` character, but it's optional.
* Evaluation of ```DONE``` input string is case insensitive.
* Run with the ```-s``` command line argument to halt the program after each generation, and continue when ```ENTER``` is pressed.
