use std::process::exit;

use input::prompt;
use model::{Galaxy, systems};

mod input;
mod model;
mod commands;
mod view;

fn main() {
    ctrlc::set_handler(move || { exit(0) })
    .expect("Error setting Ctrl-C handler");

    let mut galaxy = Galaxy::generate_new();
    let initial_klingons = galaxy.remaining_klingons();
    let initial_stardate = galaxy.stardate;
    
    view::enterprise();
    view::intro(&galaxy);
    let _ = input::prompt("Press Enter when ready to accept command");

    view::starting_quadrant(&galaxy.enterprise.quadrant);
    view::short_range_scan(&galaxy);

    loop {
        let command = input::prompt("Command?");
        if command.len() == 0 {
            continue;
        }
        match command[0].to_uppercase().as_str() { // order is weird because i built it in this order :)
            systems::SHORT_RANGE_SCAN => commands::perform_short_range_scan(&galaxy),
            systems::WARP_ENGINES => commands::gather_dir_and_speed_then_move(&mut galaxy, command[1..].into()),
            systems::SHIELD_CONTROL => commands::get_amount_and_set_shields(&mut galaxy, command[1..].into()),
            systems::DAMAGE_CONTROL => commands::run_damage_control(&mut galaxy),
            systems::LONG_RANGE_SCAN => commands::perform_long_range_scan(&mut galaxy),
            systems::COMPUTER => commands::access_computer(&galaxy, command[1..].into()),
            systems::PHASERS => commands::get_power_and_fire_phasers(&mut galaxy, command[1..].into()),
            _ => view::print_command_help()
        }

        if galaxy.enterprise.destroyed || galaxy.enterprise.is_stranded() || galaxy.stardate >= galaxy.final_stardate {
            view::end_game_failure(&galaxy);
            if galaxy.remaining_klingons() > 0 && galaxy.remaining_starbases() > 0 && galaxy.stardate < galaxy.final_stardate {
                view::replay();
                let result = prompt("");
                if result.len() > 0 && result[0].to_uppercase() == "AYE" {
                    galaxy.enterprise = Galaxy::new_captain(&galaxy.quadrants);
                    continue;
                }
            }
            break;
        } else if galaxy.remaining_klingons() == 0 {
            let efficiency = 1000.0 * f32::powi(initial_klingons as f32 / (galaxy.stardate - initial_stardate), 2);
            view::congratulations(efficiency);
            break;
        }
    }
}
