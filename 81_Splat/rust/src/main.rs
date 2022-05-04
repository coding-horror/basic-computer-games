use crate::{game::Game, stats::Stats};

mod celestial_body;
mod game;
mod stats;
mod utility;

fn main() {
    println!("\n\n\n                       SPLAT");
    println!("      CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n\n");

    println!("WELCOME TO 'SPLAT' -- THE GAME THAT SIMULATES");
    println!("A PARACHUTE JUMP. TRY OPEN YOUR CHUTE AT THE");
    println!("LAST POSSIBLE MOMENT WITHOUT GOING SPLAT.\n");

    let mut stats = Stats::new();

    loop {
        let mut game = Game::new();

        let latest_altitude = game.tick();

        if latest_altitude > 0. {
            if let Some(s) = &mut stats {
                s.add_altitude(latest_altitude);
            }
        }

        use utility::prompt_bool;
        if !prompt_bool("DO YOU WANT TO PLAY AGAIN?", true) {
            if !prompt_bool("PLEASE?", false) {
                if !prompt_bool("YES OR NO PLEASE?", false) {
                    println!("SSSSSSSSSS.");
                    break;
                }
            }
        }
    }
}
