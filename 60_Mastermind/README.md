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
### Implementation notes
in [#613](https://github.com/coding-horror/basic-computer-games/pull/613)
`1060 IF B1<>B OR W1<>W THEN I(X)=0`
was changed to:
`1060 IF B1>B OR W1>W THEN I(X)=0`
This was done because of a bug:
Originally, after guessing and getting feedback, the computer would look through every possible combination, and for all that haven't previously been marked as impossible it would check whether or not the black and white pins that that combination should get are not-equal to what its previous guess got and, if they are equal, the combination would be marked as possible, and if they aren't equal then the combination would be marked as impossible. This results in a bug where the computer eliminates the correct answer as a possible solution after the first guess, unless the first guess just happens to be correct.

this was discussed in more detail in [issue #611](https://github.com/coding-horror/basic-computer-games/issues/611)

additionally, it's recommended that you have the computer elimate it's previous guess as possible unless that guess was correct. (the rust port does this)
---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=110)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=125)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html
