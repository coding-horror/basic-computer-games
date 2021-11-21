// HELLO
//
// Converted from BASIC to Javascript by Oscar Toledo G. (nanochess)
//

function print(str)
{
    document.getElementById("output").appendChild(document.createTextNode(str));
}

function input()
{
    var input_element;
    var input_str;
    
    return new Promise(function (resolve) {
                       input_element = document.createElement("INPUT");
                       
                       print("? ");
                       input_element.setAttribute("type", "text");
                       input_element.setAttribute("length", "50");
                       document.getElementById("output").appendChild(input_element);
                       input_element.focus();
                       input_str = undefined;
                       input_element.addEventListener("keydown", function (event) {
                                                      if (event.keyCode == 13) {
                                                      input_str = input_element.value;
                                                      document.getElementById("output").removeChild(input_element);
                                                      print(input_str);
                                                      print("\n");
                                                      resolve(input_str);
                                                      }
                                                      });
                       });
}

function tab(space)
{
    var str = "";
    while (space-- > 0)
        str += " ";
    return str;
}

// Main control section
async function main()
{
    print(tab(33) + "HELLO\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("HELLO.  MY NAME IS CREATIVE COMPUTER.\n");
    print("\n");
    print("\n");
    print("WHAT'S YOUR NAME");
    ns = await input();
    print("\n");
    print("HI THERE, " + ns + ", ARE YOU ENJOYING YOURSELF HERE");
    while (1) {
        bs = await input();
        print("\n");
        if (bs == "YES") {
            print("I'M GLAD TO HEAR THAT, " + ns + ".\n");
            print("\n");
            break;
        } else if (bs == "NO") {
            print("OH, I'M SORRY TO HEAR THAT, " + ns + ". MAYBE WE CAN\n");
            print("BRIGHTEN UP YOUR VISIT A BIT.\n");
            break;
        } else {
            print("PLEASE ANSWER 'YES' OR 'NO'.  DO YOU LIKE IT HERE");
        }
    }
    print("\n");
    print("SAY, " + ns + ", I CAN SOLVED ALL KINDS OF PROBLEMS EXCEPT\n");
    print("THOSE DEALING WITH GREECE.  WHAT KIND OF PROBLEMS DO\n");
    print("YOU HAVE (ANSWER SEX, HEALTH, MONEY, OR JOB)");
    while (1) {
        cs = await input();
        print("\n");
        if (cs != "SEX" && cs != "HEALTH" && cs != "MONEY" && cs != "JOB") {
            print("OH, " + ns + ", YOUR ANSWER OF " + cs + " IS GREEK TO ME.\n");
        } else if (cs == "JOB") {
            print("I CAN SYMPATHIZE WITH YOU " + ns + ".  I HAVE TO WORK\n");
            print("VERY LONG HOURS FOR NO PAY -- AND SOME OF MY BOSSES\n");
            print("REALLY BEAT ON MY KEYBOARD.  MY ADVICE TO YOU, " + ns + ",\n");
            print("IS TO OPEN A RETAIL COMPUTER STORE.  IT'S GREAT FUN.\n");
        } else if (cs == "MONEY") {
            print("SORRY, " + ns + ", I'M BROKE TOO.  WHY DON'T YOU SELL\n");
            print("ENCYCLOPEADIAS OR MARRY SOMEONE RICH OR STOP EATING\n");
            print("SO YOU WON'T NEED SO MUCH MONEY?\n");
        } else if (cs == "HEALTH") {
            print("MY ADVICE TO YOU " + ns + " IS:\n");
            print("     1.  TAKE TWO ASPRIN\n");
            print("     2.  DRINK PLENTY OF FLUIDS (ORANGE JUICE, NOT BEER!)\n");
            print("     3.  GO TO BED (ALONE)\n");
        } else {
            print("IS YOUR PROBLEM TOO MUCH OR TOO LITTLE");
            while (1) {
                ds = await input();
                print("\n");
                if (ds == "TOO MUCH") {
                    print("YOU CALL THAT A PROBLEM?!!  I SHOULD HAVE SUCH PROBLEMS!\n");
                    print("IF IT BOTHERS YOU, " + ns + ", TAKE A COLD SHOWER.\n");
                    break;
                } else if (ds == "TOO LITTLE") {
                    print("WHY ARE YOU HERE IN SUFFERN, " + ns + "?  YOU SHOULD BE\n");
                    print("IN TOKYO OR NEW YORK OR AMSTERDAM OR SOMEPLACE WITH SOME\n");
                    print("REAL ACTION.\n");
                    break;
                } else {
                    print("DON'T GET ALL SHOOK, " + ns + ", JUST ANSWER THE QUESTION\n");
                    print("WITH 'TOO MUCH' OR 'TOO LITTLE'.  WHICH IS IT");
                }
            }
        }
        print("\n");
        print("ANY MORE PROBLEMS YOU WANT SOLVED, " + ns);
        es = await input();
        print("\n");
        if (es == "YES") {
            print("WHAT KIND (SEX, MONEY, HEALTH, JOB)");
        } else if (es == "NO") {
            print("THAT WILL BE $5.00 FOR THE ADVICE, " + ns + ".\n");
            print("PLEASE LEAVE THE MONEY ON THE TERMINAL.\n");
            print("\n");
//            d = new Date().valueOf();
//            while (new Date().valueOf() - d < 2000) ;
            print("\n");
            print("\n");
            while (1) {
                print("DID YOU LEAVE THE MONEY");
                gs = await input();
                print("\n");
                if (gs == "YES") {
                    print("HEY, " + ns + "??? YOU LEFT NO MONEY AT ALL!\n");
                    print("YOU ARE CHEATING ME OUT OF MY HARD-EARNED LIVING.\n");
                    print("\n");
                    print("WHAT A RIP OFF, " + ns + "!!!\n");
                    print("\n");
                    break;
                } else if (gs == "NO") {
                    print("THAT'S HONEST, " + ns + ", BUT HOW DO YOU EXPECT\n");
                    print("ME TO GO ON WITH MY PSYCHOLOGY STUDIES IF MY PATIENT\n");
                    print("DON'T PAY THEIR BILLS?\n");
                    break;
                } else {
                    print("YOUR ANSWER OF '" + gs + "' CONFUSES ME, " + ns + ".\n");
                    print("PLEASE RESPOND WITH 'YES' OR 'NO'.\n");
                }
            }
            break;
        }
    }
    print("\n");
    print("TAKE A WALK, " + ns + ".\n");
    print("\n");
    print("\n");
    // Line 390 not used in original
}

main();
