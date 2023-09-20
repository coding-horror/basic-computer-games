use std::io;

fn print_center(text: String, width: usize) {
    let pad_size: usize = if width > text.len() {
        (width - text.len()) / 2
    } else {
        0
    };
    println!("{}{}", " ".repeat(pad_size), text);
}

fn send_lightening() {
    println!(
        "YOU HAVE MADE ME MAD!!!
THERE MUST BE A GREAT LIGHTNING BOLT!

                              X X
                             X X
                            X X
                           X X
                          X X
                         X X
                        X X
                       X X
                      X X
                     X XXX
                    X   X
                   XX X
                    X X
                   X X
                  X X
                 X X
                X X
               X X
              X X
             X X
            XX
           X
          *

#########################

I HOPE YOU BELIEVE ME NOW, FOR YOUR SAKE!!"
    );
}

fn check_yes_answer() -> bool {
    // reads from input and return true if it starts with Y or y

    let mut answer: String = String::new();
    io::stdin()
        .read_line(&mut answer)
        .expect("Error reading from stdin");

    answer.to_uppercase().starts_with('Y')
}

fn main() {
    const PAGE_WIDTH: usize = 64;
    print_center("CHIEF".to_string(), PAGE_WIDTH);
    print_center(
        "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".to_string(),
        PAGE_WIDTH,
    );
    println!("\n\n\n");

    println!("I AM CHIEF NUMBERS FREEK, THE GREAT INDIAN MATH GOD.");
    println!("ARE YOU READY TO TAKE THE TEST YOU CALLED ME OUT FOR?");

    if !check_yes_answer() {
        println!("SHUT UP, PALE FACE WITH WISE TONGUE.");
    }

    println!("TAKE A NUMBER AND ADD 3. DIVIDE THIS NUMBER BY 5 AND");
    println!("MULTIPLY BY 8. DIVIDE BY 5 AND ADD THE SAME. SUBTRACT 1.");
    println!("  WHAT DO YOU HAVE?");

    // read a float number
    let mut answer: String = String::new();
    io::stdin()
        .read_line(&mut answer)
        .expect("Error reading from stdin");
    let guess: f32 = answer.trim().parse().expect("Input not a number");

    let calculated_answer: f32 = (guess + 1.0 - 5.0) * 5.0 / 8.0 * 5.0 - 3.0;

    println!("I BET YOUR NUMBER WAS {calculated_answer}. AM I RIGHT?");

    if check_yes_answer() {
        println!("BYE!!!");
    } else {
        println!("WHAT WAS YOUR ORIGINAL NUMBER?");

        // read a float number
        let mut answer: String = String::new();
        io::stdin()
            .read_line(&mut answer)
            .expect("Error reading from stdin");
        let claimed: f32 = answer.trim().parse().expect("Input not a number");

        println!("SO YOU THINK YOU'RE SO SMART, EH?");
        println!("NOW WATCH.");
        println!(
            "{claimed} PLUS 3 EQUALS {}. THIS DIVIDED BY 5 EQUALS {};",
            claimed + 3.0,
            (claimed + 3.0) / 5.0
        );
        println!(
            "THIS TIMES 8 EQUALS {}. IF WE DIVIDE BY 5 AND ADD 5,",
            (claimed + 3.0) / 5.0 * 8.0
        );
        println!(
            "WE GET {} , WHICH, MINUS 1, EQUALS {}.",
            ((claimed + 3.0) / 5.0 * 8.0 / 5.0) + 5.0,
            ((claimed + 3.0) / 5.0 * 8.0 / 5.0) + 4.0
        );
        println!("NOW DO YOU BELIEVE ME?");

        if check_yes_answer() {
            println!("BYE!!!");
        } else {
            send_lightening();
        }
    }
}

////////////////////////////////////////////////////////////////////
// Porting notes:
// In floating point arithmetics in "modern" languages we might see
// unfamiliar situations such as 6.9999999 instead of 7 and such.
// resolving this needs using specific mathematical libraries which
// IMO is out of scope in these basic programs
///////////////////////////////////////////////////////////////////
