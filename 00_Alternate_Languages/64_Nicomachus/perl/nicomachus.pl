#!/usr/bin/perl
use strict;


print ' 'x 33 . "NICOMA\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";
print "BOOMERANG PUZZLE FROM ARITHMETICA OF NICOMACHUS -- A.D. 90!\n";


while (1) {
	print "\n";
	print "PLEASE THINK OF A NUMBER BETWEEN 1 AND 100.\n";
	print "YOUR NUMBER DIVIDED BY 3 HAS A REMAINDER OF";
	print "? "; chomp(my $A = <STDIN>);
	print "YOUR NUMBER DIVIDED BY 5 HAS A REMAINDER OF";
	print "? "; chomp(my $B = <STDIN>);
	print "YOUR NUMBER DIVIDED BY 7 HAS A REMAINDER OF";
	print "? "; chomp(my $C = <STDIN>);
	print "\n";
	print "LET ME THINK A MOMENT...\n";
	print "\n";
	for (my $I=1; $I<=1500; $I++) { }
	my $D= 70*$A+21*$B+15*$C;

	while ($D>105) {
		$D= $D-105;
		}

	print "YOUR NUMBER WAS $D, RIGHT";

	my $Flag=0;
	do {
		print "? "; chomp($A = uc(<STDIN>));
		print "\n";
		if ($A eq "YES") { print "HOW ABOUT THAT!!\n"; $Flag=1; }
		if ($A eq "NO") { print "I FEEL YOUR ARITHMETIC IS IN ERROR.\n"; $Flag=1; }
		if ($Flag==0) { print "EH? I DON'T UNDERSTAND '$A' TRY 'YES' OR 'NO'.\n"; }
		} until ($Flag==1);

	print "\n";
	print "LET'S TRY ANOTHER.\n";
	} #goto Line20;
exit;
