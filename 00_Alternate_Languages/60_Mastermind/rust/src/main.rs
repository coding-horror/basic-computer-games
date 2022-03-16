use rand::{Rng, prelude::{thread_rng, ThreadRng}};
use std::{io, fmt::Display, str::FromStr};

//DATA
const COLORS: [&str;8] = ["Black ", "White ","Red ","Green ","Orange ","Yellow ", "Purple ", "Tan "]; //all available colors
const LETTERS: &str = "BWRGOYPT"; //letters representing the above colors

struct CODE {
    code: Vec<usize>, //maybe use a char array later, idk

}
impl CODE {
    /**
     * create generic, empty code
     */
    fn new() -> CODE {
        return CODE{code: Vec::new()};
    }
    /**
     * generates and returns a random CODE with the given parameters
     */
    fn new_random(rng: &mut ThreadRng, num_colors: usize, num_positions: usize) -> CODE {
        //data
        let mut code = CODE{code: Vec::new()};
        //generate random combination of colors
        for _i in 0..num_positions {
            code.code.push(rng.gen_range(0..num_colors));
        }
        return code;
    }
    /**
     * converts input_int from base 10 to base num_colors to generate the code
     * input_int must be between 0 and num_colors.pow(num_positions)
     */
    fn new_from_int(mut input_int: usize, num_colors: usize, num_positions: usize) -> CODE {
        //DATA
        let mut converted_number:Vec<_> = Vec::new();
        assert!(2 <= num_colors && num_colors <= 36); //if num_colors is outside of this range, things break later on

        //convert input_int into a code by effectively converting input_int from base 10 to base n where n is num_colors, uses some fancy stuff to do this
        loop {
            converted_number.push(std::char::from_digit((input_int % num_colors) as u32, num_colors as u32).unwrap()); //
            input_int /= num_colors;
            if input_int == 0 {break}
        }

        while converted_number.len() < num_positions {converted_number.push('0');} // fill remaining space with zero's
        let converted_number: Vec<_> = converted_number.iter().rev().map(|e| e.to_digit(num_colors as u32).unwrap() as usize).collect(); //reverse the vector and convert it to integers
        return CODE{code: converted_number};
    }
    /**
     * returns a code parsed from the passed string
     */
    fn new_from_string(input_string: String, num_colors: usize) -> Option<CODE> {
        let valid_chars = &LETTERS[0..num_colors];
        //DATA
        let new_code = CODE{
            code:
            input_string.to_ascii_uppercase().chars() //get an iterator with all the chars in input string converted to uppercase
            .filter( |c| { valid_chars.contains(*c)}) //remove chars that aren't in LETTERS
            .map( |x| -> usize {valid_chars.find(x).expect("invalid character")})//convert all the chars into usizes representing their index in LETTERS
            .collect() //wrap this iterator up into a vector
        };
        //if code is empty, return None, otherwise return Some(code)
        if new_code.code.is_empty() {return None;}
        else {return Some(new_code);}
    }

    /**
     * returns a string containing the code represented as characters
     */
    fn _as_human_readible_chars(&self) -> String {
        return self.code.iter().map(|i|->char{LETTERS.chars().nth(*i).expect("index out of bounds")}).collect();
    }
    /**
     * returns a string containing the code represented as words
     */
    fn _as_human_readible_words(&self) -> String {
        return self.code.iter().map(|i|->&str{COLORS.iter().nth(*i).expect("index out of bounds")}).collect();
    }
}
struct GUESS {
    code: CODE,
    blacks: usize,
    whites: usize,
}
impl GUESS {
    /**
     * create a new guess, and evaluate it
     */
    fn new(code: CODE) -> GUESS {
        return GUESS{code:code, blacks:0,whites:0 };
    }

    /**
     * evaulates itself for the number of black and white pegs it should have for a given answer
     */
    fn evaluate(&mut self, answer:&CODE) {
        //data
        let mut consumed = vec![false;answer.code.len()];

        if self.code.code.len() != answer.code.len() {
            panic!("only codes of the same length can be compared");
        }

        for i in 0..answer.code.len() {
            if self.code.code[i] == answer.code[i] { //correct value correct place
                self.blacks += 1;
                consumed[i] = true;
            }
            else {
                //check for correct value incorrect place, don't count positions that are already exact matches
                for j in 0..answer.code.len() {
                    if !consumed[j] && self.code.code[i] == answer.code[j] && self.code.code[j] != answer.code[j] {
                        self.whites += 1;
                        consumed[j] = true;
                        break;
                    }
                }
            }
        }
    }
}

