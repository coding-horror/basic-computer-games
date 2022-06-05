use std::io;

fn print_intro() {
    println!(
        "Welcome to Even Wins!
Based on evenwins.bas from Creative Computing

Even Wins is a two-person game. You start with
27 marbles in the middle of the table.

Players alternate taking marbles from the middle.
A player can take 1 to 4 marbles on their turn, and
turns cannot be skipped. The game ends when there are
no marbles left, and the winner is the one with an even
number of marbles.
"
    );
}

#[derive(Debug)]
enum PlayerType {
    Human,
    Computer,
}

#[derive(Debug)]
struct Game {
    turn: PlayerType,
    middle: u32,
    human: u32,
    computer: u32,
    min_take: u32,
    max_take: u32,
}

impl Game {
    fn get_max_take(&mut self) -> u32 {
        if self.max_take > self.middle {
            return self.middle;
        }
        return self.max_take;
    }

    fn take(&mut self, num: u32) -> bool {
        let max_take = self.get_max_take();
        if num > max_take {
            println!("You can take at most {} marbles", max_take);
            return false;
        }
        if num < self.min_take {
            println!("You must take at least {} marble!", self.min_take);
            return false;
        }

        self.middle -= num;
        match self.turn {
            PlayerType::Computer => self.computer += num,
            PlayerType::Human => self.human += num,
        }
        return true;
    }

    fn next(&mut self) {
        self.turn = match self.turn {
            PlayerType::Computer => PlayerType::Human,
            PlayerType::Human => PlayerType::Computer,
        }
    }

    fn info(&mut self) {
        println!("");
        println!(
            "marbles in the middle: {} **************************",
            self.middle
        );
        println!("# marbles you have: {}", self.human);
        println!("# marbles computer has: {}", self.computer);
        println!("");
    }

    fn wininfo(&mut self) {
        if self.middle != 0 {
            return;
        }
        println!("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!\n!! All the marbles are taken: Game Over!\n!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        self.info();

        if self.human % 2 == 0 {
            println!("You are the winner! Congratulations!");
        } else {
            println!("The computer wins: all hail mighty silicon!");
        }

        println!("");
    }
}

fn human_play(game: &mut Game) {
    println!("It's your turn!");
    loop {
        let max_take = game.get_max_take();
        println!("Marbles to take? ({} - {}) --> ", game.min_take, max_take);

        let mut num = String::new();
        io::stdin()
            .read_line(&mut num)
            .expect("Failed to read line");

        let _: u32 = match num.trim().to_uppercase().parse() {
            Ok(num) => {
                if game.take(num) {
                    println!("Okay, taking {} marble ...", num);
                    break;
                };
                println!("");
                continue;
            }
            _ => {
                println!("Please enter a whole number from 1 to 4");
                println!("");
                continue;
            }
        };
    }
}

fn compute_play(game: &mut Game) {
    println!("It's the computer's turn ...");

    let marbles_to_take: u32;

    // the magic 6 and 1.5, 5.3 3.4 4.7 3.5 was copy from python implement
    let r: f32 = (game.middle % 6) as f32;
    if game.human % 2 == 0 {
        if r < 1.5 || r > 5.3 {
            marbles_to_take = 1;
        } else {
            marbles_to_take = (r - 1.0) as u32;
        }
    } else if game.middle <= 4 {
        marbles_to_take = game.middle
    } else if r > 3.4 {
        if r < 4.7 || r > 3.5 {
            marbles_to_take = 4;
        } else {
            marbles_to_take = 1;
        }
    } else {
        marbles_to_take = (r + 1.0) as u32;
    }

    game.take(marbles_to_take);
    println!("Computer takes {} marble ...", marbles_to_take);
}

fn run_game(turn: PlayerType) {
    let mut game = Game {
        turn: turn,
        middle: 27,
        computer: 0,
        human: 0,
        min_take: 1,
        max_take: 4,
    };

    while game.middle > 0 {
        match game.turn {
            PlayerType::Computer => {
                compute_play(&mut game);
            }
            PlayerType::Human => {
                human_play(&mut game);
            }
        }
        game.info();
        game.next();
    }
    game.wininfo();
}

fn choose_first_player() -> PlayerType {
    loop {
        println!("Do you want to play first? (y/n) -->");

        let mut flag = String::new();
        io::stdin()
            .read_line(&mut flag)
            .expect("Failed to read line");

        match flag.trim().to_uppercase().as_str() {
            "Y" => return PlayerType::Human,
            "N" => return PlayerType::Computer,
            _ => {
                println!("Please enter \"y\" if you want to play first,\nor \"n\" if you want to play second.\n");
            }
        };
    }
}

fn choose_play_again() -> bool {
    println!("Would you like to play again? (y/n) --> ");
    let mut flag = String::new();
    io::stdin()
        .read_line(&mut flag)
        .expect("Failed to read line");

    match flag.trim().to_uppercase().as_str() {
        "Y" => {
            println!("\nOk, let's play again ...\n");
            return true;
        }
        _ => {
            println!("\nOk, thanks for playing ... goodbye!\n");
            return false;
        }
    }
}

fn main() {
    print_intro();
    loop {
        let first = choose_first_player();
        run_game(first);

        if !choose_play_again() {
            return;
        }
    }
}
