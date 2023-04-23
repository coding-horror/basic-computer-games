Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Perl](https://www.perl.org/)

###Bowling program in Perl

Run normally, this is a fairly faithful translation of the Basic game.
The only real differences are a few trivial fix-ups on the prints to make it
look better, and the player/frame/ball line was put before the "get the ball
going" line to make it more obvious who's turn it is.

However, if you run it with "-a" on the command line, it will go into
"advanced" mode, which means that "." is used to show pin down and "!" for
pin up, current running scores are shown at the end of each frame, and the
scoring also looks more normal at the end. This is all done because I think it
looks better and I wanted to see a score. Having a flag says you can play
whichever version of the game you like.

Note, the original code doesn't do the 10th frame correctly, in that it will
never do more than 2 balls, so the best score you can get is a 290.
This is true in both modes. That being said, it will always give you a mediocre
game; I don't think I've ever seen a score over 140.
