package main

import (
	"bufio"
	"fmt"
	"log"
	"math/rand"
	"os"
	"strconv"
	"strings"
	"time"
)

const (
	MAXFURS    = 190
	STARTFUNDS = 600
)

type Fur int8

const (
	FUR_MINK Fur = iota
	FUR_BEAVER
	FUR_ERMINE
	FUR_FOX
)

type Fort int8

const (
	FORT_MONTREAL Fort = iota
	FORT_QUEBEC
	FORT_NEWYORK
)

type GameState int8

const (
	STARTING GameState = iota
	TRADING
	CHOOSINGFORT
	TRAVELLING
)

func FURS() []string {
	return []string{"MINK", "BEAVER", "ERMINE", "FOX"}
}

func FORTS() []string {
	return []string{"HOCHELAGA (MONTREAL)", "STADACONA (QUEBEC)", "NEW YORK"}
}

type Player struct {
	funds float32
	furs  []int
}

func NewPlayer() Player {
	p := Player{}
	p.funds = STARTFUNDS
	p.furs = make([]int, 4)
	return p
}

func (p *Player) totalFurs() int {
	f := 0
	for _, v := range p.furs {
		f += v
	}
	return f
}

func (p *Player) lostFurs() {
	for f := 0; f < len(p.furs); f++ {
		p.furs[f] = 0
	}
}

