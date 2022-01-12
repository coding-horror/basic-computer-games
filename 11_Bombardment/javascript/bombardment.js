// BOMBARDMENT
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
    print(tab(33) + "BOMBARDMENT\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("YOU ARE ON A BATTLEFIELD WITH 4 PLATOONS AND YOU\n");
    print("HAVE 25 OUTPOSTS AVAILABLE WHERE THEY MAY BE PLACED.\n");
    print("YOU CAN ONLY PLACE ONE PLATOON AT ANY ONE OUTPOST.\n");
    print("THE COMPUTER DOES THE SAME WITH ITS FOUR PLATOONS.\n");
    print("\n");
    print("THE OBJECT OF THE GAME IS TO FIRE MISSILES AT THE\n");
    print("OUTPOSTS OF THE COMPUTER.  IT WILL DO THE SAME TO YOU.\n");
    print("THE ONE WHO DESTROYS ALL FOUR OF THE ENEMY'S PLATOONS\n");
    print("FIRST IS THE WINNER.\n");
    print("\n");
    print("GOOD LUCK... AND TELL US WHERE YOU WANT THE BODIES SENT!\n");
    print("\n");
    // "TEAR OFF" because it supposed this to be printed on a teletype
    print("TEAR OFF MATRIX AND USE IT TO CHECK OFF THE NUMBERS.\n");
    for (r = 1; r <= 5; r++)
        print("\n");
    ma = [];
    for (r = 1; r <= 100; r++)
        ma[r] = 0;
    p = 0;
    q = 0;
    z = 0;
    for (r = 1; r <= 5; r++) {
        i = (r - 1) * 5 + 1;
        print(i + "\t" + (i + 1) + "\t" + (i + 2) + "\t" + (i + 3) + "\t" + (i + 4) + "\n");
    }
    for (r = 1; r <= 10; r++)
        print("\n");
    c = Math.floor(Math.random() * 25) + 1;
    do {
        d = Math.floor(Math.random() * 25) + 1;
        e = Math.floor(Math.random() * 25) + 1;
        f = Math.floor(Math.random() * 25) + 1;
    } while (c == d || c == e || c == f || d == e || d == f || e == f) ;
    print("WHAT ARE YOUR FOUR POSITIONS");
    str = await input();
    g = parseInt(str);
    str = str.substr(str.indexOf(",") + 1);
    h = parseInt(str);
    str = str.substr(str.indexOf(",") + 1);
    k = parseInt(str);
    str = str.substr(str.indexOf(",") + 1);
    l = parseInt(str);
    print("\n");
    // Another "bug" your outpost can be in the same position as a computer outpost
    // Let us suppose both live in a different matrix.
    while (1) {
        // The original game didn't limited the input to 1-25
        do {
            print("WHERE DO YOU WISH TO FIRE YOUR MISSLE");
            y = parseInt(await input());
        } while (y < 0 || y > 25) ;
        if (y == c || y == d || y == e || y == f) {
            
            // The original game has a bug. You can shoot the same outpost
            // several times. This solves it.
            if (y == c)
                c = 0;
            if (y == d)
                d = 0;
            if (y == e)
                e = 0;
            if (y == f)
                f = 0;
            q++;
            if (q == 1) {
                print("ONE DOWN. THREE TO GO.\n");
            } else if (q == 2) {
                print("TWO DOWN. TWO TO GO.\n");
            } else if (q == 3) {
                print("THREE DOWN. ONE TO GO.\n");
            } else {
                print("YOU GOT ME, I'M GOING FAST. BUT I'LL GET YOU WHEN\n");
                print("MY TRANSISTO&S RECUP%RA*E!\n");
                break;
            }
        } else {
            print("HA, HA YOU MISSED. MY TURN NOW:\n");
        }
        print("\n");
        print("\n");
        do {
            m = Math.floor(Math.random() * 25 + 1);
            p++;
            n = p - 1;
            for (t = 1; t <= n; t++) {
                if (m == ma[t])
                    break;
            }
        } while (t <= n) ;
        x = m;
        ma[p] = m;
        if (x == g || x == h || x == l || x == k) {
            z++;
            if (z < 4)
                print("I GOT YOU. IT WON'T BE LONG NOW. POST " + x + " WAS HIT.\n");
            if (z == 1) {
                print("YOU HAVE ONLY THREE OUTPOSTS LEFT.\n");
            } else if (z == 2) {
                print("YOU HAVE ONLY TWO OUTPOSTS LEFT.\n");
            } else if (z == 3) {
                print("YOU HAVE ONLY ONE OUTPOST LEFT.\n");
            } else {
                print("YOU'RE DEAD. YOUR LAST OUTPOST WAS AT " + x + ". HA, HA, HA.\n");
                print("BETTER LUCK NEXT TIME.\n");
            }
        } else {
            print("I MISSED YOU, YOU DIRTY RAT. I PICKED " + m + ". YOUR TURN:\n");
        }
        print("\n");
        print("\n");
    }
}

main();
