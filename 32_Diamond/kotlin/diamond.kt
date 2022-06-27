
/**
 * Game of Diamond
 * <p>
 * Based on the BASIC game of Diamond
 * https://github.com/coding-horror/basic-computer-games/blob/main/32%20Diamond/diamond.bas
 * <p>
 *
 * Changes From Original: Input is validated.
 *
 * Converted from BASIC to Kotlin by Martin Marconcini (@Gryzor)
 * Inspired in the Java code written by Darren Cardenas.
 */


fun main() {
	Diamond().startGame()
}

class Diamond {
	init {
		printIntro()
	}

	fun startGame() {
		var body: Int
		var end: Int
		var start: Int = 1
		var row: Int = 1
		var numPerRow: Int
		var increment: Int = 2
		var lineContent: String
		var prefixIndex: Int
		
		printPrompt()

		// Read the user input
		val input = readLine()

		// Validate input
		val userInput: Int = try {
			input?.toInt() ?: -1
		} catch (e: NumberFormatException) {
			-1
		}
		if (!isValid(userInput)) {
			printInvalidInput()
			return
		}

		// Calculate how many diamonds can horizontally fit in the given space
		numPerRow = calculateDiamondsPerRow(userInput)
		end = userInput

		// Loop throw rows of Diamonds
		while (row <= numPerRow) {
			body = start
			while (canLoop(increment, body, end)) {
				lineContent = ""
				
				// Add white spaces to the "left" of the leftmost diamond.
				while (lineContent.length < ((userInput - body) / 2)) {
					lineContent += " "
				}

				// Begin loop through each column of diamonds
				for (col in 1..numPerRow) {
					prefixIndex = 1

					// Begin loop that fills each diamond with characters (not whitespace)
					for (fill in 1..body) {
						// Right side of diamond, if the index is greater than the prefix, put a Symbol.
						if (prefixIndex > PREFIX.length) {
							lineContent += SYMBOL
						}
						// Left side of diamond, pick a Prefix character (-1 since it starts at 0)
						else {
							lineContent += PREFIX[prefixIndex - 1]
							prefixIndex++
						}
					}// End loop that fills each diamond with characters

					// Is Column finished?
					if (col == numPerRow) {
						break
					}
					// Column is not finished...
					else {
						// Add whitespace on the "right" side of the current diamond, and fill the "left" side of the
						// next; doesn't fill the space to the right of the rightmost diamond.
						while (lineContent.length < (userInput * col + (userInput - body) / 2)) {
							lineContent += " "
						}

					}

				}// End loop through each column of diamonds

				// Print the current Line
				println(lineContent)

				// Increment the body that moves
				body += increment
			} //end While Loop throw rows of Diamonds

			// Increment the current Row of diamonds.
			row++

			if (start != 1) {
				start = 1
				end = userInput
				increment = 2
			} else {
				// We're rendering the "bottom half" of the total rendering.
				// Alter the parameters, and decrease the row number so the logic can loop again.
				start = userInput - 2
				end = 1
				increment = -2
				row--
			}
		} // End loop through each row of diamonds
	}

	private fun canLoop(increment: Int, body: Int, end: Int): Boolean = if (increment < 0) body >= end else body <= end

	private fun calculateDiamondsPerRow(totalDiamonds: Int): Int = LINE_WIDTH / totalDiamonds

	private fun isValid(input: Int): Boolean = (input in 5..21) && (input % 2 != 0)

	private fun printInvalidInput() = println("Invalid input")

	private fun printPrompt() {
		println(
			"""
			FOR A PRETTY DIAMOND PATTERN
			TYPE IN AN ODD NUMBER BETWEEN 5 AND 21? 
		""".trimIndent()
		)
		println()
	}

	private fun printIntro() {
		println(" ".repeat(32) + "DIAMOND")
		println(" ".repeat(14) + "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY")
		println()
		println()
	}

	companion object {
		const val LINE_WIDTH = 60
		const val PREFIX = "CC"
		const val SYMBOL = "!"
	}
}
