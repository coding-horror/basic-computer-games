<<<<<<< HEAD
//
// Stars
//
// From: BASIC Computer Games (1978), edited by David H. Ahl
//
// In this game, the computer selects a random number from 1 to 100
// (or any value you set [for MAX_NUM]).  You try to guess the number
// and the computer gives you clues to tell you how close you're
// getting.  One star (*) means you're far away from the number; seven
// stars (*******) means you're really close.  You get 7  guesses.
//
// On the surface this game is very similar to GUESS; however, the
// guessing strategy is quite different.  See if you can come up with
// one or more approaches to finding the mystery number.
//
// Bob Albrecht of People's Computer Company created this game.
//
// rust port by JW BRUCE 2022
//
// ********************************************************************
//
// Porting Notes (taken for Jeff Jetton's Python version)
//
//   The original program never exited--it just kept playing rounds
//   over and over.  This version asks to continue each time.
//
// Ideas for Modifications
//
//   Let the player know how many guesses they have remaining after
//   each incorrect guess.
//
//   Ask the player to select a skill level at the start of the game,
//   which will affect the values of MAX_NUM and MAX_GUESSES.
//   For example:
//
//       Easy   = 8 guesses, 1 to 50
//       Medium = 7 guesses, 1 to 100
//       Hard   = 6 guesses, 1 to 200
//
// *********************************************************************

// I M P O R T S
use std::io;
use std::io::stdin;
//use std::io::{stdin, stdout, Write};
use rand::Rng;

const   MAX_NUM: u8 = 100;
const   MAX_GUESSES: u8 = 7;

