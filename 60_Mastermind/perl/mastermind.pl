#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use List::Util qw{ min sum };   # Convenient list utilities
use Term::ReadLine;     # Prompt and return user input

our $VERSION = '0.000_01';

use constant MAX_GUESSES    => 10;

print <<'EOD';
                               MASTERMIND
               Creative Computing  Morristown, New Jersey


EOD

=begin comment

     MASTERMIND II
     STEVE NORTH
     CREATIVE COMPUTING
     PO BOX 789-M MORRISTOWN NEW JERSEY 07960

=end comment

=cut

# NOTE that mixed-case 'my' variables are 'global' in the sense that
# they are used in subroutines, but not passed to them.

say '';

my $number_of_colors = get_input(
    'Number of colors [1-8]: ',
    sub { m/ \A [1-8] \z /smx },
    "No more than 8, please!\n",
);

say '';

my $Number_of_Positions = get_input(
    'Number of positions: ',
    sub { m/ \A [0-9]+ \z /smx && $ARG },
    "A positive number, please\n",
);

say '';

my $number_of_rounds = get_input(
    'Number of rounds: ',
    sub { m/ \A [0-9]+ \z /smx && $ARG },
    "A positive number, please\n",
);

my $P = $number_of_colors ** $Number_of_Positions;
say 'Total possibilities = ', $P;

my @colors = ( qw{
    Black White Red Green Orange Yellow Purple Tan
})[ 0 .. $number_of_colors - 1 ];
my @Color_Codes = map { uc substr $ARG, 0, 1 } @colors;

print <<'EOD';


Color        Letter
=====        ======
EOD

foreach my $inx ( 0 .. $#colors ) {
    printf "%-13s%s\n", $colors[$inx], $Color_Codes[$inx];
}

say '';

my $computer_score = 0;  # Computer score
my $human_score = 0;  # Human score

foreach my $round_number ( 1 .. $number_of_rounds ) {

    print <<"EOD";

Round number $round_number ----

Guess my combination.

EOD

    $human_score += human_guesses( $Number_of_Positions );

    print_score( $computer_score, $human_score );

    $computer_score += computer_guesses();

    print_score( $computer_score, $human_score );

}

# Make a $pattern into a hash with one key for each possible color. The
# value for each color is the number of times it appears in the pattern.
sub hashify_pattern {
    my $pattern = uc $ARG[0];
    my %p = map { $ARG => 0 } @Color_Codes;
    $p{$ARG}++ for split qr//, $pattern;
    return \%p;
}

# Given a $pattern, a $guess at that pattern, and $black and $white
# scores, return a true value if the $black and $white scores of the
# $guess are those supplied as arguments; otherwise return a false
# value. This is used by computer_guesses() to eliminate possibilities.
sub analyze_black_white {
    my ( $pattern, $guess, $black, $white ) = @ARG;
    my $info = analyze_guess( $pattern, $guess );
    return $info->{black} == $black && $info->{white} == $white;
}

# Given a $pattern and a $guess at that pattern, return a reference to a
# hash with the following keys:
#  {guess} is the guess;
#  {black} is the black score of the guess
#  {white} is the white score of the guess
sub analyze_guess {
    my ( $pattern, $guess ) = @ARG;
    my $pattern_hash = hashify_pattern( $pattern );
    my $guess_hash = hashify_pattern( $guess );
    my $white = sum(
        map { min( $pattern_hash->{$ARG}, $guess_hash->{$ARG} ) } @Color_Codes,
    );
    my $black = 0;
    foreach my $inx ( 0 .. length( $pattern ) - 1 ) {
        if ( substr( $pattern, $inx, 1 ) eq substr( $guess, $inx, 1 ) )
        {
            $black++;
            --$white;
        }
    }
    return +{
        guess   => $guess,
        black   => $black,
        white   => $white,
    }
}

