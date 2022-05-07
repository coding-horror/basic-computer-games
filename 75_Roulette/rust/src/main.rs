mod util;

use morristown::Instructions;
use util::INSTRUCTIONS;

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
                let (num, bet) = (bet_input[0], bet_input[1]);

                if num <= 50 && bet < 500 && bet <= player && bet > 0 {
                    bets.push(bet_input);
                } else if bets.contains(&bet_input) {
                    println!("YOU MADE THAT BET ONCE ALREADY, DUM-DUM");
                } else {
                    println!("INVALID BET. TRY AGAIN");
                }
            }
        }

        /*SPIN AND CHECK RESULTS */

        /*CHECK PLAYER AND HOUSE WALLETS */

        /*ASK FOR PLAY AGAIN */

        /*IF NOT PRINT THE CHECK */
    }
}
