#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use Term::ReadLine;     # Prompt and return user input

our $VERSION = '0.000_01';

print <<'EOD';
                                 WORD
               Creative Computing  Morristown, New Jersey



I am thinking of a word -- you guess it.  I will give you
clues to help you get it.  Good luck!!


EOD

# Read the content of __DATA__, remove the trailing newlines, and store
# each line into @words. Stop at __END__, since Perl does not see this
# as an end-of-file.
my @words;
while ( <DATA> ) {
    chomp;
    last if $ARG eq '__END__';
    push @words, lc $ARG;   # Normalize case to lower.
}

# This loop represents an actual game. We execute it until the player
# does something that makes us explicitly break out.
while ( 1 ) {
    print <<'EOD';


You are starting a new game ...
EOD

    # Choose a random target word. The rand() function returns a number
    # from 0 to its argument, and coerces its argument to a scalar. In
    # scalar context, an array evaluates to the number of elements it
    # contains.
    my $target = $words[ rand @words ];

    # We generalize the code by using the actual length of the target.
    my $target_length = length $target;

    my $count = 0;      # Number of guesses

    # Make an array of the individual letters in the target. We will
    # iterate over this to determine matching letters.
    my @target_array = split qr<>, $target;

    # Make a hash of those letters. We will use this to determine common
    # letters. Any true value will do for the value of the hash. By
    # making use of this hash we avoid the nested loops of the original
    # BASIC program.
    my %target_hash = map { $ARG => 1 } @target_array;

    # We keep prompting the player until we get a response that causes
    # us to break out of the loop.
    while ( 1 ) {

        # Create the readline object. The state keyword means the
        # variable is only initialized once, no matter how many times
        # execution passes this point.
        state $term = Term::ReadLine->new( 'word' );

        # Read the next guess. A return of undef means end-of-file.
        my $guess = $term->readline( "Guess a $target_length letter word: " );
        exit unless defined $guess;

        last if $guess eq '?';  # A question mark means we give up
        if ( length( $guess ) != $target_length ) {
            # Wrong length. Ask again.
            say "You must guess a $target_length letter word.  Try again.";
            redo;       # Redo the innermost loop
        }

        $guess = lc $guess;     # Lower-case the guess
        $count++;       # Count another guess

        if ( $guess eq $target ) {
            # We guessed the word.
            say "You have guessed the word. It took $count guesses!";
            my $answer = $term->readline( 'Want to play again? [y/N]: ');
            exit unless defined $guess; # End of file
            exit unless $guess =~ m/ \A y /smxi;
            last;       # Exit the innermost loop.
        }

        my @common_letters; # Letters common to guess and target
        my $match = '-' x length $target;   # Assume no matches
        my $inx = 0;    # Iterator
        foreach my $letter ( split qr<>, $guess ) {
            if ( $target_hash{$letter} ) {
                # If the letter is in the hash, it occurs in the target
                push @common_letters, $letter;
                # If it is at the current position in the target, it is
                # an actual match.
                $target_array[$inx] eq $letter
                    and substr $match, $inx, 1, $letter;
            }
            $inx++;
        }

        say 'There were ', scalar @common_letters,
            ' matches and the common letters were... ', @common_letters;
        say "From the exact letter matches, you know................ $match";
        say '';
        say q<If you give up, type '?' for your next guess.>;
        redo;
    }

}
__DATA__
dinky
smoke
water
grass
train
might
first
candy
champ
would
clump
dopey
__END__

=head1 TITLE

word.pl - Play the game 'word' from Basic Computer Games

=head1 SYNOPSIS

 word.pl

=head1 DETAILS

This Perl script is a port of C<word>, which is the 96th entry in Basic
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
