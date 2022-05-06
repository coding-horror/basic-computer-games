use std::io;

pub enum PromptResult {
    Number(i32),
    YesNo(bool),
}

pub fn prompt(numeric: bool, msg: &str) -> PromptResult {
    use PromptResult::*;
    loop {
        println!("{}", msg);

        let mut input = String::new();

        io::stdin()
            .read_line(&mut input)
            .expect("Failed to read line.");

        let input = input.trim().to_string();

        if numeric {
            if let Ok(n) = input.parse::<i32>() {
                return Number(n);
            }

            println!("PLEASE ENTER A NUMBER.")
        } else {
            match input.to_uppercase().as_str() {
                "YES" | "Y" => return YesNo(true),
                "NO" | "N" => return YesNo(false),
                _ => println!("PLEASE ENTER (Y)ES OR (N)O."),
            }
        }
    }
}

pub fn get_disk_count() -> u8 {
    loop {
        if let PromptResult::Number(n) =
            prompt(true, "HOW MANY DISKS DO YOU WANT TO MOVE (7 IS MAX)?")
        {
            if n <= 2 {
                println!("THERE MUST BE AT LEAST 3 DISKS!")
            } else if n > 7 {
                println!("THERE CAN'T BE MORE THAN 7 DISKS!")
            } else {
                return n as u8;
            }
        }
    }
}
