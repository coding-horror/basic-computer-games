#!/usr/bin/env perl

use 5.010;      # To get 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use Getopt::Long 2.33 qw{ :config auto_version };
use List::Util qw{ shuffle };
use Pod::Usage;
use Term::ReadLine;     # Prompt and return user input

our $VERSION = '0.000_01';

my %opt;

GetOptions( \%opt,
    qw{ unicode! },
    help => sub { pod2usage( { -verbose => 2 } ) },
) or pod2usage( { -verbose => 0 } );

my @cards;
my $current_rank_value = 1;
my %rank_value;
my @suits = $opt{unicode} ?
    ( map { chr } 0x2660 .. 0x2663 ) :
    ( qw{ S H D C } );
foreach my $rank ( ( 2 .. 10 ), qw{ J Q K A } ) {
    $rank_value{$rank} = $current_rank_value++;
    foreach my $suit ( @suits ) {
        push @cards, "$suit-$rank";
    }
}

$opt{unicode}
    and binmode STDOUT, ':encoding(utf-8)';

@cards = shuffle( @cards );

print <<'EOD';
                                 WAR
               Creative Computing  Morristown, New Jersey



This is the card game of War.  Each card is given by suit-#
EOD

# Create the readline object.
state $term = Term::ReadLine->new( 'word' );

my $resp = $term->readline(
    "as $suits[0]-7 for Spade 7.  Do you want directions? [y/N]: " );
exit unless defined $resp;
if ( $resp =~ m/ \A y /smxi ) {
    print <<'EOD';
The computer gives you and it a 'card'.  The higher card
(numerically) wins.  The game ends when you choose not to
continue or when you have finished the pack.

EOD
}

my $your_score = my $computer_score = 0;

while ( 1 ) {
    my ( $you, $computer ) = splice @cards, 0, 2;
    say '';
    say "You: $you; computer: $computer";
    my $result = $rank_value{ substr $you, 2 } <=>
        $rank_value{ substr $computer, 2 };
    if ( $result < 0 ) {
        $computer_score++;
        say "The computer wins!!!  ",
            "You have $your_score and the computer has $computer_score";
    } elsif ( $result > 0 ) {
        $your_score++;
        say "You win.  ",
            "You have $your_score and the computer has $computer_score";
    } else {
        say 'Tie.  No score change.';
    }

    last unless @cards;

    $resp = $term->readline( 'Do you want to continue? [Y/n]: ' );
    last unless defined $resp;
    last if $resp =~ m/ \A n /smxi;
}

say "We have run out of cards.  ",
    "Final score:  you: $your_score;  the computer: $computer_score"
    unless @cards;
say '';
say 'Thanks for playing.  It was fun.';
__END__

=head1 TITLE

war.pl - Play the game 'War' from Basic Computer Games

=head1 SYNOPSIS

 war.pl
 war.pl --help
 war.pl --version

=head1 OPTIONS

=head2 --help

This option displays the documentation for this script. The script then
exits.

=head2 --unicode

If this Boolean option is asserted, the suits are designated by their
Unicode glyphs rather than by ASCII letters. For these to display
properly your terminal must properly interpret Unicode.

The default is C<--no-unicode>.

=head2 --version

This option displays the version of this script. The script then exits.

=head1 DETAILS

This Perl script is a port of C<war>, which is the 94th entry in Basic
Computer Games.

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
