#!/usr/bin/perl

# Basketball program in Perl
#   While this should play the same way as the fairly faithful translation version,
#   there are no GOTOs in this code. That was achieved by restructuring the program
#   in the 2 *_play() subs. All of the percentages remain the same. If writing this
#   from scratch, we really should have only a play() sub which uses the same code
#   for both teams, but the percent edge to Darmouth has been maintained here.
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

# globals
my $Defense=0;      # dartmouth defense value
my $Opponent;       # name of opponent
my @Score = (0, 0); # scores, dart is [0], opponent is [1]
my $Player = 0;     # player, 0 = dart, 1 = opp
my $Timer = 0;      # time tick, 100 ticks per game, 50 is end of first half, if tie at end then back to T=93
my $DoPlay = 1;     # true if game is still being played
my $ConTeam;        # controlling team, "dart" or "opp"
my $ShotType = 0;   # current shot type


print "\n";
print " " x 31, "BASKETBALL";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY";
print "\n\n\n";

print "THIS IS DARTMOUTH COLLEGE BASKETBALL.  YOU WILL BE DARTMOUTH\n";
print "CAPTAIN AND PLAYMAKER.  CALL SHOTS AS FOLLOWS:\n";
print "   1. LONG (30 FT.) JUMP SHOT;\n";
print "   2. SHORT (15 FT.) JUMP SHOT;\n";
print "   3. LAY UP;\n";
print "   4. SET SHOT.\n";
print "BOTH TEAMS WILL USE THE SAME DEFENSE.  CALL DEFENSE AS FOLLOWS:\n";
print "   6. PRESS;\n";
print "   6.5 MAN-TO MAN;\n";
print "   7. ZONE;\n";
print "   7.5 NONE.\n";
print "TO CHANGE DEFENSE, JUST TYPE 0 AS YOUR NEXT SHOT.\n\n";
get_defense();
print "\n";
print "CHOOSE YOUR OPPONENT: ";
chomp($Opponent = <>);

$ConTeam = center_jump();
while ($DoPlay)
{
    print "\n";
    if ($ConTeam eq "dart")
    {
        $Player = 0;
        get_your_shot();
        dartmouth_play();
    }
    else
    {
        $ShotType = 10.0 / 4 * rand(1) + 1;
        opponent_play();
    }
    if ($Timer >= 100)
    {
        check_end_game();
        last if (!$DoPlay);
        $Timer = 93;
        $ConTeam = center_jump();
    }
}
exit(0);

###############################################################

sub dartmouth_play
{
    $Player = 0;
    print "\n";
    while (1)
    {
        $Timer++;
        if ($Timer == 50) { end_first_half(); return; }
        if ($Timer == 92) { two_min_left(); }

        if ($ShotType == 0) 
        {
            get_defense();
            return; # for new ShotType
        }
        elsif ($ShotType == 1 || $ShotType == 2)
        {
            print "JUMP SHOT\n";
            if (rand(1) <= 0.341 * $Defense / 8)
            {
                print "SHOT IS GOOD.\n";
                dartmouth_score();
                last;
            }

            if (rand(1) <= 0.682*$Defense/8)
            {
                print "SHOT IS OFF TARGET.\n";
                if ($Defense/6*rand(1) > 0.45)
                {
                    print "REBOUND TO $Opponent\n";
                    last;
                }

                print "DARTMOUTH CONTROLS THE REBOUND.\n";
                if (rand(1) <= 0.4)
                {
                    $ShotType = (rand(1) <= 0.5) ? 3 : 4;
                    next;
                }
                if ($Defense == 6)
                {
                    if (rand(1) <= 0.6)
                    {
                        print "PASS STOLEN BY $Opponent, EASY LAYUP.\n";
                        opp_score();
                        next;
                    }
                }
                print "BALL PASSED BACK TO YOU.\n";
                next;
            }

            if (rand(1) <= 0.782*$Defense/8)
            {
                print "SHOT IS BLOCKED.  BALL CONTROLLED BY "; # no NL
                if (rand(1) > 0.5)
                {
                    print "$Opponent.\n";
                    last;
                }
                else
                {
                    print "DARTMOUTH.\n";
                    next;
                }
            }

            if (rand(1) > 0.843*$Defense/8)
            {
                print "CHARGING FOUL.  DARTMOUTH LOSES BALL.\n";
                last;
            }
            else
            {
                print "SHOOTER IS FOULED.  TWO SHOTS.\n";
                foul_shooting();
                last;
            }
        }
        else # elsif ($ShotType >= 3)
        {
            print '', ($ShotType > 3 ? "SET SHOT." : "LAY UP."), "\n";
            if (7 / $Defense * rand(1) <= 0.4)
            {
                print "SHOT IS GOOD.  TWO POINTS.\n";
                dartmouth_score();
                last;
            }

            if (7 / $Defense * rand(1) <= 0.7)
            {
                print "SHOT IS OFF THE RIM.\n";
                if (rand(1) <= 0.667)
                {
                    print "$Opponent CONTROLS THE REBOUND.\n";
                    last;
                }

                print "DARTMOUTH CONTROLS THE REBOUND.\n";
                next if (rand(1) <= 0.4);

                print "BALL PASSED BACK TO YOU.\n";
                next;
            }

            if (7 / $Defense * rand(1) <= 0.875)
            {
                print "SHOOTER FOULED.  TWO SHOTS.\n";
                foul_shooting();
                last;
            }

            if (7 / $Defense * rand(1) <= 0.925)
            {
                print "SHOT BLOCKED. $Opponent\'S BALL.\n";
                last;
            }

            print "CHARGING FOUL.  DARTMOUTH LOSES THE BALL.\n";
            last;
        }
    }
    $ConTeam = "opp";
}

