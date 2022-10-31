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

const (
	SEA_WIDTH        = 6
	DESTROYER_LENGTH = 2
	CRUISER_LENGTH   = 3
	CARRIER_LENGTH   = 4
)

type Point [2]int
type Vector Point
type Sea [][]int

func NewSea() Sea {
	s := make(Sea, 6)
	for r := 0; r < SEA_WIDTH; r++ {
		c := make([]int, 6)
		s[r] = c
	}

	return s
}

func getRandomVector() Vector {
	v := Vector{}

	for {
		v[0] = rand.Intn(3) - 1
		v[1] = rand.Intn(3) - 1

		if !(v[0] == 0 && v[1] == 0) {
			break
		}
	}
	return v
}

func addVector(p Point, v Vector) Point {
	newPoint := Point{}

	newPoint[0] = p[0] + v[0]
	newPoint[1] = p[1] + v[1]

	return newPoint
}

func isWithinSea(p Point, s Sea) bool {
	return (1 <= p[0] && p[0] <= len(s)) && (1 <= p[1] && p[1] <= len(s))
}

func valueAt(p Point, s Sea) int {
	return s[p[1]-1][p[0]-1]
}

func reportInputError() {
	fmt.Printf("INVALID. SPECIFY TWO NUMBERS FROM 1 TO %d, SEPARATED BY A COMMA.\n", SEA_WIDTH)
}

func getNextTarget(s Sea) Point {
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println("\n?")
		scanner.Scan()

		vals := strings.Split(scanner.Text(), ",")

		if len(vals) != 2 {
			reportInputError()
			continue
		}

		x, xErr := strconv.Atoi(strings.TrimSpace(vals[0]))
		y, yErr := strconv.Atoi(strings.TrimSpace(vals[1]))

		if (len(vals) != 2) || (xErr != nil) || (yErr != nil) {
			reportInputError()
			continue
		}

		p := Point{}
		p[0] = x
		p[1] = y
		if isWithinSea(p, s) {
			return p
		}
	}
}

func setValueAt(value int, p Point, s Sea) {
	s[p[1]-1][p[0]-1] = value
}

func hasShip(s Sea, code int) bool {
	hasShip := false
	for r := 0; r < SEA_WIDTH; r++ {
		for c := 0; c < SEA_WIDTH; c++ {
			if s[r][c] == code {
				hasShip = true
				break
			}
		}
	}
	return hasShip
}

func countSunk(s Sea, codes []int) int {
	sunk := 0

	for _, c := range codes {
		if !hasShip(s, c) {
			sunk += 1
		}
	}

	return sunk
}

func placeShip(s Sea, size, code int) {
	for {
		start := Point{}
		start[0] = rand.Intn(SEA_WIDTH) + 1
		start[1] = rand.Intn(SEA_WIDTH) + 1
		vector := getRandomVector()

		point := start
		points := []Point{}

		for i := 0; i < size; i++ {
			point = addVector(point, vector)
			points = append(points, point)
		}

		clearPosition := true
		for _, p := range points {
			if !isWithinSea(p, s) {
				clearPosition = false
				break
			}
			if valueAt(p, s) > 0 {
				clearPosition = false
				break
			}
		}
		if !clearPosition {
			continue
		}

		for _, p := range points {
			setValueAt(code, p, s)
		}
		break
	}
}

func setupShips(s Sea) {
	placeShip(s, DESTROYER_LENGTH, 1)
	placeShip(s, DESTROYER_LENGTH, 2)
	placeShip(s, CRUISER_LENGTH, 3)
	placeShip(s, CRUISER_LENGTH, 4)
	placeShip(s, CARRIER_LENGTH, 5)
	placeShip(s, CARRIER_LENGTH, 6)
}

func printIntro() {
	fmt.Println("                BATTLE")
	fmt.Println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
	fmt.Println()
	fmt.Println("THE FOLLOWING CODE OF THE BAD GUYS' FLEET DISPOSITION")
	fmt.Println("HAS BEEN CAPTURED BUT NOT DECODED:	")
	fmt.Println()
}

func printInstructions() {
	fmt.Println()
	fmt.Println()
	fmt.Println("DE-CODE IT AND USE IT IF YOU CAN")
	fmt.Println("BUT KEEP THE DE-CODING METHOD A SECRET.")
	fmt.Println()
	fmt.Println("START GAME")
}

func printEncodedSea(s Sea) {
	for x := 0; x < SEA_WIDTH; x++ {
		fmt.Println()
		for y := SEA_WIDTH - 1; y > -1; y-- {
			fmt.Printf(" %d", s[y][x])
		}
	}
	fmt.Println()
}

func wipeout(s Sea) bool {
	for c := 1; c <= 7; c++ {
		if hasShip(s, c) {
			return false
		}
	}
	return true
}

func main() {
	rand.Seed(time.Now().UnixNano())

	s := NewSea()

	setupShips(s)

	printIntro()

	printEncodedSea(s)

	printInstructions()

	splashes := 0
	hits := 0

	for {
		target := getNextTarget(s)
		targetValue := valueAt(target, s)

		if targetValue < 0 {
			fmt.Printf("YOU ALREADY PUT A HOLE IN SHIP NUMBER %d AT THAT POINT.\n", targetValue)
		}

		if targetValue <= 0 {
			fmt.Println("SPLASH! TRY AGAIN.")
			splashes += 1
			continue
		}

		fmt.Printf("A DIRECT HIT ON SHIP NUMBER %d\n", targetValue)
		hits += 1
		setValueAt(targetValue*-1, target, s)

		if !hasShip(s, targetValue) {
			fmt.Println("AND YOU SUNK IT. HURRAH FOR THE GOOD GUYS.")
			fmt.Println("SO FAR, THE BAD GUYS HAVE LOST")
			fmt.Printf("%d DESTROYER(S), %d CRUISER(S), AND %d AIRCRAFT CARRIER(S).\n", countSunk(s, []int{1, 2}), countSunk(s, []int{3, 4}), countSunk(s, []int{5, 6}))
		}

		if !wipeout(s) {
			fmt.Printf("YOUR CURRENT SPLASH/HIT RATIO IS %2f\n", float32(splashes)/float32(hits))
			continue
		}

		fmt.Printf("YOU HAVE TOTALLY WIPED OUT THE BAD GUYS' FLEET WITH A FINAL SPLASH/HIT RATIO OF %2f\n", float32(splashes)/float32(hits))

		if splashes == 0 {
			fmt.Println("CONGRATULATIONS -- A DIRECT HIT EVERY TIME.")
		}

		fmt.Println("\n****************************")
		break
	}
}
