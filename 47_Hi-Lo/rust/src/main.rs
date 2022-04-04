use rand::Rng;
use std::io;

fn main() {
    println!(
        "{: >39}\n{: >57}\n\n\n",
        "HI LO", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    );
    println!("THIS IS THE GAME OF HI LO.\n");
    println!("YOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE");
    println!("HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS.  IF YOU");
    println!("GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!");
    println!("THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,");
    println!("IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.\n");

    let mut total: u32 = 0;
    loop {
        let jackpot_amount = rand::thread_rng().gen_range(1..101); // generates a random number between 1 and 100
        for i in 0..6 {
            println!("YOUR GUESS?");

            let mut guess = String::new();

            io::stdin()
                .read_line(&mut guess)
                .expect("Failed to read the line");

            // this converts the input string into unsigned 32bit number and if the input entered is not a number
            // it will again prompt the user to enter the guess number
            let guess: u32 = match guess.trim().parse() {
                Ok(num) => num,
                Err(_) => {
                    println!("PLEASE ENTER A NUMBER VALUE.\n");
                    continue;
                }
            };

            // compare it with the jackpot amount
            if guess == jackpot_amount {
                println!("\nGOT IT!!!!!!!!!!   YOU WIN {} DOLLARS.", jackpot_amount);
                total += jackpot_amount;
                println!("YOUR TOTAL WINNINGS ARE NOW {} DOLLARS.\n", total);
                break;
            } else if guess < jackpot_amount {
                println!("YOUR GUESS IS TOO LOW.\n");
            } else {
                println!("YOUR GUESS IS TOO HIGH.\n");
            }

            // if 6 tries are over make total jackpot amount to zero
            if i == 5 {
                total = 0;
                println!(
                    "YOU BLEW IT...TOO BAD...THE NUMBER WAS {}\n",
                    jackpot_amount
                );
            }
        }
        println!("PLAY AGAIN (YES OR NO)?");
        let mut tocontinue = String::new();
        io::stdin()
            .read_line(&mut tocontinue)
            .expect("Error Getting your input");
        let tocontinue = tocontinue.trim().to_ascii_uppercase();
        if tocontinue.eq("YES") {
            println!("\n");
            continue;
        } else {
            println!("\nSO LONG.  HOPE YOU ENJOYED YOURSELF!!!\n");
            break;
        }
    }
}
