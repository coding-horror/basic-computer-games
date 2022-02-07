import java.util.ArrayList;
import java.util.Arrays;
import java.util.LinkedList;
import java.util.List;
import java.io.Reader;
import java.io.Writer;

public class Game {
    
    private Deck deck;
    private UserIo userIo;

    public Game(Deck deck, UserIo userIo) {
        this.deck = deck;
        this.userIo = userIo;
    }

    public void run() {
		userIo.println("BLACK JACK", 31);
		userIo.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n", 15);
		if(userIo.promptBoolean("DO YOU WANT INSTRUCTIONS")){
			userIo.println("THIS IS THE GAME OF 21. AS MANY AS 7 PLAYERS MAY PLAY THE");
			userIo.println("GAME. ON EACH DEAL, BETS WILL BE ASKED FOR, AND THE");
			userIo.println("PLAYERS' BETS SHOULD BE TYPED IN. THE CARDS WILL THEN BE");
			userIo.println("DEALT, AND EACH PLAYER IN TURN PLAYS HIS HAND. THE");
			userIo.println("FIRST RESPONSE SHOULD BE EITHER 'D', INDICATING THAT THE");
			userIo.println("PLAYER IS DOUBLING DOWN, 'S', INDICATING THAT HE IS");
			userIo.println("STANDING, 'H', INDICATING HE WANTS ANOTHER CARD, OR '/',");
			userIo.println("INDICATING THAT HE WANTS TO SPLIT HIS CARDS. AFTER THE");
			userIo.println("INITIAL RESPONSE, ALL FURTHER RESPONSES SHOULD BE 'S' OR");
			userIo.println("'H', UNLESS THE CARDS WERE SPLIT, IN WHICH CASE DOUBLING");
			userIo.println("DOWN IS AGAIN PERMITTED. IN ORDER TO COLLECT FOR");
			userIo.println("BLACKJACK, THE INITIAL RESPONSE SHOULD BE 'S'.");
		}

		int nPlayers = 0;
		while(nPlayers < 1 || nPlayers > 7) {
			nPlayers = userIo.promptInt("NUMBER OF PLAYERS");
		}

		deck.reshuffle();

		List<Player> players = new ArrayList<>();
		for(int i = 0; i < nPlayers; i++) {
			players.add(new Player(i + 1));
		}

		while(true) {
			int[] bets = new int[nPlayers]; // empty array initialized with all '0' valuses.
			while(!betsAreValid(bets)){
				userIo.println("BETS:");
				for(int i = 0; i < nPlayers; i++) {
					// Note that the bet for player "1" is at index "0" in the bets
					// array and take care to avoid off-by-one errors.
					bets[i] = userIo.promptInt("#" + (i + 1));  //TODO: If there isn't a need for a separate Bets in the future, combine these two lines and convert to enhanced FOR loop
					players.get(i).setCurrentBet(bets[i]);
				}
			}

			for(Player player : players){
				player.dealCard(deck.deal());
				player.dealCard(deck.deal()); //TODO: This could be in a separate loop to more acurrately follow how a game would be dealt, I couldn't figure out of the BASIC version did it
			}


			// Consider adding a Dealer class to track the dealer's hand and running total.
			// Alternately, the dealer could just be a Player instance where currentBet=0 and is ignored.
			LinkedList<Card> dealerHand = new LinkedList<>();
			Player dealer = new Player(0); //Dealer is Player 0 - this can be converted into a dealer class later on
			dealer.dealCard(deck.deal());
			// TODO deal two cards to the dealer

			// TODO handle 'insurance' if the dealer's card is an Ace.

			printInitialDeal(players, dealer);

			for(Player player : players){
				play(player, deck);
			}

			// only play the dealer if at least one player has not busted or gotten a natural blackjack (21 in the first two cards)
			// otherwise, just print the dealer's concealed card
			dealerHand = playDealer(dealerHand, deck);

			evaluateRound(players, dealerHand);
		}
    }

