// CUBE
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
    print(tab(33) + "CUBE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("DO YOU WANT TO SEE THE INSTRUCTIONS? (YES--1,NO--0)");
    b7 = parseInt(await input());
    if (b7 != 0) {
        print("THIS IS A GAME IN WHICH YOU WILL BE PLAYING AGAINST THE\n");
        print("RANDOM DECISION OF THE COMPUTER. THE FIELD OF PLAY IS A\n");
        print("CUBE OF SIDE 3. ANY OF THE 27 LOCATIONS CAN BE DESIGNATED\n");
        print("BY INPUTING THREE NUMBERS SUCH AS 2,3,1. AT THE START,\n");
        print("YOU ARE AUTOMATICALLY AT LOCATION 1,1,1. THE OBJECT OF\n");
        print("THE GAME IS TO GET TO LOCATION 3,3,3. ONE MINOR DETAIL:\n");
        print("THE COMPUTER WILL PICK, AT RANDOM, 5 LOCATIONS AT WHICH\n");
        print("IT WILL PLANT LAND MINES. IF YOU HIT ONE OF THESE LOCATIONS\n");
        print("YOU LOSE. ONE OTHER DETAIL: YOU MAY MOVE ONLY ONE SPACE \n");
        print("IN ONE DIRECTION EACH MOVE. FOR  EXAMPLE: FROM 1,1,2 YOU\n");
        print("MAY MOVE TO 2,1,2 OR 1,1,3. YOU MAY NOT CHANGE\n");
        print("TWO OF THE NUMBERS ON THE SAME MOVE. IF YOU MAKE AN ILLEGAL\n");
        print("MOVE, YOU LOSE AND THE COMPUTER TAKES THE MONEY YOU MAY\n");
        print("HAVE BET ON THAT ROUND.\n");
        print("\n");
        print("\n");
        print("ALL YES OR NO QUESTIONS WILL BE ANSWERED BY A 1 FOR YES\n");
        print("OR A 0 (ZERO) FOR NO.\n");
        print("\n");
        print("WHEN STATING THE AMOUNT OF A WAGER, PRINT ONLY THE NUMBER\n");
        print("OF DOLLARS (EXAMPLE: 250)  YOU ARE AUTOMATICALLY STARTED WITH\n");
        print("500 DOLLARS IN YOUR ACCOUNT.\n");
        print("\n");
        print("GOOD LUCK!\n");
    }
    a1 = 500;
    while (1) {
        a = Math.floor(3 * Math.random());
        if (a == 0)
            a = 3;
        b = Math.floor(3 * Math.random());
        if (b == 0)
            b = 2;
        c = Math.floor(3 * Math.random());
        if (c == 0)
            c = 3;
        d = Math.floor(3 * Math.random());
        if (d == 0)
            d = 1;
        e = Math.floor(3 * Math.random());
        if (e == 0)
            e = 3;
        f = Math.floor(3 * Math.random());
        if (f == 0)
            f = 3;
        g = Math.floor(3 * Math.random());
        if (g == 0)
            g = 3;
        h = Math.floor(3 * Math.random());
        if (h == 0)
            h = 3;
        i = Math.floor(3 * Math.random());
        if (i == 0)
            i = 2;
        j = Math.floor(3 * Math.random());
        if (j == 0)
            j = 3;
        k = Math.floor(3 * Math.random());
        if (k == 0)
            k = 2;
        l = Math.floor(3 * Math.random());
        if (l == 0)
            l = 3;
        m = Math.floor(3 * Math.random());
        if (m == 0)
            m = 3;
        n = Math.floor(3 * Math.random());
        if (n == 0)
            n = 1;
        o = Math.floor(3 * Math.random());
        if (o == 0)
            o = 3;
        print("WANT TO MAKE A WAGER?");
        z = parseInt(await input());
        if (z != 0) {
            print("HOW MUCH ");
            while (1) {
                z1 = parseInt(await input());
                if (a1 < z1) {
                    print("TRIED TO FOOL ME; BET AGAIN");
                } else {
                    break;
                }
            }
        }
        w = 1;
        x = 1;
        y = 1;
        print("\n");
        print("IT'S YOUR MOVE:  ");
        while (1) {
            str = await input();
            p = parseInt(str);
            q = parseInt(str.substr(str.indexOf(",") + 1));
            r = parseInt(str.substr(str.lastIndexOf(",") + 1));
            if (p > w + 1 || q > x + 1 || r > y + 1 || (p == w + 1 && (q >= x + 1 || r >= y + 1)) || (q == x + 1 && r >= y + 1)) {
                print("\n");
                print("ILLEGAL MOVE, YOU LOSE.\n");
                break;
            }
            w = p;
            x = q;
            y = r;
            if (p == 3 && q == 3 && r == 3) {
                won = true;
                break;
            }
            if (p == a && q == b && r == c
             || p == d && q == e && r == f
             || p == g && q == h && r == i
             || p == j && q == k && r == l
             || p == m && q == n && r == o) {
                print("******BANG******");
                print("YOU LOSE!");
                print("\n");
                print("\n");
                won = false;
                break;
            }
            print("NEXT MOVE: ");
        }
        if (won) {
            print("CONGRATULATIONS!\n");
            if (z != 0) {
                z2 = a1 + z1;
                print("YOU NOW HAVE " + z2 + " DOLLARS.\n");
                a1 = z2;
            }
        } else {
            if (z != 0) {
                print("\n");
                z2 = a1 - z1;
                if (z2 <= 0) {
                    print("YOU BUST.\n");
                    break;
                } else {
                    print(" YOU NOW HAVE " + z2 + " DOLLARS.\n");
                    a1 = z2;
                }
            }
        }
        print("DO YOU WANT TO TRY AGAIN ");
        s = parseInt(await input());
        if (s != 1)
            break;
    }
    print("TOUGH LUCK!\n");
    print("\n");
    print("GOODBYE.\n");
}

main();
