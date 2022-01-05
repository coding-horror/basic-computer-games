#!/usr/bin/perl

use strict;
use warnings;


# global variables defined here

my(@WORDS) = qw(
    GUM SIN FOR CRY LUG BYE FLY
    UGLY EACH FROM WORK TALK WITH SELF
    PIZZA THING FEIGN FIEND ELBOW FAULT DIRTY
    BUDGET SPIRIT QUAINT MAIDEN ESCORT PICKAX
    EXAMPLE TENSION QUININE KIDNEY REPLICA SLEEPER
    TRIANGLE KANGAROO MAHOGANY SERGEANT SEQUENCE
    MOUSTACHE DANGEROUS SCIENTIST DIFFERENT QUIESCENT
    MAGISTRATE ERRONEOUSLY LOUDSPEAKER PHYTOTOXIC
    MATRIMONIAL PARASYMPATHOMIMETIC THIGMOTROPISM
);
my(@PIC,$board,@guessedLetters,$guessCount,$hangCount);
my(%GUESSED);

# Subroutines defined here.

# init_variables: initialize all of the variables needed
# (this covers lines 50-90 in the original BASIC program)

sub init_variables {
    @guessedLetters = ();
    @PIC = (
        'XXXXXXX     ',
        'X     X     ',
        'X           ',
        'X           ',
        'X           ',
        'X           ',
        'X           ',
        'X           ',
        'X           ',
        'X           ',
        'X           ',
        'X           ',
    );
    $guessCount = 0; %GUESSED = ();
    $hangCount = 0;
}

# addchar: given a row & column, put the specified char in that place in @PIC
sub addchar {
    my($row,$col, $c) = @_;

    substr($PIC[$row],$col,1) = $c;
}


# main code starts here

print ' 'x31; print "Hangman\n";
print ' 'x14; print "Creative Computing  Morristown, New Jersey\n\n\n\n";

# an iteration of the PLAY block is one complete game.
# There is a continue block that will ask if the user
# wants to play another game.

PLAY: while (1) {

    init_variables();
    # Any words left?
    if (@WORDS == 0) {
        print "You did all the words!\n";
        last PLAY;
    }
    # splice a random word out of the @WORDS array
    my($thisWord) = splice(@WORDS, int(rand(scalar @WORDS)),1);
    # $board is the "game board" of the filled-out word
    # that the user is working on
    $board = '.'x(length $thisWord);

    # GUESS loop is run for every time the user guesses a letter
    GUESS: while(1) {
        print "Here are the letters you used:\n";
        printf("%s\n", join(',',@guessedLetters));
        printf("\n\n%s\n", $board);

        print "What is your guess for a letter ? ";
        chomp(my $guess = <STDIN>);
        # The %GUESSED hash allows us to quickly identify
        # letters that have already been guessed
        if ($GUESSED{lc $guess}) {
            print "You guessed that letter before!\n\n";
            redo GUESS;
        }

        # save the guessed letter
        push @guessedLetters, $guess;
        $GUESSED{lc $guess} = 1;
        ++$guessCount;

        # now look for the letter in the $thisWord var
        # and put it into the $board var wherever it
        # shows up. $foundLetter is a flag that indicates
        # whether or not the letter is found.
        my $foundLetter = 0;
        for (my $i = 0; $i < length $thisWord; ++$i) {
            if (lc substr($thisWord,$i,1) eq lc $guess) {
                $foundLetter = 1;
                substr($board, $i, 1) = substr($thisWord, $i, 1);
            }
        }

        # The user found a letter in the solution!
        if ($foundLetter) {

            # Are there any '.' chars left in the board?
            if (index($board, '.') < 0) {
                print "You found the word!\n\n";
            } else {
                printf("%s\n\n", $board);
                print "What is your guess for the word ? ";
                chomp(my $guessword = <STDIN>);
                if (lc $thisWord ne lc $guessword) {
                    print "Wrong.  Try another letter.\n";
                    # Go to the next iteration of the GUESS loop
                    next GUESS;
                }
                printf("Right! It took you %d %s!\n", $guessCount, ($guessCount == 1 ? 'guess' : 'guesses'));
            }
            # At this point the user has discovered the word and won.
            # This "next" statement takes execution down to the
            # continue block for the PLAY loop;
            next PLAY;

        } else {  # didn't find a letter

            ++$hangCount;
            print "\n\n\nSorry, that letter isn't in the word.\n";

            # The addchar() calls in the block below piece together the
            # hangman graphic, depending on how many wrong letters
            # the user has.
            if ($hangCount == 1) {
                print "First, we draw a head\n";
                addchar(2,5,"-");addchar(2,6,"-");addchar(2,7,"-");
                addchar(3,4,"("); addchar(3,5,"."); addchar(3,7,"."); addchar(3,8,")");
                addchar(4,5,"-");addchar(4,6,"-");addchar(4,7,"-");
            }
            if ($hangCount == 2) {
                print "Now we draw a body.\n";
                for (5 .. 8) {
                    addchar($_, 6, "X");
                }
            }
            if ($hangCount == 3) {
                print "Next we draw an arm.\n";
                for (3 .. 6) {
                    addchar($_, $_-1, "\\");
                }
            }
            if ($hangCount == 4) {
                print "This time it's the other arm.\n";
                addchar(3,10, "/");
                addchar(4, 9, "/");
                addchar(5, 8, "/");
                addchar(6, 7, "/");
            }
            if ($hangCount == 5) {
                print "Now, let's draw the right leg.\n";
                addchar( 9,5, "/");
                addchar(10,4, "/");
            }
            if ($hangCount == 6) {
                print "This time we draw the left leg.\n";
                addchar(9,7,"\\");
                addchar(10,8,"\\");
            }
            if ($hangCount == 7) {
                print "Now we put up a hand.\n";
                addchar(2,10,"\\"); 
            }
            if ($hangCount == 8) {
                print "Next the other hand.\n";
                addchar(2,2,"/");
            }
            if ($hangCount == 9) {
                print "Now we draw one foot\n";
                addchar(11,9,"\\");
                addchar(11,10, "-");
            }
            if ($hangCount == 10) {
                print "Here's the other foot -- you're hung!!\n";
                addchar(11,2,"-"); 
                addchar(11,3, "/");
            }

            printf("$_\n") for @PIC;
            print "\n\n";

            # Next guess if the user has not lost
            if ($hangCount < 10) {
                next GUESS;
            }

            printf("Sorry, you lose.  The word was %s\n", $thisWord);
            next PLAY;

        } # didn't find a letter block
    } # GUESS block
} # PLAY block

# This block is reached either by the player winning (see the "next PLAY")
# statement) or by the user losing (as the PLAY block is complete and 
# execution naturally comes to this continue block).
continue {
    print "Want another word ? ";
    chomp(my $in = <STDIN>);
    if ($in !~ m/^y/i) {
        # Exit the PLAY loop
        print "\nIt's been fun!  Bye for now.\n\n";
        last PLAY;
    }
    # At this point execution goes to the start of the PLAY block,
    # meaning a new game
}
