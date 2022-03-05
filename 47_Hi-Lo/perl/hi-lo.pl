#!/usr/bin/perl
use strict;


print ' 'x 34 . "HI LO\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n"; print "\n"; print "\n";
print "THIS IS THE GAME OF HI LO.\n"; print "\n";
print "YOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE\n";
print "HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS. IF YOU\n";
print "GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!\n";
print "THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY. HOWEVER,\n";
print "IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.\n"; print "\n";
my $R=0;
my $A;
do {
	print "\n";
	my $Y=int(100*rand(1));
	foreach (1..6) {
		print "YOUR GUESS $Y";
		print "? "; chomp($A = <STDIN>);
		if ($A eq $Y) { last; }
		if ($A>$Y) {
			print "YOUR GUESS IS TOO HIGH.\n";
			} else {
			print "YOUR GUESS IS TOO LOW.\n";
			}
		print "\n";
		}

	if ($A==$Y) {
		$R=$R+$Y;
		print "GOT IT!!!!!!!!!! YOU WIN $Y DOLLARS.\n";
		print "YOUR TOTAL WINNINGS ARE NOW $R DOLLARS.\n";
		} else {
		$R=0;
		print "YOU BLEW IT...TOO BAD...THE NUMBER WAS $Y"
		}
	print "\n"; print "PLAY AGAIN (YES OR NO)";
	print "? "; chomp($A = <STDIN>);
	} until (uc($A) ne "YES");
print "\n"; print "SO LONG. HOPE YOU ENJOYED YOURSELF!!!\n";
exit;
