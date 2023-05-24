package main

import (
	"bufio"
	"fmt"
	"math"
	"math/rand"
	"os"
	"strconv"
	"strings"
	"time"
)

func printIntro() {
	fmt.Println("                                 GUNNER")
	fmt.Println("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
	fmt.Print("\n\n\n")
	fmt.Println("YOU ARE THE OFFICER-IN-CHARGE, GIVING ORDERS TO A GUN")
	fmt.Println("CREW, TELLING THEM THE DEGREES OF ELEVATION YOU ESTIMATE")
	fmt.Println("WILL PLACE A PROJECTILE ON TARGET.  A HIT WITHIN 100 YARDS")
	fmt.Println("OF THE TARGET WILL DESTROY IT.")
	fmt.Println()
}

func getFloat() float64 {
	scanner := bufio.NewScanner(os.Stdin)
	for {
		scanner.Scan()
		fl, err := strconv.ParseFloat(scanner.Text(), 64)

		if err != nil {
			fmt.Println("Invalid input")
			continue
		}

		return fl
	}
}

func play() {
	gunRange := int(40000*rand.Float64() + 20000)
	fmt.Printf("\nMAXIMUM RANGE OF YOUR GUN IS %d YARDS\n", gunRange)

	killedEnemies := 0
	S1 := 0

	for {
		targetDistance := int(float64(gunRange) * (0.1 + 0.8*rand.Float64()))
		shots := 0

		fmt.Printf("\nDISTANCE TO THE TARGET IS %d YARDS\n", targetDistance)

		for {
			fmt.Print("\n\nELEVATION? ")
			elevation := getFloat()

			if elevation > 89 {
				fmt.Println("MAXIMUM ELEVATION IS 89 DEGREES")
				continue
			}

			if elevation < 1 {
				fmt.Println("MINIMUM ELEVATION IS 1 DEGREE")
				continue
			}

			shots += 1

			if shots < 6 {
				B2 := 2 * elevation / 57.3
				shotImpact := int(float64(gunRange) * math.Sin(B2))
				shotProximity := int(targetDistance - shotImpact)

				if math.Abs(float64(shotProximity)) < 100 { // hit
					fmt.Printf("*** TARGET DESTROYED *** %d ROUNDS OF AMMUNITION EXPENDED.\n", shots)
					S1 += shots

					if killedEnemies == 4 {
						fmt.Printf("\n\nTOTAL ROUNDS EXPENDED WERE: %d\n", S1)
						if S1 > 18 {
							print("BETTER GO BACK TO FORT SILL FOR REFRESHER TRAINING!")
							return
						} else {
							print("NICE SHOOTING !!")
							return
						}
					} else {
						killedEnemies += 1
						fmt.Println("\nTHE FORWARD OBSERVER HAS SIGHTED MORE ENEMY ACTIVITY...")
						break
					}
				} else { // missed
					if shotProximity > 100 {
						fmt.Printf("SHORT OF TARGET BY %d YARDS.\n", int(math.Abs(float64(shotProximity))))
					} else {
						fmt.Printf("OVER TARGET BY %d YARDS.\n", int(math.Abs(float64(shotProximity))))
					}
				}
			} else {
				fmt.Print("\nBOOM !!!!   YOU HAVE JUST BEEN DESTROYED BY THE ENEMY.\n\n\n")
				fmt.Println("BETTER GO BACK TO FORT SILL FOR REFRESHER TRAINING!")
				return
			}
		}
	}
}

func main() {
	rand.Seed(time.Now().UnixNano())
	scanner := bufio.NewScanner(os.Stdin)

	printIntro()

	for {
		play()

		fmt.Print("TRY AGAIN (Y OR N)? ")
		scanner.Scan()

		if strings.ToUpper(scanner.Text())[0:1] != "Y" {
			fmt.Println("\nOK. RETURN TO BASE CAMP.")
			break
		}
	}
}
