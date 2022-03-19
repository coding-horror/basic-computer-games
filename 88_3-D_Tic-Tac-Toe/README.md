### 3-D Tic-Tac-Toe

3-D TIC-TAC-TOE is a game of tic-tac-toe in a 4x4x4 cube. You must get 4 markers in a row or diagonal along any 3-dimensional plane in order to win.

Each move is indicated by a 3-digit number (digits not separated by commas), with each digit between 1 and 4 inclusive. The digits indicate the level, column, and row, respectively, of the move. You can win if you play correctly; although, it is considerably more difficult than standard, two-dimensional 3x3 tic-tac-toe.

This version of 3-D TIC-TAC-TOE is from Dartmouth College.

### Conversion notes

The AI code for TicTacToe2 depends quite heavily on the non-structured GOTO (I can almost hear Dijkstra now) and translation is quite challenging. This code relies very heavily on GOTOs that bind the code tightly together. Comments explain where that happens in the original.

There are at least two bugs from the original BASIC:

1. Code should only allow player to input valid 3D coordinates where every digit is between 1 and 4, but the original code allows any value between 111 and 444 (such as 297, for instance).
2. If the player moves first and the game ends in a draw, the original program will still prompt the player for a move instead of calling for a draw.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=168)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=183)


Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

(please note any difficulties or challenges in porting here)
