package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

type StartOption int8

const (
	StartUndefined StartOption = iota
	ComputerFirst
	PlayerFirst
)

type WinOption int8

const (
	WinUndefined WinOption = iota
	TakeLast
	AvoidLast
)

type GameOptions struct {
	pileSize    int
	winOption   WinOption
	startOption StartOption
	minSelect   int
	maxSelect   int
}

func NewOptions() *GameOptions {
	g := GameOptions{}

	g.pileSize = getPileSize()
	if g.pileSize < 0 {
		return &g
	}

	g.winOption = getWinOption()
	g.minSelect, g.maxSelect = getMinMax()
	g.startOption = getStartOption()

	return &g
}

func getPileSize() int {
	ps := 0
	var err error
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println("Enter Pile Size ")
		scanner.Scan()
		ps, err = strconv.Atoi(scanner.Text())
		if err == nil {
			break
		}
	}
	return ps
}

func getWinOption() WinOption {
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println("ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST:")
		scanner.Scan()
		w, err := strconv.Atoi(scanner.Text())
		if err == nil && (w == 1 || w == 2) {
			return WinOption(w)
		}
	}
}

func getStartOption() StartOption {
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println("ENTER START OPTION - 1 COMPUTER FIRST, 2 YOU FIRST ")
		scanner.Scan()
		s, err := strconv.Atoi(scanner.Text())
		if err == nil && (s == 1 || s == 2) {
			return StartOption(s)
		}
	}
}

func getMinMax() (int, int) {
	minSelect := 0
	maxSelect := 0
	var minErr error
	var maxErr error
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println("ENTER MIN AND MAX ")
		scanner.Scan()
		enteredValues := scanner.Text()
		vals := strings.Split(enteredValues, " ")
		minSelect, minErr = strconv.Atoi(vals[0])
		maxSelect, maxErr = strconv.Atoi(vals[1])
		if (minErr == nil) && (maxErr == nil) && (minSelect > 0) && (maxSelect > 0) && (maxSelect > minSelect) {
			return minSelect, maxSelect
		}
	}
}

// This handles the player's turn - asking the player how many objects
// to take and doing some basic validation around that input.  Then it
// checks for any win conditions.
// Returns a boolean indicating whether the game is over and the new pile_size.
func playerMove(pile, min, max int, win WinOption) (bool, int) {
	scanner := bufio.NewScanner(os.Stdin)
	done := false
	for !done {
		fmt.Println("YOUR MOVE")
		scanner.Scan()
		m, err := strconv.Atoi(scanner.Text())
		if err != nil {
			continue
		}

		if m == 0 {
			fmt.Println("I TOLD YOU NOT TO USE ZERO!  COMPUTER WINS BY FORFEIT.")
			return true, pile
		}

		if m > max || m < min {
			fmt.Println("ILLEGAL MOVE, REENTER IT")
			continue
		}

		pile -= m
		done = true

		if pile <= 0 {
			if win == AvoidLast {
				fmt.Println("TOUGH LUCK, YOU LOSE.")
			} else {
				fmt.Println("CONGRATULATIONS, YOU WIN.")
			}
			return true, pile
		}
	}
	return false, pile
}

// This handles the logic to determine how many objects the computer
// will select on its turn.
func computerPick(pile, min, max int, win WinOption) int {
	var q int
	if win == AvoidLast {
		q = pile - 1
	} else {
		q = pile
	}
	c := min + max

	pick := q - (c * int(q/c))

	if pick < min {
		pick = min
	} else if pick > max {
		pick = max
	}

	return pick
}

// This handles the computer's turn - first checking for the various
// win/lose conditions and then calculating how many objects
// the computer will take.
// Returns a boolean indicating whether the game is over and the new pile_size.
func computerMove(pile, min, max int, win WinOption) (bool, int) {
	// first check for end-game conditions
	if win == TakeLast && pile <= max {
		fmt.Printf("COMPUTER TAKES %d AND WINS\n", pile)
		return true, pile
	}

	if win == AvoidLast && pile <= min {
		fmt.Printf("COMPUTER TAKES %d AND LOSES\n", pile)
		return true, pile
	}

	// otherwise determine the computer's selection
	selection := computerPick(pile, min, max, win)
	pile -= selection
	fmt.Printf("COMPUTER TAKES %d AND LEAVES %d\n", selection, pile)
	return false, pile
}

// This is the main game loop - repeating each turn until one
// of the win/lose conditions is met.
func play(pile, min, max int, start StartOption, win WinOption) {
	gameOver := false
	playersTurn := (start == PlayerFirst)

	for !gameOver {
		if playersTurn {
			gameOver, pile = playerMove(pile, min, max, win)
			playersTurn = false
			if gameOver {
				return
			}
		}

		if !playersTurn {
			gameOver, pile = computerMove(pile, min, max, win)
			playersTurn = true
		}
	}
}

// Print out the introduction and rules of the game
func printIntro() {
	fmt.Printf("%33s%s\n", " ", "BATNUM")
	fmt.Printf("%15s%s\n", " ", "CREATIVE COMPUTING  MORRISSTOWN, NEW JERSEY")
	fmt.Printf("\n\n\n")
	fmt.Println("THIS PROGRAM IS A 'BATTLE OF NUMBERS' GAME, WHERE THE")
	fmt.Println("COMPUTER IS YOUR OPPONENT.")
	fmt.Println()
	fmt.Println("THE GAME STARTS WITH AN ASSUMED PILE OF OBJECTS. YOU")
	fmt.Println("AND YOUR OPPONENT ALTERNATELY REMOVE OBJECTS FROM THE PILE.")
	fmt.Println("WINNING IS DEFINED IN ADVANCE AS TAKING THE LAST OBJECT OR")
	fmt.Println("NOT. YOU CAN ALSO SPECIFY SOME OTHER BEGINNING CONDITIONS.")
	fmt.Println("DON'T USE ZERO, HOWEVER, IN PLAYING THE GAME.")
	fmt.Println("ENTER A NEGATIVE NUMBER FOR NEW PILE SIZE TO STOP PLAYING.")
	fmt.Println()
}

func main() {
	for {
		printIntro()

		g := NewOptions()

		if g.pileSize < 0 {
			return
		}

		play(g.pileSize, g.minSelect, g.maxSelect, g.startOption, g.winOption)
	}
}
