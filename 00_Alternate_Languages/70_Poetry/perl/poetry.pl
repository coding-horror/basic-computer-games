#!/usr/bin/perl
#use strict;
# Automatic converted by bas2perl.pl
# Too much spaguetti code to be properly converted.


print ' 'x 30 . "POETRY\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n"; print "\n"; print "\n";
Line90:
if ($I==1) { goto Line100; } elsif ($I==2) { goto Line101; } elsif ($I==3) { goto Line102; } elsif ($I==4) { goto Line103; } elsif ($I==5) { goto Line104; } ;
Line100:
print "MIDNIGHT DREARY"; goto Line210;
Line101:
print "FIERY EYES"; goto Line210;
Line102:
print "BIRD OR FIEND"; goto Line210;
Line103:
print "THING OF EVIL"; goto Line210;
Line104:
print "PROPHET"; goto Line210;
Line110:
if ($I==1) { goto Line111; } elsif ($I==2) { goto Line112; } elsif ($I==3) { goto Line113; } elsif ($I==4) { goto Line114; } elsif ($I==5) { goto Line115; } ;
Line111:
print "BEGUILING ME"; $U=2; goto Line210;
Line112:
print "THRILLED ME"; goto Line210;
Line113:
print "STILL SITTING...."; goto Line212;
Line114:
print "NEVER FLITTING"; $U=2; goto Line210;
Line115:
print "BURNED"; goto Line210;
Line120:
if ($I==1) { goto Line121; } elsif ($I==2) { goto Line122; } elsif ($I==3) { goto Line123; } elsif ($I==4) { goto Line124; } elsif ($I==5) { goto Line125; } ;
Line121:
print "AND MY SOUL"; goto Line210;
Line122:
print "DARKNESS THERE"; goto Line210;
Line123:
print "SHALL BE LIFTED"; goto Line210;
Line124:
print "QUOTH THE RAVEN"; goto Line210;
Line125:
if ($U==0) { goto Line210; }
print "SIGN OF PARTING"; goto Line210;
Line130:
if ($I==1) { goto Line131; } elsif ($I==2) { goto Line132; } elsif ($I==3) { goto Line133; } elsif ($I==4) { goto Line134; } elsif ($I==5) { goto Line135; } ;
Line131:
print "NOTHING MORE"; goto Line210;
Line132:
print "YET AGAIN"; goto Line210;
Line133:
print "SLOWLY CREEPING"; goto Line210;
Line134:
print "...EVERMORE"; goto Line210;
Line135:
print "NEVERMORE";
Line210:
if ($U==0 || rand(1)>.19) { goto Line212; }
print ","; $U=2;
Line212:
if (rand(1)>.65) { goto Line214; }
print " "; $U=$U+1; goto Line215;
Line214:
print "\n"; $U=0;
Line215:
$I=int(int(10*rand(1))/2)+1;
$J=$J+1; $K=$K+1;
if ($U>0 || int($J/2)!=$J/2) { goto Line240; }
print " ";
Line240:
if ($J==1) { goto Line90; } elsif ($J==2) { goto Line110; } elsif ($J==3) { goto Line120; } elsif ($J==4) { goto Line130; } elsif ($J==5) { goto Line250; } ;
Line250:
$J=0; print "\n"; if ($K>20) { goto Line270; }
goto Line215;
Line270:
print "\n"; $U=0; $K=0; goto Line110;
exit;
