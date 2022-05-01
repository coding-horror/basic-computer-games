/*
 lib.rs contains all the logic of the program
*/
use rand::{Rng, prelude::thread_rng}; //rng
use std::error::Error; //better errors
use std::io::{self, Write}; //io interactions
use std::{str::FromStr, fmt::Display}; //traits

//DATA
const COLORS: [&str;8] = ["Black ", "White ","Red ","Green ","Orange ","Yellow ", "Purple ", "Tan "]; //all available colors
const LETTERS: &str = "BWRGOYPT"; //letters representing the above colors

/// handles setup for the game
pub struct Config {
    num_colors: usize,
    num_positions: usize,
    num_rounds: usize,
    num_guesses: usize,
    total_possibilities: usize,
}
impl Config {
    /// creates and returns a new Config from user input
    pub fn new() -> Result<Config, Box<dyn Error>> {
        //DATA
        let mut config: Config = Config { 
            num_colors: 0,
            num_positions: 0,
            num_rounds: 0,
            num_guesses: 0,
            total_possibilities: 0,
        };

        //get data from user input
        //input loop
        loop {match get_number_from_input("NUMBER OF COLORS", 2, COLORS.len()) {
            Ok(num) => {
                config.num_colors = num;
                break;
            },
            Err(e) => eprintln!("{}",e),
        }}
        //input loop
        loop {match get_number_from_input("NUMBER OF POSITIONS", 2, 10) {
            Ok(num) => {
                config.num_positions = num;
                break;
            },
            Err(e) => eprintln!("{}",e),
        }}
        //input loop
        loop {match get_number_from_input("NUMBER OF ROUNDS", 1, 10) {
            Ok(num) => {
                config.num_rounds = num;
                break;
            },
            Err(e) => eprintln!("{}",e),
        }}
        //input loop
        loop {match get_number_from_input("NUMBER OF GUESSES", 10, 0) {
            Ok(num) => {
                config.num_guesses = num;
                break;
            },
            Err(e) => eprintln!("{}",e),
        }}
        //calc total posibilities
        config.total_possibilities = config.num_colors.pow(config.num_positions as u32);

        //return new config
        return Ok(config);
    }
}

/// run the program
pub fn run(config: &Config) -> Result<(), Box<dyn Error>> {
    //DATA
    let mut human_score: usize = 0;
    let mut computer_score: usize = 0;

    //print number of possibilities
    println!("\nTOTAL POSSIBILITIES = {}\n", config.total_possibilities);

    //print color letter table
    print_color_letter_table(config.num_colors);

    //for every round
    for round in 1..=config.num_rounds {
        //print round number
        println!("\n\nROUND NUMBER: {}", round);

        //computer as code-maker
        play_round_computer_codemaker(config, &mut human_score, &computer_score);

        //human as code-maker
        play_round_human_codemaker(config, &human_score, &mut computer_score);

        //update and print score

    }

    //return to main
    Ok(())
}

