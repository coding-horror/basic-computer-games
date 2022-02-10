import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.Reader;
import java.io.Writer;
import java.util.Collections;

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
