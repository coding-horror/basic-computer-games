#!/usr/bin/perl
use strict;


print ' 'x 33 . "KINEMA\n";
print ' 'x 15 . "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";

while (1) {
	print "\n";
	print "\n";
	my $Q=0;
	my $V=5+int(35*rand(1));
	print "A BALL IS THROWN UPWARDS AT $V METERS PER SECOND.\n";
	print "\n";

	my $A=.05*$V^2;
	print "HOW HIGH WILL IT GO (IN METERS)";
	$Q+= &Input($A);

	$A=$V/5;
	print "HOW LONG UNTIL IT RETURNS (IN SECONDS)";
	$Q+= &Input($A);

	my $T=1+int(2*$V*rand(1))/10;
	$A=$V-10*$T;
	print "WHAT WILL ITS VELOCITY BE AFTER $T SECONDS";
	$Q+= &Input($A);

	print "\n";
	print "$Q RIGHT OUT OF 3.";
	if ($Q<2) { next; }
	print " NOT BAD.\n";
	}

exit;


#Line500:
sub Input {
	my ($A)= @_;
	my $Point=0;
	print "? "; chomp(my $G = <STDIN>);
	if (abs(($G-$A)/$A)<.15) {
		print "CLOSE ENOUGH.\n";
		$Point=1;
		} else {
		print "NOT EVEN CLOSE....\n";
		}
	print "CORRECT ANSWER IS $A\n";
	print "\n";
	return $Point;
	}
