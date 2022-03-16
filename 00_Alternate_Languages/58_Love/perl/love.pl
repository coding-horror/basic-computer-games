#!/usr/bin/perl
use strict;


print ' 'x 33 . "LOVE\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";
print "A TRIBUTE TO THE GREAT AMERICAN ARTIST, ROBERT INDIANA.\n";
print "HIS GREATEST WORK WILL BE REPRODUCED WITH A MESSAGE OF\n";
print "YOUR CHOICE UP TO 60 CHARACTERS.  IF YOU CAN'T THINK OF\n";
print "A MESSAGE, SIMPLE TYPE THE WORD 'LOVE'\n"; print "\n";
print "YOUR MESSAGE, PLEASE? "; chomp(my $A = <STDIN>);
my $L= length($A);


#Original logic too fuzzy, remaked.
my $Width= 60;
my $Text= substr($A x ($Width/$L+1), 0, $Width);
for (my $I=1; $I<10; $I++) { print "\n"; }

my @Data= (
	60,1,12,26,9,12,3,8,24,17,8,4,6,23,21,6,4,6,22,12,5,6,5,
	4,6,21,11,8,6,4,4,6,21,10,10,5,4,4,6,21,9,11,5,4,
	4,6,21,8,11,6,4,4,6,21,7,11,7,4,4,6,21,6,11,8,4,
	4,6,19,1,1,5,11,9,4,4,6,19,1,1,5,10,10,4,4,6,18,2,1,6,8,11,4,
	4,6,17,3,1,7,5,13,4,4,6,15,5,2,23,5,1,29,5,17,8,
	1,29,9,9,12,1,13,5,40,1,1,13,5,40,1,4,6,13,3,10,6,12,5,1,
	5,6,11,3,11,6,14,3,1,5,6,11,3,11,6,15,2,1,
	6,6,9,3,12,6,16,1,1,6,6,9,3,12,6,7,1,10,
	7,6,7,3,13,6,6,2,10,7,6,7,3,13,14,10,8,6,5,3,14,6,6,2,10,
	8,6,5,3,14,6,7,1,10,9,6,3,3,15,6,16,1,1,
	9,6,3,3,15,6,15,2,1,10,6,1,3,16,6,14,3,1,10,10,16,6,12,5,1,
	11,8,13,27,1,11,8,13,27,1,60
	);


my $Pos=0;
my $Toggle=1;
foreach my $Size (@Data) {
	my $Chunk= $Toggle ? substr($Text, $Pos, $Size) : " " x $Size;
	print $Chunk;
	$Pos+= $Size;
	$Toggle= !$Toggle;
	if ($Pos>= $Width) {
		print "\n";
		$Toggle=1;
		$Pos=0;
		}
	}

for (my $I=1; $I<10; $I++) { print "\n"; }
exit;
