use crate::model::{Galaxy, GameStatus};

pub enum Message {
    RequestShortRangeScan
}

pub fn update(message: Message, model: Galaxy) -> Galaxy {
    match message {
        Message::RequestShortRangeScan => 
            Galaxy { 
                game_status: GameStatus::ShortRangeScan, 
                ..model 
            }
    }
}