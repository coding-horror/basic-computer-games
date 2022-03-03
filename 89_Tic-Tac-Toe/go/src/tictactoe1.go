package main

import (
	"fmt"
	"strconv"
)

func main() {
	fmt.Printf("Hello, World!\n")
	// Print text on the screen with 30 spaces before text
	fmt.Printf("%30s\n", "TIC TAC TOE")
	// Print text on screen with 15 spaces before text
	// And print three lines break on screen
	fmt.Printf("%15s\n\n\n\n", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
	// THIS PROGRAM PLAYS TIC TAC TOE
	// THE MACHINE GOES FIRST
	fmt.Printf("THE GAME BOARD IS NUMBERED:\n\n")
	fmt.Println("1  2  3")
	fmt.Println("8  9  4")
	fmt.Println("7  6  5")

	// Main program
	for {
		var (
			a, b, c, d, e int
			p, q, r, s    int
		)
		a = 9
		fmt.Printf("\n\n")
		computerMoves(a)
		p = readYourMove()
		b = move(p + 1)
		computerMoves(b)
		q = readYourMove()
		if q == move(b+4) {
			c = move(b + 2)
			computerMoves(c)
			r = readYourMove()
			if r == move(c+4) {
				if p%2 != 0 {
					d = move(c + 3)
					computerMoves(d)
					s = readYourMove()
					if s == move(d+4) {
						e = move(d + 6)
						computerMoves(e)
						fmt.Println("THE GAME IS A DRAW.")
					} else {
						e = move(d + 4)
						computerMoves(e)
						fmt.Println("AND WINS ********")
					}
				} else {
					d = move(c + 7)
					computerMoves(d)
					fmt.Println("AND WINS ********")
				}
			} else {
				d = move(c + 4)
				computerMoves(d)
				fmt.Println("AND WINS ********")
			}
		} else {
			c = move(b + 4)
			computerMoves(c)
			fmt.Println("AND WINS ********")
		}
	}
}
func computerMoves(move int) {
	fmt.Printf("COMPUTER MOVES %v\n", move)
}

func readYourMove() int {
	for {
		fmt.Printf("YOUR MOVE?")
		var input string
		fmt.Scan(&input)
		number, err := strconv.Atoi(input)
		if err == nil {
			return number
		}
	}
}

func move(number int) int {
	return number - 8*(int)((number-1)/8)
}
