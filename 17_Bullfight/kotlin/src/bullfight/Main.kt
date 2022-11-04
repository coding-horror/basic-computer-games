package bullfight

import bullfight.Yorn.NO
import kotlin.random.Random

val fna: Int get() = Random.nextInt(1, 2)

val l = listOf("SUPERB", "GOOD", "FAIR", "POOR", "AWFUL")
var aInt: Int = 0

fun main() {
    val d = mutableListOf(0f,0f,0f,0f,0f,0f)
    intro()
    instructions()
    d[5] = 1f
    d[4] = 1f

    aInt = Random.nextInt(1, 6)
    println("YOU HAVE DRAWN A ${l[aInt - 1]} BULL.")
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

    d[3]++
    println("PASS NUMBER ${d[3]}")
    if (d[3]>=3) {
        print("HERE COMES THE BULL.  TRY FOR A KILL")
        if (Yorn.input() == NO) {
            print("CAPE MOVE")
        }
    }
    else {
        println("THE BULL IS CHARGING AT YOU!  YOU ARE THE MATADOR--")
        print("DO YOU WANT TO KILL THE BULL")
        if (Yorn.input() == NO) {
            print("WHAT MOVE DO YOU MAKE WITH THE CAPE")
        }
    }


}

enum class Yorn(val s: String) {
    YES("YES"),NO("NO");
    override fun toString() = s

    companion object {
        fun input() : Yorn {
            do {
                print("? ")
                val z1 = readln()
                Yorn.values().firstOrNull { z1 == it.s }?.let { return it } ?: println("YES OR NO")
            } while (true)
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
    println("THE $firstAct DID A ${l[t - 1]} JOB.")

    if (t >= 4) {
        if (t == 5) {
            if (firstAct != FirstAct.toreadores) {
                println ("$fna OF THE HORSES OF THE $firstAct KILLED.")
            }
            println("$fna OF THE $firstAct KILLED.")
        }
        else {
            println(
                when (fna) {
                    1 -> "ONE OF THE $firstAct WAS KILLED."
                    2 -> "NO $firstAct WERE KILLED."
                    else -> ""
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
