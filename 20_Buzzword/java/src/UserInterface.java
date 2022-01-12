import static java.lang.System.out;

import java.util.Scanner;
import java.util.function.Supplier;

/**
 * A command line user interface that outputs a buzzword every
 * time the user requests a new one.
 */
public class UserInterface implements Runnable {

	/**
	 * Input from the user.
	 */
	private final Scanner input;

	/**
	 * The buzzword generator.
	 */
	private final Supplier<String> buzzwords;

	/**
	 * Create a new user interface.
	 *
	 * @param input The input scanner with which the user gives commands.
	 * @param buzzwords The buzzword supplier.
	 */
	public UserInterface(final Scanner input,
		final Supplier<String> buzzwords) {
		this.input = input;
		this.buzzwords = buzzwords;		
	}

	@Override
	public void run() {
		out.println("              BUZZWORD GENERATOR");
		out.println("   CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
		out.println();
		out.println();
		out.println();
		out.println("THIS PROGRAM PRINTS HIGHLY ACCEPTABLE PHRASES IN");
		out.println("'EDUCATOR-SPEAK' THAT YOU CAN WORK INTO REPORTS");
		out.println("AND SPEECHES.  WHENEVER A QUESTION MARK IS PRINTED,");
		out.println("TYPE A 'Y' FOR ANOTHER PHRASE OR 'N' TO QUIT.");
		out.println();
		out.println();
		out.println("HERE'S THE FIRST PHRASE:");

		do {
			out.println(buzzwords.get());
			out.println();
			out.print("?");
		} while ("Y".equals(input.nextLine().toUpperCase()));

		out.println("COME BACK WHEN YOU NEED HELP WITH ANOTHER REPORT!");
	}
}
