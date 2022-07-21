use std::{collections::HashMap, time::Duration};

use crate::utility;

pub struct Game {
    gates: u8,
    rating: u8,
    time: f32,
    speed: u8,
    medals: HashMap<String, u8>,
}

impl Game {
    pub fn new() -> Self {
        let gates =
            utility::prompt_int_range(1..=25, "HOW MANY GATES DOES THIS COURSE HAVE (1 TO 25)?");

        utility::prompt_command(gates);

        let mut medals = HashMap::new();
        medals.insert("GOLD".to_string(), 0);
        medals.insert("SILVER".to_string(), 0);
        medals.insert("BRONZE".to_string(), 0);

        Self {
            gates,
            rating: utility::prompt_int_range(1..=3, "RATE YOURSELF AS SKIER, (1-WORST, 3-BEST)?"),
            time: 0.0,
            speed: utility::get_random_number(18, 9),
            medals,
        }
    }

    pub fn play(&mut self) {
        Game::start_play();

        let mut completed = true;

        'main: for g in 1..=self.gates {
            println!("\nHERE COMES GATE # {}", g);
            println!("  {} M.P.H.", self.speed);

            let previous_speed = self.speed;

            loop {
                if self.process_option(utility::prompt_int_range(0..=8, "OPTION?"), g) {
                    if self.speed < 7 {
                        self.speed = previous_speed;
                        println!("LET'S BE REALISTIC, OK? LET'S GO BACK AND TRY IT AGAIN...");
                    } else {
                        let i = (g - 1) as usize;

                        self.time += (utility::MAX_SPEEDS[i] - self.speed) as f32 + 1.0;

                        if self.speed > utility::MAX_SPEEDS[i] {
                            self.time += 0.5;
                        }

                        break;
                    }
                } else {
                    completed = false;
                    break 'main;
                }
            }

            println!("YOU TOOK {} SECONDS.", self.time + utility::random_float());
        }

        if completed {
            let avg = self.time / self.gates as f32;
            let rating = self.rating as f32;
            if avg < 1.5 - (rating * 0.1) {
                println!("YOU WON A GOLD MEDAL!");
                *self.medals.get_mut("GOLD").unwrap() += 1;
            } else if avg < 2.9 - (rating * 0.1) {
                println!("YOU WON A SILVER MEDAL!");
                *self.medals.get_mut("SILVER").unwrap() += 1;
            } else if avg < 4.4 - (rating * 0.01) {
                println!("YOU WON A BRONZE MEDAL!");
                *self.medals.get_mut("BRONZE").unwrap() += 1;
            }
        }

        if utility::prompt_again() {
            self.time = 0.0;
            self.speed = utility::get_random_number(18, 9);
            self.play();
        } else {
            println!("THANKS FOR THE RACE.");

            for medal in self.medals.keys() {
                println!("{} MEDALS: {}", medal, *self.medals.get(medal).unwrap())
            }
        }
    }

    fn start_play() {
        println!("THE STARTER COUNTS DOWN...5");
        for i in 0..4 {
            std::thread::sleep(Duration::from_secs_f32(0.5));
            println!("...{}", 4 - i);
        }
        print!("...GO!");
        println!("YOU'RE OFF!");
    }

    fn process_option(&mut self, option: u8, gate: u8) -> bool {
        let mut good = true;

        match option {
            0 => println!("YOU'VE TAKEN {} SECONDS.", self.time),
            1 => {
                self.speed += utility::get_random_number(10, 5);
            }
            2 => {
                self.speed += utility::get_random_number(5, 3);
            }
            3 => {
                self.speed += utility::get_random_number(4, 1);
            }
            4 => (),
            5 => {
                self.speed -= utility::get_random_number(4, 1);
            }
            6 => {
                self.speed -= utility::get_random_number(5, 3);
            }
            7 => {
                self.speed -= utility::get_random_number(10, 5);
            }
            8 => {
                println!("***CHEAT");
                if utility::random_float() < 0.7 {
                    println!("AN OFFICIAL CAUGHT YOU!");
                    println!("YOU TOOK {} SECONDS.", self.time + utility::random_float());
                    good = false;
                } else {
                    println!("YOU MADE IT!");
                    self.time += 1.5;
                }
            }
            _ => println!("WHAT?"),
        }

        println!("  {} M.P.H.", self.speed);

        let i = (gate - 1) as usize;

        if self.speed > utility::MAX_SPEEDS[i] {
            if utility::random_float()
                < (((self.speed - utility::MAX_SPEEDS[i]) as f32 * 0.1) + 0.2)
            {
                println!(
                    "YOU WENT OVER THE MAXIMUM SPEED AND {}!",
                    if rand::random() {
                        "SNAGGED A FLAG"
                    } else {
                        "WIPED OUT"
                    }
                );
                println!("YOU TOOK {} SECONDS.", self.time + utility::random_float());
                good = false;
            } else {
                println!("YOU WENT OVER THE MAXIMUM SPEED AND MADE IT!");
            }
        } else if self.speed > (utility::MAX_SPEEDS[i] - 1) {
            println!("CLOSE ONE!");
        }

        good
    }
}
