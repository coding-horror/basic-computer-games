use std::io;

pub enum PromptResult {
    Normal(String),
    YesNo(bool),
    Numeric(i32),
}

pub fn prompt(is_numeric: Option<bool>, msg: &str) -> PromptResult {
    use PromptResult::*;

    println!("{msg}");

    loop {
        let mut input = String::new();

        io::stdin()
            .read_line(&mut input)
            .expect("Failed to read input.");

        if let Some(is_numeric) = is_numeric {
            let input = input.trim();

            if is_numeric {
                if let Ok(n) = input.parse::<i32>() {
                    return Numeric(n);
                }
                println!("PLEASE ENTER A VALID NUMBER!");
            } else {
                match input.to_uppercase().as_str() {
                    "YES" | "Y" => return YesNo(true),
                    "NO" | "N" => return YesNo(false),
                    _ => println!("PLEASE ENTER (Y)ES OR (N)O."),
                }
            }
        } else {
            return Normal(input);
        }
    }
}
