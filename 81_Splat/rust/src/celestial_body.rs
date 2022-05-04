use rand::{prelude::SliceRandom, Rng};

#[derive(Debug)]
pub enum CelestialBody {
    Mercury,
    Venus,
    Earth,
    Moon,
    Mars,
    Jupiter,
    Saturn,
    Uranus,
    Neptune,
    Sun,
}

impl CelestialBody {
    pub fn get_acceleration(&self) -> f32 {
        use CelestialBody::*;

        match self {
            Mercury => 12.2,
            Venus => 28.3,
            Earth => 32.16,
            Moon => 5.12,
            Mars => 12.5,
            Jupiter => 85.2,
            Saturn => 37.6,
            Uranus => 33.8,
            Neptune => 39.6,
            Sun => 896.,
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
        0 => Some(Mercury),
        1 => Some(Venus),
        2 => Some(Earth),
        3 => Some(Moon),
        4 => Some(Mars),
        5 => Some(Jupiter),
        6 => Some(Saturn),
        7 => Some(Uranus),
        8 => Some(Neptune),
        9 => Some(Sun),
        _ => None,
    }
}
