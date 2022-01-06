#!/usr/bin/perl

use strict;
use warnings;


#GLOBAL
my %player_bases;
my %computer_bases;
my %player_choices;
my %computer_choices;

&main;

sub main {
	&print_intro;
	&display_field;
	&populate_computer_bases;
	&populate_player_bases;
	&game_play;
}

sub game_play {
	until (keys %computer_bases == 0 || keys %player_bases == 0) {
		&player_turn;
		if (keys %computer_bases == 0) {
			exit;
		}
		&computer_turn;
	}
	exit;
}

sub computer_turn {
	# There is logic in here to ensure that the computer doesn't try to pick a target it has already picked
	my $valid_choice = 0;
	until ($valid_choice == 1) {
		my $target = int(rand(25)+1);
		if (exists $computer_choices{$target}) {
			$valid_choice = 0;
		}
		else {
			$valid_choice = 1;
			$computer_choices{$target}=1;
			if (exists $player_bases{$target}) {
				delete($player_bases{$target});
				my $size = keys %player_bases;
				if ($size > 0) {
					print "I GOT YOU.  IT WON'T BE LONG NOW. POST $target WAS HIT.\n";
					if ($size == 3) { print "YOU HAVE ONLY THREE OUTPOSTS LEFT.\n"};
					if ($size == 2) { print "YOU HAVE ONLY TWO OUTPOSTS LEFT.\n"};
					if ($size == 1) { print "YOU HAVE ONLY ONE OUTPOSTS LEFT.\n"};
				}
				else {
					print "YOU'RE DEAD. YOUR LAST OUTPOST WAS AT $target. HA, HA, HA.\nBETTER LUCK NEXT TIME\n";
				}
				
			}
			else {
				print "I MISSED YOU, YOU DIRTY RAT. I PICKED $target. YOUR TURN:\n";
			}
		}
	}
}

sub player_turn {
	print "WHERE DO YOU WISH TO FIRE YOUR MISSILE\n";
	chomp(my $target=<STDIN>);
	if (exists $computer_bases{$target}) {
		print "YOU GOT ONE OF MY OUTPOSTS!\n";
		delete($computer_bases{$target});
		my $size = keys %computer_bases;
		if ($size == 3) { print "ONE DOWN, THREE TO GO.\n"};
		if ($size == 2) { print "TWO DOWN, TWO TO GO.\n"};
		if ($size == 1) { print "THREE DOWN, ONE TO GO.\n"};
		if ($size == 0) { print "YOU GOT ME, I'M GOING FAST. BUT I'LL GET YOU WHEN\nMY TRANSISTO&S RECUP%RA*E!\n"};
	}
	else {
		print "HA, HA YOU MISSED. MY TURN NOW:\n";
	}
}

sub populate_player_bases {
	print "WHAT ARE YOUR FOUR POSITIONS\n";
	my $positions=<STDIN>;
	chomp($positions);
	my @positions = split/ /,$positions;
	foreach my $base (@positions) {
		$player_bases{$base}=0;
	}
}

sub display_field {
	for my $num (1..25) {
		if (length($num) < 2) {
			$num = " $num";
		}
		print "$num ";
		if ($num % 5 == 0) {
			print "\n";
		}
	}
}

sub populate_computer_bases {
	my $size = 0;
	until ($size == 4) {
		my $base = int(rand(25)+1);
		$computer_bases{$base}=0;
		$size = keys %computer_bases;
	}
}

sub print_intro {
    print "" * 33 + "BOMBARDMENT\n";
    print "" * 15 + " CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n";
    print "\n\n";
    print "YOU ARE ON A BATTLEFIELD WITH 4 PLATOONS AND YOU\n";
    print "HAVE 25 OUTPOSTS AVAILABLE WHERE THEY MAY BE PLACED.\n";
    print "YOU CAN ONLY PLACE ONE PLATOON AT ANY ONE OUTPOST.\n";
    print "THE COMPUTER DOES THE SAME WITH ITS FOUR PLATOONS.\n\n";
    print "THE OBJECT OF THE GAME IS TO FIRE MISSLES AT THE\n";
    print "OUTPOSTS OF THE COMPUTER.  IT WILL DO THE SAME TO YOU.\n";
    print "THE ONE WHO DESTROYS ALL FOUR OF THE ENEMY'S PLATOONS\n";
    print "FIRST IS THE WINNER.\n";
    print "GOOD LUCK... AND TELL US WHERE YOU WANT THE BODIES SENT!\n";
    print "TEAR OFF MATRIX AND USE IT TO CHECK OFF THE NUMBERS.\n";
    print "\n\n\n\n";
}
