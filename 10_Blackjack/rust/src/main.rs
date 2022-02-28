use rand::{prelude::{thread_rng, SliceRandom}};
use std::{io, collections::HashSet};

/**
 * todo list:
 * 
 * allow splitting
 * 
 * keep track of player bets
 * 
 * allow doubling down
 */


//DATA
//enums
enum PlayerType {
    Player,
    Dealer,
}
enum Play {
    Stand,
    Hit,
}
impl ToString for Play {
    fn to_string(&self) -> String {
        match self {
            &Play::Hit => return String::from("Hit"),
            &Play::Stand => return String::from("Stand"),
        }
    }
}
//structs
struct CARD<'a> {
    name: &'a str,
    value: u8,
}
impl<'a> CARD<'a> {
    /**
     * creates a new card from the passed card name
     */
    fn new(card_name: &str) -> CARD {
        return CARD { name: card_name, value: CARD::determine_value_from_name(card_name) };
    }

    /**
     * returns the value associated with a card with the passed name
     * return 0 if the passed card name doesn't exist
     */
    fn determine_value_from_name(card_name: &str) -> u8 {
        //DATA
        let value:u8;

        match card_name.to_ascii_uppercase().as_str() {
            "ACE" => value = 11,
            "2" => value = 2,
            "3" => value = 3,
            "4" => value = 4,
            "5" => value = 5,
            "6" => value = 6,
            "7" => value = 7,
            "8" => value = 8,
            "9" => value = 9,
            "10" => value = 10,
            "JACK" => value = 10,
            "QUEEN" => value = 10,
            "KING" => value = 10,
            _ => value = 0,
        }

        return value;
    }
}
struct HAND<'a> {
    cards: Vec<CARD<'a>>,
}
impl<'a> HAND<'a> {
    /**
     * returns a new empty hand
     */
    fn new() -> HAND<'a> {
        return HAND { cards: Vec::new()};
    }
    
    /**
     * add a passed card to this hand 
     */
    fn add_card(&mut self, card: CARD<'a>) {
        self.cards.push(card);
    }

    /**
     * returns the total points of the cards in this hand
     */
    fn get_total(&self) -> usize {
        let mut total:usize = 0;
        for card in &self.cards {
            total += card.value as usize;
        }

        //if there is an ACE, and the hand would otherwise bust, treat the ace like it's worth 1
        if total > 21 && self.cards.iter().any(|c| -> bool {*c.name == *"ACE"}) {
            total -= 10;
        }

        return total;
    }

    /**
     * adds the cards in hand into the discard pile
     */
    fn discard_hand(&mut self, deck: &mut DECKS<'a>) {
        let len = self.cards.len();
        for _i in 0..len {
            deck.discard_pile.push(self.cards.pop().expect("hand empty"));
        }
    }
}
struct DECKS<'a> {
    deck: Vec<CARD<'a>>, //cards in the deck
    discard_pile: Vec<CARD<'a>> //decks discard pile
}
impl<'a> DECKS<'a> {
    /**
     * creates a new full and shuffled deck, and an empty discard pile
     */
    fn new() -> DECKS<'a> {
        //returns a number of full decks of 52 cards, shuffles them
        //DATA
        let mut deck = DECKS{deck: Vec::new(), discard_pile: Vec::new()};
        let number_of_decks = 3; 

        //fill deck
        for _n in 0..number_of_decks { //fill deck with number_of_decks decks worth of cards
            for card_name in CARD_NAMES { //add 4 of each card, totaling one deck with 4 of each card
                deck.deck.push( CARD::new(card_name) );
                deck.deck.push( CARD::new(card_name) );
                deck.deck.push( CARD::new(card_name) );
                deck.deck.push( CARD::new(card_name) );
            }
        }

        //shuffle deck
        deck.shuffle();

        //return deck
        return deck;
    }

    /**
     * shuffles the deck
     */
    fn shuffle(&mut self) {
        self.deck.shuffle(&mut thread_rng());
    }

    /**
     * draw card from deck, and return it
     * if deck is empty, shuffles discard pile into it and tries again
     */
    fn draw_card(&mut self) -> CARD<'a> {
        match self.deck.pop() {
            Some(card) => return card,
            None => {
                let len = self.discard_pile.len();

                if len > 0 {//deck is empty, shuffle discard pile into deck and try again
                    println!("deck is empty, shuffling");
                    for _i in 0..len {
                        self.deck.push( self.discard_pile.pop().expect("discard pile empty") )
                    }
                    self.shuffle();
                    return self.draw_card();
                } else { //discard pile and deck are empty, should never happen
                    panic!("discard pile empty");
                }
            }
        }
    }
}

