Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Python](https://www.python.org/about/) by Alex Conconi, 2026.

---

### Python porting notes

#### Known BASIC bugs — fixed

- **Tie tack unreachable** (BASIC line 3970): The condition
  `IF O/3<>INT(O/3) THEN 4090` is logically inverted. It checks if the 
  tie tack has *not* been sold, and if so, incorrectly jumps to the 
  "Bust" message. This makes the tie tack sale permanently unreachable 
  even if the player still owns it. The Python port fixes this to restore 
  the original intended experience where both assets can be sold.

#### Known BASIC bugs/quirks — kept faithful

- **$50 buy-back materialises from nowhere** (BASIC lines 3570/3640):
  When the player buys back a pawned item, `C=C+50` adds $50 to the
  dealer's stack without deducting from the player.  Kept as-is — it
  models the dealer/pawnbroker liquidating the physical item.

- **Yes/No prompts accept anything** (e.g. BASIC lines 3550, 3880):
  `LEFT$(J$,1)="Y"` treats every non-"Y" input as "no" with no
  re-prompt.  The Python port normalises all yes/no prompts to a single
  `_read_yes_no` that retries on invalid input (matching the stricter
  "Do you wish to continue?" prompt at lines 4120–4150).

