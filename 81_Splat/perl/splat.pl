#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use List::Util qw{ shuffle };   # Shuffle an array.
use Scalar::Util qw{ looks_like_number };
use Term::ReadLine;     # Prompt and return user input

our $VERSION = '0.000_01';

use constant ROW_TPLT => ( '%4d' x 8 ) . "\n";

print <<'EOD';
                                 SPLAT
               Creative Computing  Morristown, New Jersey



Welcome to 'Splat' -- the game that simulates a parachute
jump.  Try to open your chute at the last possible 
moment without going splat.
EOD

while ( 1 ) {
    say '';
    my $initial_altitude = int( 9001 * rand() + 1000 );

    my $nominal_terminal_velocity;
    if ( get_yes_no( 'Select your own terminal velocity' ) ) {
        $nominal_terminal_velocity = get_input(
            'What terminal velocity (mi/hr)? ',
            sub { looks_like_number( $ARG ) && $ARG > 0 },
            'Please enter a positive number',
        );
        # Convert miles per hour to feet per second
        $nominal_terminal_velocity = $nominal_terminal_velocity * 5280 / 3600;
    } else {
        $nominal_terminal_velocity = int( 1000 * rand() );
        say "OK.  Terminal velocity = $nominal_terminal_velocity mi/hr"
    }
    my $terminal_velocity = dither( $nominal_terminal_velocity );

    my $nominal_gravity; # Acceleration due to gravity
    if ( get_yes_no( 'Want to select acceleration due to gravity' ) ) {
    } else {
        state $body = [
            [ q<Fine. You're on Mercury. Acceleration = 12.2 ft/sec/sec.>,
                12.2 ],
            [ q<All right. You're on Venus. Acceleration = 28.3 ft/sec/sec.>,
                28.3 ],
            [ q<Then you're on Earth. Acceleration = 32.16 ft/sec/sec.>,
                32.16 ],
            [ q<Fine. You're on the Moon. Acceleration = 5.15 ft/sec/sec.>,
                5.15 ],
            [ q<All right. You're on Mars. Acceleration = 12.5 ft/sec/sec.>,
                12.5 ],
            [ q<Then you're on Jupiter. Acceleration = 85.2 ft/sec/sec.>,
                85.2 ],
            [ q<Fine. You're on Saturn. Acceleration = 37.6 ft/sec/sec.>,
                37.6 ],
            [ q<All right. You're on Uranus. Acceleration = 33.8 ft/sec/sec.>,
                33.8  ],
            [ q<Then you're on Neptune. Acceleration = 39.6 ft/sec/sec.>,
                39.6 ],
            [ q<Fine. You're on the Sun. Acceleration = 896 ft/sec/sec.>,
                896 ],
        ];
        my $pick = $body->[ rand scalar @{ $body } ];
        say $pick->[0];
        $nominal_gravity = $pick->[1];
    }
    my $gravity = dither( $nominal_gravity );

    print <<"EOD";

    Altitude        = $initial_altitude ft
    Term. velocity  = $nominal_terminal_velocity ft/sec +/- 5%
    Acceleration    = $nominal_gravity ft/sec/sec +/- 5%
Set the timer for your freefall
EOD

    my $drop_time = get_input(
        'How many seconds? ',
        sub { m/ \A [0-9]+ \z /smx },
        "Please enter an unsigned integer\n",
    );

    print <<'EOD';
Here we go.

Time (sec)      Dist to fall (ft)
==========      =================
EOD

    if ( defined( my $altitude = make_jump(
                $initial_altitude,
                $gravity,
                $terminal_velocity,
                $drop_time ) )
    ) {
        # Successful jump
        state $succesful = [];
        state $ordinal = [ qw{ 1st 2nd 3rd } ];
        if ( defined( my $ord = $ordinal->[ @{ $succesful } ] ) ) {
            say "Amazing!!! Not nad for your $ord successful jump!!!";
        } else {
            my $jumps = @{ $succesful };
            my $worse = grep { $_ > $altitude } @{ $succesful };
            my $fractile = 1 - $worse / $jumps;
            my $better = $jumps - $worse;
            if ( $fractile <= 0.1 ) {
                print <<"EOD";
Wow!  That's some jumping.  Of the $jumps successful jumps
before yours, only $better opened their chutes lower than 
you did. 
EOD
            } elsif ( $fractile <= 0.25 ) {
                print <<"EOD";
Pretty good! $jumps successful jumps preceded yours and only
$better of them got lower than you did before their chutes
opened.
EOD
            } elsif ( $fractile <= 0.5 ) {
                print <<"EOD";
Not bad.  There have been $jumps successful jumps before yours. 
You were beaten out by $better of them.
EOD
            } elsif ( $fractile <= 0.75 ) {
                print <<"EOD";
Conservative, aren't you?  You ranked only $better in the
$jumps successful jumps before yours.
EOD
            } elsif ( $fractile <= 0.9 ) {
                print <<"EOD";
Humph!  Don't you have any sporting blood?  There were 
$jumps successful jumps before yours and you came in $worse jumps
better than the worst.  Shape up!!!
EOD
            } else {
                print <<"EOD";
Hey!  You pulled the rip cord much too soon.  $jumps successful
jumps before yours and you came in number $better!  Get with it!
EOD
            }
        }
        push @{ $succesful }, $altitude;
    } else {
        # Splat

        say q<I'll give you another chance.>;
    }

    next if get_yes_no( 'Do you want to play again' );
    next if get_yes_no( 'Please' );

    print <<'EOD';
Ssssssssss.

EOD
    last;

}

