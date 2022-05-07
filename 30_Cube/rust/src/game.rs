use crate::util;

pub type Position = (u8, u8, u8);

pub struct Game {
    wallet: usize,
    bet: Option<usize>,
    landmines: Vec<Position>,
    player: Position,
}

impl Game {
    pub fn new() -> Self {
        Game {
            wallet: 500,
            bet: None,
            landmines: util::get_landmines(),
            player: (1, 1, 1),
        }
    }

    pub fn play(&mut self) -> bool {
        self.bet = self.get_bet();

        let mut first_move = true;
        let mut result = (false, "******BANG!******\nYOU LOSE");

        loop {
            let msg = if first_move {
                first_move = false;
                "ITS YOUR MOVE"
            } else {
                "NEXT MOVE"
            };

            let (ok, p) = self.ask_position(msg);

            if ok {
                if p == (3, 3, 3) {
                    result.0 = true;
                    result.1 = "CONGRATULATIONS!";
                    break;
                } else if self.landmines.contains(&p) {
                    break;
                } else {
                    self.player = p;
                }
            } else {
                result.1 = "ILLEGAL MOVE\nYOU LOSE.";
                break;
            }
        }

        println!("{}", result.1);
        self.calculate_wallet(result.0);
        self.reset_game();

        if self.wallet <= 0 {
            println!("YOU ARE BROKE!");
            return false;
        }

        return util::prompt_bool("DO YOU WANT TO TRY AGAIN?");
    }

    fn get_bet(&self) -> Option<usize> {
        loop {
            if util::prompt_bool("WANT TO MAKE A WAGER?") {
                let b = util::prompt_number("HOW MUCH?");

                if b != 0 && b <= self.wallet {
                    return Some(b);
                } else {
                    println!("YOU CAN'T BET THAT!");
                }
            } else {
                return None;
            };
        }
    }

    fn ask_position(&self, msg: &str) -> (bool, Position) {
        if let Some(p) = util::prompt_position(msg, self.player) {
            return (true, p);
        }
        return (false, (0, 0, 0));
    }

    fn calculate_wallet(&mut self, win: bool) {
        if let Some(b) = self.bet {
            if win {
                self.wallet += b;
            } else {
                self.wallet -= b;
            }
            self.bet = None;
            println!("YOU NOW HAVE {} DOLLARS", self.wallet);
        }
    }

    fn reset_game(&mut self) {
        self.player = (1, 1, 1);
        self.landmines.clear();
        self.landmines = util::get_landmines();
    }
}
