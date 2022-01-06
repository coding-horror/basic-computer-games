#!/usr/bin/env perl
use v5.24;
use warnings;
use experimental 'signatures';
no warnings 'experimental::signatures';
use List::Util 'none';

# our board will be represented with an array of 14 slots, from 0 to 13.
# Positions 6 and 13 represent the "home pit" for the human and the
# computer, respectively.
use constant PLAYER_HOME => 6;
use constant COMPUTER_HOME => 13;

use constant FIRST => 0;
use constant AGAIN => 1;

exit main(@ARGV);

sub main {
   $|++; # disable buffering on standard output, every print will be
         # done immediately

   welcome(); # startup message

   # this array will keep track of computer-side failures, defined as
   # "the computer did not win". Whenever the computer loses or draws, the
   # specific sequence of moves will be saved and then used to drive
   # the search for a (hopefully) optimal move.
   my $failures = [];
   while ('enjoying') {

      # a new game starts, let's reset the board to the initial condition
      my $board = [ (3) x 6, 0, (3) x 6, 0 ];

      # this string will keep track of all moves performed
      my $moves = '/';

      # the human player starts
      my $turn = 'player';

      say "\n";
      print_board($board);

      while (not is_game_over($board)) {

         my $move; # this will collect the move in this turn

         if ($turn eq 'player') { # "first" move for player

            # player_move(...) does the move selected by the player,
            # returning both the selected move as well as the pit id
            # where the last seed landed
            ($move, my $landing) = player_move($board);

            # if we landed on the Player's Home Pit we get another move
            $turn = $landing == PLAYER_HOME ? 'player-again' : 'computer';
         }
         elsif ($turn eq 'player-again') { # "second" move for player

            # here we call player_move making it clear that it's the
            # second move, to get the right prompt eventually. We only
            # care for the $move as the result, so we ignore the other.
            ($move) = player_move($board, AGAIN);
            $turn = 'computer';
         }
         else {

            # the computer_move(...) function analyzes the $board as well
            # as adapting the strategy based on past "failures" (i.e.
            # matches where the computer did not win). For this it's
            # important to pass the log of these failures, as well as the
            # full record of moves in this specific match.
            ($move, my $landing) = computer_move($board, $failures, $moves);
            print "\nMY MOVE IS ", $move - 6;

            # do the second move in the turn if conditions apply
            if ($landing == COMPUTER_HOME && ! is_game_over($board)) {

               # save the first move before doing the second one!
               $moves .= "$move/";

               my ($move) = computer_move($board, $failures, $moves);
               print ',', $move - 6;
            }
            $turn = 'player';
         }

         # append the last selected move by either party, to track this
         # specific match (useful for computer's AI and ML)
         $moves .= "$move/";
         print_board($board);
      }

      # assess_victory() returns the difference between player's and
      # computer's seeds, so a negative value is a win for the computer.
      my $computer_won = assess_victory($board) < 0;

      # if this last match was a "failure" (read: not a win for the
      # computer), then record it for future memory.
      push $failures->@*, $moves unless $computer_won;
   }

   return 0;
}

# calculate the difference between the two home pits. Negative values mean
# that the computer won, 0 is a draw, positive values is a player's win.
# The difference is also returned back, in case of need.
sub assess_victory ($board) {
   say "\nGAME OVER";
   my $difference = $board->[PLAYER_HOME] - $board->[COMPUTER_HOME];
   if ($difference < 0) {
      say 'I WIN BY ', -$difference, ' POINTS';
   }
   else {
      say $difference ? "YOU WIN BY $difference POINTS" : 'DRAWN GAME';
   }
   return $difference;
}

# move the seeds from $pit and take into account possible bonuses
sub move_seeds ($board, $pit) {

   # get the seeds from the selected pit $pit
   my $seeds = $board->[$pit];
   $board->[$pit] = 0;

   # $landing will be our "moving cursor" to place seeds around
   my $landing = $pit;
   while ($seeds > 0) {
      $landing = ($landing + 1) % 14; # 12 --> 13 -[wrap]-> 0 --> 1
      --$seeds;
      ++$board->[$landing];
   }

   # check for "stealing seeds" condition. This cannot happen in home pits
   if ($landing != PLAYER_HOME && $landing != COMPUTER_HOME
       && $board->[$landing] == 1 && $board->[12 - $landing] > 0) {
      my $home = $pit < 7 ? PLAYER_HOME : COMPUTER_HOME;
      $board->[$home] += 1 + $board->[12 - $landing];
      $board->@[$landing, 12 - $landing] = (0, 0);
   }

   return ($pit, $landing);
}

