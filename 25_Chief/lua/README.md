Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Lua](https://www.lua.org/) by Alex Conconi

---

### Lua porting notes

- I did not like the old Western movie language style in the game introduction
and decided to tone it down, even if this deviates from the original BASIC
version.

- The `craps_game` function contains the main game logic: it
  - prints the game credits and presents the intro question;
  - asks for the end result and computes the original numer;
  - calls `explain_solution` to print the various steps of the computation;
  - presents the outro question and prints a `bolt` if necessary.

- Added basic input validation to accept only valid integers for numeric input.

- Minor formatting edits (lowercase, punctuation).

- Any answer to a "yes or no" question is regarded as "yes" if the input line
starts with 'y' or 'Y', else no.
