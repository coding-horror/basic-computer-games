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
    
    let mut speed_train_1;
    let mut time_difference;
    let mut speed_train_2;

    let mut guess;
    let mut answer;
    
    let mut error:f32; 

    //Game loop
    loop {
        //initialize variables
        speed_train_1 =  rng.gen_range(40..65);
        time_difference = rng.gen_range(5..20); 
        speed_train_2 = rng.gen_range(20..39);

        //print starting message / conditions
        println!("A CAR TRAVELING {} MPH CAN MAKE A CERTAIN TRIP IN\n{} HOURS LESS THAN A TRAIN TRAVELING AT {} MPH",speed_train_1,time_difference,speed_train_2);
        println!();

        //get guess
        guess = loop {
            match get_number_from_input("HOW LONG DOES THE TRIP TAKE BY CAR?",0,-1) {
                Ok(num) => break num,
                Err(err) => {
                    eprintln!("{}",err);
                    continue;
                },
            }
        };

        //calculate answer and error
        answer = time_difference * speed_train_2 / (speed_train_1 - speed_train_2);
        error = ((answer - guess) as isize).abs() as f32 * 100.0/(guess as f32) + 0.5;

        //check guess against answer
        if error > 5.0 {
            println!("SORRY, YOU WERE OFF BY {} PERCENT.", error);
            println!("CORRECT ANSWER IS {} HOURS.",answer);
        } else {
            println!("GOOD! ANSWER WITHIN {} PERCENT.", error);
        }

        //ask user if they want to go again
        match get_string_from_user_input("ANOTHER PROBLEM (Y/N)") {
            Ok(s) => if !s.to_uppercase().eq("Y") {break;} else {continue;},
            _ => break,
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
