// GAME OF EVEN WINS
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

var r = [[], []];

// Main program
async function main()
{
    print(tab(28) + "GAME OF EVEN WINS\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("DO YOU WANT INSTRUCTIONS (YES OR NO)");
    str = await input();
    print("\n");
    if (str != "NO") {
        print("THE GAME IS PLAYED AS FOLLOWS:\n");
        print("\n");
        print("AT THE BEGINNING OF THE GAME, A RANDOM NUMBER OF CHIPS ARE\n");
        print("PLACED ON THE BOARD.  THE NUMBER OF CHIPS ALWAYS STARTS\n");
        print("AS AN ODD NUMBER.  ON EACH TURN, A PLAYER MUST TAKE ONE,\n");
        print("TWO, THREE, OR FOUR CHIPS.  THE WINNER IS THE PLAYER WHO\n");
        print("FINISHES WITH A TOTAL NUMBER OF CHIPS THAT IS EVEN.\n");
        print("THE COMPUTER STARTS OUT KNOWING ONLY THE RULES OF THE\n");
        print("GAME.  IT GRADUALLY LEARNS TO PLAY WELL.  IT SHOULD BE\n");
        print("DIFFICULT TO BEAT THE COMPUTER AFTER TWENTY GAMES IN A ROW.\n");
        print("TRY IT!!!!\n");
        print("\n");
        print("TO QUIT AT ANY TIME, TYPE A '0' AS YOUR MOVE.\n");
        print("\n");
    }
    l = 0;
    b = 0;
    for (i = 0; i <= 5; i++) {
        r[1][i] = 4;
        r[0][i] = 4;
    }
    while (1) {
        a = 0;
        b = 0;
        e = 0;
        l = 0;
        p = Math.floor((13 * Math.random() + 9) / 2) * 2 + 1;
        while (1) {
            if (p == 1) {
                print("THERE IS 1 CHIP ON THE BOARD.\n");
            } else {
                print("THERE ARE " + p + " CHIPS ON THE BOARD.\n");
            }
            e1 = e;
            l1 = l;
            e = a % 2;
            l = p % 6;
            if (r[e][l] < p) {
                m = r[e][l];
                if (m <= 0) {
                    m = 1;
                    b = 1;
                    break;
                }
                p -= m;
                if (m == 1)
                    print("COMPUTER TAKES 1 CHIP LEAVING " + p + "... YOUR MOVE");
                else
                    print("COMPUTER TAKES " + m + " CHIPS LEAVING " + p + "... YOUR MOVE");
                b += m;
                while (1) {
                    m = parseInt(await input());
                    if (m == 0)
                        break;
                    if (m < 1 || m > 4 || m > p) {
                        print(m + " IS AN ILLEGAL MOVE ... YOUR MOVE");
                    } else {
                        break;
                    }
                }
                if (m == 0)
                    break;
                if (m == p)
                    break;
                p -= m;
                a += m;
            } else {
                if (p == 1) {
                    print("COMPUTER TAKES 1 CHIP.\n");
                } else {
                    print("COMPUTER TAKES " + p + " CHIPS.\n");
                }
                r[e][l] = p;
                b += p;
                break;
            }
        }
        if (m == 0)
            break;
        if (b % 2 != 0) {
            print("GAME OVER ... YOU WIN!!!\n");
            print("\n");
            if (r[e][l] != 1) {
                r[e][l]--;
            } else if (r[e1][l1] != 1) {
                r[e1][l1]--;
            }
        } else {
            print("GAME OVER ... I WIN!!!\n");
            print("\n");
        }
    }
}

main();
