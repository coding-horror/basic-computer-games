#!/usr/bin/env perl

use 5.010;      # To get 'state' and 'say'

use strict;     # Require explicit declaration of variables
use warnings;   # Enable optional compiler warnings

use English;    # Use more friendly names for Perl's magic variables
use Term::ReadLine;     # Prompt and return user input

our $VERSION = '0.000_01';

# The Perl ref() built-in returns 'HASH' for a hash reference. But we
# make it a manifest constant just to avoid typos.
use constant REF_HASH   => ref {};

print <<'EOD';
                                ANIMAL
               Creative Computing  Morristown, New Jersey



Play 'Guess the Animal'
Think of an animal and the computer will try to guess it.

EOD

# We keep the accumulated data in a tree structure, initialized here. As
# we accumulate animals, we replace the 'yes' or 'no' keys with new hash
# references.
my $database = {
    question    => 'Does it swim',  # Initial question
    yes         => 'fish',          # Result of answering 'y'
    no          => 'bird',          # Result of answering 'n'
};

while ( 1 ) {

    my $resp = get_input(
        'Are you thinking of an an animal? [y/n/list]: '
    );

    if ( $resp =~ m/ \A y /smxi ) {
        # If we got an answer beginning with 'y', walk the database
        walk_tree( $database );
    } elsif ( $resp =~ m/ \A list \z /smxi ) {
        # If we got 'list', list the currently-known animals.
        say '';
        say 'Animals I already know are:';
        say "    $_" for sort( list_animals( $database ) );
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
        my $obj = Term::ReadLine->new( 'animal' );
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

# Recurse through the database, returning the names of all animals in
# it, in an undefined order.
sub list_animals {
    my ( $node ) = @ARG;
    return $node unless REF_HASH eq ref $node;
    return( map { list_animals( $node->{$_} ) } qw{ yes no } );
}

# Find or create the desired animal.
# Ask the question stored in the node given in its argument. If the key
# selected by the answer ('yes' or 'no') is another node, recurse. If it
# is an animal name, confirm it, or add a new animal as appropriate.
sub walk_tree {
    my ( $node ) = @ARG;

    # Ask the question associated with this node. Turn the true/false
    # response into 'yes' or 'no', since those are the names of the
    # respective keys.
    my $resp = get_yes_no ( $node->{question} ) ? 'yes' : 'no';

    # Chose the datum for the response.
    my $choice = $node->{ $resp };

    # If the datum is a hash reference
    if ( REF_HASH eq ref $choice ) {

        # Recurse into it
        walk_tree( $choice );

    # Otherwise it is an actual animal (i.e. terminal node). Check it.
    } else {

        # If this is not the animal the player was thinking of
        unless ( get_yes_no( "Is it a $choice" ) ) {

            # Find out what animal the player was thinking of
            my $animal = lc get_input(
                'The animal you were thinking of was a ',
            );

            # Get a yes/no question that distinguishes the animal the
            # player was thinking of from the animal we found in the
            # tree.
            say 'Please type in a question that would distinguish a';
            my $question = get_input( "$animal from a $choice: " );

            # Find out whether the new animal is selected by 'yes' or
            # 'no'. If 'no', swap the original animal with the new one
            # for convenience.
            ( $choice, $animal ) = ( $animal, $choice ) if get_yes_no(
                "For a $animal the answer would be",
            );

            # Replace the animal we originally found by a new node
            # giving the original animal, the new animal, and the
            # question that distinguishes them.
            $node->{ $resp } = {
                question    => $question,
                no          => $animal,
                yes         => $choice,
            };
        }

        # Find out if the player wants to play again. If not, exit. If
        # so, just return.
        say '';
        exit unless get_yes_no( 'Why not try another animal' );
        return;
    }
}

__END__

=head1 TITLE

animal.pl - Play the game 'animal' from Basic Computer Games

=head1 SYNOPSIS

 animal.pl

=head1 DETAILS

This Perl script is a port of C<animal>, which is the 3ed entry in Basic
Computer Games.

The original BASIC was greatly complicated by the need to emulate a
binary tree with an array. The implementation using hashes as nodes in
an actual binary tree is much simpler.

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
