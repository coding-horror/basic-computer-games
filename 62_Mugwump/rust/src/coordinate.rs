#![allow(dead_code)]

#[derive(Debug)]
pub struct Coordinate {
    x: u8,
    y: u8,
    pub state: CoordState,
    pub mugwump_number: u8,
}

impl Coordinate {
    pub fn new(pos: (u8, u8), has_mugwump: bool, mugwump_number: i32) -> Self {
        let mut mug_no = 0;

        let state = if has_mugwump {
            mug_no = mugwump_number;
            CoordState::HasMugwump
        } else {
            CoordState::Normal
        };

        Coordinate {
            x: pos.0,
            y: pos.1,
            state,
            mugwump_number: mug_no as u8,
        }
    }

    pub fn get_pos(&self) -> (u8, u8) {
        (self.x, self.y)
    }
}

#[derive(Debug, PartialEq)]
pub enum CoordState {
    Normal,
    HasMugwump,
    Checked,
    FoundMugwump,
}
