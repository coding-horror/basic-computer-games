use crate::{
    disk::Disk,
    needle::Needle,
    util::{self, prompt, PromptResult},
};

pub struct Game {
    pub needles: Vec<Needle>,
    disk_count: u8,
    moves: usize,
}

impl Game {
    pub fn new() -> Self {
        let mut needles = Vec::new();
        let disk_count = util::get_disk_count();

        for i in 1..=3 {
            let disks = match i {
                1 => {
                    let mut disks = Vec::new();

                    let mut half_size = 7;
                    for _ in (1..=disk_count).rev() {
                        disks.push(Disk::new(half_size * 2 + 1));
                        half_size -= 1;
                    }

                    disks
                }
                2 | 3 => Vec::new(),
                _ => panic!("THERE MUST BE EXACTLY THREE NEEDLES!"),
            };

            needles.push(Needle { disks, number: i });
        }

        Game {
            needles,
            disk_count,
            moves: 0,
        }
    }

    pub fn update(&mut self) -> bool {
        self.draw();

        loop {
            let (disk_index, from_needle_index) = self.get_disk_to_move();
            let to_needle_index = self.ask_which_needle();

            if from_needle_index == to_needle_index {
                println!("DISK IS ALREADY AT THAT NEEDLE!");
                break;
            }

            let to_needle = &self.needles[to_needle_index];

            if to_needle.disks.len() == 0
                || to_needle.disks[0].size > self.needles[from_needle_index].disks[disk_index].size
            {
                self.move_disk(disk_index, from_needle_index, to_needle_index);
                break;
            } else {
                println!("CAN'T PLACE ON A SMALLER DISK!");
            }
        }

        if self.needles[2].disks.len() == self.disk_count as usize {
            self.draw();
            println!("CONGRATULATIONS!!");
            println!("YOU HAVE PERFORMED THE TASK IN {} MOVES.", self.moves);
            return true;
        }

        false
    }

    pub fn draw(&self) {
        println!("");
        for r in (1..=7).rev() {
            for n in &self.needles {
                n.draw(r)
            }
            println!("");
        }
        println!("");
    }

    fn get_disk_to_move(&self) -> (usize, usize) {
        loop {
            if let PromptResult::Number(n) = prompt(true, "WHICH DISK WOULD YOU LIKE TO MOVE?") {
                let smallest_disk = 15 - ((self.disk_count - 1) * 2);

                if n < smallest_disk as i32 || n > 15 || (n % 2) == 0 {
                    println!("PLEASE ENTER A VALID DISK!")
                } else {
                    for (n_i, needle) in self.needles.iter().enumerate() {
                        if let Some((i, _)) = needle
                            .disks
                            .iter()
                            .enumerate()
                            .find(|(_, disk)| disk.size == n as u8)
                        {
                            if i == (needle.disks.len() - 1) {
                                return (i, n_i);
                            }

                            println!("THAT DISK IS BELOW ANOTHER ONE. MAKE ANOTHER CHOICE.");
                        }
                    }
                }
            }
        }
    }

    fn ask_which_needle(&self) -> usize {
        loop {
            if let PromptResult::Number(n) = prompt(true, "PLACE DISK ON WHICH NEEDLE?") {
                if n <= 0 || n > 3 {
                    println!("PLEASE ENTER A VALID NEEDLE.");
                } else {
                    return (n - 1) as usize;
                }
            }
        }
    }

    fn move_disk(&mut self, disk: usize, from: usize, to: usize) {
        let from = &mut self.needles[from];
        let size = from.disks[disk].size;

        from.disks.remove(disk);
        self.needles[to].add(size);

        self.moves += 1;
    }
}
