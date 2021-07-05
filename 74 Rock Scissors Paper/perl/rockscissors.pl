#!/usr/bin/perl
use strict;


print ' 'x 21 . "GAME OF ROCK, SCISSORS, PAPER\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";


my $Q=0;
my $C=0;
my $H=0;

do {
	print "HOW MANY GAMES? "; chomp($Q = <STDIN>);
	if ($Q>10) { print "SORRY, BUT WE AREN'T ALLOWED TO PLAY THAT MANY.\n"; }
	} until ($Q<11);

for (my $G=1; $G<=$Q; $G++) {
	print "\n"; print "GAME NUMBER $G\n";
	my $X=int(rand(1)*3+1);

	my $K=0;
	do {
		print "3=ROCK...2=SCISSORS...1=PAPER\n";
		print "1...2...3...WHAT'S YOUR CHOICE? "; chomp($K = <STDIN>);
		if (($K-1)*($K-2)*($K-3)!=0) { print "INVALID.\n"; $K=0; } 
		} until ($K!=0);


	print "THIS IS MY CHOICE...\n";
	if ($X==1) { print "...PAPER\n"; }
	if ($X==2) { print "...SCISSORS\n"; }
	if ($X==3) { print "...ROCK\n"; }

	#Original logic too complex...
	if ($X==$K) { print "TIE GAME. NO WINNER.\n"; next; }
	my $Who=0;
	if ($X==1 && $K==3) { $Who=1; } #Paper wins over rock.
	if ($X==2 && $K==1) { $Who=1; } #Scissors wins over paper.
	if ($X==3 && $K==2) { $Who=1; } #Rock wins over scissors.
	if ($Who==1) {
		print "WOW! I WIN!!!\n"; $C=$C+1;
		} else {
		print "YOU WIN!!!\n"; $H=$H+1;
		}
	}

print "\n"; print "HERE IS THE FINAL GAME SCORE:\n";
print "I HAVE WON $C GAME(S).\n";
print "YOU HAVE WON $H GAME(S).\n";
print "AND ".($Q-($C+$H))." GAME(S) ENDED IN A TIE.\n";
print "\n"; print "THANKS FOR PLAYING!!\n";
exit;


