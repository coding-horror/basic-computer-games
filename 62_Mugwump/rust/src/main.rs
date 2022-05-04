mod coordinate;
mod draw;
mod game;
pub mod util;

use crate::game::Game;

fn main() {
    println!("\n\nMUGWUMP");
    println!("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");

    println!("THE OBJECT OF THIS GAME IS TO FIND FOUR MUGWUMPS");
    println!("HIDDEN ON A 10 BY 10 GRID.  HOMEBASE IS POSITION 0,0.");
    println!("ANY GUESS YOU MAKE MUST BE TWO NUMBERS WITH EACH");
    println!("NUMBER BETWEEN 0 AND 9, INCLUSIVE.  FIRST NUMBER");
    println!("IS DISTANCE TO RIGHT OF HOMEBASE AND SECOND NUMBER");
    println!("IS DISTANCE ABOVE HOMEBASE!\n");

    println!("YOU GET 10 TRIES.  AFTER EACH TRY, I WILL TELL");
    println!("YOU HOW FAR YOU ARE FROM EACH MUGWUMP.\n");

    let mut _quit = false;

    while !_quit {
        let mut game = Game::new();

        loop {
            if !game.tick() {
                _quit = true;
                break;
            }
        }
    }
}
