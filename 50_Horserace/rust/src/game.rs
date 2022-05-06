use std::{thread, time::Duration};

use crate::{horses::Horses, players::Players};

pub struct Game {
    horses: Horses,
    players: Players,
}

impl Game {
    pub fn new() -> Self {
        Game {
            horses: Horses::new(),
            players: Players::new(),
        }
    }

    pub fn play(&mut self) -> bool {
        self.horses.randomize_odds();
        self.horses.print_table();

        self.players.make_bets();

        println!("\n1 2 3 4 5 6 7 8");

        for _ in 1..=7 {
            self.horses.advance();
            self.draw();
            thread::sleep(Duration::from_secs(1));
        }

        let winner = self.horses.print_placements();
        self.players.process_winner(winner);

        return self.players.prompt_next_round();
    }

    pub fn draw(&self) {
        println!("=============");
        println!("XXXXSTARTXXXX");
        for row in 1..=28 {
            let neighbors = self.horses.get_at(row);

            match neighbors.len() {
                0 => println!(),
                1 => println!("{}", neighbors[0].no),
                _ => {
                    for h in neighbors {
                        print!("{} ", h.no);
                    }
                    println!();
                }
            }
        }
        println!("XXXXFINISHXXXX");
    }
}
