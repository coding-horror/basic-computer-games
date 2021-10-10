Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Python](https://www.python.org/about/)

##### Translator Notes:
I tried to preserve as much of the original layout and flow of the code
as possible.  However I did use quasi enumerated types for the Fort numbers
and Fur types.  I think this was certinaly a change for the better, and 
makes the code much easier to read.

I program in many different languages on a daily basis.  Most languages
require brackets around expressions, so I just cannot bring myself to 
write an expression without brackets.  IMHO it makes the code easier to study, 
but it does contravene the Python PEP-8 Style guide.

Interestingly the code seems to have a bug around the prices of Fox Furs.
The commodity-rate for these is stored in the variable `D1`, however some
paths through the code do not set this price.  So there was a chance of 
using this uninitialised, or whatever the previous loop set.  I don't 
think this was the original authors intent.  So I preserved the original flow
of the code (using the previous `D1` value), but also catching the 
uninitialised path, and assigning a "best guess" value.

krt@krt.com.au 2020-10-10

