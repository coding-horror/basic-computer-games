### Checkers

As published in Basic Computer Games (1978)
https://www.atariarchives.org/basicgames/showpage.php?page=40

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

The file `checkers.annotated.bas` contains an indented and annotated
version of the source code.  This is no longer valid BASIC code but
should be more readable.

## Known Issues In the Original BASIC Code
 - If the computer moves a checker to the bottom row, it promotes, but
   leaves the original checker in place. (See line 1240)
 - Human players may move non-kings as if they were kings. (See lines 1590 to 1810)
 - Human players are not required to jump if it is possible.
 - Curious writing to "I" variable without ever reading it. (See lines 1700 and 1806)
