// DIGITS
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
    print(tab(33) + "DIGITS\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS IS A GAME OF GUESSING.\n");
    print("FOR INSTRUCTIONS, TYPE '1', ELSE TYPE '0'");
    e = parseInt(await input());
    if (e != 0) {
        print("\n");
        print("PLEASE TAKE A PIECE OF PAPER AND WRITE DOWN\n");
        print("THE DIGITS '0', '1', OR '2' THIRTY TIMES AT RANDOM.\n");
        print("ARRANGE THEM IN THREE LINES OF TEN DIGITS EACH.\n");
        print("I WILL ASK FOR THEN TEN AT A TIME.\n");
        print("I WILL ALWAYS GUESS THEM FIRST AND THEN LOOK AT YOUR\n");
        print("NEXT NUMBER TO SEE IF I WAS RIGHT. BY PURE LUCK,\n");
        print("I OUGHT TO BE RIGHT TEN TIMES. BUT I HOPE TO DO BETTER\n");
        print("THAN THAT *****\n");
        print("\n");
        print("\n");
    }
    a = 0;
    b = 1;
    c = 3;
    m = [];
    k = [];
    l = [];
    n = [];
    while (1) {
        for (i = 0; i <= 26; i++) {
            m[i] = [];
            for (j = 0; j <= 2; j++) {
                m[i][j] = 1;
            }
        }
        for (i = 0; i <= 2; i++) {
            k[i] = [];
            for (j = 0; j <= 2; j++) {
                k[i][j] = 9;
            }
        }
        for (i = 0; i <= 8; i++) {
            l[i] = [];
            for (j = 0; j <= 2; j++) {
                l[i][j] = 3;
            }
        }
        l[0][0] = 2;
        l[4][1] = 2;
        l[8][2] = 2;
        z = 26;
        z1 = 8;
        z2 = 2;
        x = 0;
        for (t = 1; t <= 3; t++) {
            while (1) {
                print("\n");
                print("TEN NUMBERS, PLEASE");
                str = await input();
                for (i = 1; i <= 10; i++) {
                    n[i] = parseInt(str);
                    j = str.indexOf(",");
                    if (j >= 0) {
                        str = str.substr(j + 1);
                    }
                    if (n[i] < 0 || n[i] > 2)
                        break;
                }
                if (i <= 10) {
                    print("ONLY USE THE DIGITS '0', '1', OR '2'.\n");
                    print("LET'S TRY AGAIN.\n");
                } else {
                    break;
                }
            }
            print("\n");
            print("MY GUESS\tYOUR NO.\tRESULT\tNO. RIGHT\n");
            print("\n");
            for (u = 1; u <= 10; u++) {
                n2 = n[u];
                s = 0;
                for (j = 0; j <= 2; j++) {
                    s1 = a * k[z2][j] + b * l[z1][j] + c * m[z][j];
                    if (s > s1)
                        continue;
                    if (s < s1 || Math.random() >= 0.5) {
                        s = s1;
                        g = j;
                    }
                }
                print("  " + g + "\t\t   " + n[u] + "\t\t");
                if (g == n[u]) {
                    x++;
                    print(" RIGHT\t " + x + "\n");
                    m[z][n2]++;
                    l[z1][n2]++;
                    k[z2][n2]++;
                    z = z % 9;
                    z = 3 * z + n[u];
                } else {
                    print(" WRONG\t " + x + "\n");
                }
                z1 = z % 9;
                z2 = n[u];
            }
        }
        print("\n");
        if (x > 10) {
            print("I GUESSED MORE THAN 1/3 OF YOUR NUMBERS.\n");
            print("I WIN.\n");
        } else if (x == 10) {
            print("I GUESSED EXACTLY 1/3 OF YOUR NUMBERS.\n");
            print("IT'S A TIE GAME.\n");
        } else {
            print("I GUESSED LESS THAN 1/3 OF YOUR NUMBERS.\n");
            print("YOU BEAT ME.  CONGRATULATIONS *****\n");
        }
        print("\n");
        print("DO YOU WANT TO TRY AGAIN (1 FOR YES, 0 FOR NO)");
        x = parseInt(await input());
        if (x != 1)
            break;
    }
    print("\n");
    print("THANKS FOR THE GAME.\n");
}

main();
