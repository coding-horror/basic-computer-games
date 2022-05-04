use crate::disk::Disk;

pub struct Needle {
    pub disks: Vec<Disk>,
    pub number: u8,
}

impl Needle {
    pub fn draw(&self, row: u8) {
        //println!("printing row: {}", row);

        let offset = match self.number {
            1 => "       ",
            _ => "\t\t\t",
        };

        let row = row as usize;

        if self.disks.len() >= row {
            self.disks[row - 1].draw();
        } else {
            print!("{offset}");
            print!("*");
        }
    }
}
