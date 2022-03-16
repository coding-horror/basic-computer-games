#!/usr/bin/perl
use strict;

print ' 'x 33 . "NUMBER\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";
print "YOU HAVE 100 POINTS. BY GUESSING NUMBERS FROM 1 TO 5, YOU\n";
print "CAN GAIN OR LOSE POINTS DEPENDING UPON HOW CLOSE YOU GET TO\n";
print "A RANDOM NUMBER SELECTED BY THE COMPUTER.\n"; print "\n";
print "YOU OCCASIONALLY WILL GET A JACKPOT WHICH WILL DOUBLE(!)\n";
print "YOUR POINT COUNT. YOU WIN WHEN YOU GET 500 POINTS.\n";
print "\n"; my $P=100;

Line12:
while ($P<500) {
	print "GUESS A NUMBER FROM 1 TO 5? "; chomp(my $G = <STDIN>);
	my $R= &FNR(1);
	my $S= &FNR(1);
	my $T= &FNR(1);
	my $U= &FNR(1);
	my $V= &FNR(1);
	if ($G eq $R) { $P=$P-5; }
	if ($G eq $S) { $P=$P+5; }
	if ($G eq $T) { $P=$P+$P; print "YOU HIT THE JACKPOT!!!\n"; }
	if ($G eq $U) { $P=$P+1; }
	if ($G eq $V) { $P=$P-($P*.5); }
	if ($G<1 || $G>5) { redo; }
	print "YOU HAVE $P POINTS.\n"; print "\n";
	}
print "!!!!YOU WIN!!!! WITH $P POINTS.\n";
exit;


sub FNR {
	my ($X)= @_; #Useless...
	return int(5*rand(1)+1);
	}
