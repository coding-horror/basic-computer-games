#!/usr/bin/perl

use strict;
use warnings;

print ' ' x 30 . "CHIEF\n";
print ' ' x 15 . "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";

print "I AM CHIEF NUMBERS FREEK, THE GREAT INDIAN MATH GOD.\n";
print "ARE YOU READY TO TAKE THE TEST YOU CALLED ME OUT FOR?\n";

chomp( my $A = uc <STDIN> );
print "SHUT UP, PALE FACE WITH WISE TONGUE.\n" unless ( $A eq 'YES' );

print " TAKE A NUMBER AND ADD 3. DIVIDE THIS NUMBER BY 5 AND\n";
print "MULTIPLY BY 8. DIVIDE BY 5 AND ADD THE SAME. SUBTRACT 1.\n";
print "  WHAT DO YOU HAVE?\n";

chomp( my $B = <STDIN> );
my $C = ( $B + 1 - 5 ) * 5 / 8 * 5 - 3;

print "I BET YOUR NUMBER WAS $C. AM I RIGHT?\n";

chomp( my $D = uc <STDIN> );
if ( $D eq 'YES' ) {
    print "BYE!!!\n";
    exit;
}

print "WHAT WAS YOUR ORIGINAL NUMBER?\n";

chomp( my $K = <STDIN> );
my $F = $K + 3;
my $G = $F / 5;
my $H = $G * 8;
my $I = $H / 5 + 5;
my $J = $I - 1;

print "SO YOU THINK YOU'RE SO SMART, EH?\n";
print "NOW WATCH.\n";
print "$K PLUS 3 EQUALS $F. THIS DIVIDED BY 5 EQUALS $G;\n";
print "THIS TIMES 8 EQUALS $H. IF WE DIVIDE BY 5 AND ADD 5,\n";
print "WE GET $I , WHICH, MINUS 1, EQUALS $J.\n";
print "NOW DO YOU BELIEVE ME?\n";

chomp( my $Z = uc <STDIN> );
if ( $Z eq 'YES' ) {
    print "BYE!!!\n";
    exit;
}

print "YOU HAVE MADE ME MAD!!!\n";
print "THERE MUST BE A GREAT LIGHTNING BOLT!\n\n\n";

for my $i ( reverse 22 .. 30 ) {
    print ' ' x $i . "X X\n";
}
print ' ' x 21 . "X XXX\n";
print ' ' x 20 . "X   X\n";
print ' ' x 19 . "XX X\n";
for my $i ( reverse 13 .. 20 ) {
    print ' ' x $i . "X X\n";
}
print ' ' x 12 . "XX\n";
print ' ' x 11 . "X\n";
print ' ' x 10 . "*\n";
print "\n#########################\n\n";
print "I HOPE YOU BELIEVE ME NOW, FOR YOUR SAKE!!\n";
