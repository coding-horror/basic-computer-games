### Digits

The player writes down a set of 30 numbers (0, 1, or 2) at random prior to playing the game. The computer program, using pattern recognition techniques, attempts to guess the next number in your list.

The computer asks for 10 numbers at a time. It always guesses first and then examines the next number to see if it guessed correctly. By pure luck (or chance or probability), the computer ought to be right 10 times. It is uncanny how much better it generally does than that!

This program originated at Dartmouth; original author unknown.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=58)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=73)


Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

- The program contains a lot of mysterious and seemingly arbitrary constants.  It's not clear there is any logic or rationality behind it.
- The key equation involved in the guess (line 700) involves a factor of `A`, but `A` is always 0, making that term meaningless.  As a result, all the work to build and update array K and value Z2 appear to be meaningless, too.
