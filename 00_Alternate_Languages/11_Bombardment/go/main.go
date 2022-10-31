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

// Messages correspond to outposts remaining (3, 2, 1, 0)
var PLAYER_PROGRESS_MESSAGES = []string{
	"YOU GOT ME, I'M GOING FAST. BUT I'LL GET YOU WHEN\nMY TRANSISTO&S RECUP%RA*E!",
	"THREE DOWN, ONE TO GO.\n\n",
	"TWO DOWN, TWO TO GO.\n\n",
	"ONE DOWN, THREE TO GO.\n\n",
}

var ENEMY_PROGRESS_MESSAGES = []string{
	"YOU'RE DEAD. YOUR LAST OUTPOST WAS AT %d. HA, HA, HA.\nBETTER LUCK NEXT TIME.",
	"YOU HAVE ONLY ONE OUTPOST LEFT.\n\n",
	"YOU HAVE ONLY TWO OUTPOSTS LEFT.\n\n",
	"YOU HAVE ONLY THREE OUTPOSTS LEFT.\n\n",
}

func displayField() {
	for r := 0; r < 5; r++ {
		initial := r*5 + 1
		for c := 0; c < 5; c++ {
			//x := strconv.Itoa(initial + c)
			fmt.Printf("\t%d", initial+c)
		}
		fmt.Println()
	}
	fmt.Print("\n\n\n\n\n\n\n\n\n")
}

func printIntro() {
	fmt.Println("                                BOMBARDMENT")
	fmt.Println("                CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
	fmt.Println()
	fmt.Println()
	fmt.Println("YOU ARE ON A BATTLEFIELD WITH 4 PLATOONS AND YOU")
	fmt.Println("HAVE 25 OUTPOSTS AVAILABLE WHERE THEY MAY BE PLACED.")
	fmt.Println("YOU CAN ONLY PLACE ONE PLATOON AT ANY ONE OUTPOST.")
	fmt.Println("THE COMPUTER DOES THE SAME WITH ITS FOUR PLATOONS.")
	fmt.Println()
	fmt.Println("THE OBJECT OF THE GAME IS TO FIRE MISSLES AT THE")
	fmt.Println("OUTPOSTS OF THE COMPUTER.  IT WILL DO THE SAME TO YOU.")
	fmt.Println("THE ONE WHO DESTROYS ALL FOUR OF THE ENEMY'S PLATOONS")
	fmt.Println("FIRST IS THE WINNER.")
	fmt.Println()
	fmt.Println("GOOD LUCK... AND TELL US WHERE YOU WANT THE BODIES SENT!")
	fmt.Println()
	fmt.Println("TEAR OFF MATRIX AND USE IT TO CHECK OFF THE NUMBERS.")
	fmt.Print("\n\n\n\n")
}

func positionList() []int {
	positions := make([]int, 25)
	for i := 0; i < 25; i++ {
		positions[i] = i + 1
	}
	return positions
}

// Randomly choose 4 'positions' out of a range of 1 to 25
func generateEnemyPositions() []int {
	positions := positionList()
	rand.Shuffle(len(positions), func(i, j int) { positions[i], positions[j] = positions[j], positions[i] })
	return positions[:4]
}

func isValidPosition(p int) bool {
	return p >= 1 && p <= 25
}

func promptForPlayerPositions() []int {
	scanner := bufio.NewScanner(os.Stdin)
	var positions []int

	for {
		fmt.Println("\nWHAT ARE YOUR FOUR POSITIONS (1-25)?")
		scanner.Scan()
		rawPositions := strings.Split(scanner.Text(), " ")

		if len(rawPositions) != 4 {
			fmt.Println("PLEASE ENTER FOUR UNIQUE POSITIONS")
			goto there
		}

		for _, p := range rawPositions {
			pos, err := strconv.Atoi(p)
			if (err != nil) || !isValidPosition(pos) {
				fmt.Println("ALL POSITIONS MUST RANGE (1-25)")
				goto there
			}
			positions = append(positions, pos)
		}
		if len(positions) == 4 {
			return positions
		}

	there:
	}
}

func promptPlayerForTarget() int {
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println("\nWHERE DO YOU WISH TO FIRE YOUR MISSILE?")
		scanner.Scan()
		target, err := strconv.Atoi(scanner.Text())

		if (err != nil) || !isValidPosition(target) {
			fmt.Println("POSITIONS MUST RANGE (1-25)")
			continue
		}
		return target
	}
}

func generateAttackSequence() []int {
	positions := positionList()
	rand.Shuffle(len(positions), func(i, j int) { positions[i], positions[j] = positions[j], positions[i] })
	return positions
}

// Performs attack procedure returning True if we are to continue.
func attack(target int, positions *[]int, hitMsg, missMsg string, progressMsg []string) bool {
	for i := 0; i < len(*positions); i++ {
		if target == (*positions)[i] {
			fmt.Print(hitMsg)

			// remove the target just hit
			(*positions)[i] = (*positions)[len((*positions))-1]
			(*positions)[len((*positions))-1] = 0
			(*positions) = (*positions)[:len((*positions))-1]

			if len((*positions)) != 0 {
				fmt.Print(progressMsg[len((*positions))])
			} else {
				fmt.Printf(progressMsg[len((*positions))], target)
			}
			return len((*positions)) > 0
		}
	}
	fmt.Print(missMsg)
	return len((*positions)) > 0
}

func main() {
	rand.Seed(time.Now().UnixNano())

	printIntro()
	displayField()

	enemyPositions := generateEnemyPositions()
	enemyAttacks := generateAttackSequence()
	enemyAttackCounter := 0

	playerPositions := promptForPlayerPositions()

	for {
		// player attacks
		if !attack(promptPlayerForTarget(), &enemyPositions, "YOU GOT ONE OF MY OUTPOSTS!\n\n", "HA, HA YOU MISSED. MY TURN NOW:\n\n", PLAYER_PROGRESS_MESSAGES) {
			break
		}
		// computer attacks
		hitMsg := fmt.Sprintf("I GOT YOU. IT WON'T BE LONG NOW. POST %d WAS HIT.\n", enemyAttacks[enemyAttackCounter])
		missMsg := fmt.Sprintf("I MISSED YOU, YOU DIRTY RAT. I PICKED %d. YOUR TURN:\n\n", enemyAttacks[enemyAttackCounter])
		if !attack(enemyAttacks[enemyAttackCounter], &playerPositions, hitMsg, missMsg, ENEMY_PROGRESS_MESSAGES) {
			break
		}
		enemyAttackCounter += 1
	}

}