# Used by the computer to guess the human's choice. The return is the
# number of guesses the computer took. The return is the maximum plus
# one if the computer failed to guess.
sub computer_guesses {

    print <<'EOD';

Now I guess. Think of a combination.
EOD
    get_input(
        'Hit <return> when ready:',
    );

    # Generate all possible permutations.
    my @possible;
    foreach my $permutation ( 0 .. @Color_Codes ** $Number_of_Positions - 1 ) {
        my $guess;
        for ( 1 .. $Number_of_Positions ) {
            my $inx = $permutation % @Color_Codes;
            $guess .= $Color_Codes[ $inx ];
            $permutation = int( $permutation / @Color_Codes );
        }
        push @possible, $guess;
    }

    # Guess ...
    foreach my $guess_num ( 1 .. MAX_GUESSES ) {

        # Guess a possible permutation at random, removing it from the
        # list.
        my $guess = splice @possible, int rand @possible, 1;
        say 'My guess is: ', $guess;

        # Find out its black/white score.
        my ( $black, $white ) = split qr< , >smx, get_input(
            'Blacks, Whites: ',
            sub { m/ \A [0-9]+ , [0-9]+ \z /smx },
            "Please enter two unsigned integers\n",
        );

        # If it's all black, the computer wins.
        if ( $black == $Number_of_Positions ) {
            say "I got it in $guess_num moves!";
            return $guess_num;
        }

        # Eliminate all possible permutations that give the black/white
        # score that our guess got. If there are any left, take another
        # guess.
        next if @possible = grep { analyze_black_white( $ARG, $guess, $black,
            $white ) } @possible;

        # There were no permutations left. Complain.
        print <<'EOD';
You have given me inconsistent information.
Try again, and this time please be more careful.
EOD

        goto &computer_guesses; # Tail-call ourselves to try again.
    }

    print <<'EOD';
I used up all my moves!
I guess my CPU is just having an off day.
EOD

    return MAX_GUESSES + 1;
}

# Used to generate a pattern and process the human's guesses. The return
# is the number of guesses the human took. The return is the maximum
# plus one if the human failed to guess.
sub human_guesses {

    my @saved_moves;  # Saved moves
    my $pattern = uc join '',
        map { $Color_Codes[ rand @Color_Codes ] } 1 .. $Number_of_Positions;

    foreach my $guess_num ( 1 .. MAX_GUESSES ) {

        my $guess = uc get_input(
            "Move # $guess_num guess: ",
            sub {

                # If the user entered 'quit', bail out.
                if ( m/ \A quit \z /smxi ) {
                    die "Quitter!  My combination was $pattern\n\nGood bye\n";
                }

                # If the user entered 'board', display the board so far.
                # We return success to prevent the warning message, but
                # we also clear $ARG. The caller's caller sees this and
                # re-queries.
                if ( m/ \A board \z /smxi ) {
                    print <<'EOD';

Board
Move     Guess          Black     White
EOD
                    my $number = 1;
                    foreach my $item ( @saved_moves ) {
                        printf "%4d     %-13s  %3d       %3d\n", $number++,
                        @{ $item }{ qw{ guess black white } };
                    }
                    return undef;   # Validation failure, but suppress warning.
                }

                # End of special-case code. Below here we are dealing
                # with guess input.

                # The length of the input must equal the number of
                # positions.
                if ( $Number_of_Positions != length ) {
                    warn "Bad number of positions\n";
                    return 0;
                }

                # The input may contain only valid color codes.
                state $invalid_color = do { # Evaluated only once
                    local $LIST_SEPARATOR = '';
                    qr< [^@Color_Codes] >smxi;
                };
                if ( m/ ( $invalid_color ) /smxi ) {
                    warn "'$1' is unrecognized.\n";
                    return 0;
                }

                # We're good.
                return 1;
            },
             "Please enter 'board', 'quit', or any $Number_of_Positions of @{[
                 join ', ', map { qq<'$ARG'> } @Color_Codes ]}.\n",
        );

        my $rslt = analyze_guess( $pattern, $guess );

        push @saved_moves, $rslt;

        if ( $rslt->{black} == $Number_of_Positions ) {
            say "You guessed it in $guess_num moves.";
            return $guess_num;
        }
            
        say "You have $rslt->{black} blacks and $rslt->{white} whites.";

    }

    print <<"EOD";
