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

const MAX_ATTEMPTS = 6

func printIntro() {
	fmt.Println("HI LO")
	fmt.Println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
	fmt.Println("\n\n\nTHIS IS THE GAME OF HI LO.")
	fmt.Println("\nYOU WILL HAVE 6 TRIES TO GUESS THE AMOUNT OF MONEY IN THE")
	fmt.Println("HI LO JACKPOT, WHICH IS BETWEEN 1 AND 100 DOLLARS.  IF YOU")
	fmt.Println("GUESS THE AMOUNT, YOU WIN ALL THE MONEY IN THE JACKPOT!")
	fmt.Println("THEN YOU GET ANOTHER CHANCE TO WIN MORE MONEY.  HOWEVER,")
	fmt.Println("IF YOU DO NOT GUESS THE AMOUNT, THE GAME ENDS.")
	fmt.Println()
	fmt.Println()
}

func main() {
	rand.Seed(time.Now().UnixNano())
	scanner := bufio.NewScanner(os.Stdin)

	printIntro()

	totalWinnings := 0

	for {
		fmt.Println()
		secret := rand.Intn(1000) + 1

		guessedCorrectly := false

		for attempt := 0; attempt < MAX_ATTEMPTS; attempt++ {
			fmt.Println("YOUR GUESS?")
			scanner.Scan()
			guess, err := strconv.Atoi(scanner.Text())
			if err != nil {
				fmt.Println("INVALID INPUT")
			}

			if guess == secret {
				fmt.Printf("GOT IT!!!!!!!!!!   YOU WIN %d DOLLARS.\n", secret)
				guessedCorrectly = true
				break
			} else if guess > secret {
				fmt.Println("YOUR GUESS IS TOO HIGH.")
			} else {
				fmt.Println("YOUR GUESS IS TOO LOW.")
			}
		}

		if guessedCorrectly {
			totalWinnings += secret
			fmt.Printf("YOUR TOTAL WINNINGS ARE NOW $%d.\n", totalWinnings)
		} else {
			fmt.Printf("YOU BLEW IT...TOO BAD...THE NUMBER WAS %d\n", secret)
		}

		fmt.Println()
		fmt.Println("PLAYAGAIN (YES OR NO)?")
		scanner.Scan()

		if strings.ToUpper(scanner.Text())[0:1] != "Y" {
			break
		}
	}
	fmt.Println("\nSO LONG.  HOPE YOU ENJOYED YOURSELF!!!")
}
