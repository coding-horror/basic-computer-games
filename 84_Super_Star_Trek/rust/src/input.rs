use std::{io::{stdin, stdout, Write}, str::FromStr};

pub fn prompt(prompt_text: &str) -> Vec<String> {
    let stdin = stdin();
    let mut stdout = stdout();

    print!("{prompt_text} ");
    let _ = stdout.flush();

    let mut buffer = String::new();
    if let Ok(_) = stdin.read_line(&mut buffer) {
        return buffer.trim_end().split(" ").map(|s| s.to_string()).collect();
    }
    Vec::new()
}

pub fn prompt_value<T: FromStr + PartialOrd>(prompt_text: &str, min: T, max: T) -> Option<T> {
    let passed = prompt(prompt_text);
    if passed.len() != 1 {
        return None
    }
    match passed[0].parse::<T>() {
        Ok(n) if (n >= min && n <= max) => Some(n),
        _ => None
    }
}

pub fn param_or_prompt_value<T: FromStr + PartialOrd>(params: &Vec<String>, param_pos: usize, prompt_text: &str, min: T, max: T) -> Option<T> {
    if params.len() > param_pos {
        match params[param_pos].parse::<T>() {
            Ok(n) => Some(n),
            _ => None
        }
    } else {
        return prompt_value::<T>(prompt_text, min, max);
    }
}