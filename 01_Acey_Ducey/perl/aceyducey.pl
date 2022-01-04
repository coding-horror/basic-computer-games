#!/usr/bin/perl

use strict;
use warnings;

# The List::Util module is part of the core Perl distribution.  Using this
# means we don't need to re-invent the wheel and create a way to shuffle
# a list.
use List::Util qw(shuffle);

# Rather than put in a number of print (or say) statements here, we use a
# "here document".  This is very useful for long strings of text.  In this
# case, everything between the end of the "print" line and the line with
# "END_INSTRUCTIONS" on it will be printed verbatim.
print << 'END_INSTRUCTIONS';

Acey-Ducey
Adapted from Creative Computing, Morristown, New Jersey


Acey-Ducey is played as follows. The dealer (computer) deals two cards face up.
You have an option to bet or not bet, depending on whether or not you feel that
the next card drawn will have a value between the first two.  Aces are low.

Bets must be in whole-dollar amounts only.

If you do not want to bet, input a 0.  If you want to quit, input a -1.

END_INSTRUCTIONS

my @cards       = ( 1 .. 13 );    # That is, Ace through King.
my $keepPlaying = 1;

GAME:
while ($keepPlaying) {
    my $playerBalance = 100;      # The player starts with $100

    HAND:
    while (1) {
        print "\nYou now have $playerBalance dollars.\n\n";

        # We'll create a new array that is a shuffled version of the deck.
        my @shuffledDeck = shuffle(@cards);

        # Then, by taking the two "top cards" off the deck, we're guaranteed
        # that those will be unique.  This way we don't have to keep drawing
        # if we get, say, two queens.  We sort them as we pull them to make
        # sure that the first card is lower than the second one.
        my ( $firstCard, $secondCard ) = sort { $a <=> $b } @shuffledDeck[ 0 .. 1 ];

        print "I drew ", nameOfCard($firstCard), " and ", nameOfCard($secondCard), ".\n";

        my $bet = getValidBet($playerBalance);
        if ( $bet == 0 ) {
            print "Chicken!\n\n";
            next HAND;
        }

        if ( $bet < 0 ) {
            last GAME;
        }

        # Now we re-shuffle the whole deck again and choose a third card.
        # (Note: This is how the odds get stacked in favor of the dealer since
        # the third card can be exactly the same as the first or second.)
        @shuffledDeck = shuffle(@cards);
        my $thirdCard = $shuffledDeck[0];

        print "I drew ", nameOfCard($thirdCard), "!\n";

        if ( ( $firstCard < $thirdCard ) && ( $thirdCard < $secondCard ) ) {
            print "You win!\n\n";
            $playerBalance += $bet;
        }
        else {
            print "You lose!\n\n";
            $playerBalance -= $bet;
        }

        if ( $playerBalance <= 0 ) {
            print "Sorry, buddy, you blew your wad!\n\n";
            last HAND;
        }
    }

    $keepPlaying = promptUserToKeepPlaying();
}

print "Thanks for playing!\n";

###############
sub getValidBet {
    my $maxBet = shift;

    INPUT:
    {
        print "\nWhat's your bet? ";

        chomp( my $input = <STDIN> );

        # This regular expression will validate that the player entered an integer.
        # The !~ match operate *negates* the match, so if the player did NOT enter
        # an integer, they'll be given an error and prompted again.
        if (
            $input !~ /^        # Match the beginning of the string
                    [+-]?    # Optional plus or minus...
                    \d+      # followed by one more more digits...
                    $        # and then the end of the string
                    /x    # The x modifier ignores whitespace in this regex...
          )
        {
            print "Sorry, numbers only!\n";
            redo INPUT;
        }

        if ( $input > $maxBet ) {
            print "Sorry, my friend, you can't bet more money than you have.\n";
            print "You only have $maxBet dollars to spend!\n";
            redo INPUT;
        }

        return $input;
    }
}

# Since arrays in Perl are 0-based, we need to convert the value that we drew from
# the array to its proper position in the deck.
sub nameOfCard {
    my $value = shift;

    # Note that the Joker isn't used in this game, but since arrays in Perl are
    # 0-based, it's useful to have something there to represent the "0th"
    # position.  This way the rest of the elements match their expected values
    # (e.g., element 1 is Ace, element 7 is 7, and element 12 is Queen).

    my @cardlist = qw(Joker Ace 2 3 4 5 6 7 8 9 10 Jack Queen King);
    return $cardlist[$value];
}

sub promptUserToKeepPlaying {
    YESNO:
    {
        print "Try again (Y/N)? ";

        chomp( my $input = uc <STDIN> );

        if ( $input eq 'Y' ) {
            return 1;
        }
        elsif ( $input eq 'N' ) {
            return 0;
        }
        else {
            redo YESNO;
        }
    }
}
