#!/usr/bin/perl

use strict;
use warnings;

# used to check user's input for numerical values
use Scalar::Util qw(looks_like_number);


# global variables defined here
my($MAXNUM) = 9;

# yes_input is a subroutine that returns a true value
# if the first character of the user's input from STDIN
# is a 'Y' (checking case-insensitively via regex)
sub yes_input {
    chomp(my $A = <STDIN>);
    return $A =~ m/^Y/i;
}


# Main code starts here.

print <<HERE;
                               Reverse
              Creative Computing  Morristown, New Jersey



Reverse -- a game of skill

HERE

print "Do you want the rules? ";
if (yes_input()) {
    print <<HERE;

This is the game of 'reverse'.  To win, all you have
to do is arrange a list of numbers (1 through $MAXNUM)
in numerical order from left to right.  To move, you
tell me how many numbers (counting from the left) to
reverse.  For example, if the current list is:

2 3 4 5 1 6 7 8 9

and you reverse 4, the result will be:

5 4 3 2 1 6 7 8 9

Now if you reverse 5, you win!

1 2 3 4 5 6 7 8 9

No doubt you will like this game, but
if you want to quit, reverse 0 (zero).

HERE
}

# The PLAY block allows the user to keep playing
# as long as they want. Note the associated
# continue block at the end of the main block,
# which handles the logic of whether the user
# wants to play again or not.
PLAY: while (1) {

    # start of game. Initialize the number array.
    my(@list) = ();
    my(@digits) = (1 .. $MAXNUM);
    push @list, splice(@digits,int(rand(scalar @digits)),1) while @digits > 0;

    print "\nHere we go ... The list is:\n";
    printf("%s\n", join(' ', @list));

    my($turns) = 0;

    # The TURN block is executed every time the player takes
    # a turn. Exiting this block will mean the user is
    # taking no more turns in the current game, and execution
    # will pass to the continue code of the enclosing block
    # (i.e., the PLAY block).
    TURN: while (1) {
        my($num);

        # the CHECK block forces the user to enter a number
        # from 0 to $MAXNUM (remember, 0 means the end of the 
        # current game, which we do by executing 'last TURN',
        # taking us to the continue block below)
        CHECK: while (1) {
            print "How many shall I reverse ? ";
            chomp($num = <STDIN>);

            # Perl will throw an error if we use the user's input
            # and it isn't an actual number, so this just checks
            # to see if $num looks like a number. (See the docs
            # for Scalar::Util for some more useful subs!)
            if (! looks_like_number($num)) {
                print "Please enter a number from 1 to $MAXNUM\n";
                next CHECK;
            }
            # After the looks_like_number() check it should now
            # be safe to treat $num as a number.
            # 
            # A value of 0 means the user wants to end the current
            # game, which means exiting the TURN loop. Execution now
            # passes to the continue block below. 
            if ($num == 0) { last TURN; }

            if ($num <= $MAXNUM) { # accepting this number, so we exit the CHECK loop
                last CHECK;
            }
            printf("Oops! Too many! I can reverse at most %d\n", $MAXNUM);
        }
        # Keep track of turns
        ++$turns;

        # calculate the reversed list 
        @list = (
            reverse(@list[0 .. $num-1]),  # reversing the first $num elements...
            @list[$num .. $#list]         # ...and appending the non-reversed ones
        );

        printf("%s\n", join(' ', @list));

        # Has the user won?
        for (0 .. $#list) {
            if ($list[$_] != $_ + 1) {
                # Nope, not a correct value. Next turn!
                next TURN;
            }
        }
        # User won!
        printf("\nYou won it in %d %s!!!\n", $turns, ($turns == 1 ? 'move' : 'moves'));

        # Execution passes to the continue block of the PLAY loop
        next PLAY;

    } # end of TURN block
} # end of PLAY block

# If the user gets into the continue block, then they've won
# (or they have ended the current game with an input of 0).
# So we ask if they want to go again. 
# If they don't, then we exit the PLAY block with the
# 'last PLAY' statement.
# If they do, then execution will go to the start of the PLAY
# block automatically, and no further statements are needed.
continue {
    print "\nTry again (yes or no) ? ";

    if (! yes_input()) {
        print "\n\nO.K. Hope you had fun!!\n";
        last PLAY;
    }
}

# This location is reached by the 'last PLAY' statement, and 
# means that the user is done playing. So, nothing else to do.
