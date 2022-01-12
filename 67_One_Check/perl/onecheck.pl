#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use List::Util qw{ sum };   # Add all its arguments
use Term::ReadLine;         # Prompt and return user input

our $VERSION = '0.000_01';

print <<'EOD';
                              ONE CHECK
               Creative Computing  Morristown, New Jersey



Solitaire checker puzzle by David Ahl

48 checkers are placed on the 2 outside spaces of a
standard 64-square checkerboard.  The object is to
remove as many checkers as possible by diagonal jumps
(as in standard checkers).  Use the numbered board to
indicate the square you wish to jump from and to.  On
the board printed out on each turn '1' indicates a
checker and '0' an empty square.  When you have no
possible jumps remaining, input a '0' in response to
question 'Jump from?'
EOD

while ( 1 ) {   # Iterate indefinitely

    board_num();    # Display the numerical board.

    # Initialize the board, which is a two-dimensional array.
    my @board = map { [ ( 1 ) x 8 ] } 0 .. 7;   # Initialize to all 1.
    for my $row ( 2 .. 5 ) {        # Set the center section to 0
        for my $col ( 2 .. 5 ) {
            $board[$row][$col] = 0;
        }
    }

    print <<'EOD';
And here is the opening position of the checkers.

EOD
    board_pos( \@board );

    my $moves = 0;  # Number of moves made.

    # A game proceeds while 'Jump from' is a true value. We make use of
    # the fact that of the possible returns, only 0 evaluates false.
    while ( my $jump_from = get_input(
            'Jump from? ',
            sub {
                $ARG = lc;  # The caller sees this.
                return 1 if $ARG eq 'b';
                return unless m/ \A [0-9]+ \z /smx;
                $ARG += 0;  # Numify, because string '00' is true.
                return $ARG < 65;
            },
            "Please enter a number from 0 to 64, or 'b' to re-display the numeric board\n"
        )
    ) {
        if ( $jump_from eq 'b' ) {
            board_num();
            board_pos( \@board );
            next;
        }

        my $jump_to = get_input(
            '       to? ',
            sub { m/ \A [0-9]+ \z /smx },
            "Please enter a number from 1 to 64\n",
        );

        if ( make_move( \@board, $jump_from, $jump_to ) ) {
            $moves++;
            board_pos( \@board );
        } else {
            say 'Illegal move.  Try again.';
        }
    }

    my $checkers_left = sum( map { sum( @{ $board[$_] } ) } 0 .. 7 );
    print <<"EOD";

You made $moves jumps and had $checkers_left pieces
remaining on the board.

EOD

    last unless get_yes_no( 'Try again' );

}

print <<'EOD';

O.K.  Hope you had fun!!
EOD

# Print the numerical board
sub board_num {
    print <<'EOD';

Here is the numerical board:

EOD
    foreach my $row ( 0 .. 7 ) {
        state $tplt = ( '%3d' x 8 ) . "\n";
        my $inx = $row * 8;
        printf $tplt, map { $inx + $_ } 1 .. 8;
    }
    say '';
    return;
}

# Print the board position
sub board_pos {
    my ( $board ) = @_;
    for my $row ( 0 .. 7 ) {
        state $tplt = ( '%2d' x 8 ) . "\n";
        printf $tplt, @{ $board->[$row] };
    }
    say '';
    return;
}

# Make the move. This is a subroutine for convenience in control flow.
# We return a true value for success, and false for failure.
sub make_move {
    my ( $board, $jump_from, $jump_to ) = @_;
    $jump_from -= 1;
    $jump_to   -= 1;
    my $from_row = int( $jump_from / 8 );   # Truncates toward 0
    my $from_col = $jump_from % 8;
    my $to_row   = int( $jump_to / 8 );     # Truncates toward 0
    my $to_col   = $jump_to % 8;
    return unless $board->[$from_row][$from_col];   # From must be occupied
    return if $board->[$to_row][$to_col];           # To must be vacant
    return unless abs( $from_row - $to_row ) == 2;  # Must cross two rows
    return unless abs( $from_col - $to_col ) == 2;  # Must cross two cols
    my $over_row = ( $from_row + $to_row ) / 2;     # The row jumped over
    my $over_col = ( $from_col + $to_col ) / 2;     # The col jumped over
    $board->[$from_row][$from_col] =                # Clear the from cell
        $board->[$over_row][$over_col] = 0;         # and the jumped cell
    $board->[$to_row][$to_col] = 1;                 # Occupy the to cell
    return 1;
}

# Get input from the user. The arguments are:
# * The prompt
# * A reference to validation code. This code receives the response in
#   $ARG and returns true for a valid response.
# * A warning to print if the response is not valid. This must end in a
#   return.
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
        return $ARG if $validate->();

        # Issue the warning, and go around the merry-go-round again.
        warn $warning;
    }
}

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

one check - Play the game 'One Check' from Basic Computer Games

=head1 SYNOPSIS

 one check.pl

=head1 DETAILS

This Perl script is a port of onecheck.

This is a solitaire game played on a checker board, where the object is
to eliminate as many checkers as possible by making diagonal jumps and
removing the jumped checkers.

It is pretty much a straight port of the BASIC original.

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
