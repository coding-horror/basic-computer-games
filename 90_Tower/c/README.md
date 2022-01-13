Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [ANSI-C](https://en.wikipedia.org/wiki/ANSI_C)

##### Translator Notes:

The internals are actually more a port of the Perl (because it's a C-like
language) than of the original BASIC. Because this uses the readline
library, it is built using something like

  $ cc -lreadline -o tower tower.c
