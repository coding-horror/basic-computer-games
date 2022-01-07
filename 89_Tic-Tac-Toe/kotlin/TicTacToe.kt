import java.util.Random
import kotlin.system.exitProcess

/**
 * @author John Long based on Java from Ollie Hensman-Crook
 */
private val compChoice = Random()
private val gameBoard = Board()

fun main() {
    println("              TIC-TAC-TOE")
    println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    println("\nTHE BOARD IS NUMBERED: ")
    println(" 1  2  3\n 4  5  6\n 7  8  9\n")
    while (true) {
        // Let the player choose whether to be X or O (Player.X or Player.O)
        val (human, computer) = readXOrO()
        while (true) {
            // Get a valid move from the user and then move there
            val validMoveIndex = readValidMove()
            gameBoard.setArr(validMoveIndex, human)
            gameBoard.printBoard()

            // Computer randomly fills a square (if the game isn't already over)
            // This uses Kotlin's null handling and will only set the board
            // if validRandomMove returned a non-null value
            validRandomMove()?.let {
                gameBoard.setArr(it, computer)
                gameBoard.printBoard()
            }

            // if there is a win print if player won or the computer won and ask if they
            // want to play again
            when {
                gameBoard.isWinFor(human) -> {
                    checkPlayAgain("YOU WIN")
                    break
                }
                gameBoard.isWinFor(computer) -> {
                    checkPlayAgain("YOU LOSE")
                    break
                }
                gameBoard.isDraw() -> {
                    checkPlayAgain("DRAW")
                    break
                }
            }
        }
    }
}

private fun checkPlayAgain(result: String) {
    println("$result, PLAY AGAIN? (Y/N)")
    gameBoard.clear()
    if (!readYesOrNo()) exitProcess(0)
}

private fun readYesOrNo(): Boolean {
    while (true) {
        when (readLine()?.get(0)?.uppercaseChar()) {
            'Y' -> return true
            'N' -> return false
            else -> println("THAT'S NOT 'Y' OR 'N', TRY AGAIN")
        }
    }
}

private fun validRandomMove(): Int? {
    if (gameBoard.isDraw() || gameBoard.isWinFor(Player.O) || gameBoard.isWinFor(Player.X)) return null
    println("THE COMPUTER MOVES TO")
    // keep generating a random value until we find one that is null (unset)
    return generateSequence { 1 + compChoice.nextInt(9) }.first { gameBoard.getBoardValue(it) == null }
}

private fun readValidMove(): Int {
    println("WHERE DO YOU MOVE")
    while (true) {
        val input = readln().toIntOrNull()
        if (input != null && gameBoard.getBoardValue(input) == null) {
            return input
        } else {
            println("INVALID INPUT, TRY AGAIN")
        }
    }
}

private fun readXOrO(): Pair<Player, Player> {
    println("DO YOU WANT 'X' OR 'O'")
    while (true) {
        when (readln()[0].uppercaseChar()) {
            'X' -> return Player.X to Player.O
            'O' -> return Player.O to Player.X
            else -> println("THAT'S NOT 'X' OR 'O', TRY AGAIN")
        }
    }
}
