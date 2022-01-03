/**
 * ANIMAL
 *
 *
 * Converted from BASIC to Kotlin by John Long (@patimen)
 *
 * Animal is basically a perfect example of a binary tree. Implement it
 * as such, with the QuestionNode either having an answer if it is a terminal node
 * or a Question
 */

fun main() {
    printIntro()
    val rootQuestionNode =
        QuestionOrAnswer(question = Question("DOES IT SWIM", QuestionOrAnswer("FISH"), QuestionOrAnswer("BIRD")))
    while (true) {
        val choice = ask("ARE YOU THINKING OF AN ANIMAL")
        when {
            choice == "LIST" -> printKnownAnimals(rootQuestionNode)
            choice.startsWith("Q") -> return
            choice.startsWith("Y") -> {
                // A wrong answer means it's a new animal!
                val wrongAnswer = rootQuestionNode.getWrongAnswer()
                if (wrongAnswer == null) {
                    // The computer got the right answer!
                    println("WHY NOT TRY ANOTHER ANIMAL?")
                } else {
                    // Get a new question to ask next time
                    wrongAnswer.askForInformationAndSave()
                }
            }
        }
    }
}

// Takes care of asking a question (on the same line) and getting
// an answer or a blank string
fun ask(question: String): String {
    print("$question? ")
    return readLine()?.uppercase() ?: ""
}

// Special case for a "yes or no" question, returns true of yes
fun askYesOrNo(question: String): Boolean {
    return generateSequence {
        print("$question? ")
        readLine()
    }.firstNotNullOf { yesOrNo(it) }
}

// If neither Y (true) or N (false), return null, so the above sequence
// will just keep executing until it gets the answer
private fun yesOrNo(string: String): Boolean? =
    when (string.uppercase().firstOrNull()) {
        'Y' -> true
        'N' -> false
        else -> null
    }

private fun printKnownAnimals(question: QuestionOrAnswer) {
    println("\nANIMALS I ALREADY KNOW ARE:")
    val animals = question.getAnswers().chunked(4)
    animals.forEach { line ->
        // The '*' in front of line.toTypedArray() "spreads" the array as a list of parameters instead
        System.out.printf("%-15s".repeat(line.size), *line.toTypedArray())
        println()
    }
}

private fun printIntro() {
    println("                                ANIMAL")
    println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    println("\n\n")
    println("PLAY 'GUESS THE ANIMAL'")
    println("\n")
    println("THINK OF AN ANIMAL AND THE COMPUTER WILL TRY TO GUESS IT.")
}

class QuestionOrAnswer(private var answer: String? = null, var question: Question? = null) {
    fun getAnswers(): List<String> = answer?.let { listOf(it) } ?: question!!.getAnswers()
    fun getWrongAnswer(): QuestionOrAnswer? {
        if (answer != null) {
            // "takeUnless" will return null if the answer is "yes". In this case
            // we will return the "wrong answer", aka the terminal answer that was incorrect
            return this.takeUnless { askYesOrNo("IS IT A $answer") }
        }
        return question?.getWrongAnswer()
    }

    fun askForInformationAndSave() {
        //Failed to get it right and ran out of questions
        //Let's ask the user for the new information
        val newAnimal = ask("THE ANIMAL YOU WERE THINKING OF WAS A")
        val newQuestion = ask("PLEASE TYPE IN A QUESTION THAT WOULD DISTINGUISH A \n$newAnimal FROM A $answer\n")
        val newAnswer = askYesOrNo("FOR A $newAnimal THE ANSWER WOULD BE")

        val trueAnswer = if (newAnswer) newAnimal else answer
        val falseAnswer = if (newAnswer) answer else newAnimal
        // Replace our answer with null  and set the question with the data we just got
        // This makes it a question instead of an answer
        this.answer = null
        this.question = Question(newQuestion, QuestionOrAnswer(trueAnswer), QuestionOrAnswer(falseAnswer))
    }
}

class Question(
    private val question: String,
    private val trueAnswer: QuestionOrAnswer,
    private val falseAnswer: QuestionOrAnswer
) {
    fun getAnswers(): List<String> = trueAnswer.getAnswers() + falseAnswer.getAnswers()

    fun getWrongAnswer(): QuestionOrAnswer? =
        if (askYesOrNo(question)) {
            trueAnswer.getWrongAnswer()
        } else {
            falseAnswer.getWrongAnswer()
        }
}
