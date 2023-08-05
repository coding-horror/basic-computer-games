Original source downloaded from [Vintage Basic](http://www.vintage-basic.net/games.html).

Conversion to [MiniScript](https://miniscript.org).

Ways to play:

1. Command-Line MiniScript:
Download for your system from https://miniscript.org/cmdline/, install, and then run the program with a command such as:

	miniscript football.ms
or
	miniscript ftball.ms

2. Mini Micro:
Download Mini Micro from https://miniscript.org/MiniMicro/, launch, and then click the top disk slot and chose "Mount Folder..."  Select the folder containing the BASIC program.  Then, at the Mini Micro command prompt, enter:

	load "football"
	run
or
	load "ftball"
	run


#### Apology from the Translator

These MiniScript programs were actually ported from the JavaScript ports of the original BASIC programs.  I did that because the BASIC code (of both programs) was incomprehensible spaghetti.  The JavaScript port, however, was essentially the same — and so are the MiniScript ports.  The very structure of these programs makes them near-impossible to untangle.

If I were going to write a football simulation from scratch, I would approach it very differently.  But in that case I would have either a detailed specification of how the program should behave, or at least enough understanding of American football to design it myself as I go.  Neither is the case here (and we're supposed to be porting the original programs, not making up our own).

So, I'm sorry.  Please take these programs as proof that you can write bad code even in the most simple, elegant languages.  And I promise to try harder on future translations!
