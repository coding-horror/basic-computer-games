#!/usr/bin/perl
use strict;

print ' 'x33 . "LETTER\n";
print ' 'x15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";

print "LETTER GUESSING GAME\n\n";
print "I'LL THINK OF A LETTER OF THE ALPHABET, A TO Z.\n";
print "TRY TO GUESS MY LETTER AND I'LL GIVE YOU CLUES\n";
print "AS TO HOW CLOSE YOU'RE GETTING TO MY LETTER.\n";

while (1) {
	my $letter = 65 + int(rand(26));
	my $guesses = 0;
	print "\nO.K., I HAVE A LETTER. START GUESSING.\n";

	my $answer;
	do {
		print "\nWHAT IS YOUR GUESS? ";
		$guesses++;
		chomp($answer = <STDIN>);
		$answer = ord($answer);
		print "\n";
		print "TOO LOW. TRY A HIGHER LETTER.\n" if $answer < $letter;
		print "TOO HIGH. TRY A LOWER LETTER.\n" if $answer > $letter;
	} until($answer eq $letter);

	print "\nYOU GOT IT IN $guesses GUESSES!!\n";

	if ($guesses <= 5) {
		print "GOOD JOB !!!!!\n";
		print chr(7) x 15; # ASCII Bell
	} else {
		print "BUT IT SHOULDN'T TAKE MORE THAN 5 GUESSES!\n";
	}

	print "\nLET'S PLAY AGAIN.....\n";
}

exit;
