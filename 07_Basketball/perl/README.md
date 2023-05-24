Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Perl](https://www.perl.org/)

There are two version of the code here, a "faithful" translation (basketball-orig.pl) and
a "modern" translation (basketball.pl). The main difference between the 2 are is that the
faithful translation has 3 GOTOs in it while the modern version has no GOTO. I have added
a "TIME" print when the score is shown so the Clock is visible. Halftime is at "50" and
end of game is at 100 (per the Basic code).

The 3 GOTOs in the faitful version are because of the way the original code jumped into
the "middle of logic" that has no obivious way to avoid ... that I can see, at least while
still maintaining something of the look and structure of the original Basic.

The modern version avoided the GOTOs by restructuring the program in the 2 "play()" subs.
Despite the change, this should play the same way as the faithful version.

All of the percentages remain the same. If writing this from scratch, we really should
have only a single play() sub which uses the same code for both teams, which would also
make the game more fair ... but that wasn't done so the percent edge to Darmouth has been
maintained here.
