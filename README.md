### What are we doing?

We’re updating the first million selling computer book, [BASIC Computer Games](https://en.wikipedia.org/wiki/BASIC_Computer_Games), for 2022 and beyond!

- [Read the original book](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf) (pdf)
- [Play the original games in your browser](https://troypress.com/wp-content/uploads/user/js-basic/index.html)

### Where can we discuss it?

Please see [the discussion here](https://discourse.codinghorror.com/t/-/7927) for a worklog and conversation around this project.

### Project structure

I have moved all [the original BASIC source code](http://www.vintage-basic.net/games.html) into a folder for each project in the original book (first volume). Note that Lyle Kopnicky has generously normalized all the code (thanks Lyle!) to run against [Vintage Basic](http://www.vintage-basic.net/download.html) circa 2009:

> I’ve included all the games here for your tinkering pleasure. I’ve tested and tweaked each one of them to make sure they’ll run with Vintage BASIC, though you may see a few oddities. That’s part of the fun of playing with BASIC: it never works quite the same on two machines. The games will play better if you keep CAPS LOCK on, as they were designed to be used with capital-letter input.

Each project has subfolders corresponding to the languages we’d like to see the games ported to. This is based on the [2022 TIOBE index of top languages](https://www.tiobe.com/tiobe-index/) that are _**memory safe**_ and _**general purpose scripting languages**_ per [this post](https://discourse.codinghorror.com/t/-/7927/34):

1. C# 
2. Java
3. JavaScript
4. Kotlin
5. Lua
6. Perl
7. Python
8. Ruby
9. Rust
10. VB.NET

> 📢 Note that in March 2022 we removed Pascal / Object Pascal and replaced it with Rust as we couldn’t determine if Pascal is effectively memory safe. We’ve also added Lua, as it made the top 20 in TIOBE (as of 2022) and it is both memory safe and a scripting language. The Pascal ports were moved to the alternate languages folder.

### Alternate Languages

If you wish to port one of the programs to a language not in our list – that is, a language which is either not memory safe, or not a general purpose scripting language, you can do so via the `00_Alternate_Languages` folder. Place your port in the appropriate game subfolder, in a subfolder named for the language. Please note that these ports are appreciated, but they will not count toward the donation total at the end of the project.

### Project goals

Feel free to begin converting these classic games into the above list of modern, memory safe languages. In fact, courtesy of @mojoaxel, you can even view the JavaScript versions in your web browser at

https://coding-horror.github.io/basic-computer-games/

But first, a few guidelines:

- **These are very old games**. They date from the mid-70s so they’re not exactly examples of what kids (or anyone, really?) would be playing these days. Consider them more like classic programming exercises to teach programming.  We’re paying it forward by converting them into modern languages, so the next generation can learn from the programs in this classic book – and compare implementations across common modern languages.

- **Stay true to the original program**. These are mostly unsophisticated, simple command line / console games, so we should strive to replicate the command line / console output and behavior illustrated in the original book. See the README in the project folder for links to the original scanned source input and output. Try [running the game in your browser](https://troypress.com/wp-content/uploads/user/js-basic/index.html). Avoid the impulse to add features; keep it simple, _except_ for modern conventions, see next item 👇

- **Please DO update for modern coding conventions**. Support uppercase and lowercase. Use structured programming. Use subroutines. Try to be an example of good, modern coding practices!

- **Use lots of comments to explain what is going on**. Comment liberally! If there were clever tricks in the original code, decompose those tricks into simpler (even if more verbose) code, and use comments to explain what’s happening and why. If there is something particularly tricky about a program, edit the **Porting Notes** section of the `readme.md` to let everyone know. Those `GOTO`s can be very pesky..

- **Don’t get _too_ fancy**. Definitely use the most recent versions and features of the target language, but also try to keep the code samples simple and explainable – the goal is to teach programming in the target language, not necessarily demonstrate the cleverest one-line tricks.

### Emulation and Bugfixes

We want the general behavior of the original programs to be preserved, _however_, we also want to update them, specifically:

- allow both UPPERCASE and lowercase input and display
- incorporate any bugfixes to the original programs; see the `readme.md` in the game folder
- improved error handling for bad or erroneous input

Please note that on the back of the Basic Computer Games book it says **Microsoft 8K Basic, Rev 4.0 was the version David Ahl used to test**, so that is the level of compatibility we are looking for.  QBasic on the DOS emulation is a later version of Basic but one that retains downwards compatibility so far in our testing. We’re working on a recommended emulation to verify behavior.

### Have fun!

Thank you for taking part in this project to update a classic programming book – one of the most influential programming books in computing history – for 2022 and beyond!

NOTE: per [the official blog post announcement](https://blog.codinghorror.com/updating-the-single-most-influential-book-of-the-basic-era/), I will be **donating $5 for each contributed program in the 10 agreed upon languages to [Girls Who Code](https://girlswhocode.com/)**.
