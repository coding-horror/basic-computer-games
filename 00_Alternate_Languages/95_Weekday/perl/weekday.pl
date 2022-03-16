#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use Term::ReadLine;     # Prompt and return user input
use Time::Local qw{ timelocal };    # date-time to epoch
# FIXME timelocal() is too smart for its own good in the interpretation
# of years, and caused a bunch of Y2020 problems in Perl code that used
# it. I believe that this script avoids these problems (which only occur
# if the year is less than 1000), but it is probably safer in general to
# use timelocal_modern() or timelocal_posix(). These are also exported
# by Time::Local, but only by versions 1.28 and 1.30 respectively. This
# means that they only come (by default) with Perl 5.30 and 5.34
# respectively. Now, Time::Local is a dual-life module, meaning it can
# be upgraded from the version packaged with older Perls. But I did not
# want to assume that it HAD been upgraded. Caveat coder.
use Time::Piece;    # O-O epoch to date-time, plus formatting

our $VERSION = '0.000_01';

print <<'EOD';

                                WEEKDAY
               Creative Computing  Morristown, New Jersey



WEEKDAY is a computer demonstration that
gives facts about a date of interest to you.

EOD

my $now = localtime;
my $default_date = join ',', map { $now->$_() } qw{ mon mday year };

my $today = get_date(
    "Enter today's date in the form month,day,year (default: $default_date): ",
    "Please enter month,day,year or return for default\n",
    $default_date,
);

my $birthday = get_date(
    'Ender day of birth (or other day of interest): ',
    "Please enter month,day,year\n",
);

say '';
printf "%d/%d/%d %s a %s\n", $birthday->mon, $birthday->mday,
    $birthday->year, tense( $today, $birthday),
    ( $birthday->mday == 13 && $birthday->wday == 6 ) ?
        $birthday->fullday . ' the thirteenth --- Beware!' :
        $birthday->fullday . '.';

if ( $birthday->epoch <= $today->epoch ) {

    say '*** Happy Birthday! ***'
        if $birthday->mon == $today->mon &&
            $birthday->mday == $today->mday;

    print <<'EOD';
                        Years   Months  Days
                        -----   ------  ----
EOD

    my @delta = map { $today->$_() - $birthday->$_() } qw{ year mon mday };
    if ( $delta[2] < 0 ) {
        $delta[2] += 30;
        $delta[1] -= 1;
    }
    if ( $delta[1] < 0 ) {
        $delta[1] += 12;
        $delta[0] -= 1;
    }
    my @residue = @delta;

    my $delta_days = 365 * $delta[0] + 30 * $delta[1] + $delta[2];

    display_ymd( 'Your age (if birthdate)', compute_ymd( $delta_days ) );
    display_ymd( 'You have slept', compute_ymd( $delta_days, 0.35,
            \@residue ) );
    display_ymd( 'You have eaten', compute_ymd( $delta_days, 0.17,
            \@residue ) );
    display_ymd(
        $residue[0] > 9 ? 'You have worked/played' :
        $residue[0] > 3 ? 'You have played/studied' :
            'You have played',
        compute_ymd( $delta_days, 0.23,
            \@residue ) );
    display_ymd( 'You have relaxed', \@residue );

    say '';
    say "\t\t*** You may retire in @{[ $birthday->year + 65 ]} ***";
}

say '';

sub compute_ymd {
    my ( $delta_days, $fract, $residue ) = @ARG;
    my $days = defined $fract ? int ( $delta_days * $fract ) : $delta_days;
    my $years = int( $days / 365 );
    $days -= $years * 365;
    my $months = int( $days / 30 );
    $days -= $months * 30;

    if ( $residue ) {
        $residue->[2] -= $days;
        if ( $residue->[2] < 0 ) {
            $residue->[2] += 30;
            $residue->[1] -= 1;
        }
        $residue->[1] -= $months;
        if ( $residue->[1] < 0 ) {
            $residue->[1] += 12;
            $residue->[0] -= 1;
        }
        $residue->[0] -= $years;
    }

    return [ $years, $months, $days ];
}

sub display_ymd {
    my ( $label, $ymd ) = @ARG;
    printf "%-24s%4d%6d%8d\n", $label, @{ $ymd };
    return;
}

sub get_date {
    my ( $prompt, $warning, $default ) = @ARG;
    my ( $month, $day, $year ) = split qr< [[:punct:]] >smx, get_input(
        $prompt,
        sub {
            return 0 unless m/ \A (?: [0-9]+ [[:punct:]] ){2} ( [0-9]+ ) \z /smx;
            return 1 if $1 >= 1582;
            warn "Not prepared to give day of week prior to MDLXXXII.\n";
            return 0;
        },
        $warning,
        $default,
    );
    return localtime timelocal( 0, 0, 0, $day, $month - 1, $year );
}

sub tense {
    my ( $today, $birthday ) = @ARG;
    my $cmp = $birthday->epoch <=> $today->epoch
        or return 'is';
    return $cmp < 0 ? 'was' : 'will be';
}

# Get input from the user. The arguments are:
# * The prompt
# * A reference to validation code. This code receives the response in
#   $ARG and returns true for a valid response.
# * A warning to print if the response is not valid. This must end in a
#   return.
# * A default to return if the user simply presses <return>.
# The first valid response is returned. An end-of-file terminates the
# script.
sub get_input {
    my ( $prompt, $validate, $warning, $default ) = @ARG;

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

        # Return the default if it exists AND we got an empty line
        return $default if defined( $default ) && $ARG eq '';

        # Return the input if it is valid.
        return $ARG if $validate->();

        # Issue the warning, and go around the merry-go-round again.
        warn $warning;
    }
}

__END__

=head1 TITLE

weekday - Play the game 'Weekday' from Basic Computer Games

=head1 SYNOPSIS

 weekday.pl

=head1 DETAILS

This Perl script is a port of weekday.bas, which is the 95th entry in
Basic Computer Games.

I have replaced the manual date logic with Perl built-ins to the extent
possible. Unfortunately the kind of date math involved in the "time
spent doing ..." functionality is not well-defined, so I have been
forced to retain the original logic here. Sigh.

You can use any punctuation character you please in the date
input. So something like 2/29/2020 is perfectly acceptable.

It would also have been nice to produce a localized version that
supports day/month/year or year-month-day input, but that didn't happen.

Also nice would have been language-specific output -- especially if it
could have accommodated regional differences in which day of the week or
month is unlucky.

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
