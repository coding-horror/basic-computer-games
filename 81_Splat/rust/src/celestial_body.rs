use rand::{prelude::SliceRandom, Rng};

#[derive(Debug)]
pub enum CelestialBody {
    MERCURY,
    VENUS,
    EARTH,
    MOON,
    MARS,
    JUPITER,
    SATURN,
    URANUS,
    NEPTUNE,
    SUN,
}

impl CelestialBody {
    pub fn get_acceleration(&self) -> f32 {
        use CelestialBody::*;

        match self {
            MERCURY => 12.2,
            VENUS => 28.3,
            EARTH => 32.16,
            MOON => 5.12,
            MARS => 12.5,
            JUPITER => 85.2,
            SATURN => 37.6,
            URANUS => 33.8,
            NEPTUNE => 39.6,
            SUN => 896.,
        }
    }

    pub fn print_acceleration_message(&self) {
        let messages: [&str; 3] = ["Fine,", "All right,", "Then"];
        let m = messages.choose(&mut rand::thread_rng()).unwrap();

        println!(
            "{} YOU'RE ON {:?}. ACCELERATION = {} FT/SEC/SEC.",
            m.to_uppercase(),
            self,
            self.get_acceleration()
        );
    }
}

pub fn random_celestial_body() -> Option<CelestialBody> {
    use CelestialBody::*;

    match rand::thread_rng().gen_range(0..10) {
        0 => Some(MERCURY),
        1 => Some(VENUS),
        2 => Some(EARTH),
        3 => Some(MOON),
        4 => Some(MARS),
        5 => Some(JUPITER),
        6 => Some(SATURN),
        7 => Some(URANUS),
        8 => Some(NEPTUNE),
        9 => Some(SUN),
        _ => None,
    }
}
