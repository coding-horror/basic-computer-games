blockWidth = input("Horizontal? ").val
if blockWidth <= 1 then blockWidth = 3

blockHeight = input("Vertical? ").val
if blockHeight <= 1 then blockHeight = 5

inp = input("Centered? ").upper
centered = inp and inp[0] > "P"

printChar = input("Character (type 'all' if you want character being printed)? ")

statement = input("Statement: ").upper

//input("Set page")	  // <-- opportunity to set your pin-feed printer before proceeding!

// Define the character data.  For each character, we have 7 numbers
// which are the 9-bit binary representation of each row, plus one.
data = {}
data[" "] = [0,0,0,0,0,0,0]
data["!"] = [1,1,1,384,1,1,1]
data["?"] = [5,3,2,354,18,11,5]
data["."] = [1,1,129,449,129,1,1]
data["*"] = [69,41,17,512,17,41,69]
data["="] = [41,41,41,41,41,41,41]
data["0"] = [57,69,131,258,131,69,57]
data["1"] = [0,0,261,259,512,257,257]
data["2"] = [261,387,322,290,274,267,261]
data["3"] = [66,130,258,274,266,150,100]
data["4"] = [33,49,41,37,35,512,33]
data["5"] = [160,274,274,274,274,274,226]
data["6"] = [194,291,293,297,305,289,193]
data["7"] = [258,130,66,34,18,10,8]
data["8"] = [69,171,274,274,274,171,69]
data["9"] = [263,138,74,42,26,10,7]
data["A"] = [505,37,35,34,35,37,505]
data["B"] = [512,274,274,274,274,274,239]
data["C"] = [125,131,258,258,258,131,69]
data["D"] = [512,258,258,258,258,131,125]
data["E"] = [512,274,274,274,274,258,258]
data["F"] = [512,18,18,18,18,2,2]
data["G"] = [125,131,258,258,290,163,101]
data["H"] = [512,17,17,17,17,17,512]
data["I"] = [258,258,258,512,258,258,258]
data["J"] = [65,129,257,257,257,129,128]
data["K"] = [512,17,17,41,69,131,258]
data["L"] = [512,257,257,257,257,257,257]
data["M"] = [512,7,13,25,13,7,512]
data["N"] = [512,7,9,17,33,193,512]
data["O"] = [125,131,258,258,258,131,125]
data["P"] = [512,18,18,18,18,18,15]
data["Q"] = [125,131,258,258,322,131,381]
data["R"] = [512,18,18,50,82,146,271]
data["S"] = [69,139,274,274,274,163,69]
data["T"] = [2,2,2,512,2,2,2]
data["U"] = [128,129,257,257,257,129,128]
data["V"] = [64,65,129,257,129,65,64]
data["W"] = [256,257,129,65,129,257,256]
data["X"] = [388,69,41,17,41,69,388]
data["Y"] = [8,9,17,481,17,9,8]
data["Z"] = [386,322,290,274,266,262,260]

for c in statement
	if not data.hasIndex(c) then continue
	
	// Print character c in giant sideways banner-style!
	for datum in data[c]
		if datum then datum -= 1	// remove spurious +1
		if printChar.upper != "ALL" then c = printChar
		
		for lineRepeat in range(blockWidth-1)
			if centered then print " " * (34 - 4.5*blockHeight), ""

			for bitPos in range(9,0)
				if bitAnd(datum, 2^bitPos) then charToPrint=c else charToPrint=" "
				print charToPrint * blockHeight, ""
			end for	// next bitPos

			print
			wait 0.01  // put in a small pause so it's not too fast to see!
		end for  // next lineRepeat (repeating line according to entered Y value)

	end for  // next datum (row of this character)
	
	// Add a little space after each character
	for i in range(1, 2 * blockWidth)
		print
		wait 0.01
	end for
end for  // next character in the message
