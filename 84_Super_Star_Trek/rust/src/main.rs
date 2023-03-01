use std::{io::{stdin, stdout, Write, Read}, process::exit, str::FromStr};

use model::Galaxy;

mod model;
mod commands;
mod view;

fn main() {
    ctrlc::set_handler(move || { exit(0) })
    .expect("Error setting Ctrl-C handler");

    let mut galaxy = Galaxy::generate_new();
    
    view::enterprise();
    view::intro(&galaxy);
    let _ = prompt("Press Enter when ready to accept command");

    view::starting_quadrant(&galaxy.enterprise.quadrant);
    view::short_range_scan(&galaxy);

    loop {
        let command = prompt("Command?");
        if command.len() == 0 {
            continue;
        }
        match command[0].to_uppercase().as_str() {
            "SRS" => view::short_range_scan(&galaxy),
            "NAV" => gather_dir_and_speed_then_move(&mut galaxy, command[1..].into()),
            "SHE" => get_amount_and_set_shields(&mut galaxy, command[1..].into()),
            _ => view::print_command_help()
        }

        if galaxy.enterprise.destroyed { // todo: also check if stranded
            view::end_game_failure(&galaxy);
            break;
        }
    }
}

fn get_amount_and_set_shields(galaxy: &mut Galaxy, provided: Vec<String>) {

    // todo check for damaged module

    view::energy_available(galaxy.enterprise.total_energy);
    let value = param_or_prompt_value(&provided, 0, "Number of units to shields", 0, i32::MAX);
    if value.is_none() {
        view::shields_unchanged();
        return;
    }
    let value = value.unwrap() as u16;
    if value > galaxy.enterprise.total_energy {
        view::ridiculous();
        view::shields_unchanged();
        return;
    }

    galaxy.enterprise.shields = value;
    view::shields_set(value);
}

fn gather_dir_and_speed_then_move(galaxy: &mut Galaxy, provided: Vec<String>) {

    let course = param_or_prompt_value(&provided, 0, "Course (1-9)?", 1, 9);
    if course.is_none() {
        view::bad_nav();
        return;
    }

    let speed = param_or_prompt_value(&provided, 1, "Warp Factor (0-8)?", 0.0, 8.0);
    if speed.is_none() {
        view::bad_nav();
        return;
    }

    commands::move_klingons_and_fire(galaxy);
    if galaxy.enterprise.destroyed {
        return;
    }
    commands::move_enterprise(course.unwrap(), speed.unwrap(), galaxy);
}

fn prompt(prompt_text: &str) -> Vec<String> {
    let stdin = stdin();
    let mut stdout = stdout();

    print!("{prompt_text} ");
    let _ = stdout.flush();

    let mut buffer = String::new();
    if let Ok(_) = stdin.read_line(&mut buffer) {
        return buffer.trim_end().split(" ").map(|s| s.to_string()).collect();
    }
    Vec::new()
}

fn prompt_value<T: FromStr + PartialOrd>(prompt_text: &str, min: T, max: T) -> Option<T> {
    let passed = prompt(prompt_text);
    if passed.len() != 1 {
        return None
    }
    match passed[0].parse::<T>() {
        Ok(n) if (n >= min && n <= max) => Some(n),
        _ => None
    }
}

fn param_or_prompt_value<T: FromStr + PartialOrd>(params: &Vec<String>, param_pos: usize, prompt_text: &str, min: T, max: T) -> Option<T> {
    if params.len() > param_pos {
        match params[param_pos].parse::<T>() {
            Ok(n) => Some(n),
            _ => None
        }
    } else {
        return prompt_value::<T>(prompt_text, min, max);
    }
}