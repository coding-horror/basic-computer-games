import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.io.EOFException;
import java.io.Reader;
import java.io.StringReader;
import java.io.StringWriter;
import java.io.UncheckedIOException;

public class GameTest {

    private StringReader in;
    private StringWriter out;
    private Game game;

    private void givenInput(String input) {
        Reader in = new StringReader("\u2404"); // U+2404 is "End of Transmission" sent by CTRL+D (or CTRL+Z on Windows)
        StringWriter out = new StringWriter();
        UserIo userIo = new UserIo(in, out);
        Deck deck = new Deck((cards) -> cards);
        game = new Game(deck, userIo);
    }

    @Test
    public void shouldQuitOnCtrlD() {
        // Given
        givenInput("\u2404"); // U+2404 is "End of Transmission" sent by CTRL+D (or CTRL+Z on Windows)

        // When
        Exception e = assertThrows(UncheckedIOException.class, game::run);

        // Then
        assertTrue(e.getCause() instanceof EOFException);
        assertEquals("!END OF INPUT", e.getMessage());
    }
}
