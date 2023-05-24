#!/usr/bin/perl

# Target program in Perl
#   Modified so that if the user enters "quit" or "stop" for the input, the program will exit.
#   Values can be space and/or comma separated.
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

# globals
my $R = 1;
my $R1 = 57.296;
my $Pi = 3.14159;

print "\n";
print " " x 33, "TARGET\n";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";

print "YOU ARE THE WEAPONS OFFICER ON THE STARSHIP ENTERPRISE\n";
print "AND THIS IS A TEST TO SEE HOW ACCURATE A SHOT YOU\n";
print "ARE IN A THREE-DIMENSIONAL RANGE.  YOU WILL BE TOLD\n";
print "THE RADIAN OFFSET FOR THE X AND Z AXES, THE LOCATION\n";
print "OF THE TARGET IN THREE DIMENSIONAL RECTANGULAR COORDINATES,\n";
print "THE APPROXIMATE NUMBER OF DEGREES FROM THE X AND Z\n";
print "AXES, AND THE APPROXIMATE DISTANCE TO THE TARGET.\n";
print "YOU WILL THEN PROCEEED TO SHOOT AT THE TARGET UNTIL IT IS\n";
print "DESTROYED!\n\n";
print "GOOD LUCK!!\n\n";

while (1)
{
    my $A = rand(1) * 2 * $Pi;
    my $B = rand(1) * 2 * $Pi;
    my $P1 = 100000 * rand(1) + rand(1);
    my $X = sin($B) * cos($A) * $P1;
    my $Y = sin($B) * sin($A) * $P1;
    my $Z = cos($B) * $P1;
    print "RADIANS FROM X AXIS = $A   FROM Z AXIS = $B\n";
    print "TARGET SIGHTED: APPROXIMATE COORDINATES:  X=$X  Y=$Y  Z=$Z\n";

    while (1)
    {
        my $P3;
        $R++;

        if    ($R == 1) { $P3 = int($P1 * .05) * 20; }
        elsif ($R == 2) { $P3 = int($P1 * .1) * 10; }
        elsif ($R == 3) { $P3 = int($P1 * .5) * 2; }
        elsif ($R == 4) { $P3 = int($P1); }
        else            { $P3 = $P1; }

        print "     ESTIMATED DISTANCE: $P3\n\n";
        print "INPUT ANGLE DEVIATION FROM X, DEVIATION FROM Z, DISTANCE: ";
        chomp(my $ans = lc(<>));
        exit(0) if ($ans eq "quit" || $ans eq "stop");

        my ($A1, $B1, $P2) = split(/[,\s]+/, $ans);
        print "\n";

        if ($P2 >= 20)
        {
            $A1 /= $R1;
            $B1 /= $R1;
            print "RADIANS FROM X AXIS = $A1  FROM Z AXIS = $B1\n";
            my $X1 = $P2 * sin($B1) * cos($A1);
            my $Y1 = $P2 * sin($B1) * sin($A1);
            my $Z1 = $P2 * cos($B1);
            my $D = (($X1 - $X) ** 2 + ($Y1 - $Y) ** 2 + ($Z1 - $Z) ** 2) ** (0.5);

            if ($D <= 20)
            {
                print "\n * * * HIT * * *   TARGET IS NON-FUNCTIONAL\n";
                print "\nDISTANCE OF EXPLOSION FROM TARGET WAS $D KILOMETERS.\n";
                print "\nMISSION ACCOMPLISHED IN $R SHOTS.\n";
                last;
            }
            else
            {
                my $X2 = $X1 - $X;
                my $Y2 = $Y1 - $Y;
                my $Z2 = $Z1 - $Z;

                if ($X2 < 0) { print "SHOT BEHIND TARGET ", -$X2, " KILOMETERS.\n"; }
                else         { print "SHOT IN FRONT OF TARGET $X2 KILOMETERS.\n"; }

                if ($Y2 < 0) { print "SHOT TO RIGHT OF TARGET ", -$Y2, " KILOMETERS.\n"; }
                else         { print "SHOT TO LEFT OF TARGET $Y2 KILOMETERS.\n"; }

                if ($Z2 < 0) { print "SHOT BELOW TARGET ", -$Z2, " KILOMETERS.\n"; }
                else         { print "SHOT ABOVE TARGET $Z2 KILOMETERS.\n"; }

                print "APPROX POSITION OF EXPLOSION:  X=$X1   Y=$Y1   Z=$Z1\n";
                print "     DISTANCE FROM TARGET = $D\n\n\n";
                next;
            }
        }
        else
        {
            print "YOU BLEW YOURSELF UP!!\n";
            last;
        }
    }

    $R = 0;
    print "\n\n\n\n\nNEXT TARGET...\n\n";
}
