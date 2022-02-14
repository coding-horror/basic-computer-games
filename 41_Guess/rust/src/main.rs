//#######################################################
//
// Guess
//
// From: Basic Computer Games (1978)
//
// "In program Guess, the computer  chooses a random
//  integer between 0 and any limit and any limit you
//  set. You must then try to guess the number the
//  computer has choosen using the clues provideed by
//  the computer.
//   You should be able to guess the number in one less
//  than the number of digits needed to  represent the
//  number in binary notation - i.e. in base 2. This ought
//  to give you a clue as to the optimum search technique.
//   Guess converted from the original program in FOCAL
//  which appeared in the book "Computers in the Classroom"
//  by Walt Koetke of Lexington High School, Lexington,
//   Massaschusetts.
//
//#######################################################


use rand::Rng;
use std::io;
use std::cmp::Ordering;
// Rust haven't log2 in the standard library so I added fn log_2 
const fn num_bits<T>() -> usize { std::mem::size_of::<T>() * 8 }

fn main() {

    let mut rng = rand::thread_rng();
    let mut still_guessing = true;
    let limit = set_limit();
    let limit_goal = 1+(log_2(limit.try_into().unwrap())/log_2(2)) ;
    loop{

        let mut won = false;
        let mut guess_count = 1;
        let my_guess = rng.gen_range(1..limit);

        println!("I'm thinking of a number between 1 and {}",limit);
        println!("Now you try to guess what it is.");

        while still_guessing {
            let inp = get_input()
                .trim()
                .parse::<i64>().unwrap();
            println!("\n\n\n");
            if inp < my_guess {
                println!("Too low. Try a bigger answer");
                guess_count+=1;
            }
            else if inp > my_guess {
                println!("Too high. Try a smaller answer");
                guess_count+=1;
            }
            else {
                println!("That's it! You got it in {} tries", guess_count);
                won = true;
                still_guessing = false;
            }
        }
        if won {
            match guess_count.cmp(&limit_goal) {
                Ordering::Less => println!("Very good."),
                Ordering::Equal => println!("Good."),
                Ordering::Greater => println!("You should have been able to get it in only {}", limit_goal),
            }

            println!("\n\n\n");
            still_guessing = true;
        } else {
            println!("\n\n\n");
        }
    }
}

fn log_2(x:i32) -> u32 {
    assert!(x > 0);
    num_bits::<i32>() as u32 - x.leading_zeros() - 1
}

fn set_limit() -> i64 {

    println!("                   Guess");
    println!("\n\n\n");
    println!("This is a number guessing game. I'll think");
    println!("of a number between 1 and any limit you want.\n");
    println!("Then you have to guess what it is\n");
    println!("What limit do you want?");

    let inp = get_input().trim().parse::<i64>().unwrap();

    if inp >= 2 {
        inp
    } 
    else {
        set_limit()
    }
}

fn get_input() -> String {
    let mut input = String::new();
    io::stdin()
        .read_line(&mut input)
        .expect("Your input is not correct");
    input
}
