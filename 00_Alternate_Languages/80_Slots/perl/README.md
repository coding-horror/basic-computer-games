Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Perl](https://www.perl.org/)

This Perl script is a port of slots, which is the 80th entry in Basic
Computer Games.

I know nothing about slot machines, and my research into them says to me
that the payout tables can be fairly arbitrary. But I have taken the
liberty of deeming the BASIC program's refusal to pay on LEMON CHERRY
LEMON a bug, and made that case a double.

My justification for this is that at the point where the BASIC has
detected the double in the first and third reels it has already detected
that there is no double in the first and second reels. After the check
for a bar (and therefore a double bar) fails it goes back and checks for
a double on the second and third reels. But we know this check will
fail, since the check for a double on the first and second reels failed.
So if a loss was intended at this point, why not just call it a loss?

To restore the original behavior, comment out the entire line commented
'# Bug fix?' (about line 75) and uncomment the line with the trailing
comment '# Bug?' (about line 83).
