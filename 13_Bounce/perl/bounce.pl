#!/usr/bin/perl

# Bounce program in Perl
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

print "\n";
print " " x 31,"BOUNCE\n";
print " " x 15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";

print "THIS SIMULATION LETS YOU SPECIFY THE INITIAL VELOCITY\n";
print "OF A BALL THROWN STRAIGHT UP, AND THE COEFFICIENT OF\n";
print "ELASTICITY OF THE BALL.  PLEASE USE A DECIMAL FRACTION\n";
print "COEFFICIENCY (LESS THAN 1).\n\n";
print "YOU ALSO SPECIFY THE TIME INCREMENT TO BE USED IN\n";
print "'STROBING' THE BALL'S FLIGHT (TRY .1 INITIALLY).\n\n";

# deal with basic's tab() for line positioning
#   line = line string we're starting with
#   pos = position to start writing
#   s = string to write
# returns the resultant string, which might not have been changed
sub line_tab
{
    my ($line, $pos, $s) = @_;
    my $len = length($line);
    # if curser is past position, do nothing
    if ($len <= $pos) { $line .= " " x ($pos - $len) . $s; }
    return $line;
}

while (1)
{
    my @T;          # time slice?
    my $time_inc;   # time increment, probably in fractions of seconds
    my $velocity;   # velocity in feet/sec
    my $coeff_elas; # coeeficent of elasticity
    my $line_pos;   # position on line
    my $S1          # duration in full seconds?

    # get input
    print "TIME INCREMENT (SEC, 0=QUIT): "; chomp($time_inc = <>);
    last if ($time_inc == 0);
    print "VELOCITY (FPS): "; chomp($velocity = <>);
    print "COEFFICIENT: "; chomp($coeff_elas = <>);
    if ($coeff_elas >= 1.0 || $coeff_elas <= 0)
    {
        print "COEFFICIENT MUST BE > 0 AND < 1.0\n\n\n";
        next;
    }

    print "\nFEET\n";
    $S1 = int(70.0 / ($velocity / (16.0 * $time_inc)));
    for my $i (1 .. $S1)
    {
        $T[$i] = $velocity * $coeff_elas ** ($i - 1) / 16.0;
    }

    # draw graph
    for (my $height=int(-16.0 * ($velocity / 32.0) ** 2.0 + $velocity ** 2.0 / 32.0 + .5) ; $height >= 0 ; $height -= .5)
    {
        if (int($height) == $height) { print sprintf("%2d", $height); }
        else                         { print "  "; }
        $line_pos = 0;
        my $curr_line = "";
        for my $i (1 .. $S1)
        {
            my $time;
            for ($time=0 ; $time <= $T[$i] ; $time += $time_inc)
            {
                $line_pos += $time_inc;
                if (abs($height - (.5 * (-32) * $time ** 2.0 + $velocity * $coeff_elas ** ($i - 1) * $time)) <= .25)
                {
                    $curr_line = line_tab($curr_line, ($line_pos / $time_inc), "0");
                }
            }
            $time = ($T[$i + 1] // 0) / 2; # we can reach 1 past the end, use 0 if that happens
            last if (-16.0 * $time ** 2.0 + $velocity * $coeff_elas ** ($i - 1) * $time < $height);
        }
        print "$curr_line\n";
    }

    # draw scale
    print "  .";
    print "." x (int($line_pos + 1) / $time_inc + 1), "\n";
    print "  0";
    my $curr_line = "";
    for my $i (1 .. int($line_pos + .9995))
    {
        $curr_line = line_tab($curr_line, int($i / $time_inc), $i);
    }
    print "$curr_line\n";
    print " " x (int($line_pos + 1) / (2 * $time_inc) - 2), "SECONDS\n\n";
}
