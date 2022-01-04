/**
 * Game of Synonym
 *
 *
 * Based on the Basic game of Synonym here
 * https://github.com/coding-horror/basic-computer-games/blob/main/85%20Synonym/synonym.bas
 *
 *
 * Note:  The idea was to create a version of the 1970's Basic game in Java, without introducing
 * new features - no additional text, error checking, etc has been added.
 */

fun main() {
    println(introText)
    synonyms.forEach {
        // Inside with, "this" is the current synonym
        with(it) {
            do {
                val answer = ask("     WHAT IS A SYNONYM OF $word ? ")
                when {
                    answer == "HELP" ->
                        println("""**** A SYNONYM OF $word IS ${synonyms.random()}.""")
                    synonyms.contains(answer) ->
                        println(RANDOM_ANSWERS.random())
                    else ->
                        println("TRY AGAIN.")
                }
            } while (!synonyms.contains(answer))
        }
    }
    println("SYNONYM DRILL COMPLETED.")
}

val introText = """
${tab(33)}SYNONYM
${tab(15)}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY
A SYNONYM OF A WORD MEANS ANOTHER WORD IN THE ENGLISH
LANGUAGE WHICH HAS THE SAME OR VERY NEARLY THE SAME
 MEANING.
I CHOOSE A WORD -- YOU TYPE A SYNONYM.
IF YOU CAN'T THINK OF A SYNONYM, TYPE THE WORD 'HELP'
AND I WILL TELL YOU A SYNONYM.

    """

// prints a question and reads a string (and converts to uppercase)
private fun ask(text: String): String {
    print(text)
    return readln().uppercase()
}

// Just like TAB in BASIC
private fun tab(spaces: Int): String = " ".repeat(spaces)

val RANDOM_ANSWERS = arrayOf("RIGHT", "CORRECT", "FINE", "GOOD!", "CHECK")

// List of words and synonyms
private val synonyms = listOf(
    SynonymList("FIRST", listOf("START", "BEGINNING", "ONSET", "INITIAL")),
    SynonymList("SIMILAR", listOf("SAME", "LIKE", "RESEMBLING")),
    SynonymList("MODEL", listOf("PATTERN", "PROTOTYPE", "STANDARD", "CRITERION")),
    SynonymList("SMALL", listOf("INSIGNIFICANT", "LITTLE", "TINY", "MINUTE")),
    SynonymList("STOP", listOf("HALT", "STAY", "ARREST", "CHECK", "STANDSTILL")),
    SynonymList("HOUSE", listOf("DWELLING", "RESIDENCE", "DOMICILE", "LODGING", "HABITATION")),
    SynonymList("PIT", listOf("HOLE", "HOLLOW", "WELL", "GULF", "CHASM", "ABYSS")),
    SynonymList("PUSH", listOf("SHOVE", "THRUST", "PROD", "POKE", "BUTT", "PRESS")),
    SynonymList("RED", listOf("ROUGE", "SCARLET", "CRIMSON", "FLAME", "RUBY")),
    SynonymList("PAIN", listOf("SUFFERING", "HURT", "MISERY", "DISTRESS", "ACHE", "DISCOMFORT"))
)

class SynonymList(val word: String, val synonyms: List<String>)