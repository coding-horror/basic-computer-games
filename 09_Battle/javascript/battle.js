// BATTLE
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

var fa = [];
var ha = [];
var aa = [];
var ba = [];
var ca = [];
var la = [];

// Main program
async function main()
{
    print(tab(33) + "BATTLE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    // -- BATTLE WRITTEN BY RAY WESTERGARD  10/70
    // COPYRIGHT 1971 BY THE REGENTS OF THE UNIV. OF CALIF.
    // PRODUCED AT THE LAWRENCE HALL OF SCIENCE, BERKELEY
    while (1) {
        for (x = 1; x <= 6; x++) {
            fa[x] = [];
            ha[x] = [];
            for (y = 1; y <= 6; y++) {
                fa[x][y] = 0;
                ha[x][y] = 0;
            }
        }
        for (i = 1; i <= 3; i++) {
            n = 4 - i;
            for (j = 1; j <= 2; j++) {
                while (1) {
                    a = Math.floor(6 * Math.random() + 1);
                    b = Math.floor(6 * Math.random() + 1);
                    d = Math.floor(4 * Math.random() + 1);
                    if (fa[a][b] > 0)
                        continue;
                    m = 0;
                    switch (d) {
                        case 1:
                            ba[1] = b;
                            ba[2] = 7;
                            ba[3] = 7;
                            for (k = 1; k <= n; k++) {
                                if (m <= 1 && ba[k] != 6 && fa[a][ba[k] + 1] <= 0) {
                                    ba[k + 1] = ba[k] + 1;
                                } else {
                                    m = 2;
                                    if (ba[1] < ba[2] && ba[1] < ba[3])
                                        z = ba[1];
                                    if (ba[2] < ba[1] && ba[2] < ba[3])
                                        z = ba[2];
                                    if (ba[3] < ba[1] && ba[3] < ba[2])
                                        z = ba[3];
                                    if (z == 1)
                                        break;
                                    if (fa[a][z - 1] > 0)
                                        break;
                                    ba[k + 1] = z - 1;
                                }
                            }
                            if (k <= n)
                                continue;
                            fa[a][b] = 9 - 2 * i - j;
                            for (k = 1; k <= n; k++)
                                fa[a][ba[k + 1]] = fa[a][b];
                            break;
                        case 2:
                            aa[1] = a;
                            ba[1] = b;
                            aa[2] = 0;
                            aa[3] = 0;
                            ba[2] = 0;
                            ba[3] = 0;
                            for (k = 1; k <= n; k++) {
                                if (m <= 1 && aa[k] != 1 && ba[k] != 1 && fa[aa[k] - 1][ba[k] - 1] <= 0 && (fa[aa[k] - 1][ba[k]] <= 0 || fa[aa[k] - 1][ba[k]] != fa[aa[k]][ba[k] - 1])) {
                                    aa[k + 1] = aa[k] - 1;
                                    ba[k + 1] = ba[k] - 1;
                                } else {
                                    m = 2;
                                    if (aa[1] > aa[2] && aa[1] > aa[3])
                                        z1 = aa[1];
                                    if (aa[2] > aa[1] && aa[2] > aa[3])
                                        z1 = aa[2];
                                    if (aa[3] > aa[1] && aa[3] > aa[2])
                                        z1 = aa[3];
                                    if (ba[1] > ba[2] && ba[1] > ba[3])
                                        z2 = ba[1];
                                    if (ba[2] > ba[1] && ba[2] > ba[3])
                                        z2 = ba[2];
                                    if (ba[3] > ba[1] && ba[3] > ba[2])
                                        z2 = ba[3];
                                    if (z1 == 6 || z2 == 6)
                                        break;
                                    if (fa[z1 + 1][z2 + 1] > 0)
                                        break;
                                    if (fa[z1][z2 + 1] > 0 && fa[z1][z2 + 1] == fa[z1 + 1][z2])
                                        break;
                                    aa[k + 1] = z1 + 1;
                                    ba[k + 1] = z2 + 1;
                                }
                            }
                            if (k <= n)
                                continue;
                            fa[a][b] = 9 - 2 * i - j;
                            for (k = 1; k <= n; k++)
                                fa[aa[k + 1]][ba[k + 1]] = fa[a][b];
                            break;
                        case 3:
                            aa[1] = a;
                            aa[2] = 7;
                            aa[3] = 7;
                            for (k = 1; k <= n; k++) {
                                if (m <= 1 && aa[k] != 6 && fa[aa[k] + 1][b] <= 0) {
                                    aa[k + 1] = aa[k] + 1;
                                } else {
                                    m = 2;
                                    if (aa[1] < aa[2] && aa[1] < aa[3])
                                        z = aa[1];
                                    if (aa[2] < aa[1] && aa[2] < aa[3])
                                        z = aa[2];
                                    if (aa[3] < aa[1] && aa[3] < aa[2])
                                        z = aa[3];
                                    if (z == 1)
                                        break;
                                    if (fa[z - 1][b] > 0)
                                        break;
                                    aa[k + 1] = z - 1;
                                }
                            }
                            if (k <= n)
                                continue;
                            fa[a][b] = 9 - 2 * i - j;
                            for (k = 1; k <= n; k++)
                                fa[aa[k + 1]][b] = fa[a][b];
                            break;
                        case 4:
                            aa[1] = a;
                            ba[1] = b;
                            aa[2] = 7;
                            aa[3] = 7;
                            ba[2] = 0;
                            ba[3] = 0;
                            for (k = 1; k <= n; k++) {
                                if (m <= 1 && aa[k] != 6 && ba[k] != 1 && fa[aa[k] + 1][ba[k] - 1] <= 0 && (fa[aa[k] + 1][ba[k]] <= 0 || fa[aa[k] + 1][ba[k]] != fa[aa[k]][ba[k] - 1])) {
                                    aa[k + 1] = aa[k] + 1;
                                    ba[k + 1] = ba[k] - 1;
                                } else {
                                    m = 2;
                                    if (aa[1] < aa[2] && aa[1] < aa[3])
                                        z1 = aa[1];
                                    if (aa[2] < aa[1] && aa[2] < aa[3])
                                        z1 = aa[2];
                                    if (aa[3] < aa[1] && aa[3] < aa[2])
                                        z1 = aa[3];
                                    if (ba[1] > ba[2] && ba[1] > ba[3])
                                        z2 = ba[1];
                                    if (ba[2] > ba[1] && ba[2] > ba[3])
                                        z2 = ba[2];
                                    if (ba[3] > ba[1] && ba[3] > ba[2])
                                        z2 = ba[3];
                                    if (z1 == 1 || z2 == 6)
                                        break;
                                    if (fa[z1 - 1][z2 + 1] > 0)
                                        break;
                                    if (fa[z1][z2 + 1] > 0 && fa[z1][z2 + 1] == fa[z1 - 1][z2])
                                        break;
                                    aa[k + 1] = z1 - 1;
                                    ba[k + 1] = z2 + 1;
                                }
                            }
                            if (k <= n)
                                continue;
                            fa[a][b] = 9 - 2 * i - j;
                            for (k = 1; k <= n; k++)
                                fa[aa[k + 1]][ba[k + 1]] = fa[a][b];
                            break;
                    }
                    break;
                }
            }
        }
        print("\n");
        print("THE FOLLOWING CODE OF THE BAD GUYS' FLEET DISPOSITION\n");
        print("HAS BEEN CAPTURED BUT NOT DECODED:\n");
        print("\n");
        for (i = 1; i <= 6; i++) {
            for (j = 1; j <= 6; j++) {
                ha[i][j] = fa[j][i];
            }
        }
        for (i = 1; i <= 6; i++) {
            str = "";
            for (j = 1; j <= 6; j++) {
                str += " " + ha[i][j] + " ";
            }
            print(str + "\n");
        }
        print("\n");
        print("DE-CODE IT AND USE IT IF YOU CAN\n");
        print("BUT KEEP THE DE-CODING METHOD A SECRET.\n");
        print("\n");
        for (i = 1; i <= 6; i++) {
            for (j = 1; j <= 6; j++) {
                ha[i][j] = 0;
            }
        }
        for (i = 1; i <= 3; i++)
            la[i] = 0;
        ca[1] = 2;
        ca[2] = 2;
        ca[3] = 1;
        ca[4] = 1;
        ca[5] = 0;
        ca[6] = 0;
        s = 0;
        h = 0;
        print("START GAME\n");
        while (1) {
            str = await input();
            x = parseInt(str);
            y = parseInt(str.substr(str.indexOf(",") + 1));
            if (x < 1 || x > 6 || y < 1 || y > 6) {
                print("INVALID INPUT.  TRY AGAIN.\n");
                continue;
            }
            r = 7 - y;
            c = x;
            if (fa[r][c] <= 0) {
                s++;
                print("SPLASH!  TRY AGAIN.\n");
                continue;
            }
            if (ca[fa[r][c]] >= 4) {
                print("THERE USED TO BE A SHIP AT THAT POINT, BUT YOU SUNK IT.\n");
                print("SPLASH!  TRY AGAIN.\n");
                s++;
                continue;
            }
            if (ha[r][c] > 0) {
                print("YOU ALREADY PUT A HOLE IN SHIP NUMBER " + fa[r][c] + " AT THAT POINT.\n");
                print("SPLASH!  TRY AGAIN.\n");
                s++;
                continue;
            }
            h++;
            ha[r][c] = fa[r][c];
            print("A DIRECT HIT ON SHIP NUMBER " + fa[r][c] + "\n");
            ca[fa[r][c]]++;
            if (ca[fa[r][c]] < 4) {
                print("TRY AGAIN.\n");
                continue;
            }
            la[Math.floor((fa[r][c] - 1) / 2) + 1]++;
            print("AND YOU SUNK IT.  HURRAH FOR THE GOOD GUYS.\n");
            print("SO FAR, THE BAD GUYS HAVE LOST\n");
            print(" " + la[1] + " DESTROYER(S), " + la[2] + " CRUISER(S), AND");
            print(" " + la[3] + " AIRCRAFT CARRIER(S).\n");
            print("YOUR CURRENT SPLASH/HIT RATIO IS " + s / h + "\n");
            if (la[1] + la[2] + la[3] < 6)
                continue;
            print("\n");
            print("YOU HAVE TOTALLY WIPED OUT THE BAD GUYS' FLEET\n");
            print("WITH A FINAL SPLASH/HIT RATIO OF " + s / h + "\n");
            if (s / h <= 0) {
                print("CONGRATULATIONS -- A DIRECT HIT EVERY TIME.\n");
            }
            print("\n");
            print("****************************\n");
            print("\n");
            break;
        }
    }
}

main();
