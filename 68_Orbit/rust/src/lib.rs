/*
 lib.rs contains all the logic of the program
*/

use std::{error::Error, fmt::Display, str::FromStr, io::{self, Write}, f64::consts::PI};

use rand::{thread_rng, Rng};

/// handles setup for the game
pub struct Config {
}
impl Config {
    /// creates and returns a new Config from user input
    pub fn new() -> Result<Config, Box<dyn Error>> {
        //DATA
        let config: Config = Config {};

        //this game doesn't actually need a config

        //return new config
        return Ok(config);
    }
}

/// run the program
pub fn run(_config: &Config) -> Result<(), Box<dyn Error>> {
    
    //play game as long as user wants to play it
    while play_game() {}
    
    //return to main
    Ok(())
}

/// handles playing the game one time
/// after playing, asks the user if they want to play again and ...
/// returns true if user wants to play again, false otherwise
fn play_game() -> bool {
    //DATA
    let mut rng = thread_rng();
    let mut ship_destroyed:bool = false;
    //generate position of the ship
    let mut ship:Polar =  Polar::new(rng.gen_range(0.0..2.0*PI), rng.gen_range(100.0_f64..300.0_f64).floor());
    let ship_speed:f64 = rng.gen_range((PI/36.0)..(PI/12.0)); //10deg - 30deg, but radians

    //game
    // round loop
    for hour in 0..7 {
        //DATA
        let photon_bomb:Polar;
        let two_pi = 2.0*PI;
        let distance;
        
        //tell user the hour
        println!("THIS IS HOUR {},", hour);

        //get input for angle and distance to send the bomb
        photon_bomb = get_bomb_from_user_input();

        //move ship
        ship.angle += ship_speed;
        if ship.angle > two_pi { ship.angle -= two_pi}

        //calculate distance of bomb from ship
        distance = photon_bomb.distance_from(&ship);
        //tell user how far away they were from the target
        println!("YOUR PHOTON BOMB EXPLODED {}*10^2 MILES FROM THE ROMULAN SHIP\n\n", distance);

        //did the bomb destroy the ship?
        ship_destroyed = distance <= 50.0;
        if ship_destroyed {break;}
        //otherwise user didn't destroy the ship, run loop again
    }
    
    //print results message depending on whether or not the user destroyed the ship in time
    if ship_destroyed {
        println!("YOU HAVE SUCCESFULLY COMPLETED YOUR MISSION.");
    } else {
        println!("YOU HAVE ALLOWED THE ROMULANS TO ESCAPE.")
    }

    //prompt user to play again
    //do they want to play yes? if they do return true, otherwise false
    return match get_string_from_user_input("ANOTHER ROMULAN SHIP HAS GONE INTO ORBIT.\nDO YOU WISH TO TRY TO DESTROY IT?\n(y/n) ") {
        Ok(string) => string.chars().any(|c| c.eq_ignore_ascii_case(&'y')),
        Err(_e) => false,
    };
}

/// structure to handle polar coordinates
struct Polar {
    angle: f64,
    radius: f64,
}
impl Polar {
    /// create new polar cordinate with the given angle (in degrees) and radiurd from the origin
    fn new(angle:f64, radius:f64) -> Polar {
        //if radius is negative, correct it
        if radius < 0.0 {
            return Polar{ angle: angle + PI/2.0, radius: -radius };
        }
        Polar { angle, radius}
    }
    ///create new polar cordinate with the given angle (in degrees, converted to radians) and radius from the origin
    fn new_from_degrees(angle:usize, radius:f64) -> Polar {
        let angle_to_rads: f64 = f64::from( (angle % 360) as u16) * PI/180.0;
        
        Polar::new(angle_to_rads, radius)
    }
    ///calculates the distance between 2 coordinates using the law of cosines
    fn distance_from(&self,other:&Polar) -> f64 {
        (other.radius*other.radius + self.radius*self.radius - 2.0*other.radius*self.radius*((other.angle-self.angle).abs().cos())).sqrt().floor()
    }
}

/// gets a Polar with bomb angle and detonation range from user
fn get_bomb_from_user_input() -> Polar {
    return loop {//input loop
        //DATA
        let bomb_angle:usize;
        let bomb_altitude:f64;
        //get angle
        match get_number_from_input("AT WHAT ANGLE DO YOU WISH TO SEND YOUR PHOTON BOMB (0-360): ", 0_usize, 360_usize) {
            Ok(angle) => bomb_angle = angle,
            Err(err) => {
                println!("{}",err);
                continue;
            },
        }
        //get radius
        match get_number_from_input("HOW FAR OUT DO YOU WISH TO DETONATE IT (100.0-300.0): ", 100.0, 300.0) {
            Ok(radius) => bomb_altitude = radius,
            Err(err) => {
                println!("{}",err);
                continue;
            },
        }
        println!("");
        //create coordinates from input
        break Polar::new_from_degrees( bomb_angle, bomb_altitude);
    };
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
    processed_input = raw_input.chars().filter(|c| c.is_numeric() || c.eq_ignore_ascii_case(&'.')).collect();

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