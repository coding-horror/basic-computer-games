import java.util.Random

fun printCard(a: Int) {
    if (a < 11) println(a)
    if (a == 11) println("JACK")
    if (a == 12) println("QUEEN")
    if (a == 13) println("KING")
    if (a == 14) println("ACE")
}

fun main() {
    println("ACEY DUCEY CARD GAME")
    println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    println()
    println()
    println("ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER ")
    println("THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP")
    println("YOU HAVE AN OPTION TO BET OR NOT BET DEPENDING")
    println("ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE")
    println("A VALUE BETWEEN THE FIRST TWO.")
    println("IF YOU DO NOT WANT TO BET, INPUT A 0")
    var random = Random()
    do {
        var q = 100
        var a : Int
        var b : Int
        var m : Int
        println("YOU NOW HAVE " + q + " DOLLARS.")
        println()
        do {
            do {
                do {
                    println("HERE ARE YOUR NEXT TWO CARDS: ")
                    do {
                        a = random.nextInt(12) + 2
                        b = random.nextInt(12) + 2
                    } while (a >= b);
                    printCard(a)
                    printCard(b)
                    println()
                    println()
                    print("WHAT IS YOUR BET")
                    m = readLine()!!.toInt()
                    if (m == 0) {
                        println("CHICKEN!!")
                        println()
                    }
                } while (m == 0);
                if (m > q) {
                    println("SORRY, MY FRIEND, BUT YOU BET TOO MUCH.")
                    println("YOU HAVE ONLY " + q + " DOLLARS TO BET.")
                }
            } while (m > q);
            var c = random.nextInt(12) + 2
            printCard(c)
            println()
            if (c > a && c < b) {
                println("YOU WIN!!!")
                q += m
            }
            else {
                println("SORRY, YOU LOSE")
                if (m < q) q -= m
            }
        } while (m < q);
        println()
        println()
        println("SORRY, FRIEND, BUT YOU BLEW YOUR WAD.")
        println()
        println()
        println("TRY AGAIN (YES OR NO)")
    } while (readLine() == "YES");
    println("O.K., HOPE YOU HAD FUN!")
}
