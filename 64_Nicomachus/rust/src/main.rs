use std::io;

fn main() {
    println!("\n\n~~Nicomachus~~");
    println!("Creative Computing Morristown, New Jersey\n");

    println!("Boomerang Puzzle from Arithmetica of Nicomachus -- A.D. 90!\n");

    loop {
        println!("Please think of a number between 1 and 100.\n");

        let a = question("3");
        let b = question("5");
        let c = question("7");

        println!("\nLet me think a moment...");
        std::thread::sleep(std::time::Duration::from_secs(2));

        let d: i32 = (70 * a + 21 * b + 15 * c) % 105;

        if prompt(format!("Your number was {}, right?", d)) {
            println!("\nHow about that!!");
        } else {
            println!("\nI feel your arithmetic is in error.");
        }

        if !prompt("\nTry another?".to_string()) {
            break;
        }
    }
}

fn question(n: &str) -> i32 {
    loop {
        println!("Your number divided by {} has a remainder of?", n);

        let input = read_line().trim().parse::<i32>();

        match input {
            Ok(r) => return r,
            Err(_) => println!("Input must be a number."),
        }
    }
}

fn prompt(msg: String) -> bool {
    println!("{}", msg);

    loop {
        let input = read_line().trim().to_uppercase();
        let input = input.as_str();

        if input == "Y" || input == "YES" {
            return true;
        } else if input == "N" || input == "NO" {
            return false;
        } else {
            println!("Please input either (Y)es or (N)o.")
        }
    }
}

fn read_line() -> String {
    let mut input = String::new();

    io::stdin()
        .read_line(&mut input)
        .expect("Failed to read line.");

    input
}
