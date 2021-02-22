// Acey-Ducey
//
// Original source in BASIC by Bill Palmby and published in "Basic Computer Games"
// Cf. https://en.wikipedia.org/wiki/BASIC_Computer_Games
//
// Contributers to the Go interpretation are:
//     P. Todd Decker (https://github.com/ptdecker)

package main

import (
	"fmt"
	"math/rand"
	"strconv"
	"strings"
	"time"
)

// game constants
const startingBalance int = 100
const jack int = 11
const queen int = 12
const king int = 13
const ace int = 14 // Ace is high

// game state
var balance int
var cardDeck []int

// instructions() writes the instructions to standard output
func showInstructions() {
	fmt.Println("Acey Ducey Card Game")
	fmt.Printf("Creative Computing  Morristown, New Jersey\n\n\n")
	fmt.Printf("Acey-Ducey is played in the following manner:\n\n")
	fmt.Println("The dealer (computer) deals two cards, face up.")
	fmt.Println("You have an option to bet or pass, depending")
	fmt.Println("whether or not you feel the next card will have")
	fmt.Printf("a value between the first two.\n\n")
	fmt.Println("If the card is between, you will win your stake,")
	fmt.Println("otherwise you will lose it. Ace is 'high' (higher")
	fmt.Println("than a King). If you want to pass, enter a bet")
	fmt.Printf("amount of $0.\n\n")
}

// new() starts a new game
func newGame() {
	rand.Seed(time.Now().UnixNano())
	balance = 100
}

// shuffleDeck() resets the card deck and shuffles
func shuffleDeck() {
	fmt.Printf("... Shuffling the deck ...\n\n")
	cardDeck = cardDeck[:0] // Empties the deck slice
	for i := 0; i < 4; i++ {
		cardDeck = append(cardDeck, 2, 3, 4, 5, 6, 7, 8, 9, 10, jack, queen, king, ace)
	}
	rand.Shuffle(len(cardDeck), func(i, j int) { cardDeck[i], cardDeck[j] = cardDeck[j], cardDeck[i] })
}

// balance() writes player's current bank balance to standard output
func showBalance() {
	fmt.Printf("You now have %d dollars\n\n", balance)
}

// cardName() returns a proper name for a card
func cardName(card int) string {
	switch card {
	case jack:
		return "Jack"
	case queen:
		return "Queen"
	case king:
		return "King"
	case ace:
		return "Ace"
	default:
		return strconv.Itoa(card)
	}
}

// showFirstCards() shows the first two cards
func showFirstCards() {
	fmt.Printf("Here are your next two cards:\n\n")
	if cardDeck[0] < cardDeck[1] {
		fmt.Printf("\t%s\n\t%s\n\n", cardName(cardDeck[0]), cardName(cardDeck[1]))
	} else {
		fmt.Printf("\t%s\n\t%s\n\n", cardName(cardDeck[1]), cardName(cardDeck[0]))
	}
}

// showThirdCard() shows the third card
func showThirdCard() {
	fmt.Printf("The next card is a:\n\n")
	fmt.Printf("\t%s\n\n", cardName(cardDeck[2]))
}

// getBet() queries the player to entre a bet
func getBet() int {
	var bet int
	fmt.Print("What is your bet (to pass, enter 0)? ")
	fmt.Scanln(&bet)
	fmt.Println()
	return bet
}

// playerLost() returns true if the player lost
func playerLost() bool {
	if cardDeck[0] < cardDeck[1] && cardDeck[2] > cardDeck[0] && cardDeck[2] < cardDeck[1] {
		return false
	}
	if cardDeck[1] < cardDeck[0] && cardDeck[2] > cardDeck[1] && cardDeck[2] < cardDeck[0] {
		return false
	}
	return true
}

// Settles up by increasing player's balance if they won or decreasing it otherwise
func settleUp(bet int) {
	if playerLost() {
		fmt.Printf("Sorry, you lose!\n\n")
		balance -= bet
		return
	}
	fmt.Printf("You win!!!\n\n")
	balance += bet
}

// playerIsBroke() returns true if the player is out of money
func playerIsBroke() bool {
	return balance < 1
}

// donePlaying returns true if player wants to quit
func donePlaying() bool {
	var response string
	fmt.Print("Try again (Yes or No)? ")
	fmt.Scanln(&response)
	fmt.Println()
	return string(strings.ToUpper(response)[0]) == "N"
}

// main() program
func main() {
	showInstructions()
	newGame()
	for {
		shuffleDeck()
		showBalance()
		showFirstCards()
		bet := getBet()
		if bet == 0 {
			fmt.Printf("Chicken!!\n\n")
			continue
		}
		showThirdCard()
		settleUp(bet)
		if playerIsBroke() {
			fmt.Printf("Sorry, friend but you blew your wad\n\n")
			if donePlaying() {
				fmt.Printf("Okay hope you had fun\n\n")
				break
			}
			newGame()
		}
	}
}
