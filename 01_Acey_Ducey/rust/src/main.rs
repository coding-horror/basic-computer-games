use std::io;
use std::process;
use rand::{Rng, prelude::ThreadRng};

struct CardsPool {
    first: u8, 
    second: u8, 
    third: u8
}
impl CardsPool {
    fn new(rng: &mut ThreadRng)-> CardsPool{
        CardsPool{
            first: rng.gen_range(2..15), 
            second: rng.gen_range(2..15), 
            third: rng.gen_range(2..15)
        }
    }
}


fn main() {
    // user start bank
    let mut user_bank: u16 = 100;
    let mut rng = rand::thread_rng();
    loop {
        println!("YOU NOW HAVE {} DOLLARS.", &mut user_bank);
        println!("HERE ARE YOUR NEXT TWO CARDS:");
        // get new random cards 
        let cards = CardsPool::new(&mut rng);
        println!("{}", card_name(cards.first));
        println!("{}", card_name(cards.second));
        let mut user_bet: u16;
        user_bet = get_bet(user_bank);
        if user_bet == 0 {
            println!("CHICKEN!!!");
            continue;
        }
        println!("{}", card_name(cards.third));

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
}

fn card_name(card: u8) -> String {
    match card {
        11 => String::from("JACK"),
        12 => String::from("QUEEN"),
        13 => String::from("KING"),
        14 => String::from("ACE"),
        _  => card.to_string()
    }
}


fn get_bet(user_bank: u16) -> u16 {
    println!("WHAT IS YOUR BET? ENTER 0 IF YOU DON'T WANT TO BET (CTRL+C TO EXIT)");
    let mut bet: u16;
    let mut input = String::new();

    io::stdin()
        .read_line(&mut input)
        .expect("Sorry your input incorrect");

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

