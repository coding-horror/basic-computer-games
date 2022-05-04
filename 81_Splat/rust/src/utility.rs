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

pub fn prompt_bool(msg: &str, template: bool) -> bool {
    if template {
        println!("{} (YES OR NO)?", msg);
    } else {
        println!("{}", msg);
    }

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

pub fn get_altitude() -> f32 {
    9001. * rand::random::<f32>() + 1000.
}

pub fn get_terminal_velocity(bool_msg: &str, num_msg: &str) -> f32 {
    let mut _num = 0.0;

    if prompt_bool(bool_msg, true) {
        _num = prompt_numeric(num_msg);
    } else {
        _num = get_random_float(0., 1000.);
        println!("OK. TERMINAL VELOCTY = {} MI/HR", _num);
    }

    (_num * ((5280 / 3600) as f32)) * get_random_float(0.95, 1.05)
}

pub fn get_acceleration(bool_msg: &str, num_msg: &str) -> f32 {
    let mut _num = 0.0;

    if prompt_bool(bool_msg, true) {
        _num = prompt_numeric(num_msg);
    } else {
        let b =
            celestial_body::random_celestial_body().expect("Fatal Error: Invalid Celestial Body!");

        _num = b.get_acceleration();
        b.print_acceleration_message();
    }

    _num * get_random_float(0.95, 1.05)
}

fn get_random_float(min: f32, max: f32) -> f32 {
    rand::thread_rng().gen_range(min..=max)
}

pub fn print_splat(t: f32) {
    println!("{}\t\tSPLAT!", t);
    //get random death message
}
