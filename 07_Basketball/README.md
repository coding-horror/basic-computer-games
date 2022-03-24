### Basketball

This program simulates a game of basketball between Dartmouth College and an opponent of your choice. You are the Dartmouth captain and control the type of shot and defense during the course of the game.

There are four types of shots:
1. Long Jump Shot (30ft)
2. Short Jump Shot (15ft)
3. Lay Up
4. Set Shot

Both teams use the same defense, but you may call it:
- Enter (6): Press
- Enter (6.5): Man-to-man
- Enter (7): Zone
- Enter (7.5): None

To change defense, type "0" as your next shot.

Note: The game is biased slightly in favor of Dartmouth. The average probability of a Dartmouth shot being good is 62.95% compared to a probability of 61.85% for their opponent. (This makes the sample run slightly remarkable in that Cornell won by a score of 45 to 42 Hooray for the Big Red!)

Charles Bacheller of Dartmouth College was the original author of this game.

---

As published in Basic Computer Games (1978)
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=12)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=27)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

(please note any difficulties or challenges in porting here)

##### Original bugs

###### Initial defense selection

If a number <6 is entered for the starting defense then the original code prompts again until a value >=6 is entered,
but then skips the opponent selection center jump.

The C# port does not reproduce this behavior. It does prompt for a correct value, but will then go to opponent selection
followed by the center jump.

###### Unvalidated defense selection

The original code does not validate the value entered for the defense beyond checking that it is >=6. A large enough
defense value will guarantee that all shots are good, and the game gets rather predictable.

This bug is preserved in the C# port.
