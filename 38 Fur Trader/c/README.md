Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [ANSI-C](https://en.wikipedia.org/wiki/ANSI_C)

##### Translator Notes:
I tried to preserve as much of the original layout and flow of the code
as possible.  However I did use enumerated types for the Fort numbers
and Fur types.  I think this was certainly a change for the better, and 
makes the code much easier to read.

I also tried to minimise the use of pointers, and stuck with old-school 
C formatting, because you never know how old the compiler is.

Interestingly the code seems to have a bug around the prices of Fox Furs.
The commodity-rate for these is stored in the variable `D1`, however some
paths through the code do not set this price.  So there was a chance of 
using this uninitialised, or whatever the previous loop set.  I don't 
think this was the original authors intent.  So I preserved the original flow
of the code (using the previous `D1` value), but also catching the 
uninitialised path, and assigning a "best guess" value.

krt@krt.com.au 2020-10-10

