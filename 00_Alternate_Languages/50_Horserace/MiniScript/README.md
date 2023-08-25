Original source downloaded from [Vintage Basic](http://www.vintage-basic.net/games.html).

Conversion to [MiniScript](https://miniscript.org).

Ways to play:

1. Command-Line MiniScript:
Download for your system from https://miniscript.org/cmdline/, install, and then run the program with a command such as:

```
	miniscript horserace.ms
```
2. Mini Micro:
Download Mini Micro from https://miniscript.org/MiniMicro/, launch, and then click the top disk slot and chose "Mount Folder..."  Select the folder containing the MiniScript program and this README file.  Then, at the Mini Micro command prompt, enter:

```
	load "horserace"
	run
```

## Porting Notes

- The original program, designed to be played directly on a printer, drew a track 27 rows long.  To fit better on modern screens, I've shortened the track to 23 rows.  This is adjustable via the "trackLen" value assigned on line 72.

- Also because we're playing on a screen instead of a printer, I'm clearing the screen and pausing briefly before each new update of the track.  This is done via the `clear` API when running in Mini Micro, or by using a VT100 escape sequence in other contexts.
