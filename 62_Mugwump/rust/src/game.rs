use rand::Rng;

use crate::{
    coordinate::{CoordState, Coordinate},
    draw::draw_board,
    util,
};

pub struct Game {
    pub coords: Vec<Coordinate>,
    tries: u8,
    show_board: bool,
}

impl Game {
    pub fn new(show_board: bool) -> Self {
        let mut coords = Vec::new();
        let mut random_indexes = Vec::new();
        let get_random_index = || -> i32 { rand::thread_rng().gen_range(0..100) };

        for _ in 0..4 {
            let mut i = get_random_index();
            while random_indexes.contains(&i) {
                i = get_random_index();
            }
            random_indexes.push(i);
        }

        let mut x = 0;
        let mut y: i8 = 9;

        let mut mugwump_number = 0;

        for i in 0..100 {
            let mut has_mugwump = false;

            if random_indexes.contains(&i) {
                has_mugwump = true;
                mugwump_number += 1;
            }

            coords.push(Coordinate::new((x, y as u8), has_mugwump, mugwump_number));

            x += 1;

            if ((i + 1) % 10) == 0 {
                x = 0;
                y -= 1;
            }
        }

        Game {
            coords,
            tries: 0,
            show_board,
        }
    }

    pub fn tick(&mut self) -> Option<bool> {
        let mut game_over = false;

        if self.tries >= 10 {
            println!("SORRY THAT'S 10 TRIES. HERE IS WHERE THEY ARE HIDING");

            for m in self.get_mugwumps() {
                println!("MUGWUMP {} IS AT {:?}", m.mugwump_number, m.get_pos());
            }

            if self.show_board {
                draw_board(&self.coords, true);
            }

            game_over = true;
        }

        if self.get_mugwumps().len() == 0 {
            println!("YOU HAVE FOUND ALL MUGWUMPS!");

            game_over = true;
        }

        if game_over {
            return util::prompt_bool("THAT WAS FUN! PLAY AGAIN (Y/n)?");
        }

        self.tries += 1;

        if self.show_board {
            draw_board(&self.coords, true);
        }

        let entered_position = self.input_coordinate();
        self.check_position(entered_position);

        None
    }

    fn check_position(&mut self, pos: (u8, u8)) {
        if let Some(coord) = self.coords.iter_mut().find(|c| c.get_pos() == pos) {
            use CoordState::*;

            match coord.state {
                Normal => {
                    coord.state = Checked;
                    self.print_distances(pos);
                }
                HasMugwump => {
                    coord.state = FoundMugwump;
                    println!("YOU FOUND MUGWUMP {}", coord.mugwump_number);
                    self.print_distances(pos);
                }
                Checked | FoundMugwump => println!("YOU ALREADY LOOKED HERE!"),
            }
        }
    }

    fn print_distances(&self, (x, y): (u8, u8)) {
        let print = |m: &Coordinate| {
            let (mx, my) = m.get_pos();
            let (x, y, mx, my) = (x as i32, y as i32, mx as i32, my as i32);
            let distance = (((x - mx).pow(2) + (y - my).pow(2)) as f32).sqrt();

            println!(
                "YOU ARE {} UNITS FROM MUGWUMP {}",
                distance, m.mugwump_number
            );
        };

        for m in self.get_mugwumps() {
            print(m);
        }
    }

    fn input_coordinate(&self) -> (u8, u8) {
        let msg = format!("TURN NO. {} WHAT IS YOUR GUESS?", self.tries);
        let input = util::prompt(msg.as_str());

        if !input.contains(",") {
            println!("YOU MUST ENTER A COORDINATE: #,#");
            return (0, 0);
        }

        let axes: Vec<&str> = input.split(",").collect();
        let mut pos = [0; 2];

        for (i, a) in axes.iter().enumerate() {
            match a.parse::<usize>() {
                Ok(p) => pos[i] = p as u8,
                Err(_) => println!("YOU MUST ENTER A COORDINATE: #,#"),
            }
        }

        (pos[0], pos[1])
    }

    fn get_mugwumps(&self) -> Vec<&Coordinate> {
        self.coords
            .iter()
            .filter(|c| c.state == CoordState::HasMugwump)
            .collect()
    }
}
