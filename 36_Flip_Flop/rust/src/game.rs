pub struct Game {
    board: [char; 10],
    last_move: u8,
    entropy: f32,
    tries: u8,
}

impl Game {
    pub fn new() -> Self {
        Game {
            board: ['X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X'],
            last_move: 0,
            entropy: 0.,
            tries: 0,
        }
    }

    pub fn play(&mut self) -> bool {
        self.reset_game();

        println!("\nHERE IS THE STARTING LINE OF X'S.\n");
        println!("1 2 3 4 5 6 7 8 9 10");
        println!("X X X X X X X X X X\n");

        let mut reset = false;

        loop {
            self.tries += 1;

            match Game::get_number() {
                0 => self.reset_board(),
                11 => {
                    reset = true;
                    break;
                }
                n => {
                    self.flip(n);

                    let other = self.get_other(n, n == self.last_move);
                    if other != n {
                        self.flip(other);
                    }

                    println!("other: {}", other);
                    self.last_move = n;
                }
            }
            self.draw();

            if !self.board.iter().any(|c| *c == 'X') {
                break;
            }
        }

        if !reset {
            let t = self.tries;

            if t > 12 {
                println!("TRY HARDER NEXT TIME. IT TOOK YOU {t} GUESSES.");
            } else {
                println!("VERY GOOD. IT TOOK YOU ONLY {t} GUESSES.");
            }

            return morristown::prompt_bool("DO YOU WANT TO TRY ANOTHER PUZZLE?", false);
        }

        true
    }

    fn flip(&mut self, i: u8) {
        if (1..=10).contains(&i) {
            let i = (i - 1) as usize;
            let char = &mut self.board[i];

            match char {
                'X' => *char = '0',
                '0' => *char = 'X',
                _ => println!("INVALID BOARD CHARACTER!"),
            }
        }
    }

    fn get_other(&self, m: u8, equals_last_move: bool) -> u8 {
        let e = self.entropy;
        let m = m as f32;

        let rate = if equals_last_move {
            0.592 * (1. / (e / m + e).tan()) / (m * 2. + e).sin() - m.cos()
        } else {
            (e + m / e - m).tan() - (e / m).sin() + 336. * (8. * m).sin()
        };

        (10. * (rate - rate.floor())).floor() as u8
    }

    fn draw(&self) {
        println!("1 2 3 4 5 6 7 8 9 10");
        for c in self.board {
            print!("{c} ");
        }
        println!();
    }

    fn get_number() -> u8 {
        loop {
            let n = morristown::prompt_number::<u8>("INPUT THE NUMBER?");
            if n > 11 {
                println!("ILLEGAL ENTRY--TRY AGAIN.");
            } else {
                return n;
            }
        }
    }

    fn reset_board(&mut self) {
        self.board = ['X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X', 'X'];
    }

    fn reset_game(&mut self) {
        self.reset_board();
        self.last_move = 0;
        self.entropy = rand::random();
        self.tries = 0;
    }
}
