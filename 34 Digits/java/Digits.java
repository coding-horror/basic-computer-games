import java.util.Arrays;
import java.util.InputMismatchException;
import java.util.Scanner;

/**
 * DIGITS
 * <p>
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 */
public class Digits {

	public static void main(String[] args) {
		printIntro();
		Scanner scan = new Scanner(System.in);

		boolean showInstructions = readInstructionChoice(scan);
		if (showInstructions) {
			printInstructions();
		}

		int a = 0, b = 1, c = 3;
		int[][] m = new int[27][3];
		int[][] k = new int[3][3];
		int[][] l = new int[9][3];

		boolean continueGame = true;
		while (continueGame) {
			for (int[] ints : m) {
				Arrays.fill(ints, 1);
			}
			for (int[] ints : k) {
				Arrays.fill(ints, 9);
			}
			for (int[] ints : l) {
				Arrays.fill(ints, 3);
			}

			l[0][0] = 2;
			l[4][1] = 2;
			l[8][2] = 2;

			int z = 26, z1 = 8, z2 = 2, runningCorrect = 0;

			for (int t = 1; t <= 3; t++) {
				boolean validNumbers = false;
				int[] numbers = new int[0];
				while (!validNumbers) {
					System.out.println();
					numbers = read10Numbers(scan);
					validNumbers = true;
					for (int number : numbers) {
						if (number < 0 || number > 2) {
							System.out.println("ONLY USE THE DIGITS '0', '1', OR '2'.");
							System.out.println("LET'S TRY AGAIN.");
							validNumbers = false;
							break;
						}
					}
				}

				System.out.printf("\n%-14s%-14s%-14s%-14s", "MY GUESS", "YOUR NO.", "RESULT", "NO. RIGHT");
				for (int number : numbers) {
					int s = 0;
					int myGuess = 0;
					for (int j = 0; j <= 2; j++) {
						//What did the original author have in mind ? The first expression always results in 0 because a is always 0
						int s1 = a * k[z2][j] + b * l[z1][j] + c * m[z][j];
						if (s < s1) {
							s = s1;
							myGuess = j;
						} else if (s1 == s) {
							if (Math.random() >= 0.5) {
								myGuess = j;
							}
						}
					}

					String result;
					if (myGuess != number) {
						result = "WRONG";
					} else {
						runningCorrect++;
						result = "RIGHT";
						m[z][number] = m[z][number] + 1;
						l[z1][number] = l[z1][number] + 1;
						k[z2][number] = k[z2][number] + 1;
						z = z - (z / 9) * 9;
						z = 3 * z + number;
					}
					System.out.printf("\n%-14d%-14d%-14s%-14d", myGuess, number, result, runningCorrect);

					z1 = z - (z / 9) * 9;
					z2 = number;
				}
			}

			//print summary report
			System.out.println();
			if (runningCorrect > 10) {
				System.out.println();
				System.out.println("I GUESSED MORE THAN 1/3 OF YOUR NUMBERS.");
				System.out.println("I WIN.\u0007");
			} else if (runningCorrect < 10) {
				System.out.println("I GUESSED LESS THAN 1/3 OF YOUR NUMBERS.");
				System.out.println("YOU BEAT ME.  CONGRATULATIONS *****");
			} else {
				System.out.println("I GUESSED EXACTLY 1/3 OF YOUR NUMBERS.");
				System.out.println("IT'S A TIE GAME.");
			}

			continueGame = readContinueChoice(scan);
		}

		System.out.println("\nTHANKS FOR THE GAME.");
	}

	private static boolean readContinueChoice(Scanner scan) {
		System.out.print("\nDO YOU WANT TO TRY AGAIN (1 FOR YES, 0 FOR NO) ? ");
		int choice;
		try {
			choice = scan.nextInt();
			return choice == 1;
		} catch (InputMismatchException ex) {
			return false;
		} finally {
			scan.nextLine();
		}
	}

	private static int[] read10Numbers(Scanner scan) {
		System.out.print("TEN NUMBERS, PLEASE ? ");
		int[] numbers = new int[10];

		for (int i = 0; i < numbers.length; i++) {
			boolean validInput = false;
			while (!validInput) {
				try {
					int n = scan.nextInt();
					validInput = true;
					numbers[i] = n;
				} catch (InputMismatchException ex) {
					System.out.println("!NUMBER EXPECTED - RETRY INPUT LINE");
				} finally {
					scan.nextLine();
				}
			}
		}

		return numbers;
	}

	private static void printInstructions() {
		System.out.println("\n");
		System.out.println("PLEASE TAKE A PIECE OF PAPER AND WRITE DOWN");
		System.out.println("THE DIGITS '0', '1', OR '2' THIRTY TIMES AT RANDOM.");
		System.out.println("ARRANGE THEM IN THREE LINES OF TEN DIGITS EACH.");
		System.out.println("I WILL ASK FOR THEN TEN AT A TIME.");
		System.out.println("I WILL ALWAYS GUESS THEM FIRST AND THEN LOOK AT YOUR");
		System.out.println("NEXT NUMBER TO SEE IF I WAS RIGHT. BY PURE LUCK,");
		System.out.println("I OUGHT TO BE RIGHT TEN TIMES. BUT I HOPE TO DO BETTER");
		System.out.println("THAN THAT *****");
		System.out.println();
	}

	private static boolean readInstructionChoice(Scanner scan) {
		System.out.print("FOR INSTRUCTIONS, TYPE '1', ELSE TYPE '0' ? ");
		int choice;
		try {
			choice = scan.nextInt();
			return choice == 1;
		} catch (InputMismatchException ex) {
			return false;
		} finally {
			scan.nextLine();
		}
	}

	private static void printIntro() {
		System.out.println("                                DIGITS");
		System.out.println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
		System.out.println("\n\n");
		System.out.println("THIS IS A GAME OF GUESSING.");
	}

}

