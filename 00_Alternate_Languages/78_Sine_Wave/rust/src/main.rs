fn main() {
    let mut ticker:f64 = 0.0;
    let mut spaces ;
    
    //pring welcome message
    welcome();

    //drawing loop
    loop {
        //print however many spaces
        spaces = (26.0 + 25.0*ticker.sin()).round() as i32;
        for _i in 0..=spaces{
            print!(" "); //print a space
        }

        //print Creative or Computing
        if (ticker.round() as i32) % 2 == 0 {
            println!("CREATIVE");
        } else {
            println!("COMPUTING");
        } 

        //increment ticker
        ticker += 0.25;
    }

}

/**
 * prints welcome message
 */
fn welcome() {
    println!("
                             SINE WAVE
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY
    ");
}