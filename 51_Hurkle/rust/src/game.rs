use std::io;

use rand::Rng;

type Position = (u8, u8);

pub struct Game {
    hurkle: Position,
    tries: u8,
}

impl Game {
    pub fn new() -> Self {
        let x: u8 = rand::thread_rng().gen_range(1..=10);
        let y: u8 = rand::thread_rng().gen_range(1..=10);
        let hurkle = (x, y);

        Game { hurkle, tries: 0 }
    }

    pub fn update(&mut self) -> bool {
        if self.tries >= 5 {
            println!("SORRY, THAT'S {} GUESSES.", self.tries);
            println!("THE HURKLE IS AT {}, {}", self.hurkle.0, self.hurkle.1);
            return true;
        }
        self.tries += 1;
        self.process_guess(self.get_guess())
    }

    fn get_guess(&self) -> Position {
        let mut pos = (0, 0);

        'guess: loop {
            println!("GUESS # {}?", self.tries);

            let mut input = String::new();

            io::stdin()
                .read_line(&mut input)
                .expect("**Failed to read line**");

            let input: Vec<&str> = input.trim().split(",").collect();

            let mut is_y = false;
            for a in input {
                match a.parse::<u8>() {
                    Ok(a) => {
                        if a > 10 || a == 0 {
                            println!("GUESS AXIS CANNOT BE ZERO OR LARGER THAN TEN!");
                            break;
                        }
                        if is_y {
                            pos.1 = a;
                            break 'guess;
                        } else {
                            pos.0 = a;
                            is_y = true;
                        }
                    }
                    Err(e) => println!("{} - TRY AGAIN!", e.to_string().to_uppercase()),
                }
            }
        }

        pos
    }

    fn process_guess(&self, p: Position) -> bool {
        if p == self.hurkle {
            println!("\nYOU FOUND HIM IN {} GUESSES!", self.tries);
            return true;
        }

        let (x, y) = (p.0, p.1);
        let (hx, hy) = (self.hurkle.0, self.hurkle.1);

        let mut dir_x = "WEST";
        let mut dir_y = "SOUTH";

        let mut set_y_dir = || {
            if y < hy {
                dir_y = "NORTH";
            } else {
                dir_y = "";
            }
        };

        if x > hx {
            set_y_dir();
        } else if x < hx {
            dir_x = "EAST";
            set_y_dir();
        } else {
            dir_x = "";
            set_y_dir();
        }

        println!("GO {}{}\n", dir_y, dir_x);

        false
    }
}
