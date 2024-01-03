#!/usr/bin/env perl
package BasicComputerGames::Hammurabi;

use v5.24;
use warnings;
use experimental 'signatures';

{
   # Quick and dirty accessors
   no strict 'refs';
   for my $feature (
      qw< year population store rats_toll had_plague planted
      production_per_acre acres new_arrivals starved status max_year fed
      percent_starved total_starved cost_per_acre >
     )
   {
      *{__PACKAGE__ . '::' . $feature} = sub ($self, @new) {
         $self->{$feature} = $new[0] if @new;
         return $self->{$feature};
      };
   } ## end for my $feature (...)
}

sub new ($package, %args) {
   my $self = bless {

      # These defaults can be overridden by %args
      population          => 100,
      store               => 2800,
      rats_toll           => 200,
      production_per_acre => 3,
      acres               => 1000,
      new_arrivals        => 5,
      fed                 => 0,
      max_year            => 10,

      %args,

      # These starting values cannot be overridden by %args
      status          => 'start',
      year            => 1,
      starved         => 0,
      total_starved   => 0,
      percent_starved => 0,
      had_plague      => 0,
      planted         => 0,
      cost_per_acre   => 0,
   }, $package;

   return $self;
} ## end sub new

sub step ($self, $input) {
   my $method = $self->can('_handle_' . $self->status);
   $self->$method($input);
}

########################################################################
#
# All _handle_* methods below represents handlers for different states
# of the game, e.g. state `start` is managed by _handle_start(). Each
# handler receives two input arguments: an instance to the game object and
# the input that was collected by the UI for that particular state (if
# any).

# start of the game
sub _handle_start ($self, $input) {
   $self->status('start_of_year');
}

# start of each year
sub _handle_start_of_year ($self, $input) {
   $self->cost_per_acre(int(rand(10)) + 17);
   $self->status('advertise_cost_per_acre');
}

# intermediate state to allow for printing the cost per acre, moves
# directly to following state
sub _handle_advertise_cost_per_acre ($self, $input) {
   $self->status('buy_acres');
}

# buy acres of land, making sure to be able to cover for the cost
sub _handle_buy_acres ($self, $input) {
   return $self->status('bail_out')   if $input < 0;
   return $self->status('sell_acres') if $input == 0;
   my $cpa  = $self->cost_per_acre;
   my $cost = $cpa * $input;
   return $self->status('buy_acres_again')
     if $cost > $self->store;
   $self->acres($self->acres + $input);
   $self->store($self->store - $cost);
   return $self->status('feeding');
} ## end sub _handle_buy_acres

# intermediate state to allow for notifying that the request for new
# acres of land could not be covered, moves directly to the following
# state
sub _handle_buy_acres_again ($self, $input) {
   $self->status('buy_acres');
}

# sell acres of land, making sure to sell only what can be sold.
sub _handle_sell_acres ($self, $input) {
   return $self->status('bail_out')         if $input < 0;
   return $self->status('sell_acres_again') if $input >= $self->acres;
   $self->acres($self->acres - $input);
   $self->store($self->store + $self->cost_per_acre * $input);
   return $self->status('feeding');
} ## end sub _handle_sell_acres

# intermediate state to allow for notifying that the request to sell
# acres of land could not be covered, moves directly to the following
# state
sub _handle_sell_acres_again ($self, $input) {
   $self->status('sell_acres');
}

# feed people, making sure we have the necessary resources
sub _handle_feeding ($self, $input) {
   return $self->status('bail_out')      if $input < 0;
   return $self->status('feeding_again') if $input >= $self->store;
   $self->store($self->store - $input);
   $self->fed($input);
   $self->status('planting');
} ## end sub _handle_feeding

# intermediate state to allow for notifying that the request to use
# bushels of grain could not be covered, moves directly to the following
# state
sub _handle_feeding_again ($self, $input) {
   $self->status('feeding');
}

