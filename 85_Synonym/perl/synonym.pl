#!/usr/bin/perl

use v5.32; # for sample from List::Util, also includes 'use strict'
use warnings; # always a good idea

use List::Util qw/ any sample shuffle /; # Rather than write our own utilities, use the built in ones

my @correct = qw/ Right Correct Fine Good! Check /;

# lowercase all words here
my @synonyms = (
  [ qw/ first start beginning onset initial / ],
  [ qw/ similar alike same like resembling / ],
  [ qw/ model pattern prototype standard criterion /],
  [ qw/ small insignificant little tiny minute /],
  [ qw/ stop halt stay arrest check standstill /],
  [ qw/ house dwelling residense domicile lodging habitation /],
  [ qw/ pit hole hollow well gulf chasm abyss /],
  [ qw/ push shove thrust prod poke butt press /],
  [ qw/ red rouge scarlet crimson flame ruby /],
  [ qw/ pain suffering hurt misery distress ache discomfort /],
);

print <<__END_OF_INTRO;
                                 Synonym
                Creative Computing  Morristown, New Jersey



A synonym of a word means another word in the English
language which has the same or very nearly the same meaning
I choose a word -- you type a synonym.
If you can't think of a synonym, type the word 'HELP'
and I will tell you a synonym.

__END_OF_INTRO

foreach my $drill ( shuffle @synonyms ) {
  my $word = $drill->[0];
  my @answers = $drill->@[1 .. $drill->$#*];
  print "     What is a synonym of $word? ";
  my $response = <>;
  chomp $response;
  $response = lc $response;

  if ( $response eq 'help' ) {
    say "**** A synonym of $word is ", sample(1, @answers);
    redo;
  } elsif ( not any { $response eq $_ } @answers ) {
    say '     Try again.';
    redo;
  } else {
    say sample 1, @correct;
  }
}

say "\nSynonym drill completed.";

