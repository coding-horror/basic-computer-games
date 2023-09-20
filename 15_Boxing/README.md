### Boxing

This program simulates a three-round Olympic boxing match. The computer coaches one of the boxers and determines his punches and defences, while you do the same for your boxer. At the start of the match, you may specify your man’s best punch and his vulnerability.

There are approximately seven major punches per round, although this may be varied. The best out of three rounds wins.

Jesse Lynch of St. Paul, Minnesota created this program.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=28)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=43)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Known Bugs

- The code that handles player punch type 1 checks for opponent weakness type 4; this is almost certainly a mistake.

- Line breaks or finishing messages are omitted in various cases.  For example, if the player does a hook, and that's the opponent's weakness, then 7 points are silently awarded without outputting any description or line break, and the next sub-round will begin on the same line.

- When the opponent selects a hook, control flow falls through to the uppercut case.  Perhaps related, a player weakness of type 2 (hook) never has any effect on the game.

#### Porting Notes

(please note any difficulties or challenges in porting here)
