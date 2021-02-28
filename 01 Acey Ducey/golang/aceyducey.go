package main

import (
  "fmt"
  "math/rand"
  "time"
  "sort"
  "strings"
)


const defaultBakroll = 100

var (
  cardsNames = []string {"2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"}
)

type GameState struct {
  bankRoll int
  deck []int
  tableCards []int
  playerCard int
}

func getCardName(number int) string {
  return cardsNames[number]
}

func (gs GameState) displayBankRoll() {
  if gs.bankRoll > 0 {
    fmt.Printf("You now have %d dollars\n", gs.bankRoll)
  }
}

func (gs* GameState) shuffleDeck() {
  rand.Seed(time.Now().UnixNano())
  rand.Shuffle(len(gs.deck), func(i, j int) { gs.deck[i], gs.deck[j] = gs.deck[j], gs.deck[i] })
}


func (gs* GameState) gameLoop() {
  gs.displayBankRoll()
  for {
    fmt.Println("")
    fmt.Printf("Here are you next two cards\n")
    gs.shuffleDeck()
    gs.tableCards = gs.deck[0:2]
    sort.Ints(gs.tableCards)
    for _, cr := range gs.tableCards {
      fmt.Println(" ", getCardName(cr))
    }

    validBet := false
    for {
      fmt.Print("What is your bet? ")
      var bet int
      fmt.Scanln(&bet)
      if bet == 0 {
        validBet = true
        fmt.Println("Chicken!!")
      } else if bet > gs.bankRoll {
        fmt.Println("Sorry, my friend but you bet too much")
        fmt.Printf("You have only %d dollars to bet\n", gs.bankRoll)
      } else {
        validBet = true
        gs.playerCard = gs.deck[2]
        fmt.Println(" ", getCardName(gs.playerCard))

        if gs.tableCards[0] < gs.playerCard && gs.playerCard < gs.tableCards[1] {
          fmt.Println("You win!!")
          gs.bankRoll += bet
        } else {
          fmt.Println("Sorry, you lose")
          gs.bankRoll -= bet
        }
        gs.displayBankRoll()
      }

      if validBet {
        break
      }
    }
    if gs.bankRoll <= 0 {
      fmt.Println("Sorry, friend, but you blew your wad")
      break
    }
  }
}

func main() {
  fmt.Println("\n           Acey Ducey Card Game")
  fmt.Println("Creative Computing  Morristown, New Jersey")
  fmt.Println("\n\n")
  fmt.Println("Acey-Ducey is played in the following manner")
  fmt.Println("The dealer (computer) deals two cards face up")
  fmt.Println("You have an option to bet or not bet depending")
  fmt.Println("on whether or not you feel the card will have")
  fmt.Println("a value between the first two.")
  fmt.Println("If you do not want to bet, input a 0")
  for {
    gs := GameState {
      bankRoll: defaultBakroll,
      deck: []int {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12},
    }

    gs.gameLoop()
    fmt.Print("Try again (yes or no) ")
    var inp string
    fmt.Scanln(&inp)
    if strings.ToLower(inp) != "yes" {
      break
    }
  }
}
