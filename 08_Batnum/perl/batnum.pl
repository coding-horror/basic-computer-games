use strict;
use warnings;

my %WIN_OPTIONS = (
    "UNDEFINED" => 0,
    "TAKE_LAST" => 1,
    "AVOID_LAST" => 2,
);

my %START_OPTIONS = (
    "UNDEFINED" => 0,
    "COMPUTER_FIRST" => 1,
    "PLAYER_FIRST" => 2,
);

sub run {
    my $pile_size = undef;
    my $min_select = undef;
    my $max_select = undef;
    my $win_option = undef;
    my $start_option = undef;

    # while (1) {
        write_intro();

        ($pile_size, $min_select, $max_select, $win_option, $start_option) 
            = &get_user_input();

        play($pile_size, $min_select, $max_select, $win_option, $start_option);
    # }
}

sub write_intro {
    printf "%33s", "BATNUM\n";
    printf "%15s", "CREATIVE COMPUTING  MORRISSTOWN, NEW JERSEY\n";
    printf "%s", "\n";
    printf "%s", "\n";
    printf "%s", "\n";
    printf "%s", "THIS PROGRAM IS A 'BATTLE OF NUMBERS' GAME, WHERE THE\n";
    printf "%s", "COMPUTER IS YOUR OPPONENT.\n";
    printf "%s", "\n";
    printf "%s", "THE GAME STARTS WITH AN ASSUMED PILE OF OBJECTS. YOU\n";
    printf "%s", "AND YOUR OPPONENT ALTERNATELY REMOVE OBJECTS FROM THE PILE.\n";
    printf "%s", "WINNING IS DEFINED IN ADVANCE AS TAKING THE LAST OBJECT OR\n";
    printf "%s", "NOT. YOU CAN ALSO SPECIFY SOME OTHER BEGINNING CONDITIONS.\n";
    printf "%s", "DON'T USE ZERO, HOWEVER, IN PLAYING THE GAME.\n";
    printf "%s", "ENTER A NEGATIVE NUMBER FOR NEW PILE SIZE TO STOP PLAYING.\n";
    printf "%s", "\n";
}

# TODO: check input
sub get_user_input {
    my $pile_size = 0;
    my $min_select = 0;
    my $max_select = 0;
    my $win_option = $WIN_OPTIONS{ "UNDEFINED" };
    my $start_option = $START_OPTIONS{ "UNDEFINED" };

    while ($pile_size < 1) {
        printf "%s", "ENTER PILE SIZE: ";
        $pile_size = <>;
        $pile_size *= 1;
    }

    while ($win_option eq $WIN_OPTIONS{ "UNDEFINED" }) {
        printf "%s", "ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST: ";
        $win_option = <>;
        $win_option *= 1;
    }

    # get min select max select

    while ($start_option eq $START_OPTIONS{ "UNDEFINED" }) {
        printf "%s", "ENTER START OPTION - 1 COMPUTER FIRST, 2 YOU FIRST: ";
        $start_option = <>;
        $start_option *= 1;
    }

    return ($pile_size, $min_select, $max_select, $win_option, $start_option);
}

sub play {
    my $pile_size = shift;
    my $min_select = shift;
    my $max_select = shift;
    my $win_option = shift;
    my $start_option = shift;

    print("pile_size: $pile_size\n");
    print("win_option: $win_option\n");
}

run();
