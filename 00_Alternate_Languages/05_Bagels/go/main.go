package main

import (
	"bufio"
	"fmt"
	"math/rand"
	"os"
	"strconv"
	"strings"
	"time"
)

const MAXGUESSES int = 20

func printWelcome() {
	fmt.Println("\n                Bagels")
	fmt.Println("Creative Computing  Morristown, New Jersey")
	fmt.Println()
}
func printRules() {
	fmt.Println()
	fmt.Println("I am thinking of a three-digit number.  Try to guess")
	fmt.Println("my number and I will give you clues as follows:")
	fmt.Println("   PICO   - One digit correct but in the wrong position")
	fmt.Println("   FERMI  - One digit correct and in the right position")
	fmt.Println("   BAGELS - No digits correct")
}

func getNumber() []string {
	numbers := []string{"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"}
	rand.Shuffle(len(numbers), func(i, j int) { numbers[i], numbers[j] = numbers[j], numbers[i] })

	return numbers[:3]
}

func getValidGuess(guessNumber int) string {
	var guess string
	scanner := bufio.NewScanner(os.Stdin)
	valid := false
	for !valid {
		fmt.Printf("Guess # %d?\n", guessNumber)
		scanner.Scan()
		guess = strings.TrimSpace(scanner.Text())

		// guess must be 3 characters
		if len(guess) == 3 {
			// and should be numeric
			_, err := strconv.Atoi(guess)
			if err != nil {
				fmt.Println("What?")
			} else {
				// and the numbers should be unique
				if (guess[0:1] != guess[1:2]) && (guess[0:1] != guess[2:3]) && (guess[1:2] != guess[2:3]) {
					valid = true
				} else {
					fmt.Println("Oh, I forgot to tell you that the number I have in mind")
					fmt.Println("has no two digits the same.")
				}
			}
		} else {
			fmt.Println("Try guessing a three-digit number.")
		}
	}

	return guess
}

func buildResultString(num []string, guess string) string {
	result := ""

	// correct digits in wrong place
	for i := 0; i < 2; i++ {
		if num[i] == guess[i+1:i+2] {
			result += "PICO "
		}
		if num[i+1] == guess[i:i+1] {
			result += "PICO "
		}
	}
	if num[0] == guess[2:3] {
		result += "PICO "
	}
	if num[2] == guess[0:1] {
		result += "PICO "
	}

	// correct digits in right place
	for i := 0; i < 3; i++ {
		if num[i] == guess[i:i+1] {
			result += "FERMI "
		}
	}

	// nothing right?
	if result == "" {
		result = "BAGELS"
	}

	return result
}

func main() {
	rand.Seed(time.Now().UnixNano())
	scanner := bufio.NewScanner(os.Stdin)

	printWelcome()

	fmt.Println("Would you like the rules (Yes or No)? ")
	scanner.Scan()
	response := scanner.Text()
	if len(response) > 0 {
		if strings.ToUpper(response[0:1]) != "N" {
			printRules()
		}
	} else {
		printRules()
	}

	gamesWon := 0
	stillRunning := true

	for stillRunning {
		num := getNumber()
		numStr := strings.Join(num, "")
		guesses := 1

		fmt.Println("\nO.K.  I have a number in mind.")
		guessing := true
		for guessing {
			guess := getValidGuess(guesses)

			if guess == numStr {
				fmt.Println("You got it!!")
				gamesWon++
				guessing = false
			} else {
				fmt.Println(buildResultString(num, guess))
				guesses++
				if guesses > MAXGUESSES {
					fmt.Println("Oh well")
					fmt.Printf("That's %d guesses. My number was %s\n", MAXGUESSES, numStr)
					guessing = false
				}
			}
		}

		validRespone := false
		for !validRespone {
			fmt.Println("Play again (Yes or No)?")
			scanner.Scan()
			response := scanner.Text()
			if len(response) > 0 {
				validRespone = true
				if strings.ToUpper(response[0:1]) != "Y" {
					stillRunning = false
				}
			}
		}
	}

	if gamesWon > 0 {
		fmt.Printf("\nA %d point Bagels buff!!\n", gamesWon)
	}

	fmt.Println("Hope you had fun.  Bye")
}
