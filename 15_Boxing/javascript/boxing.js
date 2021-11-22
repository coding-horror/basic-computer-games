// BOWLING
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

// Main program
async function main()
{
    print(tab(33) + "BOXING\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("BOXING OLYMPIC STYLE (3 ROUNDS -- 2 OUT OF 3 WINS)\n");
    j = 0;
    l = 0;
    print("\n");
    print("WHAT IS YOUR OPPONENT'S NAME");
    js = await input();
    print("INPUT YOUR MAN'S NAME");
    ls = await input();
    print("DIFFERENT PUNCHES ARE: (1) FULL SWING; (2) HOOK; (3) UPPERCUT; (4) JAB.\n");
    print("WHAT IS YOUR MANS BEST");
    b = parseInt(await input());
    print("WHAT IS HIS VULNERABILITY");
    d = parseInt(await input());
    do {
        b1 = Math.floor(4 * Math.random() + 1);
        d1 = Math.floor(4 * Math.random() + 1);
    } while (b1 == d1) ;
    print(js + "'S ADVANTAGE IS " + b1 + " AND VULNERABILITY IS SECRET.\n");
    print("\n");
    knocked = 0;
    for (r = 1; r <= 3; r++) {
        if (j >= 2)
            break;
        if (l >= 2)
            break;
        x = 0;
        y = 0;
        print("ROUND " + r + " BEGIN...\n");
        for (r1 = 1; r1 <= 7; r1++) {
            i = Math.floor(10 * Math.random() + 1);
            if (i <= 5) {
                print(ls + "'S PUNCH");
                p = parseInt(await input());
                if (p == b)
                    x += 2;
                if (p == 1) {
                    print(ls + " SWINGS AND ");
                    x3 = Math.floor(30 * Math.random() + 1);
                    if (d1 == 4 || x3 < 10) {
                        print("HE CONNECTS!\n");
                        if (x > 35) {
                            r = 3;
                            break;
                        }
                        x += 15;
                    } else {
                        print("HE MISSES \n");
                        if (x != 1)
                            print("\n\n");
                    }
                } else if (p == 2) {
                    print(ls + " GIVES THE HOOK... ");
                    h1 = Math.floor(2 * Math.random() + 1);
                    if (d1 == 2) {
                        x += 7;
                    } else if (h1 != 1) {
                        print("CONNECTS...\n");
                        x += 7;
                    } else {
                        print("BUT IT'S BLOCKED!!!!!!!!!!!!!\n");
                    }
                } else if (p == 3) {
                    print(ls + " TRIES AN UPPERCUT ");
                    d5 = Math.floor(100 * Math.random() + 1);
                    if (d1 == 3 || d5 < 51) {
                        print("AND HE CONNECTS!\n");
                        x += 4;
                    } else {
                        print("AND IT'S BLOCKED (LUCKY BLOCK!)\n");
                    }
                } else {
                    print(ls + " JABS AT " + js + "'S HEAD ");
                    c = Math.floor(8 * Math.random() + 1);
                    if (d1 == 4 || c >= 4) {
                        x += 3;
                    } else {
                        print("IT'S BLOCKED.\n");
                    }
                }
            } else {
                j7 = Math.random(4 * Math.random() + 1);
                if (j7 == b1)
                    y += 2;
                if (j7 == 1) {
                    print(js + " TAKES A FULL SWING AND");
                    r6 = Math.floor(60 * Math.random() + 1);
                    if (d == 1 || r6 < 30) {
                        print(" POW!!!!! HE HITS HIM RIGHT IN THE FACE!\n");
                        if (y > 35) {
                            knocked = 1;
                            r = 3;
                            break;
                        }
                        y += 15;
                    } else {
                        print(" IT'S BLOCKED!\n");
                    }
                } else if (j7 == 2 || j7 == 3) {
                    if (j7 == 2) {
                        print(js + " GETS " + ls + " IN THE JAW (OUCH!)\n");
                        y += 7;
                        print("....AND AGAIN!\n");
                        y += 5;
                        if (y > 35) {
                            knocked = 1;
                            r = 3;
                            break;
                        }
                        print("\n");
                        // From original, it goes over from handling 2 to handling 3
                    }
                    print(ls + " IS ATTACKED BY AN UPPERCUT (OH,OH)...\n");
                    q4 = Math.floor(200 * Math.random() + 1);
                    if (d == 3 || q4 <= 75) {
                        print("AND " + js + " CONNECTS...\n");
                        y += 8;
                    } else {
                        print(" BLOCKS AND HITS " + js + " WITH A HOOK.\n");
                        x += 5;
                    }
                } else {
                    print(js + " JABS AND ");
                    z4 = Math.floor(7 * Math.random() + 1);
                    if (d == 4)
                        y += 5;
                    else if (z4 > 4) {
                        print(" BLOOD SPILLS !!!\n");
                        y += 5;
                    } else {
                        print("IT'S BLOCKED!\n");
                    }
                }
            }
        }
        if (x > y) {
            print("\n");
            print(ls + " WINS ROUND " + r + "\n");
            l++;
        } else {
            print("\n");
            print(js + " WINS ROUND " + r + "\n");
            j++;
        }
    }
    if (j >= 2) {
        print(js + " WINS (NICE GOING, " + js + ").\n");
    } else if (l >= 2) {
        print(ls + " AMAZINGLY WINS!!\n");
    } else if (knocked) {
        print(ls + " IS KNOCKED COLD AND " + js + " IS THE WINNER AND CHAMP!\n");
    } else {
        print(js + " IS KNOCKED COLD AND " + ls + " IS THE WINNER AND CHAMP!\n");
    }
    print("\n");
    print("\n");
    print("AND NOW GOODBYE FROM THE OLYMPIC ARENA.\n");
    print("\n");
}

main();
