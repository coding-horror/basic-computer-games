use rand::{RngExt, rngs::ThreadRng};
use std::io::{self, Write};

fn main() {
    println!("{:>30}BASKETBALL", "");
    println!("{:>15}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", "");
    println!("\n\n");

    println!("THIS IS DARTMOUTH COLLEGE BASKETBALL.  YOU WILL BE DARTMOUTH");
    println!(" CAPTAIN AND PLAYMAKER.  CALL SHOTS AS FOLLOWS:  1. LONG");
    println!(" (30 FT.) JUMP SHOT; 2. SHORT (15 FT.) JUMP SHOT; 3. LAY");
    println!(" UP; 4. SET SHOT.");
    println!("BOTH TEAMS WILL USE THE SAME DEFENSE.  CALL DEFENSE AS");
    println!("FOLLOWS:  6. PRESS; 6.5 MAN-TO MAN; 7. ZONE; 7.5 NONE.");
    println!("TO CHANGE DEFENSE, JUST TYPE 0 AS YOUR NEXT SHOT.");

    let mut game = BasketballGame::new();
    game.play();
}

struct BasketballGame {
    rng: ThreadRng,
    // D is the defensive alignment. It is one of 6, 6.5, 7 or 7.5 (but the
    // original code only validates that D >= 6).
    defense: f64,
    // The opponent's name.
    opponent: String,
    // Game clock. Incremented on most possessions; 50 ends the first half,
    // 92 triggers the two-minute warning and 100 forces overtime or the
    // final whistle.
    time: i32,
    // Score: index 0 is the opponent, index 1 is Dartmouth.
    score: [i32; 2],
}

impl BasketballGame {
    fn new() -> Self {
        let rng = rand::rng();

        // Line 76: prompt for the starting defense. The original accepts any
        // value >= 6, so we replicate that rather than restricting to the
        // documented options.
        let defense = loop {
            print!("YOUR STARTING DEFENSE WILL BE");
            let d: f64 = read_input();
            if d >= 6.0 {
                break d;
            }
            // BASIC line 76 falls through to 2010 which re-prompts for the
            // defensive alignment; reproducing that behaviour here.
        };
        println!();

        print!("CHOOSE YOUR OPPONENT");
        let opponent = read_line().trim().to_string();

        Self {
            rng,
            defense,
            opponent,
            time: 0,
            score: [0, 0],
        }
    }

    fn play(&mut self) {
        // Line 370: center jump to open each half/overtime period.
        self.center_jump();
    }

    /// Line 370-420: simulates the center jump that starts each period.
    fn center_jump(&mut self) {
        println!("CENTER JUMP");
        if self.rng.random::<f64>() > 3.0 / 5.0 {
            println!("DARTMOUTH CONTROLS THE TAP.");
            println!();
            self.dartmouth_possession();
        } else {
            println!("{} CONTROLS THE TAP.", self.opponent);
            self.opponent_possession();
        }
    }

    /// Lines 425-500: Dartmouth has the ball and asks the player for a shot.
    fn dartmouth_possession(&mut self) {
        let z = self.read_shot();
        // Line 460: half the time, when the clock has run out, we go to the
        // end-of-game / overtime logic instead of resolving the shot.
        if self.rng.random::<f64>() < 0.5 && self.time >= 100 {
            self.end_of_game_or_overtime();
            return;
        }

        match z {
            1 | 2 => self.dartmouth_jump_shot(),
            _ => self.dartmouth_non_jump_shot(z),
        }
    }

    /// Line 430-455: read and validate the player's shot choice.
    fn read_shot(&mut self) -> i32 {
        loop {
            print!("YOUR SHOT");
            let line = read_line();
            match line.trim().parse::<i32>() {
                Ok(z) if (0..=4).contains(&z) => return z,
                _ => print!("INCORRECT ANSWER.  RETYPE IT. "),
            }
        }
    }

