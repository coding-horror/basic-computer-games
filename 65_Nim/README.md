### Nim

NIM is one of the oldest two-person games known to man; it is believed to have originated in ancient China. The name, which was coined by the first mathematician to analyze it, comes from an archaic English verb which means to steal or to take away. Objects are arranged in rows between the two opponents as in the following example:
|         |       |           |
|---------|-------|-----------|
| XXXXXXX | Row 1 | 7 Objects |
| XXXXX   | Row 2 | 5 Objects |
| XXX     | Row 3 | 3 Objects |
| X       | Row 4 | 1 Object  |

Opponents take turns removing objects until there are none left. The one who picks up the last object wins. The moves are made according to the following rules:
1. On any given turn only objects from one row may be removed. There is no restriction on which row or on how many objects you remove. Of course, you cannot remove more than are in the row.
2. You cannot skip a move or remove zero objects.

The winning strategy can be mathematically defined, however, rather than presenting it here, we’d rather let you find it on your own. HINT: Play a few games with the computer and mark down on a piece of paper the number of objects in each stack (in binary!) after each move. Do you see a pattern emerging?

This game of NIM is from Dartmouth College and allows you to specify any starting size for the four piles and also a win option. To play traditional NIM, you would simply specify 7,5,3 and 1, and win option 1.

### Porting Notes

This can be a real challenge to port because of all the `GOTO`s going out of loops down to code. You may need breaks and continues, or other techniques.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=118)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=133)


Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html
