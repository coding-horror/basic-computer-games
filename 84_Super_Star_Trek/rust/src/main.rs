use std::io::{stdin, stdout, Write};

use model::{Galaxy, GameStatus};
use update::Message;

mod model;
mod view;
mod update;

fn main() {
    let mut galaxy = Galaxy::generate_new();
    loop {
        view::view(&galaxy);
        let command = wait_for_command(&galaxy.game_status);
        galaxy = update::update(command, galaxy)
    }
}

fn wait_for_command(game_status: &GameStatus) -> Message {
    loop {
        match game_status {
            GameStatus::NeedDirectionForNav => {
                let text = prompt("Course (1-9)?");
                if let Some(msg) = as_message(&text, game_status) {
                    return msg
                }
            },
            GameStatus::NeedSpeedForNav(_) => {
                let text = prompt("Warp Factor (0-8)?");
                if let Some(msg) = as_message(&text, game_status) {
                    return msg
                }
            },
            _ => {
                let text = prompt("Command?");
                if let Some(msg) = as_message(&text, game_status) {
                    return msg
                }
                print_command_help();
            }
        }
    }
}

fn prompt(prompt: &str) -> String {
    let stdin = stdin();
    let mut stdout = stdout();

    print!("{prompt} ");
    let _ = stdout.flush();

    let mut buffer = String::new();
    if let Ok(_) = stdin.read_line(&mut buffer) {
        return buffer.trim_end().into();
    }
    "".into()
}

fn as_message(text: &str, game_status: &GameStatus) -> Option<Message> {
    match game_status {
        GameStatus::NeedDirectionForNav => {
            match text.parse::<u8>() {
                Ok(n) if (n >= 1 && n <= 8) => Some(Message::DirectionForNav(n)),
                _ => None
            }
        },
        GameStatus::NeedSpeedForNav(dir) => {
            match text.parse::<u8>() {
                Ok(n) if (n >= 1 && n <= 8) => Some(Message::DirectionAndSpeedForNav(*dir, n)),
                _ => None
            }
        }
        _ => {
            match text {
                "SRS" => Some(Message::RequestShortRangeScan),
                "NAV" => Some(Message::RequestNavigation),
                _ => None
            }
        }
    }
}

fn print_command_help() {
    println!("valid commands are just SRS and NAV at the mo")
}