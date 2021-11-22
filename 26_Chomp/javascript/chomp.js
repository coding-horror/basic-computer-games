// CHOMP
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
var r;
var c;

function init_board()
{
    for (i = 1; i <= r; i++)
        for (j = 1; j <= c; j++)
            a[i][j] = 1;
    a[1][1] = -1;
}

function show_board()
{
    print("\n");
    print(tab(7) + "1 2 3 4 5 6 7 8 9\n");
    for (i = 1; i <= r; i++) {
        str = i + tab(6);
        for (j = 1; j <= c; j++) {
            if (a[i][j] == -1)
                str += "P ";
            else if (a[i][j] == 0)
                break;
            else
                str += "* ";
        }
        print(str + "\n");
    }
    print("\n");
}

// Main program
async function main()
{
    print(tab(33) + "CHOMP\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    for (i = 1; i <= 10; i++)
        a[i] = [];
    // *** THE GAME OF CHOMP *** COPYRIGHT PCC 1973 ***
    print("\n");
    print("THIS IS THE GAME OF CHOMP (SCIENTIFIC AMERICAN, JAN 1973)\n");
    print("DO YOU WANT THE RULES (1=YES, 0=NO!)");
    r = parseInt(await input());
    if (r != 0) {
        f = 1;
        r = 5;
        c = 7;
        print("CHOMP IS FOR 1 OR MORE PLAYERS (HUMANS ONLY).\n");
        print("\n");
        print("HERE'S HOW A BOARD LOOKS (THIS ONE IS 5 BY 7):\n");
        init_board();
        show_board();
        print("\n");
        print("THE BOARD IS A BIG COOKIE - R ROWS HIGH AND C COLUMNS\n");
        print("WIDE. YOU INPUT R AND C AT THE START. IN THE UPPER LEFT\n");
        print("CORNER OF THE COOKIE IS A POISON SQUARE (P). THE ONE WHO\n");
        print("CHOMPS THE POISON SQUARE LOSES. TO TAKE A CHOMP, TYPE THE\n");
        print("ROW AND COLUMN OF ONE OF THE SQUARES ON THE COOKIE.\n");
        print("ALL OF THE SQUARES BELOW AND TO THE RIGHT OF THAT SQUARE\n");
        print("INCLUDING THAT SQUARE, TOO) DISAPPEAR -- CHOMP!!\n");
        print("NO FAIR CHOMPING SQUARES THAT HAVE ALREADY BEEN CHOMPED,\n");
        print("OR THAT ARE OUTSIDE THE ORIGINAL DIMENSIONS OF THE COOKIE.\n");
        print("\n");
    }
    while (1) {
        print("HERE WE GO...\n");
        f = 0;
        for (i = 1; i <= 10; i++) {
            a[i] = [];
            for (j = 1; j <= 10; j++) {
                a[i][j] = 0;
            }
        }
        print("\n");
        print("HOW MANY PLAYERS");
        p = parseInt(await input());
        i1 = 0;
        while (1) {
            print("HOW MANY ROWS");
            r = parseInt(await input());
            if (r <= 9)
                break;
            print("TOO MANY ROWS (9 IS MAXIMUM). NOW ");
        }
        while (1) {
            print("HOW MANY COLUMNS");
            c = parseInt(await input());
            if (c <= 9)
                break;
            print("TOO MANY COLUMNS (9 IS MAXIMUM). NOW ");
        }
        print("\n");
        init_board();
        while (1) {
            // Print the board
            show_board();
            // Get chomps for each player in turn
            i1++;
            p1 = i1 - Math.floor(i1 / p) * p;
            if (p1 == 0)
                p1 = p;
            while (1) {
                print("PLAYER " + p1 + "\n");
                print("COORDINATES OF CHOMP (ROW,COLUMN)");
                str = await input();
                r1 = parseInt(str);
                c1 = parseInt(str.substr(str.indexOf(",") + 1));
                if (r1 >= 1 && r1 <= r && c1 >= 1 && c1 <= c && a[r1][c1] != 0)
                    break;
                print("NO FAIR. YOU'RE TRYING TO CHOMP ON EMPTY SPACE!\n");
            }
            if (a[r1][c1] == -1)
                break;
            for (i = r1; i <= r; i++)
                for (j = c1; j <= c; j++)
                    a[i][j] = 0;
        }
        // End of game detected
        print("YOU LOSE, PLAYER " + p1 + "\n");
        print("\n");
        print("AGAIN (1=YES, 0=NO!)");
        r = parseInt(await input());
        if (r != 1)
            break;
    }
}

main();
