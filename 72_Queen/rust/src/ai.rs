use crate::util;

const PREFERRED_MOVES: [u8; 5] = [158, 72, 75, 126, 127];
const SAFE_MOVES: [u8; 2] = [44, 41];

pub fn get_computer_move(loc: u8) -> u8 {
    if SAFE_MOVES.contains(&loc) {
        return random_move(loc);
    }

    for m in PREFERRED_MOVES {
        if util::is_move_legal(loc, m) && m != loc {
            return m;
        }
    }

    random_move(loc)
}

fn random_move(l: u8) -> u8 {
    let r: f32 = rand::random();

    if r > 0.6 {
        l + 11
    } else if r > 0.3 {
        l + 21
    } else {
        l + 10
    }
}
