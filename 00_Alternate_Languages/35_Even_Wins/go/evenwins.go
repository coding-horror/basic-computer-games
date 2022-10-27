package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

const MAXTAKE = 4

type PlayerType int8

const (
	HUMAN PlayerType = iota
	COMPUTER
)

type Game struct {
	table    int
	human    int
	computer int
}

func NewGame() Game {
	g := Game{}
	g.table = 27

	return g
}

func printIntro() {
	fmt.Println("Welcome to Even Wins!")
	fmt.Println("Based on evenwins.bas from Creative Computing")
	fmt.Println()
	fmt.Println("Even Wins is a two-person game. You start with")
	fmt.Println("27 marbles in the middle of the table.")
	fmt.Println()
	fmt.Println("Players alternate taking marbles from the middle.")
	fmt.Println("A player can take 1 to 4 marbles on their turn, and")
	fmt.Println("turns cannot be skipped. The game ends when there are")
	fmt.Println("no marbles left, and the winner is the one with an even")
	fmt.Println("number of marbles.")
	fmt.Println()
}

func (g *Game) printBoard() {
	fmt.Println()
	fmt.Printf(" marbles in the middle: %d\n", g.table)
	fmt.Printf("    # marbles you have: %d\n", g.human)
	fmt.Printf("# marbles computer has: %d\n", g.computer)
	fmt.Println()
}

func (g *Game) gameOver() {
	fmt.Println()
	fmt.Println("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")
	fmt.Println("!! All the marbles are taken: Game Over!")
	fmt.Println("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")
	fmt.Println()
	g.printBoard()
	if g.human%2 == 0 {
		fmt.Println("You are the winner! Congratulations!")
	} else {
		fmt.Println("The computer wins: all hail mighty silicon!")
	}
	fmt.Println()
}

func getPlural(count int) string {
	m := "marble"
	if count > 1 {
		m += "s"
	}
	return m
}

func (g *Game) humanTurn() {
	scanner := bufio.NewScanner(os.Stdin)
	maxAvailable := MAXTAKE
	if g.table < MAXTAKE {
		maxAvailable = g.table
	}

	fmt.Println("It's your turn!")
	for {
		fmt.Printf("Marbles to take? (1 - %d) --> ", maxAvailable)
		scanner.Scan()
		n, err := strconv.Atoi(scanner.Text())
		if err != nil {
			fmt.Printf("\n  Please enter a whole number from 1 to %d\n", maxAvailable)
			continue
		}
		if n < 1 {
			fmt.Println("\n  You must take at least 1 marble!")
			continue
		}
		if n > maxAvailable {
			fmt.Printf("\n  You can take at most %d %s\n", maxAvailable, getPlural(maxAvailable))
			continue
		}
		fmt.Printf("\nOkay, taking %d %s ...\n", n, getPlural(n))
		g.table -= n
		g.human += n
		return
	}
}

func (g *Game) computerTurn() {
	marblesToTake := 0

	fmt.Println("It's the computer's turn ...")
	r := float64(g.table - 6*int((g.table)/6))

	if int(g.human/2) == g.human/2 {
		if r < 1.5 || r > 5.3 {
			marblesToTake = 1
		} else {
			marblesToTake = int(r - 1)
		}
	} else if float64(g.table) < 4.2 {
		marblesToTake = 4
	} else if r > 3.4 {
		if r < 4.7 || r > 3.5 {
			marblesToTake = 4
		}
	} else {
		marblesToTake = int(r + 1)
	}

	fmt.Printf("Computer takes %d %s ...\n", marblesToTake, getPlural(marblesToTake))
	g.table -= marblesToTake
	g.computer += marblesToTake
}

func (g *Game) play(playersTurn PlayerType) {
	g.printBoard()

	for {
		if g.table == 0 {
			g.gameOver()
			return
		} else if playersTurn == HUMAN {
			g.humanTurn()
			g.printBoard()
			playersTurn = COMPUTER
		} else {
			g.computerTurn()
			g.printBoard()
			playersTurn = HUMAN
		}
	}
}

func getFirstPlayer() PlayerType {
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println("Do you want to play first? (y/n) --> ")
		scanner.Scan()

		if strings.ToUpper(scanner.Text()) == "Y" {
			return HUMAN
		} else if strings.ToUpper(scanner.Text()) == "N" {
			return COMPUTER
		} else {
			fmt.Println()
			fmt.Println("Please enter 'y' if you want to play first,")
			fmt.Println("or 'n' if you want to play second.")
			fmt.Println()
		}
	}
}

func main() {
	scanner := bufio.NewScanner(os.Stdin)

	printIntro()

	for {
		g := NewGame()

		g.play(getFirstPlayer())

		fmt.Println("\nWould you like to play again? (y/n) --> ")
		scanner.Scan()
		if strings.ToUpper(scanner.Text()) == "Y" {
			fmt.Println("\nOk, let's play again ...")
		} else {
			fmt.Println("\nOk, thanks for playing ... goodbye!")
			return
		}

	}

}
