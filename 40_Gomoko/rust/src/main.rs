use std::{
    fmt::Display,
    io::{BufRead, Write},
};

use rand::prelude::*;

fn main() {
    println!("{:>33}GOMOKU", "");
    println!("{:>15}CREATIVE COMPUTING  MORISSTOWN, NEW JERSEY\n\n\n", "");

    println!("Welcome to the oriental game of Gomoku.");
    println!("\nThe game is played on an N by N grid of a size");
    println!("that you specify. During your play, you may cover one grid");
    println!("intersection with a marker. The object of the game is to get");
    println!("5 adjacent markers in a row -- horizontally, vertically, or");
    println!("diagonally. On the board diagram, your moves are marked");
    println!("with a '1' and the computer moves with a '2'.");
    println!("\nThe computer does not keep track of who has won.");
    println!("To end the game, type -1,-1 for your move.\n");

    loop {
        let size = get_board_size();

        // Line 210: initialize the game board
        let mut game = Gomoku::new(size);

        if !game.play() {
            break;
        }
    }
}

struct Gomoku {
    board: Vec<Vec<Piece>>,
    size: isize,
}

impl Gomoku {
    fn new(size: usize) -> Self {
        Self {
            board: vec![vec![Piece::Empty; size]; size],
            size: size as isize,
        }
    }

    fn get_player_move(&self) -> (isize, isize) {
        let mut first_attempt = true;
        loop {
            if !first_attempt {
                println!("Illegal move. Try again...");
            }
            first_attempt = false;
            let response = prompt_for_input("Your play (I,J)");
            let Some((i, j)) = response.split_once(',') else {
                continue;
            };
            let Ok(i) = i.trim().parse::<isize>() else {
                continue;
            };
            let Ok(j) = j.trim().parse::<isize>() else {
                continue;
            };
            if i == -1 {
                return (-1, -1);
            }
            if i < 1 || i > self.size || j < 1 || j > self.size {
                continue;
            }
            return (i, j);
        }
    }

    fn play(&mut self) -> bool {
        let mut rng = rand::rng();

        // Line 300
        println!("\nWe alternate moves. You go first...\n");

        'next_move: loop {
            println!("{self}");

            // Line 310
            let (i, j) = self.get_player_move();

            // Line 320
            if i == -1 {
                break;
            }

            let pos = self.get(i, j);

            match *pos {
                Piece::Empty => *pos = Piece::Human,
                Piece::Computer | Piece::Human => {
                    println!("Square occupied. Try again...");
                    continue;
                }
            }

            // Lines 500-590: The computer tries an intelligent move
            for e in -1..=1 {
                for f in -1..=1 {
                    // Line 510:
                    if e == 0 && f == 0 {
                        continue;
                    }

                    // Line 540:
                    let x = i + e;
                    let y = j + f;
                    if x < 1 || x > self.size || y < 1 || y > self.size {
                        continue;
                    }

                    let pos = self.get(x, y);
                    if *pos == Piece::Human {
                        let x = i - e;
                        let y = j - f;
                        if x < 1 || x > self.size || y < 1 || y > self.size {
                            continue;
                        }
                        let pos = self.get(x, y);
                        if *pos == Piece::Empty {
                            *pos = Piece::Computer;
                            continue 'next_move;
                        }
                    }
                }
            }

            // Lines 600-660: The computer tries a random move
            loop {
                let x = rng.random_range(1..=self.size as usize) as isize;
                let y = rng.random_range(1..=self.size as usize) as isize;
                let pos = self.get(x, y);
                if *pos == Piece::Empty {
                    *pos = Piece::Computer;
                    continue 'next_move;
                }
            }
        }

        // Lines 980-999
        println!("\nThanks for the game!!");
        prompt_for_input("Play again (1 for yes, 0 for no)") == "1"
    }

    fn get(&mut self, i: isize, j: isize) -> &mut Piece {
        &mut self.board[i as usize - 1][j as usize - 1]
    }
}

impl Display for Gomoku {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        for row in &self.board {
            for col in row {
                write!(f, "{col}")?;
            }
            writeln!(f)?;
        }
        Ok(())
    }
}

#[derive(Clone, Copy, PartialEq)]
enum Piece {
    Empty,
    Computer,
    Human,
}

impl Display for Piece {
    fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result {
        write!(
            f,
            " {}",
            match self {
                Piece::Empty => '0',
                Piece::Computer => '2',
                Piece::Human => '1',
            }
        )
    }
}

fn prompt_for_input<S: AsRef<str>>(prompt: S) -> String {
    print!("{}? ", prompt.as_ref());
    let mut lock = std::io::stdout().lock();
    let _ = lock.flush();

    let mut buffer = String::new();
    let stdin = std::io::stdin();
    let mut handle = stdin.lock();
    let _ = handle.read_line(&mut buffer);
    buffer.trim().to_string()
}

fn get_board_size() -> usize {
    loop {
        let buffer = prompt_for_input("What is your board size (min 7/ max 19)");
        if let Ok(val) = buffer.trim().parse::<usize>()
            && (7..=19).contains(&val)
        {
            return val;
        }
        println!("I said, the minimum is 7, the maximum is 19.");
    }
}
