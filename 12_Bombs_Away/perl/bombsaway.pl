#!/usr/bin/env perl
use v5.24;
use warnings;
use experimental 'signatures';
no warnings 'experimental::signatures';

exit main(@ARGV);

sub main {
   $|++;
   my $mission = 'y';

   # first-level choices will allow us to select the "right" callback
   # function to start each mission
   my @choices = (\&italy, \&allies, \&japan, \&germany);

   # to support being case-insensitive "the right way" we apply the fc()
   # function (i.e. "fold case"). This is slightly overkill in this case
   # but it's better to stick to good habits.
   while (fc($mission // 'n') eq fc('y')) {
      say 'YOU ARE A PILOT IN A WORLD WAR II BOMBER.';

      my $side = choose(
         'WHAT SIDE -- ITALY(1), ALLIES(2), JAPAN(3), GERMANY(4)', 4);

      # arrays start from 0 in Perl, so our starting-from-1 side value
      # has to be offset by 1.
      $choices[$side - 1]->();

      $mission = get_input("\n\n\nANOTHER MISSION (Y OR N)");
   }
   __exit();
}

# unified exit function, make sure to shame the desertor!
sub __exit ($prefix = '') {
   say $prefix, "CHICKEN !!!\n";
   exit 0;
}

# unified input gathering. Checks if the input is closed (e.g. because the
# player hit CTRL-D) and __exit()s in case. Gets a prompt for asking a
# question, returns whatever is input (except spaces).
sub get_input ($prompt) {
   print "$prompt? ";
   defined(my $input = <STDIN>) or __exit("\n");

   # remove spaces from the input (including newlines), they are not used

   $input =~ s{\s+}{}gmxs;
   return $input;
}

# structured choosing function, gets a $prompt for asking a question and
# will iterate asking until the input is a number between 1 and $n_max.
sub choose ($prompt, $n_max) {
   while ('necessary') {
      my $side = get_input($prompt);
      return $side if $side =~ m{\A [1-9]\d* \z}mxs && $side <= $n_max;
      say 'TRY AGAIN...';
   }
}

# Italy mission has the same structure as Allies and Germany, so it's been
# refactored into a single "multiple()" (pun intended) function, providing
# the right messaging.
sub italy {
   return multiple(
      'YOUR TARGET -- ALBANIA(1), GREECE(2), NORTH AFRICA(3)',
      q{SHOULD BE EASY -- YOU'RE FLYING A NAZI-MADE PLANE.},
      'BE CAREFUL!!!',
      q{YOU'RE GOING FOR THE OIL, EH?},
   );
}


# Allies mission has the same structure as Italy and Germany, so it's been
# refactored into a single "multiple()" (pun intended) function, providing
# the right messaging.
sub allies {
   return multiple(
      'AIRCRAFT -- LIBERATOR(1), B-29(2), B-17(3), LANCASTER(4)',
      q{YOU'VE GOT 2 TONS OF BOMBS FLYING FOR PLOESTI.},
      q{YOU'RE DUMPING THE A-BOMB ON HIROSHIMA.},
      q{YOU'RE CHASING THE BISMARK IN THE NORTH SEA.},
      q{YOU'RE BUSTING A GERMAN HEAVY WATER PLANT IN THE RUHR.},
   );
}

# Japan mission is different from the other three and is coded...
# differently. The end game phases are the same as other missions though,
# hence the calls to "direct_hit()" and "endgame()" functions.
sub japan {
   say q{YOU'RE FLYING A KAMIKAZE MISSION OVER THE USS LEXINGTON.};
   my $is_first_kamikaze = get_input(q{YOUR FIRST KAMIKAZE MISSION(Y OR N)});
   if (fc($is_first_kamikaze) eq fc('n')) {
      our $guns_hit_rate = 0;
      say '';
      return endgame();
   }
   return direct_hit() if rand(1) > 0.65;
   return endgame('fail');
}

# Germany mission has the same structure as Italy and Allies, so it's been
# refactored into a single "multiple()" (pun intended) function, providing
# the right messaging.
sub germany {
   return multiple(
      "A NAZI, EH?  OH WELL.  ARE YOU GOING FOR RUSSIA(1),\n"
         . 'ENGLAND(2), OR FRANCE(3)',
      q{YOU'RE NEARING STALINGRAD.},
      q{NEARING LONDON.  BE CAREFUL, THEY'VE GOT RADAR.},
      q{NEARING VERSAILLES.  DUCK SOUP.  THEY'RE NEARLY DEFENSELESS.}
   );
}

# This function implements the workhorse for Italy, Allies and Germany
# missions, which all have the same structure. It starts with a $question
# and a few @comments, each commenting every different answer to the
# $question.
sub multiple ($question, @comments) {
   my $target = choose($question, scalar @comments);
   say "\n", $comments[$target - 1], "\n";

   # we gather the number of missions flown so far so that we can
   # use it to figure out if *this* mission will be successful. The more
   # the missions flown, the higher the probability of success.
   my $missions;
   while ('necessary') {
      $missions = get_input('HOW MANY MISSIONS HAVE YOU FLOWN');
      last if $missions < 160;
      print 'MISSIONS, NOT MILES...
150 MISSIONS IS HIGH EVEN FOR OLD-TIMERS.
NOW THEN, ';
   }
   say '';

   # a little intermediate comment based on the value of $missions
   if ($missions < 25) { say "FRESH OUT OF TRANING, EH?\n" }
   elsif ($missions >= 100) { say "THAT'S PUSHING THE ODDS!\n" }

   # let's roll a 160-faced die and compare to the missions flown so far,
   # player might not even have to engage in combat!
   return direct_hit() if $missions >= rand(160);

   # player didn't get a direct hit on the target, so we provide a
   # feedback about how much it was apart. This is part of the story.
   my $miss = 2 + int rand(30);
   say "MISSED TARGET BY $miss MILES!";
   say "NOW YOU'RE REALLY IN FOR IT !!\n";

   # here is where the game shows a little "weakness", although it might
   # have been done on purpose. We use "our" variables $missiles_hit_rate
   # and $guns_hit_rate here because the original BASIC code did not reset
   # the associated variables (respectively T and S) at every mission, thus
   # leaking state from one mission to the following ones.
   # 
   # In particular, both are leaked to the Japan mission(s), and
   # $guns_hit_rate is leaked to future "multiple()" missions that have
   # missiles only.
   #
   # This is what you get when your language only has global variables.
   #
   # Of course, this might have been done on purpose, and we'll replicate
   # this behaviour here because it adds some randomness to the game.
   our $missiles_hit_rate = 0;
   my $response = choose(
      'DOES THE ENEMY HAVE GUNS(1), MISSILES(2), OR BOTH(3)', 3);

   # Apply Gun damage for responses 1 and 3
   if ($response != 2) { # there's some guns involved, ask more
      say '';

      # see comment above as to why we have a "our" variable here
      our $guns_hit_rate =
         get_input(q{WHAT'S THE PERCENT HIT RATE OF ENEMY GUNNERS (10 TO 50)});

      # let's normalize the input a bit
      $guns_hit_rate = 0 unless $guns_hit_rate =~ m{\A [1-9]\d* \z}mxs;

      # a hit rate this low is not reasonable and is immediately punished!
      if ($guns_hit_rate < 10) {
         say q{YOU LIE, BUT YOU'LL PAY...};

         # function endgame() provides the... end game messaging, which is
         # also used by the Japan mission, so it's been factored out.
         # Passing 'fail' (or any true value) makes sure that is' a
         # failure.
         return endgame('fail'); # sure failure
      }
      say '';
   }
   # Apply missile damage for responses 2 and 3
   if ($response > 1 )  {
      $missiles_hit_rate = 35;  # remember... this is a global variable
   }

   # hand control over to the "endgame()" refactored function (also shared
   # by the Japan mission).
   return endgame();
}

sub direct_hit {
   my $killed = int rand(100);
   say "DIRECT HIT!!!!  $killed KILLED.\nMISSION SUCCESSFUL";
   return;
}

# This function provides the end game randomization and messages, shared
# across all missions. If passed a true value $fail, it will make sure that
# the outcome is... a failure. This allows coping with a few ad-hoc
# GOTO:s in the original BASIC code, while still preserving a refactored
# code.
sub endgame ($fail = 0) {
   our $missiles_hit_rate //= 0;
   our $guns_hit_rate //= 0;
   $fail ||= ($missiles_hit_rate + $guns_hit_rate) > rand(100);
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
