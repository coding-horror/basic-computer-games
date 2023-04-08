#!/usr/bin/perl

# Banner program in Perl
# Translated by Kevin Brannen (kbrannen)

use strict;
use warnings;

sub print_lines
{
    my $lines = shift;
    print "\n" x $lines;
}

# each letter is made of 7 slices (or rows);
# the initial & unused 0 is to allow for Perl arrays being 0-based
#   but allow the algorithm to be 1-based (like Basic arrays);
# the numbers are essentially the dots/columns per slice in powers of 2.
my %data = (
    " " => [ 0,  0, 0, 0, 0, 0, 0, 0 ], 
    "." => [ 0,  1, 1, 129, 449, 129, 1, 1 ], 
    "!" => [ 0,  1, 1, 1, 384, 1, 1, 1 ], 
    "=" => [ 0,  41, 41, 41, 41, 41, 41, 41 ], 
    "?" => [ 0,  5, 3, 2, 354, 18, 11, 5 ], 
    "*" => [ 0,  69, 41, 17, 512, 17, 41, 69 ], 
    "0" => [ 0,  57, 69, 131, 258, 131, 69, 57 ], 
    "1" => [ 0,  0, 0, 261, 259, 512, 257, 257 ], 
    "2" => [ 0,  261, 387, 322, 290, 274, 267, 261 ], 
    "3" => [ 0,  66, 130, 258, 274, 266, 150, 100 ], 
    "4" => [ 0,  33, 49, 41, 37, 35, 512, 33 ], 
    "5" => [ 0,  160, 274, 274, 274, 274, 274, 226 ], 
    "6" => [ 0,  194, 291, 293, 297, 305, 289, 193 ], 
    "7" => [ 0,  258, 130, 66, 34, 18, 10, 8 ], 
    "8" => [ 0,  69, 171, 274, 274, 274, 171, 69 ], 
    "9" => [ 0,  263, 138, 74, 42, 26, 10, 7 ], 
    "A" => [ 0,  505, 37, 35, 34, 35, 37, 505 ], 
    "B" => [ 0,  512, 274, 274, 274, 274, 274, 239 ], 
    "C" => [ 0,  125, 131, 258, 258, 258, 131, 69 ], 
    "D" => [ 0,  512, 258, 258, 258, 258, 131, 125 ], 
    "E" => [ 0,  512, 274, 274, 274, 274, 258, 258 ], 
    "F" => [ 0,  512, 18, 18, 18, 18, 2, 2 ], 
    "G" => [ 0,  125, 131, 258, 258, 290, 163, 101 ], 
    "H" => [ 0,  512, 17, 17, 17, 17, 17, 512 ], 
    "I" => [ 0,  258, 258, 258, 512, 258, 258, 258 ], 
    "J" => [ 0,  65, 129, 257, 257, 257, 129, 128 ], 
    "K" => [ 0,  512, 17, 17, 41, 69, 131, 258 ], 
    "L" => [ 0,  512, 257, 257, 257, 257, 257, 257 ], 
    "M" => [ 0,  512, 7, 13, 25, 13, 7, 512 ], 
    "N" => [ 0,  512, 7, 9, 17, 33, 193, 512 ], 
    "O" => [ 0,  125, 131, 258, 258, 258, 131, 125 ], 
    "P" => [ 0,  512, 18, 18, 18, 18, 18, 15 ], 
    "Q" => [ 0,  125, 131, 258, 258, 322, 131, 381 ], 
    "R" => [ 0,  512, 18, 18, 50, 82, 146, 271 ], 
    "S" => [ 0,  69, 139, 274, 274, 274, 163, 69 ], 
    "T" => [ 0,  2, 2, 2, 512, 2, 2, 2 ], 
    "U" => [ 0,  128, 129, 257, 257, 257, 129, 128 ], 
    "V" => [ 0,  64, 65, 129, 257, 129, 65, 64 ], 
    "W" => [ 0,  256, 257, 129, 65, 129, 257, 256 ], 
    "X" => [ 0,  388, 69, 41, 17, 41, 69, 388 ], 
    "Y" => [ 0,  8, 9, 17, 481, 17, 9, 8 ], 
    "Z" => [ 0,  386, 322, 290, 274, 266, 262, 260 ], 
);

my ($horz, $vert, $center, $char, $msg) = (0, 0, '', '', '');

# get args to run with
while ($horz < 1)
{
    print "HORIZONTAL (1 or more): ";
    chomp($horz = <>);
    $horz = int($horz);
}

while ($vert < 1)
{
    print "VERTICAL (1 or more): ";
    chomp($vert = <>);
    $vert = int($vert);
}

print "CENTERED (Y/N): ";
chomp($center = <>);
$center = ($center =~ m/^Y/i) ? 1 : 0;

# note you can enter multiple chars and the program will do the right thing
# thanks to the length() calls below, which was in the original Basic
print "CHARACTER TO PRINT (TYPE 'ALL' IF YOU WANT CHARACTER BEING PRINTED): ";
chomp($char = uc(<>));

while (!$msg)
{
    print "STATEMENT: ";
    chomp($msg = uc(<>));
}

print "SET PAGE TO PRINT, HIT RETURN WHEN READY";
$_ = <>;
print_lines(2 * $horz);

# print the message
for my $letter ( split(//, $msg) )
{
    if (!exists($data{$letter}))
    {
        die "Cannot use letter '$letter'!";
    }
    my @s = @{$data{$letter}};
    #if ($letter eq " ") { print_lines(7 * $horz); next; }

    my $print_letter = ($char eq "ALL") ? $letter : $char;
    for my $slice (1 .. 7)
    {
        my (@j, @f);
        for (my $k = 8; $k >= 0; $k--)
        {
            if (2**$k < $s[$slice])
            {
                $j[9 - $k] = 1;
                $s[$slice] = $s[$slice] - 2**$k;
                if ($s[$slice] == 1)
                {
                    $f[$slice] = 9 - $k;
                }
            }
            else
            {
                $j[9 - $k] = 0;
            }
        }

        for my $t1 (1 .. $horz)
        {
            print " " x int((63 - 4.5 * $vert) * $center / (length($print_letter)) + 1);
            for my $b (1 .. (defined($f[$slice]) ? $f[$slice] : 0))
            {
                my $str = $j[$b] ? $print_letter : (" " x length($print_letter));
                print $str for (1 .. $vert);
            }
            print "\n";
        }
    }

    # space between letters
    print_lines(2 * $horz);
}

# while in the original code, this seems pretty excessive
#print_lines(75);

exit(0);
