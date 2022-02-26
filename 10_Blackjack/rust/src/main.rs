use rand::{Rng, prelude::thread_rng};
use std::{io};

/**
 * todo list:
 * 
 * make a 2 player game functional
 * 
 * add more players
 * 
 * use a 3 deck pool of cards instead of an infinite amount
 * 
 * keep track of player bets
 */


//DATA
struct CARD {
    name: String,
    value: u8,
}
impl CARD {
    
}
struct HAND {
    cards: Vec<CARD>,
    total: u8,
}
impl HAND {
    fn drawCard(deck: DECK) -> CARD {
        //pull a random card from deck

    }
}
struct DECK {
    cards: Vec<CARD>,
}
struct PLAYER {
    hand: HAND,
    balance: usize,
}
impl PLAYER {
    fn new() -> PLAYER {

    }
}

struct GAME {
    human_player: PLAYER,
    computer_players: Vec<PLAYER>,
    dealer: PLAYER,

    deck: DECK,
    discard_pile: DECK,
}
impl GAME {
    fn start(num_players:u8) -> GAME {
        //ask user how many players there should be, not including them
        let num_players = loop {
            //input loop

            let mut raw_input = String::new(); // temporary variable for user input that can be parsed later
            //print prompt
            println!("How many other players should there by?");

            //read user input from standard input, and store it to raw_input
            raw_input.clear(); //clear input
            io::stdin().read_line(&mut raw_input).expect( "CANNOT READ INPUT!");

                //from input, try to read a number
            match raw_input.trim().parse::<usize>() {
                Ok(i) => break i, // this escapes the loop, returning i 
                Err(e) => {
                    println!("MEANINGLESS NUMBER.  TRY AGAIN.  {}", e.to_string().to_uppercase());
                    continue; // run the loop again
                }
            };
        };




        //return a game
        return GAME { human_player: PLAYER::new(), computer_players: (), dealer: (), deck: (), discard_pile: () }


    }
}

const card_names: [&str;13] = ["Ace","2","3","4","5","6","7","8","9","10","Jack","Queen","King"];

fn main() {
    //DATA
    //P(I,J) IS THE JTH CARD IN HAND I, Q(I) IS TOTAL OF HAND I
    //C IS THE DECK BEING DEALT FROM, D IS THE DISCARD PILE,
    //T(I) IS THE TOTAL FOR PLAYER I, S(I) IS THE TOTAL THIS HAND FOR
    //PLAYER I, B(I) IS TH BET FOR HAND I
    //R(I) IS THE LENGTH OF P(I,*)

    //print welcome message
    welcome();
    

}

fn welcome() {
    //welcome message
    println!("
                            BLACK JACK
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY    
    \n\n\n");
}

fn instructions() {
    println!("
    THIS IS THE GAME OF 21. AS MANY AS 7 PLAYERS MAY PLAY THE
    GAME. ON EACH DEAL, BETS WILL BE ASKED FOR, AND THE
    PLAYERS' BETS SHOULD BE TYPED IN. THE CARDS WILL THEN BE
    DEALT, AND EACH PLAYER IN TURN PLAYS HIS HAND. THE
    FIRST RESPONSE SHOULD BE EITHER 'D', INDICATING THAT THE
    PLAYER IS DOUBLING DOWN, 'S', INDICATING THAT HE IS
    STANDING, 'H', INDICATING HE WANTS ANOTHER CARD, OR '/',
    INDICATING THAT HE WANTS TO SPLIT HIS CARDS. AFTER THE
    INITIAL RESPONSE, ALL FURTHER RESPONSES SHOULD BE 'S' OR
    'H', UNLESS THE CARDS WERE SPLIT, IN WHICH CASE DOUBLING
    DOWN IS AGAIN PERMITTED. IN ORDER TO COLLECT FOR
    BLACKJACK, THE INITIAL RESPONSE SHOULD BE 'S'.
    NUMBER OF PLAYERS
    ");
}