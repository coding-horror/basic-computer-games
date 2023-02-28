use crate::model::{Galaxy, GameStatus};

pub enum Message {
    RequestShortRangeScan,
    RequestNavigation,
    DirectionForNav (u8),
    DirectionAndSpeedForNav (u8, u8),
}

pub fn update(message: Message, model: Galaxy) -> Galaxy {
    match message {
        Message::RequestShortRangeScan => 
            Galaxy { 
                game_status: GameStatus::ShortRangeScan, 
                ..model 
            },
        Message::RequestNavigation => {
            Galaxy { 
                game_status: GameStatus::NeedDirectionForNav, 
                ..model 
            }
        },
        Message::DirectionForNav(dir) => {
            Galaxy { 
                game_status: GameStatus::NeedSpeedForNav(dir), 
                ..model 
            }
        },
        Message::DirectionAndSpeedForNav(dir, speed) => {
            Galaxy { 
                game_status: GameStatus::ShortRangeScan, 
                ..model 
            }
        }
    }
}