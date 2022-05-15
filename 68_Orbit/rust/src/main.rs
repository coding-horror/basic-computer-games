/*
    The responsibilities that remain in the main function after separating concerns
    should be limited to the following:
 - Setting up any configuration
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

/// prints the welcome/start message
fn welcome() {
    println!("
                                Orbit
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY



    SOMEWHERE ABOVE YOUR PLANET IS A ROMULAN SHIP.

    THE SHIP IS IN A CONSTANT POLAR ORBIT.  ITS
    DISTANCE FROM THE CENTER OF YOUR PLANET IS FROM
    10,000 TO 30,000 MILES AND AT ITS PRESENT VELOCITY CAN
    CIRCLE YOUR PLANET ONCE EVERY 12 TO 36 HOURS.

    UNFORTUNATELY, THEY ARE USING A CLOAKING DEVICE SO
    YOU ARE UNABLE TO SEE THEM, BUT WITH A SPECIAL
    INSTRUMENT YOU CAN TELL HOW NEAR THEIR SHIP YOUR
    PHOTON BOMB EXPLODED.  YOU HAVE SEVEN HOURS UNTIL THEY
    HAVE BUILT UP SUFFICIENT POWER IN ORDER TO ESCAPE
    YOUR PLANET'S GRAVITY.

    YOUR PLANET HAS ENOUGH POWER TO FIRE ONE BOMB AN HOUR.

    AT THE BEGINNING OF EACH HOUR YOU WILL BE ASKED TO GIVE AN
    ANGLE (BETWEEN 0 AND 360) AND A DISTANCE IN UNITS OF
    100 MILES (BETWEEN 100 AND 300), AFTER WHICH YOUR BOMB'S
    DISTANCE FROM THE ENEMY SHIP WILL BE GIVEN.

    AN EXPLOSION WITHIN 5,000 MILES OF THE ROMULAN SHIP
    WILL DESTROY IT.

    BELOW IS A DIAGRAM TO HELP YOU VISUALIZE YOUR PLIGHT.


                              90
                        0000000000000
                     0000000000000000000
                   000000           000000
                 00000                 00000
                00000    XXXXXXXXXXX    00000
               00000    XXXXXXXXXXXXX    00000
              0000     XXXXXXXXXXXXXXX     0000
             0000     XXXXXXXXXXXXXXXXX     0000
            0000     XXXXXXXXXXXXXXXXXXX     0000
    180<== 00000     XXXXXXXXXXXXXXXXXXX     00000 ==>0
            0000     XXXXXXXXXXXXXXXXXXX     0000
             0000     XXXXXXXXXXXXXXXXX     0000
              0000     XXXXXXXXXXXXXXX     0000
               00000    XXXXXXXXXXXXX    00000
                00000    XXXXXXXXXXX    00000
                 00000                 00000
                   000000           000000
                     0000000000000000000
                        0000000000000
                             270

    X - YOUR PLANET
    O - THE ORBIT OF THE ROMULAN SHIP

    ON THE ABOVE DIAGRAM, THE ROMULAN SHIP IS CIRCLING
    COUNTERCLOCKWISE AROUND YOUR PLANET.  DON'T FORGET THAT
    WITHOUT SUFFICIENT POWER THE ROMULAN SHIP'S ALTITUDE
    AND ORBITAL RATE WILL REMAIN CONSTANT.

    GOOD LUCK.  THE FEDERATION IS COUNTING ON YOU.
    ");
}

