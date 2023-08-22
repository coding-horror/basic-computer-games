### Hexapawn

The game of Hexapawn and a method to learn a strategy for playing the game was described in Martin Gardner’s “Mathematical Games” column in the March 1962 issue of _Scientific American_. The method described in the article was for a hypothetical learning machine composed of match boxes and colored beads. This has been generalized in the program HEX.

The program learns by elimination of bad moves. All positions encountered by the program and acceptable moves from them are stored in an array. When the program encounters an unfamiliar position, the position and all legal moves from it are added to the list. If the program loses a game, it erases the move that led to defeat. If it hits a position from which all moves have been deleted (they all led to defeat), it erases the move that got it there and resigns. Eventually, the program learns to play extremely well and, indeed, is unbeatable. The learning strategy could be adopted to other simple games with a finite number of moves (tic-tac-toe, small board checkers, or other chess-based games).

The original version of this program was written by R.A. Kaapke. It was subsequently modified by Jeff Dalton and finally by Steve North of Creative Computing.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=83)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=98)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Known Bugs

- There are valid board positions that will cause the program to print "ILLEGAL BOARD PATTERN" and break.  For example: human 8,5; computer 1,5; human 9,5; computer 3,5; human 7,5.  This is a valid game-over pattern, but it is not detected as such because of incorrect logic in lines 240-320 (intended to detect whether the computer has any legal moves).

#### Porting Notes

(please note any difficulties or challenges in porting here)
