Original source downloaded from [Vintage Basic](http://www.vintage-basic.net/games.html).

Conversion to [MiniScript](https://miniscript.org).

Note that this folder (like the original BASIC programs) contains TWO different programs based on the same idea.  evenwins.ms plays deterministically; gameofevenwins.ms learns from its failures over multiple games.

Ways to play:

1. Command-Line MiniScript:
Download for your system from https://miniscript.org/cmdline/, install, and then run the program with a command such as:

	miniscript evenwins.ms

or

	miniscript gameofevenwins.ms


2. Mini Micro:
Download Mini Micro from https://miniscript.org/MiniMicro/, launch, and then click the top disk slot and chose "Mount Folder..."  Select the folder containing the MiniScript program and this README file.  Then, at the Mini Micro command prompt, enter:

	load "evenwins"
	run

or

	load "gameofevenwins"
	run
