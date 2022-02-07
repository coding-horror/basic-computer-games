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

	public void println(String text) {
		out.println(text);
	}

	public void println(String text, int leftPad) {
		IntStream.range(0, leftPad).forEach((i) -> out.print(' '));
		out.println(text);
	}

	public void print(String text) {
		out.print(text);
		out.flush();
	}

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
}
