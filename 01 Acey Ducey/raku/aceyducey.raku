#!/usr/bin/env raku
use v6.d;

=begin comment
This is a complete rewrite from scratch
=end comment

# Note that this is a special sub that can be called automatically
sub USAGE () {
    print q:to<END>
        Two cards are dealt face-up.
        You then bet how much money you want to risk.

        If the next card falls between those two cards you gain the amount you bet.
        If it is outside of those cards, you lose that amount
        If it matches one of the cards, you lose double.

        If the first two cards are the same value,
        you guess whether the next card will be higher or lower, and bet on that.
        If all three cards are the same, you lose triple your bet.
        END
}

constant $Card-Back = "\c[PLAYING CARD BACK]";

class Card {
    # for viewing
    has Str $.Str is required;

    # for comparison
    has Int $.Int is required;
    method Numeric () { $!Int }
    method Real    () { $!Int }
}

class Deck {
    # Internal use:
    # convert to a Card object
    sub card (Pair (:key($Str), :value($Int))) {
        Card.new(:$Str, :$Int)
    }

    # Internal use:
    # a base deck of Cards that needs to be shuffled
    constant @Base-Deck := flat(
            ('🂡' .. '🂮' Z=> flat 14, 2 .. 13),
            ('🂱' .. '🂾' Z=> flat 14, 2 .. 13),
            ('🃁' .. '🃎' Z=> flat 14, 2 .. 13),
            ('🃑' .. '🃞' Z=> flat 14, 2 .. 13)
    ).map(&card).List;

    # -----------------------------------
    # beginning of attributes and methods
    # -----------------------------------

    # create private attribute $!bag
    # It is a SetHash created from @Base-Deck
    # we use its .grab(|) method as .deal(|)
    has SetHash $!bag handles ('deal' => 'grab') .= new(@Base-Deck);

    # note that you should not reshuffle if you are currently holding any cards
    method reshuffle () {
        $!bag .= new(@Base-Deck);
    }

    method maybe-reshuffle (UInt $to-take = 0) {
        # If there isn't enough cards, it always reshuffles
        # otherwise there is a 1% chance that it will reshuffle
        if $to-take > +$!bag || rand < 0.1e0 {
            put 'Shuffling the cards';
            self.reshuffle;
        }
    }
}



sub MAIN () {
    my Deck $deck .= new;

    USAGE();
    print "\n\n";

    Restart:
    repeat {
        my Int $*pot = 100;

        Main-Loop:
        loop {
            # make sure that there is money left to gamble
            last if $*pot <= 0;
            # shuffle if there isn't enough cards in the deck
            # or randomly otherwise
            NEXT $deck.maybe-reshuffle(3);


            # sort numerically
            my ($a,$b) = $deck.deal(2).sort(+*);
            put "$a $b  $Card-Back";


            my Order $*more-or-less;
            my UInt $bet;


            # Check the dealt cards
            if $a == $b { # numerically equal
                $*more-or-less = do given $a {
                    when $a ==  2 { More }
                    when $a == 14 { Less }
                    default { more-or-less }
                }
            } elsif $a + 1 == $b { # consecutive cards
                put "There isn't a chance of winning, redealing.";
                redo
            }


            $bet = make-a-bet;
            if $bet == 0 {
                say 'Coward';
                redo;
            }

            # deal a new card
            my $c = $deck.deal;
            put "$a $b  $c";

            # check the new card
            {
                when $a < $c < $b {
                    put 'Win. ';
                    $*pot += $bet
                }
                when $c == $a | $b {
                    put 'Lose Double. ';
                    $*pot -= $bet * 2
                }
                default {
                    put 'Lose. ';
                    $*pot -= $bet
                }
            }

            last if $*pot <= 0;
            put "You have $*pot";
        }

    } while play-again;
}

sub more-or-less (--> Order) {
    loop {
        my $answer = prompt 'Is the next card going to be MORE or LESS than that? ';
        given fc $answer {
            when 'less' | '<' | 'before' { return Order::Less }
            when 'more' | '>' | 'after'  { return Order::More }
        }
    }
}

sub make-a-bet () {
    put "You currently have $*pot";
    my $message = do given $*more-or-less {
        when Less { 'How much money are you willing to bet that it is less? ' }
        when More { 'How much money are you willing to bet that it is more? ' }
        default { 'How much money are you willing to bet? ' }
    };

    loop {
        # redo the loop if there is an error
        CATCH { default {} }

        my $answer = prompt $message;
        if $answer eq '*' {
            return $*pot
        } elsif $answer > $*pot {
            put "You only have $*pot";
        } else {
            return $answer;
        }
    }
}

sub play-again (--> Bool) {
    my $answer = prompt "\nYou ran out of money.\nDo you want to play again? ";
    so $answer.fc eq any < y yes yeah sure ok >
}