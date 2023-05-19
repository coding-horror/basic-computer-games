Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Perl](https://www.perl.org/)

Note: The original program has a bug (see the README in the above dir). This Perl version fixes it.

Note: For input, the X value is to the right while the Y value is down.
Therefore, the top right cell is "5,1", not "1,5".

The original program was made to be played on a Teletype, i.e. a printer on paper.
That allowed the program to "black out" the input line to hide a user's input from his/her
opponent, assuming the opponent was at least looking away. To do the equivalent on a
terminal would require a Perl module that isn't installed by default (i.e. it is not
part of CORE and would also require a C compiler to install), nor do I want to issue a
shell command to "stty" to hide the input because that would restrict the game to Linux/Unix.
This means it would have to be played on the honor system.

However, if you want to try it, install the module "Term::ReadKey" ("sudo cpan -i Term::ReadKey"
if on Linux/Unix and you have root access). If the code finds that module, it will automatically
use it and hide the input ... and restore echoing input again when the games ends. If the module
is not found, input will be visible.
