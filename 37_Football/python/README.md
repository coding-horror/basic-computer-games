Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Python](https://www.python.org/about/)


## Porting notes

Variables:

* E: score_limit
* H(2): Scores
* T(2): Team toggle
* T: team who currently possesses the ball
* L: Offset
* P: Who has the ball
* K: yards
* R: Runback current team in yards
* P$(20): Actions (see data.json)

Functions:

* `P$(I)`: Access index `I` of the `P` array
* ABS: abs (absolute value)
* RND(1): random()
* GOSUB: Execute a function - will jump back to this
* GOTO: Just jump

Patterns:

* `T=T(T)`: Toggle the team who currently has the ball
