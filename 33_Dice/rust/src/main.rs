////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Dice
//
// From: BASIC Computer Games (1978)
//       Edited by David H. Ahl
//
// "Not exactly a game, this program simulates rolling
//  a pair of dice a large number of times and prints out
//  the frequency distribution.  You simply input the
//  number of rolls.  It is interesting to see how many
//  rolls are necessary to approach the theoretical
//  distribution:
//
//  2  1/36  2.7777...%
//  3  2/36  5.5555...%
//  4  3/36  8.3333...%
//    etc.
//
// "Daniel Freidus wrote this program while in the
//  seventh grade at Harrison Jr-Sr High School,
//  Harrison, New York."
//
// Rust Port by Jay, 2022
//
////////////////////////////////////////////////////////////////////////////////////////////////////////////////

use rand::Rng;
use std::io::{self, Write};

fn main() {
    let mut frequency: [i32; 13] = [0; 13];
    println!(
        "{: >38}\n{: >57}\n\n",
        "DICE", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    );
    // DANNY FREIDUS
    println!("THIS PROGRAM SIMULATES THE ROLLING OF A");
    println!("PAIR OF DICE.");
    println!("YOU ENTER THE NUMBER OF TIMES YOU WANT THE COMPUTER TO");
    println!("'ROLL' THE DICE.  WATCH OUT, VERY LARGE NUMBERS TAKE");
    println!("A LONG TIME.  IN PARTICULAR, NUMBERS OVER 5000.");
    let mut playing = true;
    while playing {
        let n = match readinput(&"HOW MANY ROLLS").trim().parse::<i32>() {
            Ok(num) => num,
            Err(_) => {
                println!("PLEASE ENTER A NUMBER");
                continue;
            }
        };
        // Dice Rolled n times
        for _i in 0..n {
            let die_1 = rand::thread_rng().gen_range(1..=6);
            let die_2 = rand::thread_rng().gen_range(1..=6);
            let total = die_1 + die_2;
            frequency[total] += 1;
        }

        // Results tabel
        println!("\nTOTAL SPOTS    NUMBER OF TIMES");
        for i in 2..13 {
            println!("{:^4}\t\t{}", i, frequency[i]);
        }

        // Continue the game
        let reply = readinput("TRY AGAIN").to_ascii_uppercase();
        if reply.starts_with("Y") || reply.eq("YES") {
            frequency = [0; 13];
        } else {
            playing = false;
        }
    }
}

// function for getting input on same line
fn readinput(str: &str) -> String {
    print!("\n{}? ", str);
    let mut input = String::new();
    io::stdout().flush().unwrap();
    io::stdin()
        .read_line(&mut input)
        .expect("Failed to get Input");
    input
}
