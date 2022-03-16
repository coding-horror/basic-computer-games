#!/usr/bin/perl
use strict;


print ' 'x 33 . "GOMOKO\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n"; print "\n"; print "\n";
#my @A;
print "WELCOME TO THE ORIENTAL GAME OF GOMOKO.\n";
print "\n"; print "THE GAME IS PLAYED ON AN N BY N GRID OF A SIZE\n";
print "THAT YOU SPECIFY. DURING YOUR PLAY, YOU MAY COVER ONE GRID\n";
print "INTERSECTION WITH A MARKER. THE OBJECT OF THE GAME IS TO GET\n";
print "5 ADJACENT MARKERS IN A ROW -- HORIZONTALLY, VERTICALLY, OR\n";
print "DIAGONALLY. ON THE BOARD DIAGRAM, YOUR MOVES ARE MARKED\n";
print "WITH A '1' AND THE COMPUTER MOVES WITH A '2'.\n";
print "\n"; print "THE COMPUTER DOES NOT KEEP TRACK OF WHO HAS WON.\n";
print "TO END THE GAME, TYPE -1,-1 FOR YOUR MOVE.\n"; print "\n";


my $Ret;
my $I;
my $J;

my @Board;
my $Size= 0;


while (1) {

	do {
		$Size= 0;
		print "WHAT IS YOUR BOARD SIZE (MIN 7/ MAX 19)"; print "? "; chomp($Size = uc(<STDIN>));
		if ($Size<7 || $Size>19) {
			$Size=0;
			print "I SAID, THE MINIMUM IS 7, THE MAXIMUM IS 19.\n";
			}
		} until ($Size);

	#==> Reset Board to zeroes...
	for (my $I=1; $I<=$Size; $I++) {
		for (my $J=1; $J<=$Size; $J++) {
			$Board[$I][$J]= 0;
			}
		}

	print "\n"; print "WE ALTERNATE MOVES. YOU GO FIRST...\n"; print "\n";

	while (1) {
		do {
			print "YOUR PLAY (I,J)"; print "? "; chomp(my $Inp = uc(<STDIN>));
			($I, $J)= split(",", $Inp);
			print "\n";
			if ($I==-1) { last; }
			$Ret= &ValidMove($I, $J, 1);
			} until ($Ret==1);
		if ($I==-1) { last; }
		$Board[$I][$J]= 1;

		my $X;
		my $Y;
		my $Found=0;
		# REM *** COMPUTER TRIES AN INTELLIGENT MOVE ***
		#==> Too complex, original basic code seems only move below user.
		$Ret= &ValidMove($I+1, $J);
		if ($Ret==1) {
			$Found=1;
			$X= $I+1;
			$Y= $J;
			}

		while($Found==0) {
		# REM *** COMPUTER TRIES A RANDOM MOVE ***
			$X= int($Size*rand(1)+1);
			$Y= int($Size*rand(1)+1);
			$Ret= &ValidMove($X, $Y, 2);
			if ($Ret==1) { $Found= 1; }
			};
		$Board[$X][$Y]=2;

		&ShowBoard();
		}

	print "\n"; print "THANKS FOR THE GAME!!\n";
	print "PLAY AGAIN (1 FOR YES, 0 FOR NO)"; print "? "; chomp(my $Q = uc(<STDIN>));
	if ($Q==0) { last; }
	}



exit;


sub ShowBoard {
	for (my $I=1; $I<=$Size; $I++) {
		print " ";
		for (my $J=1; $J<=$Size; $J++) {
			print "$Board[$I][$J]  ";
			}
		print "\n";
		}
	print "\n";
	return;
	}


sub ValidMove {
	my ($X, $Y, $Val)= @_;
	if ($X<1 || $X>$Size || $Y<1 || $Y>$Size) {
		if ($Val==1) { print "ILLEGAL MOVE. TRY AGAIN...\n"; }
		return 0;
		}
	if ($Board[$X][$Y]!=0) {
		if ($Val==1) { print "SQUARE OCCUPIED. TRY AGAIN...\n"; }
		return 0;
		}

	#$Board[$X][$Y]= $Val;
	return 1;
	}
