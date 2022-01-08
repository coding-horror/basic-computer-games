#!/usr/bin/perl

use strict;
use warnings;

# global variables

my($GRID)  = 10;
my($TRIES) = 5;


# main program starts here

# print instructions
print <<HERE;
                    Hurkle
    Creative Computing  Morristown, New Jersey

A Hurkle is hiding on a ${GRID} by ${GRID} grid. Homebase
on the grid is point 0,0 in the southwest corner,
and any point on the grid is designated by a
pair of whole numbers seperated by a comma. The first
number is the horizontal position and the second number
is the vertical position. You must try to
guess the Hurkle's gridpoint. You get ${TRIES} tries.
After each try, I will tell you the approximate
direction to go to look for the Hurkle.

HERE

# The PLAY block is a complete game from start
# to finish. The continue block prints the 
# "let's play again" message and then a new
# game is started.
PLAY: while (1) {
    my($H1) = int(rand $GRID);
    my($H2) = int(rand $GRID);

    for my $i (1 .. $TRIES) {
        printf(" Guess # %D ? ", $i);

        my($G1,$G2);
        # The CHECK loop will execute while we
        # attempt to collect valid input from
        # the player 
        CHECK: while (1) {

            chomp(my $in = <STDIN>);
            # Use a regex to attempt to parse out 
            # two integers separated by a comma.
            if ($in =~ m{(\d+)\s*,\s*(\d+)}) {
                $G1 = $1; $G2 = $2;
                last CHECK;
            }
            # Input not accepted, please try again
            print "Please enter two numbers separated by a comma ? ";
        }

        if (abs($H1 - $G1) + abs($H2 - $G2) != 0) {

            # print directional info
            printf("Go %s%s\n\n",
                ($G2 == $H2 ? '' : $G2 < $H2 ? 'north' : 'south'),
                ($G1 == $H1 ? '' : $G1 < $H1 ? 'east'  : 'west' ),
            );
        } else {
            # win!
            printf("\nYou found him in %d tries!\n", $i);
            # move to the continue block
            next PLAY;
        }
    } # tries loop

    # No more guesses
    printf("Sorry, that's %d guesses.\n", $TRIES);
    printf("The Hurkle is at %d, %d\n", $H1, $H2);
}

# Execution comes here either from the "next PLAY"
# statement, or by the PLAY block naturally ending
# after the player has lost.
continue {
    print "\nLet's play again. Hurkle is hiding.\n\n";
}
