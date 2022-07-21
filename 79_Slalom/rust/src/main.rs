use game::Game;

mod game;
mod utility;

fn main() {
    let mut game = Game::new();
    game.play();
}
