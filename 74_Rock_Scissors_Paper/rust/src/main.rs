/*
 * Rock paper scissors
 * Port from book _Basic Computer Games_
 * By David Lotts
*/
use rand::Rng;
use std::io::{self, Write};
use text_io::{try_read};
fn main() {
    let mut computer_wins = 0;
    let mut human_wins = 0;
    let mut rng = rand::thread_rng();
    println!("{:>21}", "GAME OF ROCK, SCISSORS, PAPER");
    println!("{:>15}", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    print!("\n\n\n");
    // pass by reference in rust! input() modifies num_games_q.
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
        let my_choice: i32 = rng.gen_range(1..=3); // basic: X=INT(RND(1)*3+1)
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
        println!("THIS IS MY CHOICE...");
        println!(
            "...{}",
            match my_choice {
                1 => "PAPER",
                2 => "SCISSORS",
                3 => "ROCK",
                _ => "um, what?",
            }
        );
        if my_choice == your_choice {
            //THEN 250
            println!("TIE GAME.  NO WINNER.");
        } else {
            //if (my_choice == 1 && your_choice == 3)
            //    || (my_choice > your_choice)
            if 1 == (3 + my_choice - your_choice) % 3
            {
                println!("WOW!  I WIN!!!");
                computer_wins = computer_wins + 1;
            } else {
                println!("YOU WIN!!!");
                human_wins = human_wins + 1;
            }
        }
    }
    println!();
    println!("HERE IS THE FINAL GAME SCORE:");
    println!("I HAVE WON {} GAME(S).", computer_wins);
    println!("YOU HAVE WON {} GAME(S).", human_wins);
    println!("AND {} GAME(S) ENDED IN A TIE.", qty_games - (computer_wins + human_wins));
    println!();
    println!("THANKS FOR PLAYING!!");
}

fn input_int(prompt: &str, number: &mut i32) {
    loop {
        print!("{} ? ", prompt);
        io::stdout().flush().unwrap();
        match try_read!() {
            Ok(n) => {
                *number = n;
                return;
            }
            Err(_) => println!("{}", prompt),
        }
    }
}


// TODO break out the winner decider and test it.
#[cfg(test)]
mod tests {
    #[test]
    fn winner_test() {
        assert_eq!(true,true);
    }
}