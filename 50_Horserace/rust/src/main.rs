use crate::{game::Game, util::PromptResult};

mod game;
mod horses;
mod players;
mod util;

fn main() {
    println!("\n\n\t\tHORSERACE");
    println!("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n");
    println!("WELCOME TO SOUTH PORTLAND HIGH RACETRACK\n\t\t...OWNED BY LAURIE CHEVALIER");

    if let PromptResult::YesNo(yes) = util::prompt(Some(false), "DO YOU WANT DIRECTIONS?") {
        if yes {
            println!("UP TO 10 MAY PLAY.  A TABLE OF ODDS WILL BE PRINTED.  YOU");
            println!("MAY BET ANY AMOUNT UNDER $100,000 ON ONE HORSE.");
            println!("DURING THE RACE, A HORSE WILL BE SHOWN BY ITS");
            println!("NUMBER.  THE HORSES RACE DOWN THE PAPER!\n");
        }
    }

    let mut game = Game::new();
    let mut again = true;

    while again {
        again = game.play();
    }
}
