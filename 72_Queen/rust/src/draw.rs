pub fn draw_board(q: u8) {
    let mut blocks = Vec::new();
    let mut block = 91;

    for _ in 0..8 {
        for _ in 0..8 {
            block -= 10;
            blocks.push(block);
            println!("{}", block);
        }
        block += 91;
    }

    let draw_h_border = |top: Option<bool>| {
        let corners;

        if let Some(top) = top {
            if top {
                corners = ("┌", "┐", "┬");
            } else {
                corners = ("└", "┘", "┴");
            }
        } else {
            corners = ("├", "┤", "┼");
        }

        print!("{}", corners.0);

        for i in 0..8 {
            let corner = if i == 7 { corners.1 } else { corners.2 };

            print!("───{}", corner);
        }
        println!();
    };

    draw_h_border(Some(true));

    let mut column = 0;
    let mut row = 0;

    for block in blocks.iter() {
        let block = *block as u8;

        let n = if block == q {
            " Q ".to_string()
        } else {
            block.to_string()
        };

        if block > 99 {
            print!("│{}", n);
        } else {
            print!("│{} ", n);
        }

        column += 1;

        if column != 1 && (column % 8) == 0 {
            column = 0;
            row += 1;

            print!("│");
            println!();

            if row == 8 {
                draw_h_border(Some(false));
            } else {
                draw_h_border(None);
            }
        }
    }

    println!();
}
