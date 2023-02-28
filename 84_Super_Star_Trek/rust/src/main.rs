use std::{io::{stdin, stdout, Write}, process::exit, str::FromStr};

use model::Galaxy;

use crate::text_constants::BAD_NAV;

mod model;
mod commands;
mod text_constants;

fn main() {
    ctrlc::set_handler(move || { exit(0) })
    .expect("Error setting Ctrl-C handler");

    let mut galaxy = Galaxy::generate_new();
    // init ops, starting state and notes
    commands::short_range_scan(&galaxy);

    loop {
        match prompt("Command?").as_str() {
            "SRS" => commands::short_range_scan(&galaxy),
            "NAV" => gather_dir_and_speed_then_move(&mut galaxy),
            _ => print_command_help()
        }

        // process the next command, based on it render something or update the galaxy or whatever
        // this would be: read command, and based on it run dedicated function
        // the function might get passed a mutable reference to the galaxy
    }
}

fn gather_dir_and_speed_then_move(galaxy: &mut Galaxy) {
    let dir = prompt_value::<u8>("Course (1-9)?", 1, 9);
    if dir.is_none() {
        println!("{}", BAD_NAV); 
        return;
    }

    let speed = prompt_value::<f32>("Course (1-9)?", 0.0, 8.0);
    if speed.is_none() {
        println!("{}", BAD_NAV); 
        return;
    }

    let distance = (speed.unwrap() * 8.0) as i32;
    // could be done with a step function - while distance > 0, move by digit.
    // if passing a boundary, test for the next quadrant in that direction
    // if present, change quadrant and move to border
    // else stop.
}

fn prompt(prompt_text: &str) -> String {
    let stdin = stdin();
    let mut stdout = stdout();

    print!("{prompt_text} ");
    let _ = stdout.flush();

    let mut buffer = String::new();
    if let Ok(_) = stdin.read_line(&mut buffer) {
        return buffer.trim_end().into();
    }
    "".into()
}

fn prompt_value<T: FromStr + PartialOrd>(prompt_text: &str, min: T, max: T) -> Option<T> {
    let passed = prompt(prompt_text);
    match passed.parse::<T>() {
        Ok(n) if (n >= min && n <= max) => Some(n),
        _ => None
    }
}

fn print_command_help() {
    println!("valid commands are just SRS and NAV at the mo")
}

