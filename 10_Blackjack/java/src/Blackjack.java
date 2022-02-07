import java.io.EOFException;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.Reader;
import java.io.UncheckedIOException;
import java.io.Writer;
import java.util.Collections;

public class Blackjack {
	public static void main(String[] args) {
		// Intuitively it might seem like the main program logic should be right
		// here in 'main' and that we should just use System.in and System.out
		// directly whenever we need them.  However, by externalizing the source
		// of input/output data (and the ordering of the cards via a custom
		// shuffle function), we can write non-interactive and deterministic
		// tests of the code. See UserIoTest as an example.
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
