/*
    The responsibilities that remain in the main function after separating concerns
    should be limited to the following:
 - Calling the command line logic with the argument values
 - Setting up any other configuration
 - Calling a run function in lib.rs
 - Handling the error if run returns an error
*/

use std::process;       //allows for some better error handling

mod lib;
use lib::Config;

/// main function
fn main() {
    //greet user
    welcome();

    // set up other configuration
    let config = Config::new(&args).unwrap_or_else(|err| {
        eprintln!("Problem parsing arguments: {}", err);
        lib::help();
        process::exit(1);
    });

    // run the program
    if let Err(e) = lib::run(config) {
        eprintln!("Application Error: {}", e); //use the eprintln! macro to output to standard error
        process::exit(1); //exit the program with an error code
    }

    //end of program
    println!("THANKS FOR PLAYING!");
}

/// prints the welcome/start message
fn welcome() {
    println!("
                               BULLSEYE
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



    IN THIS GAME, UP TO 20 PLAYERS THROW DARTS AT A TARGET
    WITH 10, 20, 30, AND 40 POINT ZONES.  THE OBJECTIVE IS
    TO GET 200 POINTS.
    
    | Throw | Description        | Probable Score            |
    |-------|--------------------|---------------------------|
    | 1     | Fast overarm       | Bullseye or complete miss |
    | 2     | Controlled overarm | 10, 20, or 30 points      |
    | 3     | Underarm           | Anything                  |

    ");
}