# plant crops, making sure we have the land, the seeds and the workers.
sub _handle_planting ($self, $input) {
   return $self->status('bail_out') if $input < 0;

   return $self->status('planting_fail_acres') if $input > $self->acres;

   my $store = $self->store;
   return $self->status('planting_fail_seeds')
     if $store < int($input / 2);

   return $self->status('planting_fail_people')
     if $input >= $self->population * 10;

   $self->planted($input);
   $self->store($store - int($input / 2));
   $self->status('simulate_year');
} ## end sub _handle_planting

# complain about lack of land to cover the planting request
sub _handle_planting_fail_acres ($self, $input) {
   $self->status('planting');
}

# complain about lack of seeds to cover the planting request
sub _handle_planting_fail_seeds ($self, $input) {
   $self->status('planting');
}

# complain about lack of workers to cover the planting request
sub _handle_planting_fail_people ($self, $input) {
   $self->status('planting');
}

# simulate the rest of the year after all inputs, i.e. rats, crops, etc.
sub _handle_simulate_year ($self, $input) {
   my $store = $self->store;

   # rats might take a toll during the year
   my $c         = 1 + int(rand(5));
   my $rats_toll = $c % 2 ? 0 : int($store / $c);
   $self->rats_toll($rats_toll);

   # planting also gains us grain after the harvest
   my $ppa     = $self->production_per_acre(1 + int(rand(5)));
   my $harvest = $ppa * $self->planted;

   # let's update the stored seeds finally
   $self->store($store += $harvest - $rats_toll);

   # let's see how population evolved
   my $population = $self->population;

   # how many people had full tummies
   my $fed_people = int($self->fed / 20);
   my $starved    = $population - $fed_people;
   $starved = 0 if $starved < 0;    # cannot create people from seeds
   $self->starved($starved);

   # check preliminary exit condition for a very bad year
   return $self->status('impeach_year')
     if $starved > $population * 0.45;

   # update statistics
   $self->total_starved($self->total_starved + $starved);
   my $perc = $self->percent_starved;
   my $year = $self->year;
   $perc = (($year - 1) * $perc + $starved * 100 / $population) / $year;
   $self->percent_starved($perc);

   # babies
   my $acres    = $self->acres;
   my $rand     = 1 + int(rand(5));
   my $arrivals = $self->new_arrivals(
      int(1 + $rand * (20 * $acres + $store) / $population / 100));

   $population += $arrivals - $starved;

   # HORROS, A 15% CHANCE OF PLAGUE
   my $had_plague = $self->had_plague(rand(1) < 0.15);
   $population = int($population / 2) if $had_plague;

   # save population for next round
   $self->population($population);

   # advance to next year
   $self->year(++$year);
   if ($year > $self->max_year) {
      $self->status('summary');
   }
   else {
      $self->status('start_of_year');
   }
} ## end sub _handle_simulate_year

# this is a transition after the impeachment message
sub _handle_impeach_year ($self, $input) {
   $self->status('goodbye');
}

# this is a transition after printing the summary
sub _handle_summary ($self, $input) {
   $self->status('goodbye');
}

# this is a transition after printing the final salutation message
sub _handle_goodbye ($self, $input) {
   $self->status('game_over');
}

# this is a transition after asking the king to hire someone else!
sub _handle_bail_out ($self, $input) {
   $self->status('game_over');
}

# The following package implements all the User Interface, using the
# game state (as exposed by $game->status) to figure out what to print
# and if an input is needed from the user. It all happens on the
# standard input and output.
package BasicComputerGames::Hammurabi::DefaultIO;

# All __io_* functions take a $game object as input, in case of need for
# some specific data (e.g. population amount or amassed grain bushels).
# They usually print something out and collect input from standard
# input for states that require a user input. All functions are named
# after the available states in BasicComputerGames::Hammurabi.

sub __io_start ($game) {
   say ' ' x 32, 'HAMURABI';
   say ' ' x 15, 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY';
   print "\n\n\n";
   say 'TRY YOUR HAND AT GOVERNING ANCIENT SUMERIA';
   say 'FOR A TEN-YEAR TERM OF OFFICE';
   print "\n";
   return;
} ## end sub __io_start