func printTitle() {
	fmt.Println("                               FUR TRADER")
	fmt.Println("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
	fmt.Println()
	fmt.Println()
	fmt.Println()
}

func printIntro() {
	fmt.Println("YOU ARE THE LEADER OF A FRENCH FUR TRADING EXPEDITION IN ")
	fmt.Println("1776 LEAVING THE LAKE ONTARIO AREA TO SELL FURS AND GET")
	fmt.Println("SUPPLIES FOR THE NEXT YEAR.  YOU HAVE A CHOICE OF THREE")
	fmt.Println("FORTS AT WHICH YOU MAY TRADE.  THE COST OF SUPPLIES")
	fmt.Println("AND THE AMOUNT YOU RECEIVE FOR YOUR FURS WILL DEPEND")
	fmt.Println("ON THE FORT THAT YOU CHOOSE.")
	fmt.Println()
}

func getFortChoice() Fort {
	scanner := bufio.NewScanner(os.Stdin)

	for {
		fmt.Println()
		fmt.Println("YOU MAY TRADE YOUR FURS AT FORT 1, FORT 2,")
		fmt.Println("OR FORT 3.  FORT 1 IS FORT HOCHELAGA (MONTREAL)")
		fmt.Println("AND IS UNDER THE PROTECTION OF THE FRENCH ARMY.")
		fmt.Println("FORT 2 IS FORT STADACONA (QUEBEC) AND IS UNDER THE")
		fmt.Println("PROTECTION OF THE FRENCH ARMY.  HOWEVER, YOU MUST")
		fmt.Println("MAKE A PORTAGE AND CROSS THE LACHINE RAPIDS.")
		fmt.Println("FORT 3 IS FORT NEW YORK AND IS UNDER DUTCH CONTROL.")
		fmt.Println("YOU MUST CROSS THROUGH IROQUOIS LAND.")
		fmt.Println("ANSWER 1, 2, OR 3.")
		fmt.Print(">> ")
		scanner.Scan()

		f, err := strconv.Atoi(scanner.Text())
		if err != nil || f < 1 || f > 3 {
			fmt.Println("Invalid input, Try again ... ")
			continue
		}
		return Fort(f)
	}
}

func printFortComment(f Fort) {
	fmt.Println()
	switch f {
	case FORT_MONTREAL:
		fmt.Println("YOU HAVE CHOSEN THE EASIEST ROUTE.  HOWEVER, THE FORT")
		fmt.Println("IS FAR FROM ANY SEAPORT.  THE VALUE")
		fmt.Println("YOU RECEIVE FOR YOUR FURS WILL BE LOW AND THE COST")
		fmt.Println("OF SUPPLIES HIGHER THAN AT FORTS STADACONA OR NEW YORK.")
	case FORT_QUEBEC:
		fmt.Println("YOU HAVE CHOSEN A HARD ROUTE.  IT IS, IN COMPARSION,")
		fmt.Println("HARDER THAN THE ROUTE TO HOCHELAGA BUT EASIER THAN")
		fmt.Println("THE ROUTE TO NEW YORK.  YOU WILL RECEIVE AN AVERAGE VALUE")
		fmt.Println("FOR YOUR FURS AND THE COST OF YOUR SUPPLIES WILL BE AVERAGE.")
	case FORT_NEWYORK:
		fmt.Println("YOU HAVE CHOSEN THE MOST DIFFICULT ROUTE.  AT")
		fmt.Println("FORT NEW YORK YOU WILL RECEIVE THE HIGHEST VALUE")
		fmt.Println("FOR YOUR FURS.  THE COST OF YOUR SUPPLIES")
		fmt.Println("WILL BE LOWER THAN AT ALL THE OTHER FORTS.")
	}
	fmt.Println()
}

func getYesOrNo() string {
	scanner := bufio.NewScanner(os.Stdin)
	for {
		fmt.Println("ANSWER YES OR NO")
		scanner.Scan()
		if strings.ToUpper(scanner.Text())[0:1] == "Y" {
			return "Y"
		} else if strings.ToUpper(scanner.Text())[0:1] == "N" {
			return "N"
		}
	}
}

func getFursPurchase() []int {
	scanner := bufio.NewScanner(os.Stdin)
	fmt.Printf("YOUR %d FURS ARE DISTRIBUTED AMONG THE FOLLOWING\n", MAXFURS)
	fmt.Println("KINDS OF PELTS: MINK, BEAVER, ERMINE AND FOX.")
	fmt.Println()

	purchases := make([]int, 4)

	for i, f := range FURS() {
	retry:
		fmt.Printf("HOW MANY %s DO YOU HAVE: ", f)
		scanner.Scan()
		count, err := strconv.Atoi(scanner.Text())
		if err != nil {
			fmt.Println("INVALID INPUT, TRY AGAIN ...")
			goto retry
		}
		purchases[i] = count
	}

	return purchases
}

func main() {
	rand.Seed(time.Now().UnixNano())

	printTitle()

	gameState := STARTING
	whichFort := FORT_NEWYORK
	var (
		minkPrice   int
		erminePrice int
		beaverPrice int
		foxPrice    int
	)
	player := NewPlayer()

	for {
		switch gameState {
		case STARTING:
			printIntro()
			fmt.Println("DO YOU WISH TO TRADE FURS?")
			if getYesOrNo() == "N" {
				os.Exit(0)
			}
			gameState = TRADING
		case TRADING:
			fmt.Println()
			fmt.Printf("YOU HAVE $ %1.2f IN SAVINGS\n", player.funds)
			fmt.Printf("AND %d FURS TO BEGIN THE EXPEDITION\n", MAXFURS)
			player.furs = getFursPurchase()

			if player.totalFurs() > MAXFURS {
				fmt.Println()
				fmt.Println("YOU MAY NOT HAVE THAT MANY FURS.")
				fmt.Println("DO NOT TRY TO CHEAT.  I CAN ADD.")
				fmt.Println("YOU MUST START AGAIN.")
				gameState = STARTING
			} else {
				gameState = CHOOSINGFORT
			}
		case CHOOSINGFORT:
			whichFort = getFortChoice()
			printFortComment(whichFort)
			fmt.Println("DO YOU WANT TO TRADE AT ANOTHER FORT?")
			changeFort := getYesOrNo()
			if changeFort == "N" {
				gameState = TRAVELLING
			}
		case TRAVELLING:
			switch whichFort {
			case FORT_MONTREAL:
				minkPrice = (int((0.2*rand.Float64()+0.70)*100+0.5) / 100)
				erminePrice = (int((0.2*rand.Float64()+0.65)*100+0.5) / 100)
				beaverPrice = (int((0.2*rand.Float64()+0.75)*100+0.5) / 100)
				foxPrice = (int((0.2*rand.Float64()+0.80)*100+0.5) / 100)

				fmt.Println("SUPPLIES AT FORT HOCHELAGA COST $150.00.")
				fmt.Println("YOUR TRAVEL EXPENSES TO HOCHELAGA WERE $10.00.")
				player.funds -= 160
			case FORT_QUEBEC:
				minkPrice = (int((0.30*rand.Float64()+0.85)*100+0.5) / 100)
				erminePrice = (int((0.15*rand.Float64()+0.80)*100+0.5) / 100)
				beaverPrice = (int((0.20*rand.Float64()+0.90)*100+0.5) / 100)
				foxPrice = (int((0.25*rand.Float64()+1.10)*100+0.5) / 100)

				event := int(10*rand.Float64()) + 1
				if event <= 2 {
					fmt.Println("YOUR BEAVER WERE TOO HEAVY TO CARRY ACROSS")
					fmt.Println("THE PORTAGE. YOU HAD TO LEAVE THE PELTS, BUT FOUND")
					fmt.Println("THEM STOLEN WHEN YOU RETURNED.")
					player.furs[FUR_BEAVER] = 0
				} else if event <= 6 {
					fmt.Println("YOU ARRIVED SAFELY AT FORT STADACONA.")
				} else if event <= 8 {
					fmt.Println("YOUR CANOE UPSET IN THE LACHINE RAPIDS.  YOU")
					fmt.Println("LOST ALL YOUR FURS.")
					player.lostFurs()
				} else if event <= 10 {
					fmt.Println("YOUR FOX PELTS WERE NOT CURED PROPERLY.")
					fmt.Println("NO ONE WILL BUY THEM.")
					player.furs[FUR_FOX] = 0
				} else {
					log.Fatal("Unexpected error")
				}

				fmt.Println()
				fmt.Println("SUPPLIES AT FORT STADACONA COST $125.00.")
				fmt.Println("YOUR TRAVEL EXPENSES TO STADACONA WERE $15.00.")
				player.funds -= 140
			case FORT_NEWYORK:
				minkPrice = (int((0.15*rand.Float64()+1.05)*100+0.5) / 100)
				erminePrice = (int((0.15*rand.Float64()+0.95)*100+0.5) / 100)
				beaverPrice = (int((0.25*rand.Float64()+1.00)*100+0.5) / 100)
				foxPrice = (int((0.25*rand.Float64()+1.05)*100+0.5) / 100) // not in original code

				event := int(10*rand.Float64()) + 1
				if event <= 2 {
					fmt.Println("YOU WERE ATTACKED BY A PARTY OF IROQUOIS.")
					fmt.Println("ALL PEOPLE IN YOUR TRADING GROUP WERE")
					fmt.Println("KILLED.  THIS ENDS THE GAME.")
					os.Exit(0)
				} else if event <= 6 {
					fmt.Println("YOU WERE LUCKY.  YOU ARRIVED SAFELY")
					fmt.Println("AT FORT NEW YORK.")
				} else if event <= 8 {
					fmt.Println("YOU NARROWLY ESCAPED AN IROQUOIS RAIDING PARTY.")
					fmt.Println("HOWEVER, YOU HAD TO LEAVE ALL YOUR FURS BEHIND.")
					player.lostFurs()
				} else if event <= 10 {
					minkPrice /= 2
					foxPrice /= 2
					fmt.Println("YOUR MINK AND BEAVER WERE DAMAGED ON YOUR TRIP.")
					fmt.Println("YOU RECEIVE ONLY HALF THE CURRENT PRICE FOR THESE FURS.")
				} else {
					log.Fatal("Unexpected error")
				}

				fmt.Println()
				fmt.Println("SUPPLIES AT NEW YORK COST $85.00.")
				fmt.Println("YOUR TRAVEL EXPENSES TO NEW YORK WERE $25.00.")
				player.funds -= 110
			}

			beaverValue := beaverPrice * player.furs[FUR_BEAVER]
			foxValue := foxPrice * player.furs[FUR_FOX]
			ermineValue := erminePrice * player.furs[FUR_ERMINE]
			minkValue := minkPrice * player.furs[FUR_MINK]

			fmt.Println()
			fmt.Printf("YOUR BEAVER SOLD FOR $%6.2f\n", float64(beaverValue))
			fmt.Printf("YOUR FOX SOLD FOR    $%6.2f\n", float64(foxValue))
			fmt.Printf("YOUR ERMINE SOLD FOR $%6.2f\n", float64(ermineValue))
			fmt.Printf("YOUR MINK SOLD FOR   $%6.2f\n", float64(minkValue))

			player.funds += float32(beaverValue + foxValue + ermineValue + minkValue)

			fmt.Println()
			fmt.Printf("YOU NOW HAVE $%1.2f INCLUDING YOUR PREVIOUS SAVINGS\n", player.funds)
			fmt.Println("\nDO YOU WANT TO TRADE FURS NEXT YEAR?")
			if getYesOrNo() == "N" {
				os.Exit(0)
			} else {
				gameState = TRADING
			}
		}
	}
}
