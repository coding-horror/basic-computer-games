package king53

import kotlin.math.abs
import kotlin.random.Random
import kotlin.system.exitProcess

lateinit var gameState: GameState
const val KEEP_ORIGINAL_BUGS = false
const val KEEP_ORIGINAL_SUICIDE_REFERENCE = false

val rnd: Double get() = Random.nextDouble()
fun tab(i: Int) = " ".repeat(i)
class EndOfInputException : Throwable()

fun main() {
    header()

    print("DO YOU WANT INSTRUCTIONS? ")
    readLine()?.apply {
        gameState = if (startsWith("AGAIN")) loadOldGame() else GameState()
        if (startsWith("Y")) instructions(gameState.yearsRequired)
    }
        ?: throw EndOfInputException()

    with(gameState) {
        do {

            recalculateLandCost()
            displayStatus()
            inputLandSale()
            performLandSale()
            inputWelfare()
            performWelfare()
            inputPlantingArea()
            performPlanting()
            inputPollutionControl()
            if (zeroInput()) {
                displayExitMessage()
                exitProcess(0)
            }
            val yearResult = simulateOneYear().also {
                it.displayConsequences()
            }
        } while (yearResult == YearOutcome.ContinueNextYear)
    }
}

private fun header() {
    println("${tab(34)}KING")
    println("${tab(14)}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    println()
    println()
    println()
}

fun instructions(yearsRequired: Int) {
    println(
        """


        CONGRATULATIONS! YOU'VE JUST BEEN ELECTED PREMIER OF SETATS
        DETINU, A SMALL COMMUNIST ISLAND 30 BY 70 MILES LONG. YOUR
        JOB IS TO DECIDE UPON THE CONTRY'S BUDGET AND DISTRIBUTE
        MONEY TO YOUR COUNTRYMEN FROM THE COMMUNAL TREASURY.
        THE MONEY SYSTEM IS RALLODS, AND EACH PERSON NEEDS 100
        RALLODS PER YEAR TO SURVIVE. YOUR COUNTRY'S INCOME COMES
        FROM FARM PRODUCE AND TOURISTS VISITING YOUR MAGNIFICENT
        FORESTS, HUNTING, FISHING, ETC. HALF YOUR LAND IS FARM LAND
        WHICH ALSO HAS AN EXCELLENT MINERAL CONTENT AND MAY BE SOLD
        TO FOREIGN INDUSTRY (STRIP MINING) WHO IMPORT AND SUPPORT
        THEIR OWN WORKERS. CROPS COST BETWEEN 10 AND 15 RALLODS PER
        SQUARE MILE TO PLANT.
        YOUR GOAL IS TO COMPLETE YOUR $yearsRequired YEAR TERM OF OFFICE.
        GOOD LUCK!
        """.trimIndent()
    )
}

fun loadOldGame(): GameState = GameState().apply {

    do {
        var retry = false
        print("HOW MANY YEARS HAD YOU BEEN IN OFFICE WHEN INTERRUPTED? ")
        currentYear = validatedInput { it > 0 }

        if (currentYear >= yearsRequired) {
            println("   COME ON, YOUR TERM IN OFFICE IS ONLY $yearsRequired YEARS.")
            retry = true
        }

    } while (retry)

    print("HOW MUCH DID YOU HAVE IN THE TREASURY? ")
    rallods = validatedInput { it >= 0 }

    print("HOW MANY COUNTRYMEN? ")
    countrymen = validatedInput { it >= 0 }

    print("HOW MANY WORKERS? ")
    foreignWorkers = validatedInput { it >= 0 }

    do {
        var retry = false
        print("HOW MANY SQUARE MILES OF LAND? ")
        landArea = validatedInput { it >= 0 }

        if (landArea > 2000 || landArea <= 1000) {
            println("   COME ON, YOU STARTED WITH 1000 SQ. MILES OF FARM LAND")
            println("   AND 10,000 SQ. MILES OF FOREST LAND.")
            retry = true
        }
    } while (retry)

}


/**
 * Possible outcomes for a year.
 */
sealed class YearOutcome {

    /**
     * Display output for the end of the year, for each different possible
     * year outcome.
     */
    open fun displayConsequences() {
        // Default display nothing
    }

    fun finalFate() {
        if (rnd < .5) {
            println("YOU HAVE BEEN THROWN OUT OF OFFICE AND ARE NOW")
            println("RESIDING IN PRISON.")
        } else {
            println("YOU HAVE BEEN ASSASSINATED.")
        }
        println()
        println()
    }

    object ContinueNextYear : YearOutcome()

    class Win(private val yearsRequired: Int) : YearOutcome() {
        override fun displayConsequences() {
            // The misspelling of "successfully" is in the original code.
            println(
                """

                CONGRATULATIONS!!!!!!!!!!!!!!!!!!
                YOU HAVE SUCCESFULLY COMPLETED YOUR $yearsRequired YEAR TERM
                OF OFFICE. YOU WERE, OF COURSE, EXTREMELY LUCKY, BUT
                NEVERTHELESS, IT'S QUITE AN ACHIEVEMENT. GOODBYE AND GOOD
                LUCK - YOU'LL PROBABLY NEED IT IF YOU'RE THE TYPE THAT
                PLAYS THIS GAME.


            """.trimIndent()
            )
        }
    }

    class ExtremeMismanagement(private val death: Int) : YearOutcome() {
        override fun displayConsequences() {
            println()
            println("$death COUNTRYMEN DIED IN ONE YEAR!!!!!")
            println("DUE TO THIS EXTREME MISMANAGEMENT, YOU HAVE NOT ONLY")
            println("BEEN IMPEACHED AND THROWN OUT OF OFFICE, BUT YOU")
            println(
                when ((rnd * 10.0).toInt()) {
                    in 0..3 -> "ALSO HAD YOUR LEFT EYE GOUGED OUT!"
                    in 4..6 -> "HAVE ALSO GAINED A VERY BAD REPUTATION."
                    else -> "HAVE ALSO BEEN DECLARED NATIONAL FINK."
                }
            )
        }
    }

    object TooManyPeopleDead : YearOutcome() {
        // The mistyping of "population" is in the original game.
        override fun displayConsequences() {
            println(
                """


            OVER ONE THIRD OF THE POPULTATION HAS DIED SINCE YOU
            WERE ELECTED TO OFFICE. THE PEOPLE (REMAINING)
            HATE YOUR GUTS.
        """.trimIndent()
            )
            finalFate()
        }
    }

    object AntiImmigrationRevolution : YearOutcome() {
        override fun displayConsequences() {
            println(
                """
            THE NUMBER OF FOREIGN WORKERS HAS EXCEEDED THE NUMBER
            OF COUNTRYMEN. AS A MINORITY, THEY HAVE REVOLTED AND
            TAKEN OVER THE COUNTRY.
        """.trimIndent()
            )
            finalFate()
        }
    }

    object StarvationWithFullTreasury : YearOutcome() {
        override fun displayConsequences() {
            println(
                if (KEEP_ORIGINAL_SUICIDE_REFERENCE) {
                    """
                    MONEY WAS LEFT OVER IN THE TREASURY WHICH YOU DID
                    NOT SPEND. AS A RESULT, SOME OF YOUR COUNTRYMEN DIED
                    OF STARVATION. THE PUBLIC IS ENRAGED AND YOU HAVE
                    BEEN FORCED TO EITHER RESIGN OR COMMIT SUICIDE.
                    THE CHOICE IS YOURS.
                    IF YOU CHOOSE THE LATTER, PLEASE TURN OFF YOUR COMPUTER
                    BEFORE PROCEEDING.
                """.trimIndent()
                } else {
                    """
                    MONEY WAS LEFT OVER IN THE TREASURY WHICH YOU DID
                    NOT SPEND. AS A RESULT, SOME OF YOUR COUNTRYMEN DIED
                    OF STARVATION. THE PUBLIC IS ENRAGED AND YOU HAVE
                    BEEN FORCED TO RESIGN.
                    PLEASE TURN OFF YOUR COMPUTER AND SURRENDER IT TO
                    THE NEAREST POLICE STATION.
                """.trimIndent()
                }
            )
        }
    }

}

/**
 * Record data, allow data input, and process the simulation for the game.
 */
class GameState(val yearsRequired: Int = 8) {

    /**
     * The current year. Years start with zero, but we never
     * output the current year.
     */
    var currentYear = 0

    /**
     * Keep track of each year's crop loss, so we can report increases.
     */
    private var lastYearsCropLoss: Int = 0

    /**
     * Number of countrymen who have died of either pollution
     * or starvation this year.
     *  It costs 9 rallods to bury a body.
     *  If you lose 200 people in one year, you will throw an {@see ExtremeMismanagementException}
     */
    private var death = 0

    /**
     * Last year's tourist numbers. Use this to check whether the number
     * of tourists has gone up or down each year.
     */
    private var tourists = 0

    private var moneySpentOnPollutionControl = 0

    private var moneySpentOnPlanting = 0
    /**
     * Current stock of rallods.
     * Player starts with between 59000 and 61000 rallods, but
     * mostly distributed close to 60000. 75% of the time it's
     * between 59500 and 60500.
     */
    var rallods = (60000.0 + (1000.0 * rnd) - (1000.0 * rnd)).toInt()

    /**
     * Population.
     * Initial population is about 500.
     * 75% of the time it's between 495 and 505.
     */
    var countrymen = (500 + (10 * rnd) - (10 * rnd)).toInt()

    /**
     * Land sale price is evenly between 95 and 104 rallods per
     * square mile.
     * Price doesn't change over the course of the game.
     */
    private var landPrice = (10 * rnd + 95).toInt()

    private var plantingArea = 0

    private var welfareThisYear = 0
    /**
     * Land area in square miles. Arable land is 1000 square miles less.
     * Almost all calculations use landArea-1000 because only arable
     * land is of any use.
     */
    var landArea = 2000

    /**
     * Number of foreigners brought in by companies to whom you
     * have sold land. If this gets higher than your population, there will
     * be a revolution.
     */
    var foreignWorkers = 0

    /**
     * Planting cost is recalculated every year.
     */
    private var costToPlant: Int = 1

    /**
     * There is a brief explanation of land selling only
     * on the first turn.
     */
    private var explanationOfSellingGiven = false

    private var sellThisYear: Int = 0

    /**
     * Planting cost is recalculated every year
     * at between 10 and 14 rallods.
     */
    fun recalculateLandCost() {
        costToPlant = ((rnd / 2.0) * 10.0 + 10.0).toInt()
    }

    /**
     * Show the current status of the world.
     */
    fun displayStatus() {
        println()
        println("YOU NOW HAVE $rallods RALLODS IN THE TREASURY.")
        print("$countrymen COUNTRYMEN, ")
        if (foreignWorkers != 0) {
            println("$foreignWorkers FOREIGN WORKERS, ")
        }
        println("AND $landArea SQ. MILES OF LAND.")
        println("THIS YEAR INDUSTRY WILL BUY LAND FOR $landPrice")
        println("RALLODS PER SQUARE MILE.")
        println("LAND CURRENTLY COSTS $costToPlant RALLODS PER SQUARE MILE TO PLANT.")
    }

    fun displayExitMessage() {
        println()
        println("GOODBYE.")
        println("(IF YOU WISH TO CONTINUE THIS GAME AT A LATER DATE, ANSWER")
        println("'AGAIN' WHEN ASKED IF YOU WANT INSTRUCTIONS AT THE START")
        println("OF THE GAME).")
    }

    fun performLandSale() {
        landArea -= sellThisYear
        rallods += sellThisYear * landPrice
    }

    fun performPlanting() {
        rallods -= moneySpentOnPlanting
    }

    fun performWelfare() {
        rallods -= welfareThisYear
    }

    /**
     * Ask how much land we want to sell. Immediately get the money.
     * The player has to do the calculations to work out how much
     * money that makes.
     */
    fun inputLandSale() {
        do {
            print("HOW MANY SQUARE MILES DO YOU WISH TO SELL TO INDUSTRY? ")
            sellThisYear = numberInput()
            if (sellThisYear > landArea - 1000) {
                println("***  THINK AGAIN. YOU ONLY HAVE ${landArea - 1000} SQUARE MILES OF FARM LAND.")
                if (!explanationOfSellingGiven) {
                    println()
                    println("(FOREIGN INDUSTRY WILL ONLY BUY FARM LAND BECAUSE")
                    println("FOREST LAND IS UNECONOMICAL TO STRIP MINE DUE TO TREES,")
                    println("THICKER TOP SOIL, ETC.)")
                    explanationOfSellingGiven = true
                }
            }
        } while (sellThisYear < 0 || sellThisYear > landArea - 1000)
    }


    /**
     * Input the value of `welfareThisYear`
     */
    fun inputWelfare() {
        do {
            var retry = false
            print("HOW MANY RALLODS WILL YOU DISTRIBUTE AMONG YOUR COUNTRYMEN? ")
            welfareThisYear = numberInput()

            if (welfareThisYear > rallods) {
                println("   THINK AGAIN. YOU'VE ONLY $rallods RALLODS IN THE TREASURY")
                retry = true
            }

            if (welfareThisYear < 0) {
                retry = true
            }
        } while (retry)
    }

    /**
     * Get the number of square miles to plant this year.
     * Validate the response:
     *  Each countryman can only plant 2 square miles.
     *  You can only plant on arable land.
     *  You may not spend more on planting than your treasury.
     */
    fun inputPlantingArea() {
        if (welfareThisYear == rallods) {
            plantingArea = 0
        } else {
            do {
                var retry = false
                print("HOW MANY SQUARE MILES DO YOU WISH TO PLANT? ")
                plantingArea = numberInput()
                val moneySpentOnPlanting = plantingArea * costToPlant

                if (plantingArea < 0) {
                    retry = true
                } else if (plantingArea >= 0 && plantingArea > countrymen * 2) {
                    println("   SORRY, BUT EACH COUNTRYMAN CAN ONLY PLANT 2 SQ. MILES.")
                    retry = true
                } else if (plantingArea > landArea - 1000) {
                    println("   SORRY, BUT YOU'VE ONLY ${landArea - 1000} SQ. MILES OF FARM LAND.")
                    retry = true
                } else if (moneySpentOnPlanting > rallods) {
                    println("   THINK AGAIN. YOU'VE ONLY $rallods RALLODS LEFT IN THE TREASURY.")
                    retry = true
                }
            } while (retry)
        }

    }

    /**
     * Enter amount for pollution control.
     * Validate that this does not exceed treasury.
     */
    fun inputPollutionControl() {
        do {
            var retry = false
            print("HOW MANY RALLODS DO YOU WISH TO SPEND ON POLLUTION CONTROL? ")
            moneySpentOnPollutionControl = numberInput()

            if (rallods < 0) {
                retry = true
            } else if (moneySpentOnPollutionControl > rallods) {
                println("   THINK AGAIN. YOU ONLY HAVE $rallods RALLODS REMAINING.")
                retry = true
            }

        } while (retry)
    }

    /**
     * @return true if all data entered so far has been zero.
     */
    fun zeroInput() = sellThisYear == 0 &&
            welfareThisYear == 0 &&
            plantingArea == 0 &&
            moneySpentOnPollutionControl == 0

    fun simulateOneYear(): YearOutcome {
        rallods -= moneySpentOnPollutionControl
        val rallodsAfterPollutionControl = rallods

        var starvationDeaths = 0
        if (welfareThisYear / 100.0 - countrymen < 0) {

            /*
            Wait, WHAT?
            If you spend less than 5000 rallods on welfare, no matter the current size of the
            population, then you will end the game, with the game claiming that too many
            people have died, without showing exactly how many have died?

            https://github.com/coding-horror/basic-computer-games/blob/main/53_King/king.bas#:~:text=1105%20IF%20I/100%3C50%20THEN%201700
             */
            if (welfareThisYear / 100.0 < 50)
                return YearOutcome.TooManyPeopleDead

            starvationDeaths = (countrymen - (welfareThisYear / 100.0)).toInt()
            println("$starvationDeaths COUNTRYMEN DIED OF STARVATION")
        }

        var pollutionDeaths = (rnd * (2000 - landArea)).toInt()
        if (moneySpentOnPollutionControl >= 25) {
            pollutionDeaths = (pollutionDeaths / (moneySpentOnPollutionControl / 25.0)).toInt()
        }

        if (pollutionDeaths > 0) {
            println("$pollutionDeaths COUNTRYMEN DIED OF CARBON-MONOXIDE AND DUST INHALATION")
        }

        death = pollutionDeaths + starvationDeaths
        if (death > 0) {
            println("   YOU WERE FORCED TO SPEND ${death * 9}")
            println("RALLODS ON FUNERAL EXPENSES")
            rallods -= death * 9
        }

        if (rallods < 0) {
            println("   INSUFFICIENT RESERVES TO COVER COST - LAND WAS SOLD")
            landArea += rallods / landPrice
            rallods = 1
        }

        countrymen -= death

        val newForeigners =
            if (sellThisYear > 0) {
                (sellThisYear + rnd * 10.0 + rnd * 20.0).toInt() + (if (foreignWorkers <= 0) 20 else 0)
            } else 0

        /*
        Immigration is calculated as
            One for every thousand rallods more welfare than strictly required
            minus one for every 10 starvation deaths
            plus One for every 25 rallods spent on pollution control
            plus one for every 50 square miles of arable land
            minus one for every 2 pollution deaths
         */
        val immigration = (
                (welfareThisYear / 100.0 - countrymen) / 10.0 +
                        moneySpentOnPollutionControl / 25.0 -
                        (2000 - landArea) / 50.0 -
                        pollutionDeaths / 2.0
                ).toInt()
        println(
            "$newForeigners WORKERS CAME TO THE COUNTRY AND" +
                    " ${abs(immigration)} COUNTRYMEN ${if (immigration < 0) "LEFT" else "CAME TO"}" +
                    " THE ISLAND."
        )

        countrymen += immigration
        foreignWorkers += newForeigners

        /*
        Crop loss is between 75% and 125% of the land sold to industry,
        due to the pollution that industry causes.
        Money spent on pollution control reduces pollution deaths among
        the population, but does not affect crop losses.
         */
        var cropLoss = ((2000 - landArea) * (rnd + 1.5) / 2.0).toInt()
        if (foreignWorkers > 0)
            print("OF $plantingArea SQ. MILES PLANTED,")
        if (plantingArea <= cropLoss)
            cropLoss = plantingArea
        val cropLossWorse = cropLoss > lastYearsCropLoss
        lastYearsCropLoss = cropLoss
        println(" YOU HARVESTED ${plantingArea - cropLoss} SQ. MILES OF CROPS.")

        if (cropLoss > 0) {
            println("   (DUE TO ${if (cropLossWorse) "INCREASED " else ""}AIR AND WATER POLLUTION FROM FOREIGN INDUSTRY)")
        }

        val agriculturalIncome = ((plantingArea - cropLoss) * landPrice / 2.0).toInt()
        println("MAKING $agriculturalIncome RALLODS.")
        rallods += agriculturalIncome

        val v1 = (((countrymen - immigration) * 22.0) + rnd * 500).toInt()
        val v2 = ((2000.0 - landArea) * 15.0).toInt()
        println(" YOU MADE ${abs(v1 - v2)} RALLODS FROM TOURIST TRADE.")
        if (v2 != 0 && v1 - v2 < tourists) {
            print("   DECREASE BECAUSE ")
            println(
                when ((10 * rnd).toInt()) {
                    in 0..2 -> "FISH POPULATION HAS DWINDLED DUE TO WATER POLLUTION."
                    in 3..4 -> "AIR POLLUTION IS KILLING GAME BIRD POPULATION."
                    in 5..6 -> "MINERAL BATHS ARE BEING RUINED BY WATER POLLUTION."
                    in 7..8 -> "UNPLEASANT SMOG IS DISCOURAGING SUN BATHERS."
                    else -> "HOTELS ARE LOOKING SHABBY DUE TO SMOG GRIT."
                }
            )
        }

        /*
         The original code was incorrect.
         If v3 starts at 0, for example, our money doubles, when we
         have already been told that "YOU MADE ${abs(v1 - v2)} RALLODS
         FROM TOURIST TRADE"

        See the original code
            1450 V3=INT(A+V3)
            1451 A=INT(A+V3)

            https://github.com/coding-horror/basic-computer-games/blob/main/53_King/king.bas#:~:text=1450%20V3%3DINT,INT(A%2BV3)
         */
        if (KEEP_ORIGINAL_BUGS) {
            tourists += rallods
        } else {
            tourists = abs(v1 - v2)
        }
        rallods += tourists

        return if (death > 200)
            YearOutcome.ExtremeMismanagement(death)
        else if (countrymen < 343)
            YearOutcome.TooManyPeopleDead
        else if (rallodsAfterPollutionControl / 100 > 5 && death - pollutionDeaths >= 2)
            YearOutcome.StarvationWithFullTreasury
        else if (foreignWorkers > countrymen)
            YearOutcome.AntiImmigrationRevolution
        else {
            if (currentYear++ > yearsRequired)
                YearOutcome.Win(yearsRequired)
            else
                YearOutcome.ContinueNextYear
        }

    }
}

private fun numberInput() = try {
    readLine()?.toInt() ?: throw EndOfInputException()
} catch (r: NumberFormatException) {
    0
}

class DataEntryValidationException : Throwable()
private fun validatedInput(predicate : (Int)->Boolean) =
    numberInput().apply { if (!predicate(this)) throw DataEntryValidationException() }
