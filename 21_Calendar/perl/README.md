Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Perl](https://www.perl.org/)

Actually, this is not so much a port as a complete rewrite, making use of
Perl's Posix time functionality. The calendar is for the current year (not
1979), but you can get another year by specifying it on the command line, e.g.

 `perl 21_Calendar/perl/calendar.pl 2001`

It *may* even produce output in languages other than English. But the
leftmost column will still be Sunday, even in locales where it is
typically Monday.
