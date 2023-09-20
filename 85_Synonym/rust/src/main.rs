use rand::seq::SliceRandom;
use rand::thread_rng;
use rand::Rng;
use std::io::{self, Write};

fn print_centered(text: &str, width: usize) {
    let pad_size: usize = if width > text.len() {
        (width - text.len()) / 2
    } else {
        0
    };
    println!("{}{}", " ".repeat(pad_size), text);
}

fn print_instructions() {
    println!("A SYNONYM OF A WORD MEANS ANOTHER WORD IN THE ENGLISH");
    println!("LANGUAGE WHICH HAS THE SAME OR VERY NEARLY THE SAME MEANING.");
    println!("I CHOOSE A WORD -- YOU TYPE A SYNONYM.");
    println!("IF YOU CAN'T THINK OF A SYNONYM, TYPE THE WORD 'HELP'");
    println!("AND I WILL TELL YOU A SYNONYM.\n");
}

fn ask_question(mut this_question: Vec<&str>) {
    let right_words = ["RIGHT", "CORRECT", "FINE", "GOOD!", "CHECK"];

    // use the first one in the main question
    let base_word = this_question.remove(0);

    loop {
        print!("     WHAT IS A SYNONYM OF {base_word}? ");
        io::stdout().flush().unwrap();
        let mut answer: String = String::new();
        io::stdin()
            .read_line(&mut answer)
            .expect("Failed to read the line");
        let answer = answer.trim();
        if answer == "HELP" {
            // remove one random from the answers and show it
            let random_index = thread_rng().gen_range(0..this_question.len());
            println!(
                "**** A SYNONYM OF {base_word} IS {}.",
                this_question.remove(random_index)
            );
        } else if this_question.contains(&answer) {
            println!("{}", right_words.choose(&mut rand::thread_rng()).unwrap());
            break;
        }
    }
}

fn main() {
    const PAGE_WIDTH: usize = 64;

    let mut synonyms = vec![
        vec!["FIRST", "START", "BEGINNING", "ONSET", "INITIAL"],
        vec!["SIMILAR", "ALIKE", "SAME", "LIKE", "RESEMBLING"],
        vec!["MODEL", "PATTERN", "PROTOTYPE", "STANDARD", "CRITERION"],
        vec!["SMALL", "INSIGNIFICANT", "LITTLE", "TINY", "MINUTE"],
        vec!["STOP", "HALT", "STAY", "ARREST", "CHECK", "STANDSTILL"],
        vec![
            "HOUSE",
            "DWELLING",
            "RESIDENCE",
            "DOMICILE",
            "LODGING",
            "HABITATION",
        ],
        vec!["PIT", "HOLE", "HOLLOW", "WELL", "GULF", "CHASM", "ABYSS"],
        vec!["PUSH", "SHOVE", "THRUST", "PROD", "POKE", "BUTT", "PRESS"],
        vec!["RED", "ROUGE", "SCARLET", "CRIMSON", "FLAME", "RUBY"],
        vec![
            "PAIN",
            "SUFFERING",
            "HURT",
            "MISERY",
            "DISTRESS",
            "ACHE",
            "DISCOMFORT",
        ],
    ];

    synonyms.shuffle(&mut thread_rng());

    print_centered("SYNONYM", PAGE_WIDTH);
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", PAGE_WIDTH);
    println!("\n\n\n");

    print_instructions();

    for this_question in synonyms {
        ask_question(this_question)
    }
    println!("SYNONYM DRILL COMPLETED.");
}

////////////////////////////////////////////////////////////
// Poring Notes
// Poring Note: The "HELP" function .removes a variable
// from lists and shows it. This can lead to errors when
// the list becomes empty. But since the same issue happens
// on the original BASIC program, kept it intact
////////////////////////////////////////////////////////////
