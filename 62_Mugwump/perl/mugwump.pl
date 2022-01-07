#!/usr/bin/perl

use strict;
use warnings;

# global variables defined here
my(@MUGWUMP) = ();

# subroutines defined here

# init_mugwump: pick the random places for the Mugwumps
sub init_mugwump() {
    @MUGWUMP = (); 
    for (1 .. 4) {
        push @MUGWUMP, [ int(rand 10), int(rand 10) ];
    }
}


# main code starts here

# print introductory text
print <<HERE;
                  Mugwump
Creative Computing  Morristown, New Jersey



The object of this game is to find four Mugwumps
hidden on a 10 by 10 grid.  Homebase is position 0,0.
Any guess you make must be two numbers with each
number between 0 and 9, inclusive.  First number
is distance to right of homebase and second number
is distance above homebase.

You get 10 tries.  After each try, I will tell
you how far you are from each Mugwump.

HERE


# PLAY block implements a complete game, and the
# continue block prints the "let's play again" msg
PLAY: while (1) {

    init_mugwump();
    TURN: for my $turn (1 .. 10) { 

        printf("\nTurn no %d -- what is your guess ? ", $turn);

        # Note that parsing of input is rudimentary, in keeping with the
        # spirit of the original BASIC program. Increased input checks
        # would be a good place to start working on this program!
        chomp(my $in = <STDIN>);
        my($M,$N) = split(/,/,$in);
        $M = int($M);
        $N = int($N);

        for my $i (0 .. $#MUGWUMP) {
            # -1 indicates a Mugwump that was already found
            next if $MUGWUMP[$i]->[0] == -1;

            if ($MUGWUMP[$i]->[0] == $M && $MUGWUMP[$i]->[1] == $N) {
                $MUGWUMP[$i]->[0] = -1;
                printf("You have found Mugwump %d\n", $i+1);
            } else {
                my $d = sqrt(($MUGWUMP[$i]->[0] - $M) ** 2 + ($MUGWUMP[$i]->[1] - $N) ** 2);
                printf("You are %.1f units away from Mugwump %d\n", $d, $i+1);
            }
        }

        # If a Mugwump still has not been found,
        # go to the next turn
        for my $j (0 .. $#MUGWUMP) {
            if ($MUGWUMP[$j]->[0] != -1) {
                next TURN;
            }
        }
        # You win!
        printf("You got all of them in %d %s!\n\n", $turn, ($turn == 1 ? 'turn' : 'turns'));
        # Pass execution down to the continue block
        next PLAY;

    } # end of TURN loop

    print "\nSorry, that's 10 tries.  Here's where they're hiding:\n";
    for my $i (0 .. $#MUGWUMP) {
        printf("Mugwump %d is at (%d, %d)\n", $i+1, $MUGWUMP[$i]->[0], $MUGWUMP[$i]->[1])
            if $MUGWUMP[$i]->[0] != -1;
    }
} 
continue {
    print "\nThat was fun! Let's play again.......\n";
    print "Four more Mugwumps are now in hiding.\n\n";
}

