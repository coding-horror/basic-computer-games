#!/usr/bin/perl
use strict;

################
# PORTING NOTES:
# * In basic "Tab" function are not spaces, but absolute col position on screen.
# * It was too dificult to port this one, couldn't figure out the original algorithm.
# * So the algorithm was remake.
#

print ' 'x 33 . "DIAMOND\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n"; print "\n"; print "\n";
print "FOR A PRETTY DIAMOND PATTERN,\n";
print "TYPE IN AN ODD NUMBER BETWEEN 5 AND 21? "; chomp(my $R = <STDIN>); print "\n";


my $Wid= int(60/$R)+1;
my $Dia="CC". "!" x ($R-2);

for (my $J=1; $J<$Wid; $J++) {
	for (my $K=1; $K<($R+2)*2-4; $K+=2) {
		my $Size= $K;
		if ($K>$R) { $Size=$R+($R-$K); }
		my $Chunk= substr($Dia, 0, $Size);
		for (my $L=1; $L<$Wid; $L++) {
			my $Space= " " x (($R-$Size)/2);
			if ($L>1) { $Space.=$Space; }
			print $Space.$Chunk;
			}
		print "\n";
		}
	}

exit;


