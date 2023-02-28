use std::{io::{stdin, stdout, Write}, process::exit};

use model::Galaxy;

mod model;
mod commands;

fn main() {
    ctrlc::set_handler(move || { exit(0) })
    .expect("Error setting Ctrl-C handler");

    let galaxy = Galaxy::generate_new();
    // init ops, starting state and notes
    commands::short_range_scan(&galaxy);

    loop {
        match prompt("Command?").as_str() {
            "SRS" => commands::short_range_scan(&galaxy),
            _ => print_command_help()
        }

        // process the next command, based on it render something or update the galaxy or whatever
        // this would be: read command, and based on it run dedicated function
        // the function might get passed a mutable reference to the galaxy
    }
}

fn prompt(prompt: &str) -> String {
    let stdin = stdin();
    let mut stdout = stdout();

    print!("{prompt} ");
    let _ = stdout.flush();

    let mut buffer = String::new();
    if let Ok(_) = stdin.read_line(&mut buffer) {
        return buffer.trim_end().into();
    }
    "".into()
}

fn print_command_help() {
    println!("valid commands are just SRS and NAV at the mo")
}

// match text.parse::<u8>() {
//     Ok(n) if (n >= 1 && n <= 8) => Some(Message::DirectionForNav(n)),
//     _ => None
// }