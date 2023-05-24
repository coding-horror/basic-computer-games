package main

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strings"
)

type node struct {
	text    string
	yesNode *node
	noNode  *node
}

func newNode(text string, yes_node, no_node *node) *node {
	n := node{text: text}
	if yes_node != nil {
		n.yesNode = yes_node
	}
	if no_node != nil {
		n.noNode = no_node
	}
	return &n
}

func (n *node) update(newQuestion, newAnswer, newAnimal string) {
	oldAnimal := n.text

	n.text = newQuestion

	if newAnswer == "y" {
		n.yesNode = newNode(newAnimal, nil, nil)
		n.noNode = newNode(oldAnimal, nil, nil)
	} else {
		n.yesNode = newNode(oldAnimal, nil, nil)
		n.noNode = newNode(newAnimal, nil, nil)
	}
}

func (n *node) isLeaf() bool {
	return (n.yesNode == nil) && (n.noNode == nil)
}

func listKnownAnimals(root *node) {
	if root == nil {
		return
	}

	if root.isLeaf() {
		fmt.Printf("%s           ", root.text)
		return
	}

	if root.yesNode != nil {
		listKnownAnimals(root.yesNode)
	}

	if root.noNode != nil {
		listKnownAnimals(root.noNode)
	}
}

func parseInput(message string, checkList bool, rootNode *node) string {
	scanner := bufio.NewScanner(os.Stdin)
	token := ""

	for {
		fmt.Println(message)
		scanner.Scan()
		inp := strings.ToLower(scanner.Text())

		if checkList && inp == "list" {
			fmt.Println("Animals I already know are:")
			listKnownAnimals(rootNode)
			fmt.Println()
		}

		if len(inp) > 0 {
			token = inp
		} else {
			token = ""
		}

		if token == "y" || token == "n" {
			break
		}
	}
	return token
}

func avoidVoidInput(message string) string {
	scanner := bufio.NewScanner(os.Stdin)
	answer := ""
	for {
		fmt.Println(message)
		scanner.Scan()
		answer = scanner.Text()

		if answer != "" {
			break
		}
	}
	return answer
}

func printIntro() {
	fmt.Println("                                Animal")
	fmt.Println("               Creative Computing Morristown, New Jersey")
	fmt.Println("\nPlay 'Guess the Animal'")
	fmt.Println("Think of an animal and the computer will try to guess it")
}

func main() {
	yesChild := newNode("Fish", nil, nil)
	noChild := newNode("Bird", nil, nil)
	rootNode := newNode("Does it swim?", yesChild, noChild)

	printIntro()

	keepPlaying := (parseInput("Are you thinking of an animal?", true, rootNode) == "y")

	for keepPlaying {
		keepAsking := true

		actualNode := rootNode

		for keepAsking {
			if !actualNode.isLeaf() {
				answer := parseInput(actualNode.text, false, nil)

				if answer == "y" {
					if actualNode.yesNode == nil {
						log.Fatal("invalid node")
					}
					actualNode = actualNode.yesNode
				} else {
					if actualNode.noNode == nil {
						log.Fatal("invalid node")
					}
					actualNode = actualNode.noNode
				}
			} else {
				answer := parseInput(fmt.Sprintf("Is it a %s?", actualNode.text), false, nil)
				if answer == "n" {
					newAnimal := avoidVoidInput("The animal you were thinking of was a ?")
					newQuestion := avoidVoidInput(fmt.Sprintf("Please type in a question that would distinguish a '%s' from a '%s':", newAnimal, actualNode.text))
					newAnswer := parseInput(fmt.Sprintf("For a '%s' the answer would be", newAnimal), false, nil)
					actualNode.update(newQuestion+"?", newAnswer, newAnimal)
				} else {
					fmt.Println("Why not try another animal?")
				}
				keepAsking = false
			}
		}
		keepPlaying = (parseInput("Are you thinking of an animal?", true, rootNode) == "y")
	}
}