struct PLAYER<'a> {
    hand: HAND<'a>,
    _balance: usize,
    wins: usize,
    player_type: PlayerType,
}
impl<'a> PLAYER<'a> {
    /**
     * creates a new player of the given type
     */
    fn new(player_type: PlayerType) -> PLAYER<'a> {
        return PLAYER { hand: HAND::new(), _balance: STARTING_BALANCE, wins: 0, player_type: player_type};
    }

    /**
     * returns a string of the players hand
     * 
     * if player is a dealer, returns the first card in the hand followed by *'s for every other card
     * if player is a player, returns every card and the total
     */
    fn print_hand(&self, hide_dealer:bool) -> String {
        if !hide_dealer {
            return format!(
                "{}\n\ttotal points = {}", //message
                { //cards in hand
                    let mut s:String = String::new();
                    for cards_in_hand in self.hand.cards.iter().rev() {
                        s += format!("{}\t", cards_in_hand.name).as_str();
                    }
                    s
                },
                self.hand.get_total() //total points in hand
            );
        }

        match &self.player_type {
            &PlayerType::Dealer =>  { //if this is a dealer
                return format!(
                    "{}*",//message 
                    { //*'s for other cards
                        let mut s:String = String::new();
                        let mut cards_in_hand = self.hand.cards.iter();
                        cards_in_hand.next();//consume first card drawn
                        for c in cards_in_hand.rev() {
                            s += format!("{}\t", c.name).as_str();
                        }
                        s
                    }
                );
            },
            &PlayerType::Player => { //if this is a player
                return format!(
                    "{}\n\ttotal points = {}", //message
                    { //cards in hand
                        let mut s:String = String::new();
                        for cards_in_hand in self.hand.cards.iter().rev() {
                            s += format!("{}\t", cards_in_hand.name).as_str();
                        }
                        s
                    },
                    self.hand.get_total() //total points in hand
                );
            }
        }
    }

    /** 
     * get the players 'play'
     */
    fn get_play(&self) -> Play {
        /*
         do different things depending on what type of player this is:
         if it's a dealer, use an algorithm to determine the play
         if it's a player, ask user for input
         */
        match &self.player_type { 
            &PlayerType::Dealer => {
                if self.hand.get_total() > 16 { // if total value of hand is greater than 16, stand
                    return Play::Stand;
                } else { //otherwise hit
                    return Play::Hit;
                }
            },
            &PlayerType::Player => {
                let play = get_char_from_user_input("\tWhat is your play?", &vec!['s','S','h','H']);
                match play {
                    's' | 'S' => return Play::Stand,
                    'h' | 'H' => return Play::Hit,
                    _ => panic!("get_char_from_user_input() returned invalid character"),
                }
            },
        }
    }
}

struct GAME<'a> {
    players: Vec<PLAYER<'a>>, //last item in this is the dealer
    decks: DECKS<'a>,
    rounds:usize,
    games_played:usize,
}
impl<'a> GAME<'a> {
    /**
     * creates a new game
     */
    fn new(num_players:usize) -> GAME<'a> {
        //DATA
        let mut players: Vec<PLAYER> = Vec::new();

        //add dealer
        players.push(PLAYER::new(PlayerType::Dealer));
        //create human player(s) (at least one)
        players.push(PLAYER::new(PlayerType::Player));
        for _i in 1..num_players { //one less than num_players players
            players.push(PLAYER::new(PlayerType::Player));
        }

        //ask if they want instructions
        if let 'y'|'Y' = get_char_from_user_input("Do you want instructions? (y/n)", &vec!['y','Y','n','N']) {
            instructions();
        }
        println!();

