package bullfight

import bullfight.Yorn.*
import kotlin.random.Random
import kotlin.system.exitProcess

private val Float.squared: Float
    get() = this * this
val fna: Boolean get() = Random.nextBoolean()

val quality = listOf("SUPERB", "GOOD", "FAIR", "POOR", "AWFUL")
var aInt: Int = 0
var l = 1f
var momentOfTruth = false
val d = mutableListOf(0f, 0f, 0f, 0f, 0f, 0f)

fun main() {
    intro()
    instructions()
    d[5] = 1f
    d[4] = 1f

    aInt = Random.nextInt(1, 6)
    println("YOU HAVE DRAWN A ${quality[aInt - 1]} BULL.")
    when {
        aInt < 2 -> {
            println("GOOD LUCK.  YOU'LL NEED IT.")
        }

        aInt > 4 -> {
            println("YOU'RE LUCKY.")
        }

        else -> Unit
    }

    d[1] = fight(FirstAct.picadores)
    d[2] = fight(FirstAct.toreadores)
    repeat(2) { println() }
    var gored: Boolean

    gameLoop@ do {
        d[3]++
        println("PASS NUMBER ${d[3].toInt()}")
        if (d[3] >= 3) {
            print("HERE COMES THE BULL.  TRY FOR A KILL")
            gored = killAttempt("CAPE MOVE")
        } else {
            println("THE BULL IS CHARGING AT YOU!  YOU ARE THE MATADOR--")
            print("DO YOU WANT TO KILL THE BULL")
            gored = killAttempt("WHAT MOVE DO YOU MAKE WITH THE CAPE")
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
            val f = (6 - aInt + m / 10f) * Random.nextFloat() / ((d[1] + d[2] + d[3] / 10f) * 5f)
            if (f < 0.51)
                continue
        }

        println("THE BULL HAS GORED YOU!")
        goreLoop@ do {
            when (fna) {
                false -> {
                    println("YOU ARE DEAD.")
                    d[4] = 1.5f
                }

                true -> {
                    println("YOU ARE STILL ALIVE.")
                    println()
                    print("DO YOU RUN FROM THE RING")
                    when (Yorn.input()) {

                        YES -> {
                            println("COWARD")
                            d[4] = 0f
                        }

                        NO -> {
                            println("YOU ARE BRAVE.  STUPID, BUT BRAVE.")
                            when (fna) {
                                true -> {
                                    d[4] = 2f
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
        (d[1] + d[2]) * 2.5 +
        4 * d[4] +
        2 * d[5] -
        d[3].squared / 120f -
        aInt

fun fnc() = fnd() * Random.nextFloat()

private fun killAttempt(capeMessage: String): Boolean {
    when (Yorn.input()) {

        YES ->
            when (momentOfTruth()) {
                KillResult.Success -> {
                    println()
                    println()
                    if (d[4] == 0f) {
                        println(
                            """
                            THE CROWD BOOS FOR TEN MINUTES.  IF YOU EVER DARE TO SHOW
                            YOUR FACE IN A RING AGAIN, THEY SWEAR THEY WILL KILL YOU--
                            UNLESS THE BULL DOES FIRST.
                            """.trimIndent()
                        )
                    } else {
                        if (d[4] == 2f)
                            println("THE CROWD CHEERS WILDLY!")
                        else
                            if (d[5] == 2f) {
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

                KillResult.Fail -> return true
            }

        NO ->
            print(capeMessage)

    }
    return false
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

    val k = (6 - aInt) * 10 * Random.nextFloat() / ((d[1] + d[2]) * 5 * d[3])

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
        d[5] = 2f
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

private fun <T : InputOption> stdInput(values: Array<T>): T? {
    print("? ")
    val z1 = readln()
    return values.firstOrNull { z1 == it.input }
}

private fun <T : InputOption> restrictedInput(values: Array<T>, errorMessage: String): T {
    do {
        stdInput(values)?.let { return it }
        println(errorMessage)
    } while (true)
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

    val b = 3.0 / aInt * Random.nextFloat()
    val c = when {
        b < .37 -> .5f
        b < .5 -> .4f
        b < .63 -> .3f
        b < .87 -> .2f
        else -> .1f
    }
    val t = (10 * c + .2).toInt()
    println("THE $firstAct DID A ${quality[t - 1]} JOB.")

    if (t >= 4) {
        if (t == 5) {
            if (firstAct != FirstAct.toreadores) {
                println("$fna OF THE HORSES OF THE $firstAct KILLED.")
            }
            println("$fna OF THE $firstAct KILLED.")
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
    repeat(3) {
        println()
    }
}
