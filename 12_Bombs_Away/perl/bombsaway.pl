#!/usr/bin/env perl
use v5.24;
use warnings;
use experimental 'signatures';
no warnings 'experimental::signatures';

exit main(@ARGV);

sub main {
   $|++;
   my $mission = 'y';

   my @choices = (
      { # 1 - Italy
         ask => 'YOUR TARGET -- ALBANIA(1), GREECE(2), NORTH AFRICA(3)',
         comments => [
            q{SHOULD BE EASY -- YOU'RE FLYING A NAZI-MADE PLANE.},
            'BE CAREFUL!!!',
            q{YOU'RE GOING FOR THE OIL, EH?},
         ],
      },
      { # 2 - Allies
         ask => 'AIRCRAFT -- LIBERATOR(1), B-29(2), B-17(3), LANCASTER(4)',
         comments => [
            q{YOU'VE GOT 2 TONS OF BOMBS FLYING FOR PLOESTI.},
            q{YOU'RE DUMPING THE A-BOMB ON HIROSHIMA.},
            q{YOU'RE CHASING THE BISMARK IN THE NORTH SEA.},
            q{YOU'RE BUSTING A GERMAN HEAVY WATER PLANT IN THE RUHR.},
         ],
      },
      \&japan,
      { # 4 - Germany
         ask => "A NAZI, EH?  OH WELL.  ARE YOU GOING FOR RUSSIA(1),\n"
            . 'ENGLAND(2), OR FRANCE(3)',
         comments => [
            q{YOU'RE NEARING STALINGRAD.},
            q{NEARING LONDON.  BE CAREFUL, THEY'VE GOT RADAR.},
            q{NEARING VERSAILLES.  DUCK SOUP.  THEY'RE NEARLY DEFENSELESS.},
         ],
      },
   );

   while (fc($mission // 'n') eq fc('y')) {
      say 'YOU ARE A PILOT IN A WORLD WAR II BOMBER.';

      my $side = choose(
         'WHAT SIDE -- ITALY(1), ALLIES(2), JAPAN(3), GERMANY(4)? ', 4);
      my $choice = $choices[$side - 1];
      ref($choice) eq 'HASH' ? multiple($choice) : $choice->();

      print "\n\n\nANOTHER MISSION (Y OR N)? ";
      chomp($mission = <STDIN>);
   }
   say "CHICKEN !!!\n";
   return 0;
}

sub choose ($prompt, $n_max) {
   while ('necessary') {
      print "$prompt? ";
      chomp(my $side = <STDIN>);
      return $side if $side =~ m{\A [1-9]\d* \z}mxs && $side <= $n_max;
      say 'TRY AGAIN...';
   }
}

sub multiple ($spec) {
   my $target = choose("$spec->{ask}? ", scalar $spec->{comments}->@*);
   say $spec->{comments}->[$target - 1];
   say '';

   my $missions;
   while ('necessary') {
      print 'HOW MANY MISSIONS HAVE YOU FLOWN? ';
      chomp($missions = <STDIN>);
      last if $missions < 160;
      print 'MISSIONS, NOT MILES...
150 MISSIONS IS HIGH EVEN FOR OLD-TIMERS.
NOW THEN, ';
   }
   if ($missions < 25) { say 'FRESH OUT OF TRANING, EH?' }
   elsif ($missions >= 100) { say q{THAT'S PUSHING THE ODDS!} }

   return direct_hit() if $missions >= rand(160);

   my $miss = 2 + int rand(30);
   say "MISSED TARGET BY $miss MILES!";
   say "NOW YOU'RE REALLY IN FOR IT !!\n";
   our $double_fire = 0;
   my $response = choose(
      'DOES THE ENEMY HAVE GUNS(1), MISSILES(2), OR BOTH(3)', 3);
   if ($response < 2) {
      print q{WHAT'S THE PERCENT HIT RATE OF ENEMY GUNNERS (10 TO 50)? };
      chomp (our $hit_rate = <STDIN>);
      if ($hit_rate < 10) {
         say q{YOU LIE, BUT YOU'LL PAY...};
         return endgame('fail'); # sure failure
      }
      say '';
   }
   else {
      $double_fire = 35;
   }
   return endgame();
}

sub direct_hit {
   my $killed = int rand(100);
   say "DIRECT HIT!!!!  $killed KILLED.\nMISSION SUCCESSFUL";
   return;
}

sub endgame ($fail = 0) {
   our $double_fire //= 0;
   our $hit_rate //= 0;
   $fail ||= ($double_fire + $hit_rate) > rand(100);
   if ($fail) {
      say '* * * * BOOM * * * *
YOU HAVE BEEN SHOT DOWN.....
DEARLY BELOVED, WE ARE GATHERED HERE TODAY TO PAY OUR
LAST TRIBUTE...';
   }
   else {
      say 'YOU MADE IT THROUGH TREMENDOUS FLAK!!';
   }
   return;
}

sub japan {
   say q{YOU'RE FLYING A KAMIKAZE MISSION OVER THE USS LEXINGTON.};
   print q{YOUR FIRST KAMIKAZE MISSION(Y OR N)? };
   chomp(my $is_first_kamikaze = <STDIN>);
   if (fc($is_first_kamikaze) eq fc('n')) {
      our $hit_rate = 0;
      say '';
      return endgame();
   }
   return direct_hit() if rand(1) > 0.65;
   return endgame('fail');
}
