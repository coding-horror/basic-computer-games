### Banner

This program creates a large banner on a terminal of any message you input. The letters may be any dimension of you wish although the letter height plus distance from left-hand side should not exceed 6 inches. Experiment with the height and width until you get a pleasing effect on whatever terminal you are using.

This program was written by Leonard Rosendust of Brooklyn, New York.

---

As published in Basic Computer Games (1978)
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=10)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=25)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

- The "SET PAGE" input, stored in `O$`, has no effect.  It was probably meant as an opportunity for the user to set their pin-feed printer to the top of the page before proceeding.

- The data values for each character are the bit representation of each horizontal row of the printout (vertical column of a character), plus one.  Perhaps because of this +1, the original code (and some of the ports here) are much more complicated than they need to be.

(please note any difficulties or challenges in porting here)

