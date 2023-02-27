
pub struct Galaxy {
    pub quadrants: Vec<Quadrant>,
    pub enterprise: Enterprise,
    pub game_status: GameStatus
}

#[derive(PartialEq)]
pub struct Pos(pub u8, pub u8);

impl Pos {
    pub fn as_index(&self) -> usize {
        (self.0 * 8 + self.1).into()
    }
}

pub struct Quadrant {
    pub stars: Vec<Pos>,
    pub star_bases: Vec<Pos>,
    pub klingons: Vec<Klingon>
}

pub struct Klingon {
    pub sector: Pos
}

pub struct Enterprise {
    pub quadrant: Pos,
    pub sector: Pos,
}

pub enum GameStatus {
    ShortRangeScan
}

impl Galaxy {
    pub fn generate_new() -> Self {
        Galaxy { 
            quadrants: Vec::new(), 
            enterprise: Enterprise { quadrant: Pos(0,0), sector: Pos(0,0) }, 
            game_status: GameStatus::ShortRangeScan 
        }
    }    
}