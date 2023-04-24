#!/usr/bin/perl

# Flip Flop program in Perl
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;
use Math::Trig;

print "\n";
print " " x 32, "FLIPFLOP";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";
# *** CREATED BY MICHAEL CASS

print "THE OBJECT OF THIS PUZZLE IS TO CHANGE THIS:\n\n";
print "X X X X X X X X X X\n\n";
print "TO THIS:\n\n";
print "O O O O O O O O O O\n\n";
print "BY TYPING THE NUMBER CORRESPONDING TO THE POSITION OF THE\n";
print "LETTER ON SOME NUMBERS, ONE POSITION WILL CHANGE, ON\n";
print "OTHERS, TWO WILL CHANGE.  TO RESET LINE TO ALL X'S, TYPE 0\n";
print "(ZERO) AND TO START OVER IN THE MIDDLE OF A GAME, TYPE \n";
print "11 (ELEVEN).\n\n";

sub initialize
{
    my @a;
    print "1 2 3 4 5 6 7 8 9 10\n";
    print "X X X X X X X X X X\n";
    for my $i (0 .. 10) { $a[$i] = "X"; } # make sure [0] has a value just in case
    return @a;
}

while (1)
{
    my $Q = rand(1);
    my $C = 0;
    print "HERE IS THE STARTING LINE OF X'S.\n\n";
    my @A = initialize();

    while (1)
    {
        my $M = 0;
        my $N;
        while (1)
        {
            print "\nINPUT THE NUMBER: ";
            chomp($N = <>);
            if ($N != int($N) || $N < 0 ||  $N > 11)
            {
                print "ILLEGAL ENTRY--TRY AGAIN.\n";
                next;
            }
            last;
        }
        if ($N == 11) # start a new game
        {
            print "\n\n";
            last;
        }
        if ($N == 0) # reset line
        {
            @A = initialize();
            next;
        }

        if ($M != $N)
        {
            $M = $N;
            $A[$N] = ($A[$N] eq "O") ? "X" : "O";
            while ($M == $N)
            {
                my $R = tan($Q + $N / $Q - $N) - sin($Q / $N) + 336 * sin(8 * $N);
                $N = $R - int($R);
                $N = int(10 * $N);
                if ($A[$N] eq "O")
                {
                    $A[$N] = "X";
                    next;
                }
                $A[$N] = "O";
                last; # GOTO 610

                $A[$N] = "X";
            }
        }
        else
        {
            if ($A[$N] ne "O") { $A[$N] = "O"; }
            while ($M == $N)
            {
                my $R = .592 * (1 / tan($Q / $N + $Q)) / sin( $N * 2 + $Q) - cos($N);
                $N = $R - int($R);
                $N = int(10 * $N);
                if ($A[$N] eq "O")
                {
                    $A[$N] = "X";
                    next;
                }
                $A[$N] = "O";
                last;
            }
        }

        print "1 2 3 4 5 6 7 8 9 10\n";
        for my $i (1 .. 10) { print "$A[$i] "; }
        print "\n";
        $C++;
        my $i;
        for ($i=1 ; $i <= 10 ; $i++) {
            last if ($A[$i] ne "O");
        }
        if ($i == 11)
        {
            if ($C <= 12) { print "VERY GOOD.  YOU GUESSED IT IN ONLY $C GUESSES.\n"; }
            else          { print "TRY HARDER NEXT TIME.  IT TOOK YOU $C GUESSES.\n"; }
            last;
        }
    }
    print "DO YOU WANT TO TRY ANOTHER PUZZLE (Y/N): ";
    $_ = <>;
    print "\n";
    last if (m/^n/i);
}
