use std::{io::{stdin, stdout, Write}, process::exit, str::FromStr};

use model::{Galaxy, Pos};

use crate::model::DIRECTIONS;

mod model;
mod commands;

fn main() {
    ctrlc::set_handler(move || { exit(0) })
    .expect("Error setting Ctrl-C handler");

    let mut galaxy = Galaxy::generate_new();
    // init ops, starting state and notes
    commands::short_range_scan(&galaxy);

    loop {
        match prompt("Command?").to_uppercase().as_str() {
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
    const BAD_NAV: &str = "   Lt. Sulu reports, 'Incorrect course data, sir!'";

    let dir = prompt_value::<u8>("Course (1-9)?", 1, 9);
    if dir.is_none() {
        println!("{}", BAD_NAV); 
        return;
    }

    let speed = prompt_value::<f32>("Warp Factor (0-8)?", 0.0, 8.0);
    if speed.is_none() {
        println!("{}", BAD_NAV); 
        return;
    }

    let distance = (speed.unwrap() * 8.0) as i8;
    let galaxy_pos = galaxy.enterprise.quadrant * 8u8 + galaxy.enterprise.sector;

    let (dx, dy): (i8, i8) = DIRECTIONS[(dir.unwrap() - 1) as usize];
    let mut nx = (galaxy_pos.0 as i8) + dx * distance;
    let mut ny = (galaxy_pos.1 as i8) + dy * distance;

    let mut hit_edge = false;
    if nx < 0 {
        nx = 0;
        hit_edge = true;
    }
    if ny < 0 {
        ny = 0;
        hit_edge = true;
    }
    if nx >= 64 {
        ny = 63;
        hit_edge = true;
    }
    if nx >= 64 {
        ny = 63;
        hit_edge = true;
    }
    
    let new_quadrant = Pos((nx / 8) as u8, (ny / 8) as u8);
    let new_sector = Pos((nx % 8) as u8, (ny % 8) as u8);

    if hit_edge {
        println!("Lt. Uhura report message from Starfleet Command:
        'Permission to attempt crossing of galactic perimeter
        is hereby *Denied*. Shut down your engines.'
      Chief Engineer Scott reports, 'Warp engines shut down
        at sector {} of quadrant {}.'", new_quadrant, new_sector);
    }

    galaxy.enterprise.quadrant = new_quadrant;
    galaxy.enterprise.sector = new_sector;
        
    // if new_quadrant isnt old quadrant print intro

    commands::short_range_scan(&galaxy)
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

