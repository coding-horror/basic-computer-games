use game::Game;
use util::PromptResult;

mod disk;
mod game;
mod needle;
mod util;

fn main() {
    println!("\n\n\t\tTOWERS");
    println!("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n");

    println!("TOWERS OF HANOI PUZZLE\n");

    println!("YOU MUST TRANSFER THE DISKS FROM THE LEFT TO THE RIGHT");
    println!("TOWER, ON AT A TIME, NEVER PUTTING A LARGER DISK ON A");
    println!("SMALLER DISK.\n");

    let mut quit = false;

    while !quit {
        let mut game = Game::new();

        println!("");
        println!(
            r#"IN THIS PROGRAM, WE SHALL REFER TO DISKS BY NUMERICAL CODE.
3 WILL REPRESENT THE SMALLEST DISK, 5 THE NEXT SIZE,
7 THE NEXT, AND SO ON, UP TO 15.  IF YOU DO THE PUZZLE WITH
2 DISKS, THEIR CODE NAMES WOULD BE 13 AND 15.  WITH 3 DISKS
THE CODE NAMES WOULD BE 11, 13 AND 15, ETC.  THE NEEDLES
ARE NUMBERED FROM LEFT TO RIGHT, 1 TO 3.  WE WILL
START WITH THE DISKS ON NEEDLE 1, AND ATTEMPT TO MOVE THEM
TO NEEDLE 3.

GOOD LUCK!"#
        );

        loop {
            if game.update() {
                break;
            }
        }

        if let PromptResult::YesNo(r) = util::prompt(false, "TRY AGAIN (YES OR NO)?") {
            quit = !r;
        }
    }

    println!("THANKS FOR THE GAME!");
}
