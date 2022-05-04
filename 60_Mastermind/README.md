### MasterMind

In that Match-April 1976 issue of _Creative_ we published a computerized version of Master Mind, a logic game. Master Mind is played by two people—one is called the code-maker; the other, the code-breaker. At the beginning of the game the code-maker forms a code, or combination of colored pegs. He hides these from the code-breaker. The code-breaker then attempts to deduce the code, by placing his own guesses, one at a time, on the board. After he makes a guess (by placing a combination of colored pegs on the board) the code-maker then gives the code-breaker clues to indicate how close the guess was to the code. For every peg in the guess that’s the right color but not in the right position, the code-breaker gets a white peg. Note that these black and white pegs do not indicate _which_ pegs in the guess are correct, but merely that they exist. For example, if the code was:
```
Yellow Red Red Green
```

and my guess was
```
Red Red Yellow Black
```
I would receive two white pegs and one black peg for the guess. I wouldn’t know (except by comparing previous guesses) which one of the pegs in my guess was the right color in the right position.

Many people have written computer programs to play Master Mind in the passive role, i.e., the computer is the code maker and the human is the code-breaker. This is relatively trivial; the challenge is writing a program that can also play actively as a code-breaker.

Actually, the task of getting the computer to deduce the correct combination is not at all difficult. Imagine, for instance, that you made a list of all possible codes. To begin, you select a guess from your list at random. Then, as you receive clues, you cross off from the list those combinations which you know are impossible. For example if your guess is Red Red Green Green and you receive no pegs, then you know that any combination containing either a red or a green peg is impossible and may be crossed of the list. The process is continued until the correct solution is reached or there are no more combinations left on the list (in which case you know that the code-maker made a mistake in giving you the clues somewhere).

Note that in this particular implementation, we never actually create a list of the combinations, but merely keep track of which ones (in sequential order) may be correct. Using this system, we can easily say that the 523rd combination may be correct, but to actually produce the 523rd combination we have to count all the way from the first combination (or the previous one, if it was lower than 523). Actually, this problem could be simplified to a conversion from base 10 to base (number of colors) and then adjusting the values used in the MID$ function so as not to take a zeroth character from a string if you want to experiment. We did try a version that kept an actual list of all possible combinations (as a string array), which was significantly faster than this version, but which ate tremendous amounts of memory.

At the beginning of this game, you input the number of colors and number of positions you wish to use (which will directly affect the number of combinations) and the number of rounds you wish to play. While you are playing as the code-breaker, you may type BOARD at any time to get a list of your previous guesses and clues, and QUIT to end the game. Note that this version uses string arrays, but this is merely for convenience and can easily be converted for a BASIC that has no string arrays as long as it has a MID$ function. This is because the string arrays are one-dimensional, never exceed a length greater than the number of positions and the elements never contain more than one character.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=110)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=125)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

### How the computer deduces your guess.

The computer takes the number of black pegs and white pegs that the user reports
and uses that information as a target. It then assumes its guess is the answer
and proceeds to compare the black and white pegs against all remaining possible
answers. For each set of black and white pegs it gets in these comparisons, if 
they don't match what the user reported, then they can not be part of the solution.
This can be a non-intuitive assumption, so we'll walk through it with a three color,
three position example (27 possible solutions.)

Let's just suppose our secret code we're hiding from the computer is `BWB`

First let's point out the commutative property of comparing two codes for their
black and white pegs. A black peg meaning correct color and correct position, and
a white peg meaning correct color and wrong position.  If the computer guesses
`RBW` then the black/white peg report is 0 black, 2 white.  But if `RBW` is the 
secret code and the computer guesses `BWB` the reporting for `BWB` is going to be
the same, 0 black, 2 white. 

Now lets look at a table with the reporting for every possible guess the computer 
can make while our secret code is `BWB`.
                                                         
| Guess | Black | White |     | Guess | Black | White |     | Guess | Black | White |
|-------|-------|-------|-----|-------|-------|-------|-----|-------|-------|-------|
| BBB   | 2     | 0     |     | WBB   | 1     | 2     |     | RBB   | 1     | 1     |   
| BBW   | 1     | 2     |     | WBW   | 0     | 2     |     | RBW   | 0     | 2     |   
| BBR   | 1     | 1     |     | WBR   | 0     | 2     |     | RBR   | 0     | 1     |    
| BWB   | 3     | 0     |     | WWB   | 2     | 0     |     | RWB   | 2     | 0     |    
| BWW   | 2     | 0     |     | WWW   | 1     | 0     |     | RWW   | 1     | 0     |    
| BWR   | 2     | 0     |     | WWR   | 1     | 0     |     | RWR   | 1     | 0     |    
| BRB   | 2     | 0     |     | WRB   | 1     | 1     |     | RRB   | 1     | 0     |    
| BRW   | 1     | 1     |     | WRW   | 0     | 1     |     | RRW   | 0     | 1     |    
| BRR   | 1     | 0     |     | WRR   | 0     | 1     |     | RRR   | 0     | 0     | 

The computer has guessed `RBW` and the report on it is 0 black, 2 white. The code
used to eliminate other solutions looks like this:

`1060 IF B1<>B OR W1<>W THEN I(X)=0`

which says set `RBW` as the secret and compare it to all remaining solutions and 
get rid of any that don't match the same black and white report, 0 black and 2 white. 
So let's do that.