fn main() {
    //DATA
    let mut rng = thread_rng();
    let num_colors: usize;
    let num_positions: usize;
    let num_rounds: usize;
    let num_guesses: usize;
    let total_posibilities: usize;

    let mut human_score: usize = 0;
    let mut computer_score: usize = 0;

    //print welcome message
    welcome();

    //ask user for a number of colors, positions, and rounds
    num_colors = get_number_from_user_input("NUMBER OF COLORS", "", 1, COLORS.len());
    num_positions = get_number_from_user_input("NUMBER OF POSITIONS", "", 2, 10);
    num_rounds = get_number_from_user_input("NUMBER OF ROUNDS", "", 1, 10);
    num_guesses = get_number_from_user_input("NUMBER OF GUESSES", "", 10, 0);


    //print number of posibilities
    total_posibilities = num_colors.pow(num_positions as u32);
    println!("\nTOTAL POSSIBILITIES = {}\n", total_posibilities);

    //print color letter table
    print_color_letter_table(num_colors);

    //game loop
    for round_num in 1..=num_rounds {
        //data
        let mut num_moves: usize = 1;
        let mut answer: CODE;
        let mut guess: GUESS;
        let mut guesses: Vec<GUESS> = Vec::new();
        let mut all_possibilities = vec![true; total_posibilities];


        //print round number
        println!("\n\nROUND NUMBER: {}", round_num);

        //human player is code-breaker, computer is code-maker
        //generate a combination
        answer = CODE::new_random(&mut rng, num_colors, num_positions);
        //println!("CODE: {:?}", answer._as_human_readible_chars()); //this is for troubleshooting, prints the code converted back into characters

        //round loop
        loop {
            //loop condition
            if num_moves > num_guesses {
                println!("YOU RAN OUT OF MOVES!  THAT'S ALL YOU GET!");
                println!("THE ACTUAL COMBINATION WAS: {}", answer._as_human_readible_chars());
                human_score += num_moves;
                print_scores(human_score,computer_score);
                break;
            }

            //input loop
            guess = GUESS::new(loop {
                println!("\nMOVE # {} GUESS: ", num_moves);

                //get player move
                let mut raw_input = String::new(); //temp variable to store user input
                io::stdin().read_line(&mut raw_input).expect("CANNOT READ INPUT!"); //read user input from standard input and store it to raw_input

                //attempt to parse input
                if raw_input.trim().eq_ignore_ascii_case("board") {
                    //print the board state
                    print_board(&guesses);
                    continue; //run loop again
                }
                else if raw_input.trim().eq_ignore_ascii_case("quit") {
                    //quit the game
                    println!("QUITTER!  MY COMBINATION WAS: {}\nGOOD BYE", answer._as_human_readible_words());
                    return; //exit the game
                }
                else {
                    //parse input for a code
                    match CODE::new_from_string(raw_input, num_colors) {
                        Some(code) => {
                            //ensure code is correct length
                            if code.code.len() != num_positions { // if not
                                println!("BAD NUMBER OF POSITIONS.");
                                continue; //run loop again
                            }
                            else {break code;}//break with the code
                        },
                        None => continue, //run loop again
                    }
                }
            });

            //evaluate guess
            guess.evaluate(&answer);
            let blacks = guess.blacks;
            let whites = guess.whites;
            //add guess to the list of guesses
            guesses.push(guess);

            //tell human the results
            if blacks >= num_positions { //guessed it correctly
                println!("YOU GUESSED IT IN {} MOVES!", num_moves);
                human_score += num_moves;
                print_scores(human_score,computer_score);
                break; //break from loop
            } else { //didn't
                println!("YOU HAVE {} BLACKS AND {} WHITES.", blacks, whites);
                //increment moves
                num_moves += 1;
            }
        }

        //computer is code-breaker, human player is code-maker
        println!("\nNOW I GUESS.  THINK OF A COMBINATION.\nHIT RETURN WHEN READY: ");
        //prompt user to give a valid combination #730
        //input loop
        answer = loop {
            let mut raw_input = String::new(); //temp variable to store user input
            io::stdin().read_line(&mut raw_input).expect("CANNOT READ INPUT!"); //read user input from standard input and store it to raw_input

            //attempt to create a code from the user input, if successful break the loop returning the code
            if let Some(code) = CODE::new_from_string(raw_input, num_colors) {
                if code.code.len() == num_positions {break code;} //exit loop with code
                else {println!("CODE MUST HAVE {} POSITIONS", num_positions);continue;} //tell them to try again
            }

            println!("INVALID CODE.  TRY AGAIN"); //if unsuccessful, this is printed and the loop runs again
        };

        //reset some things in preparation for computer play
        guesses.clear();
        num_moves = 0;
        //let num_colors = *answer.code.iter().max().unwrap(); //figure out the number of colors from the code | Commented bc we're enforcing that the computer cracks the same size code as the human
        //let num_positions = answer.code.len(); //figure out the number of positions from the code | Commented bc we're enforcing that the computer cracks the same size code as the human

        //round loop
        loop {
            //loop condition
            if num_moves > num_guesses {
                println!("I USED UP ALL MY MOVES!");
                println!("I GUESS MY CPU IS JUST HAVING AN OFF DAY.");
                computer_score += num_moves;
                print_scores(human_score,computer_score);
                break;
            }

            //randomly generate a guess //770
            let mut guess_int = rng.gen_range(0..total_posibilities);
            guess = GUESS::new(CODE::new());
            //if it's possible, use it //780
            if all_possibilities[guess_int] {
                guess = GUESS::new(CODE::new_from_int(guess_int, num_colors, num_positions)); //create guess
            }
            else {//if it's not possible:
                //  search all possibilities after guess, use first valid one //790
                for g in guess_int..total_posibilities {
                    if all_possibilities[g] {
                        guess_int=g;
                        guess = GUESS::new(CODE::new_from_int(guess_int, num_colors, num_positions)); //create guess
                        break;
                    }
                }
                //if none was found
                //  search all possibilities before guess, use first valid one //820
                if guess.code.code.is_empty() {
                    for g in (0..guess_int).rev() {
                        if all_possibilities[g] {
                            guess_int=g;
                            guess = GUESS::new(CODE::new_from_int(guess_int, num_colors, num_positions)); //create guess
                            break;
                        }
                    }
                }
                // if none where found, tell the user and start over #850
                if guess.code.code.is_empty() {
                    println!("YOU HAVE GIVEN ME INCONSISTENT INFORMATION.");
                    println!("PLAY AGAIN, AND THIS TIME PLEASE BE MORE CAREFUL.");
                    return; //exit game
                };
            }

            //convert guess into something readible #890
            //print it #940
            println!("MY GUESS IS: {}", guess.code._as_human_readible_chars());
            //ask user for feedback, #980
            let blacks=get_number_from_user_input("BLACKS: ", "", 0, num_positions);
            let whites=get_number_from_user_input("WHITES: ", "", 0, num_positions);

            //if we got it, end #990
            if blacks >= num_positions { //guessed it correctly
                println!("I GOT IT IN {} MOVES!", num_moves);
                computer_score += num_moves;
                print_scores(human_score,computer_score);
                break; //break from loop
            } else { //didn't
                all_possibilities[guess_int] = false;
                //if we didn't, eliminate the combinations that don't work
                //we know the number of black and white pegs for a valid answer, so eleminate all that get different amounts
                all_possibilities.iter_mut().enumerate().for_each(|b| {
                    if *b.1 { //filter out ones we already know aren't possible
                        let mut tmp_guess = GUESS::new(CODE::new_from_int(b.0, num_colors, num_positions));
                        tmp_guess.evaluate(&answer);
                        if blacks > tmp_guess.blacks || whites > tmp_guess.whites { //if number of blacks/whites is different, set it to false
                            *b.1 = false;
                        }
                    }
                });

                //increment moves
                num_moves += 1;
            }


        }

    }
}

