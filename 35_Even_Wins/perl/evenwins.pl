#!/usr/bin/perl

use strict;
use warnings;

&main;

sub main {
	&print_intro;
	&game_play;
} 

sub game_play {
	my $marbles = 27;
	my $turn = 0;
	my $player_total = 0;
	my $computer_total = 0;
	print "TYPE A '1' IF YOU WANT TO GO FIRST AND TYPE A '0' IF YOU WANT ME TO GO FIRST\n";
	my $choice = <STDIN>;
	chomp($choice);
	if ($choice == 0) {
		until ($marbles == 0) {

			my $computer_choice = &computer_select($marbles,$turn,$player_total);
			$marbles = $marbles - $computer_choice;
			$computer_total = $computer_total + $computer_choice;
			print "MY TOTAL IS $computer_total\n";

			print "TOTAL= $marbles\n";	

			if ($marbles == 0) {&determine_winner($computer_total,$player_total)};

			my $player_choice = &player_select($marbles,$turn);
			$marbles = $marbles - $player_choice;
			$player_total = $player_total + $player_choice;
			print "YOUR TOTAL IS $player_total\n";
			$turn++;
			print "TOTAL= $marbles\n";	
			if ($marbles == 0) {&determine_winner($computer_total,$player_total)};
		}
	}
	elsif ($choice == 1) {
		until ($marbles == 0) {

			my $player_choice = &player_select($marbles,$turn);
			$marbles = $marbles - $player_choice;
			$player_total = $player_total + $player_choice;
			$turn++;
			print "YOUR TOTAL IS $player_total\n";

			print "TOTAL= $marbles\n";	
			
			if ($marbles == 0) {&determine_winner($computer_total,$player_total)};

			my $computer_choice = &computer_select($marbles,$turn,$player_total);
			$marbles = $marbles - $computer_choice;
			$computer_total = $computer_total + $computer_choice;
			print "MY TOTAL IS $computer_total\n";

			print "TOTAL= $marbles\n";	

			if ($marbles == 0) {&determine_winner($computer_total,$player_total)};

		}
	}
}

sub determine_winner {
	my $computer = shift;
	my $player = shift;
	print "THAT IS ALL OF THE MARBLES.\n\n";
	print "MY TOTAL IS $computer, YOUR TOTAL IS $player\n";
	if ($player % 2 == 0) {
		print "     YOU WON.\n";
	}
	if ($computer % 2 == 0) {
		print "     I WON.\n";
	}
	my $answer = -1;
	until ($answer == 1 || $answer == 0) {
		print "DO YOU WANT TO PLAY AGAIN? TYPE 1 FOR YES AND 0 FOR NO.\n";
		$answer=<STDIN>;
		chomp($answer);
	}
	if ($answer == 1) {
		&game_play;
	}
	else {
		print "OK. SEE YOU LATER.\n";
		exit;
	}
}

sub player_select {
	my $marbles = shift;
	my $turn = shift;
	my $validity="invalid";
	if ($turn == 0) {
		print "WHAT IS YOUR FIRST MOVE\n";
	}
	else {
		print "WHAT IS YOUR NEXT MOVE\n";
	}
	until ($validity eq "valid") {
		my $num = <STDIN>;
		chomp($num);
		my $validity=&validity_check($marbles,$num);
		if ($validity eq "valid") {
			return $num;
		}
	}
}

sub computer_select {
	my $marbles = shift;
	my $turn = shift;
	my $player_total = shift;
	my $num = 2;
	my $validity = "invalid";
	if ($turn == 0) {
		print "I PICK UP $num MARBLES.\n\n";
	}
	else {
		until ($validity eq "valid") {
			my $R=$marbles-6*int(($marbles/6));
			
			if (int($player_total/2) == $player_total/2) {
				if ($R < 1.5 || $R > 5.3) {
					$num = 1;
				}
				else {
					$num = $R - 1;
				}
			}
			 
			elsif ($marbles < 4.2) {
				$num = $marbles;
			}
			elsif ($R > 3.4) {
				if ($R < 4.7 || $R > 3.5) {
					$num = 4;
				}
			else {
					$num = 1;
				}
			}
			$validity=&validity_check($marbles,$num);
		}
		print "\nI PICK UP $num MARBLES.\n\n";
	}
	return $num;
}

sub validity_check {
	my $marbles = shift;
	my $num = shift;
	if ($num > $marbles) {
		print "YOU HAVE TRIED TO TAKE MORE MARBLES THAN THERE ARE LEFT. TRY AGAIN. THERE ARE $marbles MARBLES LEFT\n";
		return "invalid";
	}
	if ($num > 0 && $num <= 4) {
		return "valid";
	}
	if ($num < 1 || $num > 4) {
		print "THE NUMBER OF MARBLES YOU TAKE MUST BE A POSITIVE INTEGER BETWEEN 1 AND 4\nWHAT IS YOUR NEXT MOVE?\n";
		return "invalid";
	}
	else {
		return "invalid";
	}
}

sub print_intro {
	print ' ' x 31 . "EVEN WINS\n";
	print ' ' x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n\n\n";
	print "THIS IS A 2 PERSON GAME CALLED 'EVEN WINS'. TO PLAY THE GAME, THE PLAYERS NEED 27 MARBLES OR OTHER OBJECTS ON THE TABLE\n\n";
	print "THE 2 PLAYERS ALTERNATE TURNS, WITH EACH PLAYER REMOVING FROM 1 TO 4 MARBLES ON EACH MOVE. THE GAME ENDS WHEN THERE ARE NO MARBLES LEFT, AND THE WINNER IS THE ONE WITH AN EVEN NUMBER OF MARBLES\n";
	print "THE ONLY RULES ARE THAT (1) YOU MUST ALTERNATE TURNS, (2) YOU MUST TAKE BETWEEN 1 AND 4 MARBLES EACH TURN, AND (3) YOU CANNOT SKIP A TURN.\n\n";
}
