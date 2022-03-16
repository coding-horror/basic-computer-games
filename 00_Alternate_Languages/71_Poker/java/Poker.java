import java.util.Random;
import java.util.Scanner;

import static java.lang.System.out;

/**
 * Port of CREATIVE COMPUTING Poker written in Commodore 64 Basic to plain Java
 *
 * Original source scanned from magazine: https://www.atariarchives.org/basicgames/showpage.php?page=129
 *
 * I based my port on the OCR'ed source code here: https://github.com/coding-horror/basic-computer-games/blob/main/71_Poker/poker.bas
 *
 * Why? Because I remember typing this into my C64 when I was a tiny little developer and having great fun playing it!
 *
 * Goal: Keep the algorithms and UX more or less as-is; Improve the control flow a bit (no goto in Java!) and rename some stuff to be easier to follow.
 *
 * Result: There are probably bugs, please let me know.
 */
public class Poker {

	public static void main(String[] args) {
		new Poker().run();
	}

	float[] cards = new float[50]; // Index 1-5 = Human hand, index 6-10 = Computer hand
	float[] B = new float[15];

	float playerValuables = 1;
	float computerMoney = 200;
	float humanMoney = 200;
	float pot = 0;

	String J$ = "";
	float computerHandValue = 0;

	int K = 0;
	float G = 0;
	float T = 0;
	int M = 0;
	int D = 0;

	int U = 0;
	float N = 1;

	float I = 0;

	float X = 0;

	int Z = 0;

	String handDescription = "";

	float V;

	void run() {
		printWelcome();
		playRound();
		startAgain();
	}

