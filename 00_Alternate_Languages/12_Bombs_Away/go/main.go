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

type Choice struct {
	idx string
	msg string
}

func playerSurvived() {
	fmt.Println("YOU MADE IT THROUGH TREMENDOUS FLAK!!")
}

func playerDeath() {
	fmt.Println("* * * * BOOM * * * *")
	fmt.Println("YOU HAVE BEEN SHOT DOWN.....")
	fmt.Println("DEARLY BELOVED, WE ARE GATHERED HERE TODAY TO PAY OUR")
	fmt.Println("LAST TRIBUTE...")
}

func missionSuccess() {
	fmt.Printf("DIRECT HIT!!!! %d KILLED.\n", int(100*rand.Int()))
	fmt.Println("MISSION SUCCESSFUL.")
}

// Takes a float between 0 and 1 and returns a boolean
// if the player has survived (based on random chance)
// Returns True if death, False if survived
func deathWithChance(probability float64) bool {
	return probability > rand.Float64()
}

func startNonKamikaziAttack() {
	numMissions := getIntInput("HOW MANY MISSIONS HAVE YOU FLOWN? ")

	for numMissions > 160 {
		fmt.Println("MISSIONS, NOT MILES...")
		fmt.Println("150 MISSIONS IS HIGH EVEN FOR OLD-TIMERS")
		numMissions = getIntInput("HOW MANY MISSIONS HAVE YOU FLOWN? ")
	}

	if numMissions > 100 {
		fmt.Println("THAT'S PUSHING THE ODDS!")
	}

	if numMissions < 25 {
		fmt.Println("FRESH OUT OF TRAINING, EH?")
	}

	fmt.Println()

	if float32(numMissions) > (160 * rand.Float32()) {
		missionSuccess()
	} else {
		missionFailure()
	}
}

func missionFailure() {
	fmt.Printf("MISSED TARGET BY %d MILES!\n", int(2+30*rand.Float32()))
	fmt.Println("NOW YOU'RE REALLY IN FOR IT !!")
	fmt.Println()

	enemyWeapons := getInputFromList("DOES THE ENEMY HAVE GUNS(1), MISSILES(2), OR BOTH(3)? ", []Choice{{idx: "1", msg: "GUNS"}, {idx: "2", msg: "MISSILES"}, {idx: "3", msg: "BOTH"}})

	// If there are no gunners (i.e. weapon choice 2) then
	// we say that the gunners have 0 accuracy for the purposes
	// of calculating probability of player death
	enemyGunnerAccuracy := 0.0
	if enemyWeapons.idx != "2" {
		enemyGunnerAccuracy = float64(getIntInput("WHAT'S THE PERCENT HIT RATE OF ENEMY GUNNERS (10 TO 50)? "))
		if enemyGunnerAccuracy < 10.0 {
			fmt.Println("YOU LIE, BUT YOU'LL PAY...")
			playerDeath()
		}
	}

	missileThreatWeighting := 35.0
	if enemyWeapons.idx == "1" {
		missileThreatWeighting = 0
	}

	death := deathWithChance((enemyGunnerAccuracy + missileThreatWeighting) / 100)

	if death {
		playerDeath()
	} else {
		playerSurvived()
	}
}

func playItaly() {
	targets := []Choice{{idx: "1", msg: "SHOULD BE EASY -- YOU'RE FLYING A NAZI-MADE PLANE."}, {idx: "2", msg: "BE CAREFUL!!!"}, {idx: "3", msg: "YOU'RE GOING FOR THE OIL, EH?"}}
	target := getInputFromList("YOUR TARGET -- ALBANIA(1), GREECE(2), NORTH AFRICA(3)", targets)
	fmt.Println(target.msg)
	startNonKamikaziAttack()
}

func playAllies() {
	aircraftMessages := []Choice{{idx: "1", msg: "YOU'VE GOT 2 TONS OF BOMBS FLYING FOR PLOESTI."}, {idx: "2", msg: "YOU'RE DUMPING THE A-BOMB ON HIROSHIMA."}, {idx: "3", msg: "YOU'RE CHASING THE BISMARK IN THE NORTH SEA."}, {idx: "4", msg: "YOU'RE BUSTING A GERMAN HEAVY WATER PLANT IN THE RUHR."}}
	aircraft := getInputFromList("AIRCRAFT -- LIBERATOR(1), B-29(2), B-17(3), LANCASTER(4): ", aircraftMessages)
	fmt.Println(aircraft.msg)
	startNonKamikaziAttack()
}

func playJapan() {
	acknowledgeMessage := []Choice{{idx: "Y", msg: "Y"}, {idx: "N", msg: "N"}}
	firstMission := getInputFromList("YOU'RE FLYING A KAMIKAZE MISSION OVER THE USS LEXINGTON.\nYOUR FIRST KAMIKAZE MISSION? (Y OR N): ", acknowledgeMessage)
	if firstMission.msg == "N" {
		playerDeath()
	}
	if rand.Float64() > 0.65 {
		missionSuccess()
	} else {
		playerDeath()
	}
}

func playGermany() {
	targets := []Choice{{idx: "1", msg: "YOU'RE NEARING STALINGRAD."}, {idx: "2", msg: "NEARING LONDON.  BE CAREFUL, THEY'VE GOT RADAR."}, {idx: "3", msg: "NEARING VERSAILLES.  DUCK SOUP.  THEY'RE NEARLY DEFENSELESS."}}
	target := getInputFromList("A NAZI, EH?  OH WELL.  ARE YOU GOING FOR RUSSIA(1),\nENGLAND(2), OR FRANCE(3)? ", targets)
	fmt.Println(target.msg)
	startNonKamikaziAttack()
}

func playGame() {
	fmt.Println("YOU ARE A PILOT IN A WORLD WAR II BOMBER.")
	side := getInputFromList("WHAT SIDE -- ITALY(1), ALLIES(2), JAPAN(3), GERMANY(4): ", []Choice{{idx: "1", msg: "ITALY"}, {idx: "2", msg: "ALLIES"}, {idx: "3", msg: "JAPAN"}, {idx: "4", msg: "GERMANY"}})
	switch side.idx {
	case "1":
		playItaly()
	case "2":
		playAllies()
	case "3":
		playJapan()
	case "4":
		playGermany()
	}
}

func main() {
	rand.Seed(time.Now().UnixNano())

	for {
		playGame()
		if getInputFromList("ANOTHER MISSION (Y OR N):", []Choice{{idx: "Y", msg: "Y"}, {idx: "N", msg: "N"}}).msg == "N" {
			break
		}
	}
}

func getInputFromList(prompt string, choices []Choice) Choice {
	scanner := bufio.NewScanner(os.Stdin)
	for {
		fmt.Println(prompt)
		scanner.Scan()
		choice := scanner.Text()
		for _, c := range choices {
			if strings.EqualFold(strings.ToUpper(choice), strings.ToUpper(c.idx)) {
				return c
			}
		}
		fmt.Println("TRY AGAIN...")
	}
}

func getIntInput(prompt string) int {
	scanner := bufio.NewScanner(os.Stdin)
	for {
		fmt.Println(prompt)
		scanner.Scan()
		choice, err := strconv.Atoi(scanner.Text())
		if err != nil {
			fmt.Println("TRY AGAIN...")
			continue
		} else {
			return choice
		}
	}
}
