#!/usr/bin/perl


my @cookie;

&main;

sub main {
	my $answer = 1;
	until ($answer == 0) {
		$answer=&game_play;
	}
}

sub game_play {
	
	# the initial game set up
	&print_intro;
	my ($players,$rows,$cols) = &get_user_info;
	&create_gameboard($rows,$cols);
	&print_gameboard($rows,$cols);
	my $game_over = 0;
	my $player = 0;

	# continuous loop until the poison pill is swallowed
	until ($game_over == 1) {
		if ($player > ($players-1)) {		#checks to make sure we're just looping thru valid players 
			$player = 0;
		}
		$player++;
		my ($user_row,$user_col) = &get_player_row_col($player,$rows,$cols);
		if ($cookie[$user_row][$user_col] == -1) {
			print "YOU LOSE, PLAYER $player\n\n";
			print "AGAIN (1=YES, 0=NO!)\n";
			my $answer=<STDIN>;
			chomp($answer);
			return($answer);
		}
		&modify_gameboard($rows,$cols,$user_row,$user_col);
		&print_gameboard($rows,$cols);
	}
	
}

sub get_player_row_col {
	my ($player,$row,$col) = @_;
	my @coords;
	my $validity="invalid";
	# Getting coordinates from user
	until ($validity eq "valid") {
		print "PLAYER $player COORDINATES OF CHOMP (ROW,COLUMN)\n";
		my $input=<STDIN>;
		chomp($input);
		@coords = split/,/,$input;

		#verifying coordinates are valid
		if ($coords[0] < 1 || $coords[0] > $row || $coords[1] < 1 || $coords[1] > $col || $cookie[$coords[0]][$coords[1]] == 0) {
			print "NO FAIR. YOU'RE TRYING TO CHOMP ON EMPTY SPACE!\n";
		}
		else {
			$validity="valid";
		}
	}
	return($coords[0],$coords[1]);
}

sub get_user_info {
	my ($players,$rows,$cols)=0;
	until ($players > 0) {
		print "HOW MANY PLAYERS\n";
		$players=<STDIN>;
		chomp($players);
	}
	until ($rows > 0 && $rows < 10) {
		print "HOW MANY ROWS\n";
		$rows=<STDIN>;
		chomp($rows);
		if ($rows > 9) {
			print "TOO MANY ROWS (9 IS MAXIMUM). NOW, ";
		}
	}
	until ($cols > 0 && $cols < 10) {
		print "HOW MANY COLUMNS\n";
		$cols=<STDIN>;
		chomp($cols);
		if ($cols > 9) {
			print "TOO MANY COLUMNS (9 IS MAXIMUM). NOW, ";
		}
	}
	return($players,$rows,$cols);
}

sub print_intro{
	print ' ' x 33 . "CHOMP\n";
	print ' ' x 15 . "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n";
	print "THIS IS THE GAME OF CHOMP (SCIENTIFIC AMERICAN, JAN 1973)\n";
	print "DO YOU WANT THE RULES (1=YES, 0=NO!)";
	my $answer = <STDIN>;
	chomp($answer);
	if ($answer == 0) {
		return;
	}
	else {
	 	print "CHOMP IS FOR 1 OR MORE PLAYERS (HUMANS ONLY).\n\n";
	 	print "HERE'S HOW A BOARD LOOKS (THIS ONE IS 5 BY 7):\n";
		&create_gameboard(5,7);
		&print_gameboard(5,7);
	 	print "THE BOARD IS A BIG COOKIE - R ROWS HIGH AND C COLUMNS\n";
	 	print "WIDE. YOU INPUT R AND C AT THE START. IN THE UPPER LEFT\n";
	 	print "CORNER OF THE COOKIE IS A POISON SQUARE (P). THE ONE WHO\n";
	 	print "CHOMPS THE POISON SQUARE LOSES. TO TAKE A CHOMP, TYPE THE\n";
	 	print "ROW AND COLUMN OF ONE OF THE SQUARES ON THE COOKIE.\n";
	 	print "ALL OF THE SQUARES BELOW AND TO THE RIGHT OF THAT SQUARE\n";
	 	print "(INCLUDING THAT SQUARE, TOO) DISAPPEAR -- CHOMP!!\n";
	 	print "NO FAIR CHOMPING SQUARES THAT HAVE ALREADY BEEN CHOMPED,\n";
	 	print "OR THAT ARE OUTSIDE THE ORIGINAL DIMENSIONS OF THE COOKIE.\n\n";
	 	print "HERE WE GO...\n";
		undef @cookie;
	}
}

#initial creation of the gameboard
sub create_gameboard {
	my $rows = shift;
	my $cols = shift;
	foreach my $row (1..($rows)) {
		foreach my $col (1..($cols)) {
			$cookie[$row][$col]=1;
		}
	}
	$cookie[1][1]=-1;
}

#modification of the gameboard based on the input from the player
sub modify_gameboard {
	my ($rows,$cols,$user_row,$user_col) = @_;
	foreach my $row ($user_row..($rows)) {
		foreach my $col ($user_col..($cols)) {
			$cookie[$row][$col]="  ";	
		}
	}
}

#prints the gameboard based on the current state of the gameboard
sub print_gameboard {
	my ($rows,$cols) = @_;
	foreach my $col (1..$cols) {
		if ($col == $cols) {
			print "$col\n";
		}
		elsif ($col == 1) {
			print "\t  $col ";
		}
		else {
			print "$col ";
		}
	}
	foreach my $row (1..($rows)) {
		print "\t$row ";
		foreach my $col (1..($cols)) {
			if ($cookie[$row][$col] == 1) {
				print "* ";
			}
			if ($cookie[$row][$col] == 0) {
				print "  ";
			}
			if ($cookie[$row][$col] == -1) {
				print "P ";
			}
		}
		print "\n";
	}
}
