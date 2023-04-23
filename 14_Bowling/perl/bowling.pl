#!/usr/bin/perl

# Bowling program in Perl
#   Run normally, this is a fairly faithful translation of the Basic game.
#   The only real differences are a few trivial fix-ups on the prints to make it
#   look better, and the player/frame/ball line was put before the "get the ball
#   going" line to make it more obvious who's turn it is.
#
#   However, if you run it with "-a" on the command line, it will go into
#   'advanced' mode, which means that "." is used to show pin down and "!" for
#   pin up, current running scores are shown at the end of each frame, and the
#   scoring also looks more normal at the end. This is all done because I think it
#   looks better and I wanted to see a score. Having a flag says you can play
#   whichever version of the game you like.
#
#   Note, the original code doesn't do the 10th frame correctly, in that it will
#   never do more than 2 balls, so the best score you can get is a 290.
#   This is true in both modes. That being said, it will always give you a mediocre game.
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

print "\n";
print " " x 34, "BASKETBALL\n";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";

# globals
my @C;              # pin position matraix?
my @Scores;         # scores: [player num][frame][ball] # ball3 is end-result, ball4 is frame score (advanced mode)
my $Answer;         # get answers
my $Num_players;    # number of players
my $Advanced = 0;   # flag, 1 to use advanced code, 0 for original code
my $char_down = '0'; # char to show pin is down
my $char_up = '+';   # char to show pin is standing

if ($ARGV[0] && $ARGV[0] eq "-a")
{
    shift;
    $Advanced = 1;
    $char_down = '.';
    $char_up = '!';
}

print "WELCOME TO THE ALLEY\n";
print "BRING YOUR FRIENDS\n";
print "OKAY LET'S FIRST GET ACQUAINTED\n\n";
print "SEE THE INSTRUCTIONS (Y/N): ";
chomp($Answer = uc(<>));
if ($Answer eq "Y")
{
    print "THE GAME OF BOWLING TAKES MIND AND SKILL. DURING THE GAME\n";
    print "THE COMPUTER WILL KEEP SCORE.YOU MAY COMPETE WITH\n";
    print "OTHER PLAYERS[UP TO FOUR]. YOU WILL BE PLAYING TEN FRAMES\n";
    print "ON THE PIN DIAGRAM 'O' MEANS THE PIN IS DOWN...'+' MEANS THE\n";
    print "PIN IS STANDING. AFTER THE GAME THE COMPUTER WILL SHOW YOUR SCORES.\n";
}

do {
    print "FIRST OF ALL...HOW MANY ARE PLAYING (1-4): ";
    $Num_players = int(<>);
} while ($Num_players < 1 || $Num_players > 4);

print "\nVERY GOOD...\n";

