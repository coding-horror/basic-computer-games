### Batnum

The game starts with an imaginary pile of objects, coins for example. You and your opponent (the computer) alternately remove objects from the pile. You specify in advance the minimum and maximum number of objects that can be taken on each turn. You also specify in advance how winning is defined:
1. To take the last object
2. To avoid taking the last object

You may also determine whether you or the computer go first.

The strategy of this game is based on modulo arithmetic. If the maximum number of objects a player may remove in a turn is M, then to gain a winning position a player at the end of his turn must leave a stack of 1 modulo (M+1) coins. If you don’t understand this, play the game 23 Matches first, then BATNUM, and have fun!

BATNUM is a generalized version of a great number of manual remove-the-object games. The original computer version was written by one of the two originators of the BASIC language, John Kemeny of Dartmouth College.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=14)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=29)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html
