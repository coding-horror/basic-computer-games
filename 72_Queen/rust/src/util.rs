use std::io;

pub enum PromptResult {
    Normal(String),
    YesNo(bool),
    Numeric(i32),
}

pub fn prompt(is_numeric: Option<bool>, msg: &str) -> PromptResult {
    use PromptResult::*;

    println!("{msg}");

    loop {
        let mut input = String::new();

        io::stdin()
            .read_line(&mut input)
            .expect("**Failed to read input**");

        if let Some(is_numeric) = is_numeric {
            let input = input.trim();

            if is_numeric {
                if let Ok(n) = input.parse::<i32>() {
                    return Numeric(n);
                }
                println!("PLEASE ENTER A VALID NUMBER!");
            } else {
                match input.to_uppercase().as_str() {
                    "YES" | "Y" => return YesNo(true),
                    "NO" | "N" => return YesNo(false),
                    _ => println!("PLEASE ENTER (Y)ES OR (N)O."),
                }
            }
        } else {
            return Normal(input);
        }
    }
}

pub fn is_move_legal(loc: u8, mov: u8) -> bool {
    let dt: i32 = mov as i32 - loc as i32;

    if dt.is_negative() {
        return false;
    }

    if (dt % 21) == 0 || (dt % 10) == 0 || (dt % 11) == 0 {
        return true;
    }

    false
}

pub fn is_legal_start(loc: u8) -> bool {
    let mut legal_spots = Vec::new();
    let start: u8 = 11;

    legal_spots.push(start);

    for i in 1..=7 {
        legal_spots.push(start + (10 * i));
    }

    for i in 1..=7 {
        legal_spots.push(start + (11 * i));
    }

    if legal_spots.contains(&loc) {
        true
    } else {
        false
    }
}

pub fn print_gameover(win: bool, forfeit: bool) {
    if win {
        println!("C O N G R A T U L A T I O N S . . .\nYOU HAVE WON--VERY WELL PLAYED.");
        println!(
            "IT LOOKS LIKE I HAVE MET MY MATCH.\nTHANKS FOR PLAYING---I CAN'T WIN ALL THE TIME.\n",
        );
    } else {
        if forfeit {
            println!("IT LOOKS LIKE I HAVE WON BY FORFEIT.\n");
        } else {
            println!("NICE TRY, BUT IT LOOKS LIKE I HAVE WON.\nTHANKS FOR PLAYING.");
        }
    }
}

pub fn intro() {
    println!("\n\n\t\tQUEEN");
    println!("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n");

    if let PromptResult::YesNo(yes) = prompt(Some(false), "DO YOU WANT INSTRUCTIONS?") {
        if yes {
            println!(
                r#"WE ARE GOING TO PLAY A GAME BASED ON ONE OF THE CHESS
MOVES.  OUR QUEEN WILL BE ABLE TO MOVE ONLY TO THE LEFT,
DOWN, OR DIAGONALLY DOWN AND TO THE LEFT.
THE OBJECT OF THE GAME IS TO PLACE THE QUEEN IN THE LOWER
LEFT HAND SQUARE BY ALTERNATING MOVES BETWEEN YOU AND THE
COMPUTER.  THE FIRST ONE TO PLACE THE QUEEN THERE WINS.
YOU GO FIRST AND PLACE THE QUEEN IN ANY ONE OF THE SQUARES
ON THE TOP ROW OR RIGHT HAND COLUMN.
THAT WILL BE YOUR FIRST MOVE.
WE ALTERNATE MOVES.
YOU MAY FORFEIT BY TYPING '0' AS YOUR MOVE.
BE SURE TO PRESS THE RETURN KEY AFTER EACH RESPONSE."#
            )
        }
    }
}
