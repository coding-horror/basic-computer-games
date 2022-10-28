package main

import (
	"bufio"
	"fmt"
	"math"
	"math/rand"
	"os"
	"strconv"
	"time"
)

func printIntro() {
	fmt.Println("                   Guess")
	fmt.Println("Creative Computing  Morristown, New Jersey")
	fmt.Println()
	fmt.Println()
	fmt.Println()
	fmt.Println("This is a number guessing game. I'll think")
	fmt.Println("of a number between 1 and any limit you want.")
	fmt.Println("Then you have to guess what it is")
}

func getLimit() (int, int) {
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println("What limit do you want?")
		scanner.Scan()

		limit, err := strconv.Atoi(scanner.Text())
		if err != nil || limit < 0 {
			fmt.Println("Please enter a number greater or equal to 1")
			continue
		}

		limitGoal := int((math.Log(float64(limit)) / math.Log(2)) + 1)
		return limit, limitGoal
	}

}

func main() {
	rand.Seed(time.Now().UnixNano())
	printIntro()

	scanner := bufio.NewScanner(os.Stdin)

	limit, limitGoal := getLimit()

	guessCount := 1
	stillGuessing := true
	won := false
	myGuess := int(float64(limit)*rand.Float64() + 1)

	fmt.Printf("I'm thinking of a number between 1 and %d\n", limit)
	fmt.Println("Now you try to guess what it is.")

	for stillGuessing {
		scanner.Scan()
		n, err := strconv.Atoi(scanner.Text())
		if err != nil {
			fmt.Println("Please enter a number greater or equal to 1")
			continue
		}

		if n < 0 {
			break
		}

		fmt.Print("\n\n\n")
		if n < myGuess {
			fmt.Println("Too low. Try a bigger answer")
			guessCount += 1
		} else if n > myGuess {
			fmt.Println("Too high. Try a smaller answer")
			guessCount += 1
		} else {
			fmt.Printf("That's it! You got it in %d tries\n", guessCount)
			won = true
			stillGuessing = false
		}
	}

	if won {
		if guessCount < limitGoal {
			fmt.Println("Very good.")
		} else if guessCount == limitGoal {
			fmt.Println("Good.")
		} else {
			fmt.Printf("You should have been able to get it in only %d guesses.\n", limitGoal)
		}
		fmt.Print("\n\n\n")
	}
}