        //return a game
        return GAME { players: players, decks: DECKS::new(), rounds: 0, games_played: 0}
    }

    /**
     * prints the score of every player
     */
    fn print_wins(&self) {
        println!("Scores:");
        for player in self.players.iter().enumerate() {
            match player.1.player_type {
                PlayerType::Player => println!("Player{} wins:\t{}", player.0, player.1.wins),
                PlayerType::Dealer => println!("Dealer  wins:\t{}", player.1.wins)
            }
        }
    }

    /**
     * plays a round of blackjack
     */
    fn play_game(&mut self) {
        //deal cards to each player
        for _i in 0..2 { // do this twice
            //draw card for each player
            self.players.iter_mut().for_each(|player| {player.hand.add_card( self.decks.draw_card() );});
        }

        let mut players_playing: HashSet<usize> = HashSet::new(); // the numbers presenting each player still active in the round
        for i in 0..self.players.len() { //runs number of players times
            players_playing.insert(i);
        }
        //round loop (each player either hits or stands)
        loop {
            //increment rounds
            self.rounds += 1;

            //print game state
            println!("\n\t\t\tGame {}\tRound {}", self.games_played, self.rounds);

            self.players.iter().enumerate().for_each(|player| {
                match player.1.player_type {
                    PlayerType::Player => println!("Player{} Hand:\t{}", player.0, player.1.print_hand(true)),
                    PlayerType::Dealer => println!("Dealer Hand:\t{}", player.1.print_hand(true))
                }
            });

            print!("\n"); //empty line

            //get the moves of all remaining players
            for player in self.players.iter_mut().enumerate() {
                match player.1.player_type {//print the player name
                    PlayerType::Player => print!("Player{}:", player.0),
                    PlayerType::Dealer => print!("Dealer:"),
                }

                //check their hand value for a blackjack(21) or bust
                let score = player.1.hand.get_total();
                if score >= 21 {
                    if score == 21 { // == 21
                        println!("\tBlackjack (21 points)");
                    } else { // > 21
                        println!("\tBust      ({} points)", score);
                    }
                    players_playing.remove(&player.0);//remove player from playing list
                    continue;
                }

                //get play
                let play = player.1.get_play();

                //process play
                match play {
                    Play::Stand => {
                        println!("\t{}s", play.to_string());
                        players_playing.remove(&player.0);//remove player from playing list
                    },
                    Play::Hit => {
                        println!("\t{}s", play.to_string());
                        //give them a card
                        player.1.hand.add_card( self.decks.draw_card() );
                    },
                }
            }

            //loop end condition
            if players_playing.is_empty() {break;}
        }

        //determine winner
        let mut top_score = 0; //player with the highest points
        let mut num_winners = 1;
        for player in self.players.iter_mut().enumerate().filter( |x| -> bool {x.1.hand.get_total()<=21}) { //players_who_didnt_bust
            let score = player.1.hand.get_total();
            if score > top_score {
                top_score = score;
                num_winners = 1;
            } else if score == top_score {
                num_winners += 1;
            }
        }

        //print everyones hand
        println!("");
        self.players.iter().enumerate().for_each(|player| {
            match player.1.player_type {
                PlayerType::Player => println!("Player{} Hand:\t{}", player.0, player.1.print_hand(false)),
                PlayerType::Dealer => println!("Dealer Hand:\t{}", player.1.print_hand(false))
            }
        });
        println!("");
        //for each player with the top score
        self.players.iter_mut().enumerate().filter(|x|->bool{x.1.hand.get_total()==top_score}).for_each(|x| {
            match x.1.player_type {
                PlayerType::Player => print!("Player{}\t", x.0),
                PlayerType::Dealer => print!("Dealer\t"),
            }
            //increment their wins
            x.1.wins += 1;
        });
        if num_winners > 1 {println!("all tie with {}\n\n\n", top_score);} 
        else {println!("wins with {}!\n\n\n",top_score);}

        //discard hands
        self.players.iter_mut().for_each(|player| {player.hand.discard_hand(&mut self.decks);});

        //increment games_played
        self.games_played += 1;
    }



}

const CARD_NAMES: [&str;13] = ["ACE","2","3","4","5","6","7","8","9","10","JACK","QUEEN","KING"];
const STARTING_BALANCE: usize = 100;

fn main() {
    //DATA
    let mut game: GAME;

    //print welcome message
    welcome();

    //create game    
    game = GAME::new( get_number_from_user_input("How many players should there be (at least 1)?", 1, 7) );

    //game loop, play game until user wants to stop
    loop {
        //print score of every user
        game.print_wins();

        //play round
        game.play_game();

        //ask if they want to play again
        match get_char_from_user_input("Play Again? (y/n)", &vec!['y','Y','n','N']) {
            'y' | 'Y' => continue,
            'n' | 'N' => break,
            _ => break,
        }
    }
}

/**
 * prints the welcome screen
 */
fn welcome() {
    //welcome message
    println!("
                            BLACK JACK
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY    
    \n\n\n");
}

/**
 * prints the instructions
 */
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

    NOTE: Currently only H and S are implemented properly
    ");
}

/**
 * gets a usize integer from user input
 */
fn get_number_from_user_input(prompt: &str, min:usize, max:usize) -> usize {
    //input loop
    return loop {
        let mut raw_input = String::new(); // temporary variable for user input that can be parsed later

        //print prompt
        println!("{}", prompt);

        //read user input from standard input, and store it to raw_input
        //raw_input.clear(); //clear input
        io::stdin().read_line(&mut raw_input).expect( "CANNOT READ INPUT!");

        //from input, try to read a number
        match raw_input.trim().parse::<usize>() {
            Ok(i) => {
                if i < min || i > max { //input out of desired range
                    println!("INPUT OUT OF VALID RANGE.  TRY AGAIN.");
                    continue; // run the loop again
                } 
                else {
                    println!();
                    break i;// this escapes the loop, returning i 
                }
            },
            Err(e) => {
                println!("INVALID INPUT.  TRY AGAIN.  {}", e.to_string().to_uppercase());
                continue; // run the loop again
            }
        };
    };
}

/**
 * gets a character from user input
 * returns the first character they type
 */
fn get_char_from_user_input(prompt: &str, valid_results: &Vec<char>) -> char {
    //input loop
    return loop {
        let mut raw_input = String::new(); // temporary variable for user input that can be parsed later

        //print prompt
        println!("{}", prompt);

        //read user input from standard input, and store it to raw_input
        //raw_input.clear(); //clear input
        io::stdin().read_line(&mut raw_input).expect( "CANNOT READ INPUT!");

        //from input, try to read a valid character
        match raw_input.trim().chars().nth(0) {
            Some(i) => {
                if !valid_results.contains(&i) { //input out of desired range
                    println!("INPUT IS NOT VALID CHARACTER.  TRY AGAIN.");
                    continue; // run the loop again
                } 
                else {
                    break i;// this escapes the loop, returning i 
                }
            },
            None => {
                println!("INVALID INPUT.  TRY AGAIN.");
                continue; // run the loop again
            }
        };
    };
}
