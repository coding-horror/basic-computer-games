### King

This is one of the most comprehensive, difficult, and interesting games. (If you’ve never played one of these games, start with HAMMURABI.)

In this game, you are Premier of Setats Detinu, a small communist island 30 by 70 miles long. Your job is to decide upon the budget of your country and distribute money to your country from the communal treasury.

The money system is Rollods; each person needs 100 Rallods per year to survive. Your country’s income comes from farm produce and tourists visiting your magnificent forests, hunting, fishing, etc. Part of your land is farm land but it also has an excellent mineral content and may be sold to foreign industry for strip mining. Industry import and support their own workers. Crops cost between 10 and 15 Rallods per square mile to plant, cultivate, and harvest. Your goal is to complete an eight-year term of office without major mishap. A word of warning: it isn’t easy!

The author of this program is James A. Storer who wrote it while a student at Lexington High School.

## Bugs

Implementers should be aware that this game contains at least one bug.

On basic line 1450

    1450 V3=INT(A+V3)
    1451 A=INT(A+V3)

...where A is the current treasury, and V3 is initially zero.
This would mean that the treasury doubles at the end of the first year, and all calculations for an increase in the treasury due to tourism are discarded.
Possibly, this made the game more playable, although impossible for the player to understand why the treasury was increasing?

A quick fix for this bug in the original code would be

    1450 V3=ABS(INT(V1-V2))
    1451 A=INT(A+V3)
    
...judging from the description of tourist income on basic line 1410

    1410 PRINT " YOU MADE";ABS(INT(V1-V2));"RALLODS FROM TOURIST TRADE."

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=96)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=111)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html
