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
    Normal,               // Leftover beans
    EndOnHomePit(bool),   // "true" if ended on Player Home Pit
    EndOnEmptyPit(usize), // "index" of the empty pit within the Row
    ChosenEmpty,
    GameOver,
}

struct Game {
    pits: [u8; 14],
    player_turn: bool,
}

impl Default for Game {
    fn default() -> Self {
        println!("\t\t\t AWARI");
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

        loop {
            self.pits[index] += 1;

            bean_amount -= 1;
            if bean_amount == 0 {
                return index;
            }

            index += 1;
            if index > self.pits.len() - 1 {
                index = 0;
            }
        }
    }

    fn play_turn(&mut self, is_repeat: bool) -> bool {
        use DistributeResult::*;

        let chosen_index = if self.player_turn {
            player_prompt(if is_repeat { "Again?" } else { "Your move?" })
        } else {
            println!("My move is ");
            0 // ai choice
        };

        match self.process_choice(chosen_index) {
            Normal => (),
            EndOnHomePit(player) => {
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
            GameOver => {
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
        }
        self.player_turn = !self.player_turn;
        false
    }

    pub fn process_choice(&mut self, index: usize) -> DistributeResult {
        use DistributeResult::*;

        if self.pits[index + 1] == 0 {
            return ChosenEmpty;
        }

        let last_index = self.step_through(index + 1);

        if self.is_gameover() {
            return GameOver;
        } else if last_index == 6 && self.player_turn {
            return EndOnHomePit(true);
        } else if last_index == 13 && !self.player_turn {
            return EndOnHomePit(false);
        } else if self.pits[last_index] == 1 {
            return EndOnEmptyPit(last_index);
        }

        Normal
    }

    fn is_gameover(&self) -> bool {
        for (i, p) in self.pits.iter().enumerate() {
            if i != 6 || i != 13 {
                if *p > 0 {
                    return false;
                }
            }
        }
        true
    }

    fn draw(&self) {
        let row_as_string = |player: bool| -> String {
            let mut row_as_string = String::new();
            let range = if player { 0..6 } else { 7..13 };
            range.for_each(|i| {
                let mut bean_amount_as_string = self.pits[i].to_string();
                bean_amount_as_string.push_str("  ");
                row_as_string.push_str(&bean_amount_as_string);
            });
            return row_as_string;
        };

        println!(
            "  {}\n{}\t\t   {}\n  {}",
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
