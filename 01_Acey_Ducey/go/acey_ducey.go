package main

import (
	"fmt"
	"math/rand"
	"regexp"
	"strconv"
	"time"
)

var playerFunds = 100

var seed = rand.NewSource(time.Now().UnixNano())
var gen = rand.New(seed)

func main() {
	game(&playerFunds)
}

func game(playerFunds *int) {
	displayIntro()
	for *playerFunds > 0 {
		turn(playerFunds)
	}
}

func displayIntro() {
	fmt.Println("		ACEY DUCEY CARD GAME		")
	fmt.Println("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY")
	fmt.Println()
	fmt.Println()
	fmt.Println(
		`ACEY-DUCEY IS PLAYED IN THE FOLLOWING MANNER
THE DEALER (COMPUTER) DEALS TWO CARDS FACE UP
YOU HAVE AN OPTION TO BET OR TO NOT BET DEPENDING
ON WHETHER OR NOT YOU FEEL THE CARD WILL HAVE
A VALUE BETWEEN THE FIRST TWO.
IF YOU DO NOT WANT TO BET, INPUT A 0
YOU NOW HAVE 100 DOLLARS`)
}

func turn(playerFunds *int) {
	firstCard := drawCard()
	secondCard := drawCard()
	fmt.Println("The cards are:")
	fmt.Println(firstCard)
	fmt.Println(secondCard)
	fmt.Println()
	bet := takeUserBet(playerFunds)
	if bet != 0 {
		fmt.Printf("You bet $%v\n", bet)
		thirdCard := drawCard()
		fmt.Printf("The third card is: %v.\n", thirdCard)
		if (thirdCard > firstCard && thirdCard < secondCard) || (thirdCard < firstCard && thirdCard > secondCard) {
			fmt.Println("You win this round!  Collect your bet.")
			*playerFunds += bet
			fmt.Printf("You now have $%v\n", *playerFunds)
		} else {
			fmt.Println("You lost this round.  Sorry.")
			*playerFunds -= bet
			fmt.Printf("You now have $%v\n", *playerFunds)
		}
	}
	if bet == 0 {
		fmt.Println("CHICKEN!!")
	}

}

func takeUserBet(playerFunds *int) int {
	var betAmount int
	var userBetString string

	fmt.Println("What is your bet?")
	fmt.Scanln(&userBetString)

	ok, err := regexp.MatchString(`\d+`, userBetString)
	if err != nil {
		fmt.Printf("%v is not a valid input. Try again!", userBetString)
		takeUserBet(playerFunds)
	}
	if ok {
		userBetInt, err := strconv.Atoi(userBetString)
		if err != nil {
			fmt.Printf("Something went wrong.  Try again!")
			takeUserBet(playerFunds)
		}
		if userBetInt > *playerFunds {
			fmt.Printf("You can't bet more than you have!  Try again!")
		} else {
			return userBetInt
		}
	}

	return betAmount
}

func drawCard() int {
	return gen.Intn(13) + 1
}
