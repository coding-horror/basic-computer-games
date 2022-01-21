#!/usr/bin/perl
#YATOL: Yet Another TOdo List
use strict;

#REM: Get list of basic files ordered by number of lines.
#REM: This way you can do the easier ones first.
my @Ret=`find .. -iname '*.bas' -exec wc -l \{\} \\; | sort -h`;


my @Langs= qw(PL JS VB PAS RB C# JAVA PY);
my @Dirs= qw(perl javascript vbnet pascal ruby csharp java python);
my %Sum;

print " "x 25 ."BAS\t";
foreach my $Dir (@Langs) {
	print "$Dir\t";
	}	
print "\n";

my $Count;
foreach my $Lin (@Ret) {
	$Count++;
	chomp $Lin;
	my ($Num, $File)= split (" ", $Lin);
	my @Parts= split(/\//, $File);
	my $Base= $Parts[1];

	my $Tab= 25-length($Base);
	print "$Base".(" "x$Tab)."$Num\t";

	foreach my $Dir (@Dirs) {
		my $Path= "../$Base/$Dir/";
		my $Ret= `ls $Path | wc -l`;
		if ($Ret>1) { print "YES"; $Sum{$Dir}++; }
			else { print "  ";}
		print "\t";
		}
	print "\n";
	
	}

print "\t\tFILES:\t\t";
foreach my $Dir (@Dirs) {
	print "$Sum{$Dir}\t";
	}
print "\n";


print "\t\tADVANCE:\t";
foreach my $Dir (@Dirs) {
	my $Per= int($Sum{$Dir}/$Count*100)."%";
	print "$Per\t";
	}
print "\n";

