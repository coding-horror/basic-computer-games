use crate::{
    ai,
    util::{self, prompt, PromptResult::*},
};

pub struct Game {
    blocks: [u8; 64],
    location: Option<u8>,
    player_move: bool,
    show_board: bool,
    forfeit: bool,
}

impl Game {
    pub fn new() -> Self {
        let mut blocks = [0 as u8; 64];

        let mut block = 91;
        let mut i = 0;

        for _ in 0..8 {
            for _ in 0..8 {
                block -= 10;
                blocks[i] = block;
                i += 1;
            }
            block += 91;
        }

        Game {
            blocks,
            location: None,
            player_move: true,
            show_board: false,
            forfeit: false,
        }
    }

    pub fn update(&mut self) -> bool {
        let mut still_going = true;

        if let Some(l) = self.location {
            if self.show_board {
                self.draw(l);
            }

            match self.player_move {
                true => self.player_move(l),
                false => {
                    std::thread::sleep(std::time::Duration::from_secs(1));
                    let loc = ai::get_computer_move(l);
                    println!("COMPUTER MOVES TO SQUARE {}", loc);
                    self.location = Some(loc);
                }
            }

            still_going = self.check_location();
        } else {
            self.set_start_location();
        }
        self.player_move = !self.player_move;
        still_going
    }

    fn set_start_location(&mut self) {
        self.draw(0);

        if let YesNo(yes) = prompt(Some(false), "UPDATE BOARD?") {
            self.show_board = yes;
        }

        loop {
            if let Numeric(n) = prompt(Some(true), "WHERE WOULD YOU LIKE TO START?") {
                let n = n as u8;

                if util::is_legal_start(n) {
                    self.location = Some(n);
                    break;
                } else {
                    println!("PLEASE READ THE DIRECTIONS AGAIN.\nYOU HAVE BEGUN ILLEGALLY.\n")
                }
            }
        }
    }

    fn player_move(&mut self, loc: u8) {
        loop {
            if let Numeric(n) = prompt(Some(true), "WHAT IS YOUR MOVE?") {
                if n == 0 {
                    self.forfeit = true;
                    break;
                }

                let n = n as u8;

                if util::is_move_legal(loc, n) {
                    self.location = Some(n);
                    break;
                } else {
                    println!("Y O U   C H E A T . . .  TRY AGAIN? ");
                }
            }
        }
    }

    fn check_location(&self) -> bool {
        if self.location == Some(158) {
            util::print_gameover(self.player_move, self.forfeit);
            return false;
        }

        true
    }

    fn draw(&self, loc: u8) {
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

        for block in self.blocks.iter() {
            let block = *block as u8;

            let n = if block == loc {
                format!("│{}", "*Q*".to_string())
            } else {
                let b = block.to_string();

                if block > 99 {
                    format!("│{}", b)
                } else {
                    format!("│{} ", b)
                }
            };

            print!("{}", n);

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
}
