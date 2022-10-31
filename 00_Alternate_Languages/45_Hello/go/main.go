package main

import (
	"bufio"
	"fmt"
	"os"
	"strings"
	"time"
)

type PROBLEM_TYPE int8

const (
	SEX PROBLEM_TYPE = iota
	HEALTH
	MONEY
	JOB
	UKNOWN
)

func getYesOrNo() (bool, bool, string) {
	scanner := bufio.NewScanner(os.Stdin)

	scanner.Scan()

	if strings.ToUpper(scanner.Text()) == "YES" {
		return true, true, scanner.Text()
	} else if strings.ToUpper(scanner.Text()) == "NO" {
		return true, false, scanner.Text()
	} else {
		return false, false, scanner.Text()
	}
}

func printTntro() {
	fmt.Println("                              HELLO")
	fmt.Println("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
	fmt.Print("\n\n\n")
	fmt.Println("HELLO. MY NAME IS CREATIVE COMPUTER.")
	fmt.Println("\nWHAT'S YOUR NAME?")
}

func askEnjoyQuestion(user string) {
	fmt.Printf("HI THERE %s, ARE YOU ENJOYING YOURSELF HERE?\n", user)

	for {
		valid, value, msg := getYesOrNo()

		if valid {
			if value {
				fmt.Printf("I'M GLAD TO HEAR THAT, %s.\n", user)
				fmt.Println()
			} else {
				fmt.Printf("OH, I'M SORRY TO HEAR THAT, %s. MAYBE WE CAN\n", user)
				fmt.Println("BRIGHTEN UP YOUR VISIT A BIT.")
			}
			break
		} else {
			fmt.Printf("%s, I DON'T UNDERSTAND YOUR ANSWER OF '%s'.\n", user, msg)
			fmt.Println("PLEASE ANSWER 'YES' OR 'NO'.  DO YOU LIKE IT HERE?")
		}
	}
}

func promptForProblems(user string) PROBLEM_TYPE {
	scanner := bufio.NewScanner(os.Stdin)
	fmt.Println()
	fmt.Printf("SAY %s, I CAN SOLVE ALL KINDS OF PROBLEMS EXCEPT\n", user)
	fmt.Println("THOSE DEALING WITH GREECE.  WHAT KIND OF PROBLEMS DO")
	fmt.Println("YOU HAVE? (ANSWER SEX, HEALTH, MONEY, OR JOB)")
	for {
		scanner.Scan()

		switch strings.ToUpper(scanner.Text()) {
		case "SEX":
			return SEX
		case "HEALTH":
			return HEALTH
		case "MONEY":
			return MONEY
		case "JOB":
			return JOB
		default:
			return UKNOWN
		}
	}
}

func promptTooMuchOrTooLittle() (bool, bool) {
	scanner := bufio.NewScanner(os.Stdin)

	scanner.Scan()

	if strings.ToUpper(scanner.Text()) == "TOO MUCH" {
		return true, true
	} else if strings.ToUpper(scanner.Text()) == "TOO LITTLE" {
		return true, false
	} else {
		return false, false
	}
}

func solveSexProblem(user string) {
	fmt.Println("IS YOUR PROBLEM TOO MUCH OR TOO LITTLE?")
	for {
		valid, tooMuch := promptTooMuchOrTooLittle()
		if valid {
			if tooMuch {
				fmt.Println("YOU CALL THAT A PROBLEM?!!  I SHOULD HAVE SUCH PROBLEMS!")
				fmt.Printf("IF IT BOTHERS YOU, %s, TAKE A COLD SHOWER.\n", user)
			} else {
				fmt.Printf("WHY ARE YOU HERE IN SUFFERN, %s?  YOU SHOULD BE\n", user)
				fmt.Println("IN TOKYO OR NEW YORK OR AMSTERDAM OR SOMEPLACE WITH SOME")
				fmt.Println("REAL ACTION.")
			}
			return
		} else {
			fmt.Printf("DON'T GET ALL SHOOK, %s, JUST ANSWER THE QUESTION\n", user)
			fmt.Println("WITH 'TOO MUCH' OR 'TOO LITTLE'.  WHICH IS IT?")
		}
	}
}

func solveHealthProblem(user string) {
	fmt.Printf("MY ADVICE TO YOU %s IS:\n", user)
	fmt.Println("     1.  TAKE TWO ASPRIN")
	fmt.Println("     2.  DRINK PLENTY OF FLUIDS (ORANGE JUICE, NOT BEER!)")
	fmt.Println("     3.  GO TO BED (ALONE)")
}

func solveMoneyProblem(user string) {
	fmt.Printf("SORRY, %s, I'M BROKE TOO.  WHY DON'T YOU SELL\n", user)
	fmt.Println("ENCYCLOPEADIAS OR MARRY SOMEONE RICH OR STOP EATING")
	fmt.Println("SO YOU WON'T NEED SO MUCH MONEY?")
}

func solveJobProblem(user string) {
	fmt.Printf("I CAN SYMPATHIZE WITH YOU %s.  I HAVE TO WORK\n", user)
	fmt.Println("VERY LONG HOURS FOR NO PAY -- AND SOME OF MY BOSSES")
	fmt.Printf("REALLY BEAT ON MY KEYBOARD.  MY ADVICE TO YOU, %s,\n", user)
	fmt.Println("IS TO OPEN A RETAIL COMPUTER STORE.  IT'S GREAT FUN.")
}

func askQuestionLoop(user string) {
	for {
		problem := promptForProblems(user)

		switch problem {
		case SEX:
			solveSexProblem(user)
		case HEALTH:
			solveHealthProblem(user)
		case MONEY:
			solveMoneyProblem(user)
		case JOB:
			solveJobProblem(user)
		case UKNOWN:
			fmt.Printf("OH %s, YOUR ANSWER IS GREEK TO ME.\n", user)
		}

		for {
			fmt.Println()
			fmt.Printf("ANY MORE PROBLEMS YOU WANT SOLVED, %s?\n", user)

			valid, value, _ := getYesOrNo()
			if valid {
				if value {
					fmt.Println("WHAT KIND (SEX, MONEY, HEALTH, JOB)")
					break
				} else {
					return
				}
			}
			fmt.Printf("JUST A SIMPLE 'YES' OR 'NO' PLEASE, %s\n", user)
		}
	}
}

func goodbyeUnhappy(user string) {
	fmt.Println()
	fmt.Printf("TAKE A WALK, %s.\n", user)
	fmt.Println()
	fmt.Println()
}

func goodbyeHappy(user string) {
	fmt.Printf("NICE MEETING YOU %s, HAVE A NICE DAY.\n", user)
}

func askForFee(user string) {
	fmt.Println()
	fmt.Printf("THAT WILL BE $5.00 FOR THE ADVICE, %s.\n", user)
	fmt.Println("PLEASE LEAVE THE MONEY ON THE TERMINAL.")
	time.Sleep(4 * time.Second)
	fmt.Print("\n\n\n")
	fmt.Println("DID YOU LEAVE THE MONEY?")

	for {
		valid, value, msg := getYesOrNo()
		if valid {
			if value {
				fmt.Printf("HEY, %s, YOU LEFT NO MONEY AT ALL!\n", user)
				fmt.Println("YOU ARE CHEATING ME OUT OF MY HARD-EARNED LIVING.")
				fmt.Println()
				fmt.Printf("WHAT A RIP OFF, %s!!!\n", user)
				fmt.Println()
			} else {
				fmt.Printf("THAT'S HONEST, %s, BUT HOW DO YOU EXPECT\n", user)
				fmt.Println("ME TO GO ON WITH MY PSYCHOLOGY STUDIES IF MY PATIENTS")
				fmt.Println("DON'T PAY THEIR BILLS?")
			}
			return
		} else {
			fmt.Printf("YOUR ANSWER OF '%s' CONFUSES ME, %s.\n", msg, user)
			fmt.Println("PLEASE RESPOND WITH 'YES' or 'NO'.")
		}
	}
}

func main() {
	scanner := bufio.NewScanner(os.Stdin)

	printTntro()
	scanner.Scan()
	userName := scanner.Text()
	fmt.Println()

	askEnjoyQuestion(userName)

	askQuestionLoop(userName)

	askForFee(userName)

	if false {
		goodbyeHappy(userName) // unreachable
	} else {
		goodbyeUnhappy(userName)
	}

}
