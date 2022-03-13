import java.util.Random

fun main() {
    println("BAGELS")
    println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    var a = mutableListOf(0, 0, 0)
    var y = 0
    var t = 255
    println()
    println()
    println()
    println("WOULD YOU LIKE THE RULES (YES OR NO)? ")
    var astring = readLine()
    if (astring?.substring(0, 1) != "N") {
        println()
        println()
        println("I AM THINKING OF A THREE-DIGIT NUMBER.  TRY TO GUESS")
        println("MY NUMBER AND I WILL GIVE YOU CLUES AS FOLLOWS:")
        println("   PICO   - ONE DIGIT CORRECT BUT IN THE WRONG POSITION")
        println("   FERMI  - ONE DIGIT CORRECT AND IN THE RIGHT POSITION")
        println("   BAGELS - NO DIGITS CORRECT")
    }
    var i = 0
    var random = Random()
    while (i < 3) {
        a[i] = random.nextInt(9) + 1
        if (i > 0) {
            for (j in 0 until i) {
                if (a[i] == a[j])
                {
                    a[i] = random.nextInt(9) + 1
                    i--
                }
            }
        }
        i++
    }
    println()
    println("O.K.  I HAVE A NUMBER IN MIND.")
    do {
        var i = 1
        while (i <= 20) {
            do {
                var repeatGuess = false
                println("GUESS #" + i)
                astring = readLine()
                if (astring?.length != 3) {
                    println("TRY GUESSING A THREE-DIGIT NUMBER.")
                    repeatGuess = true
                } else {
                    for (j in 0..2) {
                        if (astring[j].toInt() < 48 || astring[j].toInt() > 57) {
                            println("WHAT?")
                            repeatGuess = true
                        }
                    }

                    if (astring[0] == astring[1] ||
                        astring[1] == astring[2] ||
                        astring[2] == astring[0]
                    ) {
                        println("OH, I FORGOT TO TELL YOU THAT THE NUMBER I HAVE IN MIND")
                        println("HAS NO TWO DIGITS THE SAME.")
                        repeatGuess = true
                    }
                }
            } while (repeatGuess);

            var c = 0
            var d = 0
            for (j in 0..2) {
                if (astring!![j]?.toInt() - 48 == a[j]) {
                    c++
                    d++
                }
                if (j < 2 && astring!![j]?.toInt() - 48 == a[j + 1]) c++
            }
            if (astring!![2]?.toInt() - 48 == a[0]) c++
            if (astring!![0]?.toInt() - 48 == a[2]) c++

            if (d == 3) {
                break;
            }
            if (c != 0) {
                for (j in 1..c) {
                    print("PICO ")
                }
            }
            if (d != 0) {
                for (j in 1..d) {
                    print("FERMI ")
                }
            }
            if (c + d == 0) {
                print("BAGELS ")
            }
            println()
            i++
        }

        if (i == 21) {
            println("OH WELL.")
            println("THAT'S TWENTY GUESSES.  MY NUMBER WAS " + a[0] + a[1] + a[2])
        }
        else {
            println("YOU GOT IT!!!")
            println()
            y++
        }
        println("PLAY AGAIN (YES OR NO)?")
        astring = readLine()
    } while (astring?.substring(0, 1) == "Y");
    if (y != 0) {
        println()
        println("A $y POINT BAGELS BUFF!!")
    }
    println ("HOPE YOU HAD FUN.  BYE.")
}
