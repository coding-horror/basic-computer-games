#!/usr/bin/perl

use strict;
use warnings;

print ' ' x 31 . "23 MATCHES\n";
print ' ' x 15 . "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";

print " THIS IS A GAME CALLED '23 MATCHES'.\n\n";

print "WHEN IT IS YOUR TURN, YOU MAY TAKE ONE, TWO, OR THREE\n";
print "MATCHES. THE OBJECT OF THE GAME IS NOT TO HAVE TO TAKE\n";
print "THE LAST MATCH.\n\n";

print "LET'S FLIP A COIN TO SEE WHO GOES FIRST.\n";
print "IF IT COMES UP HEADS, I WILL WIN THE TOSS.\n\n";

my $N = 23;
my $Q = int( 2 * rand(5) );

if ( $Q == 1 ) {
    print "HEADS! I WIN! HA! HA!\n";
    print "PREPARE TO LOSE, MEATBALL-NOSE!!\n\n";

    print "I TAKE 2 MATCHES\n";
    $N -= 2;

    print "THE NUMBER OF MATCHES IS NOW $N\n\n";

    print "YOUR TURN -- YOU MAY TAKE 1, 2 OR 3 MATCHES.\n";
}
else {
    print "TAILS! YOU GO FIRST.\n\n";
}

print "HOW MANY DO YOU WISH TO REMOVE?\n";

INPUT:
{
    chomp( my $K = <STDIN> );

    if ( $K > 3 or $K <= 0 ) {
        print "VERY FUNNY! DUMMY!\n";
        print "DO YOU WANT TO PLAY OR GOOF AROUND?\n";
        print "NOW, HOW MANY MATCHES DO YOU WANT?\n";
        redo INPUT;
    }

    $N -= $K;

    print "THERE ARE NOW $N MATCHES REMAINING.\n";

    my $Z;

    if ( $N <= 1 ) {
        print "YOU WON, FLOPPY EARS!\n";
        print "THINK YOU'RE PRETTY SMART!\n";
        print "LET'S PLAY AGAIN AND I'LL BLOW YOUR SHOES OFF!!\n";
        exit;
    }
    elsif ( $N > 4 ) {
        $Z = 4 - $K;
    }
    else {
        $Z = $N - 1;
    }

    print "MY TURN! I REMOVE $Z MATCHES\n";

    $N -= $Z;

    if ( $N <= 1 ) {
        print "\nYOU POOR BOOB! YOU TOOK THE LAST MATCH! I GOTCHA!!\n";
        print "HA! HA! I BEAT YOU!!!\n\n";

        print "GOOD BYE LOSER!\n";
    }
    else {
        print "THE NUMBER OF MATCHES IS NOW $N\n\n";

        print "YOUR TURN -- YOU MAY TAKE 1, 2 OR 3 MATCHES.\n";
        print "HOW MANY DO YOU WISH TO REMOVE?\n";
        redo INPUT;
    }
}
