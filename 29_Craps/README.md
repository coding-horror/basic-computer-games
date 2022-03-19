### Craps

This game simulates the game of craps played according to standard Nevada craps table rules. That is:
1. A 7 or 11 on the first roll wins
2. A 2, 3, or 12 on the first roll loses
3. Any other number rolled becomes your “point.”
    - You continue to roll, if you get your point, you win.
    - If you roll a 7, you lose and the dice change hands when this happens.

This version of craps was modified by Steve North of Creative Computing. It is based on an original which appeared one day on a computer at DEC.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=52)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=67)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

    15 LET R=0

`R` is a variable that tracks winnings and losings.  Unlike other games that
start out with a lump sum of cash to spend this game assumes the user has as
much money as they want and we only track how much they lost or won.

      21 LET T=1
      22 PRINT "PICK A NUMBER AND INPUT TO ROLL DICE";
      23 INPUT Z
      24 LET X=(RND(0))
      25 LET T =T+1
      26 IF T<=Z THEN 24

This block of code does nothing other than try to scramble the random number
generator. Random number generation is not random, they are generated from the
previous generated number. Because of the slow speed of these systems back then,
gaming random number generators was a concern, mostly for gameplay quality.
If you could know the "seed value" to the generator then you could effectively
know how to get the exact same dice rolls to happen and change your bet to
maximize your winnings and minimize your losses.

The first reason this is an example of bad coding practice is the user is asked
to input a number but no clue is given as to the use of this number. This number
has no bearing on the game and as we'll see only has bearing on the internal
implementation of somehow trying to get an un-game-able seed for the random number
generator (since all future random numbers generated are based off this seed value.)

The `RND(1)` command generates a number from a seed value that is always
the same, everytime, from when the machine is booted up (old C64 behavior). In
order to avoid the same dice rolls being generated, a special call to `RND(-TI)`
would initialize the random generator with something else. But RND(-TI) is not
a valid command on all systems. So `RND(0)`, which generates a random number
from the system clock is used. But technically this could be gamed because the
system clock was driven by the bootup time, there wasn't a BIOS battery on these
systems that kept an internal real time clock going even when the system was
turned off, unlike your regular PC. Therefore, in order to ensure as true
randomness as possible, insert human reaction time by asking for human input.

But a human could just be holding down the enter key on bootup and that would
just skip any kind of multi-millisecond variance assigned by a natural human
reaction time. So, paranoia being a great motivator, a number is asked of the
user to avoid just holding down the enter key which negates the timing variance
of a human reaction.

What comes next is a bit of nonsense. The block of code loops a counter, recalling
the `RND(0)` function (and thus reseeding it with the system clock value)
and then comparing the counter to the user's number input
in order to bail out of the loop. Because the `RND(0)` function is based off the
system clock and the loop of code has no branching other than the bailout
condition, the loop also takes a fixed amount of time to execute, thus making
repeated calls to `RND(0)` predictive and this scheming to get a better random
number is pointless. Furthermore, the loop is based on the number the user inputs
so a huge number like ten million causes a very noticable delay and leaves the
user wondering if the program has errored. The author could have simply called
`RND(0)` once and used a prompt that made more sense like asking for the users
name and then using that name in the game's replies.

It is advised that you use whatever your languages' random number generator
provides and simply skip trying to recreate this bit of nonsense including
the user input.

      27 PRINT"INPUT THE AMOUNT OF YOUR WAGER.";
      28 INPUT F
      30 PRINT "I WILL NOW THROW THE DICE"
      40 LET E=INT(7*RND(1))
      41 LET S=INT(7*RND(1))
      42 LET X=E+S
      .... a bit later ....
      60 IF X=1 THEN 40
      65 IF X=0 THEN 40

`F` is a variable that represents the users wager for this betting round.
`E` and `S` represent the two individual and random dice being rolled.
This code is actually wrong because it returns a value between 0 and 6.
`X` is the sum of these dice rolls. As you'll see though further down in the
code, if `X` is zero or one it re-rolls the dice to maintain a potential
outcome of the sum of two dice between 2 and 12. This skews the normal distribution
of dice values to favor lower numbers because it does not consider that `E`
could be zero and `S` could be 2 or higher. To show this skewing of values
you can run the `distribution.bas` program which creates a histogram of the
distribution of the bad dice throw code and proper dice throw code.

Here are the results:

      DISTRIBUTION OF DICE ROLLS WITH  INT(7*RND(1))  VS  INT(6*RND(1)+1)
      THE INT(7*RND(1)) DISTRIBUTION:
      2             3             4             5             6             7             8             9             10            11            12
      6483          8662          10772         13232         15254         13007         10746         8878          6486          4357          2123
      THE INT(6*RND(1)+1) DISTRIBUTION
      2             3             4             5             6             7             8             9             10            11            12
      2788          5466          8363          11072         13947         16656         13884         11149         8324          5561          2790
