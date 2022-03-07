use rand::{Rng, prelude::{thread_rng, ThreadRng}};
use std::{io, fmt::Display, str::FromStr, iter::OnceWith};

//DATA
const COLORS: [&str;8] = ["Black ", "White ","Red ","Green ","Orange ","Yellow ", "Purple ", "Tan "]; //all available colors
const LETTERS: &str = "BWRGOYPT"; //letters representing the above colors

struct CODE {
    code: Vec<usize>, //maybe use a char array later, idk

}
impl CODE {
    /**
     * generates and returns a random CODE with the given parameters
     */
    fn new_random(rng: &mut ThreadRng, num_colors: &usize, num_positions: &usize) -> CODE {
        //data
        let mut code = CODE{code: Vec::new()};
        //generate random combination of colors
        for _i in 0..*num_positions {
            code.code.push(rng.gen_range(0..*num_colors));
        } 
        return code;
    }
    /**
     * returns a code from the parsed string
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
        if self.code.code.len() != answer.code.len() {
            panic!("only codes of the same length can be compared");
        }
        let tmp_code = self.code.code.clone(); //copy the our code so we can modify it without changing the actual code
        let mut tmp_answer: Vec<_> = answer.code.clone().iter().map(|x| Some(*x)).collect(); //copy the our code so we can modify it without changing the actual code, also wrap it for reasons
        //same value same position O(N)
        let trimmed_code: Vec<usize> =  tmp_code.iter().enumerate().filter_map(|e| 
            if Some(e.1) != answer.code.get(e.0) {
                Some(*e.1)
            }
            else {
                tmp_answer[e.0] = None;
                None
            }).collect(); //filters tmp_code, removing all values that are in the same position as in the answer
        self.blacks = self.code.code.len() - trimmed_code.len();
        //same value, wrong position
        self.whites = trimmed_code.iter().filter(|i| tmp_answer.contains(&Some(**i))).count();
    }
}

fn main() {
    //DATA
    let mut rng = thread_rng();
    let num_colors: usize;
    let num_positions: usize;
    let num_rounds: usize;
    let total_posibilities: usize;

    let mut num_moves: usize = 1;
    let mut answer: CODE;
    let mut guess: GUESS;
    let mut guesses: Vec<GUESS> = Vec::new();

    let mut human_score: usize = 0;
    let mut computer_score: usize = 0;

    //print welcome message
    welcome();

    //ask user for a number of colors, positions, and rounds
    num_colors = get_number_from_user_input("NUMBER OF COLORS", "", 1, COLORS.len());
    num_positions = get_number_from_user_input("NUMBER OF POSITIONS", "", 1, 10);
    num_rounds = get_number_from_user_input("NUMBER OF ROUNDS", "", 1, 10);

    //print number of posibilities
    total_posibilities = num_colors.pow(num_positions as u32);
    println!("\nTOTAL POSSIBILITIES = {}\n", total_posibilities);

    //print color letter table
    print_color_letter_table(num_colors);

    //game loop
    for round_num in 1..=num_rounds {
        //print round number
        println!("\n\nROUND NUMBER: {}", round_num);


        //human player is code-breaker, computer is code-maker
        //generate a combination
        answer = CODE::new_random(&mut rng, &num_colors, &num_positions);
        println!("CODE: {:?}", answer._as_human_readible_chars()); //this is for troubleshooting, prints the code converted back into characters

        //round loop
        loop {
            //loop condition
            if num_moves > 10 {
                println!("YOU RAN OUT OF MOVES!  THAT'S ALL YOU GET!");
                println!("THE ACTUAL COMBINATION WAS: {}", answer._as_human_readible_chars());
                human_score += num_moves;
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
                break; //break from loop
            } else { //didn't
                println!("YOU HAVE {} BLACKS AND {} WHITES.", blacks, whites);
                //increment moves
                num_moves += 1;
            }
        }

        //reset some things in preparation for computer play
        let mut possibilities = vec![true; total_posibilities];
        guesses.clear();
        num_moves = 0;

        //computer is code-breaker, human player is code-maker
        println!("\nNOW I GUESS.  THINK OF A COMBINATION.\nHIT RETURN WHEN READY: ");
        //prompt user to give a valid combination #730
        //input loop
        answer = loop {
            let mut raw_input = String::new(); //temp variable to store user input
            io::stdin().read_line(&mut raw_input).expect("CANNOT READ INPUT!"); //read user input from standard input and store it to raw_input
            if let Some(code) = CODE::new_from_string(raw_input, LETTERS.len()) {break code;} //attempt to create a code from the user input, if successful break the loop returning the code
            println!("INVALID CODE.  TRY AGAIN"); //if unsuccessful, this is printed and the loop runs again
        };
        //println!("CODE: {:?}", answer._as_human_readible_chars()); //this is for troubleshooting, prints the code converted back into characters
        //println!("CODE: {:?}", answer._as_human_readible_words()); //this is for troubleshooting, prints the code converted back into words

        //round loop
        for computer_move in 1..=10 {
            //randomly generate a guess //770

            //if it's possible, use it //780
            //if it's not possible: 
            //  search all possibilities after guess, use first valid one //790
            //  search all possibilities before guess, use first valid one //820
            // if none where found, tell the user and start over #850
            
            //convert guess into something readible #890
            //print it #940
            //ask user for feedback, #980

            //if we got it, end #990
            //if we didn't, eliminate the combinations that don't work

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