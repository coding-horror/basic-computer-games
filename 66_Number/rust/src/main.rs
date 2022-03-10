use rand::{Rng, prelude::thread_rng};
use std::io;

fn main() {
    //DATA
    let mut points: usize = 100;
    let mut rng = thread_rng();
    let mut number:u8;

    //print welcome message
    welcome();

    //game loop
    while points <= 500 {
        //generate number
        number = rng.gen_range(1..=5);
        //NOTE: while looking at the original basic, I realized that the outcome of your guess is effectively random
        //so instead of generating 5 variables with random values between 1-5 and doing something depedning which one has the value they guess...
        //why not just let them "guess" and do a random action without using uneeded variables? .. so that's what I did.

        //let them "guess"
        println!("GUESS A NUMBER FROM 1 TO 5");//print prompt
        if let Ok(_i) = io::stdin().read_line(&mut String::new()) {} // get input from standard in, and do nothing with it even if an error is thrown

        //do something depending on the previously generated random number
        match number {
            1 => if points>=5{points -= 5},//the if statement here prevents overflow, points is stored as an unsigned integer, so we can't let it be negative
            2 => points += 5,
            3 => {//jackpot
                points *= 2;
                println!("YOU HIT THE JACKPOT!!!");
            },
            4 => points += 1,
            5 => points /= 2,
            _ => {},
        };

        //tell then how many points they have
        println!("YOU HAVE {} POINTS.", points);
    }

    //print
}

/**
 * print the welcome message
 */
fn welcome() {
    println!("
              CREATIVE COMPUTING MORRISTOWN, NEW JERSEY



    YOU HAVE 100 POINTS.  BY GUESSING NUMBERS FROM 1 TO 5, YOU
    CAN GAIN OR LOSE POINTS DEPENDING UPON HOW CLOSE YOU GET TO
    A RANDOM NUMBER SELECTED BY THE COMPUTER.

    YOU OCCASIONALLY WILL GET A JACKPOT WHICH WILL DOUBLE(!)
    YOUR POINT COUNT.  YOU WIN WHEN YOU GET 500 POINTS

    ");
}
