Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Lua](https://www.lua.org/) by Alex Conconi

---

#### Lua porting notes

-  The `craps_main` function contains the main game loop, which iteratively
plays craps rounds by calling `play_round` and tracks winnings and losings.
- Replaced the original routine that tries to scramble the random number
generator with a proper seed initializer in Lua: `math.randomseed(os.time())`
(as advised in the general porting notes). 
= Added basic input validation to accept only positive integers for the
wager and the answer to the "If you want to play again print 5" question.
- "If you want to play again print 5 if not print 2" reads a bit odd but
we decided to leave it as is and stay true to the BASIC original version.