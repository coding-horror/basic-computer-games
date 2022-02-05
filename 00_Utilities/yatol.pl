#!/usr/bin/perl
#YATOL: Yet Another TOdo List
use strict;

#REM: Get list of basic files ordered by number of lines.
#REM: This way you can do the easier ones first.
my @Ret=`find .. -iname '*.bas' -exec wc -l \{\} \\; | sort -h`;


my @Langs= qw(PL JS VB PAS RB C# JAVA PY);
my @Dirs= qw(perl javascript vbnet pascal ruby csharp java python);
my %Sum;

my $Row=25;
my $Tab=7;

printf("%-$Row\s", "PATH");
printf("%$Tab\s", "BAS");

foreach my $Dir (@Langs) {
	printf("%$Tab\s", $Dir);
	}	
print "\n";

my $Count;
foreach my $Lin (@Ret) {
	$Count++;
	chomp $Lin;
	my ($Num, $File)= split (" ", $Lin);
	my @Parts= split(/\//, $File);
	my $Base= $Parts[1];

	printf("%-$Row\s", $Base);
	printf("%$Tab\s", "$Num");

	foreach my $Dir (@Dirs) {
		my $Path= "../$Base/$Dir/";
		my $Ret= `ls $Path | wc -l`;
		if ($Ret>1) { printf("%$Tab\s", "YES"); $Sum{$Dir}++; }
			else { printf("%$Tab\s", " ");}
		}
	print "\n";	
	}


printf("%$Row\s", "FILES:");
printf("%$Tab\s", " ");
foreach my $Dir (@Dirs) {
	printf("%$Tab\s", "$Sum{$Dir}");
	}
print "\n";

printf("%$Row\s", "ADVANCE:");
printf("%$Tab\s", " ");
foreach my $Dir (@Dirs) {
	my $Per= int($Sum{$Dir}/$Count*100)."%";
	printf("%$Tab\s", "$Per");
	}
print "\n";

