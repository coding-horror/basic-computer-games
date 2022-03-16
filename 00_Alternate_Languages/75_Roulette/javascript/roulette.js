// ROULETTE
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

var ba = [];
var ca = [];
var ta = [];
var xa = [];
var aa = [];

var numbers = [1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36];

// Main program
async function main()
{
    print(tab(32) + "ROULETTE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    // Roulette
    // David Joslin
    print("WELCOME TO THE ROULETTE TABLE\n");
    print("\n");
    print("DO YOU WANT INSTRUCTIONS");
    str = await input();
    if (str.substr(0, 1) != "N") {
        print("\n");
        print("THIS IS THE BETTING LAYOUT\n");
        print("  (*=RED)\n");
        print("\n");
        print(" 1*    2     3*\n");
        print(" 4     5*    6 \n");
        print(" 7*    8     9*\n");
        print("10    11    12*\n");
        print("---------------\n");
        print("13    14*   15 \n");
        print("16*   17    18*\n");
        print("19*   20    21*\n");
        print("22    23*   24 \n");
        print("---------------\n");
        print("25*   26    27*\n");
        print("28    29    30*\n");
        print("31    32*   33 \n");
        print("34*   35    36*\n");
        print("---------------\n");
        print("    00    0    \n");
        print("\n");
        print("TYPES OF BETS\n");
        print("\n");
        print("THE NUMBERS 1 TO 36 SIGNIFY A STRAIGHT BET\n");
        print("ON THAT NUMBER.\n");
        print("THESE PAY OFF 35:1\n");
        print("\n");
        print("THE 2:1 BETS ARE:\n");
        print(" 37) 1-12     40) FIRST COLUMN\n");
        print(" 38) 13-24    41) SECOND COLUMN\n");
        print(" 39) 25-36    42) THIRD COLUMN\n");
        print("\n");
        print("THE EVEN MONEY BETS ARE:\n");
        print(" 43) 1-18     46) ODD\n");
        print(" 44) 19-36    47) RED\n");
        print(" 45) EVEN     48) BLACK\n");
        print("\n");
        print(" 49)0 AND 50)00 PAY OFF 35:1\n");
        print(" NOTE: 0 AND 00 DO NOT COUNT UNDER ANY\n");
        print("       BETS EXCEPT THEIR OWN.\n");
        print("\n");
        print("WHEN I ASK FOR EACH BET, TYPE THE NUMBER\n");
        print("AND THE AMOUNT, SEPARATED BY A COMMA.\n");
        print("FOR EXAMPLE: TO BET $500 ON BLACK, TYPE 48,500\n");
        print("WHEN I ASK FOR A BET.\n");
        print("\n");
        print("THE MINIMUM BET IS $5, THE MAXIMUM IS $500.\n");
        print("\n");
    }
    // Program begins here
    // Type of bet(number) odds
    for (i = 1; i <= 100; i++) {
        ba[i] = 0;
        ca[i] = 0;
        ta[i] = 0;
    }
    for (i = 1; i <= 38; i++)
        xa[i] = 0;
    p = 1000;
    d = 100000;
    while (1) {
        do {
            print("HOW MANY BETS");
            y = parseInt(await input());
        } while (y < 1) ;
        for (i = 1; i <= 50; i++) {
            aa[i] = 0;
        }
        for (c = 1; c <= y; c++) {
            while (1) {
                print("NUMBER " + c + " ");
                str = await input();
                x = parseInt(str);
                z = parseInt(str.substr(str.indexOf(",") + 1));
                ba[c] = z;
                ta[c] = x;
                if (x < 1 || x > 50)
                    continue;
                if (z < 1)
                    continue;
                if (z < 5 || z > 500)
                    continue;
                if (aa[x] != 0) {
                    print("YOU MADE THAT BET ONCE ALREADY,DUM-DUM\n");
                    continue;
                }
                aa[x] = 1;
                break;
            }
        }
        print("SPINNING\n");
        print("\n");
        print("\n");
        do {
            s = Math.floor(Math.random() * 100);
        } while (s == 0 || s > 38) ;
        xa[s]++;    // Not used
        if (s > 37) {
            print("00\n");
        } else if (s == 37) {
            print("0\n");
        } else {
            for (i1 = 1; i1 <= 18; i1++) {
                if (s == numbers[i1 - 1])
                    break;
            }
            if (i1 <= 18)
                print(s + " RED\n");
            else
                print(s + " BLACK\n");
        }
        print("\n");
        for (c = 1; c <= y; c++) {
            won = 0;
            switch (ta[c]) {
                case 37:    // 1-12 (37) 2:1
                    if (s > 12) {
                        won = -ba[c];
                    } else {
                        won = ba[c] * 2;
                    }
                    break;
                case 38:    // 13-24 (38) 2:1
                    if (s > 12 && s < 25) {
                        won = ba[c] * 2;
                    } else {
                        won = -ba[c];
                    }
                    break;
                case 39:    // 25-36 (39) 2:1
                    if (s > 24 && s < 37) {
                        won = ba[c] * 2;
                    } else {
                        won = -ba[c];
                    }
                    break;
                case 40:    // First column (40) 2:1
                    if (s < 37 && s % 3 == 1) {
                        won = ba[c] * 2;
                    } else {
                        won = -ba[c];
                    }
                    break;
                case 41:    // Second column (41) 2:1
                    if (s < 37 && s % 3 == 2) {
                        won = ba[c] * 2;
                    } else {
                        won = -ba[c];
                    }
                    break;
                case 42:    // Third column (42) 2:1
                    if (s < 37 && s % 3 == 0) {
                        won = ba[c] * 2;
                    } else {
                        won = -ba[c];
                    }
                    break;
                case 43:    // 1-18 (43) 1:1
                    if (s < 19) {
                        won = ba[c];
                    } else {
                        won = -ba[c];
                    }
                    break;
                case 44:    // 19-36 (44) 1:1
                    if (s > 18 && s < 37) {
                        won = ba[c];
                    } else {
                        won = -ba[c];
                    }
                    break;
                case 45:    // Even (45) 1:1
                    if (s < 37 && s % 2 == 0) {
                        won = ba[c];
                    } else {
                        won = -ba[c];
                    }
                    break;
                case 46:    // Odd (46) 1:1
                    if (s < 37 && s % 2 != 0) {
                        won = ba[c];
                    } else {
                        won = -ba[c];
                    }
                    break;
                case 47:    // Red (47) 1:1
                    for (i = 1; i <= 18; i++) {
                        if (s == numbers[i - 1])
                            break;
                    }
                    if (i <= 18) {
                        won = ba[c];
                    } else {
                        won = -ba[c];
                    }
                    break;
                case 48:    // Black (48) 1:1
                    for (i = 1; i <= 18; i++) {
                        if (s == numbers[i - 1])
                            break;
                    }
                    if (i <= 18 || s > 36) {
                        won = -ba[c];
                    } else {
                        won = ba[c];
                    }
                    break;
                default:    // 1-36,0,00 (1-36,49,50) 35:1
                    if (ta[c] < 49 && ta[c] == s
                        || ta[c] == 49 && s == 37
                        || ta[c] == 50 && s == 38) {
                        won = ba[c] * 35;
                    } else {
                        won = -ba[c];
                    }
                    break;
            }
            d -= won;
            p += won;
            if (won < 0) {
                print("YOU LOSE " + -won + " DOLLARS ON BET " + c + "\n");
            } else {
                print("YOU WIN " + won + " DOLLARS ON BET " + c + "\n");
            }
        }
        print("\n");
        print("TOTALS:\tME\tYOU\n");
        print(" \t" + d + "\t" + p + "\n");
        if (p <= 0) {
            print("OOPS! YOU JUST SPENT YOUR LAST DOLLAR!\n");
            break;
        } else if (d <= 0) {
            print("YOU BROKE THE HOUSE!\n");
            p = 101000;
        }
        print("AGAIN");
        str = await input();
        if (str.substr(0, 1) != "Y")
            break;
    }
    if (p < 1) {
        print("THANKS FOR YOUR MONEY.\n");
        print("I'LL USE IT TO BUY A SOLID GOLD ROULETTE WHEEL\n");
    } else {
        print("TO WHOM SHALL I MAKE THE CHECK");
        str = await input();
        print("\n");
        for (i = 1; i <= 72; i++)
            print("-");
        print("\n");
        print(tab(50) + "CHECK NO. " + Math.floor(Math.random() * 100) + "\n");
        print("\n");
        print(tab(40) + new Date().toDateString());
        print("\n");
        print("\n");
        print("PAY TO THE ORDER OF-----" + str + "-----$ " + p + "\n");
        print("\n");
        print("\n");
        print(tab(10) + "\tTHE MEMORY BANK OF NEW YORK\n");
        print("\n");
        print(tab(40) + "\tTHE COMPUTER\n");
        print(tab(40) + "----------X-----\n");
        print("\n");
        for (i = 1; i <= 72; i++)
            print("-");
        print("\n");
        print("COME BACK SOON!\n");
    }
    print("\n");
}

main();
