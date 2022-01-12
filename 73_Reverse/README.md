### Reverse

The game of REVERSE requires you to arrange a list of numbers in numerical order from left to right. To move, you tell the computer how many numbers (counting from the left) to reverse. For example, if the current list is:
```
    2 3 4 5 1 6 7 8 9
```

and you reverse 4, the result will be:
```
    5 4 3 2 1 6 7 8 9
```
Now if you reverse 5, you win!

There are many ways to beat the game, but approaches tend to be either algorithmic or heuristic. The game thus offers the player a chance to play with these concepts in a practical (rather than theoretical) context.

An algorithmic approach guarantees a solution in a predictable number of moves, given the number of items in the list. For example, one method guarantees a solution in 2N - 3 moves when teh list contains N numbers. The essence of an algorithmic approach is that you know in advance what your next move will be. Once could easily program a computer to do this.

A heuristic approach takes advantage of “partial orderings” in the list at any moment. Using this type of approach, your next move is dependent on the way the list currently appears. This way of solving the problem does not guarantee a solution in a predictable number of moves, but if you are lucky and clever, you may come out ahead of the algorithmic solutions. One could not so easily program this method.

In practice, many players adopt a “mixed” strategy, with both algorithmic and heuristic features. Is this better than either “pure” strategy?

The program was created by Peter Sessions of People’s Computer Company and the notes above adapted from his original write-up.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=135)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=150)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html
