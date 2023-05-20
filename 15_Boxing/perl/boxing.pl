#!/usr/bin/perl

# Boxing program in Perl
#   Required extensive restructuring to remove all of the GOTO's.
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

# globals
my $Opp_won = 0;    # num rounds opponent has won
my $You_won = 0;    # num rounds you have won
my $Opp_name = "";  # opponent name
my $Your_name = ""; # your name
my $Your_best = 0;  # your best punch
my $Your_worst = 0; # your worst punch
my $Opp_best;       # opponent best punch
my $Opp_worst;      # opponent worst punch
my $Opp_damage;     # opponent damage ?
my $Your_damage;    # your damage ?

sub get_punch
{
    my $prompt = shift;
    my $p;
    while (1)
    {
        print "$prompt: ";
        chomp($p = int(<>));
        last if ($p >= 1 && $p <= 4);
        print "DIFFERENT PUNCHES ARE: (1) FULL SWING; (2) HOOK; (3) UPPERCUT; (4) JAB.\n";
    }
    return $p;
}

print "\n";
print " " x 33, "BOXING\n";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";

print "BOXING OLYMPIC STYLE (3 ROUNDS -- 2 OUT OF 3 WINS)\n\n";
print "WHAT IS YOUR OPPONENT'S NAME: ";
chomp($Opp_name = <>);
print "INPUT YOUR MAN'S NAME: ";
chomp($Your_name = <>);
print "DIFFERENT PUNCHES ARE: (1) FULL SWING; (2) HOOK; (3) UPPERCUT; (4) JAB.\n";
$Your_best = get_punch("WHAT IS YOUR MANS BEST");
$Your_worst = get_punch("WHAT IS HIS VULNERABILITY");

do {
    $Opp_best = int(4*rand(1)+1);
    $Opp_worst = int(4*rand(1)+1);
} while ($Opp_best == $Opp_worst);
print "$Opp_name\'S ADVANTAGE IS $Opp_best AND VULNERABILITY IS SECRET.\n\n";

for my $R (1 .. 3) # rounds
{
    last if ($Opp_won >= 2 || $You_won >= 2);
    $Opp_damage = 0;
    $Your_damage = 0;
    print "ROUND $R BEGINS...\n";
    for my $R1 (1 .. 7) # 7 events per round?
    {
        if (int(10*rand(1)+1) <= 5)
        {
            my $your_punch = get_punch("$Your_name\'S PUNCH");
            $Opp_damage += 2 if ($your_punch == $Your_best);

            if    ($your_punch == 1) { punch1(); }
            elsif ($your_punch == 2) { punch2(); }
            elsif ($your_punch == 3) { punch3(); }
            else                     { punch4(); }
            next;
        }

        my $Opp_punch = int(4*rand(1)+1);
        $Your_damage += 2 if ($Opp_punch  == $Opp_best);
            
        if    ($Opp_punch == 1) { opp1(); }
        elsif ($Opp_punch == 2) { opp2(); }
        elsif ($Opp_punch == 3) { opp3(); }
        else                    { opp4(); }
    }

    if ($Opp_damage > $Your_damage)
    {
        print "\n$Your_name WINS ROUND $R\n\n";
        $You_won++;
    }
    else
    {
        print "\n$Opp_name WINS ROUND $R\n\n";
        $Opp_won++;
    }
}

if ($Opp_won >= 2)
{
    done("$Opp_name WINS (NICE GOING, $Opp_name).");
}

#else # if ($You_won >= 2)
done("$Your_name AMAZINGLY WINS!!");

###################################################

sub done
{
    my $msg = shift;
    print $msg;
    print "\n\nAND NOW GOODBYE FROM THE OLYMPIC ARENA.\n\n";
    exit(0);
}

sub punch1
{
    # $your_punch == 1, full swing
    print "$Your_name SWINGS AND ";
    if ($Opp_worst == 4 || int(30*rand(1)+1) < 10)
    {
        print "HE CONNECTS!\n";
        if ($Opp_damage > 35)
        {
            done("$Opp_name IS KNOCKED COLD AND $Your_name IS THE WINNER AND CHAMP! ");
        }
        $Opp_damage += 15;
    }
    else
    {
        print "HE MISSES\n";
        print "\n\n" if ($Opp_damage != 1);
    }
}

sub punch2
{
    # $your_punch == 2, hook
    print "$Your_name GIVES THE HOOK... ";
    if ($Opp_worst == 2)
    {
        $Opp_damage += 7;
        return;
    }
    if (int(2*rand(1)+1) == 1)
    {
        print "BUT IT'S BLOCKED!!!!!!!!!!!!!\n";
    }
    else
    {
        print "CONNECTS...\n";
        $Opp_damage += 7;
    }
}

sub punch3
{
    # $your_punch == 3, uppercut
    print "$Your_name TRIES AN UPPERCUT ";
    if ($Opp_worst == 3 || int(100*rand(1)+1) < 51)
    {
        print "AND HE CONNECTS!\n";
        $Opp_damage += 4;
    }
    else
    {
        print "AND IT'S BLOCKED (LUCKY BLOCK!)\n";
    }
}

sub punch4
{
    # $your_punch == 4, jab
    print "$Your_name JABS AT $Opp_name\'S HEAD ";
    if ($Opp_worst == 4 || (int(8*rand(1)+1)) >= 4)
    {
        $Opp_damage += 3;
        print "\n";
    }
    else
    {
        print "IT'S BLOCKED.\n";
    }
}

sub opp1
{
    # opp_punch == 1
    print "$Opp_name TAKES A FULL SWING AND ";
    if ($Your_worst == 1 || int(60*rand(1)+1) < 30)
    {
        print " POW!!!!! HE HITS HIM RIGHT IN THE FACE!\n";
        if ($Your_damage > 35)
        {
            done("$Your_name IS KNOCKED COLD AND $Opp_name IS THE WINNER AND CHAMP!");
        }
        $Your_damage += 15;
    }
    else
    {
        print " IT'S BLOCKED!\n";
    }
}

sub opp2
{
    # opp_punch == 2
    print "$Opp_name GETS $Your_name IN THE JAW (OUCH!)\n";
    $Your_damage += 7;
    print "....AND AGAIN!\n";
    $Your_damage += 5;
    if ($Your_damage > 35)
    {
        done("$Your_name IS KNOCKED COLD AND $Opp_name IS THE WINNER AND CHAMP!");
    }
    print "\n";
    # 2 continues into opp_punch == 3
    opp3();
}

sub opp3()
{
    # opp_punch == 3
    print "$Your_name IS ATTACKED BY AN UPPERCUT (OH,OH)...\n";
    if ($Your_worst != 3 && int(200*rand(1)+1) > 75)
    {
        print " BLOCKS AND HITS $Opp_name WITH A HOOK.\n";
        $Opp_damage += 5;
    }
    else
    {
        print "AND $Opp_name CONNECTS...\n";
        $Your_damage += 8;
    }
}

sub opp4
{
    # opp_punch == 4
    print "$Opp_name JABS AND ";
    if ($Your_worst == 4)
    {
        $Your_damage += 5;
    }
    elsif (int(7*rand(1)+1) > 4)
    {
        print " BLOOD SPILLS !!!\n";
        $Your_damage += 5;
    }
    else
    {
        print "IT'S BLOCKED!\n";
    }
}
