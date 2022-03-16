// ONE CHECK
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

var a = [];

// Main program
async function main()
{
    print(tab(30) + "ONE CHECK\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    for (i = 0; i <= 64; i++)
        a[i] = 0;
    print("SOLITAIRE CHECKER PUZZLE BY DAVID AHL\n");
    print("\n");
    print("48 CHECKERS ARE PLACED ON THE 2 OUTSIDE SPACES OF A\n");
    print("STANDARD 64-SQUARE CHECKERBOARD.  THE OBJECT IS TO\n");
    print("REMOVE AS MANY CHECKERS AS POSSIBLE BY DIAGONAL JUMPS\n");
    print("(AS IN STANDARD CHECKERS).  USE THE NUMBERED BOARD TO\n");
    print("INDICATE THE SQUARE YOU WISH TO JUMP FROM AND TO.  ON\n");
    print("THE BOARD PRINTED OUT ON EACH TURN '1' INDICATES A\n");
    print("CHECKER AND '0' AN EMPTY SQUARE.  WHEN YOU HAVE NO\n");
    print("POSSIBLE JUMPS REMAINING, INPUT A '0' IN RESPONSE TO\n");
    print("QUESTION 'JUMP FROM ?'\n");
    print("\n");
    print("HERE IS THE NUMERICAL BOARD:\n");
    print("\n");
    while (1) {
        for (j = 1; j <= 57; j += 8) {
            str = "";
            for (i = 0; i <= 7; i++) {
                while (str.length < 4 * i)
                    str += " ";
                str += " " + (j + i);
            }
            print(str + "\n");
        }
        print("\n");
        print("AND HERE IS THE OPENING POSITION OF THE CHECKERS.\n");
        print("\n");
        for (j = 1; j <= 64; j++)
            a[j] = 1;
        for (j = 19; j <= 43; j += 8)
            for (i = j; i <= j + 3; i++)
                a[i] = 0;
        m = 0;
        while (1) {
            // Print board
            for (j = 1; j <= 57; j += 8) {
                str = "";
                for (i = j; i <= j + 7; i++) {
                    str += " " + a[i] + " ";
                }
                print(str + "\n");
            }
            print("\n");
            while (1) {
                print("JUMP FROM");
                f = parseInt(await input());
                if (f == 0)
                    break;
                print("TO");
                t = parseInt(await input());
                print("\n");
                // Check legality of move
                f1 = Math.floor((f - 1) / 8);
                f2 = f - 8 * f1;
                t1 = Math.floor((t - 1) / 8);
                t2 = t - 8 * t1;
                if (f1 > 7 || t1 > 7 || f2 > 8 || t2 > 8 || Math.abs(f1 - t1) != 2 || Math.abs(f2 - t2) != 2 || a[(t + f) / 2] == 0 || a[f] == 0 || a[t] == 1) {
                    print("ILLEGAL MOVE.  TRY AGAIN...\n");
                    continue;
                }
                break;
            }
            if (f == 0)
                break;
            // Update board
            a[t] = 1;
            a[f] = 0;
            a[(t + f) / 2] = 0;
            m++;
        }
        // End game summary
        s = 0;
        for (i = 1; i <= 64; i++)
            s += a[i];
        print("\n");
        print("YOU MADE " + m + " JUMPS AND HAD " + s + " PIECES\n");
        print("REMAINING ON THE BOARD.\n");
        print("\n");
        while (1) {
            print("TRY AGAIN");
            str = await input();
            if (str == "YES")
                break;
            if (str == "NO")
                break;
            print("PLEASE ANSWER 'YES' OR 'NO'.\n");
        }
        if (str == "NO")
            break;
    }
    print("\n");
    print("O.K.  HOPE YOU HAD FUN!!\n");
}

main();
