use std::io;

pub fn prompt(msg: &str) -> String {
    println!("\n{}", msg);

    let mut input = String::new();

    io::stdin()
        .read_line(&mut input)
        .expect("Failed to read line.");

    input.trim().to_string()
}

pub fn prompt_bool(msg: &str) -> Option<bool> {
    loop {
        let response = prompt(msg);

        match response.to_uppercase().as_str() {
            "Y" | "YES" => return Some(true),
            "N" | "NO" => return Some(false),
            _ => println!("PLEASE ENTER (Y)ES or (N)O."),
        }
    }
}
