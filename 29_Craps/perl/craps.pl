#!/usr/bin/perl
#

my $bank = 0;

&main;

sub main {
	&print_intro;
	my $continue=5;
	until ($continue != 5) {
		$continue=&game_play;
		&print_bank;
	}
	&final_bank;
}

sub game_play {
	my $point = 0;
	my $continue = 1;
	print "INPUT THE AMOUNT OF YOUR WAGER.\n";
	chomp(my $wager=<STDIN>);
	print "I WILL NOW THROW THE DICE\n";
	until ($continue == 0) {
		my $roll = &roll_dice;
		$continue = &check_value($roll,$wager);
	}
	print "IF YOU WANT TO PLAY AGAIN PRINT 5 IF NOT PRINT 2\n";
	chomp(my $ans=<STDIN>);
	return $ans;
}

sub print_bank {
	if ($bank < 0) {
		print "YOU ARE NOW UNDER \$$bank\n";
	}
	elsif ($bank > 0) {
		print "YOU ARE NOW AHEAD \$$bank\n";
	}
	else {
		print "YOU ARE EVEN AT 0\n";
	}
}

sub final_bank {
	if ($bank < 0) {
		print "TOO BAD, YOU ARE IN THE HOLE. COME AGAIN\n";
	}
	elsif ($bank > 0) {
		print "CONGRATULATIONS---YOU CAME OUT A WINNER. COME AGAIN!\n";
	}
	else {
		print "CONGRATULATIONS---YOU CAME OUT EVEN. NOT BAD FOR AN AMATEUR!\n";
	}
}

sub check_value {
	my $roll = shift;
	my $wager = shift;
	if ($roll == 7 || $roll == 11) {
		print "$roll - NATURAL....A WINNER!!!!\n";
		print "$roll PAYS EVEN MONEY, YOU WIN $wager DOLLARS\n";
		$bank += $wager;
		return 0;
	}
	elsif ($roll == 2 || $roll == 3 || $roll == 12) {
		if ($roll == 2) {
			print "$roll - SNAKE EYES....YOU LOSE.\n";
			print "YOU LOSE $wager DOLLARS.\n";
		}
		else {
			print "$roll - CRAPS...YOU LOSE.\n";
			print "YOU LOSE $wager DOLLARS.\n";
		}
		$bank -= $wager;
		return 0;
	}
	else {
		my $point = $roll;
		print "$point IS THE POINT. I WILL ROLL AGAIN\n";
		until (1==2) {
			$roll = &roll_dice;
			if ($roll == 7) {
				print "$roll YOU LOSE $wager\n";
				$bank -= $wager;
				return 0;
			}
			elsif ($roll == $point) {
				print "$roll - A WINNER..........CONGRATS!!!!!!!!\n";
				my $payout = $wager * 2;
				print "$roll AT 2 TO 1 ODDS PAYS YOU...LET ME SEE...$payout DOLLARS\n";
				$bank += $payout;
				return 0;
			}
			else {
				print "$roll - NO POINT. I WILL ROLL AGAIN\n";
				sleep(1);
			}
		}
	}
}

sub roll_dice {
	my $die1 = 1+int rand(6);
	my $die2 = 1+int rand(6);
	return ($die1 + $die2);
}

sub print_intro {
	print ' ' x 33 . "CRAPS\n";
	print ' ' x 15 . "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";
	print "2,3,12 ARE LOSERS; 4,5,6,8,9,10 ARE POINTS; 7,11 ARE NATURAL WINNERS.\n";
}
