### Blackjack

This is a simulation of the card game of Blackjack or 21, Las Vegas style. This rather comprehensive version allows for up to seven players. On each hand a player may get another card (a hit), stand, split a hand in the event two identical cards were received or double down. Also, the dealer will ask for an insurance bet if he has an exposed ace.

Cards are automatically reshuffled as the 51st card is reached. For greater realism, you may wish to change this to the 41st card. Actually, fanatical purists will want to modify the program so it uses three decks of cards instead of just one.

This program originally surfaced at Digital Equipment Corp.; the author is unknown.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=18)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=33)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html


#### Porting Notes

The program makes extensive use of the assumption that a boolean expression evaluates to **-1** for true.  This was the case in some classic BASIC environments but not others; and it is not the case in [JS Basic](https://troypress.com/wp-content/uploads/user/js-basic/index.html), leading to nonsensical results.  In an environment that uses **1** instead of **-1** for truth, you would need to negate the boolean expression in the following lines:
	- 570
	- 590
	- 2220
	- 2850
	- 3100
	- 3400
	- 3410
	- 3420
