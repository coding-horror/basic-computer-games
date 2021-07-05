#!/usr/bin/perl
use strict;

print ' 'x 33 . "BUNNY\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n"; print "\n"; print "\n";
#REM "BUNNY" FROM AHL'$S 'BASIC COMPUTER GAMES';

# This data contains the letter Bunny A=1, B=2, C=3...
my @A= (2,21,14,14,25);

# This data structure contains the pair of start and finish absolute position to show letters.
# -1 means next line, whatever more than 128 will stop the data read.
my @B= (
	1,2,-1,0,2,45,50,-1,0,5,43,52,-1,0,7,41,52,-1,
	1,9,37,50,-1,2,11,36,50,-1,3,13,34,49,-1,4,14,32,48,-1,
	5,15,31,47,-1,6,16,30,45,-1,7,17,29,44,-1,8,19,28,43,-1,
	9,20,27,41,-1,10,21,26,40,-1,11,22,25,38,-1,12,22,24,36,-1,
	13,34,-1,14,33,-1,15,31,-1,17,29,-1,18,27,-1,
	19,26,-1,16,28,-1,13,30,-1,11,31,-1,10,32,-1,
	8,33,-1,7,34,-1,6,13,16,34,-1,5,12,16,35,-1,
	4,12,16,35,-1,3,12,15,35,-1,2,35,-1,1,35,-1,
	2,34,-1,3,34,-1,4,33,-1,6,33,-1,10,32,34,34,-1,
	14,17,19,25,28,31,35,35,-1,15,19,23,30,36,36,-1,
	14,18,21,21,24,30,37,37,-1,13,18,23,29,33,38,-1,
	12,29,31,33,-1,11,13,17,17,19,19,22,22,24,31,-1,
	10,11,17,18,22,22,24,24,29,29,-1,
	22,23,26,29,-1,27,29,-1,28,29,-1,4096
	);


&ENTERS();
my $L= 64; #REM ASCII LETTER CODE...
print "\n";

my $P=0; #Position read iterator
my $Line= ' 'x 60;
while (1) {
	my $X= $B[$P]; $P++; #Read start position.
	if ($X<0) { print "$Line\n";; $Line= ' 'x 60; next; }
	if ($X>128) { last; }
	my $Y= $B[$P]; $P++; #Read end position.

	for (my $I=$X; $I<=$Y; $I++) { my $J=$I-5*int($I/5);
		substr($Line, $I, 1, chr($L+$A[$J])); #You can change $I for $X to get a nice bunny shadow.
		}
	}

&ENTERS();
exit;


sub ENTERS { #GOSUB 260
	for (my $I=1; $I<=6; $I++) { print chr(10); }
	}
