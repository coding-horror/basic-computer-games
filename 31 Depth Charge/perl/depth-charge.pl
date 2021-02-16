#!/usr/bin/perl

use strict;
use warnings;

print '               Depth Charge
Creative Computing Morristown, New Jersey


Depth Charge Game

Dimensions of Search Area? ';

my $g = <STDIN>;
my $n = int( log($g) / log 2 ) + 1;
print '
You are the captain of the Destroyer USS Computer
an enemy sub has been causing you trouble.  Your
mission is to destroy it.  You have ',$n,' shots.
Specify depth charge explosion point with a
trio of number -- the first two are the surface
co-ordinates; the third is the depth.
';

while(1) { ## Repeat until we say no....
  print "\nGood luck!\n\n";
  my ($a,$b,$c) = map { int rand $g } 1..3; ## Get the location
  my $hit = 0; ## Keep track if we have won yet!
  foreach ( 1..$n ) {
    print "\nTrial # $_ ? ";
    my ( $x, $y, $z ) = split m{\D+}, <STDIN>;
    if( $x==$a && $y==$b && $z==$c ) {
      $hit = 1; ## We have won
      print "\n\nB O O M ! ! You found it in $_ tries!\n";
      last;
    }
    print join q( ), 'Sonar reports show was',
      $y < $b              ? 'South'    : $y > $b ? 'North'   : (),
      $x < $a              ? 'West'     : $x > $a ? 'East'    : (),
      $x == $a && $y == $b ? ()         : 'and' ,
      $z < $c              ? 'too high' : $z > $c ? 'too low' : 'depth OK',
      ".\n";
  }

  ## Only show message if we haven't won...
  print "\nYou have been torpedoed!  Abandon ship!\nThe submarine was at $a, $b, $c\n" unless $hit;

  print "\n\nAnother game (Y or N)? ";
  last unless <STDIN> =~ m{Y}i; ## Y or y not typed so leave loop
}
## Say good bye
print "OK.  Hope you enjoyed yourself.\n\n";