    /// Lines 1040-1280: long or short jump shot for Dartmouth.
    fn dartmouth_jump_shot(&mut self) {
        self.advance_clock_with_two_minute_warning();
        println!("JUMP SHOT");

        if self.rng.random::<f64>() <= 0.341 * self.defense / 8.0 {
            println!("SHOT IS GOOD.");
            self.add_dartmouth_score();
            self.opponent_possession();
            return;
        }
        if self.rng.random::<f64>() <= 0.682 * self.defense / 8.0 {
            println!("SHOT IS OFF TARGET.");
            if self.defense / 6.0 * self.rng.random::<f64>() > 0.45 {
                println!("REBOUND TO {}", self.opponent);
                self.opponent_possession();
            } else {
                println!("DARTMOUTH CONTROLS THE REBOUND.");
                if self.rng.random::<f64>() <= 0.4 {
                    println!();
                    self.dartmouth_non_jump_shot(3);
                } else if self.defense == 6.0 && self.rng.random::<f64>() > 0.6 {
                    // Line 5100-5130: pass stolen, easy lay-up for opponent.
                    println!("PASS STOLEN BY {} EASY LAYUP.", self.opponent);
                    self.add_opponent_score();
                    self.dartmouth_possession();
                } else {
                    print!("BALL PASSED BACK TO YOU. ");
                    self.dartmouth_possession();
                }
            }
            return;
        }
        if self.rng.random::<f64>() <= 0.782 * self.defense / 8.0 {
            print!("SHOT IS BLOCKED.  BALL CONTROLLED BY ");
            if self.rng.random::<f64>() > 0.5 {
                println!("DARTMOUTH.");
                self.dartmouth_possession();
            } else {
                println!("{}.", self.opponent);
                self.opponent_possession();
            }
            return;
        }
        if self.rng.random::<f64>() <= 0.843 * self.defense / 8.0 {
            println!("SHOOTER IS FOULED.  TWO SHOTS.");
            self.foul_shots(1);
            self.opponent_possession();
            return;
        }
        println!("CHARGING FOUL.  DARTMOUTH LOSES BALL.");
        self.opponent_possession();
    }

    /// Lines 1300-1700: lay up (3) or set shot (4); also re-entered for a
    /// rebound put-back. A shot value of 0 means the player wants to change
    /// the defensive alignment (lines 2010-2040).
    fn dartmouth_non_jump_shot(&mut self, z: i32) {
        self.advance_clock_with_two_minute_warning();

        if z == 0 {
            self.change_defense();
            return;
        }

        if z == 4 {
            println!("SET SHOT.");
        } else {
            println!("LAY UP.");
        }

        if 7.0 / self.defense * self.rng.random::<f64>() <= 0.4 {
            println!("SHOT IS GOOD.  TWO POINTS.");
            self.add_dartmouth_score();
            self.opponent_possession();
            return;
        }
        if 7.0 / self.defense * self.rng.random::<f64>() <= 0.7 {
            println!("SHOT IS OFF THE RIM.");
            if self.rng.random::<f64>() > 2.0 / 3.0 {
                println!("DARTMOUTH CONTROLS THE REBOUND.");
                if self.rng.random::<f64>() > 0.4 {
                    println!("BALL PASSED BACK TO YOU.");
                    self.dartmouth_possession();
                } else {
                    self.dartmouth_non_jump_shot(3);
                }
            } else {
                println!("{} CONTROLS THE REBOUND.", self.opponent);
                self.opponent_possession();
            }
            return;
        }
        if 7.0 / self.defense * self.rng.random::<f64>() <= 0.875 {
            println!("SHOOTER FOULED.  TWO SHOTS.");
            self.foul_shots(1);
            self.opponent_possession();
            return;
        }
        if 7.0 / self.defense * self.rng.random::<f64>() <= 0.925 {
            println!("SHOT BLOCKED.  {}'S BALL.", self.opponent);
            self.opponent_possession();
            return;
        }
        println!("CHARGING FOUL.  DARTMOUTH LOSES THE BALL.");
        self.opponent_possession();
    }

