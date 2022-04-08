use rand::Rng;
use std::cmp::Ordering;
use std::io;

fn main() {
    println!(
        "{: >40}\n{: >57}\n\n\n",
        "LETTER", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    );
    println!("LETTER GUESSING GAME\n");
    println!("I'LL THINK OF A LETTER OF THE ALPHABET, A TO Z.");
    println!("TRY TO GUESS MY LETTER AND I'LL GIVE YOU CLUES");
    println!("AS TO HOW CLOSE YOU'RE GETTING TO MY LETTER.");

    loop {
        let gen_character = rand::thread_rng().gen_range('A'..='Z'); // generates a random character between A and Z
        let gen_character = String::from(gen_character);
        println!("\nO.K., I HAVE A LETTER.  START GUESSING.");
        for i in 0..999999 {
            println!("\nWHAT IS YOUR GUESS?");

            let mut guess = String::new();

            io::stdin()
                .read_line(&mut guess)
                .expect("Failed to read the line");
            println!("{}", gen_character);
            let guess = guess.trim().to_ascii_uppercase();
            match guess.cmp(&gen_character) {
                Ordering::Less => println!("\nTOO LOW.  TRY A HIGHER LETTER."),
                Ordering::Greater => println!("\nTOO HIGH.  TRY A LOWER LETTER."),
                Ordering::Equal => {
                    println!("\nYOU GOT IT IN {} GUESSES!!", i + 1);
                    if i >= 4 {
                        println!("BUT IT SHOULDN'T TAKE MORE THAN 5 GUESSES!\n");
                    } else {
                        println!("{}", std::iter::repeat("💖").take(15).collect::<String>());
                        println!("GOOD JOB !!!!!");
                    }
                    break;
                }
            }
        }
        println!("\nLET'S PLAY AGAIN.....");
    }
}
