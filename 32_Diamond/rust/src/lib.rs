/*
 lib.rs contains all the logic of the program
*/

use std::{error::Error, fmt::Display, str::FromStr, io::{self, Write}};

const LINE_WIDTH:usize = 60;
const EDGE: &str = "C";
const EDGE_WIDTH: usize = 2;
const FILL: &str = "!";

/// handles setup for the game
pub struct Config {
    diamond_size: usize,
}
impl Config {
    /// creates and returns a new Config from user input
    pub fn new() -> Result<Config, Box<dyn Error>> {
        //DATA
        let mut config: Config = Config { diamond_size: 0 };

        //get data from user input

        //get num players
        println!("FOR A PRETTY DIAMOND PATTERN,");
        //input looop
        config.diamond_size = loop {
            match get_number_from_input("TYPE IN AN ODD NUMBER BETWEEN 5 AND 21 ", 5, 21) {
                Ok(num) => break num,
                Err(e) => {
                    eprintln!("{}",e);
                    continue;
                },
            }
        };

        //return new config
        return Ok(config);
    }
}

/// run the program
pub fn run(config: &Config) -> Result<(), Box<dyn Error>> {
    //DATA
    let diamonds = LINE_WIDTH / config.diamond_size;
    let mut line: String;

    //print out diamond
    for i in 1..diamonds {
        let mut x = 1;
        let mut y = config.diamond_size.clone();
        for n in x..y {
            line = String::new();

            //add (diamond size - n) / 2 spaces
            line += &n_spaces( (config.diamond_size - n) / 2);

            for m in 1..diamonds {
                let mut c = 1;

                for a in 1..n {
                    if c > EDGE_WIDTH {
                        line += FILL;
                    } else {
                        line += EDGE;
                        c = c + 1;
                    }
                }

                //53
            }




            //print line
            println!("{}", line);
        }
    }


    //return to main
    Ok(())
}

/// returns n spaces in a string
fn n_spaces(n:usize) -> String {
    let mut output = String::new();

    for _i in 0..n {
        output += " ";
    }

    output
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