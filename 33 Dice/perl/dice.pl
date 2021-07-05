#!/usr/bin/perl
use strict;


print ' 'x 34 . "DICE\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";
my @F;

#REM DANNY FREIDUS;
print "THIS PROGRAM SIMULATES THE ROLLING OF A\n";
print "PAIR OF DICE.\n";
print "YOU ENTER THE NUMBER OF TIMES YOU WANT THE COMPUTER TO\n";
print "'ROLL' THE DICE. WATCH OUT, VERY LARGE NUMBERS TAKE\n";
print "A LONG TIME. IN PARTICULAR, NUMBERS OVER 5000.\n";

my $X;
my $Z;
do {
	for (my $Q=1; $Q<=12; $Q++) {
		$F[$Q]=0;
		}
	print "\n"; print "HOW MANY ROLLS";
	print "? "; chomp($X = <STDIN>);
	for (my $S=1; $S<=$X; $S++) {
		my $A=int(6*rand(1)+1);
		my $B=int(6*rand(1)+1);
		my $R=$A+$B;
		$F[$R]=$F[$R]+1;
		}
	print "\n";
	print "TOTAL SPOTS\tNUMBER OF TIMES\n";
	for (my $V=2; $V<=12; $V++) {
		print "$V\t\t$F[$V]\n";
		}
	print "\n";
	print "\n"; print "TRY AGAIN";
	print "? "; chomp($Z = <STDIN>);
	} until (uc($Z) ne "YES");
exit;


