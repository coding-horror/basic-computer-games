Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Python](https://www.python.org/about/)


## Porting Notes

Variables:

* C: Do you want instructions?
* A$(7): Team name + player names (6 players)
* B$(7): Team name + player names (6 players)
* T6: Minutes per game
* R: REFEREE

Functions:

* REM: A line comment


---

Looking at the JS implementation:

* as[7] / bs[7]: The team name
* ha[8] : Score of team B
* ha[9] : Score of team A
