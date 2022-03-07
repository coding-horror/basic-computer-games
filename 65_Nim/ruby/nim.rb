puts "NIM".center(80)
puts"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".center(80)
puts "\n\n\n"

#210 DIM A(100),B(100,10),D(2)
$pileArray = Array.new[100]
$bArray = Array.new
$dArray = Array.new[2]
$winOption = 0 # take-last option
$numberOfPiles = 1
$c = 0
$e = 0
$f = 0
$g = 0
$h = 0

def displayTheRules
puts "THE GAME IS PLAYED WITH A NUMBER OF PILES OF OBJECTS."
puts "ANY NUMBER OF OBJECTS ARE REMOVED FROM ONE PILE BY YOU AND"
puts "THE MACHINE ALTERNATELY.  ON YOUR TURN, YOU MAY TAKE"
puts "ALL THE OBJECTS THAT REMAIN IN ANY PILE, BUT YOU MUST"
puts "TAKE AT LEAST ONE OBJECT, AND YOU MAY TAKE OBJECTS FROM"
puts "ONLY ONE PILE ON A SINGLE TURN.  YOU MUST SPECIFY WHETHER"
puts "WINNING IS DEFINED AS TAKING OR NOT TAKING THE LAST OBJECT,"
puts "THE NUMBER OF PILES IN THE GAME, AND HOW MANY OBJECTS ARE"
puts "ORIGINALLY IN EACH PILE.  EACH PILE MAY CONTAIN A"
puts "DIFFERENT NUMBER OF OBJECTS."
puts "THE MACHINE WILL SHOW ITS MOVE BY LISTING EACH PILE AND THE"
puts "NUMBER OF OBJECTS REMAINING IN THE PILES AFTER  EACH OF ITS"
puts "MOVES."
end

def sub1570
    $z=0
    for i in 1..$numberOfPiles do
        if $pileArray[i] != 0 then
            return
        end
        $z=1
        return
    end
end

def playAnother
    put "do you want to play another game";
    return gets.strip.ucase == "YES"
end
puts "THIS IS THE GAME OF NIM."
print "DO YOU WANT INSTRUCTIONS?"
240
wantInstructions = gets.strip.upcase
if wantInstructions == "YES" then
    displayTheRules
end
#250 IF Z$="NO" THEN 440
#260 IF Z$="no" THEN 440
#270 IF Z$="YES" THEN displayTheRules
#280 IF Z$="yes" THEN displayTheRules
#290 PRINT "PLEASE ANSWER YES OR NO"
#300 GOTO 240

def sub490 # get number of piles
    print "ENTER NUMBER OF PILES:"
    while $numberOfPiles < 0 && $numberOfPiles <= 100 do
        $numberOfPiles = gets.strip.to_i
    end
end

def getPileSizes
    puts "ENTER PILE SIZES:"
    for i in 1..$numberOfPiles do
        print i
        while true do
            $pileArray[i] = gets.strip.to_i
            if $pileArray[i] < 2000 && $pileArray[i] > 0 then
                break
            end
        end
    end
end

def sub440 # get win option
    puts ""
    $winOption = 0
    while $winOption != 1 && q != 2 do
        puts "ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST"
        $winOption = gets.strip.to_i
    end
end

puts "DO YOU WANT TO MOVE FIRST?";
#630 INPUT Q9$
moveFirst = ""
while moveFirst != "YES" && moveFirst != "NO" do
    moveFirst = gets.strip.upcase
    case moveFirst
        when "YES"
            yourMove
        when "NO"
            machineMove
    end
end

#640 IF Q9$="YES" THEN 1450
#650 IF Q9$="yes" THEN 1450
#660 IF Q9$="NO" THEN 700
#670 IF Q9$="no" THEN 700
#680 PRINT "PLEASE ANSWER YES OR NO."
#690 GOTO 630

def machineMove
    if $winOption == 1 then
        sub940 # take last
    end
    $c=0
    for i in 1..$numberOfPiles do
    if $pileArray[i] != 0 then 770
    $c=$c+1
    if $c == 3 then
        sub840
    end
        $dArray[$c-1]=i
    end
    end

    if $c == 2 then
        sub920
    end
    if $pileArray[$dArray[0]] > 1 then
        machineWins
    end
    machineLoses
end

def machineLoses
    puts "MACHINE LOSES"
# 810 GOTO playAnother
    if playAnother then
        sub440 # loop for another
    end
end

def machineWins
    puts "MACHINE WINS"
#    830 GOTO playAnother
    if playAnother then
        sub440 # loop for another
    end
end

def sub840
    $c=0
    for i in 1..$numberOfPiles do
    if $pileArray[i] > 1 then
        sub940
    end
    if $pileArray[i] == 0 then 890
        $c=$c+1
    end
    if $c/2 != ($c/2).to_i then
        machineLoses
    end
    sub940 # goto
    end
end

def sub920
    if $pileArray[$dArray[0]] == 1 then
        machineWins
    end
    if $pileArray[$dArray[1]] == 1 then
        machineWins
    end
end

def sub940
    for i in 1..$numberOfPiles do
        e=$pileArray[i]
        for j in 0..10 do
        $f = $e/2
        $bArray[i][j] = 2*($f-($f.to_i))
        $e = $f.to_i
        end
    end
end

#for j in 10..0 STEP -1 do
10..0.step(-1).each do|index|
    $c=0
    $h=0
    for i in 1..$numberOfPiles do
        if $bArray[i][index] != 0 then
            $c=$c+1
            if $pileArray[i] > $h then
                $h = $pileArray[i]
                $g = i
            end
        end
    end
end

if $c/2 != ($c/2).to_i then 1190
end
$e = rand($numberOfPiles).to_i
#if $pileArray[$e] == 0 then 1140

$f = rand($pileArray[$e]).to_i
$pileArray[$e] = $pileArray[$e]-$f
sub1380
$pileArray[$g]=0
for j in 0..10 do
$bArray[$g][index]=0
$c=0
for i in 1..$numberOfPiles do
    if $bArray[i][index] != 0 then
            $c=$c+1
        end
    end
$pileArray[$g]=$pileArray[$g]+2*($c/2-($c/2)).to_i*2^j
end
if $winOption == 1 then
    sub1380
end
$c=0
for i in 1..$numberOfPiles do
if $pileArray[i]>1 then
    sub1380
end
if $pileArray[i] != 0 then
    $c=$c+1
end
if $c/2 == ($c/2).to_i then
    sub1380
end
$pileArray[$g] == 1 -$pileArray[$g]

def sub1380
    puts "PILE  SIZE"
    for i in 1..$numberOfPiles do
        put i
        put $pileArray[i]
    end
    if $winOption == 2 then # avoid take-last option
        yourMove
    end
    sub1570
    if $z == 1 then
        machineWins
    end
end

def yourMove
    put "YOUR MOVE - PILE, NUMBER TO BE REMOVED"
#    1460 INPUT x,y
x = gets.strip.to_i
y = gets.strip.to_i
    if x > $numberOfPiles then yourMove
    if x < 1 then yourMove
    if x != INT(x) then yourMove
    if y > $pileArray[x] then yourMove
    if y < 1 then
        yourMove
    end
    if y != INT(y) then
        yourMove
    end

    $pileArray[x] = $pileArray[x]-y
    sub1570 # gosub
    if $z == 1 then
        machineLoses
    end
#    1560 GOTO 700
end

end
end
end
end
end
