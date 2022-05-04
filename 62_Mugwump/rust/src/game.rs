use rand::Rng;

use crate::coordinate::Coordinate;

pub struct Game {
    pub coords: Vec<Coordinate>,
    tries: u8,
    pub state: GameState,
}

impl Game {
    pub fn new() -> Self {
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

        for i in 0..100 {
            let mut has_mugwump = false;
            if random_indexes.contains(&i) {
                has_mugwump = true;
            }

            coords.push(Coordinate::new((x, y as usize), has_mugwump));

            x += 1;

            if ((i + 1) % 10) == 0 {
                x = 0;
                y -= 1;
            }
        }

        Game {
            coords,
            tries: 0,
            state: GameState::Playing,
        }
    }
}

pub enum GameState {
    Playing,
    Win,
    Lose,
}
