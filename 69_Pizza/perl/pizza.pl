#!/usr/bin/perl
#use strict;
# Automatic converted by bas2perl.pl

print ' 'x33 . "PIZZA". "\n";
print ' 'x15 . "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY". "\n";
print "\n"; print "\n"; print "\n";
my @S; my @M;
print "PIZZA DELIVERY GAME". "\n"; print "\n";
print "WHAT IS YOUR FIRST NAME? "; chomp($N = uc(<STDIN>)); print "\n";
print "HI, ". $N. ".  IN THIS GAME YOU ARE TO TAKE ORDERS". "\n";
print "FOR PIZZAS.  THEN YOU ARE TO TELL A DELIVERY BOY". "\n";
print "WHERE TO DELIVER THE ORDERED PIZZAS.". "\n"; print "\n"; print "\n";
for ($I=1; $I<=16; $I++) {
$S[$I]= <DATA>; chomp($S[$I]);;
}
for ($I=1; $I<=4; $I++) {
$M[$I]= <DATA>; chomp($M[$I]);;
}
# TO DATA SEGMENT;
# TO DATA SEGMENT;
print "MAP OF THE CITY OF HYATTSVILLE". "\n"; print "\n";
print " -----1-----2-----3-----4-----". "\n";
$K=4;
for ($I=1; $I<=4; $I++) {
print "-". "\n"; print "-". "\n"; print "-". "\n"; print "-". "\n";
print $M[$K];
$S1=16-4*$I+1;
print "     ". $S[$S1]. "     ". $S[$S1+1]. "     ". $S[$S1+2]. "     ";
print $S[$S1+3]. "     ". $M[$K]. "\n";
$K=$K-1;
}
print "-". "\n"; print "-". "\n"; print "-". "\n"; print "-". "\n";
print " -----1-----2-----3-----4-----". "\n"; print "\n";
print "THE OUTPUT IS A MAP OF THE HOMES WHERE". "\n";
print "YOU ARE TO SEND PIZZAS.". "\n"; print "\n";
print "YOUR JOB IS TO GIVE A TRUCK DRIVER". "\n";
print "THE LOCATION OR COORDINATES OF THE". "\n";
print "HOME ORDERING THE PIZZA.". "\n"; print "\n";

Line520:
print "DO YOU NEED MORE DIRECTIONS? "; chomp($A = uc(<STDIN>));
if ($A eq "YES") { goto Line590; }
if ($A eq "NO") { goto Line750; }
print "'YES' OR 'NO' PLEASE, NOW THEN,". "\n"; goto Line520;

Line590:
print "\n"; print "SOMEBODY WILL ASK FOR A PIZZA TO BE". "\n";
print "DELIVERED.  THEN A DELIVERY BOY WILL". "\n";
print "ASK YOU FOR THE LOCATION.". "\n"; print "     EXAMPLE:". "\n";
print "THIS IS J.  PLEASE SEND A PIZZA.". "\n";
print "DRIVER TO ". $N. ".  WHERE DOES J LIVE?". "\n";
print "YOUR ANSWER WOULD BE 2,3". "\n"; print "\n";
print "UNDERSTAND? "; chomp($A = uc(<STDIN>));
if ($A eq "YES") { goto Line690; }
print "THIS JOB IS DEFINITELY TOO DIFFICULT FOR YOU. THANKS ANYWAY". "\n";
goto Line999;

Line690:
print "GOOD.  YOU ARE NOW READY TO START TAKING ORDERS.". "\n"; print "\n";
print "GOOD LUCK!!". "\n"; print "\n";

Line750:
for ($I=1; $I<=5; $I++) {
$S=int(rand(1)*16+1); print "\n";
print "HELLO ". $N. "'S PIZZA.  THIS IS ". $S[$S]. ".";
print "  PLEASE SEND A PIZZA.". "\n";

Line780:
print "  DRIVER TO ". $N. ":  WHERE DOES ". $S[$S]. " LIVE";
print "? "; chomp($Inp_ = uc(<STDIN>)); ($A[1],$A[2])= split(/,/, $Inp_);
$T=$A[1]+($A[2]-1)*4;
if ($T eq $S) { goto Line920; }
print "THIS IS ". $S[$T]. ".  I DID NOT ORDER A PIZZA.". "\n";
print "I LIVE AT ". $A[1]. ",". $A[2]. "\n";
goto Line780;

Line920:
print "HELLO ". $N. ".  THIS IS ". $S[$S]. ", THANKS FOR THE PIZZA.". "\n";
}
print "\n"; print "DO YOU WANT TO DELIVER MORE PIZZAS? "; chomp($A = uc(<STDIN>));
if ($A eq "YES") { goto Line750; }
print "\n"; print "O.K. ". $N. ", SEE YOU LATER!". "\n"; print "\n";

Line999:
exit;



__DATA__
A
B
C
D
E
F
G
H
I
J
K
L
M
N
O
P
1
2
3
4