sub get_defense
{
    $Defense = 0;
    do {
        print "YOUR NEW DEFENSIVE ALLIGNMENT IS (6, 6.5, 7. 7.5): ";
        chomp($Defense = <>);
        ($Defense) =~ m/(\d(\.\d)?)/;
    } while ($Defense < 6.0 || $Defense > 7.5)
}

sub opponent_play
{
    $Player = 1;
    print "\n";
    while (1)
    {
        $Timer++;
        if ($Timer == 50) { end_first_half(); return; }
        if ($Timer == 92) { two_min_left(); }

        if ($ShotType <= 2.0)
        {
            print "JUMP SHOT.\n";
            if (8.0 / $Defense * rand(1) <= 0.35)
            {
                print "SHOT IS GOOD.\n";
                opp_score();
                last;
            }

            if (8.0 / $Defense * rand(1) <=  0.75)
            {
                print "SHOT IS OFF RIM.\n";
                opp_missed();
                return; # for possible new ShotType or team change
            }

            if (8.0 / $Defense * rand(1) <= 0.9)
            {
                print "PLAYER FOULED.  TWO SHOTS.\n";
                foul_shooting();
                last;
            }
            print "OFFENSIVE FOUL.  DARTMOUTH'S BALL.\n";
            last;
        }
        else # ShotType >= 3
        {
            print ($ShotType > 3 ? "SET SHOT.\n" : "LAY UP.\n");
            if (7.0 / $Defense * rand(1) > 0.413)
            {
                print "SHOT IS MISSED.\n";
                {
                    opp_missed();
                    return; # for possible new ShotType or team change
                }
            }
            else
            {
                print "SHOT IS GOOD.\n";
                opp_score();
                last;
            }
        }
    }
    $ConTeam = "dart";
}

sub opp_missed
{
    if ($Defense / 6.0 * rand(1) <= 0.5)
    {
        print "DARTMOUTH CONTROLS THE REBOUND.\n";
        $ConTeam = "dart";
    }
    else
    {
        print "$Opponent CONTROLS THE REBOUND.\n";
        if ($Defense == 6)
        {
            if (rand(1) <= 0.75)
            {
                print "BALL STOLEN.  EASY LAY UP FOR DARTMOUTH.\n";
                dartmouth_score();
                #$ConTeam = "opp";
                return; # for possible new ShotType
            }
        }
        if (rand(1) <= 0.5)
        {
            print "PASS BACK TO $Opponent GUARD.\n";
            #$ConTeam = "opp";
            return; # for possible new ShotType
        }
        $ShotType = (rand(1) <= 0.5) ? 3 : 4;
    }
}

sub opp_score
{
    $Score[0] += 2;
    print_score();
}

sub dartmouth_score
{
    $Score[1] += 2;
    print_score();
}

sub print_score
{
    print "SCORE: $Score[1] TO $Score[0]\n";
    print "TIME: $Timer\n";
}

sub end_first_half
{
    print "\n   ***** END OF FIRST HALF *****\n\n";
    print "SCORE: DARTMOUTH: $Score[1]   $Opponent: $Score[0]\n\n\n";
    $ConTeam = center_jump();
}

sub get_your_shot
{
    $ShotType = -1;
    while ($ShotType < 0 || $ShotType > 4)
    {
        print "YOUR SHOT (0-4): ";
        chomp($ShotType = <>);
        $ShotType = int($ShotType);
        if ($ShotType < 0 || $ShotType > 4)
        {
            print "INCORRECT ANSWER.  RETYPE IT. ";
        }
    }
}

sub center_jump
{
    print "CENTER JUMP\n";
    if (rand(1) <= 0.6)
    {
        print "$Opponent CONTROLS THE TAP.\n";
        return "opp";
    }
    print "DARTMOUTH CONTROLS THE TAP.\n";
    return "dart";
}

sub check_end_game
{
    print "\n";
    if ($Score[1] != $Score[0])
    {
        print "   ***** END OF GAME *****\n";
        print "FINAL SCORE: DARTMOUTH: $Score[1]    $Opponent: $Score[0]\n\n";
        $DoPlay = 0;
    }
    else
    {
        print "\n   ***** END OF SECOND HALF *****\n";
        print "SCORE AT END OF REGULATION TIME:\n";
        print "        DARTMOUTH: $Score[1]    $Opponent: $Score[0]\n\n";
        print "BEGIN TWO MINUTE OVERTIME PERIOD\n";
    }
}

sub two_min_left
{
    print "\n   *** TWO MINUTES LEFT IN THE GAME ***\n\n";
}

sub foul_shooting
{
    if (rand(1) > 0.49)
    {
        if (rand(1) > 0.75)
        {
            print "BOTH SHOTS MISSED.\n";
        }
        else
        {
            print "SHOOTER MAKES ONE SHOT AND MISSES ONE.\n";
            $Score[1 - $Player]++;
        }
    }
    else
    {
        print "SHOOTER MAKES BOTH SHOTS.\n";
        $Score[1 - $Player] += 2;
    }

    print_score();
}
