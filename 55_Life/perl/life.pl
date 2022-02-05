#!/usr/bin/perl
#use strict;

print ' 'x 34 . "LIFE\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n"; print "\n"; print "\n";
print "ENTER YOUR PATTERN; \n";
$X1=1; $Y1=1; $X2=24; $Y2=70;
@A;
$C=1;

@B;
Line30:
print "? "; chomp($B[$C] = uc(<STDIN>));
if ($B[$C] eq "DONE") { $B[$C]=""; goto Line80; }
$B[$C]=~ s/\./ /g;
$C=$C+1;
goto Line30;


Line80:

$C=$C-1; $L=0; $G=0;
for ($X=1; $X<=$C-1; $X++) {
	if (length($B[$X])>$L) { $L=length($B[$X]); }
	}

$X1=11-$C/2;
$Y1=33-$L/2;
for ($X=1; $X<=$C; $X++) {
	for ($Y=1; $Y<=length($B[$X]); $Y++) {
		if (substr($B[$X],$Y-1,1) ne " ") { $A[$X1+$X][$Y1+$Y]=1; $P=$P+1; }
		}
	}
print "\n"; print "\n"; print "\n";

Line210:
print "GENERATION: ".$G."\t\tPOPULATION: ".$P; if ($I9) { print "\tINVALID!"; }
print "\n";
$X3=24; $Y3=70; $X4=1; $Y4=1; $P=0;
$G=$G+1;
for ($X=1; $X<=$X1-1; $X++) { print "\n"; }
for ($X=$X1; $X<=$X2; $X++) {
	$Row= " "x 80;
	for ($Y=$Y1; $Y<=$Y2; $Y++) {
		if ($A[$X][$Y]==2) { $A[$X][$Y]=0; goto Line270; }
		if ($A[$X][$Y]==3) { $A[$X][$Y]=1; goto Line261; }
		if ($A[$X][$Y]!=1) { goto Line270; }

		Line261:
		substr($Row, $Y, 1, "*");
		if ($X<$X3) { $X3=$X; }
		if ($X>$X4) { $X4=$X; }
		if ($Y<$Y3) { $Y3=$Y; }
		if ($Y>$Y4) { $Y4=$Y; }

		Line270:
		}
	print "$Row\n";
	}

for ($X=$X2+1; $X<=24; $X++) { print "\n"; }
$X1=$X3; $X2=$X4; $Y1=$Y3; $Y2=$Y4;
if ($X1<3) { $X1=3; $I9=-1; }
if ($X2>22) { $X2=22; $I9=-1; }
if ($Y1<3) { $Y1=3; $I9=-1; }
if ($Y2>68) { $Y2=68; $I9=-1; }
$P=0;

for ($X=$X1-1; $X<=$X2+1; $X++) {
	for ($Y=$Y1-1; $Y<=$Y2+1; $Y++) {
		$C=0;
		for ($I=$X-1; $I<=$X+1; $I++) {
			for ($J=$Y-1; $J<=$Y+1; $J++) {
				if ($A[$I][$J]==1 || $A[$I][$J]==2) { $C=$C+1; }
				}
			}
		if ($A[$X][$Y]==0) { goto Line610; }
		if ($C<3 || $C>4) { $A[$X][$Y]=2; goto Line600; }
		$P=$P+1;

		Line600:
		goto Line620;

		Line610:
		if ($C==3) { $A[$X][$Y]=3; $P=$P+1; }

		Line620:
		}
	}
$X1=$X1-1; $Y1=$Y1-1; $X2=$X2+1; $Y2=$Y2+1;
goto Line210;
exit;


