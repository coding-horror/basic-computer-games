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

    while (1) {
        write_intro();

        ($pile_size, $min_select, $max_select, $win_option, $start_option) 
            = &get_user_input();

        play($pile_size, $min_select, $max_select, $win_option, $start_option);

        printf "\n";
    }
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


sub get_user_input {
    my $pile_size = 0;
    my $min_select = 0;
    my $max_select = 0;
    my $win_option = $WIN_OPTIONS{ "UNDEFINED" };
    my $start_option = $START_OPTIONS{ "UNDEFINED" };

    while ($pile_size < 1) {
        printf "%s", "ENTER PILE SIZE: ";
        $pile_size = <STDIN>;
    }

    while ($min_select < 1 || $max_select < 1 || $min_select > $max_select) {
        printf "%s", "ENTER MIN AND MAX: ";
        my $raw_input = <STDIN>;
        ($min_select, $max_select) = split(' ', $raw_input);
    }

    while ($win_option eq $WIN_OPTIONS{ "UNDEFINED" }) {
        printf "%s", "ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST: ";
        $win_option = <STDIN>;
    }

    while ($start_option eq $START_OPTIONS{ "UNDEFINED" }) {
        printf "%s", "ENTER START OPTION - 1 COMPUTER FIRST, 2 YOU FIRST: ";
        $start_option = <STDIN>;
    }

    return ($pile_size, $min_select, $max_select, $win_option, $start_option);
}


sub play {
    my $pile_size = shift;
    my $min_select = shift;
    my $max_select = shift;
    my $win_option = shift;
    my $start_option = shift;

    my $game_over = 0;
    my $players_turn = $start_option eq $START_OPTIONS{ "PLAYER_FIRST" } ? 1 : 0;

    while (!$game_over) {
        if ($players_turn) {
            ($game_over, $pile_size) = players_move(
                $pile_size, $min_select, $max_select, $win_option);
            
            $players_turn = 0;

            if ($game_over) {
                return;
            }
        } else {
            ($game_over, $pile_size) = computers_move(
                $pile_size, $min_select, $max_select, $win_option);
            
            $players_turn = 1;
        }
    }

    return;
}


sub players_move {
    my $pile_size = shift;
    my $min_select = shift;
    my $max_select = shift;
    my $win_option = shift;

    my $finished = 0;

    while (!$finished) {
        my $remove_amount = undef;

        printf "%s", "YOUR MOVE: ";
        $remove_amount = <>;

        if ($remove_amount eq 0) {
            printf "%s", "I TOLD YOU NOT TO USE ZERO!  COMPUTER WINS BY FORFEIT.";
            return (1, $pile_size);
        } elsif ($remove_amount > $max_select || $remove_amount < $min_select) {
            printf "%s", "ILLEGAL MOVE, TRY AGAIN.\n";
            next;
        } else {
            $pile_size -= $remove_amount;
            $finished = 1;
        }

        if ($pile_size <= 0) {
            if ($win_option eq $WIN_OPTIONS{ "AVOID_LAST" }) {
                printf "%s", "TOUGH LUCK, YOU LOSE.\n";
            } else {
                printf "%s", "CONGRATULATIONS, YOU WIN.\n";
            }
            return (1, $pile_size);
        }
    }
    
    return (0, $pile_size);
}


sub computers_move {
    my $pile_size = shift;
    my $min_select = shift;
    my $max_select = shift;
    my $win_option = shift;

    if ($win_option eq $WIN_OPTIONS{ "TAKE_LAST" } && $pile_size <= $max_select) {
        printf "COMPUTER TAKES %d AND WINS.\n", $pile_size;
        return (1, $pile_size);
    }

    if ($win_option eq $WIN_OPTIONS{ "AVOID_LAST" } && $pile_size <= $min_select) {
        printf "COMPUTER TAKES %d AND LOSES.\n", $pile_size;
        return (1, $pile_size);
    }

    my $remove_amount = get_computer_remove_amount($min_select, $max_select);

    $pile_size -= $remove_amount;

    printf "COMPUTER TAKES %d AND LEAVES %d.\n", $remove_amount, $pile_size;

    return (0, $pile_size);

}


sub get_computer_remove_amount {
    my $min_select = shift;
    my $max_select = shift;

    return (int(rand($max_select - $min_select)) + $min_select);
}


run();