/// run a round with computer as code-maker
fn play_round_computer_codemaker(config: &Config, human_score:&mut usize, computer_score:&usize) {
    //DATA
    let mut rng = thread_rng();
    let mut guesses: Vec<Code> = Vec::new();
    let secret: Code;

    //generate secret
    secret = Code::new_from_int(rng.gen_range(0..config.num_colors.pow(config.num_positions.try_into().unwrap())), config);

    //round loop
    for m in 1..=config.num_guesses {
        //get guess from user input
        //input loop
        let mut guess = loop {
            //get input
            let user_input = get_string_from_user_input(format!("\nMOVE # {} GUESS: ", m).as_str()).expect("something went wrong getting user guess");

            //parse input
            if user_input.trim().eq_ignore_ascii_case("board") { //print the board state
                print_board(&guesses);
                continue; //run input loop again
            } else if user_input.trim().eq_ignore_ascii_case("quit") { //quit the game
                println!("QUITTER!  MY COMBINATION WAS: {}\nGOOD BYE", secret.as_human_readible_chars());
                return; //exit the game
            } else {
                //parse input for a code
                match Code::new_from_string(&user_input, &config) {
                    Ok(code) => {
                        //ensure code is correct length
                        if code.code.len() != config.num_positions { // if not
                            println!("BAD NUMBER OF POSITIONS.");
                            continue; //run loop again
                        }
                        else {break code;}//break with the code
                    },
                    Err(e) => {eprintln!("{}",e); continue;}, //run loop again
                }
            }
        };

        //update scores
        *human_score += 1;
        
        //evaluate guess
        guess.evaluate(&secret).expect("something went wrong evaluating user guess");

        //tell user the results
        if guess.black_pins >= config.num_positions { //guessed it correctly
            println!("YOU GUESSED IT IN {} MOVES!", m);
            print_scores(*human_score,*computer_score);
            return; //exit function
        } else { //didn't
            println!("YOU HAVE {} BLACKS AND {} WHITES.", guess.black_pins, guess.white_pins);
        }

        //add guess to the list of guesses
        guesses.push(guess);
    }

    //only runs if user doesn't guess the code
    println!("YOU RAN OUT OF MOVES!  THAT'S ALL YOU GET!");
    println!("THE ACTUAL COMBINATION WAS: {}", secret.as_human_readible_chars());
    print_scores(*human_score,*computer_score);
}

/// run a round with human as code-maker
fn play_round_human_codemaker(config: &Config, human_score:&usize, computer_score:&mut usize, ) {
    //DATA
    let mut rng = thread_rng();
    let mut all_possibilities = vec![true; config.total_possibilities];
    let _secret: Code;


    //get a secret code from user input
    println!("\nNOW I GUESS.  THINK OF A COMBINATION.\nHIT RETURN WHEN READY: ");
    // input loop
    _secret = loop {
        //get input
        let user_input = get_string_from_user_input("").expect("something went wrong getting secret from user");

        //parse input
        if let Ok(code) = Code::new_from_string(&user_input, config) {
            if code.code.len() == config.num_positions {break code;} //exit loop with code
            else {println!("CODE MUST HAVE {} POSITIONS", config.num_positions);continue;} //tell them to try again
        }
        println!("INVALID CODE.  TRY AGAIN"); //if unsuccessful, this is printed and the loop runs again
    };

    //round loop
    for m in 1..=config.num_guesses {
        let mut guess: Code = Code::new();

        //randomly generate a guess //770
        let mut guess_int = rng.gen_range(0..config.total_possibilities);
        // if possible, use it //780
        if all_possibilities[guess_int] {
            guess = Code::new_from_int(guess_int, &config); //create guess
        }
        else {// if not possible:
            // search all possibilities after guess, use first valid one //790
            for g in guess_int..config.total_possibilities {
                if all_possibilities[g] {
                    guess_int=g;
                    guess = Code::new_from_int(guess_int, &config); //create guess
                    break;
                }
            }
            // if none was found
            //  search all possibilities before guess, use first valid one //820
            if guess.code.is_empty() {
                for g in (0..guess_int).rev() {
                    if all_possibilities[g] {
                        guess_int=g;
                        guess = Code::new_from_int(guess_int, &config); //create guess
                        break;
                    }
                }
            }

            // if none where found, tell the user and start over #850
            if guess.code.is_empty() {
                println!("YOU HAVE GIVEN ME INCONSISTENT INFORMATION.");
                println!("PLAY AGAIN, AND THIS TIME PLEASE BE MORE CAREFUL.");
                return; //exit game
            };
        }

        //convert guess into something readible #890
        //print it #940
        println!("MY GUESS IS: {}", guess.as_human_readible_chars());

        //ask user for feedback, #980
        // input loop for black pegs
        loop {match get_number_from_input("BLACKS: ", 0, config.num_positions) {
            Ok(num) => {
                guess.black_pins = num;
                break;
            },
            Err(e) => eprintln!("{}",e),
        }}
        // input loop for white pegs
        loop {match get_number_from_input("WHITES: ", 0, config.num_positions) {
            Ok(num) => {
                guess.white_pins = num;
                break;
            },
            Err(e) => eprintln!("{}",e),
        }}

        //if computer guessed it, end #990
        if guess.black_pins >= config.num_positions { //guessed it correctly
            println!("I GOT IT IN {} MOVES!", m);
            print_scores(*human_score,*computer_score);
            return; //exit function
        } else { //didn't
            all_possibilities[guess_int] = false;
            //if we didn't, eliminate the combinations that don't work
            //we know the number of black and white pegs for a valid answer, so eleminate all that get different amounts
            all_possibilities.iter_mut().enumerate().for_each(|b| {
                if *b.1 { //filter out ones we already know aren't possible
                    let mut tmp_guess = Code::new_from_int(b.0, &config);
                    tmp_guess.evaluate(&guess).expect("something went wrong evaluation the computer's guess"); //compare with computer guess
                    if (guess.black_pins != tmp_guess.black_pins) || (guess.white_pins != tmp_guess.white_pins) { //if number of blacks/whites is different, set it to false
                        *b.1 = false;
                    }
                }
            });
        }
    }

    //only runs if computer doesn't guess the code
    println!("I USED UP ALL MY MOVES!");
    println!("I GUESS MY CPU IS JUST HAVING AN OFF DAY.");
    print_scores(*human_score,*computer_score);
}



