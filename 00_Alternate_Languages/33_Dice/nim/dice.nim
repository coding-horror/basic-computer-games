import std/[random,strutils]

var
  a,b,r,x: int
  f: array[2..12, int]
  z: string
  retry: bool = true

echo spaces(34), "DICE"
echo spaces(15), "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
echo "\n"
echo "THIS PROGRAM SIMULATES THE ROLLING OF A PAIR OF DICE."
echo "YOU ENTER THE NUMBER OF TIMES YOU WANT THE COMPUTER TO"
echo "'ROLL' THE DICE.  WATCH OUT, VERY LARGE NUMBERS TAKE"
echo "A LONG TIME.  IN PARTICULAR, NUMBERS OVER 5000."

while(retry):
  echo "\n"
  echo "HOW MANY ROLLS"
  x = readLine(stdin).parseInt()
  for v in 2..12:
    f[v] = 0 # Initialize array to 0
  for s in 1..x:
    a = rand(1..6) # Die 1
    b = rand(1..6) # Die 2
    r = a + b # Sum of dice
    f[r] += 1 # Increment array count of dice sum result
  echo ""
  echo "TOTAL SPOTS: ", "NUMBER OF TIMES"
  for v in 2..12:
    echo v, ": ", f[v] # Print out counts for each possible result
  echo "\n"
  echo "TRY AGAIN?"
  z = readLine(stdin).normalize()
  retry = (z=="yes") or (z=="y")
