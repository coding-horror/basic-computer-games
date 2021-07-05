#!/usr/bin/perl
#use strict;
# Automatic converted by bas2perl.pl

print ' 'x28 . "RUSSIAN ROULETTE\n";
print ' 'x15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n"; print "\n"; print "\n";
print "THIS IS A GAME OF >>>>>>>>>>RUSSIAN ROULETTE.\n";
Line10:
print "\n"; print "HERE IS A REVOLVER.\n";
Line20:
print "TYPE '1' TO SPIN CHAMBER AND PULL TRIGGER.\n";
print "TYPE '2' TO GIVE UP.\n";
print "GO";
$N=0;
Line30:
print "? "; chomp($I = <STDIN>);
if ($I ne 2) { goto Line35; }
print " CHICKEN!!!!!\n";
goto Line72;
Line35:
$N=$N+1;
if (rand(1)>.833333) { goto Line70; }
if ($N>10) { goto Line80; }
print "- CLICK -\n";
print "\n"; goto Line30;
Line70:
print " BANG!!!!! YOU'RE DEAD!\n";
print "CONDOLENCES WILL BE SENT TO YOUR RELATIVES.\n";
Line72:
print "\n"; print "\n"; print "\n";
print "...NEXT VICTIM...\n"; goto Line20;
Line80:
print "YOU WIN!!!!!\n";
print "LET SOMEONE ELSE BLOW HIS BRAINS OUT.\n";
goto Line10;
exit;


