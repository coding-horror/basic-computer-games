pub fn is_move_legal(loc: u8, mov: u8) -> bool {
    let dt: i32 = (mov - loc).into();

    if dt.is_negative() {
        return false;
    }

    if (dt % 21) == 0 || (dt % 10) == 0 || (dt % 11) == 0 {
        return true;
    }

    false
}

pub fn is_legal_start(loc: u8) -> bool {
    let mut legal_spots = Vec::new();
    let start: u8 = 11;

    legal_spots.push(start);

    for i in 1..=7 {
        legal_spots.push(start + (10 * i));
    }

    for i in 1..=7 {
        legal_spots.push(start + (11 * i));
    }

    if legal_spots.contains(&loc) {
        true
    } else {
        false
    }
}
