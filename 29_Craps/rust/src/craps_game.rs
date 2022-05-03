use crate::util;

enum GameState {
    ComeOut,
    PointRolls,
    GameOver,
}

pub struct CrapsGame {
    wallet: usize,
    bet: usize,
    point: u8,
    state: GameState,
}

impl CrapsGame {
    pub fn new() -> Self {
        let wallet = util::read_numeric("\nHow much money do you want to start with?");

        CrapsGame {
            wallet,
            bet: 0,
            point: 0,
            state: GameState::ComeOut,
        }
    }

    pub fn tick(&mut self) -> bool {
        use GameState::*;

        match self.state {
            ComeOut => self.new_round(),
            PointRolls => self.point_roll(),
            GameOver => false,
        }
    }

    fn point_roll(&mut self) -> bool {
        let point = self.point;

        println!("Rolling {point} wins. 7 loses.");

        self.prompt_roll();
        let roll = CrapsGame::roll();

        if roll == point {
            self.player_win();
        } else if roll == 7 {
            self.player_lose();
        }

        true
    }

    fn new_round(&mut self) -> bool {
        println!("\nCome out roll.");
        println!("7 and 11 win. 2, 3 and 12 lose.\n");

        loop {
            self.bet = util::read_numeric("Enter your bet:");

            if self.bet <= self.wallet {
                break;
            } else {
                println!("You don't have that much money!");
            }
        }

        self.prompt_roll();
        let point = CrapsGame::roll();

        match point {
            11 | 7 => {
                self.player_win();
            }
            2 | 3 | 12 => {
                self.player_lose();
            }
            _ => {
                self.point = point;
                self.state = GameState::PointRolls
            }
        }

        true
    }

    fn player_win(&mut self) {
        let bet = self.bet;

        println!("You won ${bet}!");

        self.wallet += bet;
        self.print_wallet();

        self.state = GameState::ComeOut;
    }

    fn player_lose(&mut self) {
        let bet = self.bet;

        println!("You lost ${bet}!");

        self.wallet -= bet;
        self.print_wallet();

        if self.wallet == 0 {
            self.game_over();
        } else {
            self.state = GameState::ComeOut;
        }
    }

    fn print_wallet(&self) {
        println!("\nYou have ${} in your wallet.", self.wallet);
    }

    fn roll() -> u8 {
        use rand::Rng;

        let roll = rand::thread_rng().gen_range(2..13);
        println!("\nYou rolled {}.", roll);

        roll
    }

    fn game_over(&mut self) {
        self.state = GameState::GameOver;
    }

    fn prompt_roll(&mut self) {
        use util::Response::*;

        let response = util::prompt("Ready to roll?");

        match response {
            Yes => (),
            No => self.game_over(),
        }
    }
}
