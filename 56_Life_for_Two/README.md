### Life for Two

LIFE-2 is based on Conway’s game of Life. You must be familiar with the rules of LIFE before attempting to play LIFE-2.

There are two players; the game is played on a 5x5 board and each player has a symbol to represent his own pieces of ‘life.’ Live cells belonging to player 1 are represented by `*` and live cells belonging to player 2 are represented by the symbol `#`.

The # and * are regarded as the same except when deciding whether to generate a live cell. An empty cell having two `#` and one `*` for neighbors will generate a `#`, i.e. the live cell generated belongs to the player who has the majority of the 3 live cells surrounding the empty cell where life is to be generated, for example:

```
|   | 1 | 2 | 3 | 4 | 5 |
|:-:|:-:|:-:|:-:|:-:|:-:|
| 1 |   |   |   |   |   |
| 2 |   |   | * |   |   |
| 3 |   |   |   | # |   |
| 4 |   |   | # |   |   |
| 5 |   |   |   |   |   |
```

A new cell will be generated at (3,3) which will be a `#` since there are two `#` and one `*` surrounding. The board will then become:
```
|   | 1 | 2 | 3 | 4 | 5 |
|:-:|:-:|:-:|:-:|:-:|:-:|
| 1 |   |   |   |   |   |
| 2 |   |   |   |   |   |
| 3 |   |   | # | # |   |
| 4 |   |   |   |   |   |
| 5 |   |   |   |   |   |
```
On the first most each player positions 3 pieces of life on the board by typing in the co-ordinates of the pieces. (In the event of the same cell being chosen by both players that cell is left empty.)

The board is then adjusted to the next generation and printed out.

On each subsequent turn each player places one piece on the board, the object being to annihilate his opponent’s pieces. The board is adjusted for the next generation and printed out after both players have entered their new piece.

The game continues until one player has no more live pieces. The computer will then print out the board and declare the winner.

The idea for this game, the game itself, and the above write-up were written by Brian Wyvill of Bradford University in Yorkshire, England.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=102)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=117)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

(please note any difficulties or challenges in porting here)

Note: The original program has a bug. The instructions say that if both players
enter the same cell that the cell is set to 0 or empty. However, the original
Basic program tells the player "ILLEGAL COORDINATES" and makes another cell be entered,
giving a slightly unfair advantage to the 2nd player.

The Perl verson of the program fixes the bug and follows the instructions.

Note: The original code had "GOTO 800" but label 800 didn't exist; it should have gone to label 999.
The Basic program has been fixed.

Note: The Basic program is written to assume it's being played on a Teletype, i.e. output is printed
on paper. To play on a terminal the input must not be echoed, which can be a challenge to do portably
and without tying the solution to a specific OS. Some versions may tell you how to do this, others might not.
