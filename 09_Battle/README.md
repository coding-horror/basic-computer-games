### Battle

BATTLE is based on the popular game Battleship which is primarily played to familiarize people with the location and designation of points on a coordinate plane.

BATTLE first randomly sets up the bad guy’s fleet disposition on a 6 by 6 matrix or grid. The fleet consists of six ships:
- Two destroyers (ships number 1 and 2) which are two units long
- Two cruisers (ships number 3 and 4) which are three units long
- Two aircraft carriers (ships number 5 and 6) which are four units long

The program then prints out this fleet disposition in a coded or disguised format (see the sample computer print-out). You then proceed to sink the various ships by typing in the coordinates (two digits. each from 1 to 6, separated by a comma) of the place where you want to drop a bomb, if you’ll excuse the expression. The computer gives the appropriate response (splash, hit, etc.) which you should record on a 6 by 6 matrix. You are thus building a representation of the actual fleet disposition which you will hopefully use to decode the coded fleet disposition printed out by the computer. Each time a ship is sunk, the computer prints out which ships have been sunk so far and also gives you a “SPLASH/HIT RATIO.”

The first thing you should learn is how to locate and designate positions on the matrix, and specifically the difference between “3,4” and “4,3.” Our method corresponds to the location of points on the coordinate plane rather than the location of numbers in a standard algebraic matrix: the first number gives the column counting from left to right and the second number gives the row counting from bottom to top.

The second thing you should learn about is the splash/hit ratio. “What is a ratio?” A good reply is “It’s a fraction or quotient.” Specifically, the spash/hit ratio is the number of splashes divided by the number of hits. If you had 9 splashes and 15 hits, the ratio would be 9/15 or 3/5, both of which are correct. The computer would give this splash/hit ratio as .6.

The main objective and primary education benefit of BATTLE comes from attempting to decode the bad guys’ fleet disposition code. To do this, you must make a comparison between the coded matrix and the actual matrix which you construct as you play the game.

The original author of both the program and these descriptive notes is Ray Westergard of Lawrence Hall of Science, Berkeley, California.

---

As published in Basic Computer Games (1978)
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=15)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=30)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

- The original game has no way to re-view the fleet disposition code once it scrolls out of view.  Ports should consider allowing the user to enter "?" at the "??" prompt, to reprint the disposition code.  (This is added by the MiniScript port under Alternate Languages, for example.)