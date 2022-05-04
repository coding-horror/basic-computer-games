use crate::celestial_body;
use rand::Rng;
use std::io;

pub fn read_line() -> String {
    let mut input = String::new();
    io::stdin()
        .read_line(&mut input)
        .expect("Failed to read line.");
    input.trim().to_uppercase()
}

pub fn prompt_bool(msg: &str) -> bool {
    println!(" {} (YES OR NO)?", msg);

    loop {
        let response = read_line();

        match response.as_str() {
            "YES" => return true,
            "NO" => return false,
            _ => println!("PLEASE ENTER YES OR NO."),
        }
    }
}

pub fn prompt_numeric(msg: &str) -> f32 {
    println!("{}", msg);

    loop {
        let response = read_line();

        if let Some(_) = response.chars().find(|c| !c.is_numeric()) {
            println!("PLEASE ENTER A NUMBER.");
        } else {
            return response.parse::<f32>().unwrap();
        }
    }
}

pub fn get_terminal_velocity(bool_msg: &str, num_msg: &str) -> f32 {
    let mut _num = 0.0;

    if prompt_bool(bool_msg) {
        _num = prompt_numeric(num_msg);
    } else {
        _num = rand::thread_rng().gen_range(0.0..=1000.0);
        println!("OK. TERMINAL VELOCTY = {} MI/HR", _num);
    }

    _num * ((5280 / 3600) as f32)
}

pub fn get_acceleration(bool_msg: &str, num_msg: &str) -> f32 {
    let mut _num = 0.0;

    if prompt_bool(bool_msg) {
        _num = prompt_numeric(num_msg);
    } else {
        let b =
            celestial_body::random_celestial_body().expect("Fatal Error: Invalid Celestial Body!");

        _num = b.get_acceleration();
        b.print_acceleration_message();
    }

    _num
}
