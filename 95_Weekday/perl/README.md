Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Perl](https://www.perl.org/)

I have replaced the manual date logic with Perl built-ins to the extent
possible. Unfortunately the kind of date math involved in the "time
spent doing ..." functionality is not well-defined, so I have been
forced to retain the original logic here. Sigh.

You can use any punctuation character you please in the date
input. So something like 2/29/2020 is perfectly acceptable.

It would also have been nice to produce a localized version that
supports day/month/year or year-month-day input, but that didn't happen.

Also nice would have been language-specific output -- especially if it
could have accommodated regional differences in which day of the week or
month is unlucky.

Tom Wyant
