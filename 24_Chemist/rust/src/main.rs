use std::process;//allows for some better error handling

mod lib; //allows access to lib.rs
use lib::Config;

/// main function
/// responsibilities:
/// - Calling the command line logic with the argument values
/// - Setting up any other configuration
/// - Calling a run function in lib.rs
/// - Handling the error if run returns an error
fn main() {
    //greet user
    welcome();

    // set up other configuration
    let mut config = Config::new().unwrap_or_else(|err| {
        eprintln!("Problem configuring program: {}", err);
        process::exit(1);
    });

    // run the program
    if let Err(e) = lib::run(&mut config) {
        eprintln!("Application Error: {}", e); //use the eprintln! macro to output to standard error
        process::exit(1); //exit the program with an error code
    }

    //end of program
    println!("THANKS FOR PLAYING!");
}

/// print the welcome message
fn welcome() {
    println!("
                                Chemist
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY


The fictitious chemical kryptocyanic acid can only be
diluted by the ratio of 7 parts water to 3 parts acid.
If any other ratio is attempted, the acid becomes unstable
and soon explodes.  Given the amount of acid, you must
decide how much water to add for dilution.  If you miss
you face the consequences.
    ");
}
