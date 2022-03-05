import com.pcholt.console.testutils.ConsoleTest
import org.junit.Test

class AnimalKtTest : ConsoleTest() {

    @Test
    fun `given a standard setup, find the fish`() {
        assertConversation(
            """
            $title
            ARE YOU THINKING OF AN ANIMAL? {YES}
            DOES IT SWIM? {YES}
            IS IT A FISH? {YES}
            WHY NOT TRY ANOTHER ANIMAL?
            ARE YOU THINKING OF AN ANIMAL? {QUIT}
            """
        ) {
            main()
        }
    }

    @Test
    fun `given a standard setup, create a cow, and verify`() {
        assertConversation(
            """
            $title
            ARE YOU THINKING OF AN ANIMAL? {YES}
            DOES IT SWIM? {NO}
            IS IT A BIRD? {NO}
            THE ANIMAL YOU WERE THINKING OF WAS A? {COW}
            PLEASE TYPE IN A QUESTION THAT WOULD DISTINGUISH A
            COW FROM A BIRD
            ? {DOES IT EAT GRASS}
            FOR A COW THE ANSWER WOULD BE? {YES}
            ARE YOU THINKING OF AN ANIMAL? {YES}
            DOES IT SWIM? {NO}
            DOES IT EAT GRASS? {YES}
            IS IT A COW? {YES}
            WHY NOT TRY ANOTHER ANIMAL?
            ARE YOU THINKING OF AN ANIMAL? {QUIT}
        """
        ) {
            main()
        }
    }

    private val title = """
                                        ANIMAL
                      CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY

        PLAY 'GUESS THE ANIMAL'
        THINK OF AN ANIMAL AND THE COMPUTER WILL TRY TO GUESS IT.
    """
}
