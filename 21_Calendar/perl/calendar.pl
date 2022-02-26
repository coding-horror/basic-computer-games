#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use POSIX qw{ strftime };
use Term::ReadLine;     # Prompt and return user input
use Time::Local ();

BEGIN {
    *time_gm =
        Time::Local->can( 'timegm_modern' ) ||
        Time::Local->can( 'timegm' );
}

our $VERSION = '0.000_01';

use constant COLUMN_WIDTH       => 6;
use constant SECONDS_PER_DAY    => 86400;

binmode STDOUT, ':encoding(utf-8)';

my $year = @ARGV ? $ARGV[0] : ( localtime )[5] + 1900;
my $is_leap_year = is_leap_year( $year );
my $year_len = 365 + $is_leap_year;
print <<'EOD';
                                CALENDAR
               Creative Computing  Morristown, New Jersey


EOD

my @mon_len = ( 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 );
$mon_len[1] += $is_leap_year;

foreach my $month ( 0 .. 11 ) {
    my $epoch = time_gm( 0, 0, 0, 1, $month, $year );
    my @start_time = gmtime( $epoch );
    my ( $week_day, $year_day ) = @start_time[ 6, 7 ];
    my $label = strftime( '%B %Y', @start_time );
    $label .= ' ' x ( ( 14 - length $label ) / 2 );
    printf "\n** %3d ****** %14s ****** %3d **\n",
    $year_day, $label, $year_len - $year_day;
    {
        my $day = 1 + ( 7 - $week_day ) % 7;
        foreach my $wd ( 0 .. 6 ) {
            my $ep = time_gm( 0, 0, 0, $day + $wd, $month, $year );
            printf '%*s', COLUMN_WIDTH, strftime( '%a', gmtime $ep );
        }
        print "\n";
    }
    say '*' x ( COLUMN_WIDTH * 7 );
    print ' ' x ( COLUMN_WIDTH * $week_day );
    my $month_day = 1;
    while ( $week_day < 7 ) {
        printf '%*d', COLUMN_WIDTH, $month_day++;
        $week_day++;
    }
    print "\n";
    $week_day = 0;
    while ( $month_day <= $mon_len[$month] ) {
        printf '%*d', COLUMN_WIDTH, $month_day++;
        $week_day++;
        unless ( $week_day % 7 ) {
            print "\n";
            $week_day = 0;
        }
    }
    print "\n" if $week_day;

}

sub is_leap_year {
    my ( $year ) = 1;
    return 0 if $year % 4;
    return 1 if $year % 100;
    return 0 if $year % 400;
    return 1;
}

__END__

=head1 TITLE

calendar - Play the game 'Calendar' from Basic Computer Games

=head1 SYNOPSIS

 calendar.pl

=head1 DETAILS

This Perl script is a port of calendar, which is the 21st
entry in Basic Computer Games.

Actually, it is not so much a port as a complete rewrite, making use of
Perl's Posix time functionality. The calendar is for the current year
(not 1979), but you can get another year by specifying it on the command
line, e.g.

 perl 21_Calendar/perl/calendar.pl 2001

It B<may> even produce output in languages other than English. But the
leftmost column will still be Sunday, even in locales where it is
typically Monday.

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