Remember, `RBW` is pretending to be the secret code here. These are the remaining
solutions reporting their black and white pegs against `RBW`.

| Guess | Black | White |     | Guess | Black | White |     | Guess | Black | White |
|-------|-------|-------|-----|-------|-------|-------|-----|-------|-------|-------|
| BBB   | 1     | 0     |     | WBB   | 1     | 1     |     | RBB   | 2     | 0     |   
| BBW   | 2     | 0     |     | WBW   | 2     | 0     |     | RBW   | 3     | 0     |   
| BBR   | 1     | 1     |     | WBR   | 1     | 2     |     | RBR   | 2     | 0     |    
| BWB   | 0     | 2     |     | WWB   | 0     | 2     |     | RWB   | 1     | 2     |    
| BWW   | 1     | 1     |     | WWW   | 1     | 0     |     | RWW   | 2     | 0     |    
| BWR   | 0     | 3     |     | WWR   | 1     | 1     |     | RWR   | 1     | 1     |    
| BRB   | 0     | 2     |     | WRB   | 0     | 3     |     | RRB   | 1     | 1     |    
| BRW   | 1     | 2     |     | WRW   | 1     | 1     |     | RRW   | 2     | 0     |    
| BRR   | 0     | 2     |     | WRR   | 0     | 2     |     | RRR   | 1     | 0     | 

Now we are going to eliminate every solution that **DOESN'T** matches 0 black and 2 white.

| Guess    | Black | White |     | Guess    | Black | White |     | Guess    | Black | White |
|----------|-------|-------|-----|----------|-------|-------|-----|----------|-------|-------|
| ~~~BBB~~ | 1     | 0     |     | ~~~WBB~~ | 1     | 1     |     | ~~~RBB~~ | 2     | 0     |   
| ~~~BBW~~ | 2     | 0     |     | ~~~WBW~~ | 2     | 0     |     | ~~~RBW~~ | 3     | 0     |   
| ~~~BBR~~ | 1     | 1     |     | ~~~WBR~~ | 1     | 2     |     | ~~~RBR~~ | 2     | 0     |    
| BWB      | 0     | 2     |     | WWB      | 0     | 2     |     | ~~~RWB~~ | 1     | 2     |    
| ~~~BWW~~ | 1     | 1     |     | ~~~WWW~~ | 1     | 0     |     | ~~~RWW~~ | 2     | 0     |    
| ~~~BWR~~ | 0     | 3     |     | ~~~WWR~~ | 1     | 1     |     | ~~~RWR~~ | 1     | 1     |    
| BRB      | 0     | 2     |     | ~~~WRB~~ | 0     | 3     |     | ~~~RRB~~ | 1     | 1     |    
| ~~~BRW~~ | 1     | 2     |     | ~~~WRW~~ | 1     | 1     |     | ~~~RRW~~ | 2     | 0     |    
| BRR      | 0     | 2     |     | WRR      | 0     | 2     |     | ~~~RRR~~ | 1     | 0     |          
                                   
 That wipes out all but five solutions. Notice how the entire right column of solutions 
 is eliminated, including our original guess of `RBW`, therefore eliminating any 
 special case to specifically eliminate this guess from the solution set when we first find out
 its not the answer.
 
 Continuing on, we have the following solutions left of which our secret code, `BWB` 
 is one of them. Remember our commutative property explained previously. 

| Guess | Black | White |
|-------|-------|-------|
| BWB   | 0     | 2     |
| BRB   | 0     | 2     |
| BRR   | 0     | 2     |
| WWB   | 0     | 2     |
| WRR   | 0     | 2     |

So for its second pick, the computer will randomly pick one of these remaining solutions. Let's pick
the middle one, `BRR`, and perform the same ritual. Our user reports to the computer 
that it now has 1 black, 0 whites when comparing to our secret code `BWB`. Let's 
now compare `BRR` to the remaining five solutions and eliminate any that **DON'T**
report 1 black and 0 whites.

| Guess    | Black | White |
|----------|-------|-------|
| BWB      | 1     | 0     |
| ~~~BRB~~ | 2     | 0     |
| ~~~BRR~~ | 3     | 0     |
| ~~~WWB~~ | 0     | 1     |
| ~~~WRR~~ | 2     | 0     | 

Only one solution matches and its our secret code! The computer will guess this
one next as it's the only choice left, for a total of three moves. 
Coincidentally, I believe the expected maximum number of moves the computer will 
make is the number of positions plus one for the initial guess with no information.
This is because it is winnowing down the solutions 
logarithmically on average. You noticed on the first pass, it wiped out 22 
solutions. If it was doing this logarithmically the worst case guess would 
still eliminate 18 of the solutions leaving 9 (3<sup>2</sup>).  So we have as
a guideline:

 Log<sub>(# of Colors)</sub>TotalPossibilities
 
but TotalPossibilities = (# of Colors)<sup># of Positions</sup>

so you end up with the number of positions as a guess limit. If you consider the
simplest non-trivial puzzle, two colors with two positions, and you guess BW or 
WB first, the most you can logically deduce if you get 1 black and 1 white is 
that it is either WW, or BB which could bring your total guesses up to three 
which is the number of positions plus one.  So if your computer's turn is taking
longer than the number of positions plus one to find the answer then something 
is wrong with your code. 