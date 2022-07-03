use std::{thread, time::Duration};

use rand::Rng;

// AI "learning" is not implemented. Don't have the time. - Ugur

fn main() {
    loop {
        let mut game = Game::default();

        loop {
            game.draw();
            if game.play_turn(false) {
                break;
            }
        }
    }
}

enum DistributeResult {
    Normal,
    // Leftover beans
    EndOnHomePit(bool),
    // "true" if ended on Player Home Pit
    EndOnEmptyPit(usize),
    // "index" of the empty pit within the Row
    ChosenEmpty,
}

struct Game {
    pits: [u8; 14],
    player_turn: bool,
}

impl Default for Game {
    fn default() -> Self {
        println!("\n\n\t\t AWARI");
        println!("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY");

        Self {
            pits: [3, 3, 3, 3, 3, 3, 0, 3, 3, 3, 3, 3, 3, 0],
            player_turn: true,
        }
    }
}

impl Game {
    fn step_through(&mut self, mut index: usize) -> usize {
        let mut bean_amount = self.pits[index];
        self.pits[index] = 0;

        loop {
            index += 1;

            if index > self.pits.len() - 1 {
                index = 0;
            }

            self.pits[index] += 1;

            bean_amount -= 1;
            if bean_amount == 0 {
                return index;
            }
        }
    }

    fn play_turn(&mut self, is_repeat: bool) -> bool {
        use DistributeResult::*;

        if self.is_game_over() {
            println!("\nGame Over!");
            let (player_beans, ai_beans) = (self.pits[6], self.pits[13]);
            if player_beans == ai_beans {
                println!("It's a draw")
            } else if player_beans > ai_beans {
                println!("You win by {}", player_beans - ai_beans);
            } else {
                println!("I win by {}", ai_beans - player_beans);
            }
            return true;
        }

        let chosen_index = if self.player_turn {
            player_prompt(if is_repeat { "Again?" } else { "Your move?" }) - 1
        } else {
            println!("========================");

            thread::sleep(Duration::from_secs(1));

            let non_empty_pits: Vec<usize> = self
                .pits
                .iter()
                .enumerate()
                .filter(|&(i, p)| (7..13).contains(&i) && *p > 0)
                .map(|(i, _)| i)
                .collect();
            let random_index = rand::thread_rng().gen_range(0..non_empty_pits.len());
            let ai_move = non_empty_pits[random_index];

            println!("My move is {}", ai_move - 6);

            println!("========================");
            ai_move
        };

        match self.process_choice(chosen_index) {
            Normal => (),
            EndOnHomePit(player) => {
                self.draw();

                if player == self.player_turn && !is_repeat {
                    self.play_turn(true);
                }
            }
            EndOnEmptyPit(last_index) => {
                let opposite_index = 12 - last_index;
                let home_index = if self.player_turn { 6 } else { 13 };
                let won_beans = 1 + self.pits[opposite_index];

                self.pits[last_index] = 0;
                self.pits[opposite_index] = 0;
                self.pits[home_index] += won_beans;
            }
            ChosenEmpty => {
                println!("Chosen pit is empty");
                return self.play_turn(is_repeat);
            }
        }

        if !is_repeat {
            self.player_turn = !self.player_turn;
        }

        false
    }

    pub fn process_choice(&mut self, index: usize) -> DistributeResult {
        use DistributeResult::*;

        if self.pits[index] == 0 {
            return ChosenEmpty;
        }

        let last_index = self.step_through(index);

        if last_index == 6 && self.player_turn {
            return EndOnHomePit(true);
        } else if last_index == 13 && !self.player_turn {
            return EndOnHomePit(false);
        } else if self.pits[last_index] == 1 {
            return EndOnEmptyPit(last_index);
        }

        Normal
    }

    fn is_game_over(&self) -> bool {
        let player_empty = !(0..6).any(|i| self.pits[i] > 0);
        let ai_empty = !(7..13).any(|i| self.pits[i] > 0);
        player_empty || ai_empty
    }

    fn draw(&self) {
        let row_as_string = |player: bool| -> String {
            let mut row_as_string = String::new();

            let range = if player { 0..6 } else { 7..13 };

            range.for_each(|i| {
                let mut bean_amount_as_string = self.pits[i].to_string();
                bean_amount_as_string.push_str("  ");

                if player {
                    row_as_string.push_str(&bean_amount_as_string);
                } else {
                    row_as_string.insert_str(0, &bean_amount_as_string);
                }
            });

            row_as_string
        };

        println!(
            "\n  {}\n{}                  {}\n  {}\n",
            row_as_string(false),
            self.pits[13].to_string(),
            self.pits[6].to_string(),
            row_as_string(true)
        );
    }
}

pub fn player_prompt(message: &str) -> usize {
    loop {
        let mut input = String::new();
        println!("{}", message);

        if let Ok(_) = std::io::stdin().read_line(&mut input) {
            match input.trim().parse::<usize>() {
                Ok(n) => {
                    if (1..=6).contains(&n) {
                        return n;
                    } else {
                        println!("Enter a number between 1 and 6")
                    }
                }
                Err(e) => {
                    println!("{}", e);
                }
            }
        }
    }
}
