use crate::game::Game;

mod game;
mod util;

fn main() {
    println!("\n\n\t\tCUBE");
    println!("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n");

    if util::prompt_bool("DO YOU WANT TO SEE THE INSTRUCTIONS? (YES--1,NO--0)") {
        println!("\nThis is a game in which you will be playing against the");
        println!("random decisions of the computer. The field of play is a");
        println!("cube of side 3. Any of the 27 locations can be designated");
        println!("by inputing three numbers such as 2,3,1. At the start,");
        println!("you are automatically at location 1,1,1. The object of");
        println!("the game is to get to location 3,3,3. One minor detail:");
        println!("the computer will pick, at random, 5 locations at which");
        println!("it will plant land mines. If you hit one of these locations");
        println!("you lose. One other detail: You may move only one space");
        println!("in one direction each move. For example: From 1,1,2 you");
        println!("may move to 2,1,2 or 1,1,3. You may not change");
        println!("two of the numbers on the same move. If you make an illegal");
        println!("move, you lose and the computer takes the money you may");
        println!("have bet on that round.\n");
        println!("When stating the amount of a wager, print only the number");
        println!("of dollars (example: 250) you are automatically started with");
        println!("500 dollars in your account.\n");
        println!("Good luck!\n");
    }

    let mut game = Game::new();

    loop {
        if !game.play() {
            println!("\nTOUGH LUCK\n\nGOODBYE!\n");
            break;
        }
    }
}
