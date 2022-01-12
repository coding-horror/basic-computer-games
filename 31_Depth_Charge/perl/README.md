Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Perl](https://www.perl.org/)

## Conversion

Not a difficult conversion - but a chance to throw in a few ways
Perl makes life easy.

 * To get the sub permission which is a random location in the g x g x g grid we can use:
   * assigning multiple variables in list form ($a,$b,$c) = (?,?,?)
   * where the list on the right hand side is generated with a map function

 * We use ternarys to generate the message if you miss the sub.
   * We use join to stitch the pieces of the string together.
   * If we have a ternary where we don't want to return anything we return an empty list rather than an empty string - if you return the latter you still get the padding spaces.

