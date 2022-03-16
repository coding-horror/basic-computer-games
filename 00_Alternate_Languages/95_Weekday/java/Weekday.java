import java.util.Scanner;

/**
 * WEEKDAY
 *
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 *
 */
public class Weekday {

	//TABLE OF VALUES FOR THE MONTHS TO BE USED IN CALCULATIONS.
	//Dummy value added at index 0, so we can reference directly by the month number value
	private final static int[] t = new int[]{-1, 0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5};

	public static void main(String[] args) {
		printIntro();

		Scanner scanner = new Scanner(System.in);
		System.out.print("ENTER TODAY'S DATE IN THE FORM: 3,24,1979 ");
		DateStruct todaysDate = readDate(scanner);

		System.out.print("ENTER DAY OF BIRTH (OR OTHER DAY OF INTEREST) ");
		DateStruct dateOfInterest = readDate(scanner);

		int I1 = (dateOfInterest.year - 1500) / 100;
		//TEST FOR DATE BEFORE CURRENT CALENDAR.
		if ((dateOfInterest.year - 1582) >= 0) {
			int A = I1 * 5 + (I1 + 3) / 4;
			int I2 = (A - b(A) * 7);
			int Y2 = (dateOfInterest.year / 100);
			int Y3 = (dateOfInterest.year - Y2 * 100);
			A = Y3 / 4 + Y3 + dateOfInterest.day + t[dateOfInterest.month] + I2;
			calculateAndPrintDayOfWeek(I1, A, todaysDate, dateOfInterest, Y3);

			if ((todaysDate.year * 12 + todaysDate.month) * 31 + todaysDate.day
					== (dateOfInterest.year * 12 + dateOfInterest.month) * 31 + dateOfInterest.day) {
				return; //stop the program
			}

			int I5 = todaysDate.year - dateOfInterest.year;
			System.out.print("\n");
			int I6 = todaysDate.month - dateOfInterest.month;
			int I7 = todaysDate.day - dateOfInterest.day;
			if (I7 < 0) {
				I6 = I6 - 1;
				I7 = I7 + 30;
			}
			if (I6 < 0) {
				I5 = I5 - 1;
				I6 = I6 + 12;
			}
			if (I5 < 0) {
				return; //do nothing. end the program
			} else {
				if (I7 != 0) {
					printHeadersAndAge(I5, I6, I7);
				} else {
					if (I6 != 0) {
						printHeadersAndAge(I5, I6, I7);
					} else {
						System.out.println("***HAPPY BIRTHDAY***");
						printHeadersAndAge(I5, I6, I7);
					}
				}
			}

			int A8 = (I5 * 365) + (I6 * 30) + I7 + (I6 / 2);
			int K5 = I5;
			int K6 = I6;
			int K7 = I7;
			//CALCULATE RETIREMENT DATE.
			int E = dateOfInterest.year + 65;
			// CALCULATE TIME SPENT IN THE FOLLOWING FUNCTIONS.
			float F = 0.35f;
			System.out.printf("%-28s", "YOU HAVE SLEPT");
			DateStruct scratchDate = new DateStruct(K6, K7, K5); //K5 is a temp year, K6 is month, K7 is day
			printStatisticRow(F, A8, scratchDate);
			K5 = scratchDate.year;
			K6 = scratchDate.month;
			K7 = scratchDate.day;

			F = 0.17f;
			System.out.printf("%-28s", "YOU HAVE EATEN");

			scratchDate = new DateStruct(K6, K7, K5);
			printStatisticRow(F, A8, scratchDate);
			K5 = scratchDate.year;
			K6 = scratchDate.month;
			K7 = scratchDate.day;

			F = 0.23f;
			if (K5 > 3) {
				if (K5 > 9) {
					System.out.printf("%-28s", "YOU HAVE WORKED/PLAYED");
				} else {
					System.out.printf("%-28s", "YOU HAVE PLAYED/STUDIED");
				}
			} else {
				System.out.printf("%-28s", "YOU HAVE PLAYED");
			}

			scratchDate = new DateStruct(K6, K7, K5);
			printStatisticRow(F, A8, scratchDate);
			K5 = scratchDate.year;
			K6 = scratchDate.month;
			K7 = scratchDate.day;

			if (K6 == 12) {
				K5 = K5 + 1;
				K6 = 0;
			}
			System.out.printf("%-28s%14s%14s%14s%n", "YOU HAVE RELAXED", K5, K6, K7);
			System.out.printf("%16s***  YOU MAY RETIRE IN %s ***%n", " ", E);
			System.out.printf("%n%n%n%n%n");
		} else {
			System.out.println("NOT PREPARED TO GIVE DAY OF WEEK PRIOR TO MDLXXXII.");
		}
	}


