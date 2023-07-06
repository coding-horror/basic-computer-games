#!/usr/bin/perl

# Gunner program in Perl
#   Required extensive restructuring to remove all of the GOTO's.
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

# globals
my $Max_range = int(40000*rand(1)+20000);
my $Total_shots = 0;
my $Games = 0;

print "\n";
print " " x 30, "GUNNER\n";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";

print "YOU ARE THE OFFICER-IN-CHARGE, GIVING ORDERS TO A GUN\n";
print "CREW, TELLING THEM THE DEGREES OF ELEVATION YOU ESTIMATE\n";
print "WILL PLACE A PROJECTILE ON TARGET.  A HIT WITHIN 100 YARDS\n";
print "OF THE TARGET WILL DESTROY IT.\n\n";
print "MAXIMUM RANGE OF YOUR GUN IS $Max_range YARDS.\n\n";

GAME: while (1)
{
    my $target_dist = int($Max_range * (.1 + .8 * rand(1)));
    my $shots = 0;
    print "DISTANCE TO THE TARGET IS $target_dist YARDS.\n\n";
    while (1)
    {
        my $elevation = get_elevation(); # in degrees
        $shots++;
        my $dist = int($target_dist - ($Max_range * sin(2 * $elevation / 57.3)));
        if (abs($dist) < 100)
        {
            print "*** TARGET DESTROYED ***  $shots ROUNDS OF AMMUNITION EXPENDED.\n";
            $Total_shots += $shots;
            if ($Games++ == 4)
            {
                print "\n\nTOTAL ROUNDS EXPENDED WERE: $Total_shots\n";
                if ($Total_shots > 18) { print "BETTER GO BACK TO FORT SILL FOR REFRESHER TRAINING!\n"; }
                else                   { print "NICE SHOOTING !!\n"; }
                last;
            }
            print "\nTHE FORWARD OBSERVER HAS SIGHTED MORE ENEMY ACTIVITY...\n";
            next GAME;
        }
        if ($dist > 100) { print "SHORT OF TARGET BY ", abs($dist)," YARDS.\n"; }
        else             { print "OVER TARGET BY ", abs($dist), " YARDS.\n"; }

        if ($shots >= 5)
        {
            print "\nBOOM !!!!   YOU HAVE JUST BEEN DESTROYED BY THE ENEMY.\n\n\n\n"; 
            print "BETTER GO BACK TO FORT SILL FOR REFRESHER TRAINING!\n";
            last;
        }
    }

    print "\nTRY AGAIN (Y OR N): ";
    chomp(my $ans=uc(<>));
    if ($ans ne "Y") { last; }
    else             { $Games = 0; $Total_shots = 0; }
}

print "\nOK.  RETURN TO BASE CAMP.\n";

####################################

sub get_elevation
{
    my $elevation;
    while (1)
    {
        print "\nELEVATION: ";
        chomp($elevation = <>);
        if    ($elevation > 89) { print "MAXIMUM ELEVATION IS 89 DEGREES.\n"; }
        elsif ($elevation < 1)  { print "MINIMUM ELEVATION IS ONE DEGREE.\n"; }
        else                    { return $elevation; }
    }
}
