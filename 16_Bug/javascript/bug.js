// BUG
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

function draw_head()
{
    print("        HHHHHHH\n");
    print("        H     H\n");
    print("        H O O H\n");
    print("        H     H\n");
    print("        H  V  H\n");
    print("        HHHHHHH\n");
}

// Main program
async function main()
{
    print(tab(34) + "BUG\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    a = 0;
    b = 0;
    h = 0;
    l = 0;
    n = 0;
    p = 0;
    q = 0;
    r = 0;
    s = 0;
    t = 0;
    u = 0;
    v = 0;
    y = 0;
    print("THE GAME BUG\n");
    print("I HOPE YOU ENJOY THIS GAME.\n");
    print("\n");
    print("DO YOU WANT INSTRUCTIONS");
    str = await input();
    if (str != "NO") {
        print("THE OBJECT OF BUG IS TO FINISH YOUR BUG BEFORE I FINISH\n");
        print("MINE. EACH NUMBER STANDS FOR A PART OF THE BUG BODY.\n");
        print("I WILL ROLL THE DIE FOR YOU, TELL YOU WHAT I ROLLED FOR YOU\n");
        print("WHAT THE NUMBER STANDS FOR, AND IF YOU CAN GET THE PART.\n");
        print("IF YOU CAN GET THE PART I WILL GIVE IT TO YOU.\n");
        print("THE SAME WILL HAPPEN ON MY TURN.\n");
        print("IF THERE IS A CHANGE IN EITHER BUG I WILL GIVE YOU THE\n");
        print("OPTION OF SEEING THE PICTURES OF THE BUGS.\n");
        print("THE NUMBERS STAND FOR PARTS AS FOLLOWS:\n");
        print("NUMBER\tPART\tNUMBER OF PART NEEDED\n");
        print("1\tBODY\t1\n");
        print("2\tNECK\t1\n");
        print("3\tHEAD\t1\n");
        print("4\tFEELERS\t2\n");
        print("5\tTAIL\t1\n");
        print("6\tLEGS\t6\n");
        print("\n");
        print("\n");
    }
    while (y == 0) {
        z = Math.floor(6 * Math.random() + 1);
        c = 1;
        print("YOU ROLLED A " + z + "\n");
        switch (z) {
            case 1:
                print("1=BODY\n");
                if (b == 0) {
                    print("YOU NOW HAVE A BODY.\n");
                    b = 1;
                    c = 0;
                } else {
                    print("YOU DO NOT NEED A BODY.\n");
                }
                break;
            case 2:
                print("2=NECK\n");
                if (n == 0) {
                    if (b == 0) {
                        print("YOU DO NOT HAVE A BODY.\n");
                    } else {
                        print("YOU NOW HAVE A NECK.\n");
                        n = 1;
                        c = 0;
                    }
                } else {
                    print("YOU DO NOT NEED A NECK.\n");
                }
                break;
            case 3:
                print("3=HEAD\n");
                if (n == 0) {
                    print("YOU DO NOT HAVE A NECK.\n");
                } else if (h == 0) {
                    print("YOU NEEDED A HEAD.\n");
                    h = 1;
                    c = 0;
                } else {
                    print("YOU HAVE A HEAD.\n");
                }
                break;
            case 4:
                print("4=FEELERS\n");
                if (h == 0) {
                    print("YOU DO NOT HAVE A HEAD.\n");
                } else if (a == 2) {
                    print("YOU HAVE TWO FEELERS ALREADY.\n");
                } else {
                    print("I NOW GIVE YOU A FEELER.\n");
                    a++;
                    c = 0;
                }
                break;
            case 5:
                print("5=TAIL\n");
                if (b == 0) {
                    print("YOU DO NOT HAVE A BODY.\n");
                } else if (t == 1) {
                    print("YOU ALREADY HAVE A TAIL.\n");
                } else {
                    print("I NOW GIVE YOU A TAIL.\n");
                    t++;
                    c = 0;
                }
                break;
            case 6:
                print("6=LEG\n");
                if (l == 6) {
                    print("YOU HAVE 6 FEET ALREADY.\n");
                } else if (b == 0) {
                    print("YOU DO NOT HAVE A BODY.\n");
                } else {
                    l++;
                    c = 0;
                    print("YOU NOW HAVE " + l + " LEGS.\n");
                }
                break;
        }
        x = Math.floor(6 * Math.random() + 1) ;
        print("\n");
        date = new Date().valueOf;
        while (date - new Date().valueOf < 1000000) ;
        print("I ROLLED A " + x + "\n");
        switch (x) {
            case 1:
                print("1=BODY\n");
                if (p == 1) {
                    print("I DO NOT NEED A BODY.\n");
                } else {
                    print("I NOW HAVE A BODY.\n");
                    c = 0;
                    p = 1;
                }
                break;
            case 2:
                print("2=NECK\n");
                if (q == 1) {
                    print("I DO NOT NEED A NECK.\n");
                } else if (p == 0) {
                    print("I DO NOT HAVE A BODY.\n");
                } else {
                    print("I NOW HAVE A NECK.\n");
                    q = 1;
                    c = 0;
                }
                break;
            case 3:
                print("3=HEAD\n");
                if (q == 0) {
                    print("I DO NOT HAVE A NECK.\n");
                } else if (r == 1) {
                    print("I DO NOT NEED A HEAD.\n");
                } else {
                    print("I NEEDED A HEAD.\n");
                    r = 1;
                    c = 0;
                }
                break;
            case 4:
                print("4=FEELERS\n");
                if (r == 0) {
                    print("I DO NOT HAVE A HEAD.\n");
                } else if (s == 2) {
                    print("I HAVE 2 FEELERS ALREADY.\n");
                } else {
                    print("I GET A FEELER.\n");
                    s++;
                    c = 0;
                }
                break;
            case 5:
                print("5=TAIL\n");
                if (p == 0) {
                    print("I DO NOT HAVE A BODY.\n");
                } else if (u == 1) {
                    print("I DO NOT NEED A TAIL.\n");
                } else {
                    print("I NOW HAVE A TAIL.\n");
                    u = 1;
                    c = 0;
                }
                break;
            case 6:
                print("6=LEGS\n");
                if (v == 6) {
                    print("I HAVE 6 FEET.\n");
                } else if (p == 0) {
                    print("I DO NOT HAVE A BODY.\n");
                } else {
                    v++;
                    c = 0;
                    print("I NOW HAVE " + v + " LEGS.\n");
                }
                break;
        }
        if (a == 2 && t == 1 && l == 6) {
            print("YOUR BUG IS FINISHED.\n");
            y++;
        }
        if (s == 2 && p == 1 && v == 6) {
            print("MY BUG IS FINISHED.\n");
            y += 2;
        }
        if (c == 1)
            continue;
        print("DO YOU WANT THE PICTURES");
        str = await input();
        if (str == "NO")
            continue;
        print("*****YOUR BUG*****\n");
        print("\n");
        print("\n");
        if (a != 0) {
            for (z = 1; z <= 4; z++) {
                print(tab(10));
                for (x = 1; x <= a; x++) {
                    print("A ");
                }
                print("\n");
            }
        }
        if (h != 0)
            draw_head();
        if (n != 0) {
            for (z = 1; z <= 2; z++)
                print("          N N\n");
        }
        if (b != 0) {
            print("     BBBBBBBBBBBB\n");
            for (z = 1; z <= 2; z++)
                print("     B          B\n");
            if (t == 1)
                print("TTTTTB          B\n");
            print("     BBBBBBBBBBBB\n");
        }
        if (l != 0) {
            for (z = 1; z <= 2; z++) {
                print(tab(5));
                for (x = 1; x <= l; x++)
                    print(" L");
                print("\n");
            }
        }
        for (z = 1; z <= 4; z++)
            print("\n");
        print("*****MY BUG*****\n");
        print("\n");
        print("\n");
        print("\n");
        if (s != 0) {
            for (z = 1; z <= 4; z++) {
                print(tab(10));
                for (x = 1; x <= s; x++) {
                    print("F ");
                }
                print("\n");
            }
        }
        if (r != 0)
            draw_head();
        if (q != 0) {
            for (z = 1; z <= 2; z++)
                print("          N N\n");
        }
        if (p != 0) {
            print("     BBBBBBBBBBBB\n");
            for (z = 1; z <= 2; z++)
                print("     B          B\n");
            if (u == 1)
                print("TTTTTB          B\n");
            print("     BBBBBBBBBBBB\n");
        }
        if (v != 0) {
            for (z = 1; z <= 2; z++) {
                print(tab(5));
                for (x = 1; x <= v; x++)
                    print(" L");
                print("\n");
            }
        }
        for (z = 1; z <= 4; z++)
            print("\n");
    }
    print("I HOPE YOU ENJOYED THE GAME, PLAY IT AGAIN SOON!!\n");
}

main();
