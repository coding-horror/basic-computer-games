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
        println!("HERE ARE YOUR NEXT TWO CARDS:");
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
            println!("YOU WIN!!!\n");
            user_bank += user_bet;
        } else {
            println!("SORRY, YOU LOSE\n");
            user_bank -= user_bet;
        }

        if user_bank == 0 {
            println!("\nSORRY, FRIEND, BUT YOU BLEW YOUR WAD.\n");
            println!("TRY AGAIN? (yes OR no)");
            let mut input = String::new();
            io::stdin().read_line(&mut input).expect("Incorrect input");

            if "yes" == input {
                user_bank = 100;
            } else {
                println!("O.K., HOPE YOU HAD FUN!");
            }
        }
    }
}

fn hello() {
    println!(" 🂡  ACEY DUCEY CARD GAME 🂱");
    println!("CREATIVE COMPUTING - MORRISTOWN, NEW JERSEY");
    println!(" ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER");
    println!("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP");
    println!("YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING");
    println!("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE");
    println!("A VALUE BETWEEN THE FIRST TWO.");
    println!("IF YOU DO NOT WANT TO BET IN A ROUND, ENTER 0");
    println!("\n\n\n");
}

fn get_bet(user_bank: u16) -> u16 {
    println!("WHAT IS YOUR BET? ENTER 0 IF YOU DON'T WANT TO BET (CTRL+C TO EXIT)");
    let bet: u16;
    let mut input = String::new();

    io::stdin()
        .read_line(&mut input)
        .expect("Sorry your input incorrect");

    // XXX: Unhandled input
    bet = input.trim().parse::<u16>().unwrap();
    match bet {
        0 => bet,
        bet if bet < user_bank => bet,
        _ => {
            println!("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.");
            println!("YOU HAVE ONLY {} DOLLARS TO BET.", user_bank);
            get_bet(user_bank)
        }
    }
}
