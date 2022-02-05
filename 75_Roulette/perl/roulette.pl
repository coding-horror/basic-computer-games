#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use POSIX qw{ strftime };   # Time formatting
use Term::ReadLine;         # Prompt and return user input

our $VERSION = '0.000_01';

# A main() function is not usual in Perl scripts. I have installed one
# here to make the script into a "modulino." The next line executes
# main() if and only if caller() returns false. It will do this if we
# were loaded by another Perl script but not otherwise. This was done so
# I could test the payout and spin formatting logic.
main() unless caller;

sub main {

    print <<'EOD';
                                ROULETTE
               Creative Computing  Morristown, New Jersey




Welcome to the roulette table.

EOD

    if ( get_yes_no( 'Do you want instructions' ) ) {
        print <<'EOD';

This is the betting layout
  (*=red)

 1*    2     3*
 4     5*    6 
 7*    8     9*
10    11    12*
---------------
13    14*   15 
16*   17    18*
19*   20    21*
22    23*   24 
---------------
25*   26    27*
28    29    30*
31    32*   33 
34*   35    36*
---------------
    00    0    

Types of bets:

The numbers 1 to 36 signify a straight bet
on that number.
These pay off 35:1

The 2:1 bets are:
 37) 1-12     40) first column
 38) 13-24    41) second column
 39) 25-36    42) third column

The even money bets are:
 43) 1-18     46) odd
 44) 19-36    47) red
 45) even     48) black

 49) 0 and 50) 00 pay off 35:1
 Note: 0 and 00 do not count under any
       bets except their own.

When I ask for each bet, type the number
and the amount, separated by a comma.
For example: to bet $500 on black, type 48,500
when I ask for a bet.

The minimum bet is $5, the maximum is $500.

EOD
}

    my $P = 1000;
    my $D = 100000.;

    while ( 1 ) {   # Iterate indefinitely

        my $Y = get_input( 'How many bets? ',
            sub { m/ \A [0-9]+ \z /smx && $ARG > 0 && $ARG <= 50 },
            "Please enter a positive integer no greater than 50\n",
        );
        my @B;
        my @T;
        foreach my $C ( 1 .. $Y ) {
            my ( $X, $Z ) = split qr< , >smx, get_input(
                "Number $C: ",
                sub { m/ \A ( [0-9]+ ) , ( [0-9]+ ) \z /smx
                    && $1 > 0 && $1 <= 50 && $2 >= 5 && $2 <= 500 },
                "Please enter two comma-separated positive numbers\n",
            );
            if ( $B[$X] ) {
                say 'You made that bet once already, dum-dum.';
                redo;
            }
            $B[$X] = $Z;    # BASIC does $B[$C] = $Z
            $T[$C] = $X;
        }

        print <<'EOD';

    Spinning ...

EOD
        my $S = int rand 38;    # Zero-based, versus 1-based in BASIC

        say format_spin( $S );

        say '';

        foreach my $C ( 1 .. $Y ) {
            my $X = $T[$C];
            my $payout = payout( $S, $X ) * $B[$X];
            $D -= $payout;
            $P += $payout;
            if ( $payout > 0 ) {
                say "You win $payout dollars on bet $C";
            } else {
                $payout = -$payout;
                say "You lose $payout dollars on bet $C";
            }
        }
        say "Totals\tMe\tYou";
        say "\t$D\t$P";
        say '';


        last unless get_yes_no( 'Again' );
    }

    say '';

    if ( $P > 0 ) {
        my $B = get_input(
            'To whom shall I make out the check? ',
        );
        my $check_number = 1000 + int rand 9000;
        my $todays_date = strftime( '%B %d, %Y', localtime );
        print <<"EOD";

------------------------------------------------------------ Check number $check_number

                                        $todays_date

Pay to the order of ------ $B -----  \$$P

          The Memory Bank of New York

                                        The Computer
                                        ---------X-----

-------------------------------------------------------------------------------

Come back soon!
EOD
    } else {
        print <<'EOD';
Thanks for your money.
I'll use it to buy a solid gold roulette wheel
EOD
    }
}

{
    # Define the kind of each possible spin. 0 is '0' or '00', 1 is
    # black, and 2 is red. We assign the values in a BEGIN block because
    # execution never actually reaches this point in the script.
    my @kind;
    BEGIN {
        @kind = ( 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1, 2, 1,
            2, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 1, 2, 1, 2, 1, 2, 1, 2, 0,
            0 );
    }

    # Convert the spin (0-37) to its name on the wheel.
    sub format_spin {
        my ( $number ) = @_;
        state $format = [
            sub { '0' x ( $_[0] - 35 ) },
            sub { sprintf '%s Black', $_[0] + 1 },
            sub { sprintf '%s Red', $_[0] + 1 },
        ];
        return $format->[$kind[$number]]( $number );
    }

    # Compute the payout given the spin (0-37) and the bet (1-50).
    sub payout {
        my ( $number, $bet ) = @_;
        # We compute the payout on '0' and '00' directly, since under
        # our rules they are only eligible for the 35-to-1 bet.
        $kind[$number]
            or return $number == $bet - 49 + 36 ? 35 : -1;
        --$bet; # #bet is 1-based coming in
        # Dispatch table for computing the payout for spins 0-36.
        state $payout = [
            ( sub { $_[0] == $_[1] ? 35 : -1 } ) x 36,
            ( sub { int( $_[0] / 12 ) == $_[1] - 36 ? 2 : -1 } ) x 3,
            ( sub { $_[0] % 3 == $_[1] - 39 ? 2 : -1 } ) x 3,
            ( sub { int( $_[0] / 18 ) == $_[1] - 42 ? 1 : -1 } ) x 2,
            ( sub { $_[0] % 2 == 45 - $_[1] ? 1 : -1 } ) x 2,
            ( sub { $kind[$_[0]] == 48 - $_[1] ? 1 : -1 } ) x 2,
            ( sub { -1 } ) x 2, # Bet on '0' or '00' loses
        ];
        return $payout->[$bet]->( $number, $bet );
    }
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

roulette - Play the game 'Roulette' from Basic Computer Games

=head1 SYNOPSIS

 roulette.pl

=head1 DETAILS

This Perl script is a port of roulette, which is the 75th
entry in Basic Computer Games.

The main internal changes are converting the roulette slot numbering to
0-based and replacing most of the payout logic with a dispatch table.
These changes were tested for correctness against the original BASIC.

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
