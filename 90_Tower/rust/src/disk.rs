pub struct Disk {
    size: u8,
}

impl Disk {
    pub fn new(size: u8) -> Self {
        Disk { size }
    }

    pub fn draw(&self) {
        let draw_space = || {
            let space_amount = (15 - self.size) / 2;

            if space_amount > 0 {
                for _ in 0..space_amount {
                    print!(" ");
                }
            }
        };

        draw_space();
        for _ in 0..self.size {
            print!("*");
        }
        draw_space();
        print!("   ");
    }
}
