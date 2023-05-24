package main

import (
	"bufio"
	"fmt"
	"math/rand"
	"os"
	"sort"
	"strconv"
	"strings"
	"time"
)

var welcome = `
Acey-Ducey is played in the following manner
The dealer (computer) deals two cards face up
You have an option to bet or not bet depending
on whether or not you feel the card will have
a value between the first two.
If you do not want to bet, input a 0
  `

func main() {
	rand.Seed(time.Now().UnixNano())
	scanner := bufio.NewScanner(os.Stdin)

	fmt.Println(welcome)

	for {
		play(100)
		fmt.Println("TRY AGAIN (YES OR NO)")
		scanner.Scan()
		response := scanner.Text()
		if strings.ToUpper(response) != "YES" {
			break
		}
	}

	fmt.Println("O.K., HOPE YOU HAD FUN!")
}

func play(money int) {
	scanner := bufio.NewScanner(os.Stdin)
	var bet int

	for {
		// Shuffle the cards
		cards := []int{1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}
		rand.Shuffle(len(cards), func(i, j int) { cards[i], cards[j] = cards[j], cards[i] })

		// Take the first two for the dealer and sort
		dealerCards := cards[0:2]
		sort.Ints(dealerCards)

		fmt.Printf("YOU NOW HAVE %d DOLLARS.\n\n", money)
		fmt.Printf("HERE ARE YOUR NEXT TWO CARDS:\n%s\n%s", getCardName(dealerCards[0]), getCardName(dealerCards[1]))
		fmt.Printf("\n\n")

		//Check if Bet is Valid
		for {
			fmt.Println("WHAT IS YOUR BET:")
			scanner.Scan()
			b, err := strconv.Atoi(scanner.Text())
			if err != nil {
				fmt.Println("PLEASE ENTER A POSITIVE NUMBER")
				continue
			}
			bet = b

			if bet == 0 {
				fmt.Printf("CHICKEN!\n\n")
				goto there
			}

			if (bet > 0) && (bet <= money) {
				break
			}
		}

		// Draw Players Card
		fmt.Printf("YOUR CARD: %s\n", getCardName(cards[2]))
		if (cards[2] > dealerCards[0]) && (cards[2] < dealerCards[1]) {
			fmt.Println("YOU WIN!!!")
			money = money + bet
		} else {
			fmt.Println("SORRY, YOU LOSE")
			money = money - bet
		}
		fmt.Println()

		if money <= 0 {
			fmt.Printf("%s\n", "SORRY, FRIEND, BUT YOU BLEW YOUR WAD.")
			return
		}
	there:
	}
}

func getCardName(c int) string {
	switch c {
	case 11:
		return "JACK"
	case 12:
		return "QUEEN"
	case 13:
		return "KING"
	case 14:
		return "ACE"
	default:
		return strconv.Itoa(c)
	}
}
