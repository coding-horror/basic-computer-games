import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.io.Reader;
import java.io.StringReader;
import java.io.StringWriter;

import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.junit.jupiter.params.provider.ValueSource;

public class UserIoTest {

    @ParameterizedTest(name = "''{0}'' is accepted as ''no''")
    @ValueSource(strings = {"N", "n", "No", "NO", "no"})
    public void testPromptBooleanAcceptsNo(String response) {
        // Given
        Reader in = new StringReader(response + "\n");
        StringWriter out = new StringWriter();
        UserIo userIo = new UserIo(in, out);

        // When
        boolean result = userIo.promptBoolean("TEST");

        // Then
        assertEquals("TEST? ", out.toString());
        assertFalse(result);
    }

    @ParameterizedTest(name = "''{0}'' is accepted as ''yes''")
    @ValueSource(strings = {"Y", "y", "Yes", "YES", "yes", "", "foobar"})
    public void testPromptBooleanAcceptsYes(String response) {
        // Given
        Reader in = new StringReader(response + "\n");
        StringWriter out = new StringWriter();
        UserIo userIo = new UserIo(in, out);

        // When
        boolean result = userIo.promptBoolean("TEST");

        // Then
        assertEquals("TEST? ", out.toString());
        assertTrue(result);
    }

    @ParameterizedTest(name = "''{0}'' is accepted as number")
    @CsvSource({
        "1,1",
        "0,0",
        "-1,-1",
    })
    public void testPromptIntAcceptsNumbers(String response, int expected) {
        // Given
        Reader in = new StringReader(response + "\n");
        StringWriter out = new StringWriter();
        UserIo userIo = new UserIo(in, out);

        // When
        int result = userIo.promptInt("TEST");

        // Then
        assertEquals("TEST? ", out.toString());
        assertEquals(expected, result);
    }

    @Test
    @DisplayName("promptInt should print an error and reprompt if given a non-numeric response")
    public void testPromptIntRepromptsOnNonNumeric() {
        // Given
        Reader in = new StringReader("foo\n1"); // word, then number
        StringWriter out = new StringWriter();
        UserIo userIo = new UserIo(in, out);

        // When
        int result = userIo.promptInt("TEST");

        // Then
        assertEquals("TEST? !NUMBER EXPECTED - RETRY INPUT LINE\n? ", out.toString());
        assertEquals(1, result);
    }
}
