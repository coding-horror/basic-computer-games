use rand::{prelude::ThreadRng, Rng};
use std::{fmt, io, mem};

#[derive(PartialEq, Eq, PartialOrd, Ord)]
struct Card(u8);

impl Card {
    fn new_random(rng: &mut ThreadRng) -> Card {
        Card(rng.gen_range(2..15))
    }
}

impl fmt::Display for Card {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        write!(
            f,
            "{}",
            match self.0 {
                11 => String::from("JACK"),
                12 => String::from("QUEEN"),
                13 => String::from("KING"),
                14 => String::from("ACE"),
                otherwise => otherwise.to_string(),
            }
        )
    }
}

struct CardsPool(Card, Card, Card);

impl CardsPool {
    fn new() -> CardsPool {
        let mut rng = rand::thread_rng();
        let mut first = Card::new_random(&mut rng);
        let mut second = Card::new_random(&mut rng);
        let third = Card::new_random(&mut rng);

        if first > second {
            mem::swap(&mut first, &mut second);
        }

        CardsPool(first, second, third)
    }

    fn is_in_win_range(&self) -> bool {
        self.0 <= self.2 && self.2 <= self.1
    }
}

fn main() {
    hello();
    // user start bank
    let mut user_bank: u16 = 100;

    loop {
        println!("YOU NOW HAVE {} DOLLARS.", &mut user_bank);
        println!("\nHERE ARE YOUR NEXT TWO CARDS:");
        // get new random cards
        let cards = CardsPool::new();

        println!("{}", cards.0);
        println!("{}", cards.1);

        let user_bet: u16 = get_bet(user_bank);

        if user_bet == 0 {
            println!("CHICKEN!!!\n");
            continue;
        } else {
            println!("THANK YOU! YOUR BET IS {} DOLLARS.", user_bet);
        }

        println!("\nTHE THIRD CARD IS:");
        println!("{}", cards.2);

        if cards.is_in_win_range() {
            println!("\nYOU WIN!!!");
            user_bank += user_bet;
        } else {
            println!("\nSORRY, YOU LOSE");
            user_bank -= user_bet;
        }

        if user_bank == 0 {
            println!("\nSORRY, FRIEND, BUT YOU BLEW YOUR WAD.");
            println!("\nTRY AGAIN? (YES OR NO)");
            let mut input = String::new();
            io::stdin().read_line(&mut input).expect("Incorrect input");

            if input.trim().to_lowercase() == "yes" {
                user_bank = 100;
                println!();
            } else {
                println!("\nO.K., HOPE YOU HAD FUN!");
                break;
            }
        }
    }
}

fn hello() {
    println!("         🂡  ACEY DUCEY CARD GAME 🂱");
    println!("CREATIVE COMPUTING - MORRISTOWN, NEW JERSEY");
    println!("ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER");
    println!("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP");
    println!("YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING");
    println!("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE");
    println!("A VALUE BETWEEN THE FIRST TWO.");
    println!("IF YOU DO NOT WANT TO BET IN A ROUND, ENTER 0");
    println!("\n\n");
}

fn get_bet(user_bank: u16) -> u16 {
    loop {
        println!("\nWHAT IS YOUR BET? ENTER 0 IF YOU DON'T WANT TO BET (CTRL+C TO EXIT)");
        let bet: u16;
        let mut input = String::new();

        io::stdin()
            .read_line(&mut input)
            .expect("CANNOT READ INPUT!");

        match input.trim().parse::<u16>() {
            Ok(i) => bet = i,
            Err(e) => {
                println!("CHECK YOUR INPUT! {}!", e.to_string().to_uppercase());
                continue;
            }
        };

        match bet {
            bet if bet <= user_bank => return bet,
            _ => {
                println!("\nSORRY, MY FRIEND, BUT YOU BET TOO MUCH.");
                println!("YOU HAVE ONLY {} DOLLARS TO BET.", user_bank);
            }
        };
    }
}