You ran out of moves.  That's all you get.

The actual combination was: $pattern
EOD

    return MAX_GUESSES + 1;
}

# Print the $computer and $human score
sub print_score {
    my ( $computer, $human ) = @ARG;
    print <<"EOD";
Score:
     Computer: $computer
        Human: $human
EOD
    return;
}

# Get input from the user. The arguments are:
# * The prompt
# * A reference to validation code. This code receives the response in
#   $ARG and returns true for a valid response.
# * A warning to print if the response is not valid. This must end in a
#   return. It is suppressed if the validation code returned undef.
# The first valid response is returned. An end-of-file terminates the
# script.
sub get_input {
    my ( $prompt, $validate, $warning ) = @ARG;

    # If no validator is passed, default to one that always returns
    # true.
    $validate ||= sub { 1 };

    # Create the readline object. The 'state' causes the variable to be
    # initialized only once, no matter how many times this subroutine is
    # called. The do { ... } is a compound statement used because we
    # need to tweak the created object before we store it.
    state $term = do {
        my $obj = Term::ReadLine->new( 'reverse' );
        $obj->ornaments( 0 );
        $obj;
    };

    while ( 1 ) {   # Iterate indefinitely

        # Read the input into the topic variable, localized to prevent
        # Spooky Action at a Distance. We exit on undef, which signals
        # end-of-file.
        exit unless defined( local $ARG = $term->readline( $prompt ) );

        # Return the input if it is valid.
        return $ARG if my $rslt = $validate->();

        # Issue the warning, and go around the merry-go-round again.
        warn $warning if defined $rslt;
    }
}

# NOTE the following is unused, but left in place in case someone wants
# to add a 'Do you want instructions?'
#
# Get a yes-or-no answer. The argument is the prompt, which will have
# '? [y/n]: ' appended. The donkey work is done by get_input(), which is
# requested to validate the response as beginning with 'y' or 'n',
# case-insensitive. The return is a true value for 'y' and a false value
# for 'n'.
sub get_yes_no {
    my ( $prompt ) = @ARG;
    state $map_answer = {
        n   => 0,
        y   => 1,
    };
    my $resp = lc get_input(
        "$prompt? [y/n]: ",
        sub { m/ \A [yn] /smxi },
        "Please respond 'y' or 'n'\n",
    );
    return $map_answer->{ substr $resp, 0, 1 };
}

__END__

=head1 TITLE

mastermind - Play the game 'Mastermind' from Basic Computer Games

=head1 SYNOPSIS

 mastermind.pl

=head1 DETAILS

This Perl script is a port of mastermind, which is the 60th
entry in Basic Computer Games.

This is pretty much a re-implementation of the BASIC, taking advantage
of Perl's array functionality and working directly with the alphabetic
color codes.

=head1 PORTED BY

Thomas R. Wyant, III F<wyant at cpan dot org>

=head1 COPYRIGHT AND LICENSE

Copyright (C) 2022 by Thomas R. Wyant, III

This program is free software; you can redistribute it and/or modify it
under the same terms as Perl 5.10.0. For more details, see the Artistic
License 1.0 at
L<https://www.perlfoundation.org/artistic-license-10.html>, and/or the
Gnu GPL at L<http://www.gnu.org/licenses/old-licenses/gpl-1.0.txt>.

This program is distributed in the hope that it will be useful, but
without any warranty; without even the implied warranty of
merchantability or fitness for a particular purpose.

=cut

# ex: set expandtab tabstop=4 textwidth=72 :
