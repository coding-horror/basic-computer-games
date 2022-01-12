#!/usr/bin/env perl
use v5.24;
use warnings;
use experimental 'signatures';
no warnings 'experimental::signatures';

use constant TARGET => 158;

main(@ARGV);

sub main (@args) {
   welcome();
   help() if ask_yes_no('DO YOU WANT INSTRUCTIONS');
   do { one_match() } while ask_yes_no('ANYONE ELSE CARE TO TRY');
   __exit();
}

sub one_match {
   print_board();

   # the player can choose the starting position in the top row or the
   # right column
   my $move = ask_first_move() or return forfeit();

   # we alternate moves between computer or player from now on
   while ('playing') {
      $move = computer_move($move);
      say "COMPUTER MOVES TO SQUARE $move";
      return print_computer_victory() if $move == TARGET;

      $move = ask_player_move($move) or return forfeit();
      return print_player_victory() if $move == TARGET;
   }
}

sub is_valid_move ($move, $current, $skip_prevalidation = 0) {

   # pre-validation is needed for moves coming from the user
   if (! $skip_prevalidation) {
      state $valid_position = { map { $_ => 1 } board_identifiers() };
      return 0 unless $move =~ m{\A [1-9]\d+ \z}mxs;
      return 1 if $move == 0;
      return 0 unless $valid_position->{$move};
      return 0 if $move <= $current;
   }

   # the move might be valid in general, let's check from $current
   my $delta = $move - $current;

   # a valid move differs from the current position by a multiple of 10,
   # or 11, or 21. If dividing by all of them yields a remainder, then
   # the move is not valid
   return 0 if $delta % 10 && $delta % 11 && $delta % 21;

   # otherwise it is
   return 1;
}

sub ask_player_move ($current) {
   while ('necessary') {
      my $move = ask_input('WHAT IS YOUR MOVE');
      return $move if is_valid_move($move, $current);
      say "\nY O U   C H E A T . . .  TRY AGAIN";
   }
}

sub computer_move ($current) {

   # this game has some optimal/safe positions from where it's possible
   # to win with the right strategy. We will aim for them, if possible
   state $optimals = [ 158, 127, 126, 75, 73 ];
   for my $optimal ($optimals->@*) {

      # moves can only increase, if we did not find any optimal move so far
      # then there's no point going on
      last if $optimal <= $current;

      # computer moves are "syntactically" valid, skip pre-validation
      return $optimal if is_valid_move($optimal, $current, 'skip');

   }

   # cannot reach an optimal position... resort to randomness
   my $z = rand();
   return $current + 11 if $z > 0.6; # move down
   return $current + 21 if $z > 0.3; # move diagonally
   return $current + 10;           ; # move horizontally
}

sub board_identifiers {
   return (
      81,   71,  61,  51,  41,  31,  21,  11,
      92,   82,  72,  62,  52,  42,  32,  22,
      103,  93,  83,  73,  63,  53,  43,  33,
      114, 104,  94,  84,  74,  64,  54,  44,
      125, 115, 105,  95,  85,  75,  65,  55,
      136, 126, 116, 106,  96,  86,  76,  66,
      147, 137, 127, 117, 107,  97,  87,  77,
      158, 148, 138, 128, 118, 108,  98,  88,
   );
}

sub print_player_victory {
   print <<'END';

C O N G R A T U L A T I O N S . . .

YOU HAVE WON--VERY WELL PLAYED.
IT LOOKS LIKE I HAVE MET MY MATCH.
THANKS FOR PLAYING---I CAN'T WIN ALL THE TIME.

END
}

sub print_computer_victory {
   print <<'END';

NICE TRY, BUT IT LOOKS LIKE I HAVE WON.
THANKS FOR PLAYING.

END
}

sub forfeit { say "\nIT LOOKS LIKE I HAVE WON BY FORFEIT.\n" }

sub ask_input ($prompt) {
   print "$prompt? ";
   defined(my $input = <STDIN>) or __exit();

   # remove spaces from the input (including newlines), they are not used
   $input =~ s{\s+}{}gmxs;

   return $input;
}

sub ask_yes_no ($prompt) {
   while ('necessary') {
      my $input = ask_input($prompt);
      return 1 if $input =~ m{\A (?: yes | y) \z}imxs;
      return 0 if $input =~ m{\A (?:  no | n) \z}imxs;
      say q{PLEASE ANSWER 'YES' OR 'NO'.};
   }
}

sub ask_first_move {
   while ('necessary') {
      my $input = ask_input('WHERE WOULD YOU LIKE TO START');
      if ($input =~ m{\A (?: 0 | [1-9]\d+) \z}mxs) {
         return 0 unless $input;
         my $diagonal = int($input / 10);
         my $row = $input % 10;
         return $input if $row == 1 || $row == $diagonal;
      }
      say <<'END'
PLEASE READ THE DIRECTIONS AGAIN.
YOU HAVE BEGUN ILLEGALLY.

END
   }
}

sub __exit {
   say "\nOK --- THANKS AGAIN.";
   exit 0;
}

sub welcome {
   print <<'END'
                                 QUEEN
               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



END
}

sub help {
   print <<'END';
WE ARE GOING TO PLAY A GAME BASED ON ONE OF THE CHESS
MOVES.  OUR QUEEN WILL BE ABLE TO MOVE ONLY TO THE LEFT,
DOWN, OR DIAGONALLY DOWN AND TO THE LEFT.

THE OBJECT OF THE GAME IS TO PLACE THE QUEEN IN THE LOWER
LEFT HAND SQUARE BY ALTERNATING MOVES BETWEEN YOU AND THE
COMPUTER.  THE FIRST ONE TO PLACE THE QUEEN THERE WINS.

YOU GO FIRST AND PLACE THE QUEEN IN ANY ONE OF THE SQUARES
ON THE TOP ROW OR RIGHT HAND COLUMN.
THAT WILL BE YOUR FIRST MOVE.
WE ALTERNATE MOVES.
YOU MAY FORFEIT BY TYPING '0' AS YOUR MOVE.
BE SURE TO PRESS THE RETURN KEY AFTER EACH RESPONSE.


END
}

sub print_board {
   say '';
   my @ids = board_identifiers();
   my $row_template = join '  ', ($ENV{ORIGINAL} ? '%d' : '%3d') x 8;
   for my $A (0 .. 7) {
      my $start = $A * 8;
      my @range = $start .. $start + 7;
      say ' ', sprintf $row_template, @ids[@range];
      say "\n";
   }
   say '';
}
