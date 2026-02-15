use rand::{RngExt, rngs::ThreadRng};
use std::{
    convert::Infallible,
    io::{self, BufRead, Write},
    str::FromStr,
};

fn main() {
    println!("{:>30}GUNNER", "");
    println!("{:>15}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", "");
    println!("\n\n");

    println!("You are the officer-in-charge, giving orders to a gun");
    println!("crew, telling them the degrees of elevation you estimate");
    println!("will place a projectile on target.  A hit wihtin 100 yards");
    println!("of the target will destroy it.\n");

    let mut game = GunnerGame::new();
    while game.run() == PlayAgain::Yes {
        // do nothing -- just loop around and call game.run() again
    }
}

struct GunnerGame {
    gun_range: usize, // each game has different range for the gun. "R" in the basic code
    targets_destroyed: isize, // the "Z" counter in basic
    total_rounds: isize, // how many rounds were fired across all targets. "S1" in basic
    rng: ThreadRng,   // our PRNG
}

impl GunnerGame {
    fn new() -> Self {
        let mut rng = rand::rng();
        let gun_range = rng.random_range(20_000..=60_000);

        Self {
            gun_range,
            rng,
            targets_destroyed: 0,
            total_rounds: 0,
        }
    }

    fn run(&mut self) -> PlayAgain {
        println!("Maximum range of your gun is {} yards.\n", self.gun_range);

        for targets in 0..4 {
            if targets > 0 {
                // After the first round, we want to let the user know that there's
                // another target in sight
                println!("\nThe forward observer has sighted more enemy activity...");
            }

            // let the player try to destroy it
            let target_destroyed = self.handle_target();

            if !target_destroyed {
                println!("\nBoom !!!!   You have just been destroyed by the enemy.\n\n\n");
                println!("Better go back to Fort Sill for refresher training!");

                println!();
                break;
            } else {
                self.targets_destroyed += 1;
            }
        }

        // only display success message if all targets were destroyed
        if self.targets_destroyed == 4 {
            println!("\n\nTotal rounds expended were: {}", self.total_rounds);
            if self.total_rounds <= 18 {
                println!("Nice shooting !!");
            }
        }

        println!("\n");
        get_input("Try again (Y or N)")
    }

    fn handle_target(&mut self) -> bool {
        let target_distance =
            ((self.gun_range as f32) * (0.1 + 0.8 * self.rng.random::<f32>())) as isize;

        println!("Distance to the target is {target_distance} yards.\n\n");

        // The player gets 5 shots. On the 6th shot we still ask for an elevation,
        // but then report that they've been destroyed.
        for shots_fired in 1..=6 {
            let elevation = loop {
                let elevation: f32 = get_input("Elevation");

                // Let the player know how well they did.
                if elevation < 1.0 {
                    println!("Miniumum elevation is one degree.");
                } else if elevation > 89.0 {
                    println!("Maximum elevation is 89 degrees.");
                } else {
                    break elevation;
                }
            };

            // Only allow the player to destroy the target with the first 5 shots. The
            // sixth is ignored.
            if shots_fired < 6 {
                let angle = 2.0 * elevation / 57.3;
                let intersection = self.gun_range as f32 * angle.sin();
                let delta = target_distance - (intersection as isize);

                if delta.abs() < 100 {
                    println!(
                        "*** Target Destroyed ***  {shots_fired} rounds of ammunition expended."
                    );
                    self.total_rounds += shots_fired;
                    return true;
                } else if delta > 100 {
                    println!("Short of target by {delta} yards.");
                } else {
                    println!("Over target by {} yards.", delta.abs());
                }
            }
        }

        false
    }
}

#[derive(PartialEq, Eq)]
enum PlayAgain {
    Yes,
    No,
}

impl FromStr for PlayAgain {
    type Err = Infallible;

    fn from_str(line: &str) -> Result<Self, Self::Err> {
        if line.to_uppercase() == "Y" {
            Ok(Self::Yes)
        } else {
            Ok(Self::No)
        }
    }
}

fn get_input<R: FromStr, S: AsRef<str>>(prompt: S) -> R {
    loop {
        print!("{}? ", prompt.as_ref());
        let mut stdout = io::stdout().lock();
        let _ = stdout.flush();

        let mut buffer = String::new();
        let stdin = std::io::stdin();
        let mut handle = stdin.lock();
        let _ = handle.read_line(&mut buffer);

        if let Ok(result) = buffer.trim().to_string().parse::<R>() {
            return result;
        }
        println!("?Re-enter");
    }
}
