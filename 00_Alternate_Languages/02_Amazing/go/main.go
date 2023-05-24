package main

import (
	"bufio"
	"fmt"
	"log"
	"math/rand"
	"os"
	"strconv"
	"time"
)

func main() {
	rand.Seed(time.Now().UnixNano())

	printWelcome()

	h, w := getDimensions()
	m := NewMaze(h, w)
	m.draw()
}

type direction int64

const (
	LEFT direction = iota
	UP
	RIGHT
	DOWN
)

const (
	EXIT_DOWN  = 1
	EXIT_RIGHT = 2
)

type maze struct {
	width    int
	length   int
	used     [][]int
	walls    [][]int
	enterCol int
}

func NewMaze(w, l int) maze {
	if (w < 2) || (l < 2) {
		log.Fatal("invalid dimensions supplied")
	}

	m := maze{width: w, length: l}

	m.used = make([][]int, l)
	for i := range m.used {
		m.used[i] = make([]int, w)
	}

	m.walls = make([][]int, l)
	for i := range m.walls {
		m.walls[i] = make([]int, w)
	}

	// randomly determine the entry column
	m.enterCol = rand.Intn(w)

	// determine layout of walls
	m.build()

	// add an exit
	col := rand.Intn(m.width - 1)
	row := m.length - 1
	m.walls[row][col] = m.walls[row][col] + 1

	return m
}

func (m *maze) build() {
	row := 0
	col := 0
	count := 2

	for {
		possibleDirs := m.getPossibleDirections(row, col)

		if len(possibleDirs) != 0 {
			row, col, count = m.makeOpening(possibleDirs, row, col, count)
		} else {
			for {
				if col != m.width-1 {
					col = col + 1
				} else if row != m.length-1 {
					row = row + 1
					col = 0
				} else {
					row = 0
					col = 0
				}

				if m.used[row][col] != 0 {
					break
				}
			}
		}

		if count == (m.width*m.length)+1 {
			break
		}
	}

}

func (m *maze) getPossibleDirections(row, col int) []direction {
	possible_dirs := make(map[direction]bool, 4)
	possible_dirs[LEFT] = true
	possible_dirs[UP] = true
	possible_dirs[RIGHT] = true
	possible_dirs[DOWN] = true

	if (col == 0) || (m.used[row][col-1] != 0) {
		possible_dirs[LEFT] = false
	}
	if (row == 0) || (m.used[row-1][col] != 0) {
		possible_dirs[UP] = false
	}
	if (col == m.width-1) || (m.used[row][col+1] != 0) {
		possible_dirs[RIGHT] = false
	}
	if (row == m.length-1) || (m.used[row+1][col] != 0) {
		possible_dirs[DOWN] = false
	}

	ret := make([]direction, 0)
	for d, v := range possible_dirs {
		if v {
			ret = append(ret, d)
		}
	}
	return ret
}

func (m *maze) makeOpening(dirs []direction, row, col, count int) (int, int, int) {
	dir := rand.Intn(len(dirs))

	if dirs[dir] == LEFT {
		col = col - 1
		m.walls[row][col] = int(EXIT_RIGHT)
	} else if dirs[dir] == UP {
		row = row - 1
		m.walls[row][col] = int(EXIT_DOWN)
	} else if dirs[dir] == RIGHT {
		m.walls[row][col] = m.walls[row][col] + EXIT_RIGHT
		col = col + 1
	} else if dirs[dir] == DOWN {
		m.walls[row][col] = m.walls[row][col] + EXIT_DOWN
		row = row + 1
	}

	m.used[row][col] = count
	count = count + 1
	return row, col, count
}

// draw the maze
func (m *maze) draw() {
	for col := 0; col < m.width; col++ {
		if col == m.enterCol {
			fmt.Print(".  ")
		} else {
			fmt.Print(".--")
		}
	}
	fmt.Println(".")

	for row := 0; row < m.length; row++ {
		fmt.Print("|")
		for col := 0; col < m.width; col++ {
			if m.walls[row][col] < 2 {
				fmt.Print("  |")
			} else {
				fmt.Print("   ")
			}
		}
		fmt.Println()
		for col := 0; col < m.width; col++ {
			if (m.walls[row][col] == 0) || (m.walls[row][col] == 2) {
				fmt.Print(":--")
			} else {
				fmt.Print(":  ")
			}
		}
		fmt.Println(".")
	}
}

func printWelcome() {
	fmt.Println("                            AMAZING PROGRAM")
	fmt.Print("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")
}

func getDimensions() (int, int) {
	scanner := bufio.NewScanner(os.Stdin)

	fmt.Println("Enter a width ( > 1 ):")
	scanner.Scan()
	w, err := strconv.Atoi(scanner.Text())
	if err != nil {
		log.Fatal("invalid dimension")
	}

	fmt.Println("Enter a height ( > 1 ):")
	scanner.Scan()
	h, err := strconv.Atoi(scanner.Text())
	if err != nil {
		log.Fatal("invalid dimension")
	}

	return w, h
}