/**
 * print the welcome message
 */
fn welcome() {
    println!("
                             MASTERMIND
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY


    ");
}

/**
 * print scores
 */
fn print_scores(human_score:usize, computer_score:usize) {
    println!("SCORE\n\tCOMPUTER: {}\n\tHUMAN: {}", computer_score, human_score);
}

/**
 * print the color - letter table
 * only prints the first num_colors pairs
 */
fn print_color_letter_table(num_colors: usize) {
    println!("COLOR\tLETTER");
    println!("=====\t======");
    for i in 0..num_colors {
        println!("{}\t{}", COLORS[i], &LETTERS[i..i+1]);
    }
}

fn print_board(guesses: &Vec<GUESS>) {
    println!("BOARD");
    println!("MOVE\tGUESS\t\tBLACK\tWhite");
    for guess in guesses.iter().enumerate() {
        println!("{}\t{}\t\t{}\t{}", guess.0,guess.1.code._as_human_readible_chars(),guess.1.blacks,guess.1.whites);
    }

}

/**
 * gets a number from user input
 * pass an empty &str for error_message if you don't want one printed
 * pass a min lower  than the max to have minimun and maximun bounds
 * pass a min higher than the max to only have a minumum bound
 * pass a min equal   to  the max to only have a maximun bound
 */
fn get_number_from_user_input<T: Display + PartialOrd + FromStr>(prompt: &str, error_message: &str, min:T, max:T) -> T {
    //DATA
    let mut raw_input = String::new(); // temporary variable for user input that can be parsed later

    //input loop
    return loop {

        //print prompt
        println!("{}", prompt);
        //read user input from standard input, and store it to raw_input
        raw_input.clear(); //clear input
        io::stdin().read_line(&mut raw_input).expect( "CANNOT READ INPUT!");

        //from input, try to read a number
        if let Ok(i) = raw_input.trim().parse() {
            //what bounds must the input fall into
            if min < max {  //have a min and max bound: [min,max]
                if i >= min && i <= max {//is input valid, within bounds
                    break i; //exit the loop with the value i, returning it
                } else {println!("ONLY BETWEEN {} AND {}, PLEASE!", min, max);} //print error message specific to this case
            } else if min > max { //only a min bound: [min, infinity)
                if i >= min {break i;} else {println!("NO LESS THAN {}, PLEASE!", min);}
            } else { //only a max bound: (-infinity, max]
                if i <= max {break i;} else {println!("NO MORE THAN {}, PLEASE!", max);}
            }
            continue; //continue to the next loop iteration
        };
        //this is only reached if a number couldn't be parsed from the input
        if !error_message.is_empty() {println!("{}",error_message);} //if they gave an error message to use, print it
    };
}
