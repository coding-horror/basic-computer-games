package main

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"strconv"
)

func printWelcome() {
	fmt.Println("                 CHANGE")
	fmt.Println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
	fmt.Println()
	fmt.Println()
	fmt.Println()
	fmt.Println("I, YOUR FRIENDLY MICROCOMPUTER, WILL DETERMINE")
	fmt.Println("THE CORRECT CHANGE FOR ITEMS COSTING UP TO $100.")
	fmt.Println()
}

func computeChange(cost, payment float64) {
	change := int(math.Round((payment - cost) * 100))

	if change == 0 {
		fmt.Println("\nCORRECT AMOUNT, THANK YOU.")
		return
	}

	if change < 0 {
		fmt.Printf("\nSORRY, YOU HAVE SHORT-CHANGED ME $%0.2f\n", float64(change)/-100.0)
		print()
		return
	}

	fmt.Printf("\nYOUR CHANGE, $%0.2f:\n", float64(change)/100.0)

	d := change / 1000
	if d > 0 {
		fmt.Printf("  %d TEN DOLLAR BILL(S)\n", d)
		change -= d * 1000
	}

	d = change / 500
	if d > 0 {
		fmt.Printf("  %d FIVE DOLLAR BILL(S)\n", d)
		change -= d * 500
	}

	d = change / 100
	if d > 0 {
		fmt.Printf("  %d ONE DOLLAR BILL(S)\n", d)
		change -= d * 100
	}

	d = change / 50
	if d > 0 {
		fmt.Println("  1 HALF DOLLAR")
		change -= d * 50
	}

	d = change / 25
	if d > 0 {
		fmt.Printf("  %d QUARTER(S)\n", d)
		change -= d * 25
	}

	d = change / 10
	if d > 0 {
		fmt.Printf("  %d DIME(S)\n", d)
		change -= d * 10
	}

	d = change / 5
	if d > 0 {
		fmt.Printf("  %d NICKEL(S)\n", d)
		change -= d * 5
	}

	if change > 0 {
		fmt.Printf("  %d PENNY(S)\n", change)
	}
}

func main() {
	scanner := bufio.NewScanner(os.Stdin)

	printWelcome()

	var cost, payment float64
	var err error
	for {
		fmt.Println("COST OF ITEM?")
		scanner.Scan()
		cost, err = strconv.ParseFloat(scanner.Text(), 64)
		if err != nil || cost < 0.0 {
			fmt.Println("INVALID INPUT. TRY AGAIN.")
			continue
		}
		break
	}
	for {
		fmt.Println("\nAMOUNT OF PAYMENT?")
		scanner.Scan()
		payment, err = strconv.ParseFloat(scanner.Text(), 64)
		if err != nil {
			fmt.Println("INVALID INPUT. TRY AGAIN.")
			continue
		}
		break
	}

	computeChange(cost, payment)
	fmt.Println()
}
