// HANGMAN
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

print(tab(32) + "HANGMAN\n");
print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
print("\n");
print("\n");
print("\n");

var pa = [];
var la = [];
var da = [];
var na = [];
var ua = [];

var words = ["GUM","SIN","FOR","CRY","LUG","BYE","FLY",
             "UGLY","EACH","FROM","WORK","TALK","WITH","SELF",
             "PIZZA","THING","FEIGN","FIEND","ELBOW","FAULT","DIRTY",
             "BUDGET","SPIRIT","QUAINT","MAIDEN","ESCORT","PICKAX",
             "EXAMPLE","TENSION","QUININE","KIDNEY","REPLICA","SLEEPER",
             "TRIANGLE","KANGAROO","MAHOGANY","SERGEANT","SEQUENCE",
             "MOUSTACHE","DANGEROUS","SCIENTIST","DIFFERENT","QUIESCENT",
             "MAGISTRATE","ERRONEOUSLY","LOUDSPEAKER","PHYTOTOXIC",
             "MATRIMONIAL","PARASYMPATHOMIMETIC","THIGMOTROPISM"];

// Main control section
async function main()
{
    c = 1;
    n = 50;
    while (1) {
        for (i = 1; i <= 20; i++)
            da[i] = "-";
        for (i = 1; i <= n; i++)
            ua[i] = 0;
        m = 0;
        ns = "";
        for (i = 1; i <= 12; i++) {
            pa[i] = [];
            for (j = 1; j <= 12; j++) {
                pa[i][j] = " ";
            }
        }
        for (i = 1; i <= 12; i++) {
            pa[i][1] = "X";
        }
        for (i = 1; i <= 7; i++) {
            pa[1][i] = "X";
        }
        pa[2][7] = "X";
        if (c >= n) {
            print("YOU DID ALL THE WORDS!!\n");
            break;
        }
        do {
            q = Math.floor(n * Math.random()) + 1;
        } while (ua[q] == 1) ;
        ua[q] = 1;
        c++;
        t1 = 0;
        as = words[q - 1];
        l = as.length;
        for (i = 1; i <= as.length; i++)
            la[i] = as[i - 1];
        while (1) {
            while (1) {
                print("HERE ARE THE LETTERS YOU USED:\n");
                print(ns + "\n");
                print("\n");
                for (i = 1; i <= l; i++) {
                    print(da[i]);
                }
                print("\n");
                print("\n");
                print("WHAT IS YOUR GUESS");
                str = await input();
                if (ns.indexOf(str) != -1) {
                    print("YOU GUESSED THAT LETTER BEFORE!\n");
                } else {
                    break;
                }
            }
            ns += str;
            t1++;
            r = 0;
            for (i = 1; i <= l; i++) {
                if (la[i] == str) {
                    da[i] = str;
                    r++;
                }
            }
            if (r == 0) {
                m++;
                print("\n");
                print("\n");
                print("SORRY, THAT LETTER ISN'T IN THE WORD.\n");
                switch (m) {
                    case 1:
                        print("FIRST, WE DRAW A HEAD\n");
                        break;
                    case 2:
                        print("NOW WE DRAW A BODY.\n");
                        break;
                    case 3:
                        print("NEXT WE DRAW AN ARM.\n");
                        break;
                    case 4:
                        print("THIS TIME IT'S THE OTHER ARM.\n");
                        break;
                    case 5:
                        print("NOW, LET'S DRAW THE RIGHT LEG.\n");
                        break;
                    case 6:
                        print("THIS TIME WE DRAW THE LEFT LEG.\n");
                        break;
                    case 7:
                        print("NOW WE PUT UP A HAND.\n");
                        break;
                    case 8:
                        print("NEXT THE OTHER HAND.\n");
                        break;
                    case 9:
                        print("NOW WE DRAW ONE FOOT.\n");
                        break;
                    case 10:
                        print("HERE'S THE OTHER FOOT -- YOU'RE HUNG!!\n");
                        break;
                }
                switch (m) {
                    case 1:
                        pa[3][6] = "-";
                        pa[3][7] = "-";
                        pa[3][8] = "-";
                        pa[4][5] = "(";
                        pa[4][6] = ".";
                        pa[4][8] = ".";
                        pa[4][9] = ")";
                        pa[5][6] = "-";
                        pa[5][7] = "-";
                        pa[5][8] = "-";
                        break;
                    case 2:
                        for (i = 6; i <= 9; i++)
                            pa[i][7] = "X";
                        break;
                    case 3:
                        for (i = 4; i <= 7; i++)
                            pa[i][i - 1] = "\\";
                        break;
                    case 4:
                        pa[4][11] = "/";
                        pa[5][10] = "/";
                        pa[6][9] = "/";
                        pa[7][8] = "/";
                        break;
                    case 5:
                        pa[10][6] = "/";
                        pa[11][5] = "/";
                        break;
                    case 6:
                        pa[10][8] = "\\";
                        pa[11][9] = "\\";
                        break;
                    case 7:
                        pa[3][11] = "\\";
                        break;
                    case 8:
                        pa[3][3] = "/";
                        break;
                    case 9:
                        pa[12][10] = "\\";
                        pa[12][11] = "-";
                        break;
                    case 10:
                        pa[12][3] = "-";
                        pa[12][4] = "/";
                        break;
                }
                for (i = 1; i <= 12; i++) {
                    str = "";
                    for (j = 1; j <= 12; j++)
                        str += pa[i][j];
                    print(str + "\n");
                }
                print("\n");
                print("\n");
                if (m == 10) {
                    print("SORRY, YOU LOSE.  THE WORD WAS " + as + "\n");
                    print("YOU MISSED THAT ONE.  DO YOU ");
                    break;
                }
            } else {
                for (i = 1; i <= l; i++)
                    if (da[i] == "-")
                        break;
                if (i > l) {
                    print("YOU FOUND THE WORD!\n");
                    break;
                }
                print("\n");
                for (i = 1; i <= l; i++)
                    print(da[i]);
                print("\n");
                print("\n");
                print("WHAT IS YOUR GUESS FOR THE WORD");
                bs = await input();
                if (as == bs) {
                    print("RIGHT!!  IT TOOK YOU " + t1 + " GUESSES!\n");
                    break;
                }
                print("WRONG.  TRY ANOTHER LETTER.\n");
                print("\n");
            }
        }
        print("WANT ANOTHER WORD");
        str = await input();
        if (str != "YES")
            break;
    }
    print("\n");
    print("IT'S BEEN FUN!  BYE FOR NOW.\n");
    // Lines 620 and 990 unused in original
}

main();
