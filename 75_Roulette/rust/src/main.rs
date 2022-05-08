mod util;

use morristown::Instructions;
use rand::Rng;
use util::INSTRUCTIONS;

use crate::util::print_check;

fn main() {
    morristown::print_intro("ROULETTE");

    let date = morristown::prompt_multi(
        "ENTER CURRENT DATE (AS IN 'JANUARY 23, 1978)",
        ",",
        Some((2, 2)),
    );

    Instructions::new_multiline(
        true,
        false,
        "DO YOU WANT INSTRUCTIONS?",
        INSTRUCTIONS.to_vec(),
    )
    .print();

    let mut house: usize = 100000;
    let mut player: usize = 1000;

    loop {
        let bet_count = morristown::prompt_number_range::<u8>("HOW MANY BETS?", 1, std::u8::MAX);
        let mut bets = Vec::new();

        for i in 1..=bet_count {
            loop {
                let msg = format!("NUMBER {}?", i);
                let bet_input =
                    morristown::prompt_multi_number::<usize>(msg.as_str(), ",", Some((2, 2)));
                let (bet_num, wager) = (bet_input[0], bet_input[1]);

                if bet_num > 0 && bet_num <= 50 && wager > 5 && wager < 500 && wager <= player {
                    bets.push(bet_input);
                } else if bets.contains(&bet_input) {
                    println!("YOU MADE THAT BET ONCE ALREADY, DUM-DUM");
                } else {
                    println!("INVALID BET. TRY AGAIN");
                }
            }
        }

        /*SPIN AND CHECK RESULTS */
        println!("\nSPINNING");
        let spin: u8 = rand::thread_rng().gen_range(1..=38);

        let color = if util::REDS.contains(&spin) {
            "RED"
        } else {
            "BLACK"
        };

        println!("\n{} {}", spin, color);

        for (i, bet) in bets.iter().enumerate() {
            let (bet_num, wager) = (bet[0] as u8, bet[1]);
            let (win, payoff) = util::process_bet(bet_num, spin);

            let msg = if win {
                let wager = wager * payoff as usize;
                player += wager;
                house -= wager;
                "WIN"
            } else {
                player -= wager;
                house += wager;
                "LOSE"
            };

            println!("YOU {msg} {wager} DOLLARS ON BET {}", i + 1);
        }

        println!("TOTALS\tME\tYOU");
        println!("\t\t{house}\t{player}");

        if player <= 0 {
            println!("OOPS! YOU JUST SPENT YOUR LAST DOLLAR");
            println!("THANKS FOR YOUR MONEY");
            println!("I'LL USE IT TO BUY A SOLID GOLD ROULETTE WHEEL");
            break;
        }

        if house <= 0 {
            println!("YOU BROKE THE HOUSE!");
            print_check(player);
            break;
        }

        if !morristown::prompt_bool("AGAIN?", false) {
            print_check(player);
            break;
        }
    }
}
