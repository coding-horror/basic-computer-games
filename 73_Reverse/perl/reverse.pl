#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use List::Util qw{ shuffle };   # Shuffle an array.
use Term::ReadLine;     # Prompt and return user input

our $VERSION = '0.000_01';

# Manifest constant for size of list.
use constant NUMBER_OF_NUMBERS  => 9;

print <<'EOD';
                                REVERSE
               Creative Computing  Morristown, New Jersey



Reverse -- a game of skill

EOD

# Display the rules if desired. There is no straightforward way to
# interpolate a manifest constant into a string, but @{[ ... ]} will
# interpolate any expression.
print <<"EOD" if get_yes_no( 'Do you want the rules' );

This is the game of 'Reverse'.  To win, all you have
to do is arrange a list of numbers (1 through @{[ NUMBER_OF_NUMBERS ]})
in numerical order from left to right.  To move, you
tell me how many numbers (counting from the left) to
reverse.  For example, if the current list is:

2 3 4 5 1 6 7 8 9

and you reverse 4, the result will be:

5 4 3 2 1 6 7 8 9

Now if you reverse 5, you win!

1 2 3 4 5 6 7 8 9

No doubt you will like this game, but
if you want to quit, reverse 0 (zero).

EOD

while ( 1 ) {   # Iterate until something interrupts us.

    # Populate the list with the integers from 1, shuffled. If we
    # accidentally generate a winning list, just redo the loop.
    my @list = shuffle( 1 .. NUMBER_OF_NUMBERS );
    redo if is_win( \@list );

    print <<"EOD";

Here we go ... The list is:
EOD

    my $moves = 0;  # Move counter

    while ( 1 ) {   # Iterate until something interrupts us.
        print <<"EOD";

@list

EOD

        # Read the number of values to reverse. Zero is special-cased to
        # take us out of this loop.
        last unless my $max_index = get_input(
            'How many shall I reverse (0 to quit)? ',
            sub {
                return m/ \A [0-9]+ \z /smx &&
                    $ARG <= NUMBER_OF_NUMBERS;
            },
            "Oops! Too many! I can reverse at most " .
                NUMBER_OF_NUMBERS,
        );

        --$max_index;   # Convert number to reverse to upper index

        # Use a Perl array slice and the reverse() built-in to reverse
        # the beginning of the list.
        @list[ 0 .. $max_index ] = reverse @list[ 0 .. $max_index ];

        $moves++;   # Count a move

        # If we have not won, iterate again.
        next unless is_win( \@list );

        # Announce the win, and drop out of the loop.
        print <<"EOD";

You won it in $moves moves!!!
EOD
        last;
    }

    # Drop out of this loop unless the player wants to play again.
    say '';
    last unless get_yes_no( 'Try again' );
}

print <<'EOD';

O.K. Hope you had fun!!
EOD

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

# Determine if a given list represents a win. The argument is a
# reference to the array containing the list. We return a true value for
# a win, or a false value otherwise.
sub is_win {
    my ( $list ) = @_;
    my $expect = 1; # We expect the first element to be 1;

    # Iterate over the array.
    foreach my $element ( @{ $list } ) {

        # If the element does not have the expected value, we return
        # false. We post-increment the expected value en passant.
        $element == $expect++
            or return 0;
    }

    # All elements had the expected value, so we won. Return a true
    # value.
    return 1;
}

__END__

=head1 TITLE

reverse.pl - Play the game 'reverse' from Basic Computer Games

=head1 SYNOPSIS

 reverse.pl

=head1 DETAILS

This Perl script is a port of C<reverse>, which is the 73rd entry in
Basic Computer Games.

The cool thing about this port is the fact that, in a language with
array slices, list assignments, and a C<reverse()> built-in, the
reversal is a single assignment statement.

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
