#!/usr/bin/perl

# Combat program in Perl
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

# globals
my $User_army;
my $User_navy;
my $User_AF;
my $Comp_army = 30000;
my $Comp_navy = 20000;
my $Comp_AF = 22000;
my $Attack_type;
my $Attack_num;

print "\n";
print " " x 33, "COMBAT\n";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";

print "I AM AT WAR WITH YOU.\nWE HAVE 72000 SOLDIERS APIECE.\n\n";

do {
    print "DISTRIBUTE YOUR FORCES.\n";
    print "\tME\t  YOU\n";
    print "ARMY\t$Comp_army\t";
    chomp($User_army = <>);
    print "NAVY\t$Comp_navy\t";
    chomp($User_navy = <>);
    print "A. F.\t$Comp_AF\t";
    chomp($User_AF = <>);
} while ($User_army + $User_navy + $User_AF > 72000);

do {
    print "YOU ATTACK FIRST. TYPE (1) FOR ARMY; (2) FOR NAVY;\n";
    print "AND (3) FOR AIR FORCE.\n";
    chomp($Attack_num = <>);
} while ($Attack_type < 1 && $Attack_type > 3);
do {
    print "HOW MANY MEN\n";
    chomp($Attack_type = <>);
} while ($Attack_type < 0
         || ($Attack_num == 1 && $Attack_type > $User_army)
         || ($Attack_num == 2 && $Attack_type > $User_navy)
         || ($Attack_num == 3 && $Attack_type > $User_AF));

if ($Attack_num == 1)
{
    if ($Attack_type<$User_army/3)
    {
        print "YOU LOST $Attack_type MEN FROM YOUR ARMY.\n";
        $User_army = int($User_army-$Attack_type);
    }
    if ($Attack_type<2*$User_army/3)
    {
        print "YOU LOST ", int($Attack_type/3), " MEN, BUT I LOST ", int(2*$Comp_army/3), "\n";
        $User_army = int($User_army-$Attack_type/3);
        $Comp_army = int(2*$Comp_army/3);
    }
    else
    {
        s270();
    }
}
elsif ($Attack_num == 2)
{
    if ($Attack_type < $Comp_navy/3)
    {
        print "YOUR ATTACK WAS STOPPED!\n";
        $User_navy = int($User_navy-$Attack_type);
    }
    if ($Attack_type < 2*$Comp_navy/3)
    {
        print "YOU DESTROYED ", int(2*$Comp_navy/3), " OF MY ARMY.\n";
        $Comp_navy = int(2*$Comp_navy/3);
    }
    else
    {
        s270();
    }
}
else # $Attack_num == 3
{
    if ($Attack_type < $User_AF/3)
    {
        print "YOUR ATTACK WAS WIPED OUT.\n";
        $User_AF = int($User_AF-$Attack_type);
    }
    if ($Attack_type < 2*$User_AF/3)
    {
        print "WE HAD A DOGFIGHT. YOU WON - AND FINISHED YOUR MISSION.\n";
        $Comp_army = int(2*$Comp_army/3);
        $Comp_navy = int($Comp_navy/3);
        $Comp_AF = int($Comp_AF/3);
    }
    else
    {
        print "YOU WIPED OUT ONE OF MY ARMY PATROLS, BUT I DESTROYED\n";
        print "TWO NAVY BASES AND BOMBED THREE ARMY BASES.\n";
        $User_army = int($User_army/4);
        $User_navy = int($User_navy/3);
        $User_AF = int(2*$User_AF/3);
    }
}

print "\n\tYOU\tME\n";
print "ARMY\t$User_army\t$Comp_army\n";
print "NAVY\t$User_navy\t$Comp_navy\n";
print "A. F.\t$User_AF\t$Comp_AF\n";
do {
    print "WHAT IS YOUR NEXT MOVE?\n";
    print "ARMY=1  NAVY=2  AIR FORCE=3\n";
    chomp($Attack_type = <>);
} while ($Attack_type < 1 && $Attack_type > 3);
do {
    print "HOW MANY MEN\n";
    chomp($Attack_num = <>);
} while ($Attack_num < 0
         || ($Attack_type == 1 && $Attack_num > $User_army)
         || ($Attack_type == 2 && $Attack_num > $User_navy)
         || ($Attack_type == 3 && $Attack_num > $User_AF));

if ($Attack_num == 1)
{
    if ($Attack_num < $Comp_army/2)
    {
        print "I WIPED OUT YOUR ATTACK!\n";
        $User_army -= $Attack_num;
    }
    else
    {
        print "YOU DESTROYED MY ARMY!\n";
        $Comp_army = 0;
    }
}
elsif ($Attack_num == 2)
{
    if ($Attack_num < $Comp_navy/2)
    {
        print "I SUNK TWO OF YOUR BATTLESHIPS, AND MY AIR FORCE\n";
        print "WIPED OUT YOUR UNGAURDED CAPITOL.\n";
        $User_army /= 4;
        $User_navy /= 2;
    }
    else
    {
        print "YOUR NAVY SHOT DOWN THREE OF MY XIII PLANES,\n";
        print "AND SUNK THREE BATTLESHIPS.\n";
        $Comp_AF = 2*$Comp_AF/3;
        $Comp_navy /= 2;
    }
}
else # $Attack_num == 3
{
    if ($Attack_num > $Comp_AF/2)
    {
        print "MY NAVY AND AIR FORCE IN A COMBINED ATTACK LEFT\n";
        print "YOUR COUNTRY IN SHAMBLES.\n";
        $User_army /= 3;
        $User_navy /= 3;
        $User_AF /= 3;
    }
    else
    {
        print "ONE OF YOUR PLANES CRASHED INTO MY HOUSE. I AM DEAD.\n";
        print "MY COUNTRY FELL APART.\n";
        $Comp_army = $Comp_navy = $Comp_AF = 0;
    }
}

print "\nFROM THE RESULTS OF BOTH OF YOUR ATTACKS,\n";
my $total_user = $User_army+$User_navy+$User_AF;
my $total_comp = $Comp_army+$Comp_navy+$Comp_AF;
if ($total_user > 3/2*($total_comp))
{
    print "YOU WON, OH! SHUCKS!!!!\n";
}
elsif ($total_user < 2/3*($total_comp))
{
    print "YOU LOST-I CONQUERED YOUR COUNTRY.  IT SERVES YOU\n";
    print "RIGHT FOR PLAYING THIS STUPID GAME!!!\n";
}
else
{
    print "THE TREATY OF PARIS CONCLUDED THAT WE TAKE OUR\n";
    print "RESPECTIVE COUNTRIES AND LIVE IN PEACE.\n";
}
print "\n";
exit(0);

#######################################################

sub s270
{
    print "YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO\n";
    print "OF YOUR AIR FORCE BASES AND 3 ARMY BASES.\n";
    $User_army = int($User_army/3);
    $User_AF = int($User_AF/3);
    $Comp_navy = int(2*$Comp_navy/3);
}
