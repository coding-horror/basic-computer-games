/*
 * Rock paper scissors
 * Originally from the wonderful book: _Basic Computer Games_
 * Port to Rust By David Lotts
*/
use nanorand::{tls::TlsWyRand, Rng};
use std::io::{self, Write};
use strum::EnumCount;
use strum_macros::{Display, EnumCount, FromRepr};
use text_io::try_read;
fn main() {
    let mut computer_wins = 0;
    let mut human_wins = 0;
    let mut rng = nanorand::tls_rng();
    println!("{:>21}", "GAME OF ROCK, SCISSORS, PAPER");
    println!("{:>15}", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    print!("\n\n\n");
    // pass by reference in rust! input() modifies this variable.
    let mut qty_games = 0;
    loop {
        input_int("HOW MANY GAMES", &mut qty_games);
        if qty_games < 11 {
            break;
        }
        println!("SORRY, BUT WE AREN'T ALLOWED TO PLAY THAT MANY.");
    }
    for game_number in 1..=qty_games {
        println!();
        println!("GAME NUMBER {}", game_number);
        let mut your_choice = 0;
        loop {
            println!("3=ROCK...2=SCISSORS...1=PAPER");
            input_int("1...2...3...WHAT'S YOUR CHOICE", &mut your_choice);
            // interesting validation in original BASIC: IF (K-1)*(K-2)*(K-3)==0
            if (1..=3).contains(&your_choice) {
                break;
            }
            println!("INVALID.");
        }
        // Convert number to enum.  Note the type change.  Really it is a new variable.
        let your_choice = Choice::from_repr((your_choice - 1) as usize).unwrap();
        let my_choice = Choice::new_random(&mut rng);

        println!("THIS IS MY CHOICE...");
        println!("...{}", my_choice.to_string());
        let winner = Winner::decide_winner(my_choice, your_choice);
        println!(
            "{}",
            match winner {
                Winner::Tie => {
                    "TIE GAME.  NO WINNER."
                }
                Winner::Computer => {
                    computer_wins = computer_wins + 1;
                    "WOW!  I WIN!!!"
                }
                Winner::Human => {
                    human_wins = human_wins + 1;
                    "YOU WIN!!!"
                }
            }
        )
    }
    println!();
    println!("HERE IS THE FINAL GAME SCORE:");
    println!("I HAVE WON {} GAME(S).", computer_wins);
    println!("YOU HAVE WON {} GAME(S).", human_wins);
    println!(
        "AND {} GAME(S) ENDED IN A TIE.",
        qty_games - (computer_wins + human_wins)
    );
    println!();
    println!("THANKS FOR PLAYING!!");
}

#[derive(FromRepr, Debug, PartialEq, EnumCount, Display)]
pub enum Choice {
    PAPER,
    SCISSORS,
    ROCK,
}

impl Choice {
    /// Returns randomly selected paper..rock.
    fn new_random(rng: &mut TlsWyRand) -> Choice {
        Choice::from_repr(rng.generate_range(0..Choice::COUNT)).unwrap()
    }
}

#[derive(FromRepr, Debug, PartialEq, EnumCount)]
pub enum Winner {
    Human,
    Computer,
    Tie,
}

impl Winner {
    /// take opponent's choices and decide the winner
    // I really learned alot about enums here, and now you can too!
    // Originally I broke this out for auto testing.
    pub fn decide_winner(my_choice: Choice, your_choice: Choice) -> Winner {
        let my_choice = my_choice as u8;
        let your_choice = your_choice as u8;
        if my_choice == your_choice {
            return Winner::Tie;
        }
        // wordy but clear way:
        //    if (my_choice == 1 && your_choice == 3) || (my_choice > your_choice)
        // consice but opaque way:
        if 1 == (3 + my_choice - your_choice) % 3 {
            return Winner::Computer;
        }
        return Winner::Human;
    }
}

/// print the prompt, wait for a number and newline.  Loop if invalid.
fn input_int(prompt: &str, number: &mut i32) {
    loop {
        print!("{} ? ", prompt);
        io::stdout().flush().unwrap();
        if let Ok(n) = try_read!() {
            *number = n;
            return;
        }
    }
}

/// Test winner decider for every case.
#[cfg(test)]
mod tests {
    #[test]
    fn winner_test() {
        use super::*;
        use Choice::*;
        use Winner::*; //decide_winner(my_choice=computer, your_choice=human)
        assert_eq!(Winner::decide_winner(PAPER, PAPER), Tie);
        assert_eq!(Winner::decide_winner(PAPER, SCISSORS), Human);
        assert_eq!(Winner::decide_winner(PAPER, ROCK), Computer);
        assert_eq!(Winner::decide_winner(SCISSORS, PAPER), Computer);
        assert_eq!(Winner::decide_winner(SCISSORS, SCISSORS), Tie);
        assert_eq!(Winner::decide_winner(SCISSORS, ROCK), Human);
        assert_eq!(Winner::decide_winner(ROCK, PAPER), Human);
        assert_eq!(Winner::decide_winner(ROCK, SCISSORS), Computer);
        assert_eq!(Winner::decide_winner(ROCK, ROCK), Tie);
    }
}
