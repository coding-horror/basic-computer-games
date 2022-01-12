#!/usr/bin/perl
use strict;
use warnings;

print ' ' x 30 ."SINE WAVE\n";
print ' ' x 15 ."CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n";
print "\n\n\n\n\n";

my $B=0;

for (my $T=0; $T<40; $T+=.25) {
	my $A=int(26+25*sin($T));
	print ' ' x $A;
	if ($B==0) { print "CREATIVE\n"; }
	if ($B==1) { print "COMPUTING\n"; }
	$B= !$B; #Toggle
	}
