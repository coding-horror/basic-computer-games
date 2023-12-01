use std::io::{self, Write};

/// Print out the introduction and rules for the game.
fn print_intro() {
    println!();
    println!();
    println!("{:>33}", "BATNUM");
    println!("{:>15}", "CREATIVE COMPUTING  MORRISSTOWN, NEW JERSEY");
    println!();
    println!();
    println!();
    println!("THIS PROGRAM IS A 'BATTLE OF NUMBERS' GAME, WHERE THE");
    println!("COMPUTER IS YOUR OPPONENT.");
    println!();
    println!("THE GAME STARTS WITH AN ASSUMED PILE OF OBJECTS. YOU");
    println!("AND YOUR OPPONENT ALTERNATELY REMOVE OBJECTS FROM THE PILE.");
    println!("WINNING IS DEFINED IN ADVANCE AS TAKING THE LAST OBJECT OR");
    println!("NOT. YOU CAN ALSO SPECIFY SOME OTHER BEGINNING CONDITIONS.");
    println!("DON'T USE ZERO, HOWEVER, IN PLAYING THE GAME.");
    println!("ENTER A NEGATIVE NUMBER FOR NEW PILE SIZE TO STOP PLAYING.");
    println!();
}

/// This requests the necessary parameters to play the game.
/// five game parameters:
/// * pile_size - the starting size of the object pile
/// * min_select - minimum selection that can be made on each turn
/// * max_select - maximum selection that can be made on each turn
/// * start_option - computer first or player first
/// * win_option - goal is to take the last object
///                or the goal is to not take the last object
struct Params {
    pub pile_size: usize,
    pub min_select: usize,
    pub max_select: usize,
    pub start_option: StartOption,
    pub win_option: WinOption,
}

#[derive(PartialEq, Eq)]
enum StartOption {
    ComputerFirst,
    PlayerFirst,
}

#[derive(PartialEq, Eq)]
enum WinOption {
    TakeLast,
    AvoidLast,
}

impl Params {
    pub fn get_params() -> Self {
        let pile_size = Self::get_pile_size();
        let (min_select, max_select) = Self::get_min_max();
        let start_option = Self::get_start_option();
        let win_option = Self::get_win_option();

        Self {
            pile_size,
            min_select,
            max_select,
            start_option,
            win_option,
        }
    }

    fn get_pile_size() -> usize {
        print!("ENTER PILE SIZE ");
        let _ = io::stdout().flush();
        read_input_integer()
    }

    fn get_win_option() -> WinOption {
        print!("ENTER WIN OPTION: 1 TO TAKE LAST, 2 TO AVOID LAST: ");
        let _ = io::stdout().flush();

        loop {
            match read_input_integer() {
                1 => {
                    return WinOption::TakeLast;
                }
                2 => {
                    return WinOption::AvoidLast;
                }
                _ => {
                    print!("Please enter 1 or 2 ");
                    let _ = io::stdout().flush();
                    continue;
                }
            }
        }
    }

    fn get_start_option() -> StartOption {
        print!("ENTER START OPTION: 1 COMPUTER FIRST, 2 YOU FIRST ");
        let _ = io::stdout().flush();

        loop {
            match read_input_integer() {
                1 => {
                    return StartOption::ComputerFirst;
                }
                2 => {
                    return StartOption::PlayerFirst;
                }
                _ => {
                    print!("Please enter 1 or 2 ");
                    let _ = io::stdout().flush();
                    continue;
                }
            }
        }
    }

    fn get_min_max() -> (usize, usize) {
        print!("ENTER MIN ");
        let _ = io::stdout().flush();
        let min = read_input_integer();

        print!("ENTER MAX ");
        let _ = io::stdout().flush();
        let max = read_input_integer();

        (min, max)
    }
}

fn read_input_integer() -> usize {
    loop {
        let mut input = String::new();
        io::stdin()
            .read_line(&mut input)
            .expect("Failed to read line");
        match input.trim().parse::<usize>() {
            Ok(num) => {
                if num == 0 {
                    print!("Must be greater than zero ");
                    let _ = io::stdout().flush();
                    continue;
                }
                return num;
            }
            Err(_err) => {
                print!("Please enter a number greater than zero ");
                let _ = io::stdout().flush();
                continue;
            }
        }
    }
}

fn player_move(pile_size: &mut usize, params: &Params) -> bool {
    loop {
        print!("YOUR MOVE ");
        let _ = io::stdout().flush();

        let player_move = read_input_integer();
        if player_move == 0 {
            println!("I TOLD YOU NOT TO USE ZERO!  COMPUTER WINS BY FORFEIT.");
            return true;
        }
        if player_move > params.max_select || player_move < params.min_select {
            println!("ILLEGAL MOVE, REENTER IT");
            continue;
        }
        *pile_size -= player_move;
        if *pile_size == 0 {
            if params.win_option == WinOption::AvoidLast {
                println!("TOUGH LUCK, YOU LOSE.");
            } else {
                println!("CONGRATULATIONS, YOU WIN.")
            }
            return true;
        }
        return false;
    }
}

fn computer_pick(pile_size: usize, params: &Params) -> usize {
    let q = if params.win_option == WinOption::AvoidLast {
        pile_size - 1
    } else {
        pile_size
    };
    let c = params.min_select + params.max_select;
    let computer_pick = q - (c * (q / c));
    let computer_pick = if computer_pick < params.min_select {
        params.min_select
    } else {
        computer_pick
    };
    if computer_pick > params.max_select {
        params.max_select
    } else {
        computer_pick
    }
}

fn computer_move(pile_size: &mut usize, params: &Params) -> bool {
    if params.win_option == WinOption::TakeLast && *pile_size <= params.max_select {
        println!("COMPUTER TAKES {pile_size} AND WINS.");
        return true;
    }
    if params.win_option == WinOption::AvoidLast && *pile_size >= params.min_select {
        println!("COMPUTER TAKES {} AND LOSES.", params.min_select);
        return true;
    }

    let curr_sel = computer_pick(*pile_size, params);
    *pile_size -= curr_sel;
    println!("COMPUTER TAKES {curr_sel} AND LEAVES {pile_size}");
    false
}

fn play_game(params: &Params) {
    let mut pile_size = params.pile_size;

    if params.start_option == StartOption::ComputerFirst && computer_move(&mut pile_size, params) {
        return;
    }

    loop {
        if player_move(&mut pile_size, params) {
            return;
        }
        if computer_move(&mut pile_size, params) {
            return;
        }
    }
}

fn main() -> ! {
    loop {
        print_intro();
        let params = Params::get_params();
        play_game(&params);
    }
}