sub get_player_move ($board, $prompt) {
   print "\n$prompt? ";
   while (defined(my $move = <STDIN>)) {
      chomp($move); # remove newline
      return $move - 1 if $move =~ m{\A[1-6]\z}mxs && $board->[$move - 1];
      print 'ILLEGAL MOVE\nAGAIN? ';
   }
   die "goodbye\n";
}

sub player_move ($board, $stage = FIRST) {
   my $prompt = $stage == FIRST ? 'YOUR MOVE' : 'AGAIN';
   my $selected_move = get_player_move($board, $prompt);
   return move_seeds($board, $selected_move);
}

sub computer_move ($board, $failures, $moves) {

   # we will go through all possible moves for the computer and all
   # possible responses by the player, collecting the "best" move in terms
   # of reasonable outcome (assuming that each side wants to maximize their
   # outcome. $best_move will eventually contain the best move for the
   # computer, and $best_difference the best difference in scoring (as
   # seen from the computer).
   my ($best_move, $best_difference);
   for my $c_move (7 .. 12) {
      next unless $board->[$c_move]; # only consider pits with seeds inside

      # we work on a copy of the board to do all our trial-and-errors
      my $copy = [ $board->@* ];
      move_seeds($copy, $c_move);

      # it's time to "think like a player" and see what's the "best" move
      # for the player in this situation. This heuristic is "not perfect"
      # but it seems OK anyway.
      my $best_player_score = 0;
      for my $p_move (0 .. 5) {
         next unless $copy->[$p_move]; # only pits with seeds inside
         my $landing = $copy->[$p_move] + $p_move;

         # the player's score for this move, calculated as additional seeds
         # placed in the player's pit. The original algorithm sets this to
         # 1 only if the $landing position is greater than 13, which can
         # be obtained by setting the ORIGINAL environment variable to a
         # "true" value (in Perl terms). Otherwise it is calculated
         # according to the real rules for the game.
         my $p_score = $ENV{ORIGINAL} ? $landing > 13 : int(($landing - 5) / 14);

         # whatever, the landing position must be within the bounds
         $landing %= 14;

         # if the conditions apply, the player's move might win additional
         # seeds, which we have to to take into account.
         $p_score += $copy->[12 - $landing]
            if $copy->[$landing] == 0
            && $landing != PLAYER_HOME && $landing != COMPUTER_HOME;

         # let's compare this move's score against the best collected
         # so far (as a response to a specific computer's move).
         $best_player_score = $p_score if $p_score > $best_player_score;
      }

      # the overall score for the player is the additional seeds we just
      # calculated into $best_player_score plus the seeds that were already
      # in the player's pit
      $best_player_score += $copy->[PLAYER_HOME];

      # the best difference we can aim for with this computer's move must
      # assume that the player will try its best
      my $difference = $copy->[COMPUTER_HOME] - $best_player_score;

      # now it's time to check this computer's move against the history
      # of failed matches. $candidate_moves will be the "candidate" list
      # of moves if we accept this one.
      my $candidate_moves = $moves . $c_move . '/';
      for my $failure ($failures->@*) {

         # index(.) returns 0 if and only if $candidate_moves appears at
         # the very beginning of $failure, i.e. it matches a previous
         # behaviour.
         next if index($failure, $candidate_moves) != 0;

         # same sequence of moves as before... assign a penalty
         $difference -= 2;
      }

      # update $best_move and $best_difference if they need to
      ($best_move, $best_difference) = ($c_move, $difference)
         if (! defined $best_move) || ($best_difference < $difference);
   }

   # apply the selected move and return
   return move_seeds($board, $best_move);
}

sub welcome {
   say ' ' x 34, 'AWARI';
   say ' ' x 15, 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY';
}

sub print_board ($board) {
   my $template = '
    %2d  %2d  %2d  %2d  %2d  %2d
 %2d                         %d
    %2d  %2d  %2d  %2d  %2d  %2d
';
   printf $template, $board->@[12, 11, 10, 9, 8 , 7, 13, 6, 0 .. 5];
   return;
}

sub is_game_over ($board) {

   # game over if the player's side is empty
   return 1 if none { $_ } $board->@[0 ..  5];

   # game over if the computers' side is empty
   return 1 if none { $_ } $board->@[7 .. 12];

   # not game over
   return 0;
}
