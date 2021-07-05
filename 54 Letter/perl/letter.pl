#!/usr/bin/perl
use strict;

print ' 'x33 . "LETTER\n";
print ' 'x15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";

print "LETTER GUESSING GAME\n"; print "\n";
print "I'LL THINK OF A LETTER OF THE ALPHABET, A TO Z.\n";
print "TRY TO GUESS MY LETTER AND I'LL GIVE YOU CLUES\n";
print "AS TO HOW CLOSE YOU'RE GETTING TO MY LETTER.\n";

my $A;
while (1) {
	my $L= 65+int(rand(1)*26);
	my $G= 0;
	print "\n"; print "O.K., I HAVE A LETTER. START GUESSING.\n";

	do {
		print "\n"; print "WHAT IS YOUR GUESS? ";
		$G=$G+1;
		chomp($A= <STDIN>);
		$A= ord($A);
		print "\n";
		if ($A<$L) { print "TOO LOW. TRY A HIGHER LETTER.\n"; }
		if ($A>$L) { print "TOO HIGH. TRY A LOWER LETTER.\n"; }
		} until($A eq $L);

	print "\n"; print "YOU GOT IT IN $G GUESSES!!\n";

	if ($G<=5) {
		print "GOOD JOB !!!!!\n";
		for (my $N=1; $N<=15; $N++) { print chr(7); } #ASCII Bell.
		} else {
		print "BUT IT SHOULDN'T TAKE MORE THAN 5 GUESSES!\n";
		}

	print "\n";
	print "LET'S PLAN AGAIN.....\n";
	}

exit;