fn main() -> io::Result<()> {
    print_header();
    if !read_lowercase_input()?.starts_with('n') {
        print_rules();
    }
    loop {
        let secret_number : u8 = rand::thread_rng().gen_range(1..101);
        let mut guess_count = 0;
        let mut player_won: bool = false;
        
        println!("\n\nOK, I am thinking of a number, start guessing.");
        while guess_count < MAX_GUESSES && !player_won {
            
            guess_count += 1;        

            println!("Your guess? ");
            let mut guess = String::new();
            io::stdin()
                .read_line(&mut guess)
                .expect("Failed to read line");

            let guess: u8 = match guess.trim().parse() {
                Ok(num) => num,
                Err(_) => continue,
            };
            
            // USE THIS STATEMENT FOR DEBUG PURPOSES
            // println!("Guess #{} is {}. secret number is {}",guess_count, guess, secret_number);
            
            if guess == secret_number {
                // winner winner chicken dinner
                player_won = true;
                println!("**************************************************!!!");
                println!("You got it in {guess_count} guesses!!!");
            } else {
                print_stars( guess, secret_number) ;
            }      
        }
        
        // player exhausted their number of guesses and did not win.
        if !player_won {
            println!("Sorry, that's {guess_count} guesses, number was {secret_number}");
        }

        println!("\nPlay again (yes or no)?");
        if !read_lowercase_input()?.starts_with('y') {
            return Ok(());
=======
use rand::Rng;
use std::io;

fn main() {
    println!(
        "{: >39}\n{: >57}\n\n\n",
        "STARS", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    );
    // STARS - PEOPLE'S COMPUTER CENTER, MENLO PARK, CA
    // A IS LIMIT ON NUMBER, M IS NUMBER OF GUESSES
    let a: u32 = 101;
    let m: u32 = 7;
    let mut need_instrut = String::new();

    println!("DO YOU WANT INSTRUCTIONS?");
    io::stdin()
        .read_line(&mut need_instrut)
        .expect("Failed to get input");

    if need_instrut[..1].to_ascii_lowercase().eq("y") {
        println!("I AM THINKING OF A WHOLE NUMBER FROM 1 TO {}", a - 1);
        println!("TRY TO GUESS MY NUMBER.  AFTER YOU GUESS, I");
        println!("WILL TYPE ONE OR MORE STARS (*).  THE MORE");
        println!("STARS I TYPE, THE CLOSER YOU ARE TO MY NUMBER.");
        println!("ONE STAR (*) MEANS FAR AWAY, SEVEN STARS (*******)");
        println!("MEANS REALLY CLOSE!  YOU GET {} GUESSES.\n\n", m);
    }

    loop {
        println!("\nOK, I AM THINKING OF A NUMBER, START GUESSING.\n");
        let rand_number: i32 = rand::thread_rng().gen_range(1..a) as i32; // generates a random number between 1 and 100

        // GUESSING BEGINS, HUMAN GETS M GUESSES
        for i in 0..m {
            let mut guess = String::new();
            println!("YOUR GUESS?");
            io::stdin()
                .read_line(&mut guess)
                .expect("Failed to get input");
            let guess: i32 = match guess.trim().parse() {
                Ok(num) => num,
                Err(_) => {
                    println!("PLEASE ENTER A NUMBER VALUE.\n");
                    continue;
                }
            };
            if guess == rand_number {
                print!("");
                for _i in 0..50 {
                    print!("*");
                }
                println!("!!!");
                println!("YOU GOT IT IN {} GUESSES!!!  LET'S PLAY AGAIN...\n", i + 1);
                break;
            } else {
                match_guess(rand_number - guess);
            }

            if i == 6 {
                println!(
                    "SORRY, THAT'S {} GUESSES. THE NUMBER WAS {}",
                    m, rand_number
                );
            }
>>>>>>> 3e27c70ca800f5efbe6bc1a7d180211decf55b7d
        }
    }
}

<<<<<<< HEAD
// guess is wrong, so print stars to show how far away they are
fn print_stars( guess: u8, target: u8) {
    // choose to use u8 in main, but currently (1.59.0) does not
    //   have abs() defined for u8.  abs() is defined for i16, so
    //   this provide an opportunity to demonstrate casting in rust
    let diff : i16 = ((guess as i16)-(target as i16)).abs();
    
    // Since we only print 1-7 stars, this finite set of choices is
    //   small enough that we can use rust's match keyword.
    // The match "arms" here use the inclusive range notation.
    //   The exlusive range notation is not an approved feature of
    //   rust, yet.
    match diff {
        1..=2 => println!("*******"),
        3..=4 => println!("******"),
        5..=8 => println!("*****"),
        9..=16 => println!("****"),
        17..=32 => println!("***"),
        33..=64 => println!("**"),
        _ => println!("*"),
    }
}

//
fn read_lowercase_input() -> io::Result<String> {
    let mut input = String::new();
    stdin().read_line(&mut input)?;
    Ok(input.trim().to_lowercase())
}

// Text to print at the start of the game
fn print_header() {
    println!("\n                   Stars");
    println!("Creative-Computing  Morristown, New Jersey");
    println!("\n\n");
    println!("Do you want instructions? ");
}

// Instructions on how to play
fn print_rules() {
    println!();
    println!("I am thinking of a whole number from 1 to {}", MAX_NUM);
    println!("Try to guess my number.  After you guess, I");
    println!("will type one or more stars (*).  The more");
    println!("stars I type, the closer you are to my number.");
    println!("one star (*) means far away, seven stars (*******)");
    println!("means really close!  You get {} guesses.", MAX_GUESSES);
}
=======
fn match_guess(diff: i32) {
    if diff.abs() >= 64 {
        println!("*\n");
    } else if diff.abs() >= 32 {
        println!("**\n");
    } else if diff.abs() >= 16 {
        println!("***\n");
    } else if diff.abs() >= 8 {
        println!("****\n");
    } else if diff.abs() >= 4 {
        println!("*****\n");
    } else if diff.abs() >= 2 {
        println!("******\n");
    } else {
        println!("*******\n");
    }
}
>>>>>>> 3e27c70ca800f5efbe6bc1a7d180211decf55b7d
