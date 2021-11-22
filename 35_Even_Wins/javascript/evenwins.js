// EVEN WINS
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

var ma = [];
var ya = [];

// Main program
async function main()
{
    print(tab(31) + "EVEN WINS\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    y1 = 0;
    m1 = 0;
    print("     THIS IS A TWO PERSON GAME CALLED 'EVEN WINS.'\n");
    print("TO PLAY THE GAME, THE PLAYERS NEED 27 MARBLES OR\n");
    print("OTHER OBJECTS ON A TABLE.\n");
    print("\n");
    print("\n");
    print("     THE 2 PLAYERS ALTERNATE TURNS, WITH EACH PLAYER\n");
    print("REMOVING FROM 1 TO 4 MARBLES ON EACH MOVE.  THE GAME\n");
    print("ENDS WHEN THERE ARE NO MARBLES LEFT, AND THE WINNER\n");
    print("IS THE ONE WITH AN EVEN NUMBER OF MARBLES.\n");
    print("\n");
    print("\n");
    print("     THE ONLY RULES ARE THAT (1) YOU MUST ALTERNATE TURNS,\n");
    print("(2) YOU MUST TAKE BETWEEN 1 AND 4 MARBLES EACH TURN,\n");
    print("AND (3) YOU CANNOT SKIP A TURN.\n");
    print("\n");
    print("\n");
    print("\n");
    while (1) {
        print("     TYPE A '1' IF YOU WANT TO GO FIRST, AND TYPE\n");
        print("A '0' IF YOU WANT ME TO GO FIRST.\n");
        c = parseInt(await input());
        print("\n");
        if (c != 0) {
            t = 27;
            print("\n");
            print("\n");
            print("\n");
            print("TOTAL= " + t + "\n");
            print("\n");
            print("\n");
            print("WHAT IS YOUR FIRST MOVE");
            m = 0;
        } else {
            t = 27;
            m = 2;
            print("\n");
            print("TOTAL= " + t + "\n");
            print("\n");
            m1 += m;
            t -= m;
        }
        while (1) {
            if (m) {
                print("I PICK UP " + m + " MARBLES.\n");
                if (t == 0)
                    break;
                print("\n");
                print("TOTAL= " + t + "\n");
                print("\n");
                print("     AND WHAT IS YOUR NEXT MOVE, MY TOTAL IS " + m1 + "\n");
            }
            while (1) {
                y = parseInt(await input());
                print("\n");
                if (y < 1 || y > 4) {
                    print("\n");
                    print("THE NUMBER OF MARBLES YOU MUST TAKE BE A POSITIVE\n");
                    print("INTEGER BETWEEN 1 AND 4.\n");
                    print("\n");
                    print("     WHAT IS YOUR NEXT MOVE?\n");
                    print("\n");
                } else if (y > t) {
                    print("     YOU HAVE TRIED TO TAKE MORE MARBLES THAN THERE ARE\n");
                    print("LEFT.  TRY AGAIN.\n");
                } else {
                    break;
                }
            }
            
            y1 += y;
            t -= y;
            if (t == 0)
                break;
            print("TOTAL= " + t + "\n");
            print("\n");
            print("YOUR TOTAL IS " + y1 + "\n");
            if (t < 0.5)
                break;
            r = t % 6;
            if (y1 % 2 != 0) {
                if (t >= 4.2) {
                    if (r <= 3.4) {
                        m = r + 1;
                        m1 += m;
                        t -= m;
                    } else if (r < 4.7 || r > 3.5) {
                        m = 4;
                        m1 += m;
                        t -= m;
                    } else {
                        m = 1;
                        m1 += m;
                        t -= m;
                    }
                } else {
                    m = t;
                    t -= m;
                    print("I PICK UP " + m + " MARBLES.\n");
                    print("\n");
                    print("TOTAL = 0\n");
                    m1 += m;
                    break;
                }
            } else {
                if (r < 1.5 || r > 5.3) {
                    m = 1;
                    m1 += m;
                    t -= m;
                } else {
                    m = r - 1;
                    m1 += m;
                    t -= m;
                    if (t < 0.2) {
                        print("I PICK UP " + m + " MARBLES.\n");
                        print("\n");
                        break;
                    }
                }
            }
        }
        print("THAT IS ALL OF THE MARBLES.\n");
        print("\n");
        print(" MY TOTAL IS " + m1 + ", YOUR TOTAL IS " + y1 +"\n");
        print("\n");
        if (m1 % 2 != 0) {
            print("     YOU WON.  DO YOU WANT TO PLAY\n");
        } else {
            print("     I WON.  DO YOU WANT TO PLAY\n");
        }
        print("AGAIN?  TYPE 1 FOR YES AND 0 FOR NO.\n");
        a1 = parseInt(await input());
        if (a1 == 0)
            break;
        m1 = 0;
        y1 = 0;
    }
    print("\n");
    print("OK.  SEE YOU LATER\n");
}

main();
