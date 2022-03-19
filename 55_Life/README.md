### Life

The Game of Life was originally described in _Scientific American_, October 1970, in an article by Martin Gardner. The game itself was originated by John Conway of Gonville and Caius College, University of Cambridge England.

In the “manual” game, organisms exist in the form of counters (chips or checkers) on a large checkerboard and die or reproduce according to some simple genetic rules. Conway’s criteria for choosing his genetic laws were carefully delineated as follows:
1. There should be no initial pattern for which there is a simple proof that the population can grow without limit.
2. There should be simple initial patterns that apparently do grow without limit.
3. There should be simple initial patterns that grow and change for a considerable period of time before coming to an end in three possible ways:
    1. Fading away completely (from overcrowding or from becoming too sparse)
    2. Settling into a stable configuration that remains unchanged thereafter
    3. Entering an oscillating phase in which they repeat an endless cycle of two or more periods

In brief, the rules should be such as to make the behavior of the population relatively unpredictable. Conway’s genetic laws are delightfully simple. First note that each cell of the checkerboard (assumed to be an infinite plane) has eight neighboring cells, four adjacent orthogonally, four adjacent diagonally. The rules are:
1. Survivals. Every counter with two or three neighboring counters survives for the next generation.
2. Deaths. Each counter with four or more neighbors dies (is removed) from overpopulation. Every counter with one neighbor or none dies from isolation.
3. Births. Each empty cell adjacent to exactly three neighbors — no more — is a birth cell. A counter is placed on it at the next move.

It is important to understand that all births and deaths occur simultaneously. Together they constitute a single generation or, as we shall call it, a “move” in the complete “life history” of the initial configuration.

You will find the population constantly undergoing unusual, sometimes beautiful and always unexpected change. In a few cases the society eventually dies out (all counters vanishing), although this may not happen until after a great many generations. Most starting patterns either reach stable figures — Conway calls them “still lifes” — that cannot change or patterns that oscillate forever. Patterns with no initial symmetry tend to become symmetrical. Once this happens the symmetry cannot be lost, although it may increase in richness.

Conway used a DEC PDP-7 with a graphic display to observe long-lived populations. You’ll probably find this more enjoyable to watch on a CRT than a hard-copy terminal.

Since MITS 8K BASIC does not have LINE INPUT, to enter leading blanks in the patter, type a “.” at the start of the line. This will be converted to a space by BASIC, but it permits you to type leading spaces. Typing DONE indicates that you are finished entering the pattern. See sample run.

Clark Baker of Project DELTA originally wrote this version of LIFE which was further modified by Steve North of Creative Computing.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=100)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=115)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html


#### Porting Notes

(please note any difficulties or challenges in porting here)
