### 23 Matches

In the game of twenty-three matches, you start with 23 matches lying on a table. On each turn, you may take 1, 2, or 3 matches. You alternate moves with the computer and the one who has to take the last match loses.

The easiest way to devise a winning strategy is to start at the end of the game. Since your wish to leave the last match to your opponent, you would like to have either 4, 3, or 2 on your last turn you so can take away 3, 2, or 1 and leave 1. Consequently, you would like to leave your opponent with 5 on his next to last turn so, no matter what his move, you are left with 4, 3, or 2. Work this backwards to the beginning and you’ll find the game can effectively be won on the first move. Fortunately, the computer gives you the first move, so if you play wisely, you can win.

After you’ve mastered 23 Matches, move on to BATNUM and then to NUM.

This version of 23 Matches was originally written by Bob Albrecht of People’s Computer Company.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=177)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=192)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

There is an oddity (you can call it a bug, but it is no big deal) in the original code. If there are only two or three matches left at the player's turn and the player picks all of them (or more), the game would still register that as a win for the player.
