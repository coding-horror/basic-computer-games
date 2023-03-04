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

pub fn prompt_yes_no(prompt_text: &str) -> bool {
    loop {
        let response = prompt(&format!("{prompt_text} (Y/N)"));
        if response.len() == 0 {
            continue;
        }
        let first_word = response[0].to_uppercase();
        if first_word.starts_with("Y") {
            return true;
        }
        if first_word.starts_with("N") {
            return false;
        }
    }
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
    let mut res: Option<T> = None;
    if params.len() > param_pos {
        match params[param_pos].parse::<T>() {
            Ok(n) if (n >= min && n <= max) => res = Some(n),
            _ => ()
        }
    }
    if res.is_some() {
        return res;
    }
    return prompt_value::<T>(prompt_text, min, max);    
}