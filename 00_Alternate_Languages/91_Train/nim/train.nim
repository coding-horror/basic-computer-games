import std/[random,strutils]

var
  carSpeed, diff, err, guess, trainSpeed, carTime: int
  stillplaying: bool = true

randomize() # Seed the random number generator

# Return a tuple that'll be carSpeed, diff, trainSpeed
proc randomNumbers(): (int,int,int) =
  result = (rand(41..65), rand(6..20), rand(21..39))

# Do we want to play again?
proc tryAgain(): bool =
  echo "ANOTHER PROBLEM (YES OR NO)"
  var answer = readLine(stdin).normalize()
  result = (answer == "y") or (answer == "yes")

echo spaces(33), "TRAIN"
echo spaces(15), "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
echo "\n"
echo "TIME - SPEED DISTANCE EXERCISE"

while stillplaying:
  echo ""
  (carSpeed, diff, trainSpeed) = randomNumbers() # Get random numbers for prompt
  echo "A CAR TRAVELING ", carSpeed, " MPH CAN MAKE A CERTAIN TRIP IN"
  echo diff, " HOURS LESS THAN A TRAIN TRAVELING AT ", trainSpeed, " MPH."
  echo "HOW LONG DOES THE TRIP TAKE BY CAR?"
  guess = readLine(stdin).parseInt() # Get guess
  carTime = (diff * trainSpeed / (carSpeed - trainSpeed)).toInt() # Calculate answer
  err = (((carTime - guess) * 100) / guess).toInt().abs() # Calculate error to an absolute value
  if err > 5: # Error within 5%?
    echo "SORRY.  YOU WERE OFF BY ", err, " PERCENT."
  else:
    echo "GOOD! ANSWER WITHIN ", err, " PERCENT."
  echo "CORRECT ANSWER IS ", carTime, " HOURS."
  stillplaying = tryAgain()
