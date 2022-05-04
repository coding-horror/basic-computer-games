use crate::coordinate::{CoordState, Coordinate};

pub fn draw_board(coords: Vec<Coordinate>) {
    let draw_top_bottom = |is_top: bool| {
        let (mut left, mut right) = ("╔", "╗");

        if !is_top {
            (left, right) = ("╚", "╝");
        }

        for i in 0..11 {
            if i == 0 {
                print!("{}══", left);
            } else if i == 10 {
                print!("═══{}", right)
            } else {
                print!("══");
            }
        }
        println!("");
    };

    draw_top_bottom(true);

    let mut y: i8 = 9;

    print!("║ {} ", y);
    for (i, c) in coords.iter().enumerate() {
        let mut _char = ' ';

        match c.state {
            CoordState::Normal => _char = '-',
            CoordState::HasMugwump => _char = '𑗌',
            CoordState::Checked => _char = '*',
        }

        print!("{} ", _char);

        if ((i + 1) % 10) == 0 {
            y -= 1;

            print!("║");
            println!("");

            if i != 99 {
                print!("║ {} ", y);
            }
        }
    }

    print!("║ 𑗌 ");
    for i in 0..10 {
        print!("{} ", i);

        if i == 9 {
            print!("║");
        }
    }
    println!("");

    draw_top_bottom(false);
}