	private void printInitialDeal(List<Player> players, Player dealer) {
		// Prints the initial deal in the following format:
		/*
		PLAYER 1     2    DEALER
               7    10     4   
               2     A   
	   */
		
        StringBuilder output = new StringBuilder(); 
		output.append("PLAYERS ");
		for (Player player : players) {
			output.append(player.getPlayerNumber() + "\t");
		}
		output.append("DEALER\n");
		//Loop through two rows of cards		
        for (int j = 0; j < 2; j++) {
			output.append("\t");
			for (Player player : players) {
				output.append(player.getHand().get(j).toString()).append("\t");
			}
			if(j == 0 ){
				output.append(dealer.getHand().get(j).toString());
			}
			output.append("\n");
		}
		System.out.print(output);
	}

	/**
	 * Plays the players turn. Prompts the user to hit (H), stay (S), or if
	 * appropriate, split (/) or double down (D), and then performs those
	 * actions. On a hit, prints "RECEIVED A  [x]  HIT? "
	 * 
	 * @param player
	 * @param deck
	 */
	private void play(Player player, Deck deck) {
		// TODO implement play(player, deck)
		// If the player hits, deal another card. If the player stays, return. If the player busts, return.
		// delegate to evaluateHand(hand) to determine whether the player busted.
		// Use promptBoolean and promptInt as examples to start with for prompting actions
		// initially prompt with "PLAYER [x] ?" where x is the player number and accept H, S, D, or /
		// after hitting, prompt "RECEIVED A  [c]  HIT? " where c is the card received and only accept H or S
		// handle splitting and doubling down, or feel free to skip implementing
		// split/double down for now, but leave a todo if that is unfinished
		// after the first pass.
	}

	private int evaluateHand(LinkedList<Card> hand){
		// TODO implement evaluateHand
		//   'int' is maybe the wrong return type. We need to indicate a bust and somehow communicate the ambiguity of aces.
		//   OR maybe we stick with 'int' and use -1 for a bust and otherwise determine the value of aces that gives the highest non-bust score.
		//   but note that we also need a distinction between a natural Blackjack (21 in only 2 cards) and a 21 with more than 2 cards (the natural blackjack wins)
		return 0;
	}

	/**
	 * Play the dealer's hand. The dealer draws until they have >=17 or busts. Prints each draw as in the following example:
	 * 
	 * DEALER HAS A  5 CONCEALED FOR A TOTAL OF 11 
	 * DRAWS 10   ---TOTAL IS 21
	 * 
	 * TODO find out if the dealer draws on a "soft" 17 (17 using an ace as 11) or not in the original basic code.
	 * 
	 * @param dealerHand
	 * @return
	 */
	private LinkedList<Card> playDealer(LinkedList<Card> dealerHand, Deck deck) {
		// TODO implement playDealer
		return null;
	}

	/**
	 * Evaluates the result of the round, prints the results, and updates player/dealer totals.
	 * @param players
	 * @param dealerHand
	 */
	private void evaluateRound(List<Player> players, LinkedList<Card> dealerHand) {
		// TODO implement evaluateRound
		//   print something like:
		/*
		PLAYER 1 LOSES   100 TOTAL=-100 
		PLAYER 2  WINS   150 TOTAL= 150
		DEALER'S TOTAL= 200
		*/
		// this should probably take in a "Dealer" instance instead of just the dealer hand so we can update the dealer's total.
		// currentBets of each player are added/subtracted from the dealer total depending on whether they win/lose (accounting for doubling down, insurance etc.)
		// remember to handle a "PUSH" when the dealer ties and the bet is returned.
	}

	/**
	 * Validates that all bets are between 1 and 500 (inclusive).
	 * 
	 * @param bets The array of bets for each player.
	 * @return true if all bets are valid, false otherwise.
	 */
	public boolean betsAreValid(int[] bets) {
		return Arrays.stream(bets)
			.allMatch(bet -> bet >= 1 && bet <= 500);
	}

}
