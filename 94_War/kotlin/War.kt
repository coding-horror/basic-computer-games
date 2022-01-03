/**
 * Converted FROM BASIC to Kotlin by John Long, with hints from the Java by Nahid Mondol.
 *
 */

val suits = listOf("S", "H", "C", "D")
val ranks = listOf("2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A")

// Create the deck programmatically
val fullDeck = suits.flatMap { suit ->
    ranks.map { rank ->
        Card(suit, rank)
    }
}

class Card(private val suit: String, private val rank: String) {
    // Allow comparison of cards to each other
    operator fun compareTo(other: Card): Int = this.rankValue.compareTo(other.rankValue)
    // We can figure relative rank by the order in the ranks value
    private val rankValue: Int = ranks.indexOf(rank)
    override fun toString(): String = "$suit-$rank"
}

fun main() {
    introMessage()
    showDirectionsBasedOnInput()
    playGame()
    println("THANKS FOR PLAYING.  IT WAS FUN.")
}

private fun introMessage() {
    println("\t         WAR")
    println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    println("THIS IS THE CARD GAME OF WAR. EACH CARD IS GIVEN BY SUIT-#")
    print("AS S-7 FOR SPADE 7. DO YOU WANT DIRECTIONS? ")
}

private fun showDirectionsBasedOnInput() {
    if (getYesOrNo()) {
        println("THE COMPUTER GIVES YOU AND IT A 'CARD'. THE HIGHER CARD")
        println("(NUMERICALLY) WINS. THE GAME ENDS WHEN YOU CHOOSE NOT TO ")
        println("CONTINUE OR WHEN YOU HAVE FINISHED THE PACK.\n")
    }
}

// Stay in loop until player chooses an option, then return "true" for yes or "false" for no
private fun getYesOrNo() = generateSequence { readln() }.firstNotNullOf { it.asYesOrNo }

// Since this returns null for an incorrect value, above firstNotNullOf will keep looping until
// we get something valid
private val String.asYesOrNo: Boolean?
    get() =
        when (this.lowercase()) {
            "yes" -> true
            "no" -> false
            else -> {
                println("YES OR NO, PLEASE.   ")
                null
            }
        }


private fun playGame() {
    // Shuffle the deck than break it into 26 pairs
    val pairs = fullDeck.shuffled().chunked(2)
    val score = Score(0, 0)
    val lastPlayerCard = pairs.last().first()
    // We use "destructuring" to extract the pair of cards directly to a variable here
    pairs.forEach { (playerCard, computerCard) ->
        println("YOU: $playerCard\tCOMPUTER: $computerCard")
        when {
            playerCard > computerCard -> score.playerWins()
            computerCard > playerCard -> score.computerWins()
            else -> println("TIE.  NO SCORE CHANGE.")
        }
        // Doesn't make sense to ask to continue if we have no more cards left to deal
        if (playerCard != lastPlayerCard) {
            println("DO YOU WANT TO CONTINUE")
            if (!getYesOrNo()) {
                return
            }
        }
    }
    score.printFinalScore()
    return
}


class Score(private var player: Int, private var computer: Int) {
    fun playerWins() {
        player++
        printScore("YOU WIN.")
    }

    fun computerWins() {
        computer++
        printScore("THE COMPUTER WINS!!!")
    }

    private fun printScore(text: String) {
        println("$text YOU HAVE $player AND THE COMPUTER HAS $computer")
    }

    // Only print if you go through the whole deck
    fun printFinalScore() {
        println("WE HAVE RUN OUT OF CARDS.  FINAL SCORE:   YOU: $player  THE COMPUTER:$computer")
    }
}
