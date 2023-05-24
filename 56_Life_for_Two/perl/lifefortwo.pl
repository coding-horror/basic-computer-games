#!/usr/bin/perl

# Life_For_Two program in Perl
#   Required extensive restructuring to remove all of the GOTO's.
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

# try to load module to hide input, set RKey to true if found
my $Rkey = eval { require Term::ReadKey } // 0;
END { Term::ReadKey::ReadMode('normal') if ($Rkey); }

# globals
my @Board;      # 2D board
my @X;          # ?
my @Y;          # ?
my $Player;     # 1 or 2
my $M2 = 0;     # ?
my $M3 = 0;     # ?

# add 0 on front to make data 1 based
my @K = (0,3,102,103,120,130,121,112,111,12,21,30,1020,1030,1011,1021,1003,1002,1012);
my @A = (0,-1,0,1,0,0,-1,0,1,-1,-1,1,-1,-1,1,1,1);

print "\n";
print " " x 33, "LIFE2\n";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";
print " " x 10, "U.B. LIFE GAME\n";
for my $j (1 .. 5) { for my $k (1 .. 5) { $Board[$j][$k] = 0; } }

for (1 .. 2)
{
    $Player = $_; # if we make $Player the loop var, the global isn't set
    my $p1 = ($Player == 2) ? 30 : 3;
    print "\nPLAYER $Player - 3 LIVE PIECES.\n";
    for (1 .. 3)
    {
        get_input();
        $Board[$X[$Player]][$Y[$Player]] = $p1 if ($Player != 99);
    }
}
print_board(); # print board after initial input

while (1)
{
    print "\n";
    calc_board();  # calc new positions
    print_board(); # print current board after calc

    if ($M2 == 0 && $M3 == 0)
    {
        print "\nA DRAW\n";
        last;
    }
    if ($M3 == 0)
    {
        win(1);
        last;
    }
    if ($M2 == 0)
    {
        win(2);
        last;
    }

    for (1 .. 2)
    {
        $Player = $_; # if we make $Player the loop var, the global isn't set
        print "\n\nPLAYER $Player ";
        get_input();
        last if ($Player == 99);
    }
    next if ($Player == 99);

    $Board[$X[1]][$Y[1]] = 100;
    $Board[$X[2]][$Y[2]] = 1000;
}
exit(0);

###########################################################

sub win
{
    my $p = shift;
    print "\nPLAYER $p IS THE WINNER\n";
}

sub calc_board
{
    for my $j (1 .. 5)
    {
        for my $k (1 .. 5)
        {
            if ($Board[$j][$k] > 99)
            {
                $Player = $Board[$j][$k] > 999 ? 10 : 1;
                for (my $c = 1 ; $c <= 15 ; $c += 2)
                {
                    $Board[$j+$A[$c]][$k+$A[$c+1]] = ($Board[$j+$A[$c]][$k+$A[$c+1]] // 0) + $Player;
                }
            }
        }
    }
}

sub print_board
{
    $M2 = 0;
    $M3 = 0;
    for my $j (0 .. 6)
    {
        print "\n";
        for my $k (0 .. 6)
        {
            if ($j != 0 && $j != 6)
            {
                if ($k != 0 && $k != 6)
                {
                    print_row($j, $k);
                    next;
                }
                if ($j == 6)
                {
                    print "0\n";
                    return;
                }
                print " $j ";
            }
            else
            {
                if ($k == 6)
                {
                    print " 0 ";
                    last;
                }
                print " $k ";
            }
        }
    }
}

sub print_row
{
    my ($j, $k) = @_;

    if ($Board[$j][$k] >= 3)
    {
        my $c;
        for $c (1 .. 18)
        {
            if ($Board[$j][$k] == $K[$c])
            {
                if ($c <= 9)
                {
                    $Board[$j][$k] = 100;
                    $M2++;
                    print " * ";
                }
                else
                {
                    $Board[$j][$k] = 1000;
                    $M3++;
                    print " # ";
                }
                return;
            }
        }
    }
    $Board[$j][$k] = 0;
    print "   ";
}

sub get_input
{
    while (1)
    {
        print "X,Y\n";
        my $ans;

        if ($Rkey)
        {
            # code to hide input
            Term::ReadKey::ReadMode('noecho');
            $ans = Term::ReadKey::ReadLine(0);
            Term::ReadKey::ReadMode('restore');
            print "\n"; # do this since the one entered was hidden
        }
        else
        {
            # normal, input visible
            chomp($ans = <>);
        }

        ($Y[$Player], $X[$Player]) = split(/[,\s]+/, $ans, 2);
        if ($X[$Player] > 5 || $X[$Player] < 1 || $Y[$Player] > 5 || $Y[$Player] < 1)
        {
            print "ILLEGAL COORDS. RETYPE\n";
            next;
        }
        # this tells you the cell was already taken not zero it out, bug!
        #if ($Board[$X[$Player]][$Y[$Player]] != 0)
        #{
        #    print "ILLEGAL COORDS. RETYPE\n";
        #    next;
        #}
        last;
    }

    return if ($Player == 1 || $X[1] != $X[2] || $Y[1] != $Y[2]);

    print "SAME COORD.  SET TO 0\n";
    $Board[$X[$Player]+1][$Y[$Player]+1] = 0;
    $Player = 99;
}
