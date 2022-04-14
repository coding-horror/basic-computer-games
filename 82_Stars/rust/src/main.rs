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
        }
    }
}

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
