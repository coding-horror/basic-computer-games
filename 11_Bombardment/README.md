### Bombardment

BOMBARDMENT is played on two, 5x5 grids or boards with 25 outpost locations numbered 1 to 25. Both you and the computer have four platoons of troops that can be located at any four outposts on your respective grids.

At the start of the game, you locate (or hide) your four platoons on your grid. The computer does the same on its grid. You then take turns firing missiles or bombs at each other’s outposts trying to destroy all four platoons. The one who finds all four opponents’ platoons first, wins.

This program was slightly modified from the original written by Martin Burdash of Parlin, New Jersey.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=22)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=37)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Known Bugs

- Though the instructions say you can't place two platoons on the same outpost, the code does not enforce this.  So the player can "cheat" and guarantee a win by entering the same outpost number two or more times.

#### Porting Notes

- To ensure the instructions don't scroll off the top of the screen, we may want to insert a "(Press Return)" or similar prompt before printing the tear-off matrix.

(please note any difficulties or challenges in porting here)
