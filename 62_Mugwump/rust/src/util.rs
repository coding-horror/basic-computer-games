use std::io;

pub fn prompt(msg: &str) -> String {
    println!("\n{}", msg);

    let mut input = String::new();

    io::stdin()
        .read_line(&mut input)
        .expect("Failed to read line.");

    input.trim().to_string()
}
