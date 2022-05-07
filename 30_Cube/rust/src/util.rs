use std::num::ParseIntError;

use crate::game::Position;

pub fn get_random_position() -> Position {
    (get_random_axis(), get_random_axis(), get_random_axis())
}

fn get_random_axis() -> u8 {
    rand::Rng::gen_range(&mut rand::thread_rng(), 1..=3)
}

pub fn get_landmines() -> Vec<Position> {
    let mut landmines = Vec::new();

    for _ in 0..5 {
        let mut m = get_random_position();
        while landmines.contains(&m) {
            m = get_random_position();
        }
        landmines.push(m);
    }

    landmines
}

fn read_line() -> Result<usize, ParseIntError> {
    let mut input = String::new();
    std::io::stdin()
        .read_line(&mut input)
        .expect("~~Failed reading line!~~");
    input.trim().parse::<usize>()
}

pub fn prompt_bool(msg: &str) -> bool {
    loop {
        println!("{}", msg);

        if let Ok(n) = read_line() {
            if n == 1 {
                return true;
            } else if n == 0 {
                return false;
            }
        }
        println!("ENTER YES--1 OR NO--0\n");
    }
}

pub fn prompt_number(msg: &str) -> usize {
    loop {
        println!("{}", msg);

        if let Ok(n) = read_line() {
            return n;
        }
        println!("ENTER A NUMBER\n");
    }
}

pub fn prompt_position(msg: &str, prev_pos: Position) -> Option<Position> {
    loop {
        println!("{}", msg);

        let mut input = String::new();
        std::io::stdin()
            .read_line(&mut input)
            .expect("~~Failed reading line!~~");

        let input: Vec<&str> = input.trim().split(",").collect();

        let pp = [prev_pos.0, prev_pos.1, prev_pos.2];
        let mut pos = Vec::new();

        if input.len() != 3 {
            println!("YOU MUST ENTER 3 AXES!");
        } else {
            for a in input {
                if let Ok(n) = a.parse::<u8>() {
                    if n == 0 || n > 3 {
                        println!("YOU MUST ENTER AN AXIS BETWEEN 1 AND 3!");
                    } else {
                        pos.push(n);
                    }
                } else {
                    println!("INVALID LOCATION.");
                }
            }

            let mut moved = false;
            for (i, p) in pos.iter().enumerate() {
                let dt = ((*p as isize) - (pp[i] as isize)).abs();

                if dt > 1 {
                    return None;
                }

                if dt == 1 {
                    if moved {
                        return None;
                    } else {
                        moved = true;
                    }
                }
            }
        }

        if pos.len() == 3 {
            let pos = (pos[0], pos[1], pos[2]);
            if pos == prev_pos {
                println!("YOU ARE ALREADY THERE!");
            } else {
                return Some(pos);
            }
        }
    }
}