sub __io_start_of_year ($game) {
   print "\n\n";
   say "HAMURABI:  I BEG TO REPORT TO YOU,";
   printf "IN YEAR %d , %d PEOPLE STARVED, %d CAME TO THE CITY,\n",
     $game->year, $game->starved, $game->new_arrivals;
   say 'A HORRIBLE PLAGUE STRUCK!  HALF THE PEOPLE DIED.'
     if $game->had_plague;
   say 'POPULATION IS NOW ', $game->population;
   say 'THE CITY NOW OWNS ', $game->acres,           ' ACRES.';
   say 'YOU HARVESTED ', $game->production_per_acre, ' BUSHELS PER ACRE.';
   say 'THE RATS ATE ',  $game->rats_toll,           ' BUSHELS.';
   say 'YOU NOW HAVE ',  $game->store,               ' BUSHELS IN STORE.';
   print "\n";
   return;
} ## end sub __io_start_of_year

sub get_input ($game = undef) {
   while (<STDIN>) {
      chomp(my $input = $_);
      return 0 unless $input;
      return $input if $input =~ m{\A -? \d+ \z}mxs;
      print "REENTER?\n?? ";
   } ## end while (<STDIN>)
   die "\n";
} ## end sub get_input

sub __io_bail_out ($game) {
   say "\nHAMURABI:  I CANNOT DO WHAT YOU WISH.";
   say 'GET YOURSELF ANOTHER STEWARD!!!!!';
   return;
}

sub __not_enough_bushels ($game) {
   say 'HAMURABI:  THINK AGAIN.  YOU HAVE ONLY';
   say $game->store, ' BUSHELS OF GRAIN. NOW, THEN,';
}

sub __not_enough_acres ($game) {
   say 'HAMURABI:  THINK AGAIN.  YOU OWN ONLY ',
     $game->acres, ' ACRES.  NOW, THEN,';
}

sub __io_buy_acres ($game) {
   print 'HOW MANY ACRES DO YOU WISH TO BUY?? ';
   return get_input();
}

sub __io_advertise_cost_per_acre ($game) {
   say 'LAND IS TRADING AT ', $game->cost_per_acre, ' BUSHELS PER ACRE.';
   return;
}

sub __io_sell_acres ($game) {
   print 'HOW MANY ACRES DO YOU WISH TO SELL?? ';
   return get_input();
}

sub __io_feeding ($game) {
   print "\nHOW MANY BUSHELS DO YOU WISH TO FEED YOUR PEOPLE?? ";
   return get_input();
}

sub __io_planting ($game) {
   print "\nHOW MANY ACRES DO YOU WISH TO PLANT WITH SEED?? ";
   return get_input();
}

sub __io_buy_acres_again ($game) { __not_enough_bushels($game) }

sub __io_sell_acres_again ($game) { __not_enough_acres($game) }

sub __io_feeding_again ($game) { __not_enough_bushels($game) }

sub __io_planting_fail_acres ($game) { __not_enough_acres($game) }

sub __io_planting_fail_seeds ($game) { __not_enough_bushels($game) }

sub __io_planting_fail_people ($game) {
   say 'BUT YOU HAVE ONLY ', $game->population,
     ' PEOPLE TO TEND THE FIELDS!  NOW, THEN,';
}

sub __impeachment {
   say 'DUE TO THIS EXTREME MISMANAGEMENT YOU HAVE NOT ONLY';
   say 'BEEN IMPEACHED AND THROWN OUT OF OFFICE BUT YOU HAVE';
   say 'ALSO BEEN DECLARED NATIONAL FINK!!!!';
}

sub __io_impeach_year ($game) {
   printf "\nYOU STARVED %d PEOPLE IN ONE YEAR!!!\n", $game->starved;
   return __impeachment();
}

sub __io_goodbye ($game) {
   say "\nSO LONG FOR NOW.\n";
   return;
}

