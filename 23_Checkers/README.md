### Checkers

This program plays checkers. The pieces played by the computer are marked with an “X”, yours are marked “O”. A move is made by specifying the coordinates of the piece to be moved (X, Y). Home (0,0) is in the bottom left and X specifies distance to the right of home (i.e., column) and Y specifies distance above home (i.e. row). You then specify where you wish to move to.

THe original version of the program by Alan Segal was not able to recognize (or permit) a double or triple jump. If you tried one, it was likely that your piece would disappear altogether!

Steve North of Creative Computing rectified this problem and Lawrence Neal contributed modifications to allow the program to tell which player has won the game. The computer does not play a particularly good game but we leave it to _you_ to improve that.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=40)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=55)

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
