import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.Reader;
import java.io.Writer;
import java.util.Collections;

/**
 * Plays a game of blackjack on the terminal. Looking at the code, the reader
 * might conclude that this implementation is "over engineered." We use many
 * techniques and patterns developed for much larger code bases to create more
 * maintainable code, which may not be as relevant for a simple game of
 * Blackjack. To wit, the rules and requirements are not likely to ever change
 * so there is not so much value making the code flexible.
 * 
 * Nevertheless, this is meant to be an example that the reader can learn good
 * Java coding techniques from. Furthermore, many of the "over-engineering"
 * tactics are as much about testability as they are about maintainability.
 * Imagine trying to manually test infrequent scenarios like Blackjack,
 * insurance, or splitting without any ability to automate a specific scenario
 * and the value of unit testing becomes immediately apparent.
 * 
 * Another "unnecessary" aspect of this codebase is good Javadoc. Again, this is
 * meant to be educational, but another often overlooked benefit is that most
 * IDEs will display Javadoc in "autocomplete" suggestions. This is remarkably
 * helpful when using a class as a quick reminder of what you coded earlier.
 * This is true even if no one ever publishes or reads the HTML output of the
 * javadoc.
 * 
 */
public class Blackjack {
	public static void main(String[] args) {
		// Intuitively it might seem like the main program logic should be right
		// here in 'main' and that we should just use System.in and System.out
		// directly whenever we need them.  However, notice that System.out and
		// System.in are just an OutputStream and InputStream respectively. By
		// allowing alternate streams to be specified to Game at runtime, we can
		// write non-interactive tests of the code. See UserIoTest as an
		// example.
		// Likewise, by allowing an alternative "shuffle" algorithm, test code
		// can provide a deterministic card ordering.
		try (Reader in = new InputStreamReader(System.in)) {
			Writer out = new OutputStreamWriter(System.out);
			UserIo userIo = new UserIo(in, out);
			Deck deck = new Deck(cards -> {
				userIo.println("RESHUFFLING");
			    Collections.shuffle(cards);
			    return cards;
			});
			Game game = new Game(deck, userIo);
			game.run();
		} catch (Exception e) {
			// This allows us to elegantly handle CTRL+D / CTRL+Z by throwing an exception.
			System.out.println(e.getMessage());
			System.exit(1);
		}
	}
}
