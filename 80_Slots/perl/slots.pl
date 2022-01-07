#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use List::Util qw{ shuffle };   # Shuffle an array.
use Scalar::Util qw{ looks_like_number };
use Term::ReadLine;     # Prompt and return user input

our $VERSION = '0.000_01';

print <<'EOD';
                                 SLOTS
               Creative Computing  Morristown, New Jersey



You are in the H&M casino, in front of one of our
one-arm bandits.  Bet from $1 to $100.
To pull the arm, punch the return key after making your bet.
EOD

my $winnings = 0;  # Winnings

while ( 1 ) {   # Iterate indefinitely

    say '';

    my $bet = get_input( 'Your bet? ',
        sub { m/ \A [0-9]+ \z /smx },
        'Please enter a whole number between 0 and 100',
    );
    if ( $bet > 100 ) {
        say 'The house limit is $100';
        next;
    }
    if ( $bet < 1 ) {
        say 'The minimum bet is $1';
        next;
    }

    say "\a" x 10;
    my $reel_x = int( 6 * rand() );
    my $reel_y = int( 6 * rand() );
    my $reel_z = int( 6 * rand() );
    foreach my $column ( $reel_x, $reel_y, $reel_z ) {
        state $symbol = [ qw{ Bar Bell Orange Lemon Plum Cherry } ];
        print $symbol->[$column], "\a" x 5, ' ';
    }

    use constant YOU_WON    => 'You won!';
    use constant YOU_LOST   => 'You lost.';

    say '';
    if ( $reel_x == $reel_y ) {
        if ( $reel_y == $reel_z ) {
            if ( $reel_z ) {
                say '** TOP DOLLAR **';
                $winnings += 11 * $bet;
            } else {
                say '*** JACKPOT ***';
                $winnings += 101 * $bet;
            }
            say YOU_WON;
        } elsif ( $reel_y ) {
            $winnings += double( $bet );
        } else {
            $winnings += double_bar( $bet );
        }
    } elsif ( $reel_x == $reel_z ) {
        if ( $reel_z ) {
            $winnings += double( $bet );
            # NOTE that the below code is what is actually implemented
            # in the basic, but it is implemented strangely enough (a
            # GOTO a line that contains a test that, if I understand the
            # control flow, must fail) that I wonder if it is an error.
            # I know nothing about slot machines, but research suggests
            # the payoff table is fairly arbitrary. The code above makes
            # code above makes the game orthogonal.
            # $winnings += you_lost( $bet );
        } else {
            $winnings += double_bar( $bet );
        }
    } elsif ( $reel_y == $reel_z ) {
        if ( $reel_z ) {
            $winnings += double( $bet );
        } else {
            $winnings += double_bar( $bet );
        }
    } else {
        $winnings += you_lost( $bet );
    }

    say 'Your standings are $', $winnings;

    last unless get_yes_no( 'Again' );

}

if ( $winnings < 0 ) {
    say 'Pay up!  Please leave your money on the terminal.';
} elsif ( $winnings > 0 ) {
    say 'Collect your winnings from the H&M cashier.';
} else {
    say 'Hey, you broke even.';
}

sub double {
    my ( $bet ) = @_;
    say 'DOUBLE!';
    say YOU_WON;
    return 3 * $bet;
}

sub double_bar {
    my ( $bet ) = @_;
    say '* DOUBLE BAR *';
    say YOU_WON;
    return 6 * $bet;
}

sub you_lost {
    my ( $bet ) = @_;
    say YOU_LOST;
    return -$bet;
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

slots - Play the game 'Slots' from Basic Computer Games

=head1 SYNOPSIS

 slots.pl

=head1 DETAILS

This Perl script is a port of C<slots>, which is the 80th entry in Basic
Computer Games.

I know nothing about slot machines, and my research into them says to me
that the payout tables can be fairly arbitrary. But I have taken the
liberty of deeming the BASIC program's refusal to pay on LEMON CHERRY
LEMON a bug, and made that case a double.

My justification for this is that at the point where the BASIC has
detected the double in the first and third reels it has already detected
that there is no double in the first and second reels. After the check
for a bar (and therefore a double bar) fails it goes back and checks for
a double on the second and third reels. But we know this check will
fail, since the check for a double on the first and second reels failed.
So if a loss was intended at this point, why not just call it a loss?

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