# Return the first argument modified by up to plus or minus some
# fraction specified by the second argument (default 0.05)
sub dither {
    my ( $arg, $fract ) = @_;
    $fract //= 1 / 20;
    return $arg + ( $arg * rand() * $fract ) - ( $arg * rand() * $fract );
}

use constant FORMAT_FALL    => "%10.1f      %10d\n";
use constant FORMAT_SPLAT   => "%10.1f      %s\n";
sub make_jump {
    my ( $initial_altitude, $gravity, $terminal_velocity, $drop_time ) = @_;
    my $altitude;
    foreach my $step ( 0 .. 8 ) {
        my $time = $step * $drop_time / 8;
        if ( $time > $terminal_velocity / $gravity ) {
            # Terminal velocity reached
            printf "Terminal velocity reached at T plus %.2f seconds.\n",
                $terminal_velocity / $gravity;
            for my $step ( $step .. 8 ) {
                my $time = $step * $drop_time / 8;
                $altitude = $initial_altitude - (
                    $terminal_velocity * $terminal_velocity /
                    ( 2 * $gravity ) + $terminal_velocity * (
                        $time - $terminal_velocity / $gravity ) );
                if ( $altitude > 0 ) {
                    printf FORMAT_FALL, $time, $altitude;
                } else {
                    splat(
                        $terminal_velocity / $gravity + (
                            $initial_altitude -
                            $terminal_velocity * $terminal_velocity /
                            ( 2 * $gravity ) ) / $terminal_velocity,
                    );
                    return;
                }
            }
            last;
        } else {
            $altitude = $initial_altitude - $gravity / 2 * $time * $time;
            if ( $altitude > 0 ) {
                printf FORMAT_FALL, $time, $altitude;
            } else {
                splat( sqrt( 2 * $initial_altitude / $gravity ) );
                return;
            }
        }
    }

    say 'Chute open.';
    return $altitude;
}

sub splat {
    my ( $time ) = @_;
    printf FORMAT_SPLAT, $time, 'Splat!';
    state $rip = [
        q<Requiescat in pace.>,
        q<May the angel of heaven lead you into paradise.>,
        q<Rest in peace.>,
        q<Son-of-a-gun.>,
        q<#$%&&%!$>,
        q<A kick in the pants is a boost if you're headed right.>,
        q<Hmmm. Should have picked a shorter time.>,
        q<Mutter. Mutter. Mutter.>,
        q<Pushing up daisies.>,
        q<Easy come, easy go.>,
    ];
    say $rip->[ rand scalar @{ $rip } ];
    return;
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

splat.pl - Play the game 'splat' from Basic Computer Games

=head1 SYNOPSIS

 splat.pl

=head1 DETAILS

This Perl script is a port of C<splat>, which is the 73rd entry in
Basic Computer Games.

This is a very basic port. All I really did was untangle the spaghetti.

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