# Final summary for the game, print statistics and evaluation
sub __io_summary ($game) {
   my $starved = $game->total_starved;
   my $years   = $game->max_years;
   my $p1      = 100 * $starved / $years;
   my $l       = $game->acres / $game->population;
   printf "IN YOUR %d-YEAR TERM OF OFFICE, %d PERCENT OF THE\n",
     $years, $p1;
   say 'POPULATION STARVED PER YEAR ON THE AVERAGE, I.E. A TOTAL OF';
   printf "%d PEOPLE DIED!!\n", $starved;
   say 'YOU STARTED WITH 10 ACRES PER PERSON AND ENDED WITH';
   printf "%.2f ACRES PER PERSON.\n\n", $l;

   if ($p1 > 33 || $l < 7) {
      __impeachment();
   }
   elsif ($p1 > 10 || $l < 9) {
      say 'YOUR HEAVY-HANDED PERFORMANCE SMACKS OF NERO AND IVAN IV.';
      say 'THE PEOPLE (REMAINING) FIND YOU AN UNPLEASANT RULER, AND,';
      say 'FRANKLY, HATE YOUR GUTS!!';
   }
   elsif ($p1 > 3 || $l < 10) {
      my $haters = int($game->population * rand(0.8));
      say 'YOUR PERFORMANCE COULD HAVE BEEN SOMEWHAT BETTER, BUT';
      say "REALLY WASN'T TOO BAD AT ALL.  $haters PEOPLE";
      say 'WOULD DEARLY LIKE TO SEE YOU ASSASSINATED BUT WE ALL HAVE OUR';
      say 'TRIVIAL PROBLEMS.';
   } ## end elsif ($p1 > 3 || $l < 10)
   else {
      say 'A FANTASTIC PERFORMANCE!!!  CHARLEMANGE, DISRAELI, AND';
      say 'JEFFERSON COMBINED COULD NOT HAVE DONE BETTER!';
   }

   return;
} ## end sub __io_summary

# this class method allows using this module... easily. Call with
# arguments to be fed to the BasicComputerGames::Hammurabi constructor.
sub run ($package, @args) {
   my $game = BasicComputerGames::Hammurabi->new(@args);
   while ((my $status = $game->status) ne 'game_over') {
      eval {
         my $retval;
         if (my $cb = $package->can('__io_' . $status)) {
            $retval = $cb->($game);
         }
         $game->step($retval);
         1;
      } or last;
   } ## end while ((my $status = $game...))
   say '';
   return 0;
} ## end sub run

# Modulino (https://gitlab.com/polettix/notechs/-/snippets/1868370)
exit __PACKAGE__->run(@ARGV) unless caller;

1;
__END__

=pod

=encoding UTF-8

=head1 NAME

BasicComputerGames::Hammurabi - the Hammurabi game from BASIC

=head1 SYNOPSIS

   use BasicComputerGames::Hammurabi;

   # if you have a way to manage the UI yourself, then you can get the
   # game logic handler
   my $game_handler = BasicComputerGames::Hammurabi->new;
   while ((my $status = $game_handler->status) ne 'game_over') {
      # figure out what to print out with $status, this is totally
      # up to the interface implementation, which also has to collect
      # the inputs
      my $retval = manage_ui_for($game_handler);

      # now we feed whatever came from the interface back to the handler
      $game_handler->step($retval);
   }

   # Want the plain terminal experience? No problem:
   BasicComputerGames::Hammurabi::DefaultIO->run;

=head1 IMPLEMENTATION DETAILS

The code tries to behave like the original BASIC, including some dubious
conditions checks that e.g. do not allow using the full potential of
available resources for lack of an equal sign.

The calculation of the final average of starved people per year is
differnet from the original and avoids what is considered (by me) a bug
that kicks in when there are years in which nobody starves.

=head1 AUTHOR

Adapted by Flavio Poletti from the BASIC version by David Ahl. Game text
copied verbatim from the original BASIC implementation, including typos.

=cut
