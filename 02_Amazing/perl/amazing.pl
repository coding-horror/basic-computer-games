#! /usr/bin/perl
use strict;
use warnings;

# Translated from BASIC by Alex Kapranoff

use feature qw/say/;

# width and height of the maze
my ($width, $height) = input_dimensions();

# wall masks for all cells
my @walls;

# flags of previous visitation for all cells
my %is_visited;

# was the path out of the maze found?
my $path_found = 0;

# column of entry to the maze in the top line
my $entry_col = int(rand($width));

# cell coordinates for traversal
my $col = $entry_col;
my $row = 0;

$is_visited{$row, $col} = 1;

# looping until we visit every cell
while (keys %is_visited < $width * $height) {
    if (my @dirs = get_possible_directions()) {
        my $dir = $dirs[rand @dirs];

        # modify current cell wall if needed
        $walls[$row]->[$col] |= $dir->[2];

        # move the position
        $row += $dir->[0];
        $col += $dir->[1];

        # we found the exit!
        if ($row == $height) {
            $path_found = 1;
            --$row;

            if ($walls[$row]->[$col] == 1) {
                ($row, $col) = get_next_branch(0, 0);
            }
        }
        else {
            # modify the new cell wall if needed
            $walls[$row]->[$col] |= $dir->[3];

            $is_visited{$row, $col} = 1;
        }
    }
    else {
        ($row, $col) = get_next_branch($row, $col);
    }
}

unless ($path_found) {
    $walls[-1]->[rand $width] |= 1;
}

print_maze();

sub input_dimensions {
    # Print the banner and returns the dimensions as two integers > 1.
    # The integers are parsed from the first line of standard input.
    say ' ' x 28, 'AMAZING PROGRAM';
    say ' ' x 15, 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY';
    print "\n" x 4;

    my ($w, $h) = (0, 0);

    while ($w <= 1 || $h <= 1) {
        print 'WHAT ARE YOUR WIDTH AND LENGTH? ';

        ($w, $h) = <STDIN> =~ / \d+ /xg;
        
        if ($w < 1 || $h < 1) {
            say "MEANINGLESS DIMENSIONS.  TRY AGAIN."
        }
    }

    print "\n" x 4;

    return ($w, $h);
}

sub get_possible_directions {
    # Returns a list of all directions that are available to go to
    # from the current coordinates. "Down" is available on the last line
    # until we go there once and mark it as the path through the maze.
    #
    # Each returned direction element contains changes to the coordindates and to
    # the wall masks of the previous and next cell after the move.

    my @rv;
    # up
    if ($row > 0 && !$is_visited{$row - 1, $col}) {
        push @rv, [-1, 0, 0, 1];
    }
    # left
    if ($col > 0 && !$is_visited{$row, $col - 1}) {
        push @rv, [0, -1, 0, 2];
    }
    # right
    if ($col < $width - 1 && !$is_visited{$row, $col + 1}) {
        push @rv, [0, 1, 2, 0];
    }
    # down
    if ($row < $height - 1 && !$is_visited{$row + 1, $col}
        || $row == $height - 1 && !$path_found
    ) {
        push @rv, [1, 0, 1, 0];
    }

    return @rv;
}

sub get_next_branch {
    # Returns the cell coordinates to start a new maze branch from.
    # It looks for a visited cell starting from passed position and
    # going down in the natural traversal order incrementing column and
    # rows with a rollover to start at the bottom right corner.
    my ($y, $x) = @_;
    do {
        if ($x < $width - 1) {
            ++$x;
        } elsif ($y < $height - 1) {
            ($y, $x) = ($y + 1, 0);
        } else {
            ($y, $x) = (0, 0);
        }
    } while (!$is_visited{$y, $x});

    return ($y, $x);
}

sub print_maze {
    # Print the full maze based on wall masks.
    # For each cell, we mark the absense of the wall to the right with
    # bit 2 and the absense of the wall down with bit 1. Full table:
    # 0 -> both walls are present
    # 1 -> wall down is absent
    # 2 -> wall to the right is absent
    # 3 -> both walls are absent
    say join('.', '', map { $_ == $entry_col ? '  ' : '--' } 0 .. $width - 1), '.';

    for my $row (@walls) {
        say join('  ', map { $_ & 2 ? ' ' : 'I' } 0, @$row);
        say join(':', '', map { $_ & 1 ? '  ' : '--' } @$row), '.';
    }

    return;
}
