use std::{io::{stdin, stdout, Write}, process::exit, str::FromStr};

use model::Galaxy;

use crate::model::Condition;

mod model;
mod commands;

fn main() {
    ctrlc::set_handler(move || { exit(0) })
    .expect("Error setting Ctrl-C handler");

    let mut galaxy = Galaxy::generate_new();
    // todo: init options, starting state and notes
    commands::short_range_scan(&galaxy);

    loop {
        match prompt("Command?").to_uppercase().as_str() {
            "SRS" => commands::short_range_scan(&galaxy),
            "NAV" => gather_dir_and_speed_then_move(&mut galaxy),
            _ => print_command_help()
        }

        if galaxy.enterprise.condition == Condition::Destroyed { // todo: also check if stranded
            println!("Is is stardate {}.
            There were {} Klingon battle cruisers left at
            the end of your mission.
            ", galaxy.stardate, galaxy.remaining_klingons());
            break;
        }
    }
}

fn gather_dir_and_speed_then_move(galaxy: &mut Galaxy) {
    const BAD_NAV: &str = "   Lt. Sulu reports, 'Incorrect course data, sir!'";

    let course = prompt_value::<u8>("Course (1-9)?", 1, 9);
    if course.is_none() {
        println!("{}", BAD_NAV); 
        return;
    }

    let speed = prompt_value::<f32>("Warp Factor (0-8)?", 0.0, 8.0);
    if speed.is_none() {
        println!("{}", BAD_NAV); 
        return;
    }

    let quadrant = &mut galaxy.quadrants[galaxy.enterprise.quadrant.as_index()];
    for k in 0..quadrant.klingons.len() {
        let new_sector = quadrant.find_empty_sector();
        quadrant.klingons[k].sector = new_sector;
    }

    // todo: check if enterprise is protected by a starbase

    for k in 0..quadrant.klingons.len() {
        quadrant.klingons[k].fire_on(&mut galaxy.enterprise);
    }

    if galaxy.enterprise.condition == Condition::Destroyed {
        return;
    }
    commands::move_enterprise(course.unwrap(), speed.unwrap(), galaxy);
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

