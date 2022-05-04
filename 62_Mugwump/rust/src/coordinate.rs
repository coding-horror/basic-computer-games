#[derive(Debug)]
pub struct Coordinate {
    x: usize,
    y: usize,
    pub state: CoordState,
}

impl Coordinate {
    pub fn new(pos: (usize, usize), has_mugwump: bool) -> Self {
        let state = if has_mugwump {
            CoordState::HasMugwump
        } else {
            CoordState::Normal
        };

        Coordinate {
            x: pos.0,
            y: pos.1,
            state,
        }
    }
}

#[derive(Debug, PartialEq)]
pub enum CoordState {
    Normal,
    HasMugwump,
    Checked,
}