struct Code {
    code: Vec<usize>,
    black_pins: usize,
    white_pins: usize,
}
impl Code {
    /// create generic, empty code
    fn new() -> Code {
        return Code{code: Vec::new(), black_pins:0, white_pins:0};
    }
    /// converts input_int from base 10 to base num_colors to generate the code
    /// input_int must be between 0 and num_colors.pow(num_positions)
    fn new_from_int(mut input_int: usize, config: &Config) -> Code {
        //DATA
        let mut converted_number:Vec<_> = Vec::new();
        assert!(2 <= config.num_colors && config.num_colors <= 36); //if num_colors is outside of this range, things break later on

        //convert input_int into a code by effectively converting input_int from base 10 to base n where n is num_colors, uses some fancy stuff to do this
        loop {
            converted_number.push(std::char::from_digit((input_int % config.num_colors).try_into().unwrap(), config.num_colors.try_into().unwrap()).unwrap()); //
            input_int /= config.num_colors;
            if input_int == 0 {break}
        }
        
        while converted_number.len() < config.num_positions {converted_number.push('0');} // fill remaining space with zero's
        let converted_number: Vec<_> = converted_number.iter().rev().map(|e| e.to_digit(config.num_colors.try_into().unwrap()).unwrap() .try_into().unwrap()).collect(); //reverse the vector and convert it to integers
        return Code{code: converted_number, black_pins:0, white_pins:0};
    }
    /// returns a code parsed from the passed string
    fn new_from_string(input_string: &str, config: &Config) -> Result<Code,Box<dyn Error>> {
        let valid_chars = &LETTERS[0..config.num_colors];
        //DATA
        let new_code = Code{
            code: {
                input_string.to_ascii_uppercase().chars() //get an iterator with all the chars in input string converted to uppercase
                .filter( |c| { valid_chars.contains(*c)}) //remove chars that aren't in LETTERS
                .map( |x| -> usize {valid_chars.find(x).expect("invalid character")})//convert all the chars into usizes representing their index in LETTERS
                .collect() //wrap this iterator up into a vector
            },
            black_pins: 0,
            white_pins: 0,
        };
        //if code is empty, return None, otherwise return Some(code)
        if new_code.code.is_empty() {return Err(String::from("Input String did not contain enough valid characters").into());}
        else {return Ok(new_code);}
    }

    /// returns a string containing the code represented as characters
    fn as_human_readible_chars(&self) -> String {
        return self.code.iter().map(|i|->char{LETTERS.chars().nth(*i).expect("index out of bounds")}).collect();
    }

