use std::{ops::{Mul, Add}, fmt::Display};

use rand::Rng;

pub struct Galaxy {
    pub quadrants: Vec<Quadrant>,
    pub enterprise: Enterprise
}

#[derive(PartialEq, Clone, Copy)]
pub struct Pos(pub u8, pub u8);

impl Pos {
    pub fn as_index(&self) -> usize {
        (self.0 * 8 + self.1).into()
    }
}

impl Mul<u8> for Pos {
    type Output = Self;

    fn mul(self, rhs: u8) -> Self::Output {
        Pos(self.0 * rhs, self.1 * rhs)
    }
}

impl Add<Pos> for Pos {
    type Output = Self;

    fn add(self, rhs: Pos) -> Self::Output {
        Pos(self.0 + rhs.0, self.1 + rhs.1)
    }
}

impl Display for Pos {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(f, "{} , {}", self.0, self.1)
    }
}

pub const COURSES : [(i8, i8); 8] = [
    (1, 0),
    (1, -1),
    (0, -1),
    (-1, -1),
    (-1, 0),
    (-1, 1),
    (0, 1),
    (1, 1),
];

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

impl Galaxy {
    pub fn generate_new() -> Self {
        let quadrants = Self::generate_quadrants();

        let mut rng = rand::thread_rng();
        let enterprise_quadrant = Pos(rng.gen_range(0..8), rng.gen_range(0..8));
        let enterprise_sector = quadrants[enterprise_quadrant.as_index()].find_empty_sector();

        Galaxy { 
            quadrants: quadrants, 
            enterprise: Enterprise { quadrant: enterprise_quadrant, sector: enterprise_sector }
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