use crate::game::Game;

mod game;
mod horses;
mod players;
mod util;

fn main() {
    let mut game = Game::new();
    let mut again = true;

    while again {
        again = game.play();
    }
}
