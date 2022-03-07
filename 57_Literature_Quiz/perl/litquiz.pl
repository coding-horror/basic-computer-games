#!/usr/bin/perl
use strict;


print ' 'x 25 . "LITERATURE QUIZ\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n"; print "\n"; print "\n";
print "TEST YOUR KNOWLEDGE OF CHILDREN'S LITERATURE.\n";
print "\n"; print "THIS IS A MULTIPLE-CHOICE QUIZ.\n";
print "TYPE A 1, 2, 3, OR 4 AFTER THE QUESTION MARK.\n";
print "\n"; print "GOOD LUCK!\n";
my $R=0;


print "\n"; print "\n";
print "IN PINOCCHIO, WHAT WAS THE NAME OF THE CAT\n";
print "1)TIGGER, 2)CICERO, 3)FIGARO, 4)GUIPETTO";
print "? "; chomp(my $A = <STDIN>);

if ($A eq 3) {
	$R++;
	print "VERY GOOD! HERE'S ANOTHER.\n";
	} else {
	print "SORRY...FIGARO WAS HIS NAME.\n";
	}


print "\n"; print "\n";
print "FROM WHOSE GARDEN DID BUGS BUNNY STEAL THE CARROTS?\n";
print "1)MR. NIXON'S, 2)ELMER FUDD'S, 3)CLEM JUDD'S, 4)STROMBOLI'S";
print "? "; chomp($A = <STDIN>);

if ($A eq 2) {
	print "PRETTY GOOD!\n";
	$R=$R+1;
	} else {
	print "TOO BAD...IT WAS ELMER FUDD'S GARDEN.\n";
	}


print "\n"; print "\n";
print "IN THE WIZARD OF OS, DOROTHY'S DOG WAS NAMED\n";
print "1)CICERO, 2)TRIXIA, 3)KING, 4)TOTO";
print "? "; chomp($A = <STDIN>);
if ($A eq 4) {
	print "YEA! YOU'RE A REAL LITERATURE GIANT.\n";
	$R=$R+1;
	} else {
	print "BACK TO THE BOOKS,...TOTO WAS HIS NAME.\n";
	}


print "\n"; print "\n";
print "WHO WAS THE FAIR MAIDEN WHO ATE THE POISON APPLE\n";
print "1)SLEEPING BEAUTY, 2)CINDERELLA, 3)SNOW WHITE, 4)WENDY";
print "? "; chomp($A = <STDIN>);
if ($A eq 3) {
	print "GOOD MEMORY!\n";
	$R=$R+1;
	} else {
	print "OH, COME ON NOW...IT WAS SNOW WHITE.\n";
	}


print "\n"; print "\n";
if ($R eq 4) {
	print "WOW! THAT'S SUPER! YOU REALLY KNOW YOUR NURSERY\n";
	print "YOUR NEXT QUIZ WILL BE ON 2ND CENTURY CHINESE\n";
	print "LITERATURE (HA, HA, HA)\n";
	exit
	}
if ($R<2) {
	print "UGH. THAT WAS DEFINITELY NOT TOO SWIFT. BACK TO\n";
	print "NURSERY SCHOOL FOR YOU, MY FRIEND.\n";
	exit;
	}

print "NOT BAD, BUT YOU MIGHT SPEND A LITTLE MORE TIME\n";
print "READING THE NURSERY GREATS.\n";
exit;
