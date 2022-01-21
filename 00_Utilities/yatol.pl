#!/usr/bin/perl
#YATOL: Yet Another TOdo List
use strict;

#REM: Get list of basic files ordered by number or lines.
#REM: This way you can do the easier ones first.
my @Ret=`find .. -iname '*.bas' -exec wc -l \{\} \\; | sort -h`;


my @Langs= qw(PL JS VB PAS RB C# JAVA PY);
my @Dirs= qw(perl javascript vbnet pascal ruby csharp java python);

print " "x 25 ."LINES\t";
foreach my $Dir (@Langs) {
	print "$Dir\t";
	}	
print "\n";

foreach my $Lin (@Ret) {
	chomp $Lin;
	my ($Num, $File)= split (" ", $Lin);
	my @Parts= split(/\//, $File);
	my $Base= $Parts[1];

	my $Tab= 25-length($Base);
	print "$Base".(" "x$Tab)."$Num\t";

	foreach my $Dir (@Dirs) {
		my $Path= "../$Base/$Dir/";
		my $Ret= `ls $Path | wc -l`;
		if ($Ret>1) { print "YES"; }
			else { print "  ";}
		print "\t";
		}
	print "\n";
	}