    /**
     * evaulates itself for the number of black and white pegs it should have when compared to a given secret
     */
    fn evaluate(&mut self, secret:&Code) -> Result<(),Box<dyn Error>> {
        //data
        let mut consumed = vec![false;secret.code.len()];

        if self.code.len() != secret.code.len() {
            return Err(String::from("only codes of the same length can be compared").into());
        }

        for i in 0..secret.code.len() {
            if self.code[i] == secret.code[i] { //correct value correct place
                self.black_pins += 1;
                consumed[i] = true;
            }
            else {
                //check for correct value incorrect place, don't count positions that are already exact matches
                for j in 0..secret.code.len() {
                    if !consumed[j] && self.code[i] == secret.code[j] && self.code[j] != secret.code[j] {
                        self.white_pins += 1;
                        consumed[j] = true;
                        break;
                    }
                }
            }
        }

        return Ok(())
    }
}

/// print scores
fn print_scores(human_score:usize, computer_score:usize) {
    println!("SCORE\n\tCOMPUTER: {}\n\tHUMAN: {}", computer_score, human_score);
}

/// print the color - letter table
/// only prints the first num_colors pairs
fn print_color_letter_table(num_colors:usize) {
    println!("COLOR\tLETTER");
    println!("=====\t======");
    for i in 0..num_colors {
        println!("{}\t{}", COLORS[i], &LETTERS[i..i+1]);
    }
}

/// prints the board state, previous guesses and the number of black/white pins for each
fn print_board(guesses: &[Code]) {
    println!("BOARD");
    println!("MOVE\tGUESS\t\tBLACK\tWhite");
    for guess in guesses.iter().enumerate() {
        println!("{}\t{}\t\t{}\t{}", guess.0,guess.1.as_human_readible_chars(),guess.1.black_pins,guess.1.white_pins);
    }
}

/// gets a string from user input
fn get_string_from_user_input(prompt: &str) -> Result<String, Box<dyn Error>> {
    //DATA
    let mut raw_input = String::new();

    //print prompt
    print!("{}", prompt);
    //make sure it's printed before getting input
    io::stdout().flush().expect("couldn't flush stdout");

    //read user input from standard input, and store it to raw_input, then return it or an error as needed
    raw_input.clear(); //clear input
    match io::stdin().read_line(&mut raw_input) {
        Ok(_num_bytes_read) => return Ok(String::from(raw_input.trim())),
        Err(err) => return Err(format!("ERROR: CANNOT READ INPUT!: {}", err).into()),
    }
}

/// generic function to get a number from the passed string (user input)
/// pass a min lower  than the max to have minimun and maximun bounds
/// pass a min higher than the max to only have a minumum bound
/// pass a min equal   to  the max to only have a maximun bound
/// 
/// Errors:
/// no number on user input
fn get_number_from_input<T:Display + PartialOrd + FromStr>(prompt: &str, min:T, max:T) -> Result<T, Box<dyn Error>> {
    //DATA
    let raw_input: String;
    let processed_input: String;

    
    //input looop
    raw_input = loop {
        match get_string_from_user_input(prompt) {
            Ok(input) => break input,
            Err(e) => {
                eprintln!("{}",e);
                continue;
            },
        }
    };

    //filter out num-numeric characters from user input
    processed_input = raw_input.chars().filter(|c| c.is_numeric()).collect();

    //from input, try to read a number
    match processed_input.trim().parse() {
        Ok(i) => {
            //what bounds must the input fall into
            if min < max {  //have a min and max bound: [min,max]
                if i >= min && i <= max {//is input valid, within bounds
                    return Ok(i); //exit the loop with the value i, returning it
                } else { //print error message specific to this case
                    return Err(format!("ONLY BETWEEN {} AND {}, PLEASE!", min, max).into());
                } 
            } else if min > max { //only a min bound: [min, infinity)
                if i >= min {
                    return Ok(i);
                } else {
                    return Err(format!("NO LESS THAN {}, PLEASE!", min).into());
                }
            } else { //only a max bound: (-infinity, max]
                if i <= max {
                    return Ok(i);
                } else {
                    return Err(format!("NO MORE THAN {}, PLEASE!", max).into());
                }
            }
        },
        Err(_e) => return Err(format!("Error: couldn't find a valid number in {}",raw_input).into()),
    }
}