If the dice rolls are fair then we should see the largest occurrence be a 7 and
the smallest should be 2 and 12. Furthermore the occurrences should be
symetrical meaning there should be roughly the same amount of 2's as 12's, the
same amount of 3's as 11's, 4's as 10's and so on until you reach the middle, 7.
But notice in the skewed dice roll, 6 is the most rolled number not 7, and the
rest of the numbers are not symetrical, there are many more 2's than 12's.
So the lesson is test your code.

The proper way to model a dice throw, in almost every language is
    `INT(6*RND(1)+1)` or `INT(6*RND(1))+1`

SideNote: `X` was used already in the
previous code block discussed but its value was never used. This is another
poor coding practice: **Don't reuse variable names for different purposes.**

      50 IF X=7 THEN 180
      55 IF X=11 THEN 180
      60 IF X=1 THEN 40
      62 IF X=2 THEN 195
      65 IF X=0 THEN 40
      70 IF X=2 THEN 200
      80 IF X=3 THEN 200
      90 IF X=12 THEN 200
      125 IF X=5 THEN 220
      130 IF X =6 THEN 220
      140 IF X=8 THEN 220
      150 IF X=9 THEN 220
      160 IF X =10 THEN 220
      170 IF X=4 THEN 220

This bit of code determines the routing of where to go for payout, or loss.
Of course, line 60 and 65 are pointless as we've just shown and should be removed
as long as the correct dice algorithm is also changed.

      62 IF X=2 THEN 195
      ....
      70 IF X=2 THEN 200
The check for a 2 has already been made and the jump is done. Line 70 is
therefore redundant and can be left out. The purpose of line 62 is only to
print a special output, "SNAKE EYES!" which we'll see in the next block creates
duplicate code.

Lines 125-170 are also pointlessly checked because we know previous values have
been ruled out, only these last values must remain, and they are all going to
the same place, line 220. Line 125-170 could have simply been replaced with
`GOTO 220`



      180 PRINT X "- NATURAL....A WINNER!!!!"
      185 PRINT X"PAYS EVEN MONEY, YOU WIN"F"DOLLARS"
      190 GOTO 210
      195 PRINT X"- SNAKE EYES....YOU LOSE."
      196 PRINT "YOU LOSE"F "DOLLARS."
      197 LET F=0-F
      198 GOTO 210
      200 PRINT X " - CRAPS...YOU LOSE."
      205 PRINT "YOU LOSE"F"DOLLARS."
      206 LET F=0-F
      210 LET R= R+F
      211 GOTO 320

This bit of code manages instant wins or losses due to 7,11 or 2,3,12. As
mentioned previously, lines 196 and 197 are essentially the same as lines
205 and 206. A simpler code would be just to jump after printing the special
message of "SNAKE EYES!" to line 205.

Lines 197 and 206 just negate the wager by subtracting it from zero. Just saying
`F = -F` would have sufficed. Line 210 updates your running total of winnings
or losses with this bet.

      220 PRINT X "IS THE POINT. I WILL ROLL AGAIN"
      230 LET H=INT(7*RND(1))
      231 LET Q=INT(7*RND(1))
      232 LET O=H+Q
      240 IF O=1 THEN 230
      250 IF O=7 THEN 290
      255 IF O=0 THEN 230

This code sets the point, the number you must re-roll to win without rolling
a 7, the most probable number to roll. Except in this case again, it has the
same incorrect dice rolling code and therefore 6 is the most probable number
to roll. The concept of DRY (don't repeat yourself) is a coding practice which
encourages non-duplication of code because if there is an error in the code, it
can be fixed in one place and not multiple places like in this code. The scenario
might be that a programmer sees some wrong code, fixes it, but neglects to
consider that there might be duplicates of the same wrong code elsewhere.  If
you practice DRY then you never worry much about behaviors in your code diverging
due to duplicate code snippets.

      260 IF O=X THEN 310
      270 PRINT O " - NO POINT. I WILL ROLL AGAIN"
      280 GOTO 230
      290 PRINT O "- CRAPS. YOU LOSE."
      291 PRINT "YOU LOSE $"F
      292 F=0-F
      293 GOTO 210
      300 GOTO 320
      310 PRINT X"- A WINNER.........CONGRATS!!!!!!!!"
      311 PRINT X "AT 2 TO 1 ODDS PAYS YOU...LET ME SEE..."2*F"DOLLARS"
      312 LET F=2*F
      313 GOTO 210

This is the code to keep rolling until the point is made or a seven is rolled.
Again we see the negated `F` wager and lose message duplicated. This code could
have been reorganized using a subroutine, or in BASIC, the GOSUB command, but
in your language its most likely just known as a function or method. You can
do a `grep -r 'GOSUB'` from the root directory to see other BASIC programs in
this set that use GOSUB.

The rest of the code if fairly straight forward, replay the game or end with
a report of your winnings or losings.
