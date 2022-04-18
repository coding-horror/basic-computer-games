/*
 lib.rs contains all the logic of the program
*/

use std::{error::Error, fmt::Display, str::FromStr, io::{self, Write}};

use rand::Rng;

/// handles setup for the game
pub struct Config {
    players: Vec<Player>,
}
impl Config {
    /// creates and returns a new Config from user input
    pub fn new() -> Result<Config, Box<dyn Error>> {
        //DATA
        let mut config: Config = Config { players: Vec::new() };
        let num_players: usize;

        //get data from user input

        //get num players
        //input looop
        num_players = loop {
            match get_number_from_input("HOW MANY PLAYERS? ", 1, 0) {
                Ok(num) => break num,
                Err(e) => {
                    println!("{}",e);
                    continue;
                },
            }
        };

        //get names of all players
        for id in 1..=num_players {
            let name = get_string_from_user_input( format!("NAME OF PLAYER#{}: ", id).as_str())?;
            config.players.push(Player::from(&name));
        }

        //return new config
        return Ok(config);
    }
}
pub struct Player {
    name: String,
    score: usize,
}
impl Player {
    fn from(name: &str) -> Player {
        return Player { name: String::from(name), score: 0 };
    }
}

/// run the program
pub fn run(config: &mut Config) -> Result<(), Box<dyn Error>> {
    //DATA
    let mut round = 1;
    let mut rng = rand::thread_rng();

    //gameloop
    loop {
        //print round
        println!("");
        println!("");
        println!("ROUND {}", round);
        println!("---------");

        //each players play
        for player in config.players.iter_mut() {
            //prompt user for players move
            //input loop
            let throw = loop {
                match get_number_from_input( format!("{}'S THROW (input 1, 2, or 3): ", player.name).as_str(), 1, 3) {
                    Ok(t) => break t,
                    Err(err) => {
                        println!("{}", err);
                        continue;
                    },
                };
            };

            //get probabilities of the various outcomes based on the throw
            let p_bullseye;
            let p_30_point_zone;
            let p_20_point_zone;
            let p_10_point_zone;
            match throw {
                1 => {
                    p_bullseye = 0.65;
                    p_30_point_zone = 0.55;
                    p_20_point_zone = 0.5;
                    p_10_point_zone = 0.5;
                },
                2 => {
                    p_bullseye = 0.99;
                    p_30_point_zone = 0.77;
                    p_20_point_zone = 0.43;
                    p_10_point_zone = 0.01;
                },
                _ => {
                    p_bullseye = 0.95;
                    p_30_point_zone = 0.75;
                    p_20_point_zone = 0.45;
                    p_10_point_zone = 0.05;
                },
            }

            //determine results
            let roll = rng.gen::<f64>();
            if roll >= p_bullseye{
                println!("BULLSEYE!!  40 POINTS!");
                player.score += 40;
            } else if roll >= p_30_point_zone {
                println!("30-POINT ZONE!");
                player.score += 30;
            } else if roll >= p_20_point_zone {
                println!("20-POINT ZONE");
                player.score += 20;
            } else if roll >= p_10_point_zone {
                println!("WHEW!  10 POINTS.");
                player.score += 10;
            } else {
                println!("MISSED THE TARGET!  TOO BAD.");
            }

            //print their score
            println!("TOTAL SCORE = {}", player.score);
        }

        //stuff to do before next round
        if config.players.iter().any(|player| player.score >= 200) {
            //print congradulations and scores
            println!("\nWE HAVE A WINNER!!\n");
            for player in config.players.iter() {
                println!("{} SCORED {} POINTS", player.name, player.score);
            }
            //exit loop
            break;
        } else {
            //otherwise do another round
            round += 1;
        }
    }

    
    

    //return to main
    Ok(())
}

/// gets a string from user input
fn get_string_from_user_input(prompt: &str) -> Result<String, Box<dyn Error>> {
    //DATA
    let mut raw_input = String::new();

    //print prompt
    print!("{}", prompt);
    //make sure it's printed before getting input
    io::stdout().flush().unwrap();

    //read user input from standard input, and store it to raw_input, then return it or an error as needed
    raw_input.clear(); //clear input
    match io::stdin().read_line(&mut raw_input) {
        Ok(_num_bytes_read) => return Ok(String::from(raw_input.trim())),
        Err(err) => return Err(format!("ERROR: CANNOT READ INPUT!: {}", err).into()),
    }
}

/// generic function to get a number from the passed string (user input)
/// pass a min lower  than the max to have minimun and maximun bounds
/// pass a min higher than the max to only have a minumum bound
/// pass a min equal   to  the max to only have a maximun bound
/// 
/// Errors:
/// no number on user input
fn get_number_from_input<T:Display + PartialOrd + FromStr>(prompt: &str, min:T, max:T) -> Result<T, Box<dyn Error>> {
    //DATA
    let raw_input: String;
    let processed_input: String;

    
    //input looop
    raw_input = loop {
        match get_string_from_user_input(prompt) {
            Ok(input) => break input,
            Err(e) => {
                eprintln!("{}",e);
                continue;
            },
        }
    };

    //filter out num-numeric characters from user input
    processed_input = raw_input.chars().filter(|c| c.is_numeric()).collect();

    //from input, try to read a number
    match processed_input.trim().parse() {
        Ok(i) => {
            //what bounds must the input fall into
            if min < max {  //have a min and max bound: [min,max]
                if i >= min && i <= max {//is input valid, within bounds
                    return Ok(i); //exit the loop with the value i, returning it
                } else { //print error message specific to this case
                    return Err(format!("ONLY BETWEEN {} AND {}, PLEASE!", min, max).into());
                } 
            } else if min > max { //only a min bound: [min, infinity)
                if i >= min {
                    return Ok(i);
                } else {
                    return Err(format!("NO LESS THAN {}, PLEASE!", min).into());
                }
            } else { //only a max bound: (-infinity, max]
                if i <= max {
                    return Ok(i);
                } else {
                    return Err(format!("NO MORE THAN {}, PLEASE!", max).into());
                }
            }
        },
        Err(_e) => return Err(format!("Error: couldn't find a valid number in {}",raw_input).into()),
    }
}
