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
    print(tab(34) + "BOWL\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    c = [];
    a = [];
    for (i = 0; i <= 15; i++)
        c[i] = 0;
    print("WELCOME TO THE ALLEY\n");
    print("BRING YOUR FRIENDS\n");
    print("OKAY LET'S FIRST GET ACQUAINTED\n");
    print("\n");
    print("THE INSTRUCTIONS (Y/N)\n");
    str = await input();
    if (str.substr(0, 1) == "Y") {
        print("THE GAME OF BOWLING TAKES MIND AND SKILL.DURING THE GAME\n");
        print("THE COMPUTER WILL KEEP SCORE.YOU MAY COMPETE WITH\n");
        print("OTHER PLAYERS[UP TO FOUR].YOU WILL BE PLAYING TEN FRAMES\n");
        print("ON THE PIN DIAGRAM 'O' MEANS THE PIN IS DOWN...'+' MEANS THE\n");
        print("PIN IS STANDING.AFTER THE GAME THE COMPUTER WILL SHOW YOUR\n");
        print("SCORES .\n");
    }
    print("FIRST OF ALL...HOW MANY ARE PLAYING");
    r = parseInt(await input());
    while (1) {
        print("\n");
        print("VERY GOOD...\n");
        for (i = 1; i <= 100; i++) {
            a[i] = [];
            for (j = 1; j <= 6; j++)
                a[i][j] = 0;
        }
        f = 1;
        do {
            for (p = 1; p <= r; p++) {
                // m = 0; // Repeated in original
                b = 1;
                m = 0;
                q = 0;
                for (i = 1; i <= 15; i++)
                    c[i] = 0;
                while (1) {
                    // Ball generator using mod '15' system
                    print("TYPE ROLL TO GET THE BALL GOING.\n");
                    ns = await input();
                    k = 0;
                    d = 0;
                    for (i = 1; i <= 20; i++) {
                        x = Math.floor(Math.random() * 100);
                        for (j = 1; j <= 10; j++)
                            if (x < 15 * j)
                                break;
                        c[15 * j - x] = 1;
                    }
                    // Pin diagram
                    print("PLAYER: " + p + " FRAME: " + f + " BALL: " + b + "\n");
                    print("\n");
                    for (i = 0; i <= 3; i++) {
                        str = "";
                        for (j = 1; j <= 4 - i; j++) {
                            k++;
                            while (str.length < i)
                                str += " ";
                            if (c[k] == 1)
                                str += "O ";
                            else
                                str += "+ ";
                        }
                        print(str + "\n");
                    }
                    // Roll analysis
                    for (i = 1; i <= 10; i++)
                        d += c[i];
                    if (d - m == 0)
                        print("GUTTER!!\n");
                    if (b == 1 && d == 10) {
                        print("STRIKE!!!!!\n");
                        q = 3;
                    }
                    if (b == 2 && d == 10) {
                        print("SPARE!!!!\n");
                        q = 2;
                    }
                    if (b == 2 && d < 10) {
                        print("ERROR!!!\n");
                        q = 1;
                    }
                    if (b == 1 && d < 10) {
                        print("ROLL YOUR 2ND BALL\n");
                    }
                    // Storage of the scores
                    print("\n");
                    a[f * p][b] = d;
                    if (b != 2) {
                        b = 2;
                        m = d;
                        if (q == 3) {
                            a[f * p][b] = d;
                        } else {
                            a[f * p][b] = d - m;
                            if (q == 0) // ROLL
                                continue;
                        }
                    }
                    break;
                }
                a[f * p][3] = q;
            }
        } while (++f < 11) ;
        print("FRAMES\n");
        for (i = 1; i <= 10; i++)
            print(" " + i + " ");
        print("\n");
        for (p = 1; p <= r; p++) {
            for (i = 1; i <= 3; i++) {
                for (j = 1; j <= 10; j++) {
                    print(" " + a[j * p][i] + " ");
                }
                print("\n");
            }
            print("\n");
        }
        print("DO YOU WANT ANOTHER GAME");
        str = await input();
        if (str.substr(0, 1) != "Y")
            break;
        // Bug in original game, jumps to 2610, without restarting P variable
    }
}

main();
