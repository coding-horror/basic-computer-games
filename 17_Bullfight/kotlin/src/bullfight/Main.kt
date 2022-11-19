package bullfight

import bullfight.Yorn.*
import kotlin.random.Random
import kotlin.system.exitProcess

private val Boolean.asInteger get() = if (this) 1 else 2
private val Float.squared: Float
    get() = this * this
val fna: Boolean get() = RandomNumbers.nextBoolean()

enum class Quality(private val typeName: String) {
    Superb("SUPERB"),
    Good("GOOD"),
    Fair("FAIR"),
    Poor("POOR"),
    Awful("AWFUL");
    override fun toString() = typeName

    val level get() = (ordinal + 1).toFloat()
}

enum class BullDeath(val factor: Float) {
    Alive(1f), Dead(2f);
}

var l = 1f
lateinit var bullQuality: Quality
var momentOfTruth = false
var picadoresSuccess = 0f
var toreadoresSuccess = 0f
var passNumber = 0f
var honor = 0f
var bullDeath = BullDeath.Alive


interface RandomNumberSource {
    fun nextBoolean(): Boolean
    fun nextInt(from: Int, until: Int): Int
    fun nextFloat(): Float
}

object RandomNumbers : RandomNumberSource {
    override fun nextBoolean() = Random.nextBoolean()
    override fun nextInt(from: Int, until: Int) = Random.nextInt(from, until)
    override fun nextFloat() = Random.nextFloat()
}


fun main() {
    intro()
    instructions()
    bullDeath = BullDeath.Alive
    honor = 1f

    bullQuality = Quality.values()[RandomNumbers.nextInt(1, 6)]
    println("YOU HAVE DRAWN A $bullQuality BULL.")
    when (bullQuality) {
        Quality.Superb -> println("GOOD LUCK.  YOU'LL NEED IT.")
        Quality.Awful -> println("YOU'RE LUCKY.")
        else -> Unit
    }

    picadoresSuccess = fight(FirstAct.picadores)
    toreadoresSuccess = fight(FirstAct.toreadores)
    println()
    println()

    var gored: Boolean

    gameLoop@ do {
        passNumber++
        println("PASS NUMBER ${passNumber.toInt()}")
        gored = if (passNumber >= 3) {
            print("HERE COMES THE BULL.  TRY FOR A KILL")
            killAttempt("CAPE MOVE")
        } else {
            println("THE BULL IS CHARGING AT YOU!  YOU ARE THE MATADOR--")
            print("DO YOU WANT TO KILL THE BULL")
            killAttempt("WHAT MOVE DO YOU MAKE WITH THE CAPE")
        }

        if (!gored) {
            val move = restrictedInput(
                values = Cape.values(),
                errorMessage = "DON'T PANIC, YOU IDIOT!  PUT DOWN A CORRECT NUMBER"
            )
            val m = when (move) {
                Cape.Veronica -> 3f
                Cape.Outside -> 2f
                Cape.Swirl -> 0.5f
            }

            l += m
            val f =
                (6 - bullQuality.level + m / 10f) * RandomNumbers.nextFloat() / ((picadoresSuccess + toreadoresSuccess + passNumber / 10f) * 5f)
            if (f < 0.51)
                continue
        }

        println("THE BULL HAS GORED YOU!")
        goreLoop@ do {
            when (fna) {
                false -> {
                    println("YOU ARE DEAD.")
                    honor = 1.5f
                    gameResult()
                }

                true -> {
                    println("YOU ARE STILL ALIVE.")
                    println()
                    print("DO YOU RUN FROM THE RING")
                    when (Yorn.input()) {

                        YES -> {
                            println("COWARD")
                            honor = 0f
                        }

                        NO -> {
                            println("YOU ARE BRAVE.  STUPID, BUT BRAVE.")
                            when (fna) {
                                true -> {
                                    honor = 2f
                                    continue@gameLoop
                                }

                                false -> {
                                    println("YOU ARE GORED AGAIN!")
                                    continue@goreLoop
                                }
                            }
                        }
                    }

                }
            }
        } while (true)

    } while (true)

}

fun fnd() = 4.5 +
        l / 6 -
        (picadoresSuccess + toreadoresSuccess) * 2.5 +
        4 * honor +
        2 * bullDeath.factor -
        passNumber.squared / 120f -
        bullQuality.level

fun fnc() = fnd() * RandomNumbers.nextFloat()

private fun killAttempt(capeMessage: String): Boolean {
    when (Yorn.input()) {

        YES ->
            when (momentOfTruth()) {
                KillResult.Success -> gameResult()
                KillResult.Fail -> return true
            }

        NO ->
            print(capeMessage)

    }
    return false
}

