// SPLAT
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

var aa = [];

// Main program
async function main()
{
    print(tab(33) + "SPLAT\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    for (i = 0; i <= 42; i++)
        aa[i] = 0;
    print("WELCOME TO 'SPLAT' -- THE GAME THAT SIMULATES A PARACHUTE\n");
    print("JUMP.  TRY TO OPEN YOUR CHUTE AT THE LAST POSSIBLE\n");
    print("MOMENT WITHOUT GOING SPLAT.\n");
    while (1) {
        print("\n");
        print("\n");
        d1 = 0;
        v = 0;
        a = 0;
        n = 0;
        m = 0;
        d1 = Math.floor(9001 * Math.random() + 1000);
        print("SELECT YOUR OWN TERMINAL VELOCITY (YES OR NO)");
        while (1) {
            a1s = await input();
            if (a1s == "YES" || a1s == "NO")
                break;
            print("YES OR NO");
        }
        if (a1s == "YES") {
            print("WHAT TERMINAL VELOCITY (MI/HR)");
            v1 = parseFloat(await input());
            v1 = v1 * (5280 / 3600);
        } else {
            v1 = Math.floor(1000 * Math.random());
            print("OK.  TERMINAL VELOCITY = " + v1 + " MI/HR\n");
        }
        v = v1 + ((v1 * Math.random()) / 20) - ((v1 * Math.random()) / 20);
        print("WANT TO SELECT ACCELERATION DUE TO GRAVITY (YES OR NO)");
        while (1) {
            b1s = await input();
            if (b1s == "YES" || b1s == "NO")
                break;
            print("YES OR NO");
        }
        if (b1s == "YES") {
            print("WHAT ACCELERATION (FT/SEC/SEC)");
            a2 = parseFloat(await input());
        } else {
            switch (Math.floor(1 + (10 * Math.random()))) {
                case 1:
                    print("FINE. YOU'RE ON MERCURY. ACCELERATION=12.2 FT/SEC/SEC.\n");
                    a2 = 12.2;
                    break;
                case 2:
                    print("ALL RIGHT. YOU'RE ON VENUS. ACCELERATION=28.3 FT/SEC/SEC.\n");
                    a2 = 28.3;
                    break;
                case 3:
                    print("THEN YOU'RE ON EARTH. ACCELERATION=32.16 FT/SEC/SEC.\n");
                    a2 = 32.16;
                    break;
                case 4:
                    print("FINE. YOU'RE ON THE MOON. ACCELERATION=5.15 FT/SEC/SEC.\n");
                    a2 = 5.15;
                    break;
                case 5:
                    print("ALL RIGHT. YOU'RE ON MARS. ACCELERATION=12.5 FT/SEC/SEC.\n");
                    a2 = 12.5;
                    break;
                case 6:
                    print("THEN YOU'RE ON JUPITER. ACCELERATION=85.2 FT/SEC/SEC.\n");
                    a2 = 85.2;
                    break;
                case 7:
                    print("FINE. YOU'RE ON SATURN. ACCELERATION=37.6 FT/SEC/SEC.\n");
                    a2 = 37.6;
                    break;
                case 8:
                    print("ALL RIGHT. YOU'RE ON URANUS. ACCELERATION=33.8 FT/SEC/SEC.\n");
                    a2 = 33.8;
                    break;
                case 9:
                    print("THEN YOU'RE ON NEPTUNE. ACCELERATION=39.6 FT/SEC/SEC.\n");
                    a2 = 39.6;
                    break;
                case 10:
                    print("FINE. YOU'RE ON THE SUN. ACCELERATION=896 FT/SEC/SEC.\n");
                    a2 = 896;
                    break;
            }
        }
        a = a2 + ((a2 * Math.random()) / 20) - ((a2 * Math.random()) / 20);
        print("\n");
        print("    ALTITUDE         = " + d1 + " FT\n");
        print("    TERM. VELOCITY   = " + v1 + " FT/SEC +/-5%\n");
        print("    ACCELERATION     = " + a2 + " FT/SEC/SEC +/-5%\n");
        print("SET THE TIMER FOR YOUR FREEFALL.\n");
        print("HOW MANY SECONDS");
        t = parseFloat(await input());
        print("HERE WE GO.\n");
        print("\n");
        print("TIME (SEC)\tDIST TO FALL (FT)\n");
        print("==========\t=================\n");
        terminal = false;
        crash = false;
        for (i = 0; i <= t; i += t / 8) {
            if (i > v / a) {
                terminal = true;
                break;
            }
            d = d1 - ((a / 2) * Math.pow(i, 2));
            if (d <= 0) {
                print(Math.sqrt(2 * d1 / a) + "\tSPLAT\n");
                crash = true;
                break;
            }
            print(i + "\t" + d + "\n");
        }
        if (terminal) {
            print("TERMINAL VELOCITY REACHED AT T PLUS " + v/a + " SECONDS.\n");
            for (; i <= t; i += t / 8) {
                d = d1 - ((Math.pow(v, 2) / (2 * a)) + (v * (i - (v / a))));
                if (d <= 0) {
                    print(((v / a) + ((d1 - (Math.pow(v, 2) / (2 * a))) / v)) + "\tSPLAT\n");
                    crash = true;
                    break;
                }
                print(i + "\t" + d + "\n");
            }
        }
        if (!crash) {
            print("CHUTE OPEN\n");
            k = 0;
            k1 = 0;
            for (j = 0; j <= 42; j++) {
                if (aa[j] == 0)
                    break;
                k++;
                if (d < aa[j])
                    k1++;
            }
            // In original jumps to line 540 (undefined) when table is full
            aa[j] = d;
            if (j <= 2) {
                print("AMAZING!!! NOT BAD FOR YOUR ");
                if (j == 0)
                    print("1ST ");
                else if (j == 1)
                    print("2ND ");
                else
                    print("3RD ");
                print("SUCCESSFUL JUMP!!!\n");
            } else {
                if (k - k1 <= 0.1 * k) {
                    print("WOW!  THAT'S SOME JUMPING.  OF THE " + k + " SUCCESSFUL JUMPS\n");
                    print("BEFORE YOURS, ONLY " + (k - k1) + " OPENED THEIR CHUTES LOWER THAN\n");
                    print("YOU DID.\n");
                } else if (k - k1 <= 0.25 * k) {
                    print("PRETTY GOOD! " + k + " SUCCESSFUL JUMPS PRECEDED YOURS AND ONLY\n");
                    print((k - k1) + " OF THEM GOT LOWER THAN YOU DID BEFORE THEIR CHUTES\n");
                    print("OPENED.\n");
                } else if (k - k1 <= 0.5 * k) {
                    print("NOT BAD.  THERE HAVE BEEN " + k + " SUCCESSFUL JUMPS BEFORE YOURS.\n");
                    print("YOU WERE BEATEN OUT BY " + (k - k1) + " OF THEM.\n");
                } else if (k - k1 <= 0.75 * k) {
                    print("CONSERVATIVE, AREN'T YOU?  YOU RANKED ONLY " + (k - k1) + " IN THE\n");
                    print(k + " SUCCESSFUL JUMPS BEFORE YOURS.\n");
                } else if (k - k1 <= 0.9 * k) {
                    print("HUMPH!  DON'T YOU HAVE ANY SPORTING BLOOD?  THERE WERE\n");
                    print(k + " SUCCESSFUL JUMPS BEFORE YOURS AND YOU CAME IN " + k1 + "JUMPS\n");
                    print("BETTER THAN THE WORST.  SHAPE UP!!!\n");
                } else {
                    print("HEY!  YOU PULLED THE RIP CORD MUCH TOO SOON.  " + k + " SUCCESSFUL\n");
                    print("JUMPS BEFORE YOURS AND YOU CAME IN NUMBER " + (k - k1) + "!  GET WITH IT!\n");
                }
            }
        } else {
            switch (Math.floor(1 + 10 * Math.random())) {
                case 1:
                    print("REQUIESCAT IN PACE.\n");
                    break;
                case 2:
                    print("MAY THE ANGEL OF HEAVEN LEAD YOU INTO PARADISE.\n");
                    break;
                case 3:
                    print("REST IN PEACE.\n");
                    break;
                case 4:
                    print("SON-OF-A-GUN.\n");
                    break;
                case 5:
                    print("#%&&%!$\n");
                    break;
                case 6:
                    print("A KICK IN THE PANTS IS A BOOST IF YOU'RE HEADED RIGHT.\n");
                    break;
                case 7:
                    print("HMMM. SHOULD HAVE PICKED A SHORTER TIME.\n");
                    break;
                case 8:
                    print("MUTTER. MUTTER. MUTTER.\n");
                    break;
                case 9:
                    print("PUSHING UP DAISIES.\n");
                    break;
                case 10:
                    print("EASY COME, EASY GO.\n");
                    break;
            }
            print("I'LL GIVE YOU ANOTHER CHANCE.\n");
        }
        while (1) {
            print("DO YOU WANT TO PLAY AGAIN");
            str = await input();
            if (str == "YES" || str == "NO")
                break;
            print("YES OR NO\n");
        }
        if (str == "YES")
            continue;
        print("PLEASE");
        while (1) {
            str = await input();
            if (str == "YES" || str == "NO")
                break;
            print("YES OR NO");
        }
        if (str == "YES")
            continue;
        break;
    }
    print("SSSSSSSSSS.\n");
    print("\n");
}

main();
