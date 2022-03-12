import java.util.ArrayList;
import java.util.Collection;
import java.util.LinkedList;
import java.util.List;
import java.text.DecimalFormat;

/**
 * This is the primary class that runs the game itself.
 */
public class Game {
    
    private Deck deck;
    private UserIo userIo;

    public Game(Deck deck, UserIo userIo) {
        this.deck = deck;
        this.userIo = userIo;
    }

	/**
	 * Run the game, running rounds until ended with CTRL+D/CTRL+Z or CTRL+C
	 */
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
			while(!betsAreValid(players)){
				userIo.println("BETS:");
				for(int i = 0; i < nPlayers; i++) {
					double bet = userIo.promptDouble("#" + (i + 1)); // 1st player is "Player 1" not "Player 0"
					players.get(i).setCurrentBet(bet);
				}
			}

			// It doesn't *really* matter whether we deal two cards at once to each player
			// or one card to each and then a second card to each, but this technically
			// mimics the way a deal works in real life.
			for(int i = 0; i < 2; i++){
				for(Player player : players){
					player.dealCard(deck.deal());
				}
			}

			// Consider adding a Dealer class to track the dealer's hand and running total.
			// Alternately, the dealer could just be a Player instance where currentBet=0 and is ignored.
			LinkedList<Card> dealerHand = new LinkedList<>();
			Player dealer = new Player(0); //Dealer is Player 0 - this can be converted into a dealer class later on
			dealer.dealCard(deck.deal());
			dealer.dealCard(deck.deal());

			printInitialDeal(players, dealer);

			if(dealer.getHand().get(0).getValue() == 1) {
				collectInsurance(players);
			}

			if(ScoringUtils.scoreHand(dealer.getHand()) == 21) {
				userIo.println("DEALER HAS " + dealer.getHand().get(1).toProseString() + " IN THE HOLE");
				userIo.println("FOR BLACKJACK");
			} else {
				Card dealerFirstCard = dealer.getHand().get(0);
				if(dealerFirstCard.getValue() == 1 || dealerFirstCard.getValue() > 9) {
					userIo.println("");
					userIo.println("NO DEALER BLACKJACK.");
				} // else dealer blackjack is imposible
				for(Player player : players){
					play(player);
				}

				if(shouldPlayDealer(players)){
					playDealer(dealer);
				} else {
					userIo.println("DEALER HAD " + dealer.getHand().get(1).toProseString() + " CONCEALED.");
				}
			}

