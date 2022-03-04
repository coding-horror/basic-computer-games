use rand::prelude::{thread_rng, SliceRandom};
use std::io;

//DATA
struct CARD {
    suit: u8, //1-4     1=hearts    2=diamonds     3=clubs     4=spades
    value: u8, //2-14   2,3,4,5,6,7,8,9,10,jack,queen,king,ace
}
impl CARD {
    /**
     * creates a new card from the passed card name
     */
    fn new(suit:u8,value:u8) -> CARD {
        return CARD { suit: suit, value: value };
    }
}
impl ToString for CARD {
    /**
     * returns string representation of the Card
     */
    fn to_string(&self) -> String {
        let mut name: String;
        //value
        name = match self.value {
            2..=10 => self.value.to_string() + " of ",
            11 => String::from("Jack of "),
            12 => String::from("Queen of "),
            13 => String::from("King of "),
            14 => String::from("Ace of "),
            _ => panic!("card has an invalid value"),
        };
        //suit
        name += match self.suit {
            1=>"Hearts",
            2=>"Diamonds",
            3=>"Clubs",
            4=>"Spades",
            _ => panic!("card has an invalid suit"),
        };
        return name;
    }
}
struct GAME {
    deck: Vec<CARD>, //cards in the deck
    player_score: usize, //human player score
    computer_score:usize,//computer score
}
impl GAME {
    /**
     * creates a new game, filling a deck with 52 cards, and setting scores to 0
     */
    fn new() -> GAME{
        //DATA
        let mut game = GAME{deck: Vec::new(), player_score: 0, computer_score: 0};
        //fill deck
        for suit in 1..=4 { //4 suits
            for value in 2..=14 { //13 cards
                game.deck.push( CARD::new(suit,value) );
            }
        }
        //shuffle deck
        game.deck.shuffle(&mut thread_rng());
        //return game
        return game;
    }

    /**
     * fully play game
     */
    fn play_game(&mut self) {
        //DATA
        let mut human_card: CARD;
        let mut computer_card: CARD;

        //game loop
        while self.deck.len() >=2 {
            //draw cards
            human_card = self.deck.pop().expect("DECK IS EMPTY!");
            computer_card = self.deck.pop().expect("DECK IS EMPTY!");

            //print cards
            println!("\nYou: {}\t\tComputer: {}",human_card.to_string(), computer_card.to_string());

            //determine winner
            if human_card.value > computer_card.value { //human wins
                self.player_score += 1;
                println!("You win. You have {} and the computer has {}", human_card.value, computer_card.value);
            }
            else if human_card.value < computer_card.value { //computer wins
                self.computer_score += 1;
                println!("The computer wins!!! You have {} and the computer has {}", human_card.value, computer_card.value);
            }
            else { //tie
                println!("Tie. No score change.")
            }
            if !get_yes_no_from_user_input("Do you want to continue? y/n") {break;}
        }
        if self.deck.is_empty() {
            println!("WE HAVE RUN OUT OF CARDS.  FINAL SCORE:  YOU: {} THE COMPUTER: {}",self.player_score, self.computer_score);
        }
        println!("\nThanks for playing. It was fun.");
    }
}

fn main() {
    //print welcome message
    welcome();
    
    //game loop
    let mut game = GAME::new();
    loop {
        game.play_game();
        if !get_yes_no_from_user_input("\nPlay again? (yes or no) ") {break;}
        game = GAME::new();
    }
}

/**
 * print welcome message
 */
fn welcome() {
    println!("
                                War\n
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n
    This is the card game of war. Each card is given by # of suit
    ex: Ace of Spades\n
    ");
    if get_yes_no_from_user_input("Do you want directions?") {
        println!("
        The computer gives you and it a 'card'. The higher card
        (numerically) wins. The game ends when you choose not to
        continue or when you have finished the pack.\n
        ");
    }
}

/**
 * returns true if user input starts with y or Y,
 * false otherwise
 */
fn get_yes_no_from_user_input(prompt: &str) -> bool {
    let mut raw_input = String::new(); // temporary variable for user input that can be parsed later

    //print prompt
    println!("{}", prompt);
    //read user input from standard input, and store it to raw_input
    //raw_input.clear(); //clear input
    io::stdin().read_line(&mut raw_input).expect( "CANNOT READ INPUT!");

    //from input, try to read a valid character
    if let Some(i) = raw_input.trim().chars().nth(0) {
        if i == 'y' || i == 'Y' {
            return true;
        }
    }
    //default case
    return false;
}