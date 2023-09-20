use rand::seq::SliceRandom;
use std::io::{self, Write};

fn main() {
    let words = vec![
        vec![
            "Ability",
            "Basal",
            "Behavioral",
            "Child-centered",
            "Differentiated",
            "Discovery",
            "Flexible",
            "Heterogeneous",
            "Homogenous",
            "Manipulative",
            "Modular",
            "Tavistock",
            "Individualized",
        ],
        vec![
            "learning",
            "evaluative",
            "objective",
            "cognitive",
            "enrichment",
            "scheduling",
            "humanistic",
            "integrated",
            "non-graded",
            "training",
            "vertical age",
            "motivational",
            "creative",
        ],
        vec![
            "grouping",
            "modification",
            "accountability",
            "process",
            "core curriculum",
            "algorithm",
            "performance",
            "reinforcement",
            "open classroom",
            "resource",
            "structure",
            "facility",
            "environment",
        ],
    ];

    // intro text
    println!("\n           Buzzword Generator");
    println!("Creative Computing  Morristown, New Jersey");
    println!("\n\n");
    println!("This program prints highly acceptable phrases in");
    println!("'educator-speak' that you can work into reports");
    println!("and speeches.  Whenever a question mark is printed,");
    println!("type a 'Y' for another phrase or 'N' to quit.");
    println!("\n\nHere's the first phrase:");

    let mut continue_running: bool = true;

    while continue_running {
        let mut first_word: bool = true;
        for section in &words {
            if !first_word {
                print!(" ");
            }
            first_word = false;
            print!("{}", section.choose(&mut rand::thread_rng()).unwrap());
        }
        print!("\n\n? ");
        io::stdout().flush().unwrap();

        let mut cont_question: String = String::new();
        io::stdin()
            .read_line(&mut cont_question)
            .expect("Failed to read the line");
        if !cont_question.to_uppercase().starts_with("Y") {
            continue_running = false;
        }
    }
    println!("Come back when you need help with another report!\n");

}


/////////////////////////////////////////////////////////////////////////
//
// Porting Notes
//
//   The original program stored all 39 words in one array, then
//   built the buzzword phrases by randomly sampling from each of the
//   three regions of the array (1-13, 14-26, and 27-39).
//
//   Here, we're storing the words for each section in separate
//   tuples.  That makes it easy to just loop through the sections
//   to stitch the phrase together, and it easily accommodates adding
//   (or removing) elements from any section.  They don't all need to
//   be the same length.
//
//   The author of this program (and founder of Creative Computing
//   magazine) first started working at DEC--Digital Equipment
//   Corporation--as a consultant helping the company market its
//   computers as educational products.  He later was editor of a DEC
//   newsletter named "EDU" that focused on using computers in an
//   educational setting.  No surprise, then, that the buzzwords in
//   this program were targeted towards educators!
//
/////////////////////////////////////////////////////////////////////////
