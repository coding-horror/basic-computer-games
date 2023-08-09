Original source downloaded from [Vintage Basic](http://www.vintage-basic.net/games.html).

Conversion to [MiniScript](https://miniscript.org).

Ways to play:

1. Command-Line MiniScript:
Download for your system from https://miniscript.org/cmdline/, install, and then run the program with a command such as:

	miniscript blackjack.ms

But note that the current release (1.2.1) of command-line MiniScript does not properly flush the output buffer when line breaks are suppressed, as this program does when prompting for your next action after a Hit.  So, method 2 (below) is recommended for now.

2. Mini Micro:
Download Mini Micro from https://miniscript.org/MiniMicro/, launch, and then click the top disk slot and chose "Mount Folder..."  Select the folder containing the MiniScript program and this README file.  Then, at the Mini Micro command prompt, enter:

	load "blackjack"
	run