private fun gameResult() {
    println()
    println()
    if (honor == 0f) {
        println(
            """
                            THE CROWD BOOS FOR TEN MINUTES.  IF YOU EVER DARE TO SHOW
                            YOUR FACE IN A RING AGAIN, THEY SWEAR THEY WILL KILL YOU--
                            UNLESS THE BULL DOES FIRST.
                            """.trimIndent()
        )
    } else {
        if (honor == 2f)
            println("THE CROWD CHEERS WILDLY!")
        else
            if (bullDeath == BullDeath.Dead) {
                println("THE CROWD CHEERS!")
                println()
            }
        println("THE CROWD AWARDS YOU")
        if (fnc() < 2.4)
            println("NOTHING AT ALL.")
        else if (fnc() < 4.9)
            println("ONE EAR OF THE BULL.")
        else
            if (fnc() < 7.4)
                println("BOTH EARS OF THE BULL!")
            else
                println("OLE!  YOU ARE 'MUY HOMBRE'!! OLE!  OLE!")
        println()
    }
    println()
    println("ADIOS")
    println()
    println()
    println()
    exitProcess(0)
}

enum class KillResult { Success, Fail }

fun momentOfTruth(): KillResult {
    momentOfTruth = true
    print(
        """
        
        IT IS THE MOMENT OF TRUTH.
        
        HOW DO YOU WANT TO KILL THE BULL
    """.trimIndent()
    )

    val k =
        (6 - bullQuality.level) * 10 * RandomNumbers.nextFloat() / ((picadoresSuccess + toreadoresSuccess) * 5 * passNumber)

    val chance = when (stdInput(KillMethod.values())) {
        KillMethod.OverHorns -> .8
        KillMethod.Chest -> .2
        null -> {
            println("YOU PANICKED.  THE BULL GORED YOU.")
            return KillResult.Fail
        }
    }
    return if (k <= chance) {
        println("YOU KILLED THE BULL!")
        bullDeath = BullDeath.Dead
        KillResult.Success
    } else {
        println("THE BULL HAS GORED YOU!")
        KillResult.Fail
    }
}

interface InputOption {
    val input: String
}

enum class Cape(override val input: String) : InputOption {
    Veronica("0"),
    Outside("1"),
    Swirl("2"),
}

enum class KillMethod(override val input: String) : InputOption {
    OverHorns("4"),
    Chest("5"),
}

private fun <T : InputOption> restrictedInput(values: Array<T>, errorMessage: String): T {
    do {
        stdInput(values)?.let { return it }
        println(errorMessage)
    } while (true)
}

private fun <T : InputOption> stdInput(values: Array<T>): T? {
    print("? ")
    val z1 = readln()
    return values.firstOrNull { z1 == it.input }
}

enum class Yorn(override val input: String) : InputOption {
    YES("YES"), NO("NO");

    companion object {
        fun input(): Yorn {
            return restrictedInput(values(), "YES OR NO")
        }
    }
}

enum class FirstAct(val str: String) {
    picadores("PICADORES"), toreadores("TOREADORES");

    override fun toString() = str
}

fun fight(firstAct: FirstAct): Float {

    val b = 3.0 / bullQuality.level * RandomNumbers.nextFloat()
    val firstActQuality = when {
        b < .37 -> Quality.Awful
        b < .5 -> Quality.Poor
        b < .63 -> Quality.Fair
        b < .87 -> Quality.Good
        else -> Quality.Superb
    }
    val c = firstActQuality.level / 10f
    val t = firstActQuality.level
    println("THE $firstAct DID A $firstActQuality JOB.")

    if (t >= 4f) {
        if (t == 5f) {
            if (firstAct != FirstAct.toreadores) {
                println("${fna.asInteger} OF THE HORSES OF THE $firstAct KILLED.")
            }
            println("${fna.asInteger} OF THE $firstAct KILLED.")
        } else {
            println(
                when (fna) {
                    true -> "ONE OF THE $firstAct WAS KILLED."
                    false -> "NO $firstAct WERE KILLED."
                }
            )
        }
    }
    println()

    return c
}

private fun instructions() {
    print("DO YOU WANT INSTRUCTIONS? ")
    if (readln().trim() != "NO") {
        println("HELLO, ALL YOU BLOODLOVERS AND AFICIONADOS.")
        println("HERE IS YOUR BIG CHANCE TO KILL A BULL.")
        println()
        println("ON EACH PASS OF THE BULL, YOU MAY TRY")
        println("0 - VERONICA (DANGEROUS INSIDE MOVE OF THE CAPE)")
        println("1 - LESS DANGEROUS OUTSIDE MOVE OF THE CAPE")
        println("2 - ORDINARY SWIRL OF THE CAPE.")
        println()
        println("INSTEAD OF THE ABOVE, YOU MAY TRY TO KILL THE BULL")
        println("ON ANY TURN: 4 (OVER THE HORNS), 5 (IN THE CHEST).")
        println("BUT IF I WERE YOU,")
        println("I WOULDN'T TRY IT BEFORE THE SEVENTH PASS.")
        println()
        println("THE CROWD WILL DETERMINE WHAT AWARD YOU DESERVE")
        println("(POSTHUMOUSLY IF NECESSARY).")
        println("THE BRAVER YOU ARE, THE BETTER THE AWARD YOU RECEIVE.")
        println()
        println("THE BETTER THE JOB THE PICADORES AND TOREADORES DO,")
        println("THE BETTER YOUR CHANCES ARE.")
    }
    repeat(2) {
        println()
    }
}

fun intro() {
    println(" ".repeat(34) + "BULL")
    println(" ".repeat(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    println()
    println()
    println()
}
