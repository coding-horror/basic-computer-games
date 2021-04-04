#!/usr/bin/perl
use strict;


print ' 'x 32 . "BULLSEYE\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";

print "IN THIS GAME, UP TO 20 PLAYERS THROW DARTS AT A TARGET\n";
print "WITH 10, 20, 30, AND 40 POINT ZONES. THE OBJECTIVE IS\n";
print "TO GET 200 POINTS.\n\n";

print "THROW\t\tDESCRIPTION\t\tPROBABLE SCORE\n";
print" 1\t\tFAST OVERARM\t\tBULLSEYE OR COMPLETE MISS\n";
print" 2\t\tCONTROLLED OVERARM\t10, 20 OR 30 POINTS\n";
print" 3\t\tUNDERARM\t\tANYTHING\n\n";

my @A; my $M=0; my $R=0; my @S; my @W; for (my $I=1; $I<=20; $I++) { $S[$I]= 0; }
print "HOW MANY PLAYERS? "; chomp(my $N = <STDIN>); print "\n";
for (my $I=1; $I<=$N; $I++) {
	print "NAME OF PLAYER #$I? "; chomp($A[$I] = <STDIN>);
	}

do {
	$R= $R+1; print "\n"; print "ROUND $R---------\n";
	for (my $I=1; $I<=$N; $I++) {
		my $Flag=0;
		my $T;
		do {
			print "\n"; print "$A[$I]'S THROW? "; chomp($T = <STDIN>);
			if ($T<1 || $T>3) { print "INPUT 1, 2, OR 3!\n"; $Flag=0; }
				else { $Flag=1; }
			} until ($Flag==1);

		my $P1; my $P2; my $P3; my $P4;
		if ($T==1) { $P1=.65; $P2=.55; $P3=.5; $P4=.5; }
		if ($T==2) { $P1=.99; $P2=.77; $P3=.43; $P4=.01; }
		if ($T==3) { $P1=.95; $P2=.75; $P3=.45; $P4=.05; }

		my $B=0;
		my $U= rand(1);
		if ($U>= $P1) { print "BULLSEYE!! 40 POINTS!\n"; $B=40; }
			elsif ($U>= $P2) { print "30-POINT ZONE!\n"; $B=30; }
			elsif ($U>= $P3) { print "20-POINT ZONE\n"; $B=20; }
			elsif ($U>= $P4) { print "WHEW! 10 POINTS.\n"; $B=10; }
			else { print "MISSED THE TARGET! TOO BAD.\n"; $B=0; }
		$S[$I]= $S[$I]+$B; print "TOTAL SCORE =$S[$I]";
		}

	for (my $I=1; $I<=$N; $I++) {
		if ($S[$I]>=200) { $M= $M+1; $W[$M]= $I; } 
		}
	} until ($M!=0);

print "\n"; print "WE HAVE A WINNER!!\n\n";
for (my $I=1; $I<=$M; $I++) { print $A[$W[$I]]." SCORED ".$S[$W[$I]]." POINTS.\n"; }
print "\n"; print "THANKS FOR THE GAME.\n"; exit;


