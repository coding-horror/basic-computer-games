package main

import (
	"bufio"
	"fmt"
	"math/rand"
	"os"
	"strconv"
	"strings"
)

func printWelcome() {
	fmt.Println("\n                   Dice")
	fmt.Println("Creative Computing  Morristown, New Jersey")
	fmt.Println()
	fmt.Println()
	fmt.Println("This program simulates the rolling of a")
	fmt.Println("pair of dice.")
	fmt.Println("You enter the number of times you want the computer to")
	fmt.Println("'roll' the dice.   Watch out, very large numbers take")
	fmt.Println("a long time.  In particular, numbers over 5000.")
	fmt.Println()
}

func main() {
	printWelcome()
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println("\nHow many rolls? ")
		scanner.Scan()
		numRolls, err := strconv.Atoi(scanner.Text())
		if err != nil {
			fmt.Println("Invalid input, try again...")
			continue
		}

		// We'll track counts of roll outcomes in a 13-element list.
		// The first two indices (0 & 1) are ignored, leaving just
		// the indices that match the roll values (2 through 12).
		results := make([]int, 13)

		for n := 0; n < numRolls; n++ {
			d1 := rand.Intn(6) + 1
			d2 := rand.Intn(6) + 1
			results[d1+d2] += 1
		}

		// Display final results
		fmt.Println("\nTotal Spots   Number of Times")
		for i := 2; i < 13; i++ {
			fmt.Printf(" %-14d%d\n", i, results[i])
		}

		fmt.Println("\nTry again? ")
		scanner.Scan()
		if strings.ToUpper(scanner.Text()) == "Y" {
			continue
		} else {
			os.Exit(1)
		}

	}
}
