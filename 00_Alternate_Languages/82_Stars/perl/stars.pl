#!/usr/bin/perl

use v5.11; # for say and use strict
use warnings;

my $MAX_NUMBER = 100;
my $MAX_GUESSES = 7;

print<<__END_OF_INTRO;
                                  Stars
               Creative Computing  Morristown, New Jersey



__END_OF_INTRO

print "Do you want instructions? ";
chomp( my $answer = <> );
if ( $answer !~ /^N/i ) {
  print<<__END_OF_INSTRUCTIONS;
I am thinking of a whole number from 1 to $MAX_NUMBER
Try to guess my number.  After you guess, I
will type one or more stars (*).  The more
stars I type, the closer you are to my number.
One star (*) means far away, seven stars (*******)
means really close!  You get $MAX_GUESSES guesses.
__END_OF_INSTRUCTIONS
}


while (1) {
  my $number_to_guess = int(rand($MAX_NUMBER) + 1);
  say "\n\nOK, I am thinking of a number, start guessing.";

  my $guess_number = 1;
  while ( $guess_number <= $MAX_GUESSES ) {
    print "\nYour Guess? ";
    chomp( my $guess = <> );
    last if $guess == $number_to_guess;
    $guess_number++;
    my $difference = abs $guess - $number_to_guess;
    print '*' if $difference < 2;
    print '*' if $difference < 4;
    print '*' if $difference < 8;
    print '*' if $difference < 16;
    print '*' if $difference < 32;
    print '*' if $difference < 64;
    print "*\n";
  }
  if ( $guess_number > $MAX_GUESSES ) { # didn't guess
    say "\nSorry, that's $MAX_GUESSES guesses, number was $number_to_guess";
  } else { # winner!
    say '*' x 50, '!!!';
    say "You got it in $guess_number guesses!!!  Let's play again...";
  }
}
