#![allow(dead_code)]

use rand::Rng;

struct Game {
    coords: Vec<Coordinate>,
    tries: u8,
    pub state: GameState,
}

impl Game {
    fn new() -> Self {
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
            println!("current pos: {:?}", (x, y));

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

    fn draw_board(&self) {
        let draw_top_bottom = |is_top: bool| {
            let (mut left, mut right) = ("╔", "╗");

            if !is_top {
                (left, right) = ("╚", "╝");
            }

            for i in 0..11 {
                if i == 0 {
                    print!("{}══", left);
                } else if i == 10 {
                    print!("═══{}", right)
                } else {
                    print!("══");
                }
            }
            println!("");
        };

        println!("coords length: {}", self.coords.len());

        draw_top_bottom(true);

        // Draw points
        let mut y: i8 = 9;

        print!("║ {} ", y);

        for (i, c) in self.coords.iter().enumerate() {
            let mut char = '-';

            match c.state {
                CoordState::Normal => (),
                CoordState::HasMugwump => char = '𑗌',
                CoordState::Checked => char = '*',
            }

            print!("{} ", char);

            if (i % 10) == 0 {
                print!("║");
                println!("");
                print!("║ {} ", y);
                y -= 1;
            }
        }

        print!("║ 𑗌 ");
        for i in 0..10 {
            print!("{} ", i);

            if i == 9 {
                print!("║");
            }
        }
        println!("");

        draw_top_bottom(false);
    }
}

enum GameState {
    Playing,
    Win,
    Lose,
}

#[derive(Debug)]
struct Coordinate {
    x: usize,
    y: usize,
    state: CoordState,
}

impl Coordinate {
    fn new(pos: (usize, usize), has_mugwump: bool) -> Self {
        let state = if has_mugwump {
            CoordState::HasMugwump
        } else {
            CoordState::Normal
        };

        Coordinate {
            x: pos.0,
            y: pos.1,
            state,
        }
    }
}

#[derive(Debug, PartialEq)]
enum CoordState {
    Normal,
    HasMugwump,
    Checked,
}

fn main() {
    println!("\n\nMUGWUMP");
    println!("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");

    println!("THE OBJECT OF THIS GAME IS TO FIND FOUR MUGWUMPS");
    println!("HIDDEN ON A 10 BY 10 GRID.  HOMEBASE IS POSITION 0,0.");
    println!("ANY GUESS YOU MAKE MUST BE TWO NUMBERS WITH EACH");
    println!("NUMBER BETWEEN 0 AND 9, INCLUSIVE.  FIRST NUMBER");
    println!("IS DISTANCE TO RIGHT OF HOMEBASE AND SECOND NUMBER");
    println!("IS DISTANCE ABOVE HOMEBASE!");
    println!("YOU GET 10 TRIES.  AFTER EACH TRY, I WILL TELL");
    println!("YOU HOW FAR YOU ARE FROM EACH MUGWUMP.\n");

    let game = Game::new();
    game.draw_board();
}
