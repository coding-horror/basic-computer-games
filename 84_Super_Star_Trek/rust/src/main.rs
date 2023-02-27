use std::io::stdin;

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
    let stdin = stdin();
    loop {
        match game_status {
            _ => {
                println!("Command?");
                let mut buffer = String::new();
                if let Ok(_) = stdin.read_line(&mut buffer) {
                    let text = buffer.trim_end();
                    if let Some(msg) = as_message(text, game_status) {
                        return msg
                    }
                    print_command_help();
                }
            }
        }
    }
}

fn as_message(text: &str, game_status: &GameStatus) -> Option<Message> {
    if text == "" {
        return None
    }
    match game_status {
        _ => {
            match text {
                "SRS" => Some(Message::RequestShortRangeScan),
                _ => None
            }
        }
    }
}

fn print_command_help() {
    println!("valid commands are just SRS at the mo")
}