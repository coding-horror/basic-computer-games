Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Lua](https://www.lua.org/) by Alex Conconi

---

### Porting notes for Lua:

- This is a straightfoward port with only minor modifications for input validation and text formatting.
- The "Try again?" question accepts 'y', 'yes', 'n', 'no' (case insensitive), whereas the original BASIC version defaults to no unless 'YES' is typed.
- The "How many rolls?" question presents a more user friendly message in case of invalid input.
