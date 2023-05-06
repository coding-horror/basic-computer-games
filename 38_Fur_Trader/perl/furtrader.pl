#!/usr/bin/perl

# Fur Trader program in Perl
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

# globals
my @Pelts = (qw(0 MINK BEAVER ERMINE FOX ));
my $Num_pelts = 4;
my @Quantity; # how many of each fur
my $Money;
my $Max_pelts = 190;
my $Ermine_price;  # like we have @Pelts and @Quantity we could have @Prices
my $Beaver_price;  # then have 4 constants as index into the arrays to avoid
my $Fox_price;     # the magic numbers 1-4, or better have a array of objects (really a hash)
my $Mink_price;    # with the 3 keys (name, number, price), but well keep it
                   # with 4 vars like the basic program did

print "\n";
print " " x 31, "FUR TRADER\n";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n";

init();
while (1)
{
    my $ans = get_yesno();
    last if ($ans ne "YES");

    $Ermine_price = new_price(.15, 0.95);
    $Beaver_price = new_price(.25, 1.00);

    print "\nYOU HAVE \$$Money SAVINGS.\n";
    print "AND $Max_pelts FURS TO BEGIN THE EXPEDITION.\n";
    print "\nYOUR $Max_pelts FURS ARE DISTRIBUTED AMONG THE FOLLOWING\n";
    print "KINDS OF PELTS: MINK, BEAVER, ERMINE AND FOX.\n";
    reset_furs();

    my $total = 0;
    for my $j ( 1 .. $Num_pelts)
    {
        print "\nHOW MANY $Pelts[$j] PELTS DO YOU HAVE? ";
        chomp(my $ans = <>);
        $Quantity[$j] = int($ans);
        $total += $Quantity[$j];
    }
    if ($total > $Max_pelts)
    {
        print "\nYOU MAY NOT HAVE THAT MANY FURS.\n";
        print "DO NOT TRY TO CHEAT.  I CAN ADD.\n";
        print "YOU MUST START AGAIN.\n";
        init();
        next;
    }

    print "\nYOU MAY TRADE YOUR FURS AT FORT 1, FORT 2,\n";
    print "OR FORT 3.  FORT 1 IS FORT HOCHELAGA (MONTREAL)\n";
    print "AND IS UNDER THE PROTECTION OF THE FRENCH ARMY.\n";
    print "FORT 2 IS FORT STADACONA (QUEBEC) AND IS UNDER THE\n";
    print "PROTECTION OF THE FRENCH ARMY.  HOWEVER, YOU MUST\n";
    print "MAKE A PORTAGE AND CROSS THE LACHINE RAPIDS.\n";
    print "FORT 3 IS FORT NEW YORK AND IS UNDER DUTCH CONTROL.\n";
    print "YOU MUST CROSS THROUGH IROQUOIS LAND.\n";
    my $done = 0;
    while (!$done)
    {
        $ans = 0;
        while ($ans < 1 || $ans > 3)
        {
            no warnings; # in case user enters alpha chars, then int() will return 0 with no warnings
            print "ANSWER 1, 2, OR 3: ";
            $ans = int(<>);
        }
        # returns 0 if they want to go somewhere else;
        # or returns the old basic line number to show what to do
        if ($ans == 1)    { $done = fort1(); }
        elsif ($ans == 2) { $done = fort2(); }
        elsif ($ans == 3) { $done = fort3(); }
    }

    if ($done == 1410)
    {
        print "YOUR BEAVER SOLD FOR \$", $Beaver_price * $Quantity[2], "\t";
    }

    if ($done <= 1414)
    {
        print "YOUR FOX SOLD FOR \$", $Fox_price * $Quantity[4], "\n";
        print "YOUR ERMINE SOLD FOR \$", $Ermine_price * $Quantity[3], "\t";
        print "YOUR MINK SOLD FOR \$", $Mink_price * $Quantity[1], "\n";
    }

    # 1418 is always done
    $Money += $Mink_price * $Quantity[1] + $Beaver_price * $Quantity[2] + $Ermine_price * $Quantity[3] + $Fox_price * $Quantity[4];
    print "\nYOU NOW HAVE \$$Money INCLUDING YOUR PREVIOUS SAVINGS\n";
    print "\nDO YOU WANT TO TRADE FURS NEXT YEAR? ";
}
exit(0);

###############################################################

sub init
{
    print "YOU ARE THE LEADER OF A FRENCH FUR TRADING EXPEDITION IN\n";
    print "1776 LEAVING THE LAKE ONTARIO AREA TO SELL FURS AND GET\n";
    print "SUPPLIES FOR THE NEXT YEAR.  YOU HAVE A CHOICE OF THREE\n";
    print "FORTS AT WHICH YOU MAY TRADE.  THE COST OF SUPPLIES\n";
    print "AND THE AMOUNT YOU RECEIVE FOR YOUR FURS WILL DEPEND\n";
    print "ON THE FORT THAT YOU CHOOSE.\n";

    $Money = 600;
    print "DO YOU WISH TO TRADE FURS?\n";
}

sub new_price
{
    my ($base, $factor) = @_;
    return int(($base * rand(1) + $factor) * 100 + .5) / 100;
}

sub supplies_fs
{
    print "SUPPLIES AT FORT STADACONA COST \$125.00.\n";
    print "YOUR TRAVEL EXPENSES TO STADACONA WERE \$15.00.\n";
}