    /// Lines 2010-2040: change the defensive alignment. Any value >= 6 is
    /// accepted, matching the original (unvalidated) behaviour.
    fn change_defense(&mut self) {
        loop {
            print!("YOUR NEW DEFENSIVE ALLIGNMENT IS");
            let d: f64 = read_input();
            if d >= 6.0 {
                self.defense = d;
                break;
            }
        }
        self.dartmouth_possession();
    }

    /// Lines 3000-3810: opponent has the ball. Picks a shot type at random.
    fn opponent_possession(&mut self) {
        self.advance_clock();

        let z1 = 10.0 / 4.0 * self.rng.random::<f64>() + 1.0;
        if z1 > 2.0 {
            self.opponent_non_jump_shot(z1);
        } else {
            self.opponent_jump_shot();
        }
    }

    /// Lines 3040-3310: opponent jump shot.
    fn opponent_jump_shot(&mut self) {
        println!("JUMP SHOT.");
        if 8.0 / self.defense * self.rng.random::<f64>() <= 0.35 {
            println!("SHOT IS GOOD.");
            self.add_opponent_score();
            self.dartmouth_possession();
            return;
        }
        if 8.0 / self.defense * self.rng.random::<f64>() <= 0.75 {
            println!("SHOT IS OFF RIM.");
            if self.defense / 6.0 * self.rng.random::<f64>() > 0.5 {
                println!("{} CONTROLS THE REBOUND.", self.opponent);
                if self.defense == 6.0 && self.rng.random::<f64>() > 0.75 {
                    // Line 5000-5030: ball stolen, easy lay-up for Dartmouth.
                    println!("BALL STOLEN.  EASY LAY UP FOR DARTMOUTH.");
                    self.add_dartmouth_score();
                    self.opponent_possession();
                } else if self.rng.random::<f64>() > 0.5 {
                    println!("PASS BACK TO {} GUARD.", self.opponent);
                    self.opponent_possession();
                } else {
                    self.opponent_non_jump_shot(3.0);
                }
            } else {
                println!("DARTMOUTH CONTROLS THE REBOUND.");
                self.dartmouth_possession();
            }
            return;
        }
        if 8.0 / self.defense * self.rng.random::<f64>() <= 0.9 {
            println!("PLAYER FOULED.  TWO SHOTS.");
            self.foul_shots(0);
            self.dartmouth_possession();
            return;
        }
        println!("OFFENSIVE FOUL.  DARTMOUTH'S BALL.");
        self.dartmouth_possession();
    }

    /// Lines 3500-3810: opponent lay up or set shot. `z1` carries the
    /// randomly chosen shot type so the put-back message can be re-shown.
    fn opponent_non_jump_shot(&mut self, z1: f64) {
        if z1 > 3.0 {
            println!("SET SHOT.");
        } else {
            println!("LAY UP");
        }
        if 7.0 / self.defense * self.rng.random::<f64>() <= 0.413 {
            println!("SHOT IS GOOD.");
            self.add_opponent_score();
            self.dartmouth_possession();
            return;
        }
        println!("SHOT IS MISSED.");
        if self.defense / 6.0 * self.rng.random::<f64>() > 0.5 {
            println!("{} CONTROLS THE REBOUND.", self.opponent);
            if self.defense == 6.0 && self.rng.random::<f64>() > 0.75 {
                println!("BALL STOLEN.  EASY LAY UP FOR DARTMOUTH.");
                self.add_dartmouth_score();
                self.opponent_possession();
            } else if self.rng.random::<f64>() > 0.5 {
                self.opponent_non_jump_shot(z1);
            } else {
                println!("PASS BACK TO {} GUARD.", self.opponent);
                self.opponent_possession();
            }
        } else {
            println!("DARTMOUTH CONTROLS THE REBOUND.");
            self.dartmouth_possession();
        }
    }

