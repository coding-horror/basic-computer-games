use std::io;


fn main() {
    let mut user_bank: u16 = 100;

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
