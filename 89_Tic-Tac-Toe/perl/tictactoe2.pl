#!/usr/bin/perl

use strict;
use warnings;

#GLOBALs
my %board = (
	1	=>	0,
	2	=>	0,
	3	=>	0,
	4	=>	0,
	5	=>	0,
	6	=>	0,
	7	=>	0,
	8	=>	0,
	9	=>	0,
);

my %winning_combos = (
	1	=> [1,2,3],
	2	=> [4,5,6],
	3	=> [7,8,9],
	4	=> [1,4,7],
	5	=> [2,5,8],
	6	=> [3,6,9],
	7	=> [1,5,9],
	8	=> [7,5,3],
);

my $player=100;
my $player_goal=0;
my $computer=100;
my $computer_goal=0;
my $count=0;

&main;

sub main {
	&print_intro;
	print "DO YOU WANT 'X' OR 'O'\n";
	chomp(my $ans = <STDIN>);
	&assign_X_and_O($ans);
	if ($ans eq "X") {
		until ($count >= 9) {
			&player_choice;
			$count++;
			&print_board;
			&check_for_winners;
			&computer_choice;
			$count++;
			&print_board;
			&check_for_winners;
		}
	}
	else {
		until ($count >= 9) {
			&computer_choice;
			$count++;
			&print_board;
			&check_for_winners;
			if ($count >= 9) {
				print "IT'S A DRAW. THANK YOU.\n";
				exit;
			}
			&player_choice;
			$count++;
			&print_board;
			&check_for_winners;
		}
	}
	print "IT'S A DRAW. THANK YOU.\n";
	exit;
}

# This will check to see if anyone has won by adding up the various 3-in-a-row lines.
sub check_for_winners {
	my %tally;
	foreach my $key (keys %winning_combos) {
		foreach my $val (@{$winning_combos{$key}}) {
			$tally{$key}+=$board{$val};
		}
	}
	foreach my $key (keys %tally) {
		if ($tally{$key} == $player_goal) {
			print "YOU BEAT ME!! GOOD GAME.\n";
			exit;
		}
		if ($tally{$key} == $computer_goal) {
			print "I WIN, TURKEY!!!\n";
			exit;
		}
	}
}

#On the computer's turn it will first check to see if it should block the player.  If it finds it isn't going to win or need to block a player, the it will choose a spot to place it's X or O.

sub computer_choice {
	my $move;
	$move=&check_for_blocks_or_wins;;
	if ($move > 9) {
		$move=&check_for_corners;
	}
	print "THE COMPUTER MOVES TO...\n";
	$board{$move}=$computer;
}

sub check_for_corners {
	my @precedence;
	if ($count == 0) {
		@precedence=(1,9,7,3,5,2,4,6,8);
	}
	else {
		@precedence=(5,2,4,6,8,1,9,7,3);
	}
	foreach my $move (@precedence) {
		my $validity=&check_occupation($move);
		if ($validity eq "valid") {
			return $move;
		}
	}
}

sub check_for_blocks_or_wins {
	my %tally;
	my $validity = "invalid";
	my $move = 10;
	foreach my $key (keys %winning_combos) {
		foreach my $val (@{$winning_combos{$key}}) {
			$tally{$key}+=$board{$val};
		}
	}
	foreach my $key (keys %tally) {
		if (abs($tally{$key}) == 2) {
			until ($validity eq "valid") {
				foreach my $val (@{$winning_combos{$key}}) {
					$validity=&check_occupation($val);
					if ($validity eq "valid") {
						$move = $val;
						last;
					}
				}
			}
			return $move;
		}
	}
	return $move;
}

sub player_choice {
	my $validity = "invalid";
	my $ans = "";
	until ($validity eq "valid") {
		print "WHERE DO YOU MOVE? ";
		chomp($ans = <STDIN>);
		$validity=&check_occupation($ans);
		if ($validity eq "invalid") {print "THAT SQUARE IS OCCUPIED.\n\n"}
	}
	$board{$ans}=$player;
}

sub check_occupation {
	my $space = shift;
	if ($board{$space}==0) { return "valid" }
	else {return "invalid"};
}

sub print_board {
	foreach my $num (1..9) {
		my $char = &which_char($board{$num});
		if ($num == 4 || $num == 7) { print "\n---+---+---\n";}
		print "$char";
		if ($num % 3 > 0) { print "!" }
	}
	print "\n";
}

sub which_char {
	my $val=shift;
	if ($val == 0) {return "   ";}
	elsif ($val == 1) {return " X ";}
	else {return " O ";}
}

sub assign_X_and_O {
	my $ans = shift;
	if ($ans eq "X") {
		$player = 1;
		$computer = -1;
		$player_goal=3;
		$computer_goal=-3;
	}
	else {
		$player = -1;
		$computer = 1;
		$player_goal=-3;
		$computer_goal=3;
	}
}

sub print_intro {
	print ' ' x 30 . "TIC-TAC-TOE\n";
	print ' ' x 15 . "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";
	print "THE BOARD IS NUMBERED:\n";
	print "1  2  3\n4  5  6\n7  8  9\n\n\n";
}
