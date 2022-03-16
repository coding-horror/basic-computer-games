import java.util.Arrays;
import java.util.InputMismatchException;
import java.util.Scanner;

/**
 * GOMOKO
 * <p>
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 */
public class Gomoko {

	private static final int MIN_BOARD_SIZE = 7;
	private static final int MAX_BOARD_SIZE = 19;

	public static void main(String[] args) {
		printIntro();
		Scanner scan = new Scanner(System.in);
		int boardSize = readBoardSize(scan);

		boolean continuePlay = true;
		while (continuePlay) {
			int[][] board = new int[boardSize][boardSize];
			//initialize the board elements to 0
			for (int[] ints : board) {
				Arrays.fill(ints, 0);
			}

			System.out.println("\n\nWE ALTERNATE MOVES.  YOU GO FIRST...");

			boolean doneRound = false;
			while (!doneRound) {
				Move playerMove = null;
				boolean validMove = false;
				while (!validMove) {
					playerMove = readMove(scan);
					if (playerMove.i == -1 || playerMove.j == -1) {
						doneRound = true;
						System.out.println("\nTHANKS FOR THE GAME!!");
						System.out.print("PLAY AGAIN (1 FOR YES, 0 FOR NO)? ");
						final int playAgain = scan.nextInt();
						scan.nextLine();
						if (playAgain == 1) {
							continuePlay = true;
							break;
						} else {
							continuePlay = false;
							break;
						}
					} else if (!isLegalMove(playerMove, boardSize)) {
						System.out.println("ILLEGAL MOVE.  TRY AGAIN...");
					} else if (board[playerMove.i - 1][playerMove.j - 1] != 0) {
						System.out.println("SQUARE OCCUPIED.  TRY AGAIN...");
					} else {
						validMove = true;
					}
				}

				if (!doneRound) {
					board[playerMove.i - 1][playerMove.j - 1] = 1;
					Move computerMove = getComputerMove(playerMove, board, boardSize);
					if (computerMove == null) {
						computerMove = getRandomMove(board, boardSize);
					}
					board[computerMove.i - 1][computerMove.j - 1] = 2;

					printBoard(board);
				}
			}

		}
	}

	//*** COMPUTER TRIES AN INTELLIGENT MOVE ***
	private static Move getComputerMove(Move playerMove, int[][] board, int boardSize) {
		for (int e = -1; e <= 1; e++) {
			for (int f = -1; f <= 1; f++) {
				if ((e + f - e * f) != 0) {
					var x = playerMove.i + f;
					var y = playerMove.j + f;
					final Move newMove = new Move(x, y);
					if (isLegalMove(newMove, boardSize)) {
						if (board[newMove.i - 1][newMove.j - 1] != 0) {
							newMove.i = newMove.i - e;
							newMove.i = newMove.j - f;
							if (!isLegalMove(newMove, boardSize)) {
								return null;
							} else {
								if (board[newMove.i - 1][newMove.j - 1] == 0) {
									return newMove;
								}
							}
						}
					}
				}
			}
		}
		return null;
	}

	private static void printBoard(int[][] board) {
		for (int[] ints : board) {
			for (int cell : ints) {
				System.out.printf(" %s", cell);
			}
			System.out.println();
		}
	}

	//*** COMPUTER TRIES A RANDOM MOVE ***
	private static Move getRandomMove(int[][] board, int boardSize) {
		boolean legalMove = false;
		Move randomMove = null;
		while (!legalMove) {
			randomMove = randomMove(boardSize);
			legalMove = isLegalMove(randomMove, boardSize) && board[randomMove.i - 1][randomMove.j - 1] == 0;

		}
		return randomMove;
	}

	private static Move randomMove(int boardSize) {
		int x = (int) (boardSize * Math.random() + 1);
		int y = (int) (boardSize * Math.random() + 1);
		return new Move(x, y);
	}

	private static boolean isLegalMove(Move move, int boardSize) {
		return (move.i >= 1) && (move.i <= boardSize) && (move.j >= 1) && (move.j <= boardSize);
	}

	private static void printIntro() {
		System.out.println("                                GOMOKO");
		System.out.println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
		System.out.println("\n\n");
		System.out.println("WELCOME TO THE ORIENTAL GAME OF GOMOKO.");
		System.out.println("\n");
		System.out.println("THE GAME IS PLAYED ON AN N BY N GRID OF A SIZE");
		System.out.println("THAT YOU SPECIFY.  DURING YOUR PLAY, YOU MAY COVER ONE GRID");
		System.out.println("INTERSECTION WITH A MARKER. THE OBJECT OF THE GAME IS TO GET");
		System.out.println("5 ADJACENT MARKERS IN A ROW -- HORIZONTALLY, VERTICALLY, OR");
		System.out.println("DIAGONALLY.  ON THE BOARD DIAGRAM, YOUR MOVES ARE MARKED");
		System.out.println("WITH A '1' AND THE COMPUTER MOVES WITH A '2'.");
		System.out.println("\nTHE COMPUTER DOES NOT KEEP TRACK OF WHO HAS WON.");
		System.out.println("TO END THE GAME, TYPE -1,-1 FOR YOUR MOVE.\n ");
	}

	private static int readBoardSize(Scanner scan) {
		System.out.print("WHAT IS YOUR BOARD SIZE (MIN 7/ MAX 19)? ");

		boolean validInput = false;
		int input = 0;
		while (!validInput) {
			try {
				input = scan.nextInt();
				if (input < MIN_BOARD_SIZE || input > MAX_BOARD_SIZE) {
					System.out.printf("I SAID, THE MINIMUM IS %s, THE MAXIMUM IS %s.\n", MIN_BOARD_SIZE, MAX_BOARD_SIZE);
				} else {
					validInput = true;
				}
			} catch (InputMismatchException ex) {
				System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE\n");
				validInput = false;
			} finally {
				scan.nextLine();
			}
		}
		return input;
	}

	private static Move readMove(Scanner scan) {
		System.out.print("YOUR PLAY (I,J)? ");
		boolean validInput = false;
		Move move = new Move();
		while (!validInput) {
			String input = scan.nextLine();
			final String[] split = input.split(",");
			try {
				move.i = Integer.parseInt(split[0]);
				move.j = Integer.parseInt(split[1]);
				validInput = true;
			} catch (NumberFormatException nfe) {
				System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE\n? ");
			}

		}
		return move;
	}

	private static class Move {
		int i;
		int j;

		public Move() {
		}

		public Move(int i, int j) {
			this.i = i;
			this.j = j;
		}

		@Override
		public String toString() {
			return "Move{" +
					"i=" + i +
					", j=" + j +
					'}';
		}
	}

}
