import java.io.BufferedReader;
import java.io.EOFException;
import java.io.IOException;
import java.io.PrintWriter;
import java.io.Reader;
import java.io.UncheckedIOException;
import java.io.Writer;
import java.util.stream.IntStream;

/**
 * This class is responsible for printing output to the screen and reading input
 * from the user. It must be initialized with a reader to get input data from
 * and a writer to send output to. Typically these will wrap System.in and
 * System.out respectively, but can be a StringReader and StringWriter when
 * running in test code.
 */
public class UserIo {

    private BufferedReader in;
    private PrintWriter out;

	/**
	 * Initializes the UserIo with the given reader/writer. The reader will be
	 * wrapped in a BufferedReader and so should <i>not</i> be a BufferedReader
	 * already (to avoid double buffering).
	 * 
	 * @param in Typically an InputStreamReader wrapping System.in or a StringReader
	 * @param out Typically an OuputStreamWriter wrapping System.out or a StringWriter
	 */
    public UserIo(Reader in, Writer out) {
        this.in = new BufferedReader(in);
        this.out = new PrintWriter(out, true);
    }

	/**
	 * Print the line of text to output including a trailing linebreak.
	 * 
	 * @param text the text to print
	 */
	public void println(String text) {
		out.println(text);
	}

	/**
	 * Print the given text left padded with spaces.
	 * 
	 * @param text The text to print
	 * @param leftPad The number of spaces to pad with.
	 */
	public void println(String text, int leftPad) {
		IntStream.range(0, leftPad).forEach((i) -> out.print(' '));
		out.println(text);
	}

	/**
	 * Print the given text <i>without</i> a trailing linebreak.
	 * 
	 * @param text The text to print.
	 */
	public void print(String text) {
		out.print(text);
		out.flush();
	}

	/**
	 * Reads a line of text from input.
	 * 
	 * @return The line entered into input.
	 * @throws UncheckedIOException if the line is null (CTRL+D or CTRL+Z was pressed)
	 */
	private String readLine() {
		try {
			String line = in.readLine();
			if(line == null) {
				throw new UncheckedIOException("!END OF INPUT", new EOFException());
			}
			return line;
		} catch (IOException e) {
			throw new UncheckedIOException(e);
		}
	}

	/**
	 * Prompt the user via input.
	 * 
	 * @param prompt The text to display as a prompt. A question mark and space will be added to the end, so if prompt = "EXAMPLE" then the user will see "EXAMPLE? ".
	 * @return The line read from input.
	 */
	public String prompt(String prompt) {
		print(prompt + "? ");
		return readLine();
	}

	/**
	 * Prompts the user for a "Yes" or "No" answer.
	 * @param prompt The prompt to display to the user on STDOUT.
	 * @return false if the user enters a value beginning with "N" or "n"; true otherwise.
	 */
	public boolean promptBoolean(String prompt) {
		print(prompt + "? ");

		String input = readLine();

		if(input.toLowerCase().startsWith("n")) {
			return false;
		} else {
			return true;
		}
	}

	/**
	 * Prompts the user for an integer.  As in Vintage Basic, "the optional
	 * prompt string is followed by a question mark and a space." and if the
	 * input is non-numeric, "an error will be generated and the user will be
	 * re-prompted.""
	 *
	 * @param prompt The prompt to display to the user.
	 * @return the number given by the user.
	 */
	public int promptInt(String prompt) {
		print(prompt + "? ");

		while(true) {
			String input = readLine();
			try {
				return Integer.parseInt(input);
			} catch(NumberFormatException e) {
				// Input was not numeric.
				println("!NUMBER EXPECTED - RETRY INPUT LINE");
				print("? ");
				continue;
			}
		}
	}

	/**
	 * Prompts the user for a double.  As in Vintage Basic, "the optional
	 * prompt string is followed by a question mark and a space." and if the
	 * input is non-numeric, "an error will be generated and the user will be
	 * re-prompted.""
	 *
	 * @param prompt The prompt to display to the user.
	 * @return the number given by the user.
	 */
	public double promptDouble(String prompt) {
		print(prompt + "? ");

		while(true) {
			String input = readLine();
			try {
				return Double.parseDouble(input);
			} catch(NumberFormatException e) {
				// Input was not numeric.
				println("!NUMBER EXPECTED - RETRY INPUT LINE");
				print("? ");
				continue;
			}
		}
	}
}
