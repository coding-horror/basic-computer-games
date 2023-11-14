use std::collections::HashMap;
use std::io::{self, stdin};

const LETTERS: [(char, [usize; 7]); 42] = [
    (' ', [0, 0, 0, 0, 0, 0, 0]),
    ('A', [505, 37, 35, 34, 35, 37, 505]),
    ('G', [125, 131, 258, 258, 290, 163, 101]),
    ('E', [512, 274, 274, 274, 274, 258, 258]),
    ('T', [2, 2, 2, 512, 2, 2, 2]),
    ('W', [256, 257, 129, 65, 129, 257, 256]),
    ('L', [512, 257, 257, 257, 257, 257, 257]),
    ('S', [69, 139, 274, 274, 274, 163, 69]),
    ('O', [125, 131, 258, 258, 258, 131, 125]),
    ('N', [512, 7, 9, 17, 33, 193, 512]),
    ('F', [512, 18, 18, 18, 18, 2, 2]),
    ('K', [512, 17, 17, 41, 69, 131, 258]),
    ('B', [512, 274, 274, 274, 274, 274, 239]),
    ('D', [512, 258, 258, 258, 258, 131, 125]),
    ('H', [512, 17, 17, 17, 17, 17, 512]),
    ('M', [512, 7, 13, 25, 13, 7, 512]),
    ('?', [5, 3, 2, 354, 18, 11, 5]),
    ('U', [128, 129, 257, 257, 257, 129, 128]),
    ('R', [512, 18, 18, 50, 82, 146, 271]),
    ('P', [512, 18, 18, 18, 18, 18, 15]),
    ('Q', [125, 131, 258, 258, 322, 131, 381]),
    ('Y', [8, 9, 17, 481, 17, 9, 8]),
    ('V', [64, 65, 129, 257, 129, 65, 64]),
    ('X', [388, 69, 41, 17, 41, 69, 388]),
    ('Z', [386, 322, 290, 274, 266, 262, 260]),
    ('I', [258, 258, 258, 512, 258, 258, 258]),
    ('C', [125, 131, 258, 258, 258, 131, 69]),
    ('J', [65, 129, 257, 257, 257, 129, 128]),
    ('1', [0, 0, 261, 259, 512, 257, 257]),
    ('2', [261, 387, 322, 290, 274, 267, 261]),
    ('*', [69, 41, 17, 512, 17, 41, 69]),
    ('3', [66, 130, 258, 274, 266, 150, 100]),
    ('4', [33, 49, 41, 37, 35, 512, 33]),
    ('5', [160, 274, 274, 274, 274, 274, 226]),
    ('6', [194, 291, 293, 297, 305, 289, 193]),
    ('7', [258, 130, 66, 34, 18, 10, 8]),
    ('8', [69, 171, 274, 274, 274, 171, 69]),
    ('9', [263, 138, 74, 42, 26, 10, 7]),
    ('=', [41, 41, 41, 41, 41, 41, 41]),
    ('!', [1, 1, 1, 384, 1, 1, 1]),
    ('0', [57, 69, 131, 258, 131, 69, 57]),
    ('.', [1, 1, 129, 449, 129, 1, 1]),
];

fn main() {
    print_banner().ok();
}

fn read_input() -> io::Result<String> {
    let mut input = String::new();
    stdin().read_line(&mut input)?;
    Ok(input.trim().to_uppercase())
}

fn read_input_number() -> io::Result<usize> {
    loop {
        match read_input()?.parse::<usize>() {
            Ok(num) => {
                if num > 0 {
                    break Ok(num);
                } else {
                    println!("Must be greater than zero");
                }
            }
            Err(_) => println!("Please enter a number greater than zero"),
        }
    }
}

fn user_input() -> io::Result<(usize, usize, bool, String, String)> {
    println!("Horizontal");
    let horizontal = read_input_number()?;
    println!();

    println!("Vertical ");
    let vertical = read_input_number()?;
    println!();

    println!("Centered ");
    let is_entered = read_input()?.starts_with('Y');
    println!();

    println!("Character (type 'ALL' if you want character being printed) ");
    let character = read_input()?;
    println!();

    println!("Statement ");
    let statement = read_input()?;
    println!();

    // This means to prepare printer, just press Enter
    println!("Set page ");
    read_input()?;
    println!();

    Ok((horizontal, vertical, is_entered, character, statement))
}

fn print_banner() -> io::Result<()> {
    let letters = HashMap::from(LETTERS);

    let (horizontal, vertical, is_entered, character, statement) = user_input()?;

    for statement_char in statement.chars() {
        let x_str = if character == "ALL" {
            statement_char.to_string()
        } else {
            character.clone()
        };

        if x_str == " " {
            for _ in 0..(7 * horizontal) {
                println!();
            }
            continue;
        }

        let mut s = [0; 7];
        if let Some(ss) = letters.get(&statement_char) {
            s.copy_from_slice(ss);
        } else {
            println!("\nCannot print {statement_char}\n");
        }
        let mut f = [0; 7];
        let mut j = [false; 9];

        for u in 0..s.len() {
            for k in (0..=8).rev() {
                let mask = 1usize << k;
                j[8 - k] = if mask >= s[u] {
                    false
                } else {
                    s[u] -= mask;
                    true
                };
                if s[u] == 1 {
                    f[u] = 8 - k;
                    break;
                }
            }

            let offset_str = if is_entered {
                let n = (63 * 2 - vertical * 9) / 2 / x_str.len() + 1;
                " ".repeat(n)
            } else {
                "".to_string()
            };

            let mut content_str = String::new();
            for b in &j[0..=f[u]] {
                if *b {
                    content_str += &x_str.repeat(vertical);
                } else {
                    content_str += &" ".repeat(x_str.len()).repeat(vertical);
                }
            }

            for _ in 0..horizontal {
                println!("{offset_str}{content_str}");
            }
        }

        for _ in 0..(2 * horizontal - 1) {
            println!();
        }
    }

    Ok(())
}
