//! Rust port of "Bowl" (14_Bowling) from _Basic Computer Games_ (1978).
use rand::{rngs::ThreadRng, RngExt};
use std::io::{self, BufRead, Write};

/// Number of cells in the pin-accumulator array (`DIM C(15)` in BASIC).
const CELLS: usize = 15;
/// The first ten cells correspond to the ten real pins drawn on the diagram.
const PINS: usize = 10;

fn main() {
    println!("{:>34}BOWL", "");
    println!("{:>15}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", "");
    println!("\n\n");

    println!("WELCOME TO THE ALLEY");
    println!("BRING YOUR FRIENDS");
    println!("OKAY LET'S FIRST GET ACUUAINTED");
    println!();

    let mut game = BowlingGame::new();
    loop {
        let answer = prompt_line("THE INSTRUCTIONS (Y/N)");
        if answer.starts_with('Y') {
            print_instructions();
        } else if answer.starts_with('N') {
            // fall through to the game
        } else {
            continue;
        }

        game.play();

        let again = prompt_line("DO YOU WANT ANOTHER GAME");
        if !again.starts_with('Y') {
            break;
        }
    }
}

fn print_instructions() {
    println!("THE GAME OF BOWLING TAKES MIND AND SKILL.DURING THE GAME");
    println!("THE COMPUTER WILL KEEP SCORE.YOU MAY COMPETE WITH");
    println!("OTHER PLAYERS[UP TO FOUR].YOU WILL BE PLAYING TEN FRAMES");
    println!("ON THE PIN DIAGRAM 'O' MEANS THE PIN IS DOWN...'+' MEANS THE");
    println!("PIN IS STANDING.AFTER THE GAME THE COMPUTER WILL SHOW YOUR");
    println!("SCORES .");
}

struct BowlingGame {
    rng: ThreadRng,
}

impl BowlingGame {
    fn new() -> Self {
        Self { rng: rand::rng() }
    }

    /// Play one complete game (10 frames, all players), then print the scorecard.
    fn play(&mut self) {
        let players = loop {
            // "FIRST OF ALL...HOW MANY ARE PLAYING"
            let line = prompt_line("FIRST OF ALL...HOW MANY ARE PLAYING");
            if line.is_empty() {
                // EOF on input — bail out of this game.
                return;
            }
            if let Ok(n) = line.trim().parse::<usize>() {
                if (1..=4).contains(&n) {
                    break n;
                }
            }
            println!("?RE-ENTER");
        };

        println!();
        println!("VERY GOOD...");

        // One row per player: [ball1, ball2, status] for each of 10 frames.
        let mut scores: Vec<[[u32; 3]; 10]> = (0..players).map(|_| [[0; 3]; 10]).collect();

        for frame in 1..=10 {
            for (player_idx, player_scores) in scores.iter_mut().enumerate() {
                self.play_ball(frame, player_idx + 1, player_scores);
            }
        }

        print_scorecard(&scores);
    }

    /// Roll the balls for one player in one frame, recording the result.
    fn play_ball(&mut self, frame: usize, player: usize, scores: &mut [[u32; 3]; 10]) {
        let mut prev_down = 0; // M: pins down after the previous ball this frame
        let mut status = 0; // Q: 3 strike, 2 spare, 1 error, 0 otherwise

        let mut cells = [0u32; CELLS + 1];

        for ball in 1..=2 {
            // "TYPE ROLL TO GET THE BALL GOING."
            let _ = prompt_line("TYPE ROLL TO GET THE BALL GOING");

            // Knock down more pins: 20 random hits, each landing in cell
            // (15*J - X) where J is the smallest bucket with X < 15*J.
            self.roll_pins(&mut cells);

            // Draw the triangle diagram (only the first 10 cells are real pins).
            print!("PLAYER:{} FRAME:{} BALL:{}\n", player, frame, ball);
            print_pin_diagram(&cells);

            // Count pins down among the real pins (cells 1..=10).
            let down = cells[1..=PINS].iter().filter(|&&c| c == 1).count() as u32;

            // Roll analysis. Order mirrors the original IF chain.
            if down == prev_down {
                println!("GUTTER!!");
            }
            if ball == 1 && down == 10 {
                println!("STRIKE!!!!!\x07\x07\x07\x07");
                status = 3;
            }
            if ball == 2 && down == 10 {
                println!("SPARE!!!!");
                status = 2;
            }
            if ball == 2 && down < 10 {
                println!("ERROR!!!");
                status = 1;
            }
            if ball == 1 && down < 10 {
                println!("ROLL YOUR 2ND BALL");
            }
            println!();

            // Record the score for this ball: total pins down so far this frame.
            scores[frame - 1][ball - 1] = down;

            if ball == 1 {
                prev_down = down;
                // On a strike, the frame ends; otherwise prompt the second ball.
                if status == 3 {
                    scores[frame - 1][1] = 0;
                    break;
                }
            }
        }

        scores[frame - 1][2] = status;
    }

    fn roll_pins(&mut self, cells: &mut [u32]) {
        for _ in 0..20 {
            let x = (self.rng.random::<f32>() * 100.0) as usize; // INT(RND(1)*100)
            // Find the smallest J in 1..=10 with x < 15*J.
            let mut j = 1;
            while j <= 10 && x >= 15 * j {
                j += 1;
            }
            // j may exceed 10 if x >= 150 (impossible here since x < 100) — guard anyway.
            if j <= 10 {
                let idx = 15 * j - x; // 1..=15
                if idx >= 1 && idx <= CELLS {
                    cells[idx] = 1;
                }
            }
        }
    }
}

/// Draw the standing/down pins as a triangle: '+' standing, 'O' down.
fn print_pin_diagram(cells: &[u32]) {
    let mut k = 1; // pin index, 1-based (K in BASIC)
    for i in 0..4 {
        print!("{}", " ".repeat(i));
        for _ in 0..(4 - i) {
            if cells[k] == 1 {
                print!("O ");
            } else {
                print!("+ ");
            }
            k += 1;
        }
        println!();
    }
    println!();
}

fn print_scorecard(scores: &[[[u32; 3]; 10]]) {
    println!("FRAMES");
    for frame in 1..=10 {
        print!("{} ", frame);
    }
    println!();
    for player_scores in scores {
        for row in 0..3 {
            for frame in 0..10 {
                print!("{} ", player_scores[frame][row]);
            }
            println!();
        }
        println!();
    }
}

fn prompt_line(prompt: &str) -> String {
    print!("{}? ", prompt);
    let _ = io::stdout().lock().flush();

    let mut buffer = String::new();
    // Treat EOF (or a read error) as an empty line so callers can decide what
    // to do — for this game, an empty answer never matches Y/N, so the program
    // simply exits its current loop instead of spinning forever on EOF.
    let _ = io::stdin().lock().read_line(&mut buffer);
    buffer.trim().to_uppercase()
}
