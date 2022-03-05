#!/usr/bin/perl

use strict;
use warnings;

&main;

# Main subroutine

sub main {
	&print_intro;
	while (1==1) {
		&game_play; 					#function that actually plays the game
	}
}

sub game_play {
	my $num = 0;
	my $sum = 0;
	my $tries = 0;
	until ($num == 2) {               	# there are 2 dice rolls so we do it until the num equals 2
		$num++;
		my $roll = 1+int rand(6);		# getting a random number between 1 and 6
		&print_dice($roll);				# function call to print out the dice
		$sum = $sum + $roll;			# keeping track of the summary
		#print "Sum: $sum Roll: $roll\n";
		if ($num == 1) {
			print "\n   +\n\n";			# if its the first roll then print an addition sign
		}
		if ($num == 2) {
			print "     =? ";			# if its the second roll print the equals sign and wait for an answer
			my $answer = <STDIN>;
			chomp($answer);
			if ($answer == 0) {
				die "You input '0', Thanks for playing!\n";
			}
			elsif ($answer == $sum) {
				print "RIGHT!\n\nTHE DICE ROLL AGAIN\n\n";
			}
			else {						# code execution if they don't get the right answer
				print "NO,COUNT THE SPOTS AND GIVE ANOTHER ANSWER\n";
				print "     =? ";
				$answer = <STDIN>;
				chomp($answer);
				if ($answer == $sum){
					print "RIGHT!\n\nTHE DICE ROLL AGAIN\n\n";
				}
			    else {
					print "N0, THE ANSWER IS $sum\n";
				}

			}
		}
	}
}

sub print_dice {
	my $roll = shift;
	print " -----\n";
	if ($roll == 1) {
		&print_blank;
		&print_one_mid;
		&print_blank;
	}
	if ($roll == 2) {
		&print_one_left;
		&print_blank;
		&print_one_right;
	}
	if ($roll == 3) {
		&print_one_left;
		&print_one_mid;
		&print_one_right;
	}
	if ($roll == 4) {
		&print_two;
		&print_blank;
		&print_two;
	}
	if ($roll == 5) {
		&print_two;
		&print_one_mid;
		&print_two;
	}
	if ($roll == 6) {
		&print_two;
		&print_two;
		&print_two;
	}
	print " -----\n";
}

sub print_one_left {
	print "I *   I\n";
}

sub print_one_mid {
	print "I  *  I\n";
}

sub print_one_right {
	print "I   * I\n";
}

sub print_two {
	print "I * * I\n";
}

sub print_blank {
	print "I     I\n";
}

sub print_intro {
	my $spaces = " "x31;
	print "$spaces MATH DICE\n";
	$spaces = " "x15;
	print "$spaces CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n";
	print "THIS PROGRAM GENERATES SUCCESSIVE PICTURES OF TWO DICE.\n";
	print "WHEN TWO DICE AND AN EQUAL SIGN FOLLOWED BY A QUESTION\n";
	print "MARK HAVE BEEN PRINTED, TYPE YOUR ANSWER AND THE RETURN KEY.\n";
	print "TO CONCLUDE THE LESSON, TYPE '0' AS YOUR ANSWER.\n\n\n";
}
