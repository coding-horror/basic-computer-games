
fun main() {
    println(introText)
    var winnings = 0
    do {
        winnings += playGame()
        println("YOUR TOTAL WINNINGS ARE NOW $winnings DOLLARS")
    } while(playAgain())
    
    println("SO LONG.  HOPE YOU ENJOYED YOURSELF!!!")
}

fun playGame():Int {
    val amount = (1..100).random()
    repeat(6) {
        println("YOUR GUESS")
        val guess = readln().toInt()
        when {
            guess == amount -> {
                println("GOT IT!!!!!!!! YOU WIN $amount DOLLARS.")
                return amount
            }
            guess > amount -> println("YOUR GUESS IS TOO HIGH")
            else -> println("YOUR GUESS IS TOO LOW")
        }
    }
    println("YOU BLEW IT...TOO BAD...THE NUMBER WAS $amount")
    return 0
}

fun playAgain():Boolean {
    println("PLAY AGAIN (YES OR NO)")
    return readLine()?.uppercase() == "YES"
}


val introText = """
    HI LO
    CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY
    
    
    THIS IS THE GAME OF HI LO.
    
    YOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE
    HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS.  IF YOU
    GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!
    THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,
    IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS

""".trimIndent()