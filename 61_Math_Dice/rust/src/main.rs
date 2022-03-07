use rand::{Rng, prelude::{thread_rng}};
use std::io;

fn main() {
    //DATA
    let num_tries:u8 = 2; //number of tries the player gets each round, must be at least 1
    let mut rng = thread_rng();
    let mut user_guess: u8;
    let mut dice_1:u8;
    let mut dice_2:u8;

    //print welcome message
    welcome();

    //game loop
    loop {
        //roll dice
        dice_1 = rng.gen_range(1..=6);
        dice_2 = rng.gen_range(1..=6);

        //print dice
        print_dice(dice_1);
        println!("   +");
        print_dice(dice_2);
        println!("   =");

        //get user guess, they have 2 tries
        for t in 0..num_tries {
            //get guess
            user_guess = get_number_from_user_input("", "That's not a valid number!", 1, 12);

            //if they get it wrong
            if user_guess != (dice_1+dice_2) {
                //print different message depending on what try they're on
                if t < num_tries-1 { // user has tries left
                    println!("NO, COUNT THE SPOTS AND GIVE ANOTHER ANSWER.");
                    println!("   =");
                }
                else { //this is their last try
                    println!("NO, THE ANSWER IS {}", dice_1+dice_2);
                }
            }
            else {
                println!("RIGHT!");
                break;
            }
        }

        //play again
        println!("\nThe dice roll again....");
    }

}

/**
 * prints the welcome message to the console
 */
fn welcome() {
    println!("
                              MATH DICE
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY
    \n\n
    THIS PROGRAM GENERATES SUCCESSIVE PICTURES OF TWO DICE.
    WHEN TWO DICE AND AN EQUAL SIGN FOLLOWED BY A QUESTION
    MARK HAVE BEEN PRINTED, TYPE YOUR ANSWER AND THE RETURN KEY.
    TO CONCLUDE THE LESSON, PRESS Ctrl+C AS YOUR ANSWER.\n
    ");
}

/**
 * print the dice,
 */
fn print_dice(dice_value:u8) {
    //data

    //top
    println!(" ----- ");
    //first layer
    match dice_value {
        4|5|6 => println!("| * * |"),
        2|3 => println!("| *   |"),
        _=>println!("|     |"),
    }

    //second layer
    match dice_value {
        1|3|5 => println!("|  *  |"),
        2|4 => println!("|     |"),
        _=>println!("| * * |"),
    }

    //third layer
    match dice_value {
        4|5|6 => println!("| * * |"),
        2|3 => println!("|   * |"),
        _=>println!("|     |"),
    }

    //bottom
    println!(" ----- ");
}

/**
 * gets a integer from user input
 */
fn get_number_from_user_input(prompt: &str, error_message: &str, min:u8, max:u8) -> u8 {
    //input loop
    return loop {
        let mut raw_input = String::new(); // temporary variable for user input that can be parsed later

        //print prompt
        println!("{}", prompt);
        //read user input from standard input, and store it to raw_input
        //raw_input.clear(); //clear input
        io::stdin().read_line(&mut raw_input).expect( "CANNOT READ INPUT!");

        //from input, try to read a number
        match raw_input.trim().parse::<u8>() {
            Ok(i) => {
                if i < min || i > max { //input out of desired range
                    println!("{}  ({}-{})", error_message, min,max);
                    continue; // run the loop again
                }
                else {
                    break i;// this escapes the loop, returning i
                }
            },
            Err(e) => {
                println!("{}  {}", error_message, e.to_string().to_uppercase());
                continue; // run the loop again
            }
        };
    };
}
