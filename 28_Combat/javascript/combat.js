// COMBAT
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
    print(tab(33) + "COMBAT\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("I AM AT WAR WITH YOU.\n");
    print("WE HAVE 72000 SOLDIERS APIECE.\n");
    do {
        print("\n");
        print("DISTRIBUTE YOUR FORCES.\n");
        print("\tME\t  YOU\n");
        print("ARMY\t30000\t");
        a = parseInt(await input());
        print("NAVY\t20000\t");
        b = parseInt(await input());
        print("A. F.\t22000\t");
        c = parseInt(await input());
    } while (a + b + c > 72000) ;
    d = 30000;
    e = 20000;
    f = 22000;
    print("YOU ATTACK FIRST. TYPE (1) FOR ARMY; (2) FOR NAVY;\n");
    print("AND (3) FOR AIR FORCE.\n");
    y = parseInt(await input());
    do {
        print("HOW MANY MEN\n");
        x = parseInt(await input());
    } while ((y == 1 && x > a) || (y == 2 && x > b) || (y == 3 && x > c)) ;
    switch (y) {
        case 1:
            if (x < a / 3.0) {
                print("YOU LOST " + x + " MEN FROM YOUR ARMY.\n");
                a -= x;
                break;
            }
            if (x < 2 * a / 3) {
                print("YOU LOST " + Math.floor(x / 3.0) + " MEN, BUT I LOST " + Math.floor(2 * d / 3.0) + "\n");
                a = Math.floor(a - x / 3.0);
                d = 0;
                break;
            }
            print("YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO\n");
            print("OF YOUR AIR FORCE BASES AND 3 ARMY BASES.\n");
            a = Math.floor(a / 3.0);
            c = Math.floor(c / 3.0);
            e = Math.floor(2 * e / 3.0);
            break;
        case 2:
            if (x < e / 3) {
                print("YOUR ATTACK WAS STOPPED!\n");
                b -= x;
                break;
            }
            if (x < 2 * e / 3) {
                print("YOU DESTROYED " + Math.floor(2 * e / 3.0) + " OF MY ARMY.\n");
                e = Math.floor(e / 3.0);
                break;
            }
            print("YOU SUNK ONE OF MY PATROL BOATS, BUT I WIPED OUT TWO\n");
            print("OF YOUR AIR FORCE BASES AND 3 ARMY BASES.\n");
            a = Math.floor(a / 3.0);
            c = Math.floor(c / 3.0);
            e = Math.floor(2 * e / 3.0);
            break;
        case 3:
            if (x < c / 3.0) {
                print("YOUR ATTACK WAS WIPED OUT.\n");
                c -= x;
                break;
            }
            if (x < 2 * c / 3) {
                print("WE HAD A DOGFIGHT. YOU WON - AND FINISHED YOUR MISSION.\n");
                d = Math.floor(2 * d / 3.0);
                e = Math.floor(e / 3.0);
                f = Math.floor(f / 3.0);
                break;
            }
            print("YOU WIPED OUT ONE OF MY ARMY PATROLS, BUT I DESTROYED\n");
            print("TWO NAVY BASES AND BOMBED THREE ARMY BASES.\n");
            a = Math.floor(a / 4);
            b = Math.floor(b / 3.0);
            d = Math.floor(2 * d / 3.0);
            break;
    }
    print("\n");
    print("\tYOU\tME\n");
    print("ARMY\t" + a + "\t" + d + "\n");
    print("NAVY\t" + b + "\t" + e + "\n");
    print("A. F.\t" + c + "\t" + f + "\n");
    print("WHAT IS YOUR NEXT MOVE?\n");
    print("ARMY=1  NAVY=2  AIR FORCE=3\n");
    g = parseInt(await input());
    do {
        print("HOW MANY MEN\n");
        t = parseInt(await input());
    } while (t < 0 || (g == 1 && t > a) || (g == 2 && t > b) || (g == 3 && t > c)) ;
    crashed = false;
    switch (g) {
        case 1:
            if (t < d / 2) {
                print("I WIPED OUT YOUR ATTACK!\n");
                a -= t;
            } else {
                print("YOU DESTROYED MY ARMY!\n");
                d = 0;
            }
            break;
        case 2:
            if (t < e / 2) {
                print("I SUNK TWO OF YOUR BATTLESHIPS, AND MY AIR FORCE\n");
                print("WIPED OUT YOUR UNGUARDED CAPITOL.\n");
                a /= 4.0;
                b /= 2.0;
                break;
            }
            print("YOUR NAVY SHOT DOWN THREE OF MY XIII PLANES.\n");
            print("AND SUNK THREE BATTLESHIPS.\n");
            f = 2 * f / 3;
            e /= 2;
            break;
        case 3:
            if (t > f / 2) {
                print("MY NAVY AND AIR FORCE IN A COMBINED ATTACK LEFT\n");
                print("YOUR COUNTRY IN SHAMBLES.\n");
                a /= 3.0;
                b /= 3.0;
                c /= 3.0;
                break;
            }
            print("ONE OF YOUR PLANES CRASHED INTO MY HOUSE. I AM DEAD.\n");
            print("MY COUNTRY FELL APART.\n");
            crashed = true;
            won = 1;
            break;
    }
    if (!crashed) {
        won = 0;
        print("\n");
        print("FROM THE RESULTS OF BOTH OF YOUR ATTACKS,\n");
        if (a + b + c > 3.0 / 2.0 * (d + e + f))
            won = 1;
        if (a + b + c < 2.0 / 3.0 * (d + e + f))
            won = 2;
    }
    if (won == 0) {
        print("THE TREATY OF PARIS CONCLUDED THAT WE TAKE OUR\n");
        print("RESPECTIVE COUNTRIES AND LIVE IN PEACE.\n");
    } else if (won == 1) {
        print("YOU WON, OH! SHUCKS!!!!\n");
    } else if (won == 2) {
        print("YOU LOST-I CONQUERED YOUR COUNTRY.  IT SERVES YOU\n");
        print("RIGHT FOR PLAYING THIS STUPID GAME!!!\n");
    }
}

main();
