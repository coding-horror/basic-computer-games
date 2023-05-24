use std::io::stdin;

use rand::Rng;

fn main() {
    println!("\n\t\tTRAP");
    println!("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n");

    let max_guess = 6;
    let max_number = 100;

    prompt_instructions();

    loop {
        let number = rand::thread_rng().gen_range(1..(max_number + 1));
        let mut guesses = 1u8;

        loop {
            let (min, max) = prompt_numbers(guesses);

            if min == number && max == number {
                println!("\nYou got it!!!");
                break;
            } else if (min..=max).contains(&number) {
                println!("You have trapped my number.");
            } else if number < min {
                println!("My number is smaller than your trap numbers.");
            } else if number > max {
                println!("My number is bigger than your trap numbers.");
            }

            guesses += 1;
            if guesses > max_guess {
                println!("\nSorry, that was {max_guess} guesses. Number was {number}");
                break;
            }
        }

        println!("\nTry again.");
    }
}

fn prompt_instructions() {
    println!("Instructions?\t");

    let mut input = String::new();
    if let Ok(_) = stdin().read_line(&mut input) {
        match input.to_uppercase().trim() {
            "YES" | "Y" => {
                println!("\nI am thinking of a number between 1 and 100");
                println!("Try to guess my number. On each guess,");
                println!("you are to enter 2 numbers, trying to trap");
                println!("my number between the two numbers. I will");
                println!("tell you if you have trapped my number, if my");
                println!("number is larger than your two numbers, or if");
                println!("my number is smaller than your two numbers.");
                println!("If you want to guess one single number, type");
                println!("your guess for both your trap numbers.");
                println!("You get 6 guesses to get my number.");
            }
            _ => (),
        }
    }
}

fn prompt_numbers(guess: u8) -> (u8, u8) {
    loop {
        let mut nums: Vec<u8> = Vec::new();
        println!("\nGuess # {guess} ?");

        let mut input = String::new();
        if let Ok(_) = stdin().read_line(&mut input) {
            let input: Vec<&str> = input.trim().split(",").collect();

            for string in input {
                if let Ok(number) = string.parse::<u8>() {
                    nums.push(number);
                } else {
                    break;
                }
            }

            if nums.len() == 2 {
                if nums[0] <= nums[1] {
                    return (nums[0], nums[1]);
                }
            }
        }
    }
}
