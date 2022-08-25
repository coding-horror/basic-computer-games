### Cube

CUBE is a game played on the facing sides of a cube with a side dimension of 2. A location is designated by three numbers — e.g., 1, 2, 1. The object is to travel from 1, 1, 1 to 3, 3, 3 by moving one horizontal or vertical (not diagonal) square at a time without striking one of 5 randomly placed landmines. You are staked to $500; prior to each play of the game you may make a wager whether you will reach your destination. You lose if you hit a mine or try to make an illegal move — i.e., change more than one digit from your previous position.

Cube was created by Jerimac Ratliff of Fort Worth, Texas.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=53)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=68)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

(please note any difficulties or challenges in porting here)

##### Randomization Logic

The BASIC code uses an interesting technique for choosing the random coordinates for the mines. The first coordinate is
chosen like this:

```basic
380 LET A=INT(3*(RND(X)))
390 IF A<>0 THEN 410
400 LET A=3
```

where line 410 is the start of a similar block of code for the next coordinate. The behaviour of `RND(X)` depends on the
value of `X`. If `X` is greater than zero then it returns a random value between 0 and 1. If `X` is zero it returns the
last random value generated, or 0 if no value has yet been generated.

If `X` is 1, therefore, the first line above set `A` to 0, 1, or 2. The next 2 lines replace a 0 with a 3. The
replacement values varies for the different coordinates with the result that the random selection is biased towards a
specific set of points. If `X` is 0, the `RND` calls all return 0, so the coordinates are the known. It appears that
this technique was probably used to allow testing the game with a well-known set of locations for the mines. However, in
the code as it comes to us, the value of `X` is never set and is thus 0, so the mine locations are never randomized.

The C# port implements the biased randomized mine locations, as seems to be the original intent, but includes a
command-line switch to enable the deterministic execution as well.
