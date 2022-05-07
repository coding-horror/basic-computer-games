use crate::{
    game::Game,
    util::{prompt, PromptResult::*},
};

mod game;
mod util;
mod ai;

fn main() {
    util::intro();

    loop {
        let mut game = Game::new();

        loop {
            if !game.update() {
                break;
            }
        }

        if let YesNo(y) = prompt(Some(false), "\nANYONE ELSE CARE TO TRY?") {
            if !y {
                println!("OK --- THANKS AGAIN.");
                break;
            }
        }
    }
}
