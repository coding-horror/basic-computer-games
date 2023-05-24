package main

import (
	"bufio"
	"fmt"
	"math/rand"
	"os"
	"strings"
	"time"
)

func main() {
	rand.Seed(time.Now().UnixNano())
	words := [][]string{
		{
			"Ability",
			"Basal",
			"Behavioral",
			"Child-centered",
			"Differentiated",
			"Discovery",
			"Flexible",
			"Heterogeneous",
			"Homogenous",
			"Manipulative",
			"Modular",
			"Tavistock",
			"Individualized",
		}, {
			"learning",
			"evaluative",
			"objective",
			"cognitive",
			"enrichment",
			"scheduling",
			"humanistic",
			"integrated",
			"non-graded",
			"training",
			"vertical age",
			"motivational",
			"creative",
		}, {
			"grouping",
			"modification",
			"accountability",
			"process",
			"core curriculum",
			"algorithm",
			"performance",
			"reinforcement",
			"open classroom",
			"resource",
			"structure",
			"facility",
			"environment",
		},
	}

	scanner := bufio.NewScanner(os.Stdin)

	// Display intro text
	fmt.Println("\n           Buzzword Generator")
	fmt.Println("Creative Computing  Morristown, New Jersey")
	fmt.Println("\n\n")
	fmt.Println("This program prints highly acceptable phrases in")
	fmt.Println("'educator-speak' that you can work into reports")
	fmt.Println("and speeches.  Whenever a question mark is printed,")
	fmt.Println("type a 'Y' for another phrase or 'N' to quit.")
	fmt.Println("\n\nHere's the first phrase:")

	for {
		phrase := ""
		for _, section := range words {
			if len(phrase) > 0 {
				phrase += " "
			}
			phrase += section[rand.Intn(len(section))]
		}
		fmt.Println(phrase)
		fmt.Println()

		// continue?
		fmt.Println("?")
		scanner.Scan()
		if strings.ToUpper(scanner.Text())[0:1] != "Y" {
			break
		}
	}
	fmt.Println("Come back when you need help with another report!")
}
