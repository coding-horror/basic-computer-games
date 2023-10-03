### Poker

You and the computer are opponents in this game of draw poker. At the start of the game, each player is given $200. The game ends when either player runs out of money, although if you go broke the computer will offer to buy back your wristwatch or diamond tie tack.

The computer opens the betting before the draw; you open the betting after the draw. If you don’t have a hand that’s worth anything and you want to fold, bet 0. Prior to the draw, to check the draw, you may bet .5. Of course, if the computer has made a bet, you must match it in order to draw or, if you have a good hand, you may raise the bet at any time.

The author is A. Christopher Hall of Trinity College, Hartford, Connecticut.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=129)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=144)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Known Bugs

- If you bet more than the computer has, it will still see you, resulting in a negative balance.  (To handle this properly, the computer would need to go "all in" and reduce your bet to an amount it can match; or else lose the game, which is what happens to the human player in the same situation.)

- If you are low on cash and sell your watch, then make a bet much smaller than the amount you just gained from the watch, it sometimes nonetheless tells you you "blew your wad" and ends the game.

- When the watch is sold (in either direction), the buyer does not actually lose any money.

- The code in the program about selling your tie tack is unreachable due to a logic bug.


#### Porting Notes

(please note any difficulties or challenges in porting here)
