use std::process::exit;

use model::Galaxy;

mod input;
mod model;
mod commands;
mod view;

fn main() {
    ctrlc::set_handler(move || { exit(0) })
    .expect("Error setting Ctrl-C handler");

    let mut galaxy = Galaxy::generate_new();
    
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
            model::systems::SHORT_RANGE_SCAN => commands::perform_short_range_scan(&galaxy),
            model::systems::WARP_ENGINES => commands::gather_dir_and_speed_then_move(&mut galaxy, command[1..].into()),
            model::systems::SHIELD_CONTROL => commands::get_amount_and_set_shields(&mut galaxy, command[1..].into()),
            model::systems::DAMAGE_CONTROL => commands::display_damage_control(&galaxy.enterprise),
            model::systems::LONG_RANGE_SCAN => commands::perform_long_range_scan(&mut galaxy),
            model::systems::COMPUTER => commands::access_computer(&galaxy, command[1..].into()),
            _ => view::print_command_help()
        }

        if galaxy.enterprise.destroyed || galaxy.enterprise.check_stranded() {
            view::end_game_failure(&galaxy);
            // todo check if can restart
            break;
        }

        // todo check for victory
    }
}
