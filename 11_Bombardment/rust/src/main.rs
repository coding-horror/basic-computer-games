use morristown::PromptMultiOption;
use rand::{prelude::SliceRandom, thread_rng};
use std::time::Duration;

fn main() {
    morristown::print_intro("BOMBARDMENT");

    println!(
        r#"

YOU ARE ON A BATTLEFIELD WITH 4 PLATOONS AND YOU
HAVE 25 OUTPOSTS AVAILABLE WHERE THEY MAY BE PLACED.
YOU CAN ONLY PLACE ONE PLATOON AT ANY ONE OUTPOST.
THE COMPUTER DOES THE SAME WITH ITS FOUR PLATOONS.

THE OBJECT OF THE GAME IS TO FIRE MISSLES AT THE
OUTPOSTS OF THE COMPUTER.  IT WILL DO THE SAME TO YOU.
THE ONE WHO DESTROYS ALL FOUR OF THE ENEMY'S PLATOONS
FIRST IS THE WINNER.

GOOD LUCK... AND TELL US WHERE YOU WANT THE BODIES SENT!

TEAR OFF MATRIX AND USE IT TO CHECK OFF THE NUMBERS.


"#
    );

    let mut all_positions = Vec::with_capacity(25);

    for i in 1u8..=25 {
        all_positions.push(i);

        print!("{i}\t");
        if (i % 5) == 0 {
            println!();
        }
    }

    let mut player_positions = morristown::prompt_multi_number::<u8>(
        "WHAT ARE YOUR FOUR POSITIONS?\n",
        ",",
        Some(PromptMultiOption::UnitAmount(4)),
        Some(1..=25),
    );

    let mut rng = thread_rng();

    let ai_positions = rand::seq::index::sample(&mut rng, 24, 4).into_vec();
    let mut ai_positions: Vec<u8> = ai_positions.iter().map(|i| (i + 1) as u8).collect();

    loop {
        if !player_turn(&mut ai_positions) {
            break;
        } else if !ai_turn(&mut all_positions, &mut player_positions) {
            break;
        }
    }
}

fn player_turn(ai_positions: &mut Vec<u8>) -> bool {
    let player_missile =
        morristown::prompt_number_range::<u8>("WHERE DO YOU WISH TO FIRE YOUR MISSILE?", 1..=25);

    if let Some(index) = ai_positions.iter().position(|p| *p == player_missile) {
        ai_positions.remove(index);

        let remaining = ai_positions.len();

        if remaining > 0 {
            let down = 4 - remaining;

            println!(
                "YOU GOT ONE OF MY OUTPOSTS.\n{} DOWN, {} TO GO\n",
                get_text_from_number(down),
                get_text_from_number(remaining)
            );
        } else {
            println!(
                "YOU GOT ME, I'M GOING FAST. BUT I'LL GET YOU WHEN MY TRANSISTO&S RECUP%RA*E!\n\n"
            );
            return false;
        }
    } else {
        println!("HA, HA YOU MISSED. MY TURN NOW\n");
    }
    true
}

fn ai_turn(all_positions: &mut Vec<u8>, player_positions: &mut Vec<u8>) -> bool {
    std::thread::sleep(Duration::from_secs(1));

    let ai_missile = *all_positions
        .choose(&mut rand::thread_rng())
        .expect("AI RAN OUT OF OPTIONS!");

    let index = all_positions
        .iter()
        .position(|p| p == &ai_missile)
        .expect("AI CHOOSE AN INVALID POSITION!");

    all_positions.remove(index);

    if let Some(index) = player_positions.iter().position(|p| p == &ai_missile) {
        player_positions.remove(index);

        let remaining = player_positions.len();

        if remaining > 0 {
            println!("I GOT YOU. IT WON'T BE LONG NOW. POST {ai_missile} WAS HIT.");
            println!(
                "YOU HAVE ONLY {} OUTPOST LEFT.\n",
                get_text_from_number(remaining)
            );
        } else {
            println!("YOU'RE DEAD. YOUR LAST OUTPOST WAS AT {ai_missile} . HA, HA, HA.");
            println!("BETTER LUCK NEXT TIME.");
            return false;
        }
    } else {
        println!("I MISSED YOU, YOU DIRTY RAT. I PICKED {ai_missile} . YOUR TURN.\n")
    }

    true
}

fn get_text_from_number(n: usize) -> &'static str {
    match n {
        1 => "ONE",
        2 => "TWO",
        3 => "THREE",
        _ => panic!("INVALID INDEX!"),
    }
}
