mod craps_game;
mod util;
use crate::craps_game::CrapsGame;

fn main() {
    println!("~~Craps~~");
    println!("Creative Computing Morristown, New Jersey\n");

    let mut quit = false;

    while !quit {
        let mut game = CrapsGame::new();

        loop {
            if !game.tick() {
                use util::Response::*;

                match util::prompt("New Game?") {
                    Yes => (),
                    No => quit = true,
                }

                break;
            }
        }
    }
}