			evaluateRound(players, dealer);
		} 
    }

	protected void collectInsurance(Iterable<Player> players) {
		boolean isInsurance = userIo.promptBoolean("ANY INSURANCE");
		if(isInsurance) {
			userIo.println("INSURANCE BETS");
			for(Player player : players) {
				while(true) {
					double insuranceBet = userIo.promptDouble("# " + player.getPlayerNumber() + " ");
					// 0 indicates no insurance for that player.
					if(insuranceBet >= 0 && insuranceBet <= (player.getCurrentBet() / 2)) {
						player.setInsuranceBet(insuranceBet);
						break;
					}
				}
			}
		}
	}

	/**
	 * Print the cards for each player and the up card for the dealer.
	 */
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
		userIo.print(output.toString());
	}

	/**
	 * Plays the players turn. Prompts the user to hit (H), stay (S), or if
	 * appropriate, split (/) or double down (D), and then performs those
	 * actions. On a hit, prints "RECEIVED A  [x]  HIT? "
	 * 
	 * @param player
	 */
	protected void play(Player player) {
		play(player, 1);
	}

	private void play(Player player, int handNumber) {
		String action;
		if(player.isSplit()){
			action = userIo.prompt("HAND " + handNumber);
		} else {
			action = userIo.prompt("PLAYER " + player.getPlayerNumber() + " ");
		}
		while(true){
			if(action.equalsIgnoreCase("H")){ // HIT
				Card c = deck.deal();
				player.dealCard(c, handNumber);
				if(ScoringUtils.scoreHand(player.getHand(handNumber)) > 21){
					userIo.println("RECEIVED " + c.toProseString() + "  ...BUSTED");
					break;
				}
				action = userIo.prompt("RECEIVED " + c.toProseString() + " HIT");
			} else if(action.equalsIgnoreCase("S")){ // STAY
				break;
			} else if(action.equalsIgnoreCase("D") && player.canDoubleDown(handNumber)) { // DOUBLE DOWN
				Card c = deck.deal();
				player.doubleDown(c, handNumber);
				if(ScoringUtils.scoreHand(player.getHand(handNumber)) > 21){
					userIo.println("RECEIVED " + c.toProseString() + "  ...BUSTED");
					break;
				}
				userIo.println("RECEIVED " + c.toProseString());
				break;
			} else if(action.equalsIgnoreCase("/")) { // SPLIT
				if(player.isSplit()) {
					// The original basic code printed different output
					// if a player tries to split twice vs if they try to split
					// a non-pair hand.
					action = userIo.prompt("TYPE H, S OR D, PLEASE");
				} else if(player.canSplit()) {
					player.split();
					Card card = deck.deal();
					player.dealCard(card, 1);
					userIo.println("FIRST HAND RECEIVES " + card.toProseString());
					card = deck.deal();
					player.dealCard(card, 2);
					userIo.println("SECOND HAND RECEIVES " + card.toProseString());					
					if(player.getHand().get(0).getValue() > 1){ //Can't play after splitting aces
						play(player, 1);
						play(player, 2);
					}
					return; // Don't fall out of the while loop and print another total
				} else {
					userIo.println("SPLITTING NOT ALLOWED");
					action = userIo.prompt("PLAYER " + player.getPlayerNumber() + " ");
				}
			} else {
				if(player.getHand(handNumber).size() == 2) {
					action = userIo.prompt("TYPE H,S,D, OR /, PLEASE");
				} else {
					action = userIo.prompt("TYPE H, OR S, PLEASE");
				}
			}
		}
		int total = ScoringUtils.scoreHand(player.getHand(handNumber));
		if(total == 21) {
			userIo.println("BLACKJACK");
		} else {
			userIo.println("TOTAL IS " + total);
		}
	}

	/**
	 * Check the Dealer's hand should be played out. If every player has either busted or won with natural Blackjack,
	 * the Dealer doesn't need to play.
	 * 
	 * @param players
	 * @return boolean whether the dealer should play
	 */

	protected boolean shouldPlayDealer(List<Player> players){
		for(Player player : players){
			int score = ScoringUtils.scoreHand(player.getHand());
			if(score < 21 || (score == 21 && player.getHand().size() > 2)){
				return true;
			}
			if(player.isSplit()){				
				int splitScore = ScoringUtils.scoreHand(player.getHand(2));
				if(splitScore < 21 || (splitScore == 21 && player.getHand(2).size() > 2)){
					return true;
				}
			}
		}
		return false;
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
	protected void playDealer(Player dealer) {
		int score = ScoringUtils.scoreHand(dealer.getHand());
		userIo.println("DEALER HAS " + dealer.getHand().get(1).toProseString() + " CONCEALED FOR A TOTAL OF " + score);

		if(score < 17){
			userIo.print("DRAWS ");
		}
		while(score < 17) {
			Card dealtCard = deck.deal();
			dealer.dealCard(dealtCard);
			score = ScoringUtils.scoreHand(dealer.getHand());
			userIo.print(dealtCard.toString() + " ");
		}
		
		if(score > 21) {
			userIo.println("...BUSTED\n");
		} else {
			userIo.println("---TOTAL IS " + score + "\n");
		}
	}

	/**
	 * Evaluates the result of the round, prints the results, and updates player/dealer totals.
	 * @param players
	 * @param dealerHand
	 */
	protected void evaluateRound(List<Player> players, Player dealer) {
		/*
		PLAYER 1 LOSES   100 TOTAL=-100 
		PLAYER 2  WINS   150 TOTAL= 150
		DEALER'S TOTAL= 200
		*/
		// this should probably take in a "Dealer" instance instead of just the dealer hand so we can update the dealer's total.
		// currentBets of each player are added/subtracted from the dealer total depending on whether they win/lose (accounting for doubling down, insurance etc.)
		// remember to handle a "PUSH" when the dealer ties and the bet is returned.

		DecimalFormat formatter = new DecimalFormat("0.#"); //Removes trailing zeros
		for(Player player : players){
			int result = ScoringUtils.compareHands(player.getHand(), dealer.getHand());
			double totalBet = 0;
			if(result > 0){
				totalBet += player.getCurrentBet();
			} else if(result < 0){
				totalBet -= player.getCurrentBet();
			}
			if(player.isSplit()) {
				int splitResult = ScoringUtils.compareHands(player.getHand(2), dealer.getHand());
				if(splitResult > 0){
					totalBet += player.getSplitBet();
				} else if(splitResult < 0){
					totalBet -= player.getSplitBet();
				} 
			}
			if(player.getInsuranceBet() != 0){
				int dealerResult = ScoringUtils.scoreHand(dealer.getHand());
				if(dealerResult == 21 && dealer.getHand().size() == 2){
					totalBet += (player.getInsuranceBet() * 2);
				} else {
					totalBet -= player.getInsuranceBet();
				}
			}
			
			userIo.print("PLAYER " + player.getPlayerNumber());
			if(totalBet < 0) {
				userIo.print(" LOSES " + String.format("%6s", formatter.format(Math.abs(totalBet)))); 
			} else if(totalBet > 0) {
				userIo.print("  WINS  " + String.format("%6s", formatter.format(totalBet))); 
			} else {
				userIo.print(" PUSHES      ");
			}
			player.recordRound(totalBet);
			dealer.recordRound(totalBet*-1);
			userIo.println(" TOTAL= " + formatter.format(player.getTotal()));
			player.resetHand();
		}
		userIo.println("DEALER'S TOTAL= " + formatter.format(dealer.getTotal()) + "\n");	
	}

	/**
	 * Validates that all bets are between 0 (exclusive) and 500 (inclusive). Fractional bets are valid.
	 * 
	 * @param players The players with their current bet set.
	 * @return true if all bets are valid, false otherwise.
	 */
	public boolean betsAreValid(Collection<Player> players) {
		return players.stream()
			.map(Player::getCurrentBet)
			.allMatch(bet -> bet > 0 && bet <= 500);
	}

}
