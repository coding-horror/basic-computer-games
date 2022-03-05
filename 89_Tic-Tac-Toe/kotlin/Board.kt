/**
 * @author John Long based on Java by Ollie Hensman-Crook
 */

enum class Player(val char: Char) {
    X('X'),
    O('O')
}

class Board {
    // Initialize an array of size nine with all values set to null
    private var boxes: Array<Player?> = arrayOfNulls(9)

    /**
     * Place 'X' or 'O' on the board position passed
     * @param position
     * @param player
     */
    fun setArr(position: Int, player: Player) {
        boxes[position - 1] = player
    }

    fun printBoard() {
        System.out.format(
            """
  %c !  %c !  %c
----+----+----
  %c !  %c !  %c
----+----+----
  %c !  %c !  %c
""",
            // converts each box to a char and then passes them in order to format
            // If the person is unassigned, use a space ' '
     *(boxes.map{it?.char ?: ' '}.toTypedArray()))
    }

    /**
     * @param x
     * @return the value of the char at a given position
     */
    fun getBoardValue(x: Int): Player? {
        return boxes[x - 1]
    }

    private val winningCombos = listOf(
        // horizontal
        listOf(0,1,2),
        listOf(3,4,5),
        listOf(6,7,8),
        // diagonal
        listOf(0,4,8),
        listOf(2,4,6),
        // vertical
        listOf(0,3,6),
        listOf(1,4,7),
        listOf(2,5,8)
    )
    /**
     * Go through the board and check for win
     * @param player
     * @return whether a win has occurred
     */
    fun isWinFor(player: Player): Boolean {
        // Check if any winningCombos have all their boxes set to player
        return winningCombos.any{ combo ->
            combo.all { boxes[it] == player }
        }
    }

    fun isDraw(): Boolean {
        return !isWinFor(Player.X) && !isWinFor(Player.O) && boxes.all { it != null }
    }

    /**
     * Reset the board
     */
    fun clear() {
        boxes = arrayOfNulls(9)
    }
}
