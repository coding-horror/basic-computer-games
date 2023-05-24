/*
 lib.rs contains all the logic of the program
*/
use rand::{Rng, prelude::thread_rng}; //rng
use std::error::Error; //better errors
use std::io::{self, Write}; //io interactions
use std::{str::FromStr, fmt::Display}; //traits

//DATA

/// handles setup for the game
pub struct Config {
}
impl Config {
    /// creates and returns a new Config from user input
    pub fn new() -> Result<Config, Box<dyn Error>> {
        //DATA
        let config: Config = Config {
        };
        
        //return new config
        return Ok(config);
    }
}

/// run the program
pub fn run(_config: &Config) -> Result<(), Box<dyn Error>> {
    //DATA
    let mut rng = thread_rng();
    let mut lives: i8 = 9;

    let mut amount_of_acid:i8;

    let mut guess:f32;
    let mut answer:f32;
    
    let mut error:f32; 

    //Game loop
    loop {
        //initialize variables
        amount_of_acid =  rng.gen_range(1..50);
        answer = 7.0 * (amount_of_acid as f32/3.0);

        //print starting message / conditions
        println!();
        //get guess
        guess = loop {
            match get_number_from_input(&format!("{} Liters of Kryptocyanic acid.  How much water? ", amount_of_acid),0.0,-1.0) {
                Ok(num) => break num,
                Err(err) => {
                    eprintln!("{}",err);
                    continue;
                },
            }
        };

        //calculate error
        error = (answer as f32 - guess).abs() / guess;
        
        println!("answer: {} | error: {}%", answer,error*100.);

        //check guess against answer
        if error > 0.05 { //error > 5%
            println!(" Sizzle!  You may have just been desalinated into a blob");
            println!(" of quivering protoplasm!");
            //update lives
            lives -= 1;

            if lives <= 0 {
                println!(" Your 9 lives are used, but you will be long remembered for");
                println!(" your contributions to the field of comic book chemistry.");
                break;
            }
            else {
                println!(" However, you may try again with another life.")
            } 
        } else {
            println!(" Good job!  You may breathe now, but don't inhale the fumes!");
            println!();
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
    io::stdout().flush().expect("couldn't flush stdout");

    //read user input from standard input, and store it to raw_input, then return it or an error as needed
    raw_input.clear(); //clear input
    match io::stdin().read_line(&mut raw_input) {
        Ok(_num_bytes_read) => return Ok(String::from(raw_input.trim())),
        Err(err) => return Err(format!("ERROR: CANNOT READ INPUT!: {}", err).into()),
    }
}
/// generic function to get a number from the passed string (user input)
/// pass a min lower  than the max to have minimum and maximum bounds
/// pass a min higher than the max to only have a minimum bound
/// pass a min equal   to  the max to only have a maximum bound
/// 
/// Errors:
/// no number on user input
fn get_number_from_input<T:Display + PartialOrd + FromStr>(prompt: &str, min:T, max:T) -> Result<T, Box<dyn Error>> {
    //DATA
    let raw_input: String;
    let processed_input: String;

    
    //input loop
    raw_input = loop {
        match get_string_from_user_input(prompt) {
            Ok(input) => break input,
            Err(e) => {
                eprintln!("{}",e);
                continue;
            },
        }
    };

    //filter out non-numeric characters from user input
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
