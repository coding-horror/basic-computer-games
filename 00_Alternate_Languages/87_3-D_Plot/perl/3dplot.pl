#!/usr/bin/perl
use strict;
use warnings;

print ' 'x32 ."3D PLOT\n";
print ' 'x15 ."CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";

sub FNA {
	my ($Z)= @_;
	return 30*exp(-$Z*$Z/100);
	}

print "\n";

for (my $X=-30; $X<=30; $X+=1.5) {
	my $L=0;
	my $Line=" "x80; #Empty buffer string;
	my $Y1=5*int(sqrt(900-$X*$X)/5);
	for (my $Y=$Y1; $Y>=-$Y1; $Y-=5) {
		my $Z=int(25+&FNA(sqrt($X*$X+$Y*$Y))-.7*$Y);
		if ($Z<=$L) { next; }
		$L= $Z;
		substr $Line, $Z, 1, "*"; #Plot on the line by sustitution.
		}
	print "$Line\n"; #Now print the line.
	}
