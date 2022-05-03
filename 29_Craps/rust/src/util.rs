use std::io;

pub enum Response {
    Yes,
    No,
}

pub fn read_line() -> String {
    let mut input = String::new();

    io::stdin()
        .read_line(&mut input)
        .expect("Error reading line.");

    input
}

pub fn read_numeric(message: &str) -> usize {
    loop {
        println!("{}", message);

        let mut ok = true;

        let input = read_line();

        for c in input.trim().chars() {
            if !c.is_numeric() {
                println!("You can only enter a number!");
                ok = false;
                break;
            }
        }

        if ok {
            let input = input.trim().parse();

            let _ = match input {
                Ok(i) => return i,
                Err(e) => {
                    println!("please input a number ({})", e);
                }
            };
        }
    }
}

pub fn prompt(msg: &str) -> Response {
    use Response::*;

    let mut _r = Response::Yes;

    loop {
        println!("\n{}", msg);

        let response = read_line().trim().to_uppercase();

        match response.as_str() {
            "YES" | "Y" => {
                _r = Yes;
                break;
            }
            "NO" | "N" => {
                _r = No;
                break;
            }
            _ => println!("Please input (Y)es or (N)o."),
        };
    }

    _r
}
