#!/usr/bin/perl

use strict;
use warnings;

my $matches = 23;

# Print the whole instruction block
# as a here doc.
print <<HERE;
                              23 Matches
              Creative Computing  Morristown, New Jersey



 This is a game called '23 matches'.

When it is your turn, you may take one, two, or three
matches. The object of the game is not to have to take
the last match.

Let's flip a coin to see who goes first.
If it comes up heads, i will win the toss.

HERE

# standard-issue coin toss logic
if (rand() > 0.5) {
    print "Heads! I win! Ha! Ha\n";
    print "Prepare to lose, meatball-nose!\n\n";
    print "I take 2 matches\n";
    $matches -= 2;
} else {
    print "Tails! You go first.\n\n";
}

# Loop forever. We will exit the loop when someone
# wins (see 'last' statements below).
TURN: while (1) {
    my($user,$computer);

    # By the original BASIC game logic these lines are
    # printed out only after the first move has been
    # taken (by the computer or the user). So if the
    # number of matches is less than 23, then a turn
    # has already been taken at this point, and we
    # print the lines.
    if ($matches < 23) {
        printf("The number of matches is now %d\n\n", $matches);
        print "Your turn -- you may take 1, 2 or 3 matches.\n";
    }
    print "How many do you wish to remove\t? ";

    # This while loop forces the user to enter a
    # value between 1 and 3
    USERIN: while (1) {
        chomp($user = <STDIN>);
        if ($user <= 0 or $user > 3) {
            print "Very funny! Dummy!\n";
            print "Do you want to play or goof around?\n";
            print "Now, how many matches do you want\t? ";
        } else { last USERIN; }
    }
    $matches -= $user;

    # Note the checking of the number to determine
    # whether to print 'is'/'are' etc.
    printf("There %s now %d %s remaining.\n", 
        ($matches == 1 ? 'is' : 'are'), 
        $matches, 
        ($matches == 1 ? 'match' : 'matches')
        );

    if ($matches <= 1) {
        print "\nYou won, floppy ears !\n";
        print "Think you're pretty smart !\n";
        print "Let's play again and I'll blow your shoes off !!\n\n";
        last TURN;
    }

    # The playing strategy is left as an exercise for
    # the reader to figure out.
    if ($matches > 4) {
        $computer = 4 - $user;
    } else {
        $computer = $matches - 1;
    }

    # Again, printing the correct form of 'match'/'matches'
    # depending on the number being printed
    printf("My turn ! I remove %d %s\n", 
        $computer, 
        ($computer == 1 ? 'match' : 'matches')
        );
    $matches -= $computer;

    if ($matches <= 1) {
        print "\nYou poor boob! You took the last match! I gotcha!!\n";
        print "Ha ! Ha ! I beat you !!!\n\n";
        print "Good bye loser!\n\n";
        last TURN;
    }

    # If execution reaches this point, then there 
    # are still matches to take, so we loop back.
}