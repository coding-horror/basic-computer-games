### Bowling

This is a simulated bowling game for up to four players. You play 10 frames. To roll the ball, you simply type “ROLL.” After each roll, the computer will show you a diagram of the remaining pins (“0” means the pin is down, “+” means it is still standing), and it will give you a roll analysis:
- GUTTER
- STRIKE
- SPARE
- ERROR (on second ball if pins still standing)

Bowling was written by Paul Peraino while a student at Woodrow Wilson High School, San Francisco, California.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=26)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=41)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Known Bugs

- In the original code, scores is not kept accurately in multiplayer games.  It stores scores in F*P, where F is the frame and P is the player.  So, for example, frame 8 player 1 (index 16) clobbers the score from frame 4 player 2 (also index 16).

- Even when scores are kept accurately, they don't match normal bowling rules.  In this game, the score for each ball is just the total number of pins down after that ball, and the third row of scores is a status indicator (3 for strike, 2 for spare, 1 for anything else).

- The program crashes with a "NEXT without FOR" error if you elect to play again after the first game.

#### Porting Notes

- The funny control characters in the "STRIKE!" string literal are there to make the terminal beep.
