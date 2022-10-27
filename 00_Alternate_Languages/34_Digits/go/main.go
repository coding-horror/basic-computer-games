package main

import (
	"bufio"
	"fmt"
	"math/rand"
	"os"
	"strconv"
	"time"
)

func printIntro() {
	fmt.Println("                                DIGITS")
	fmt.Println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
	fmt.Println()
	fmt.Println()
	fmt.Println("THIS IS A GAME OF GUESSING.")
}

func readInteger(prompt string) int {
	scanner := bufio.NewScanner(os.Stdin)
	for {
		fmt.Println(prompt)
		scanner.Scan()
		response, err := strconv.Atoi(scanner.Text())

		if err != nil {
			fmt.Println("INVALID INPUT, TRY AGAIN... ")
			continue
		}

		return response
	}
}

func printInstructions() {
	fmt.Println()
	fmt.Println("PLEASE TAKE A PIECE OF PAPER AND WRITE DOWN")
	fmt.Println("THE DIGITS '0', '1', OR '2' THIRTY TIMES AT RANDOM.")
	fmt.Println("ARRANGE THEM IN THREE LINES OF TEN DIGITS EACH.")
	fmt.Println("I WILL ASK FOR THEN TEN AT A TIME.")
	fmt.Println("I WILL ALWAYS GUESS THEM FIRST AND THEN LOOK AT YOUR")
	fmt.Println("NEXT NUMBER TO SEE IF I WAS RIGHT. BY PURE LUCK,")
	fmt.Println("I OUGHT TO BE RIGHT TEN TIMES. BUT I HOPE TO DO BETTER")
	fmt.Println("THAN THAT *****")
	fmt.Println()
}

func readTenNumbers() []int {
	numbers := make([]int, 10)

	numbers[0] = readInteger("FIRST NUMBER: ")
	for i := 1; i < 10; i++ {
		numbers[i] = readInteger("NEXT NUMBER:")
	}

	return numbers
}

func printSummary(correct int) {
	fmt.Println()

	if correct > 10 {
		fmt.Println()
		fmt.Println("I GUESSED MORE THAN 1/3 OF YOUR NUMBERS.")
		fmt.Println("I WIN.\u0007")
	} else if correct < 10 {
		fmt.Println("I GUESSED LESS THAN 1/3 OF YOUR NUMBERS.")
		fmt.Println("YOU BEAT ME.  CONGRATULATIONS *****")
	} else {
		fmt.Println("I GUESSED EXACTLY 1/3 OF YOUR NUMBERS.")
		fmt.Println("IT'S A TIE GAME.")
	}
}

func buildArray(val, row, col int) [][]int {
	a := make([][]int, row)
	for r := 0; r < row; r++ {
		b := make([]int, col)
		for c := 0; c < col; c++ {
			b[c] = val
		}
		a[r] = b
	}
	return a
}

func main() {
	rand.Seed(time.Now().UnixNano())

	printIntro()
	if readInteger("FOR INSTRUCTIONS, TYPE '1', ELSE TYPE '0' ? ") == 1 {
		printInstructions()
	}

	a := 0
	b := 1
	c := 3

	m := buildArray(1, 27, 3)
	k := buildArray(9, 3, 3)
	l := buildArray(3, 9, 3)

	for {
		l[0][0] = 2
		l[4][1] = 2
		l[8][2] = 2

		z := float64(26)
		z1 := float64(8)
		z2 := 2
		runningCorrect := 0

		var numbers []int
		for round := 1; round <= 4; round++ {
			validNumbers := false
			for !validNumbers {
				numbers = readTenNumbers()
				validNumbers = true
				for _, n := range numbers {
					if n < 0 || n > 2 {
						fmt.Println("ONLY USE THE DIGITS '0', '1', OR '2'.")
						fmt.Println("LET'S TRY AGAIN.")
						validNumbers = false
						break
					}
				}
			}

			fmt.Printf("\n%-14s%-14s%-14s%-14s\n", "MY GUESS", "YOUR NO.", "RESULT", "NO. RIGHT")

			for _, n := range numbers {
				s := 0
				myGuess := 0

				for j := 0; j < 3; j++ {
					s1 := a*k[z2][j] + b*l[int(z1)][j] + c*m[int(z)][j]

					if s < s1 {
						s = s1
						myGuess = j
					} else if s1 == s && rand.Float64() > 0.5 {
						myGuess = j
					}
				}
				result := ""

				if myGuess != n {
					result = "WRONG"
				} else {
					runningCorrect += 1
					result = "RIGHT"
					m[int(z)][n] = m[int(z)][n] + 1
					l[int(z1)][n] = l[int(z1)][n] + 1
					k[int(z2)][n] = k[int(z2)][n] + 1
					z = z - (z/9)*9
					z = 3.0*z + float64(n)
				}
				fmt.Printf("\n%-14d%-14d%-14s%-14d\n", myGuess, n, result, runningCorrect)

				z1 = z - (z/9)*9
				z2 = n
			}
			printSummary(runningCorrect)
			if readInteger("\nDO YOU WANT TO TRY AGAIN (1 FOR YES, 0 FOR NO) ? ") != 1 {
				fmt.Println("\nTHANKS FOR THE GAME.")
				os.Exit(0)
			}
		}
	}
}
