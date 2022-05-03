use crate::progress::Progress;
use rand::Rng;
use std::io;

pub struct WordGame<'a> {
    word: &'a str,
    progress: Progress,
    guess: String,
    guesses: usize,
}

impl WordGame<'_> {
    pub fn new() -> Self {
        const WORDS: [&str; 12] = [
            "DINKY", "SMOKE", "WATER", "GRASS", "TRAIN", "MIGHT", "FIRST", "CANDY", "CHAMP",
            "WOULD", "CLUMP", "DOPEY",
        ];

        println!("\nYou are starting a new game...");

        let random_index: usize = rand::thread_rng().gen_range(0..WORDS.len());

        //println!("word is: {}", WORDS[random_index]);

        WordGame {
            word: WORDS[random_index],
            progress: Progress::new(),
            guess: String::new(),
            guesses: 0,
        }
    }

    pub fn tick(&mut self) -> bool {
        self.guesses += 1;

        println!("\n\nGuess a five letter word?");

        let mut game_over = false;

        if WordGame::<'_>::read_guess(self) {
            game_over = WordGame::<'_>::process_guess(self);
        }

        game_over
    }

    fn read_guess(&mut self) -> bool {
        let mut guess = String::new();

        io::stdin()
            .read_line(&mut guess)
            .expect("Failed to read line.");

        let invalid_input = |message: &str| {
            println!("\n{} Guess again.", message);
            return false;
        };

        let guess = guess.trim();

        for c in guess.chars() {
            if c.is_numeric() {
                return invalid_input("Your guess cannot include numbers.");
            }
            if !c.is_ascii_alphabetic() {
                return invalid_input("Your guess must only include ASCII characters.");
            }
        }

        if guess.len() != 5 {
            return invalid_input("You must guess a 5 letter word");
        }

        self.guess = guess.to_string();

        true
    }

    fn process_guess<'a>(&mut self) -> bool {
        let guess = self.guess.to_uppercase();

        let mut matches: Vec<char> = Vec::new();

        for (i, c) in guess.chars().enumerate() {
            if self.word.contains(c) {
                matches.push(c);

                if self.word.chars().nth(i).unwrap() == c {
                    self.progress.set_char_at(c, i);
                }
            }
        }

        let match_amount = matches.len();

        println!(
            "There were {} matches and the common letters were....{}",
            match_amount,
            matches.into_iter().collect::<String>()
        );

        print!("From the exact letter matches you know....");
        self.progress.print();

        if match_amount == 5 {
            println!(
                "\n\nYou have guessed the word. It took {} guesses!\n",
                self.guesses
            );
            return true;
        }

        false
    }
}
