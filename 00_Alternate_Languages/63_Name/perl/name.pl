#!/usr/bin/perl
use strict;

print ' 'x34 . "NAME\n";
print ' 'x15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";

print "HELLO.\n"; print "MY NAME IS CREATIVE COMPUTER.\n";
print "WHAT'S YOUR NAME (FIRST AND LAST): ";
chomp (my $A = <STDIN>);

my @B= split("", $A); #Convert string to array of characters.

print "\n"; print "THANK YOU, ";
print reverse @B;

print ".\n"; print "OOPS! I GUESS I GOT IT BACKWARDS. A SMART\n";
print "COMPUTER LIKE ME SHOULDN'T MAKE A MISTAKE LIKE THAT!\n\n";
print "BUT I JUST NOTICED YOUR LETTERS ARE OUT OF ORDER.\n";
print "LET'S PUT THEM IN ORDER LIKE THIS: ";
print sort @B;

print "\n\n";
print "DON'T YOU LIKE THAT BETTER? ";
chomp (my $D = <STDIN>);
if (uc($D) eq "YES") {
	print "\n"; print "I KNEW YOU'D AGREE!!\n";
	} else {
	print "\n"; print "I'M SORRY YOU DON'T LIKE IT THAT WAY.\n";
	}
print "\n"; print "I REALLY ENJOYED MEETING YOU $A.\n";
print "HAVE A NICE DAY!\n";
exit;
