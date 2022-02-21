use std::fmt::{self, Display};
use std::io::{stdin, stdout, Write};

const WIN: [(usize, usize, usize); 8] = [
    (0, 1, 2),
    (3, 4, 5),
    (6, 7, 8),
    (0, 4, 8),
    (2, 4, 6),
    (0, 3, 6),
    (1, 4, 7),
    (2, 5, 8),
];

type Board = [Sign; 9];

fn main() {
    let mut board: Board = [Sign::E; 9];
    let mut sign = Sign::X;
    loop {
        clear();
        render(&board, &sign);
        let (win, winner) = check_board(board);
        if win {
            match winner {
                Sign::X => break println!("Looks like X own this one!"),
                Sign::O => break println!("O is the winner!!"),
                Sign::C => break println!("Cat got this one!"),
                Sign::E => {}
            }
        }
        let num = input("Pick a number 1 - 9:> ");
        if let Some(Sign::E) = board.get(num) {
            board.get_mut(num).map(|s| *s = sign);
            sign = if sign == Sign::X { Sign::O } else { Sign::X };
        }
    }
}

#[derive(Debug, Copy, Clone, PartialEq)]
enum Sign {
    X,
    O,
    C,
    E,
}

impl Display for Sign {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        let sign = match self {
            Self::X => 'X',
            Self::O => 'O',
            Self::C => 'C',
            Self::E => ' ',
        };
        write!(f, "{}", sign)
    }
}

fn check_board(board: Board) -> (bool, Sign) {
    for &(a, b, c) in WIN.iter() {
        if board[a] == board[b] && board[a] == board[c] {
            return (true, board[a]);
        }
    }
    if !board.contains(&Sign::E) {
        return (true, Sign::C);
    }
    (false, Sign::E)
}

fn clear() {
    println!("\x1b[2J\x1b[0;0H");
}

fn input(message: &str) -> usize {
    let mut out = String::new();
    loop {
        print!("{}", message);
        stdout().flush().expect("Failed to flush to stdout.");
        stdin().read_line(&mut out).expect("Failed to read line");
        let num = out.trim().parse::<usize>();
        match num {
            Ok(n) => match n {
                1..=9 => return n - 1,
                _ => println!("The number needs to be between 1 - 9."),
            },
            Err(_) => println!("'{}' is not a number.", out.trim()),
        }
        out.clear();
    }
}

fn render(spots: &Board, sign: &Sign) {
    println!("                    The board is numbered");
    println!(
        " {} │ {} │ {}  [{}: Turn]    1 │ 2 │ 3",
        spots[0], spots[1], spots[2], sign
    );
    println!("⎼⎼⎼╄⎼⎼⎼╄⎼⎼⎼             ⎼⎼⎼╄⎼⎼⎼╄⎼⎼⎼");
    println!(
        " {} │ {} │ {}               4 │ 5 │ 6 ",
        spots[3], spots[4], spots[5]
    );
    println!("⎼⎼⎼╄⎼⎼⎼╄⎼⎼⎼             ⎼⎼⎼╄⎼⎼⎼╄⎼⎼⎼");
    println!(
        " {} │ {} │ {}               7 │ 8 │ 9 ",
        spots[6], spots[7], spots[8]
    );
}
