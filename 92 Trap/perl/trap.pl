#!/usr/bin/perl
use strict;

print ' 'x 34 . "TRAP\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n"; print "\n"; print "\n";
my $G=6;
my $N=100;
# REM-TRAP;
# REM-STEVE ULLMAN, 8-1-72;

print "INSTRUCTIONS";
print "? "; chomp(my $Z = uc(<STDIN>));
if (substr($Z,0,1) eq "Y") {
	print "I AM THINKING OF A NUMBER BETWEEN 1 AND $N\n";
	print "TRY TO GUESS MY NUMBER. ON EACH GUESS,\n";
	print "YOU ARE TO ENTER 2 NUMBERS, TRYING TO TRAP\n";
	print "MY NUMBER BETWEEN THE TWO NUMBERS. I WILL\n";
	print "TELL YOU IF YOU HAVE TRAPPED MY NUMBER, IF MY\n";
	print "NUMBER IS LARGER THAN YOUR TWO NUMBERS, OR IF\n";
	print "MY NUMBER IS SMALLER THAN YOUR TWO NUMBERS.\n";
	print "IF YOU WANT TO GUESS ONE SINGLE NUMBER, TYPE\n";
	print "YOUR GUESS FOR BOTH YOUR TRAP NUMBERS.\n";
	print "YOU GET $G GUESSES TO GET MY NUMBER.\n";
	}

while (1) {
	my $Flag= 0;
	my $X=int($N*rand(1))+1;
	for (my $Q=1; $Q<=$G; $Q++) {
		print "\n";
		print "GUESS #$Q ";
		print "? "; chomp(my $Pair= uc(<STDIN>));
		my ($A, $B)= split(",", $Pair);
		if ($A eq $B && $X eq $A) { $Flag=1; last; }

		if ($A>$B) { ($A,$B)= ($B,$A); }
		if ($X>$B) {
			print "MY NUMBER IS LARGER THAN YOUR TRAP NUMBERS.\n";
			next;
			}
		if ($X<$A) {
			print "MY NUMBER IS SMALLER THAN YOUR TRAP NUMBERS.\n";
			next;
			}
		print "YOU HAVE TRAPPED MY NUMBER.\n";
		}

	if ($Flag==0) {
		print "SORRY, THAT'S $G GUESSES. THE NUMBER WAS $X\n";
		} else {
		print "YOU GOT IT!!!\n";
		}

	print "\n";
	print "TRY AGAIN.\n";
	print "\n";
	}
	
exit;


