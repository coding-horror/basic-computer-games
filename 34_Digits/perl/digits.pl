#!/usr/bin/perl

# Digits program in Perl
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

# globals
my $Answer;

print "\n";
print " " x 33, "DIGITS";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";

print "THIS IS A GAME OF GUESSING.\n";
print "FOR INSTRUCTIONS, TYPE '1', ELSE TYPE '0': ";
chomp($Answer = <>);
if ($Answer == 1)
{
    print "\nPLEASE TAKE A PIECE OF PAPER AND WRITE DOWN\n";
    print "THE DIGITS '0', '1', OR '2' THIRTY TIMES AT RANDOM.\n";
    print "ARRANGE THEM IN THREE LINES OF TEN DIGITS EACH.\n";
    print "I WILL ASK FOR THEN TEN AT A TIME.\n";
    print "I WILL ALWAYS GUESS THEM FIRST AND THEN LOOK AT YOUR\n";
    print "NEXT NUMBER TO SEE IF I WAS RIGHT. BY PURE LUCK,\n";
    print "I OUGHT TO BE RIGHT TEN TIMES. BUT I HOPE TO DO BETTER\n";
    print "THAN THAT. *****\n\n\n";
}

my ($A, $B, $C) = (0, 1, 3);
my (@M, @K, @L); # DIM M(26,2),K(2,2),L(8,2)
while (1)
{
    for my $i (0 .. 26) { for my $j (0 .. 2) { $M[$i][$j] = 1; } }
    for my $i (0 .. 2)  { for my $j (0 .. 2) { $K[$i][$j] = 9; } }
    for my $i (0 .. 8)  { for my $j (0 .. 2) { $L[$i][$j] = 3; } }
    $L[0][0] = $L[4][1] = $L[8][2] = 2;
    my $Z = 26;
    my $Z1 = 8;
    my $Z2 = 2;
    my $X = 0;
    my @N;

    for my $T (1 .. 3)
    {
        my $have_input = 0;
        while (!$have_input)
        {
            $have_input = 1;
            print "\nTEN NUMBERS, PLEASE: ";
            chomp($Answer = <>);
            $Answer = "0 " . $Answer; # need to be 1-based, so prepend a throw-away value for [0]
            @N = split(/\s+/, $Answer);
            for my $i (1 .. 10)
            {
                if (!defined($N[$i]) || ($N[$i] != 0 && $N[$i] != 1 && $N[$i] != 2))
                {
                    print "ONLY USE THE DIGITS '0', '1', OR '2'.\n";
                    print "LET'S TRY AGAIN.";
                    $have_input = 0;
                    last;
                }
            }
        }

        print "\nMY GUESS\tYOUR NO.\tRESULT\tNO. RIGHT\n\n";
        for my $U (1 .. 10)
        {
            my $num = $N[$U];
            my $S = 0;
            my $G;
            for my $J (0 .. 2)
            {
                my $S1 = $A * $K[$Z2][$J] + $B * $L[$Z1][$J] + $C * $M[$Z][$J];
                next if ($S > $S1);
                if ($S >= $S1)
                {
                    next if (rand(1) < .5);
                }
                $S = $S1;
                $G = $J;
            } # NEXT J
            print "  $G\t\t$N[$U]\t\t";
            if ($G == $N[$U])
            {
                $X++;
                print "RIGHT\t$X\n";
                $M[$Z][$num]++;
                $L[$Z1][$num]++;
                $K[$Z2][$num]++;
                $Z -= int($Z / 9) * 9;
                $Z = 3 * $Z + $N[$U];
            }
            else
            {
                print "WRONG\t$X\n";
            }
            $Z1 = $Z - int($Z / 9) * 9;
            $Z2 = $N[$U];
        } # NEXT U
    } # NEXT T

    print "\n";
    if ($X == 10)
    {
        print "I GUESSED EXACTLY 1/3 OF YOUR NUMBERS.\n";
        print "IT'S A TIE GAME.\n";
    }
    elsif ($X > 10)
    {
        print "I GUESSED MORE THAN 1/3, OR $X, OF YOUR NUMBERS.\n";
        print "I WIN.\a\a\a\a\a\a\a\a\a\a"
    }
    else
    {
        print "I GUESSED LESS THAN 1/3, OR $X, OF YOUR NUMBERS.\n";
        print "YOU BEAT ME.  CONGRATULATIONS *****\n";
    }

    print "\nDO YOU WANT TO TRY AGAIN (1 FOR YES, 0 FOR NO): ";
    chomp($Answer = <>);
    last if ($Answer != 1);
}
print "\nTHANKS FOR THE GAME.\n";
