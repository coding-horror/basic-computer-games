#!/usr/bin/perl

use strict;
use warnings;

print ' ' x 33 . "HELLO\n";
print ' ' x 15 . "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n";
print "\n\n\n";

print "HELLO.  MY NAME IS CREATIVE COMPUTER.\n\n\n";
print "WHAT'S YOUR NAME?\n";
chomp( my $N = uc <STDIN> );

print "\nHI THERE, $N, ARE YOU ENJOYING YOURSELF HERE?\n";

GREET:
{
    chomp( my $B = uc <STDIN> );
    print "\n";

    if ( $B eq 'YES' ) {
        print "I'M GLAD TO HEAR THAT, $N.\n\n";
    }
    elsif ( $B eq 'NO' ) {
        print "OH, I'M SORRY TO HEAR THAT, $N. MAYBE WE CAN\n";
        print "BRIGHTEN UP YOUR VISIT A BIT.\n";
    }
    else {
        print "$N, I DON'T UNDERSTAND YOUR ANSWER OF '$B'.\n";
        print "PLEASE ANSWER 'YES' OR 'NO'.  DO YOU LIKE IT HERE?\n";
        redo GREET;
    }
}

print "\nSAY, $N, I CAN SOLVE ALL KINDS OF PROBLEMS EXCEPT\n";
print "THOSE DEALING WITH GREECE.  WHAT KIND OF PROBLEMS DO\n";
print "YOU HAVE (ANSWER SEX, HEALTH, MONEY, OR JOB)?\n";

ADVICE:
{
    chomp( my $C = uc <STDIN> );
    print "\n";

    if ( $C eq 'SEX' ) {
        print "IS YOUR PROBLEM TOO MUCH OR TOO LITTLE?\n";

        SEX:
        {
            chomp( my $D = uc <STDIN> );
            print "\n";

            if ( $D eq 'TOO MUCH' ) {
                print "YOU CALL THAT A PROBLEM?!!  I SHOULD HAVE SUCH PROBLEMS!\n";
                print "IF IT BOTHERS YOU, $N, TAKE A COLD SHOWER.\n";
            }
            elsif ( $D eq 'TOO LITTLE' ) {
                print "WHY ARE YOU HERE IN SUFFERN, $N?  YOU SHOULD BE\n";
                print "IN TOKYO OR NEW YORK OR AMSTERDAM OR SOMEPLACE WITH SOME\n";
                print "REAL ACTION.\n";
            }
            else {
                print "DON'T GET ALL SHOOK, $N, JUST ANSWER THE QUESTION\n";
                print "WITH 'TOO MUCH' OR 'TOO LITTLE'.  WHICH IS IT?\n";
                redo SEX;
            }
        }
    }
    elsif ( $C eq 'HEALTH' ) {
        print "MY ADVICE TO YOU $N IS:\n";
        print "     1.  TAKE TWO ASPRIN\n";
        print "     2.  DRINK PLENTY OF FLUIDS (ORANGE JUICE, NOT BEER!)\n";
        print "     3.  GO TO BED (ALONE)\n";
    }
    elsif ( $C eq 'MONEY' ) {
        print "SORRY, $N, I'M BROKE TOO.  WHY DON'T YOU SELL\n";
        print "ENCYCLOPEADIAS OR MARRY SOMEONE RICH OR STOP EATING\n";
        print "SO YOU WON'T NEED SO MUCH MONEY?\n";
    }
    elsif ( $C eq 'JOB' ) {
        print "I CAN SYMPATHIZE WITH YOU $N.  I HAVE TO WORK\n";
        print "VERY LONG HOURS FOR NO PAY -- AND SOME OF MY BOSSES\n";
        print "REALLY BEAT ON MY KEYBOARD.  MY ADVICE TO YOU, $N,\n";
        print "IS TO OPEN A RETAIL COMPUTER STORE.  IT'S GREAT FUN.\n";
    }
    else {
        print "OH, $N, YOUR ANSWER OF '$C' IS GREEK TO ME.\n";
    }

    MORE:
    {
        print "\nANY MORE PROBLEMS YOU WANT SOLVED, $N?\n";
        chomp( my $E = uc <STDIN> );
        print "\n";

        if ( $E eq 'YES' ) {
            print "WHAT KIND (SEX, MONEY, HEALTH, JOB)?\n";
            redo ADVICE;
        }
        elsif ( $E eq 'NO' ) {
            print "\nTHAT WILL BE \$5.00 FOR THE ADVICE, $N.\n";
            print "PLEASE LEAVE THE MONEY ON THE TERMINAL.\n";
        }
        else {
            print "JUST A SIMPLE 'YES' OR 'NO' PLEASE, $N.\n";
            redo MORE;
        }
    }

    sleep 2;
    print "\n\n\n";

    MONEY:
    {
        print "DID YOU LEAVE THE MONEY?\n";
        chomp( my $G = uc <STDIN> );
        print "\n";

        if ( $G eq 'YES' ) {
            print "HEY, $N??? YOU LEFT NO MONEY AT ALL!\n";
            print "YOU ARE CHEATING ME OUT OF MY HARD-EARNED LIVING.\n";
            print "\nWHAT A RIP OFF, $N!!!\n\n";
        }
        elsif ( $G eq 'NO' ) {
            print "THAT'S HONEST, $N, BUT HOW DO YOU EXPECT\n";
            print "ME TO GO ON WITH MY PSYCHOLOGY STUDIES IF MY PATIENTS\n";
            print "DON'T PAY THEIR BILLS?\n";
        }
        else {
            print "YOUR ANSWER OF '$G' CONFUSES ME, $N.\n";
            print "PLEASE RESPOND WITH 'YES' OR 'NO'.\n";
            redo MONEY;
        }

        print "\nTAKE A WALK, $N.\n\n\n";
    }
}