	private static void printStatisticRow(float F, int A8, DateStruct scratchDate) {
		int K1 = (int) (F * A8);
		int I5 = K1 / 365;
		K1 = K1 - (I5 * 365);
		int I6 = K1 / 30;
		int I7 = K1 - (I6 * 30);
		int K5 = scratchDate.year - I5;
		int K6 = scratchDate.month - I6;
		int K7 = scratchDate.day - I7;
		if (K7 < 0) {
			K7 = K7 + 30;
			K6 = K6 - 1;
		}
		if (K6 <= 0) {
			K6 = K6 + 12;
			K5 = K5 - 1;
		}
		//to return the updated values of K5, K6, K7 we send them through the scratchDate
		scratchDate.year = K5;
		scratchDate.month = K6;
		scratchDate.day = K7;
		System.out.printf("%14s%14s%14s%n", I5, I6, I7);
	}

	private static void printHeadersAndAge(int I5, int I6, int I7) {
		System.out.printf("%14s%14s%14s%14s%14s%n", " ", " ", "YEARS", "MONTHS", "DAYS");
		System.out.printf("%14s%14s%14s%14s%14s%n", " ", " ", "-----", "------", "----");
		System.out.printf("%-28s%14s%14s%14s%n", "YOUR AGE (IF BIRTHDATE)", I5, I6, I7);
	}

	private static void calculateAndPrintDayOfWeek(int i1, int a, DateStruct dateStruct, DateStruct dateOfInterest, int y3) {
		int b = (a - b(a) * 7) + 1;
		if (dateOfInterest.month > 2) {
			printDayOfWeek(dateStruct, dateOfInterest, b);
		} else {
			if (y3 == 0) {
				int aa = i1 - 1;
				int t1 = aa - a(aa) * 4;
				if (t1 == 0) {
					if (b != 0) {
						b = b - 1;
						printDayOfWeek(dateStruct, dateOfInterest, b);
					} else {
						b = 6;
						b = b - 1;
						printDayOfWeek(dateStruct, dateOfInterest, b);
					}
				}
			}
		}
	}

	/**
	 * PRINT THE DAY OF THE WEEK THE DATE FALLS ON.
	 */
	private static void printDayOfWeek(DateStruct dateStruct, DateStruct dateOfInterest, int b) {
		if (b == 0) {
			b = 7;
		}
		if ((dateStruct.year * 12 + dateStruct.month) * 31
				+ dateStruct.day
				<
				(dateOfInterest.year * 12
						+ dateOfInterest.month) * 31 + dateOfInterest.day) {
			System.out.printf("%s / %s / %s WILL BE A ", dateOfInterest.month, dateOfInterest.day, dateOfInterest.year);
		} else if ((dateStruct.year * 12 + dateStruct.month) * 31
				+ dateStruct.day == (dateOfInterest.year * 12 + dateOfInterest.month)
				* 31 + dateOfInterest.day) {
			System.out.printf("%s / %s / %s IS A ", dateOfInterest.month, dateOfInterest.day, dateOfInterest.year);
		} else {
			System.out.printf("%s / %s / %s WAS A ", dateOfInterest.month, dateOfInterest.day, dateOfInterest.year);
		}
		switch (b) {
			case 1:
				System.out.println("SUNDAY.");
				break;
			case 2:
				System.out.println("MONDAY.");
				break;
			case 3:
				System.out.println("TUESDAY.");
				break;
			case 4:
				System.out.println("WEDNESDAY.");
				break;
			case 5:
				System.out.println("THURSDAY.");
				break;
			case 6:
				if (dateOfInterest.day == 13) {
					System.out.println("FRIDAY THE THIRTEENTH---BEWARE!");
				} else {
					System.out.println("FRIDAY.");
				}
				break;
			case 7:
				System.out.println("SATURDAY.");
				break;
		}
	}

	private static int a(int a) {
		return a / 4;
	}

	private static int b(int a) {
		return a / 7;
	}


	private static void printIntro() {
		System.out.println("                                WEEKDAY");
		System.out.println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
		System.out.println("\n\n\n");
		System.out.println("WEEKDAY IS A COMPUTER DEMONSTRATION THAT");
		System.out.println("GIVES FACTS ABOUT A DATE OF INTEREST TO YOU.");
		System.out.println("\n");
	}

	/**
	 * Read user input for a date, do some validation and return a simple date structure
	 */
	private static DateStruct readDate(Scanner scanner) {
		boolean done = false;
		int mm = 0, dd = 0, yyyy = 0;
		while (!done) {
			String input = scanner.next();
			String[] tokens = input.split(",");
			if (tokens.length < 3) {
				System.out.println("DATE EXPECTED IN FORM: 3,24,1979 - RETRY INPUT LINE");
			} else {
				try {
					mm = Integer.parseInt(tokens[0]);
					dd = Integer.parseInt(tokens[1]);
					yyyy = Integer.parseInt(tokens[2]);
					done = true;
				} catch (NumberFormatException nfe) {
					System.out.println("NUMBER EXPECTED - RETRY INPUT LINE");
				}
			}
		}
		return new DateStruct(mm, dd, yyyy);
	}

	/**
	 * Convenience date structure to hold user date input
	 */
	private static class DateStruct {
		int month;
		int day;
		int year;

		public DateStruct(int month, int day, int year) {
			this.month = month;
			this.day = day;
			this.year = year;
		}
	}

}
