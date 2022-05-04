use crate::utility;

pub struct Game {}

impl Game {
    pub fn new() -> Game {
        let terminal_velocity = utility::get_terminal_velocity(
            "SELECT YOUR OWN TERMINAL VELOCITY",
            "WHAT TERMINAL VELOCITY (MI/HR)?",
        );

        let acceleration = utility::get_acceleration(
            "WANT TO SELECT ACCELERATION DUE TO GRAVITY",
            "WHAT ACCELERATION (FT/SEC/SEC)?",
        );

        Game {}
    }

    pub fn tick(&self) -> Option<bool> {
        todo!()
    }
}