while (1)
{
    # reset all scores
    for my $p (1 .. $Num_players) # players
    {
        for my $f (1 .. 10) # frames
        {
            for my $b (1 .. 3) # balls
            {
                $Scores[$p][$f][$b] = 0;
            }
        }
    }

    # play the game
    for my $frame (1 .. 10) # frame
    {
        for my $curr_player (1 .. $Num_players) # player
        {
            my $last_pins=0;    # pins down for last ball
            my $ball=1;         # ball number, 1 or 2
            my $end_result=0;   # result at end of turn: 3=strike, 2=spare, 1=pins-left
            for my $i (1 .. 15) { $C[$i] = 0 }

            while (1) # another ball
            {
                # ARK BALL GENERATOR USING MOD '15' SYSTEM
                my $K=0;
                my $curr_pins=0; # pins down for this ball
                for my $i (1 .. 20)
                {
                    my $x = int(rand(1) * 100);
                    my $j;
                    for ($j=1 ; $j <= 10 ; $j++)
                    {
                        last if ($x < 15 * $j);
                    }
                    $C[15 * $j - $x] = 1;
                }

                # ARK PIN DIAGRAM
                print "PLAYER: $curr_player  FRAME: $frame  BALL: $ball\n";
                print "PRESS ENTER TO GET THE BALL GOING.";
                $Answer = <>; # not used, just need an enter
                for my $i (0 .. 3)
                {
                    print "\n";
                    print " " x $i; # avoid the TAB(), just shift each row over for the triangle
                    for my $j (1 .. 4 - $i)
                    {
                        $K++;
                        print ($C[$K] == 1 ? " $char_down" : " $char_up");
                    }
                }
                print "\n";

                # ARK ROLL ANALYSIS
                for my $i (1 .. 10)
                {
                    $curr_pins += $C[$i];
                }
                if ($curr_pins - $last_pins == 0)
                {
                    print "GUTTER!!\n";
                }
                if ($ball == 1 && $curr_pins == 10)
                {
                    print "STRIKE!!!!!\a\a\a\a\n"; # \a is for bell
                    $end_result = 3;
                }
                elsif ($ball == 2 && $curr_pins == 10)
                {
                    print "SPARE!!!!\n";
                    $end_result = 2;
                }
                elsif ($ball == 2 && $curr_pins < 10)
                {
                    if ($Advanced) { print 10 - $curr_pins, " PENS LEFT!!!\n"; }
                    else           { print "ERROR!!!\n"; }
                    $end_result = 1;
                }
                if ($ball == 1 && $curr_pins < 10)
                {
                    print "ROLL YOUR 2ND BALL\n";
                }
                print "\n";

                # ARK STORAGE OF THE SCORES
                if ($Advanced) { $Scores[$curr_player][$frame][$ball] = $curr_pins - $last_pins; }
                else           { $Scores[$curr_player][$frame][$ball] = $curr_pins; }
                if ($ball == 1)
                {
                    $ball = 2;
                    $last_pins = $curr_pins;

                    if ($end_result == 3) # strike, no more rolls, goto last
                    {
                        $Scores[$curr_player][$frame][$ball] = $curr_pins;
                    }
                    else
                    {
                        $Scores[$curr_player][$frame][$ball] = $curr_pins - $last_pins;
                        next if ($end_result == 0); # next roll
                    }
                }
                last;
            }
            $Scores[$curr_player][$frame][3] = $end_result;
        } # next player
        if ($Advanced)
        {
            print "Scores:\n";
            for my $p (1 .. $Num_players)
            {
                my $total = calc_score($p);
                print "\tPlayer $p: $total\n";
            }
            print "\n";
        }
    } # next frame

    # end of game, show full scoreboard
    show_scoreboard();

    print "DO YOU WANT ANOTHER GAME (Y/N): ";
    chomp($Answer = uc(<>));
    print "\n";
    last if ($Answer ne "Y");
}
exit(0);

sub show_scoreboard
{
    print "FRAMES\n";
    for my $i (1 .. 10)
    {
        print " $i ";
    }
    print "\n";
    my @results = ( "-", ".", "/", "X" );
    for my $p (1 .. $Num_players)
    {
        print "Player $p\n" if ($Advanced);
        my $ball_max = ($Advanced ? 4 : 3);
        for my $b (1 .. $ball_max)
        {
            for my $f (1 .. 10)
            {
                if ($b != 3) { print sprintf("%2d ", $Scores[$p][$f][$b]); }
                else         { print sprintf("%2s ", $results[$Scores[$p][$f][$b]]); }
            }
            print "\n";
        }
        print "\n";
    }
}

sub calc_score
{
    my $player = shift;
    my $total = 0;
    for my $frame (1 .. 10)
    {
        my $score = 0;
        if ($frame == 10 || $Scores[$player][$frame][3] == 1) # pins
        {
            $score = $Scores[$player][$frame][1] + $Scores[$player][$frame][2];
        }
        elsif ($Scores[$player][$frame][3] == 2) # spare
        {
            $score = 10 + $Scores[$player][$frame+1][1];
        }
        elsif ($Scores[$player][$frame][3] == 3) # strike
        {
            $score = 10 + $Scores[$player][$frame+1][1];
            if ($Scores[$player][$frame+1][1] == 10)
            {
                $score += ($frame < 9 ? $Scores[$player][$frame+2][1] : $Scores[$player][$frame+1][2]);
            }
            else
            {
                $score += $Scores[$player][$frame+1][2];
            }
        }
        $Scores[$player][$frame][4] = $score;
        $total += $score;
    }
    return $total;
}
