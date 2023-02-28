use rand::Rng;

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

#[derive(PartialEq)]
pub enum SectorStatus {
    Empty, Star, StarBase, Klingon
}

pub struct Quadrant {
    pub stars: Vec<Pos>,
    pub star_base: Option<Pos>,
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
        let quadrants = Self::generate_quadrants();

        let mut rng = rand::thread_rng();
        let enterprise_quadrant = Pos(rng.gen_range(0..8), rng.gen_range(0..8));
        let enterprise_sector = quadrants[enterprise_quadrant.as_index()].find_empty_sector();

        Galaxy { 
            quadrants: quadrants, 
            enterprise: Enterprise { quadrant: enterprise_quadrant, sector: enterprise_sector }, 
            game_status: GameStatus::ShortRangeScan 
        }
    }    

    fn generate_quadrants() -> Vec<Quadrant> {
        let mut rng = rand::thread_rng();
        let mut result = Vec::new();
        for _ in 0..64 {

            let mut quadrant = Quadrant { stars: Vec::new(), star_base: None, klingons: Vec::new() };
            let star_count = rng.gen_range(0..=7);
            for _ in 0..star_count {
                quadrant.stars.push(quadrant.find_empty_sector());
            }

            if rng.gen::<f64>() > 0.96 {
                quadrant.star_base = Some(quadrant.find_empty_sector());
            }

            let klingon_count = 
                match rng.gen::<f64>() {
                    n if n > 0.98 => 3,
                    n if n > 0.95 => 2,
                    n if n > 0.8 => 1,
                    _ => 0
                };
                for _ in 0..klingon_count {
                    quadrant.klingons.push(Klingon { sector: quadrant.find_empty_sector() });
                }

            result.push(quadrant);
        }
        result
    }
}

impl Quadrant {
    pub fn sector_status(&self, sector: &Pos) -> SectorStatus {
        if self.stars.contains(&sector) {
            SectorStatus::Star
        } else if self.is_starbase(&sector) {
            SectorStatus::StarBase
        } else if self.has_klingon(&sector) {
            SectorStatus::Klingon
        } else {
            SectorStatus::Empty
        }
    }

    fn is_starbase(&self, sector: &Pos) -> bool {
        match &self.star_base {
            None => false,
            Some(p) => p == sector
        }
    }

    fn has_klingon(&self, sector: &Pos) -> bool {
        let klingons = &self.klingons;
        klingons.into_iter().find(|k| &k.sector == sector).is_some()
    }

    fn find_empty_sector(&self) -> Pos {
        let mut rng = rand::thread_rng();
        loop {
            let pos = Pos(rng.gen_range(0..8), rng.gen_range(0..8));
            if self.sector_status(&pos) == SectorStatus::Empty {
                return pos
            }
        }
    }
}