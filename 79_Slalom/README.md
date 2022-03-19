### Slalom

This game simulates a slalom run down a course with one to 25 gates. The user picks the number of gates and has some control over his speed down the course.

If you’re not a skier, here’s your golden opportunity to try it with minimal risk. If you are a skier, here’s something to do while your leg is in a cast.

SLALOM was written by J. Panek while a student at Dartmouth College.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=147)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=162)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

In the original version, the data pointer doesn't reset after a race is completed. This causes subsequent races to error at some future point at line 540,

    540    READ Q
