/*
 lib.rs contains all the logic of the program
*/

use std::{error::Error, fmt::Display, str::FromStr, io};

/// handles setup for the game
pub struct Config {
    player_names: Vec<String>,
}
impl Config {
    /// creates and returns a new Config from user input
    pub fn new() -> Result<Config, Box<dyn Error>> {
        //DATA
        let mut config: Config = Config { player_names: Vec::new() };
        let num_players: usize;

        //get data from user input

        //get num players
        //input looop
        num_players = loop {
            match get_number_from_input("HOW MANY PLAYERS? ") {
                Ok(num) => break num,
                Err(e) => {
                    println!("{}",e);
                    continue;
                },
            }
        };

        //get names of all players
        for id in 1..=num_players



        //return new config
        return Ok(config);
    }
}

/// run the program
pub fn run(config: Config) -> Result<(), Box<dyn Error>> {


    //return to main
    Ok(())
}

/// gets a string from user input
fn get_string_from_user_input(prompt: &str) -> Result<String, Box<dyn Error>> {
    //DATA
    let mut raw_input = String::new();

    //print prompt
    print!("{} ", prompt);

    //read user input from standard input, and store it to raw_input, then return it or an error as needed
    raw_input.clear(); //clear input
    match io::stdin().read_line(&mut raw_input) {
        Ok(_num_bytes_read) => return Ok(raw_input),
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
                } else {return Err(format!("ONLY BETWEEN {} AND {}, PLEASE!", min, max)).into();} //print error message specific to this case
            } else if min > max { //only a min bound: [min, infinity)
                if i >= min {return Ok(i);} else {return Err(format!("NO LESS THAN {}, PLEASE!", min)).into();}
            } else { //only a max bound: (-infinity, max]
                if i <= max {return Ok(i);} else {return Err(format!("NO MORE THAN {}, PLEASE!", max)).into();}
            }
            return Err(format!("Error: input out of valid range", max)).into();
            
        },
        Err(_e) => return Err(format!("Error: couldn't find a valid number in {}",raw_input).into()),
    }
}
