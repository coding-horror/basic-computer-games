pub type Position = (u8, u8, u8);

pub struct Game {
    wallet: u8,
    bet: u8,
    landmines: Vec<Position>,
    player: Position,
}

impl Game {
    pub fn new() -> Self {
        let mut landmines = Vec::new();

        let mut m: Position = (0, 0, 0);
        while !landmines.contains(&m) {}
    }
}