sub supplies_ny
{
    print "SUPPLIES AT NEW YORK COST \$80.00.\n";
    print "YOUR TRAVEL EXPENSES TO NEW YORK WERE \$25.00.\n";
}

sub reset_furs
{
    for my $j (1 .. $Num_pelts) { $Quantity[$j] = 0; }
}

sub get_yesno
{
    my $ans;
    print "ANSWER YES OR NO: ";
    chomp($ans = uc(<>));
    return $ans;
}

sub trade_elsewhere
{
    print "DO YOU WANT TO TRADE AT ANOTHER FORT? ";
    my $ans = get_yesno();
    return $ans;
}

sub fort1
{
    print "\nYOU HAVE CHOSEN THE EASIEST ROUTE.  HOWEVER, THE FORT\n";
    print "IS FAR FROM ANY SEAPORT.  THE VALUE\n";
    print "YOU RECEIVE FOR YOUR FURS WILL BE LOW AND THE COST\n";
    print "OF SUPPLIES HIGHER THAN AT FORTS STADACONA OR NEW YORK.\n";
    my $ans = trade_elsewhere();
    if ($ans eq "YES") { return 0; }
    
    $Money -= 160;
    $Mink_price = new_price(.2, .7 );
    $Ermine_price = new_price(.2, .65);
    $Beaver_price = new_price(.2, .75);
    $Fox_price = new_price(.2, .8 );
    print "\nSUPPLIES AT FORT HOCHELAGA COST \$150.00.\n";
    print "YOUR TRAVEL EXPENSES TO HOCHELAGA WERE \$10.00.\n";
    return 1410;
}

sub fort2
{
    print "\nYOU HAVE CHOSEN A HARD ROUTE.  IT IS, IN COMPARSION,\n";
    print "HARDER THAN THE ROUTE TO HOCHELAGA BUT EASIER THAN\n";
    print "THE ROUTE TO NEW YORK.  YOU WILL RECEIVE AN AVERAGE VALUE\n";
    print "FOR YOUR FURS AND THE COST OF YOUR SUPPLIES WILL BE AVERAGE.\n";
    my $ans = trade_elsewhere();
    if ($ans eq "YES") { return 0; }

    $Money -= 140;
    print "\n";
    $Mink_price = new_price(.3,  .85);
    $Ermine_price = new_price(.15, .8);
    $Beaver_price = new_price(.2,  .9);
    my $P = int(10 * rand(1)) + 1;
    if ($P <= 2)
    {
        $Quantity[2] = 0;
        print "YOUR BEAVER WERE TOO HEAVY TO CARRY ACROSS\n";
        print "THE PORTAGE.  YOU HAD TO LEAVE THE PELTS, BUT FOUND\n";
        print "THEM STOLEN WHEN YOU RETURNED.\n";
        supplies_fs();
        return 1414;
    }
    elsif ($P <= 6)
    {
        print "YOU ARRIVED SAFELY AT FORT STADACONA.\n";
        supplies_fs();
    }
    elsif ($P <= 8)
    {
        reset_furs();
        print "YOUR CANOE UPSET IN THE LACHINE RAPIDS.  YOU\n";
        print "LOST ALL YOUR FURS.\n";
        supplies_fs();
        return 1418;
    }
    elsif ($P <= 10)
    {
        $Quantity[4] = 0;
        print "YOUR FOX PELTS WERE NOT CURED PROPERLY.\n";
        print "NO ONE WILL BUY THEM.\n";
        supplies_fs();
    }
    return 1410;
}

sub fort3
{
    print "\nYOU HAVE CHOSEN THE MOST DIFFICULT ROUTE.  AT\n";
    print "FORT NEW YORK YOU WILL RECEIVE THE HIGHEST VALUE\n";
    print "FOR YOUR FURS.  THE COST OF YOUR SUPPLIES\n";
    print "WILL BE LOWER THAN AT ALL THE OTHER FORTS.\n";
    my $ans = trade_elsewhere();
    if ($ans eq "YES") { return 0; }

    $Money -= 105;
    print "\n";
    $Mink_price = new_price(.15, 1.05);
    $Fox_price = new_price(.25, 1.1);
    $Fox_price = new_price(.25, 1.1);
    my $P = int(10 * rand(1)) + 1;
    if ($P <= 2)
    {
        print "YOU WERE ATTACKED BY A PARTY OF IROQUOIS.\n";
        print "ALL PEOPLE IN YOUR TRADING GROUP WERE\n";
        print "KILLED.  THIS ENDS THE GAME.\n";
        exit(0);
    }
    elsif ($P <= 6)
    {
        print "YOU WERE LUCKY.  YOU ARRIVED SAFELY\n";
        print "AT FORT NEW YORK.\n";
        supplies_ny();
    }
    elsif ($P <= 8)
    {
        reset_furs();
        print "YOU NARROWLY ESCAPED AN IROQUOIS RAIDING PARTY.\n";
        print "HOWEVER, YOU HAD TO LEAVE ALL YOUR FURS BEHIND.\n";
        supplies_ny();
        return 1418;
    }
    elsif ($P <= 10)
    {
        $Beaver_price /= 2;
        $Mink_price /= 2;
        print "YOUR MINK AND BEAVER WERE DAMAGED ON YOUR TRIP.\n";
        print "YOU RECEIVE ONLY HALF THE CURRENT PRICE FOR THESE FURS.\n";
        supplies_ny();
    }
    return 1410;
}
