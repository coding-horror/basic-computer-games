use game::Game;

mod disk;
mod game;
mod needle;
mod util;

fn main() {
    let game = Game::new();
    game.draw();
}
