package main

import (
	"fmt"
	"math/rand"
	"regexp"
	"strconv"
	"strings"
	"time"
)

type Game struct {
	secret      [3]int
	guess       [3]int
	turnCounter int
}

func main() {
	game := Game{}
	initTurnCounter(&game)
	printIntro()
	setSecretNumber(&game)
	fmt.Println("Secret Number: ", game.secret) //debug
	playTurn(&game)
}

func initTurnCounter(game *Game) {
	game.turnCounter = 1
}

func setSecretNumber(game *Game) {
	game.secret = pickSecretNumber()
}

func playTurn(game *Game) {
	userTurn(game)
	evaluateGuess(game)
}

func evaluateGuess(game *Game) {
	if game.guess == game.secret {
		fmt.Println("YOU GOT IT!!!")
	} else {
		checkForBagel(game)
		checkForPico(game)
		checkForFermi(game)
		fmt.Print("\n")
		playTurn(game)
	}
}

func checkForBagel(game *Game) {
	var matchesFound bool
	for i := 0; i < len(game.secret); i++ {
		for j := 0; j < len(game.guess); j++ {
			if game.guess[j] == game.secret[i] {
				matchesFound = true
			}
		}
	}
	if !matchesFound {
		fmt.Print("BAGEL ")
	}
}

func checkForPico(game *Game) {
	for i := 0; i < len(game.secret); i++ {
		for j := 0; j < len(game.guess); j++ {
			if game.guess[j] == game.secret[i] && game.guess[j] != game.secret[j] {
				fmt.Print("PICO ")
			}
		}
	}

}

func checkForFermi(game *Game) {
	for i := 0; i < len(game.secret); i++ {
		for j := 0; j < len(game.guess); j++ {
			if game.guess[j] == game.secret[i] && game.guess[j] == game.secret[j] {
				fmt.Print("FERMI ")
			}
		}
	}

}

func printIntro() {
	fmt.Println()
	fmt.Println("		BAGELS")
	fmt.Println("Creative Computing Morristown, New Jersey")
	fmt.Println()

	if promptForRulesDisplay() {
		displayRules()

	}

}

func userTurn(game *Game) bool {
	fmt.Printf("Guess #%v	?", game.turnCounter)
	var userGuessString string
	fmt.Scanln(&userGuessString)

	ok, err := regexp.MatchString(`[0-9]{3}`, userGuessString)
	if err != nil {
		fmt.Printf("%v is not a valid input. Try again!", userGuessString)
		userTurn(game)
	}
	if ok {
		fmt.Println("OK")
		sliceString := strings.Split(userGuessString, "")
		guessIntSlice := [3]int{}
		for i := 0; i < len(sliceString); i++ {
			x, err := strconv.ParseInt(sliceString[i], 10, 64)
			if err != nil {
				fmt.Println("Error parsing int from sliced string.")
			}
			guessIntSlice[i] = int(x)
		}
		game.guess = guessIntSlice
		game.turnCounter++
	}

	return true
}

func promptForRulesDisplay() bool {
	fmt.Println("Would you like to go over the rules (y/n)? ")
	var response string
	fmt.Scanln(&response)
	return response == "y"
}

func displayRules() {
	fmt.Println("I AM THINKING OF A THREE DIGIT NUMBER, TRY TO GUESS")
	fmt.Println("MY NUMBER AND I WILL GIVE YOU CLUES AS FOLLOWS:")
	fmt.Println("PICO 	- ONE DIGIT CORRECT BUT IN THE WRONG POSITION")
	fmt.Println("FERMI	- ONE DIGIT CORRECT AND IN THE RIGHT POSITION")
	fmt.Println("BAGELS	- NO DIGITS CORRECT")
}

func pickSecretNumber() [3]int {
	secNumber := [3]int{}
	for secNumber[0] == secNumber[1] ||
		secNumber[1] == secNumber[2] ||
		secNumber[2] == secNumber[0] {
		secNumber = generateSliceOfThreeRandInt()
	}
	fmt.Println("O.K.  I HAVE A NUMBER IN MIND.")
	return secNumber
}

func generateRandInt() int {
	seed := rand.NewSource(time.Now().UnixNano())
	r := rand.New(seed)
	return r.Intn(9)
}

func generateSliceOfThreeRandInt() [3]int {
	secNumber := [3]int{}
	secNumber[0] = generateRandInt()
	secNumber[1] = generateRandInt()
	secNumber[2] = generateRandInt()
	return secNumber
}

//Intro Display
//Option for rules printout.
//Rules Display

//Computer makes secret number
//Guess must be three digit number 000 - 999
//Guess must have no repeating numbers.

//Player makes guess
//If guess is correct, player wins
//If guess is incorrect, computer offers feedback:
//PICO		- one digit is correct, but in the wrong position
//FERMI		- One digit is in the correct position.
//BAGELS	- No digit is correct.
