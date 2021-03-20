import java.util.Scanner;

public class Number {

	public static void main(String[] args) {
		printIntro();
		int points = 100; //start with 100 points for the user

		Scanner scan = new Scanner(System.in);
		boolean done = false;
		while (!done) {
			System.out.print("GUESS A NUMBER FROM 1 TO 5? ");
			int g = scan.nextInt();

			//Initialize 5 random numbers between 1-5
			var r = randomNumber(1);
			var s = randomNumber(1);
			var t = randomNumber(1);
			var u = randomNumber(1);
			var v = randomNumber(1);

			if (r == g) {
				points -= 5;
			} else if (s == g) {
				points += 5;
			} else if (t == g) {
				points += points;
			} else if (u == g) {
				points += 1;
			} else if (v == g) {
				points -= points * 0.5;
			} else {
				continue; //Doesn't match any of our random numbers, so just ask for another guess
			}

			if (points > 500) {
				done = true;
			} else {
				System.out.println("YOU HAVE " + points + " POINTS.");
			}
		}

		System.out.println("!!!!YOU WIN!!!! WITH " + points + " POINTS.\n");
	}

	private static int randomNumber(int x) {
		//Note: 'x' is totally ignored as was in the original basic listing
		return (int) (5 * Math.random() + 1);
	}

	private static void printIntro() {
		System.out.println("                                NUMBER");
		System.out.println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
		System.out.println("\n\n\n");
		System.out.println("YOU HAVE 100 POINTS.  BY GUESSING NUMBERS FROM 1 TO 5, YOU");
		System.out.println("CAN GAIN OR LOSE POINTS DEPENDING UPON HOW CLOSE YOU GET TO");
		System.out.println("A RANDOM NUMBER SELECTED BY THE COMPUTER.");
		System.out.println("\n");
		System.out.println("YOU OCCASIONALLY WILL GET A JACKPOT WHICH WILL DOUBLE(!)");
		System.out.println("YOUR POINT COUNT.  YOU WIN WHEN YOU GET 500 POINTS.");
	}
}
