### What are we doing?

We're updating the first million selling computer book, [BASIC Computer Games](https://en.wikipedia.org/wiki/BASIC_Computer_Games), for 2021!

### Where can we discuss it?

Please see [the discussion here](https://discourse.codinghorror.com/t/updating-101-basic-computer-games-for-2021/7927) for a worklog and conversation around this project.

### Project structure

I have moved all [the original BASIC source code](http://www.vintage-basic.net/games.html) into a folder for each project in the original book (first volume). Note that Lyle Kopnicky has generously normalized all the code (thanks Lyle!) to run against [Vintage Basic](http://www.vintage-basic.net/download.html) circa 2009:

> I've included all the games here for your tinkering pleasure. I've tested and tweaked each one of them to make sure they'll run with Vintage BASIC, though you may see a few oddities. That's part of the fun of playing with BASIC: it never works quite the same on two machines. The games will play better if you keep CAPS LOCK on, as they were designed to be used with capital-letter input.

Each project has subfolders corresponding to the languages we'd like to see the games ported to. This is based on the [February 2021 TIOBE index of top languages](https://www.tiobe.com/tiobe-index/) that are _memory safe_ and _general purpose scripting language_ per [this post](https://discourse.codinghorror.com/t/updating-101-basic-computer-games-for-2021/7927/34):

- Java
- Python
- C#
- VB.NET
- JavaScript
- Ruby
- Delphi / Object Pascal
- Perl

### Project goals

Feel free to begin converting these classic games into the above list of modern, memory safe languages. But first, a few guidelines:

- **These are very old games**. They date from the mid-70s so they're not exactly examples of what kids (or anyone, really?) would be playing these days. Consider them more like classic programming exercises to teach programming.  We're paying it forward by converting them into modern languages, so the next generation can learn from the programs in this classic book -- and compare implementations across common modern languages.

- **Stay true to the original program**. These are mostly unsophisticated, simple command line / console games, so we should strive to replicate the command line / console output and behavior illustrated in the original book. See the README in the project folder for links to the original scanned source input and output. Avoid the impulse to add features; keep it simple.

- **Please DO update for modern coding conventions**. Support uppercase and lowercase. Use structured programming. Use subroutines. Try to be an example of good, modern coding practices!

- **Use lots of comments to explain what is going on**. Comment liberally! If there were clever tricks in the original code, decompose those tricks into simpler (even if more verbose) code, and use comments to explain what's happening and why.

- **Don't get _too_ fancy**. Definitely use the most recent versions and features of the target language, but also try to keep the code samples simple and explainable -- the goal is to teach programming in the target language, not necessarily demonstrate the cleverest one-line tricks.

### Have fun!

Thank you for taking part in this project to update a classic programming book -- one of the most influential programming books in computing history -- for 2021!