	void printWelcome() {
		tab(33);
		out.println("POKER");
		tab(15);
		out.print("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
		out.println();
		out.println();
		out.println();
		out.println("WELCOME TO THE CASINO.  WE EACH HAVE $200.");
		out.println("I WILL OPEN THE BETTING BEFORE THE DRAW; YOU OPEN AFTER.");
		out.println("TO FOLD BET 0; TO CHECK BET .5.");
		out.println("ENOUGH TALK -- LET'S GET DOWN TO BUSINESS.");
		out.println();
	}

	void tab(int number) {
		System.out.print("\t".repeat(number));
	}

	int random0to10() {
		return new Random().nextInt(10);
	}

	int removeHundreds(long x) {
		return _int(x - (100F * _int(x / 100F)));
	}

	void startAgain() {
		pot = 0;
		playRound();
	}

	void playRound() {
		if (computerMoney <= 5) {
			computerBroke();
		}

		out.println("THE ANTE IS $5.  I WILL DEAL:");
		out.println();

		if (humanMoney <= 5) {
			playerBroke();
		}

		pot = pot + 10;
		humanMoney = humanMoney - 5;
		computerMoney = computerMoney - 5;
		for (int Z = 1; Z < 10; Z++) {
			generateCards(Z);
		}
		out.println("YOUR HAND:");
		N = 1;
		showHand();
		N = 6;
		I = 2;

		describeHand();

		out.println();

		if (I != 6) {
			if (U >= 13) {
				if (U <= 16) {
					Z = 35;
				} else {
					Z = 2;
					if (random0to10() < 1) {
						Z = 35;
					}
				}
				computerOpens();
				playerMoves();
			} else if (random0to10() >= 2) {
				computerChecks();
			} else {
				I = 7;
				Z = 23;
				computerOpens();
				playerMoves();
			}
		} else if (random0to10() <= 7) {
			if (random0to10() <= 7) {
				if (random0to10() >= 1) {
					Z = 1;
					K = 0;
					out.print("I CHECK. ");
					playerMoves();
				} else {
					X = 11111;
					I = 7;
					Z = 23;
					computerOpens();
					playerMoves();
				}
			} else {
				X = 11110;
				I = 7;
				Z = 23;
				computerOpens();
				playerMoves();
			}
		} else {
			X = 11100;
			I = 7;
			Z = 23;
			computerOpens();
			playerMoves();
		}
	}

	void playerMoves() {
		playersTurn();
		checkWinnerAfterFirstBet();
		promptPlayerDrawCards();
	}

	void computerOpens() {
		V = Z + random0to10();
		computerMoves();
		out.print("I'LL OPEN WITH $" + V);
		K = _int(V);
	}

	@SuppressWarnings("StatementWithEmptyBody")
	void computerMoves() {
		if (computerMoney - G - V >= 0) {
		} else if (G != 0) {
			if (computerMoney - G >= 0) {
				computerSees();
			} else {
				computerBroke();
			}
		} else {
			V = computerMoney;
		}
	}

	void promptPlayerDrawCards() {
		out.println();
		out.println("NOW WE DRAW -- HOW MANY CARDS DO YOU WANT");
		inputPlayerDrawCards();
	}

	void inputPlayerDrawCards() {
		T = Integer.parseInt(readString());
		if (T == 0) {
			computerDrawing();
		} else {
			Z = 10;
			if (T < 4) {
				playerDrawsCards();
			} else {
				out.println("YOU CAN'T DRAW MORE THAN THREE CARDS.");
				inputPlayerDrawCards();
			}
		}
	}

	// line # 980
	void computerDrawing() {
		Z = _int(10 + T);
		for (U = 6; U <= 10; U++) {
			if (_int((float) (X / Math.pow(10F, (U - 6F)))) == (10 * (_int((float) (X / Math.pow(10, (U - 5))))))) {
				drawNextCard();
			}
		}
		out.print("I AM TAKING " + _int(Z - 10 - T) + " CARD");
		if (Z == 11 + T) {
			out.println();
		} else {
			out.println("S");
		}

		N = 6;
		V = I;
		I = 1;
		describeHand();
		startPlayerBettingAndReaction();
	}

	void drawNextCard() {
		Z = Z + 1;
		drawCard();
	}

	@SuppressWarnings("StatementWithEmptyBody")
	void drawCard() {
		cards[Z] = 100 * new Random().nextInt(4) + new Random().nextInt(100);
		if (_int(cards[Z] / 100) > 3) {
			drawCard();
		} else if (cards[Z] - 100 * _int(cards[Z] / 100) > 12) {
			drawCard();
		} else if (Z == 1) {
		} else {
			for (K = 1; K <= Z - 1; K++) {
				if (cards[Z] == cards[K]) {
					drawCard();
				}
			}
			if (Z <= 10) {
			} else {
				N = cards[U];
				cards[U] = cards[Z];
				cards[Z] = N;
			}
		}
	}

	void playerDrawsCards() {
		out.println("WHAT ARE THEIR NUMBERS:");
		for (int Q = 1; Q <= T; Q++) {
			U = Integer.parseInt(readString());
			drawNextCard();
		}

		out.println("YOUR NEW HAND:");
		N = 1;
		showHand();
		computerDrawing();
	}

	void startPlayerBettingAndReaction() {
		computerHandValue = U;
		M = D;

		if (V != 7) {
			if (I != 6) {
				if (U >= 13) {
					if (U >= 16) {
						Z = 2;
						playerBetsAndComputerReacts();
					} else {
						Z = 19;
						if (random0to10() == 8) {
							Z = 11;
						}
						playerBetsAndComputerReacts();
					}
				} else {
					Z = 2;
					if (random0to10() == 6) {
						Z = 19;
					}
					playerBetsAndComputerReacts();
				}
			} else {
				Z = 1;
				playerBetsAndComputerReacts();
			}
		} else {
			Z = 28;
			playerBetsAndComputerReacts();
		}
	}

	void playerBetsAndComputerReacts() {
		K = 0;
		playersTurn();
		if (T != .5) {
			checkWinnerAfterFirstBetAndCompareHands();
		} else if (V == 7 || I != 6) {
			computerOpens();
			promptAndInputPlayerBet();
			checkWinnerAfterFirstBetAndCompareHands();
		} else {
			out.println("I'LL CHECK");
			compareHands();
		}
	}

	void checkWinnerAfterFirstBetAndCompareHands() {
		checkWinnerAfterFirstBet();
		compareHands();
	}

	void compareHands() {
		out.println("NOW WE COMPARE HANDS:");
		J$ = handDescription;
		out.println("MY HAND:");
		N = 6;
		showHand();
		N = 1;
		describeHand();
		out.print("YOU HAVE ");
		K = D;
		printHandDescriptionResult();
		handDescription = J$;
		K = M;
		out.print(" AND I HAVE ");
		printHandDescriptionResult();
		out.print(". ");
		if (computerHandValue > U) {
			computerWins();
		} else if (U > computerHandValue) {
			humanWins();
		} else if (handDescription.contains("A FLUS")) {
			someoneWinsWithFlush();
		} else if (removeHundreds(M) < removeHundreds(D)) {
			humanWins();
		} else if (removeHundreds(M) > removeHundreds(D)) {
			computerWins();
		} else {
			handIsDrawn();
		}
	}

	void printHandDescriptionResult() {
		out.print(handDescription);
		if (!handDescription.contains("A FLUS")) {
			K = removeHundreds(K);
			printCardValue();
			if (handDescription.contains("SCHMAL")) {
				out.print(" HIGH");
			} else if (!handDescription.contains("STRAIG")) {
				out.print("'S");
			} else {
				out.print(" HIGH");
			}
		} else {
			K = K / 100;
			printCardColor();
			out.println();
		}
	}

	void handIsDrawn() {
		out.print("THE HAND IS DRAWN.");
		out.print("ALL $" + pot + " REMAINS IN THE POT.");
		playRound();
	}

	void someoneWinsWithFlush() {
		if (removeHundreds(M) > removeHundreds(D)) {
			computerWins();
		} else if (removeHundreds(D) > removeHundreds(M)) {
			humanWins();
		} else {
			handIsDrawn();
		}
	}

	@SuppressWarnings("StatementWithEmptyBody")
	void checkWinnerAfterFirstBet() {
		if (I != 3) {
			if (I != 4) {
			} else {
				humanWins();
			}
		} else {
			out.println();
			computerWins();
		}
	}

	void computerWins() {
		out.print(". I WIN. ");
		computerMoney = computerMoney + pot;
		potStatusAndNextRoundPrompt();
	}

	void potStatusAndNextRoundPrompt() {
		out.println("NOW I HAVE $" + computerMoney + " AND YOU HAVE $" + humanMoney);
		out.print("DO YOU WISH TO CONTINUE");

		if (yesFromPrompt()) {
			startAgain();
		} else {
			System.exit(0);
		}
	}

	private boolean yesFromPrompt() {
		String h = readString();
		if (h != null) {
			if (h.toLowerCase().matches("y|yes|yep|affirmative|yay")) {
				return true;
			} else if (h.toLowerCase().matches("n|no|nope|fuck off|nay")) {
				return false;
			}
		}
		out.println("ANSWER YES OR NO, PLEASE.");
		return yesFromPrompt();
	}

	void computerChecks() {
		Z = 0;
		K = 0;
		out.print("I CHECK. ");
		playerMoves();
	}

	void humanWins() {
		out.println("YOU WIN.");
		humanMoney = humanMoney + pot;
		potStatusAndNextRoundPrompt();
	}

	// line # 1740
	void generateCards(int Z) {
		cards[Z] = (100 * new Random().nextInt(4)) + new Random().nextInt(100);
		if (_int(cards[Z] / 100) > 3) {
			generateCards(Z);
			return;
		}
		if (cards[Z] - 100 * (_int(cards[Z] / 100)) > 12) {
			generateCards(Z);
			return;
		}
		if (Z == 1) {return;}
		for (int K = 1; K <= Z - 1; K++) {// TO Z-1
			if (cards[Z] == cards[K]) {
				generateCards(Z);
				return;
			}
		}
		if (Z <= 10) {return;}
		float N = cards[U];
		cards[U] = cards[Z];
		cards[Z] = N;
	}

	// line # 1850
	void showHand() {
		for (int cardNumber = _int(N); cardNumber <= N + 4; cardNumber++) {
			out.print(cardNumber + "--  ");
			printCardValueAtIndex(cardNumber);
			out.print(" OF");
			printCardColorAtIndex(cardNumber);
			if (cardNumber / 2 == (cardNumber / 2)) {
				out.println();
			}
		}
	}

	// line # 1950
	void printCardValueAtIndex(int Z) {
		K = removeHundreds(_int(cards[Z]));
		printCardValue();
	}

	void printCardValue() {
		if (K == 9) {
			out.print("JACK");
		} else if (K == 10) {
			out.print("QUEEN");
		} else if (K == 11) {
			out.print("KING");
		} else if (K == 12) {
			out.print("ACE");
		} else if (K < 9) {
			out.print(K + 2);
		}
	}

	// line # 2070
	void printCardColorAtIndex(int Z) {
		K = _int(cards[Z] / 100);
		printCardColor();
	}

	void printCardColor() {
		if (K == 0) {
			out.print(" CLUBS");
		} else if (K == 1) {
			out.print(" DIAMONDS");
		} else if (K == 2) {
			out.print(" HEARTS");
		} else if (K == 3) {
			out.print(" SPADES");
		}
	}

	// line # 2170
	void describeHand() {
		U = 0;
		for (Z = _int(N); Z <= N + 4; Z++) {
			B[Z] = removeHundreds(_int(cards[Z]));
			if (Z == N + 4) {continue;}
			if (_int(cards[Z] / 100) != _int(cards[Z + 1] / 100)) {continue;}
			U = U + 1;
		}
		if (U != 4) {
			for (Z = _int(N); Z <= N + 3; Z++) {
				for (K = Z + 1; K <= N + 4; K++) {
					if (B[Z] <= B[K]) {continue;}
					X = cards[Z];
					cards[Z] = cards[K];
					B[Z] = B[K];
					cards[K] = X;
					B[K] = cards[K] - 100 * _int(cards[K] / 100);
				}
			}
			X = 0;
			for (Z = _int(N); Z <= N + 3; Z++) {
				if (B[Z] != B[Z + 1]) {continue;}
				X = (float) (X + 11 * Math.pow(10, (Z - N)));
				D = _int(cards[Z]);

				if (U >= 11) {
					if (U != 11) {
						if (U > 12) {
							if (B[Z] != B[Z - 1]) {
								fullHouse();
							} else {
								U = 17;
								handDescription = "FOUR ";
							}
						} else {
							fullHouse();
						}
					} else if (B[Z] != B[Z - 1]) {
						handDescription = "TWO PAIR, ";
						U = 12;
					} else {
						handDescription = "THREE ";
						U = 13;
					}
				} else {
					U = 11;
					handDescription = "A PAIR OF ";
				}
			}

			if (X != 0) {
				schmaltzHand();
			} else {
				if (B[_int(N)] + 3 == B[_int(N + 3)]) {
					X = 1111;
					U = 10;
				}
				if (B[_int(N + 1)] + 3 != B[_int(N + 4)]) {
					schmaltzHand();
				} else if (U != 10) {
					U = 10;
					X = 11110;
					schmaltzHand();
				} else {
					U = 14;
					handDescription = "STRAIGHT";
					X = 11111;
					D = _int(cards[_int(N + 4)]);
				}
			}
		} else {
			X = 11111;
			D = _int(cards[_int(N)]);
			handDescription = "A FLUSH IN";
			U = 15;
		}
	}

	void schmaltzHand() {
		if (U >= 10) {
			if (U != 10) {
				if (U > 12) {return;}
				if (removeHundreds(D) <= 6) {
					I = 6;
				}
			} else {
				if (I == 1) {
					I = 6;
				}
			}
		} else {
			D = _int(cards[_int(N + 4)]);
			handDescription = "SCHMALTZ, ";
			U = 9;
			X = 11000;
			I = 6;
		}
	}

	void fullHouse() {
		U = 16;
		handDescription = "FULL HOUSE, ";
	}

	void playersTurn() {
		G = 0;
		promptAndInputPlayerBet();
	}

	String readString() {
		Scanner sc = new Scanner(System.in);
		return sc.nextLine();
	}

	@SuppressWarnings("StatementWithEmptyBody")
	void promptAndInputPlayerBet() {
		out.println("WHAT IS YOUR BET");
		T = readFloat();
		if (T - _int(T) == 0) {
			processPlayerBet();
		} else if (K != 0) {
			playerBetInvalidAmount();
		} else if (G != 0) {
			playerBetInvalidAmount();
		} else if (T == .5) {
		} else {
			playerBetInvalidAmount();
		}
	}

	private float readFloat() {
		try {
			return Float.parseFloat(readString());
		} catch (Exception ex) {
			System.out.println("INVALID INPUT, PLEASE TYPE A FLOAT. ");
			return readFloat();
		}
	}

	void playerBetInvalidAmount() {
		out.println("NO SMALL CHANGE, PLEASE.");
		promptAndInputPlayerBet();
	}

	void processPlayerBet() {
		if (humanMoney - G - T >= 0) {
			humanCanAffordBet();
		} else {
			playerBroke();
			promptAndInputPlayerBet();
		}
	}

	void humanCanAffordBet() {
		if (T != 0) {
			if (G + T >= K) {
				processComputerMove();
			} else {
				out.println("IF YOU CAN'T SEE MY BET, THEN FOLD.");
				promptAndInputPlayerBet();
			}
		} else {
			I = 3;
			moveMoneyToPot();
		}
	}

	void processComputerMove() {
		G = G + T;
		if (G == K) {
			moveMoneyToPot();
		} else if (Z != 1) {
			if (G > 3 * Z) {
				computerRaisesOrSees();
			} else {
				computerRaises();
			}
		} else if (G > 5) {
			if (T <= 25) {
				computerRaisesOrSees();
			} else {
				computerFolds();
			}
		} else {
			V = 5;
			if (G > 3 * Z) {
				computerRaisesOrSees();
			} else {
				computerRaises();
			}
		}
	}

	void computerRaises() {
		V = G - K + random0to10();
		computerMoves();
		out.println("I'LL SEE YOU, AND RAISE YOU" + V);
		K = _int(G + V);
		promptAndInputPlayerBet();
	}

	void computerFolds() {
		I = 4;
		out.println("I FOLD.");
	}

	void computerRaisesOrSees() {
		if (Z == 2) {
			computerRaises();
		} else {
			computerSees();
		}
	}

	void computerSees() {
		out.println("I'LL SEE YOU.");
		K = _int(G);
		moveMoneyToPot();
	}

	void moveMoneyToPot() {
		humanMoney = humanMoney - G;
		computerMoney = computerMoney - K;
		pot = pot + G + K;
	}

	void computerBusted() {
		out.println("I'M BUSTED.  CONGRATULATIONS!");
		System.exit(0);
	}

	@SuppressWarnings("StatementWithEmptyBody")
	private void computerBroke() {
		if ((playerValuables / 2) == _int(playerValuables / 2) && playerBuyBackWatch()) {
		} else if (playerValuables / 3 == _int(playerValuables / 3) && playerBuyBackTieRack()) {
		} else {
			computerBusted();
		}
	}

	private int _int(float v) {
		return (int) Math.floor(v);
	}

	private boolean playerBuyBackWatch() {
		out.println("WOULD YOU LIKE TO BUY BACK YOUR WATCH FOR $50");
		if (yesFromPrompt()) {
			computerMoney = computerMoney + 50;
			playerValuables = playerValuables / 2;
			return true;
		} else {
			return false;
		}
	}

	private boolean playerBuyBackTieRack() {
		out.println("WOULD YOU LIKE TO BUY BACK YOUR TIE TACK FOR $50");
		if (yesFromPrompt()) {
			computerMoney = computerMoney + 50;
			playerValuables = playerValuables / 3;
			return true;
		} else {
			return false;
		}
	}

	// line # 3830
	@SuppressWarnings("StatementWithEmptyBody")
	void playerBroke() {
		out.println("YOU CAN'T BET WITH WHAT YOU HAVEN'T GOT.");
		if (playerValuables / 2 != _int(playerValuables / 2) && playerSellWatch()) {
		} else if (playerValuables / 3 != _int(playerValuables / 3) && playerSellTieTack()) {
		} else {
			playerBusted();
		}
	}

	private void playerBusted() {
		out.println("YOUR WAD IS SHOT. SO LONG, SUCKER!");
		System.exit(0);
	}

	private boolean playerSellWatch() {
		out.println("WOULD YOU LIKE TO SELL YOUR WATCH");
		if (yesFromPrompt()) {
			if (random0to10() < 7) {
				out.println("I'LL GIVE YOU $75 FOR IT.");
				humanMoney = humanMoney + 75;
			} else {
				out.println("THAT'S A PRETTY CRUMMY WATCH - I'LL GIVE YOU $25.");
				humanMoney = humanMoney + 25;
			}
			playerValuables = playerValuables * 2;
			return true;
		} else {
			return false;
		}
	}

	private boolean playerSellTieTack() {
		out.println("WILL YOU PART WITH THAT DIAMOND TIE TACK");

		if (yesFromPrompt()) {
			if (random0to10() < 6) {
				out.println("YOU ARE NOW $100 RICHER.");
				humanMoney = humanMoney + 100;
			} else {
				out.println("IT'S PASTE.  $25.");
				humanMoney = humanMoney + 25;
			}
			playerValuables = playerValuables * 3;
			return true;
		} else {
			return false;
		}
	}

}
