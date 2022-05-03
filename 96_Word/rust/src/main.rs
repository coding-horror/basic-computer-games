use crate::word_game::WordGame;
use std::io;
mod progress;
mod word_game;

fn main() {
    println!("\n\n~~WORD~~");
    println!("Creative Computing Morristown, New Jersey");

    println!("\nI am thinking of a word -- you guess it.");
    println!("I will give you clues to help you get it.");
    println!("Good luck!!\n");

    let mut quit = false;

    while quit == false {
        let mut game = WordGame::new();
        let mut game_over = false;

        while game_over == false {
            game_over = game.tick();
        }

        quit = !play_again();
    }
}

fn play_again() -> bool {
    let mut again = true;
    let mut valid_response = false;

    while valid_response == false {
        println!("Want to play again? (Y/n)");

        let mut response = String::new();

        io::stdin()
            .read_line(&mut response)
            .expect("Failed to read line.");

        match response.trim().to_uppercase().as_str() {
            "Y" | "YES" => valid_response = true,
            "N" | "NO" => {
                again = false;
                valid_response = true;
            }
            _ => (),
        }
    }

    again
}
