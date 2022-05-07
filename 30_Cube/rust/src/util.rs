pub fn get_random_position() -> crate::game::Position {
    (get_random_axis(), get_random_axis(), get_random_axis())
}

fn get_random_axis() -> u8 {
    rand::Rng::gen_range(&mut rand::thread_rng(), 1..=3)
}

pub fn prompt(yes_no: bool, msg: &str) -> (u8, bool) {
    loop {
        let mut input = String::new();
        std::io::stdin()
            .read_line(&mut input)
            .expect("~~Failed reading line!~~");
        let input = input.trim();

        if yes_no {
            if let Ok(n) = input.parse::<u8>(){
                
            }
        }
    }
}