    /// Lines 4000-4110: two foul shots. `team` is 0 for the opponent (points
    /// added to Dartmouth's opponent) or 1 for Dartmouth (points added to
    /// Dartmouth).
    fn foul_shots(&mut self, team: usize) {
        if self.rng.random::<f64>() <= 0.49 {
            println!("SHOOTER MAKES BOTH SHOTS.");
            self.score[1 - team] += 2;
            self.print_score();
        } else if self.rng.random::<f64>() <= 0.75 {
            println!("SHOOTER MAKES ONE SHOT AND MISSES ONE.");
            self.score[1 - team] += 1;
            self.print_score();
        } else {
            println!("BOTH SHOTS MISSED.");
            self.print_score();
        }
    }

    /// Line 6010: prints the running score.
    fn print_score(&self) {
        println!("SCORE: {} TO {}", self.score[1], self.score[0]);
    }

    /// Line 7000: Dartmouth scores two points, then prints the score.
    fn add_dartmouth_score(&mut self) {
        self.score[1] += 2;
        self.print_score();
    }

    /// Line 6000: opponent scores two points, then prints the score.
    fn add_opponent_score(&mut self) {
        self.score[0] += 2;
        self.print_score();
    }

    /// Lines 1041/1301/3008: advance the clock and handle the end-of-half
    /// break at T == 50.
    fn advance_clock(&mut self) {
        self.time += 1;
        if self.time == 50 {
            self.end_of_first_half();
        }
    }

    /// Lines 1042-1046 / 1302-1304 / 3012-3015: advance the clock, handle the
    /// end-of-half break at T == 50 and the two-minute warning at T == 92.
    fn advance_clock_with_two_minute_warning(&mut self) {
        self.time += 1;
        if self.time == 50 {
            self.end_of_first_half();
        }
        if self.time == 92 {
            self.two_minute_warning();
        }
    }

    /// Lines 610-620: two-minute warning message.
    fn two_minute_warning(&self) {
        println!();
        println!("   *** TWO MINUTES LEFT IN THE GAME ***");
        println!();
    }

    /// Lines 8000-8020: end of the first half, then re-jump to start the
    /// second half.
    fn end_of_first_half(&mut self) {
        println!();
        println!("   ***** END OF FIRST HALF *****");
        println!();
        println!(
            "SCORE: DARTMOUTH:{}   {}:{}",
            self.score[1], self.opponent, self.score[0]
        );
        println!();
        println!();
        self.center_jump();
    }

    /// Lines 490-520: the clock has expired. Either start overtime (if the
    /// game is tied) or end the game.
    fn end_of_game_or_overtime(&mut self) {
        println!();
        if self.score[1] != self.score[0] {
            println!("   ***** END OF GAME *****");
            println!(
                "FINAL SCORE: DARTMOUTH:{}   {}:{}",
                self.score[1], self.opponent, self.score[0]
            );
            return;
        }

        println!();
        println!("   ***** END OF SECOND HALF *****");
        println!();
        println!("SCORE AT END OF REGULATION TIME:");
        println!(
            "        DARTMOUTH:{}   {}:{}",
            self.score[1], self.opponent, self.score[0]
        );
        println!();
        println!("BEGIN TWO MINUTE OVERTIME PERIOD");
        self.time = 93;
        self.center_jump();
    }
}

/// Reads a line of input, flushing stdout first so the prompt is visible.
fn read_line() -> String {
    print!("? ");
    let _ = io::stdout().flush();

    let mut buffer = String::new();
    let _ = io::stdin().read_line(&mut buffer);
    buffer
}

/// Reads a line and parses it as the requested numeric type, re-prompting on
/// any parse failure.
fn read_input<T: std::str::FromStr>() -> T {
    loop {
        let line = read_line();
        if let Ok(value) = line.trim().parse::<T>() {
            return value;
        }
    }
}
