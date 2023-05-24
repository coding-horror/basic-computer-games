#!/usr/bin/perl

# Basketball program in Perl
#   This is fairly faithful translation from the original Basic.
#   This becomes apparent because there are actually 3 GOTOs still present
#   because of the way the original code jumped into the "middle of logic"
#   that has no obivious way to avoid ... that I can see.
#   For better structure and no GOTOs, see the other version of this program.
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
    if ($ShotType == 1 || $ShotType == 2)
    {
        $Timer++;
        if ($Timer == 50)
        {
            end_first_half();
            return;
        }
        if ($Timer == 92)
        {
            two_min_left();
        }

        print "JUMP SHOT\n";
        if (rand(1) <= 0.341 * $Defense / 8)
        {
            print "SHOT IS GOOD.\n";
            dartmouth_score();
            $ConTeam = "opp";
            return;
        }

        if (rand(1) <= 0.682*$Defense/8)
        {
            print "SHOT IS OFF TARGET.\n";
            if ($Defense/6*rand(1) > 0.45)
            {
                print "REBOUND TO ", $Opponent, "\n";
                $ConTeam = "opp";
                return;
            }

            print "DARTMOUTH CONTROLS THE REBOUND.\n";
            if (rand(1) <= 0.4) { goto L1300; }
            if ($Defense == 6)
            {
                if (rand(1) <= 0.6)
                {
                    print "PASS STOLEN BY $Opponent, EASY LAYUP.\n";
                    opp_score();
                    $ConTeam = "dart"; return;
                    return;
                }
            }
            print "BALL PASSED BACK TO YOU.\n";
            $ConTeam = "dart";
            return;
        }

        if (rand(1) <= 0.782*$Defense/8)
        {
            print "SHOT IS BLOCKED.  BALL CONTROLLED BY "; # no NL
            if (rand(1) > 0.5)
            {
                print "$Opponent.\n";
                $ConTeam = "opp";
                return;
            }
            else
            {
                print "DARTMOUTH.\n";
                $ConTeam = "dart";
                return;
            }
        }

        if (rand(1) > 0.843*$Defense/8)
        {
            print "CHARGING FOUL.  DARTMOUTH LOSES BALL.\n";
            $ConTeam = "opp";
            return;
        }
        else
        {
            print "SHOOTER IS FOULED.  TWO SHOTS.\n";
            foul_shooting();
            $ConTeam = "opp";
            return;
        }
    }

    L1300:
    while (1)
    {
        $Timer++;
        if ($Timer == 50)
        {
            end_first_half();
            return;
        }
        if ($Timer == 92) { two_min_left(); }
        if ($ShotType == 0) 
        {
            get_defense();
            return;
        }
        print '', ($ShotType > 3 ? "SET SHOT." : "LAY UP."), "\n";
        if (7 / $Defense * rand(1) <= 0.4)
        {
            print "SHOT IS GOOD.  TWO POINTS.\n";
            dartmouth_score();
            $ConTeam = "opp";
            return;
        }

        if (7 / $Defense * rand(1) <= 0.7)
        {
            print "SHOT IS OFF THE RIM.\n";
            if (rand(1) <= 0.667)
            {
                print "$Opponent CONTROLS THE REBOUND.\n";
                $ConTeam = "opp";
                return;
            }

            print "DARTMOUTH CONTROLS THE REBOUND.\n";
            next if (rand(1) <= 0.4);

            print "BALL PASSED BACK TO YOU.\n";
            $ConTeam = "dart";
            return;
        }

        if (7 / $Defense * rand(1) <= 0.875)
        {
            print "SHOOTER FOULED.  TWO SHOTS.\n";
            foul_shooting();
            $ConTeam = "opp";
            return;
        }

        if (7 / $Defense * rand(1) <= 0.925)
        {
            print "SHOT BLOCKED. $Opponent\'S BALL.\n";
            $ConTeam = "opp";
            return;
        }

        print "CHARGING FOUL.  DARTMOUTH LOSES THE BALL.\n";
        $ConTeam = "opp";
        return;
    }
}

sub get_defense
{
    $Defense = 0;
    while ($Defense < 6 || $Defense > 7.5)
    {
        print "YOUR NEW DEFENSIVE ALLIGNMENT IS (6, 6.5, 7. 7.5): ";
        chomp($Defense = <>);
        ($Defense) =~ m/(\d(\.\d)?)/;
    }
}

sub opponent_play
{
    $Player = 1;
    $Timer++;
    if ($Timer == 50)
    {
        end_first_half();
        $ConTeam = center_jump();
        return;
    }

    print "\n";
    while (1)
    {
        my $shot = 10.0 / 4 * rand(1) + 1;
        if ($shot <= 2.0)
        {
            print "JUMP SHOT.\n";
            if (8.0 / $Defense * rand(1) <= 0.35)
            {
                print "SHOT IS GOOD.\n";
                opp_score();
                $ConTeam = "dart";
                return;
            }

            if (8.0 / $Defense * rand(1) <=  0.75)
            {
                print "SHOT IS OFF RIM.\n";

                L3110:
                if ($Defense / 6.0 * rand(1) <= 0.5)
                {
                    print "DARTMOUTH CONTROLS THE REBOUND.\n";
                    $ConTeam = "dart";
                    return;
                }
                print "$Opponent CONTROLS THE REBOUND.\n";
                if ($Defense == 6)
                {
                    if (rand(1) <= 0.75)
                    {
                        print "BALL STOLEN.  EASY LAY UP FOR DARTMOUTH.\n";
                        dartmouth_score();
                        $ConTeam = "opp";
                        return;
                    }
                }
                if (rand(1) <= 0.5)
                {
                    print "PASS BACK TO $Opponent GUARD.\n";
                    $ConTeam = "opp";
                    return;
                }
                goto L3500;
            }

            if (8.0 / $Defense * rand(1) <= 0.9)
            {
                print "PLAYER FOULED.  TWO SHOTS.\n";
                foul_shooting();
                $ConTeam = "dart";
                return;
            }
            print "OFFENSIVE FOUL.  DARTMOUTH'S BALL.\n";
            $ConTeam = "dart";
            return;
        }

        L3500:
        print ($shot > 3 ? "SET SHOT.\n" : "LAY UP.\n");
        if (7.0 / $Defense * rand(1) > 0.413)
        {
            print "SHOT IS MISSED.\n";
            {
            no warnings;
            goto L3110;
            }
        }
        else
        {
            print "SHOT IS GOOD.\n";
            opp_score();
            $ConTeam = "dart";
            return;
        }
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
    center_jump();
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
