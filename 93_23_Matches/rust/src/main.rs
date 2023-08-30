use std::io;
use std::io::{stdin, stdout, BufRead, Write};

fn main() -> io::Result<()> {
    intro();
    let mut input = stdin().lock();
    let mut buf = String::with_capacity(16);
    // variable `N` in the original game
    let mut matches: u8 = 23;
    if fastrand::bool() {
        println!("TAILS! YOU GO FIRST. \n");
    } else {
        println!("HEADS! I WIN! HA! HA!\nPREPARE TO LOSE, MEATBALL-NOSE!!\n\nI TAKE 2 MATCHES");
        matches -= 2;
        println!("THE NUMBER OF MATCHES IS NOW {matches}\n");
        println!("YOUR TURN -- YOU MAY TAKE 1, 2 OR 3 MATCHES.");
    }
    loop {
        // variable `K` in the original game
        let human_picked = read_matches(&mut input, &mut buf)?;
        matches = matches.saturating_sub(human_picked);
        if matches == 0 {
            // this can only happen if the player could win with the next turn but they take too
            // many matches (e.g. if there are three matches left and the player takes three instead of
            // two). In the original game, this would count as a win for the player.
            println!("\nYOU POOR BOOB! YOU TOOK THE LAST MATCH! I GOTCHA!!\nHA ! HA ! I BEAT YOU !!!\n\nGOOD BYE LOSER!");
            return Ok(());
        }

        println!("THERE ARE NOW {matches} MATCHES REMAINING.");

        if matches <= 1 {
            println!("YOU WON, FLOPPY EARS !\nTHINK YOU'RE PRETTY SMART !\nLETS PLAY AGAIN AND I'LL BLOW YOUR SHOES OFF !!");
            return Ok(());
        }

        // variable `Z` in the original game
        let ai_picked = ai_pick(matches, human_picked);
        println!("MY TURN ! I REMOVE {ai_picked} MATCHES");
        matches = matches.saturating_sub(ai_picked);
        // The AI will never pick the last match except for the case where only one match is left (which is handled above)
        if matches == 1 {
            println!("\nYOU POOR BOOB! YOU TOOK THE LAST MATCH! I GOTCHA!!\nHA ! HA ! I BEAT YOU !!!\n\nGOOD BYE LOSER!");
            return Ok(());
        }
        println!("THE NUMBER OF MATCHES IS NOW {matches}\n");
        println!("YOUR TURN -- YOU MAY TAKE 1, 2 OR 3 MATCHES.");
    }
}

fn intro() {
    println!(
        r"                               23 MATCHES
               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



 THIS IS A GAME CALLED '23 MATCHES'.

WHEN IT IS YOUR TURN, YOU MAY TAKE ONE, TWO, OR THREE
MATCHES. THE OBJECT OF THE GAME IS NOT TO HAVE TO TAKE
THE LAST MATCH.

LET'S FLIP A COIN TO SEE WHO GOES FIRST.
IF IT COMES UP HEADS, I WILL WIN THE TOSS.
"
    );
}

fn read_matches<R: BufRead>(mut input: R, buf: &mut String) -> io::Result<u8> {
    print!("HOW MANY DO YOU WISH TO REMOVE ?? ");
    stdout().flush()?;
    loop {
        let input = read_int(&mut input, buf)?;
        if input <= 0 || input > 3 {
            print!("VERY FUNNY! DUMMY!\nDO YOU WANT TO PLAY OR GOOF AROUND?\nNOW, HOW MANY MATCHES DO YOU WANT              ?? ");
            stdout().flush()?;
        } else {
            return Ok(input as u8);
        }
    }
}

fn read_int<R: BufRead>(mut input: R, buf: &mut String) -> io::Result<i8> {
    loop {
        buf.clear();
        input.read_line(buf)?;
        let line = buf.trim();
        // This is implicit behaviour in the original code: empty input is equal to 0
        if line.is_empty() {
            return Ok(0);
        }
        if let Ok(n) = line.parse::<i8>() {
            return Ok(n);
        } else {
            print!("??REENTER\n?? ");
            stdout().flush()?;
        }
    }
}

fn ai_pick(matches: u8, human_picked: u8) -> u8 {
    if matches < 4 {
        matches - 1
    } else {
        4 - human_picked
    }
}
