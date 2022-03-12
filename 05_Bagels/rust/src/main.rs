use std::io;
use std::io::{stdin, stdout, Write};

use rand::prelude::SliceRandom;

type Digits = [u8; 3];

fn main() -> io::Result<()> {
    print_header();
    if !read_lowercase_input()?.starts_with('n') {
        print_rules();
    }
    let mut win_count = 0;
    loop {
        let solution = generate_random_digits();
        println!("\nO.K. I have a number in mind.");
        if guess(solution)? {
            win_count += 1;
            println!("You got it!!!");
        } else {
            println!(
                "\nOh well\nThat's twenty guesses: My number was {}{}{}",
                solution[0], solution[1], solution[2]
            );
        }
        println!("\nPlay again (yes or no)?");
        if read_lowercase_input()? != "yes" {
            println!();
            if win_count > 0 {
                println!("A {win_count} point bagels buff!!");
            }
            println!("Hope you had fun. Bye.");
            return Ok(());
        }
    }
}

fn print_header() {
    println!("                  Bagels");
    println!("Creative-Computing  Morristown, New Jersey");
    println!();
    println!("Would you like the rules (yes or no)?");
}

fn print_rules() {
    println!();
    println!("I am thinking of a three-digit number. Try to guess");
    println!("my number and I will give you clues as follows:");
    println!("   Pico   - one digit correct but in the wrong position");
    println!("   Fermi  - one digit correct and in the right position");
    println!("   Bagles - no digits correct");
}

fn read_lowercase_input() -> io::Result<String> {
    let mut input = String::new();
    stdin().read_line(&mut input)?;
    Ok(input.trim().to_lowercase())
}

fn generate_random_digits() -> Digits {
    let range = 0..10;
    let mut numbers = range.into_iter().collect::<Vec<u8>>();
    numbers.shuffle(&mut rand::thread_rng());
    let mut digits = Digits::default();
    let len = digits.len();
    digits.copy_from_slice(&numbers[..len]);
    digits
}

fn guess(solution: Digits) -> io::Result<bool> {
    for round in 1..21 {
        let guess = read_valid_guess(round)?;
        if guess == solution {
            return Ok(true);
        } else {
            let mut pico = 0;
            let mut fermi = 0;
            for i in 0..guess.len() {
                if guess[i] == solution[i] {
                    fermi += 1;
                } else if solution.contains(&guess[i]) {
                    pico += 1;
                }
            }
            let mut status = String::new();
            if pico == 0 && fermi == 0 {
                status += "Bagels";
            } else {
                for _ in 0..pico {
                    status += "Pico ";
                }
                for _ in 0..fermi {
                    status += "Fermi ";
                }
            };
            println!("{}", status.trim_end());
        }
    }
    Ok(false)
}

fn read_valid_guess(round: u8) -> io::Result<Digits> {
    let mut guess = Digits::default();
    loop {
        let space = " ".repeat(if round < 10 { 5 } else { 4 });
        print!("Guess #{round}{space}? ");
        stdout().flush()?;
        let input = read_lowercase_input()?;
        if input.len() == guess.len() {
            let mut i = 0;
            for c in input.chars() {
                if let Ok(digit) = c.to_string().parse::<u8>() {
                    if guess[..i].contains(&digit) {
                        println!("\nOh, I forgot to tell you that the number I have in mind\nhas no two digits the same.");
                        break;
                    }
                    guess[i] = digit;
                    i += 1;
                    if i == guess.len() {
                        return Ok(guess);
                    }
                } else {
                    println!("What?");
                    break;
                }
            }
        } else {
            println!("Try guessing a three-digit number.");
        }
    }
}
