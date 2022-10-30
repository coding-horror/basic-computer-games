package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

func printLightning() {
	fmt.Println("************************************")
	n := 24
	for n > 16 {
		var b strings.Builder
		b.Grow(n + 3)
		for i := 0; i < n; i++ {
			b.WriteString(" ")
		}
		b.WriteString("x x")
		fmt.Println(b.String())
		n--
	}
	fmt.Println("                x xxx")
	fmt.Println("               x   x")
	fmt.Println("              xx xx")
	n--
	for n > 8 {
		var b strings.Builder
		b.Grow(n + 3)
		for i := 0; i < n; i++ {
			b.WriteString(" ")
		}
		b.WriteString("x x")
		fmt.Println(b.String())
		n--
	}
	fmt.Println("        xx")
	fmt.Println("       x")
	fmt.Println("************************************")
}

func printSolution(n float64) {
	fmt.Printf("\n%f plus 3 gives %f. This divided by 5 equals %f\n", n, n+3, (n+3)/5)
	fmt.Printf("This times 8 gives %f. If we divide 5 and add 5.\n", ((n+3)/5)*8)
	fmt.Printf("We get %f, which, minus 1 equals %f\n", (((n+3)/5)*8)/5+5, ((((n+3)/5)*8)/5+5)-1)
}

func play() {
	fmt.Println("\nTake a Number and ADD 3. Now, Divide this number by 5 and")
	fmt.Println("multiply by 8. Now, Divide by 5 and add the same. Subtract 1")

	youHave := getFloat("\nWhat do you have?")
	compGuess := (((youHave-4)*5)/8)*5 - 3
	if getYesNo(fmt.Sprintf("\nI bet your number was %f was I right(Yes or No)? ", compGuess)) {
		fmt.Println("\nHuh, I knew I was unbeatable")
		fmt.Println("And here is how i did it")
		printSolution(compGuess)
	} else {
		originalNumber := getFloat("\nHUH!! what was you original number? ")
		if originalNumber == compGuess {
			fmt.Println("\nThat was my guess, AHA i was right")
			fmt.Println("Shamed to accept defeat i guess, don't worry you can master mathematics too")
			fmt.Println("Here is how i did it")
			printSolution(compGuess)
		} else {
			fmt.Println("\nSo you think you're so smart, EH?")
			fmt.Println("Now, Watch")
			printSolution(originalNumber)

			if getYesNo("\nNow do you believe me? ") {
				print("\nOk, Lets play again sometime bye!!!!")
			} else {
				fmt.Println("\nYOU HAVE MADE ME VERY MAD!!!!!")
				fmt.Println("BY THE WRATH OF THE MATHEMATICS AND THE RAGE OF THE GODS")
				fmt.Println("THERE SHALL BE LIGHTNING!!!!!!!")
				printLightning()
				fmt.Println("\nI Hope you believe me now, for your own sake")
			}
		}
	}
}

func getFloat(prompt string) float64 {
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println(prompt)
		scanner.Scan()
		val, err := strconv.ParseFloat(scanner.Text(), 64)
		if err != nil {
			fmt.Println("INVALID INPUT, TRY AGAIN")
			continue
		}
		return val
	}
}

func getYesNo(prompt string) bool {
	scanner := bufio.NewScanner(os.Stdin)
	fmt.Println(prompt)
	scanner.Scan()

	return (strings.ToUpper(scanner.Text())[0:1] == "Y")

}

func main() {
	fmt.Println("I am CHIEF NUMBERS FREEK, The GREAT INDIAN MATH GOD.")

	if getYesNo("\nAre you ready to take the test you called me out for(Yes or No)? ") {
		play()
	} else {
		fmt.Println("Ok, Nevermind. Let me go back to my great slumber, Bye")
	}
}
