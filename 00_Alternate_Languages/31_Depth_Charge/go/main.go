package main

import (
	"bufio"
	"fmt"
	"math"
	"math/rand"
	"os"
	"strconv"
	"strings"
	"time"
)

type Position []int

func NewPosition() Position {
	p := make([]int, 3)
	return Position(p)
}

func showWelcome() {
	fmt.Print("\033[H\033[2J")
	fmt.Println("                DEPTH CHARGE")
	fmt.Println("    Creative Computing  Morristown, New Jersey")
	fmt.Println()
}

func getNumCharges() (int, int) {
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println("Dimensions of search area?")
		scanner.Scan()
		dim, err := strconv.Atoi(scanner.Text())
		if err != nil {
			fmt.Println("Must enter an integer number. Please try again...")
			continue
		}
		return dim, int(math.Log2(float64(dim))) + 1
	}
}

func askForNewGame() {
	scanner := bufio.NewScanner(os.Stdin)

	fmt.Println("Another game (Y or N): ")
	scanner.Scan()
	if strings.ToUpper(scanner.Text()) == "Y" {
		main()
	}
	fmt.Println("OK. Hope you enjoyed yourself")
	os.Exit(1)
}

func showShotResult(shot, location Position) {
	result := "Sonar reports shot was "

	if shot[1] > location[1] { // y-direction
		result += "north"
	} else if shot[1] < location[1] { // y-direction
		result += "south"
	}

	if shot[0] > location[0] { // x-direction
		result += "east"
	} else if shot[0] < location[0] { // x-direction
		result += "west"
	}

	if shot[1] != location[1] || shot[0] != location[0] {
		result += " and "
	}
	if shot[2] > location[2] {
		result += "too low."
	} else if shot[2] < location[2] {
		result += "too high."
	} else {
		result += "depth OK."
	}

	fmt.Println(result)
}

func getShot() Position {
	scanner := bufio.NewScanner(os.Stdin)

	for {
		shotPos := NewPosition()
		fmt.Println("Enter coordinates: ")
		scanner.Scan()
		rawGuess := strings.Split(scanner.Text(), " ")
		if len(rawGuess) != 3 {
			goto there
		}
		for i := 0; i < 3; i++ {
			val, err := strconv.Atoi(rawGuess[i])
			if err != nil {
				goto there
			}
			shotPos[i] = val
		}
		return shotPos
	there:
		fmt.Println("Please enter coordinates separated by spaces")
		fmt.Println("Example: 3 2 1")
	}
}

func getRandomPosition(searchArea int) Position {
	pos := NewPosition()
	for i := 0; i < 3; i++ {
		pos[i] = rand.Intn(searchArea)
	}
	return pos
}

func playGame(searchArea, numCharges int) {
	rand.Seed(time.Now().UTC().UnixNano())
	fmt.Println("\nYou are the captain of the destroyer USS Computer.")
	fmt.Println("An enemy sub has been causing you trouble. Your")
	fmt.Printf("mission is to destroy it. You have %d shots.\n", numCharges)
	fmt.Println("Specify depth charge explosion point with a")
	fmt.Println("trio of numbers -- the first two are the")
	fmt.Println("surface coordinates; the third is the depth.")
	fmt.Println("\nGood luck!")
	fmt.Println()

	subPos := getRandomPosition(searchArea)

	for c := 0; c < numCharges; c++ {
		fmt.Printf("\nTrial #%d\n", c+1)

		shot := getShot()

		if shot[0] == subPos[0] && shot[1] == subPos[1] && shot[2] == subPos[2] {
			fmt.Printf("\nB O O M ! ! You found it in %d tries!\n", c+1)
			askForNewGame()
		} else {
			showShotResult(shot, subPos)
		}
	}

	// out of depth charges
	fmt.Println("\nYou have been torpedoed! Abandon ship!")
	fmt.Printf("The submarine was at %d %d %d\n", subPos[0], subPos[1], subPos[2])
	askForNewGame()

}

func main() {
	showWelcome()

	searchArea, numCharges := getNumCharges()

	playGame(searchArea, numCharges)
}
