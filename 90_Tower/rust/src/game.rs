use crate::{needle::Needle, util};

pub struct Game {
    pub needles: Vec<Needle>,
    _moves: usize,
}

impl Game {
    pub fn new() -> Self {
        let mut needles = Vec::new();

        for i in 1..=3 {
            let disks = match i {
                1 => util::generate_disks(4),
                2 => util::generate_disks(3),
                _ => Vec::new(),
            };

            needles.push(Needle { disks, number:i });
        }

        Game { needles, _moves: 0 }
    }

    pub fn draw(&self) {
        for r in (1..=7).rev() {
            for n in &self.needles {
                n.draw(r)
            }
            println!("");
        }
    }
}
