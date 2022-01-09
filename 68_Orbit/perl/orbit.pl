#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use Term::ReadLine;     # Prompt and return user input

our $VERSION = '0.000_01';

use constant PI => atan2( 0, -1 );
use constant DEG_TO_RAD => atan2( 0, -1 ) / 180;

print <<'EOD';
                                 ORBIT
               Creative Computing  Morristown, New Jersey



Somewhere above your planet is a Romulan ship.

The ship is in a constant polar orbit.  Its
distance from the center of your planet is from
10,000 to 30,000 miles and at its present velocity can
circle your planet once every 12 to 36 hours.

Unfortunately, they are using a cloaking device so
you are unable to see them, but with a special
instrument you can tell how near their ship your
photon bomb exploded.  You have seven hours until they
have built up sufficient power in order to escape
your planet's gravity.

Your planet has enough power to fire one bomb an hour.

At the beginning of each hour you will be asked to give an
angle (between 0 and 360) and a distance in units of
100 miles (between 100 and 300), after which your bomb's
distance from the enemy ship will be given.

An explosion within 5,000 miles of the Romulan ship
will destroy it.

Below is a diagram to help you visualize your plight.


                          90
                    0000000000000
                 0000000000000000000
               000000           000000
             00000                 00000
            00000    XXXXXXXXXXX    00000
           00000    XXXXXXXXXXXXX    00000
          0000     XXXXXXXXXXXXXXX     0000
         0000     XXXXXXXXXXXXXXXXX     0000
        0000     XXXXXXXXXXXXXXXXXXX     0000
180<== 00000     XXXXXXXXXXXXXXXXXXX     00000 ==>0
        0000     XXXXXXXXXXXXXXXXXXX     0000
         0000     XXXXXXXXXXXXXXXXX     0000
          0000     XXXXXXXXXXXXXXX     0000
           00000    XXXXXXXXXXXXX    00000
            00000    XXXXXXXXXXX    00000
             00000                 00000
               000000           000000
                 0000000000000000000
                    0000000000000
                         270

X - Your planet
O - The orbit of the Romulan ship

On the above diagram, the Romulan ship is circling
counterclockwise around your planet.  Don't forget that
without sufficient power the Romulan ship's altitude
and orbital rate will remain constant.

Good luck.  The Federation is counting on you.
EOD

while ( 1 ) {   # Iterate indefinitely

    my $romulan_angle = int( 360 * rand() );
    my $romulan_distance = int( 200 * rand() + 200 );
    my $romulan_velocity = int( 20 * rand() + 10 );

    my $hour = 0;
    while ( 1 ) {   # Iterate indefinitely
        $hour++;

        print <<"EOD";


This is hour $hour, at what angle do you wish to send
EOD
        my $bomb_angle = get_input(
            'do you wish to send your photon bomb? ',
            sub { m/ \A [0-9]+ \z /smx },
            "Please enter an integer angle in degrees\n",
        );
        say '';
        my $bomb_distance = get_input(
            'How far out do you wish to detonate it? ',
            sub { m/ \A [0-9]+ \z /smx },
            "Please enter an integer distance in hundreds of miles\n",
        );

        $romulan_angle = ( $romulan_angle + $romulan_velocity ) % 360;
        my $miss_angle = abs( $romulan_angle - $bomb_angle );
        $miss_angle = 360 - $miss_angle if $miss_angle >= 180;
        my $miss_distance = int sqrt(
            $romulan_distance * $romulan_distance +
            $bomb_distance * $bomb_distance -
            2 * $romulan_distance * $bomb_distance *
                cos( $miss_angle * DEG_TO_RAD ) );
        print <<"EOD";

Your photon bomb exploded $miss_distance*10^2 miles from the
Romulan ship.
EOD
        if ( $miss_distance <= 50 ) {
            say "\nYou have successfully completed your mission.";
            last;
        } elsif ( $hour > 6 ) {
            say "\nYou have allowed the Romulans to escape.";
            last;
        }
    }

    say "\nAnother Romulan ship has gone into orbit.";
    last unless get_yes_no( 'Do you wish to try to destroy it' );
}

print <<'EOD';

Good bye.
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

__END__

=head1 TITLE

orbit - Play the game 'Orbit' from Basic Computer Games

=head1 SYNOPSIS

 orbit.pl

=head1 DETAILS

This Perl script is a port of orbit, which is the 68th entry in Basic
Computer Games.

In this game you are a planetary defense gunner trying to shoot down a
cloaked Romulan ship before it can escape.

This is pretty much a straight port of the BASIC into idiomatic Perl.

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
