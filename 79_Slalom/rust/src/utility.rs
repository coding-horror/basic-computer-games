use std::ops::RangeInclusive;

pub fn get_random_number(n1: u8, n2: u8) -> u8 {
    (rand::random::<f32>() * (n1 - n2) as f32 + n2 as f32) as u8
}

pub fn random_float() -> f32 {
    rand::random()
}

pub fn prompt_int_range(range: RangeInclusive<u8>, msg: &str) -> u8 {
    loop {
        if let Ok(number) = get_input(msg).parse::<u8>() {
            if range.contains(&number) {
                return number;
            } else {
                println!(
                    "Enter a number between {} and {}",
                    range.start(),
                    range.end()
                );
            }
        }
    }
}

pub fn prompt_command(gates: u8) {
    println!("TYPE 'INS' FOR INSTRUCTIONS");
    println!("TYPE 'MAX' FOR APPROXIMATE MAXIMUM SPEEDS");
    println!("TYPE 'RUN' FOR THE BEGINNING OF THE RACE");
    loop {
        match get_input("COMMAND--?").to_lowercase().as_str() {
            "ins" => print_instructions(),
            "max" => print_max_speeds(gates),
            "run" => break,
            _ => println!("Invalid command!"),
        }
    }
}

pub fn prompt_again() -> bool {
    if get_input("DO YOU WANT TO RACE AGAIN").to_lowercase() == "yes".to_string() {
        true
    } else {
        false
    }
}

fn get_input(msg: &str) -> String {
    println!("{msg}");
    let mut input = String::new();
    std::io::stdin()
        .read_line(&mut input)
        .expect("Error reading line!");
    input.trim().to_string()
}

fn print_instructions() {
    println!("***SLALOM: THIS IS THE 1976 WINTER OLYMPIC GIANT SLALOM. YOU ARE");
    println!("\t\tTHE AMERICAN TEAM'S ONLY HOPE OF A GOLD MEDAL.\n");
    println!("\t0--TYPE THIS IF YOU WANT TO HOW LONG YOU'VE TAKEN");
    println!("\t1--TYPE THIS IF YOU WANT TO SPEED UP A LOT");
    println!("\t2--TYPE THIS IF YOU WANT TO SPEED UP A LITTLE");
    println!("\t3--TYPE THIS IF YOU WANT TO SPEED UP A TEENSY");
    println!("\t4--TYPE THIS IF YOU WANT TO KEEP GOING THE SAME SPEED");
    println!("\t5--TYPE THIS IF YOU WANT TO CHECK A TEENSY");
    println!("\t6--TYPE THIS IF YOU WANT TO CHECK A LITTLE");
    println!("\t7--TYPE THIS IF YOU WANT TO CHECK A LOT");
    println!("\t8--TYPE THIS IF YOU WANT TO CHEAT AND TRY TO SKIP A GATE");
    println!("\nTHE PLACE TO USE THESE OPTIONS IS WHEN THE COMPUTER ASKS:\n");
    println!("OPTION?");
    println!("\n\t\tGOOD LUCK,\n");
}

fn print_max_speeds(gates: u8) {
    println!();
    println!("GATE      MAX");
    println!(" #      M.P.H");
    println!("-------------");

    for i in 0..gates {
        println!("{}        {}", i + 1, MAX_SPEEDS[i as usize]);
    }
}

pub static MAX_SPEEDS: [u8; 25] = [
    14, 18, 26, 29, 18, 25, 28, 32, 29, 20, 29, 29, 25, 21, 26, 29, 20, 21, 20, 18, 26, 25, 33, 31,
    22,
];
