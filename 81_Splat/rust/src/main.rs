use crate::game::Game;

mod celestial_body;
mod game;
mod utility;

fn main() {
    println!("\n\n\n                       SPLAT");
    println!("      CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n\n");

    println!("WELCOME TO 'SPLAT' -- THE GAME THAT SIMULATES");
    println!("A PARACHUTE JUMP. TRY OPEN YOUR CHUTE AT THE");
    println!("LAST POSSIBLE MOMENT WITHOUT GOING SPLAT.\n");

    //let mut quit = false;

    // while !quit {
    loop {
        let mut game = Game::new();
        if !game.tick() {
            break;
        }

        /* loop {
            if let Some(play_again) = game.tick() {
                if !play_again {
                    quit = true;
                }
                break;
            }
        } */
    }
}
