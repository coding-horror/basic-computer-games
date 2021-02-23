#!/usr/bin/perl
use strict;
use warnings;

print ' 'x33 ."TRAIN\n";
print ' 'x15 ."CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";
print "TIME - SPEED DISTANCE EXERCISE\n"; print "\n";


my $A= ""; #We must declare this before...
do {
	my $C= int(25*rand(1))+40;
	my $D= int(15*rand(1))+5;
	my $T= int(19*rand(1))+20;

	print " A CAR TRAVELING $C MPH CAN MAKE A CERTAIN TRIP IN\n";
	print "$D HOURS LESS THAN A TRAIN TRAVELING AT $T MPH.\n";
	print "HOW LONG DOES THE TRIP TAKE BY CAR\n";
	chomp ($A = <STDIN>);

	my $V= $D*$T/($C-$T);
	my $E= int(abs(($V-$A)*100/$A)+.5);
	if ($E>5) { 
		print "SORRY.  YOU WERE OFF BY $E PERCENT.\n";
		} else {
		print "GOOD! ANSWER WITHIN $E PERCENT.\n";
		}

	print "CORRECT ANSWER IS $V HOURS.\n";
	print "\n";
	print "ANOTHER PROBLEM (YES OR NO)\n";
	chomp ($A = uc(<STDIN>)); #Uppercased
	} until ($A ne "YES");
