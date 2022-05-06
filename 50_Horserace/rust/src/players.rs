use crate::util::{self, PromptResult};

#[derive(Debug)]
pub struct Player {
    pub name: String,
    pub money: u32,
    pub playing: bool,
    pub horse_no: u8,
    pub bet: u32,
}

impl Player {
    fn new(name: String) -> Self {
        Player {
            name,
            money: 100000,
            playing: true,
            horse_no: 0,
            bet: 0,
        }
    }
}

#[derive(Debug)]
pub struct Players {
    players: Vec<Player>,
}

impl Players {
    pub fn new() -> Self {
        let players;

        loop {
            if let PromptResult::Numeric(n) = util::prompt(Some(true), "HOW MANY WANT TO BET?") {
                if n <= 0 {
                    println!("THERE CAN'T BE (LESS THAN) ZERO PLAYERS!");
                } else if n > 10 {
                    println!("THERE CAN'T BE MORE THAN TEN PLAYERS!");
                } else {
                    println!("WHEN ? APPEARS, TYPE NAME");
                    players = Players::generate_players(n);
                    break;
                }
            }
        }

        Players { players }
    }

    pub fn make_bets(&mut self) {
        println!("PLACE YOUR BETS...HORSE # THEN AMOUNT");

        for p in self.players.iter_mut() {
            if !p.playing {
                continue;
            }

            let name = format!("{}?", p.name);

            'prompt: loop {
                if let PromptResult::Normal(response) = util::prompt(None, name.as_str()) {
                    let response: Vec<&str> = response.trim().split(",").collect();

                    for (i, n) in response.iter().enumerate() {
                        if let Ok(n) = n.parse::<i32>() {
                            if n.is_negative() {
                                println!("YOU CAN'T ENTER A NEGATIVE NUMBER!")
                            } else {
                                match i {
                                    0 => {
                                        if n > 8 {
                                            println!("INVALID HORSE #")
                                        } else {
                                            p.horse_no = n as u8;
                                        }
                                    }
                                    1 => {
                                        if n == 0 {
                                            println!("YOU CAN'T BET NOTHING!");
                                        } else if n > p.money as i32 {
                                            println!("YOU DON'T HAVE ENOUGH MONEY!")
                                        } else {
                                            p.bet = n as u32;
                                            break 'prompt;
                                        }
                                    }
                                    _ => println!("YOU CAN'T ENTER MORE THAN 2 NUMBERS!"),
                                }
                            }
                        } else {
                            println!("ONLY ENTER NUMBERS PLEASE!");
                        }
                    }
                }
            }
        }
    }

    fn generate_players(n: i32) -> Vec<Player> {
        let mut players: Vec<Player> = Vec::new();

        for _ in 0..n {
            loop {
                if let PromptResult::Normal(name) = util::prompt(None, "?") {
                    let name = name.trim().to_uppercase();

                    if name.is_empty() {
                        println!("NAME CAN'T BE EMPTY!");
                    } else if let Some(_) = players.iter().find(|p| p.name == name) {
                        println!("THERE IS ALREADY A PLAYER WITH THAT NAME!");
                    } else {
                        players.push(Player::new(name));
                        break;
                    }
                }
            }
        }

        players
    }

    pub fn process_winner(&mut self, no: u8) {
        println!();
        for p in self.players.iter_mut() {
            if !p.playing {
                continue;
            }

            if p.horse_no == no {
                p.money += p.bet;
                println!("{} WON ${}! THEY HAVE ${}.", p.name, p.bet, p.money);
            } else {
                p.money -= p.bet;
                println!("{} LOST ${}. THEY HAVE ${} LEFT.", p.name, p.bet, p.money);
            }
            p.bet = 0;
            p.horse_no = 0;
        }
        println!();
    }

    pub fn prompt_next_round(&mut self) -> bool {
        for p in self.players.iter_mut() {
            let msg = format!("{}, DO YOU WANT TO BET ON THE NEXT RACE?", p.name);
            if let PromptResult::YesNo(yes) = util::prompt(Some(false), msg.as_str()) {
                p.playing = yes;
            }
        }

        if let None = self.players.iter().find(|p| p.playing) {
            return false;
        }

        true
    }
}
