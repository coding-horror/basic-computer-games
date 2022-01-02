#!/usr/bin/perl

use v5.24; # for say and use strict
use warnings;

sub get_pennies {
  my $query = shift;

  print "$query? ";
  my $in = <>;
  chomp $in;
  $in =~ /([\d.]+)/; # the first match of digits and decimal points
  return int( $1 * 100 );
}

sub make_change {
  my $change = shift;

  state %change_options = (
    'Penny' => { value => 1, plural => 'Pennies' },
    'Nickel' => { value => 5 },
    'Dime' => { value => 10 },
    'Quarter' => { value => 25 },
    'One Half Dollar' => { value => 50 },
    'One Dollar Bill' => { value => 100 * 1 },
    'Five Dollar Bill' => { value => 100 * 5 },
    '10 Dollar Bill' => { value => 100 * 10 },
  );

  foreach my $unit ( sort { $change_options{$b}->{value} <=> $change_options{$a}->{value} } keys %change_options ) {
    my $value = $change_options{$unit}->{value};
    next if $value > $change;
    my $number = int( $change / $value );
    if ( $number > 1 ) {
      $unit = exists $change_options{$unit}->{plural} ? $change_options{$unit}->{plural} : "${unit}s";
    }
    say "$number $unit";
    $change -= $number * $value;
  }
}

print <<'__END_OF_INTRO';
                              Change
               Creative Computing  Morristown, New Jersey


I, Your friendly microcomputer, will determine
the correct change for items costing up to $100.


__END_OF_INTRO

while ( 1 ) {
  my $cost = get_pennies( 'Cost of item' );
  my $payment = get_pennies( 'Amount of payment');

  my $change = $payment - $cost;
  my $change_formatted = sprintf( "%.2f", $change / 100 );
  if ( $change == 0 ) {
    say 'Correct amount, thank you.';
  } elsif ( $change < 0 ) {
    say 'Sorry, you have short-changed me $', abs($change_formatted);
  } else {
    say 'Your change, $', $change_formatted;
    make_change( $change );
    say "Thank you, come again\n\n";
  }
}
