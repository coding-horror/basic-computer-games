mod game;
use crate::game::Game;

fn main() {
    morristown::print_intro("FLIPFLOP");

    println!("THE OBJECT OF THIS PUZZLE IS TO CHANGE THIS:\n");
    println!("X X X X X X X X X X\n");
    println!("TO THIS:\n");
    println!("O O O O O O O O O O\n");
    println!("BY TYPING THE NUMBER CORRESPONDING TO THE POSITION OF THE");
    println!("LETTER ON SOME NUMBERS, ONE POSITION WILL CHANGE, ON");
    println!("OTHERS, TWO WILL CHANGE.  TO RESET LINE TO ALL X'S, TYPE 0");
    println!("(ZERO) AND TO START OVER IN THE MIDDLE OF A GAME, TYPE ");
    println!("11 (ELEVEN).");

    let mut game = Game::new();

    loop {
        if !game.play() {
            break;
        }
    }
}
