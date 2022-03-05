#!/usr/bin/perl
use strict;

my $L1;
while (1) {
	print ' 'x 33 . "GUESS\n";
	print ' 'x 15 . "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n";
	print "\n"; print "\n"; print "\n";
	print "THIS IS A NUMBER GUESSING GAME. I'LL THINK\n";
	print "OF A NUMBER BETWEEN 1 AND ANY LIMIT YOU WANT.\n";
	print "THEN YOU HAVE TO GUESS WHAT IT IS.\n";
	print "\n";
	print "WHAT LIMIT DO YOU WANT";
	print "? "; chomp(my $L = <STDIN>);
	print "\n";
	$L1= int(log($L)/log(2))+1;

	while (1) {
		print "I'M THINKING OF A NUMBER BETWEEN 1 AND $L\n";
		my $G=0;
		print "NOW YOU TRY TO GUESS WHAT IT IS.\n";
		my $M=int($L*rand(1)+1);
		my $N=0;
		while (1) {
			while (1) {
				print "? "; chomp($N = <STDIN>);
				if ($N>0) { last; }
				}
			$G=$G+1;
			if ($N==$M) { last; }
			if ($N>$M) { print "TOO HIGH. TRY A SMALLER ANSWER.\n"; }
				else { print "TOO LOW. TRY A BIGGER ANSWER.\n"; }
			}
		print "THAT'S IT! YOU GOT IT IN $G TRIES.\n";
		if ($G<$L1) { print "VERY "; }
		if ($G<=$L1) { print "GOOD.\n"; }
		if ($G>$L1) { print "YOU SHOULD HAVE BEEN ABLE TO GET IT IN ONLY $L1\n"; }
		&ENTERS();
		}
	}

exit;


sub ENTERS { #GOSUB 70
	for (my $H=1; $H<=5; $H++) {
		print "\n";
		}
	return;
	}
