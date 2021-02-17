#!/usr/bin/env raku
use v6.d;

=begin comment
This is intended to be a close translation of the original.

While Raku is "supposed" to have a `goto`, no one has ever bothered to implemented it.
So we use `repeat {...} while False;` and `redo` to emulate them.
Could have instead used `loop {...}` and `last` instead,
but that wouldn't line up with the `goto` statements of the original.

I also took the liberty of vastly simplifying the dealing of the cards
in lines 260..630 and 730..890
I might change them to be more similar to the original later
=end comment

## the following lines replace lines 10..80
put qq:to<END>;
    { ' ' x 26 }ACEY DUCEY CARD GAME
    { ' ' x 15 }CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY


    ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER
    THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP
    YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING
    ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE
    A VALUE BETWEEN THE FIRST TWO.
    IF YOU DO NOT WANT TO BET, INPUT A 0
    END

constant @CARDS = flat 2..10, <JACK QUEEN KING ACE>;

## used to sort the cards in the same order as they
## appear in @CARDS
constant %CARD-INDEX = @CARDS[]:p .invert;
## @CARDS[]:p   eqv   (0 => 2, 1 => 3 ... 8 => 10, 9 => "Jack", 10 => "Queen", 11 => "King", 12 => "Ace")
## .invert() inverts the key and value
# %CARD-INDEX = (
#   2     =>  0,
#   ...
#   10    =>  8,
#   Jack  =>  9,
#   Queen => 10,
#   King  => 11,
#   Ace   => 12,
# )

START: repeat {
    # my $N = 100; # line 100
    my $Q = 100; # line 110

    my ($A,$B);
    my $M;
    MAIN-LOOP: repeat {
        put "YOU NOW HAVE $Q DOLLARS."; # line 120
        put(); # line 130

        DEAL-HAND: repeat {
            ## The following 2 lines replace lines 260..630
            # the two cards can't match, so use pick() instead of roll()
            # we also want them sorted in the same order as they are in @CARDS
            my @hand = @CARDS.pick(2).sort({ %CARD-INDEX{$_} });
            ($A,$B) = @hand;
            put "HERE ARE YOUR NEXT TWO CARDS: ", @hand;

            put(); # line 640
            # put(); # line 650

            BET: repeat {
                $M = prompt "WHAT IS YOUR BET "; # line 660
                given $M {
                    when 0 { # line 670
                        put "CHICKEN!!"; # line 675
                        put(); # line 676
                        redo DEAL-HAND; # line 677
                    }
                    when $M <= $Q {} # line 680
                    when $M > $Q { # line 680 also
                        put "SORRY, MY FRIEND, BUT YOU BET TOO MUCH."; # line 690
                        put "YOU HAVE ONLY $Q DOLLARS TO BET."; # line 700
                        redo BET; # line 710
                    }

                    ## This is new.
                    ## It is used to handle errors like non-numeric bets
                    ## (It's also why line 680 is split among two lines above)
                    default {
                        put "PLEASE GIVE A NUMBER BETWEEN 0 AND $Q";
                        redo BET;
                    }
                }
            } while False; # BET:
        } while False; # DEAL-HAND:

        ## The following two lines replace lines 730..890
        my $C = @CARDS.pick;
        put $C;

        ## This replaces lines 910..970
        # note also that lines 210,220,240,250 were moved here
        my ($a,$b,$c) = %CARD-INDEX{$A,$B,$C};
        if $a < $c < $b { # lines 930,970
            put "YOU WIN!!!"; # line 950
            $Q += $M; # line 210
            redo MAIN-LOOP; # line 220
        }
        put "SORRY, YOU LOSE"; # line 970

        # still have money left?
        if $M < $Q { # line 980
            $Q -= $M; # line 240
            redo MAIN-LOOP; # line 250
        }
    } while False; # MAIN-LOOP:

    # out of money
    put(); # line 990
    put(); # line 1000
    put "SORRY, FRIEND, BUT YOU BLEW YOUR WAD."; # line 1010
    put(); put(); # line 1015

    my $Again = prompt "TRY AGAIN (YES OR NO)"; # line 1020
    put(); put(); # line 1025

    if $Again.uc eq "YES" { # line 1030
        redo START; # line 1030
    }

    put "O.K., HOPE YOU HAD FUN!"; #line 1040
    # exit; # line 1050
} while False; # START:
