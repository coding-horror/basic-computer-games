use std::io;
use rand::{Rng, prelude::ThreadRng};

struct CardsPool {
    first: u8, 
    second: u8, 
    third: u8
}
impl CardsPool {
    fn new(rng: &mut ThreadRng)-> CardsPool{
        let mut f = rng.gen_range(2..15);
        let mut s = rng.gen_range(2..15);

        if f > s {
           let x = f;
           f = s;
           s = x;
        }

        CardsPool{
            first: f, 
            second: s,
            third: rng.gen_range(2..15)
        }
    }
}


fn main() {
    hello();
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
            println!("CHICKEN!!!\n");
            continue;
        }
        else {
            println!("THANK YOU! YOUR BET IS {} DOLLARS.", &mut user_bet);
        }

        println!("\nTHE THIRD CARD IS:");
        println!("{}", card_name(cards.third));

        if cards.first <= cards.third && cards.third <= cards.second {
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
            io::stdin()
                .read_line(&mut input)
                .expect("Incorrect input");

            if String::from("yes") == input {
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
    let bet: u16;
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

