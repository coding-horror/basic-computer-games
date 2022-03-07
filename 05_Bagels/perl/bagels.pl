#!/usr/bin/perl

use strict;
use warnings;

# global variable declaration (just the user's score)
my($Y) = 0;


# yes_input is a subroutine that returns a true value
# if the first character of the user's input from STDIN
# is a 'Y' (checking case-insensitively via regex)
sub yes_input {
    chomp(my $A = <STDIN>);
    return $A =~ m/^Y/i;
}

# Main code starts here.

print ' 'x32; print "Bagels\n";
print ' 'x14; print "Creative Computing  Morristown, New Jersey\n\n";

# Provide instructions if requested
print "Would you like the rules (yes or no)? ";
if (yes_input()) {

    # Print out the instructions using a here doc
    # (useful for large blocks of text)
    print <<HERE;

I am thinking of a three-digit number.  Try to guess
my number and I will give you clues as follows:
  PICO   - one digit correct but in the wrong position
  FERMI  - one digit correct and in the right place
  BAGELS - no digits correct
HERE
}

# There are three code loops here. The outermost one, labeled 'PLAY',
# performs one game start to finish every time it runs.
# The next loop is a for loop that implements the required 20 guesses.
# And the innermost loop, labeled 'CHECK', does the game logic of
# checking the user's guess to make sure it's valid, figuring out
# the response, and rewarding the user if they won.

PLAY: while (1) {

    # The computer's number is three randomly selected unique digits.
    # To generate the number, 3 digits are randomly splice()d out
    # of the @DIGITS array, which is initialized with the digits 0 to 9.
    my @DIGITS = (0..9);
    my @A = ();
    push @A, splice(@DIGITS, int(rand(scalar @DIGITS)), 1) for 1..3;

    print "\n";
    print "O.K.  I have a number in mind.\n";

    for my $i (1..20) {

        # Note that the CHECK loop will automatically loop to ask
        # for the user's input, and it's only with the 'next' and
        # 'last' statements that this loop is exited. So if the
        # user's input is somehow invalid, we just print out an
        # appropriate message. Execution will by default return
        # to the start of the loop, and only leaves the loop if
        # the user's input is successfully parsed and accepted
        # (look for the 'next' and 'last' statements below).

        CHECK: while (1) {
            printf("Guess # %2d ? ", $i);
            chomp(my $A = <STDIN>);

            # Use a regex to check if the user entered three digits,
            # and complain if they did not.
            if ($A !~ m{^(\d)(\d)(\d)$}) {
                print "What?\n";
                # Program execution will now pass through the rest
                # of the logic below and loop back to the start
                # of the CHECK loop.
            } else {

                # As a side effect of the regex match above, the
                # $1, $2, and $3 variables are each of the digits
                # of the user's guess. Perl treats numbers and
                # strings interchangably, so we will not have to
                # use the ASC() conversion functions required
                # by the BASIC program.
                my @B = ($1,$2,$3);

                # Check for duplicate digits in the user's guess
                if ($B[0] == $B[1] || $B[0] == $B[2] || $B[1] == $B[2]) {
                    print "Oh, I forgot to tell you that the number I have in mind\n";
                    print "has no two digits the same.\n";
                    # Again, no further action is required here
                    # because we want to loop back to the start
                    # of the CHECK loop.
                } else {

                    # This code block is the actual game logic, so
                    # it's executed only if the user's input has
                    # passed all the above checks.
                    my($C,$D);
                    $C = 0; $D = 0;

                    # As a replacement for the original BASIC logic,
                    # this for loop works over an anonymous array of
                    # pairs of digits to compare the computer's and
                    # the user's digits to see how many similar ones
                    # there are. Keep in mind that Perl arrays are
                    # zero-indexed, so we're comparing items numbered
                    # 0, 1, and 2, instead of 1, 2, and 3 in BASIC.

                    for my $PAIR ( [0,1], [1,0], [1,2], [2,1], [0,2], [2,0] ) {
                        if ($A[$PAIR->[0]] == $B[$PAIR->[1]]) {
                            ++$C;
                        }
                    }

                    # Check for digits that are correctly guessed
                    for my $i (0..2) {
                        if ($A[$i] == $B[$i]) {
                            ++$D;
                        }
                    }

                    # If the user guessed all 3 digits they get
                    # a point, and the 'PLAY' loop is restarted
                    # (see the 'continue' loop below)
                    if ($D == 3) {
                        print "You got it!!!\n\n";
                        ++$Y;
                        next PLAY;
                    }

                    # Print out the clues. The 'x' operator
                    # prints out the string the indicated number
                    # of times. The "BAGELS" line uses Perl's
                    # ternary operator to print the word if
                    # the expression ($C + $D) is equal to 0.

                    printf("%s%s%s\n",
                        "PICO " x$C,
                        "FERMI "x$D,
                        ($C+$D==0 ? "BAGELS" : '')
                    );

                    # Program execution leaves the CHECK loop and
                    # goes to the next iteration of the $i loop.
                    last CHECK;

                } # end of game logic else block
            } # end of regex match else block

            # If program execution reaches this particular point,
            # then the user's input has not been accepted (the
            # only ways out of this loop are the "next PLAY" statement
            # when the user wins, and the "last CHECK" statement
            # when the user's input is successfully parsed).
            # So the program execution goes back to the top of the
            # CHECK loop, printing the request for user input
            # again.

        } # end of CHECK loop

        # This location is reached by the "last CHECK" statement,
        # and it's another execution of the $i loop.

    } # end of $i loop

    # If program execution reaches here, the user has guessed 20
    # times and not won.

    print "Oh well.\n";
    printf("That's twenty guesses. My number was %s\n", join('',@A));

} # end of the PLAY loop

# This 'continue' block is executed before the conditional part of the
# PLAY loop is evaluated, so we can ask if the user wants another game
# (i.e., if we should restart the PLAY loop).

continue {

    # This 'continue' loop is reached either when the PLAY loop has completed
    # or via the 'next PLAY' statement when the user wins a game. In either
    # case we ask if the player wants to go again, and use the 'last'
    # statement to exit the loop if the response is not yes.
    print "Play again (yes or no) ? ";
    last unless yes_input();
}

# And as in the original BASIC program, print out
# the user's score only if it is > 0.
printf("A %d point bagels buff!\n", $Y) if $Y > 0;
print "Hope you had fun.  Bye.\n";
