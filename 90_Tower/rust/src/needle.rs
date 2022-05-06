use crate::disk::Disk;

pub struct Needle {
    pub disks: Vec<Disk>,
    pub number: u8,
}

impl Needle {
    pub fn draw(&self, row: u8) {
        let row = row as usize;

        if self.disks.len() >= row {
            self.disks[row - 1].draw();
        } else {
            let offset = "       ";

            print!("{offset}");
            print!("*");
            print!("{offset}   ");
        }
    }

    pub fn add(&mut self, size: u8) {
        self.disks.push(Disk { size });
    }
}
