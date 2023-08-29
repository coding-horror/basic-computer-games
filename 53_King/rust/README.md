King
====

Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [rust](https://www.rust-lang.org/).

Porting Notes
-------------

### Floats

The original code implicitly uses floating point numbers in many places which are explicitly cast to integers. In this port, I avoided using floats and tried to replicate the behaviour using just integers. It is possible that I missed some places where rounding a value would have made a difference. If you find such a bug, please notify me or make implement a fix yourself.

### Signed Numbers

I used unsigned integers for most of the program because it was easier than to check for negative values all the time. Unfortunately, that made the code a bit whacky in one or two places.

Since I only allow input of positive numbers, it is not possible to exit the game when entering the stats to resume a game, which would be possible by entering negative numbers in the original game.

### Bugs

I tried to fix all bugs listed in the [main README for King](../README.md). I have tested this implementation a bit but not extensively, so there may be some portation bugs. If you find them, you are free to fix them.

Future Development
------------------

I plan to add some tests and tidy up the code a bit, but this version should be feature-complete.
