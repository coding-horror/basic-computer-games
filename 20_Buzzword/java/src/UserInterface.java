import java.io.PrintStream;
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
	 * Output to the user.
	 */
	private final PrintStream output;

	/**
	 * The buzzword generator.
	 */
	private final Supplier<String> buzzwords;

	/**
	 * Create a new user interface.
	 *
	 * @param input The input scanner with which the user gives commands.
	 * @param output The output to show messages to the user.
	 * @param buzzwords The buzzword supplier.
	 */
	public UserInterface(final Scanner input,
			final PrintStream output,
			final Supplier<String> buzzwords) {
		this.input = input;
		this.output = output;
		this.buzzwords = buzzwords;
	}

	@Override
	public void run() {
		output.println("              BUZZWORD GENERATOR");
		output.println("   CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
		output.println();
		output.println();
		output.println();
		output.println("THIS PROGRAM PRINTS HIGHLY ACCEPTABLE PHRASES IN");
		output.println("'EDUCATOR-SPEAK' THAT YOU CAN WORK INTO REPORTS");
		output.println("AND SPEECHES.  WHENEVER A QUESTION MARK IS PRINTED,");
		output.println("TYPE A 'Y' FOR ANOTHER PHRASE OR 'N' TO QUIT.");
		output.println();
		output.println();
		output.println("HERE'S THE FIRST PHRASE:");

		do {
			output.println(buzzwords.get());
			output.println();
			output.print("?");
		} while ("Y".equals(input.nextLine().toUpperCase()));

		output.println("COME BACK WHEN YOU NEED HELP WITH ANOTHER REPORT!");
	}
}
