use std::{ops::{Mul, Add}, fmt::Display, collections::HashMap};

use rand::Rng;

use crate::view;

pub struct Galaxy {
    pub stardate: f32,
    pub final_stardate: f32,
    pub quadrants: Vec<Quadrant>,
    pub enterprise: Enterprise
}

pub struct Quadrant {
    pub stars: Vec<Pos>,
    pub star_base: Option<Pos>,
    pub klingons: Vec<Klingon>
}

pub struct Klingon {
    pub sector: Pos,
    energy: f32
}

impl Klingon {
    pub fn fire_on(&mut self, enterprise: &mut Enterprise) {
        let mut rng = rand::thread_rng();
        let attack_strength = rng.gen::<f32>();
        let dist_to_enterprise = self.sector.abs_diff(enterprise.sector) as f32;
        let hit_strength = self.energy * (2.0 + attack_strength) / dist_to_enterprise;
        
        self.energy /= 3.0 + attack_strength;

        enterprise.take_hit(self.sector, hit_strength as u16);
    }
}

pub struct Enterprise {
    pub destroyed: bool,
    pub damaged: HashMap<String, f32>,
    pub quadrant: Pos,
    pub sector: Pos,
    pub photon_torpedoes: u8,
    pub total_energy: u16,
    pub shields: u16,
}
impl Enterprise {
    fn take_hit(&mut self, sector: Pos, hit_strength: u16) {
        if self.destroyed {
            return;
        }
        
        view::enterprise_hit(&hit_strength, &sector);

        self.shields = (self.shields - hit_strength).max(0);

        if self.shields <= 0 {
            view::enterprise_destroyed();
            self.destroyed = true
        }

        view::shields_hit(self.shields);
        
        if hit_strength >= 20 {
            self.take_damage(hit_strength)
        }
    }

    fn take_damage(&mut self, hit_strength: u16) {
        let mut rng = rand::thread_rng();

        let hit_past_shield = hit_strength as f32 / self.shields as f32;
        if rng.gen::<f32>() > 0.6 || hit_past_shield < 0.02 {
            return
        }

        let system = systems::KEYS[rng.gen_range(0..systems::KEYS.len())].to_string();
        let damage = hit_past_shield + rng.gen::<f32>() * 0.5;
        self.damage_system(&system, damage);
    }

    pub fn damage_system(&mut self, system: &str, damage: f32) {
        self.damaged.entry(system.to_string()).and_modify(|d| *d -= damage).or_insert(-damage);
    }

    pub fn repair_system(&mut self, system: &str, amount: f32) -> bool {
        let existing_damage = self.damaged[system];
        if existing_damage + amount >= -0.1 {
            self.damaged.remove(system);
            return true;
        }
    
        self.damaged.entry(system.to_string()).and_modify(|d| *d += amount);
        return false;
    }

    pub fn check_stranded(&self) -> bool {
        if self.total_energy < 10 || (self.total_energy - self.shields < 10 && self.damaged.contains_key(systems::SHIELD_CONTROL)) {
            view::stranded();
            return true;
        }
        return false;
    }
}

pub mod systems {

    pub const SHORT_RANGE_SCAN: &str = "SRS";
    pub const WARP_ENGINES: &str = "NAV";
    pub const SHIELD_CONTROL: &str = "SHE";
    pub const DAMAGE_CONTROL: &str = "DAM";
    pub const LONG_RANGE_SCAN: &str = "LRS";

    pub const KEYS: [&str; 5] = [
        SHORT_RANGE_SCAN, WARP_ENGINES, SHIELD_CONTROL, DAMAGE_CONTROL, LONG_RANGE_SCAN
    ];

    pub fn name_for(key: &str) -> String {
        match key {
            SHORT_RANGE_SCAN => "Short Range Scanners".into(),
            WARP_ENGINES => "Warp Engines".into(),
            SHIELD_CONTROL => "Shield Control".into(),
            DAMAGE_CONTROL => "Damage Control".into(),
            LONG_RANGE_SCAN => "Long Range Scanners".into(),
            _ => "Unknown".into()
        }
    }
}

pub struct EndPosition {
    pub quadrant: Pos,
    pub sector: Pos,
    pub hit_edge: bool,
    pub energy_cost: u16,
}

#[derive(PartialEq, Clone, Copy, Debug)]
pub struct Pos(pub u8, pub u8);

impl Pos {
    pub fn as_index(&self) -> usize {
        (self.0 * 8 + self.1).into()
    }

    fn abs_diff(&self, other: Pos) -> u8 {
        self.0.abs_diff(other.0) + self.1.abs_diff(other.1)
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
        write!(f, "{} , {}", self.0 + 1, self.1 + 1)
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

impl Galaxy {
    pub fn remaining_klingons(&self) -> u8 {
        let quadrants = &self.quadrants;
        quadrants.into_iter().map(|q| { q.klingons.len() as u8 }).sum::<u8>()
    }

    pub fn remaining_starbases(&self) -> u8 {
        let quadrants = &self.quadrants;
        quadrants.into_iter().filter(|q| q.star_base.is_some()).count() as u8
    }

    pub fn generate_new() -> Self {
        let quadrants = Self::generate_quadrants();

        let mut rng = rand::thread_rng();
        let enterprise_quadrant = Pos(rng.gen_range(0..8), rng.gen_range(0..8));
        let enterprise_sector = quadrants[enterprise_quadrant.as_index()].find_empty_sector();
        let stardate = rng.gen_range(20..=40) as f32 * 100.0;

        Galaxy { 
            stardate,
            final_stardate: stardate + rng.gen_range(25..=35) as f32,
            quadrants: quadrants, 
            enterprise: Enterprise { 
                destroyed: false,
                damaged: HashMap::new(),
                quadrant: enterprise_quadrant, 
                sector: enterprise_sector,
                photon_torpedoes: 28,
                total_energy: 3000,
                shields: 0 }
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
                    quadrant.klingons.push(Klingon { sector: quadrant.find_empty_sector(), energy: rng.gen_range(100..=300) as f32 });
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

    pub fn find_empty_sector(&self) -> Pos {
        let mut rng = rand::thread_rng();
        loop {
            let pos = Pos(rng.gen_range(0..8), rng.gen_range(0..8));
            if self.sector_status(&pos) == SectorStatus::Empty {
                return pos
            }
        }
    }
}
