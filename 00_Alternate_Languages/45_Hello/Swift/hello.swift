
func tab(_ number_Of_Spaces: Int) {
    var spaces = ""
    
    for _ in 1...number_Of_Spaces {
        spaces += " "
    }
    print(spaces, terminator:"")
}


func get_Input() -> String {
    let input = readLine()
    return (input == nil ? "" : input!.uppercased())
}


func main()
{
    var done = false,
        answered = false,
        maybe_More = false,
        paid = false
    var reply = ""
    var name = "STRANGER"
    
    tab (33)
    print("HELLO")
    tab (15)
    print("CREATIVE COMPUTING   MORRISTOWN, NEW JERSEY\n")
    
    print("HELLO.  MY NAME IS CREATIVE COMPUTER.")
    print("WHAT'S YOUR NAME? ")
    let input = readLine()
    if (input != nil && input != "") {
        name = input!.uppercased()
    }
    
    print("\nHI THERE, \(name), ARE YOU ENJOYING YOURSELF HERE?")

    reply = get_Input()
    while (reply != "YES" && reply != "NO") {
        print("\(name), I DON'T UNDERSTAND YOUR ANSWER OF '\(reply)'.")
        print("PLEASE ANSWER 'YES' OR 'NO'. DO YOU LIKE IT HERE?")
        reply = get_Input()
    }
    
    if (reply == "YES") {
        print("\nI'M GLAD TO HEAR THAT, \(name).\n")
    }
    else {
        print("\nOH, I'M SORRY TO HEAR THAT, \(name). MAYBE WE CAN "
            + "BRIGHTEN UP YOUR VISIT A BIT.\n")
    }

    print("SAY, \(name), I CAN SOLVE ALL KINDS OF PROBLEMS EXCEPT "
        + "THOSE DEALING WITH GREECE.  WHAT KIND OF PROBLEMS DO "
        + "YOU HAVE (ANSWER SEX, HEALTH, MONEY, OR JOB)?")

    while (!done) {
        reply = get_Input()
        
        if (reply == "JOB") {
            print("\nI CAN SYMPATHIZE WITH YOU \(name).   I HAVE TO WORK "
                + "VERY LONG HOURS FOR NO PAY -- AND SOME OF MY BOSSES "
                + "REALLY BEAT ON MY KEYBOARD.    MY ADVICE TO YOU, \(name), IS TO "
                + "OPEN A RETAIL COMPUTER STORE.  IT'S GREAT FUN.\n")
        }

        else if (reply == "MONEY") {
            print("\nSORRY, \(name), I'M BROKE TOO.  WHY DON'T YOU SELL "
                + "ENCYCLOPEADIAS OR MARRY SOMEONE RICH OR STOP EATING "
                + "SO YOU WON'T NEED SO MUCH MONEY?\n")
        }
        
        else if (reply == "HEALTH") {
            print("\nMY ADVICE TO YOU \(name) IS:")
            print("         1.  TAKE TWO ASPRIN")
            print("         2.  DRINK PLENTY OF FLUIDS (ORANGE JUICE, NOT BEER!)")
            print("         3.  GO TO BED (ALONE)\n")
        }
        
        else if (reply == "SEX") {
            print("\nIS YOUR PROBLEM TOO MUCH OR TOO LITTLE?")
            
            answered = false
            while (!answered) {
                reply = get_Input()
                if (reply == "TOO MUCH") {
                    print("\nYOU CALL THAT A PROBLEM?!!  I SHOULD HAVE SUCH PROBLEMS!")
                    print("IF IT BOTHERS YOU, \(name), TAKE A COLD SHOWER.\n")
                    answered = true
                }
                else if (reply == "TOO LITTLE") {
                    print("\nWHY ARE YOU HERE IN SUFFERN, \(name)? YOU SHOULD BE "
                        + "IN TOKYO OR NEW YORK OR AMSTERDAM OR SOMEPLACE WITH SOME "
                        + "REAL ACTION.\n")
                    answered = true
                }
                else {
                    print("\nDON'T GET ALL SHOOK, \(name), JUST ANSWER THE QUESTION "
                        + "WITH 'TOO MUCH' OR 'TOO LITTLE'. WHICH IS IT?")
                }
            }
        }
        
        else {  // not one of the prescribed categories
            print("\nOH, \(name), YOUR ANSWER OF '\(reply)' IS GREEK TO ME.\n")
        }
        
        print("\nANY MORE PROBLEMS YOU WANT SOLVED, \(name)? ")
        
        maybe_More = true
        while (maybe_More) {
            reply = get_Input()
            if (reply == "NO") {
                done = true
                maybe_More = false
            }
            else if (reply == "YES") {
                print("\nWHAT KIND (SEX, MONEY, HEALTH, JOB) ")
                maybe_More = false
            }
            else {
                print("\nJUST A SIMPLE 'YES' OR 'NO' PLEASE, \(name). ")
            }
        } // no further questions
    } // end of 'not done' loop
    
    print("\nTHAT WILL BE $5.00 FOR THE ADVICE, \(name).")
    print("PLEASE LEAVE THE MONEY ON THE TERMINAL.")
    // pause a few seconds
    print("\n\n\nDID YOU LEAVE THE MONEY? ")
    reply = get_Input()
    while (!paid) {
        if (reply == "YES") {
            print("\nHEY, \(name)??? YOU LEFT NO MONEY AT ALL!")
            print("YOU ARE CHEATING ME OUT OF MY HARD-EARNED LIVING.\n")
            print("WHAT A RIP OFF, \(name)!!!\n")
            print("TAKE A WALK, \(name).")
            paid = true
        }
        else if (reply == "NO") {
            print("THAT'S HONEST, \(name), BUT HOW DO YOU EXPECT "
                + "ME TO GO ON WITH MY PSYCHOLOGY STUDIES IF MY PATIENTS "
                + "DON'T PAY THEIR BILLS?\n")
            print("NICE MEETING YOU, \(name), HAVE A NICE DAY.")
                paid = true
        }
        else {
            print("YOUR ANSWER OF '\(reply)' CONFUSES ME, \(name).")
            print("PLEASE RESPOND WITH 'YES' OR 'NO'.")
        }
    }
}

main()
