// SALVO
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
var ba = [];
var ca = [];
var da = [];
var ea = [];
var fa = [];
var ga = [];
var ha = [];
var ka = [];
var w;
var r3;
var x;
var y;
var v;
var v2;

function sgn(k)
{
    if (k < 0)
        return -1;
    if (k > 0)
        return 1;
    return 0;
}

function fna(k)
{
    return (5 - k) * 3 - 2 * Math.floor(k / 4) + sgn(k - 1) - 1;
}

function fnb(k)
{
    return k + Math.floor(k / 4) - sgn(k - 1);
}

function generate_random()
{
    x = Math.floor(Math.random() * 10 + 1);
    y = Math.floor(Math.random() * 10 + 1);
    v = Math.floor(3 * Math.random() - 1);
    v2 = Math.floor(3 * Math.random() - 1);
}

// Main program
async function main()
{
    print(tab(33) + "SALVO\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    z8 = 0;
    for (w = 1; w <= 12; w++) {
        ea[w] = -1;
        ha[w] = -1;
    }
    for (x = 1; x <= 10; x++) {
        ba[x] = [];
        ka[x] = [];
        for (y = 1; y <= 10; y++) {
            ba[x][y] = 0;
            ka[x][y] = 0;
        }
    }
    for (x = 1; x <= 12; x++) {
        fa[x] = 0;
        ga[x] = 0;
    }
    for (x = 1; x <= 10; x++) {
        aa[x] = [];
        for (y = 1; y <= 10; y++) {
            aa[x][y] = 0;
        }
    }
    u6 = 0;
    for (k = 4; k >= 1; k--) {
        do {
            generate_random();
        } while (v + v2 + v * v2 == 0 || y + v * fnb(k) > 10 || y + v * fnb(k) < 1 || x + v2 * fnb(k) > 10 || x + v2 * fnb(k) < 1) ;
        u6++;
        if (u6 > 25) {
            for (x = 1; x <= 10; x++) {
                aa[x] = [];
                for (y = 1; y <= 10; y++) {
                    aa[x][y] = 0;
                }
            }
            u6 = 0;
            k = 5;
            continue;
        }
        for (z = 0; z <= fnb(k); z++) {
            fa[z + fna(k)] = x + v2 * z;
            ga[z + fna(k)] = y + v * z;
        }
        u8 = fna(k);
        if (u8 <= u8 + fnb(k)) {
            retry = false;
            for (z2 = u8; z2 <= u8 + fnb(k); z2++) {
                if (u8 >= 2) {
                    for (z3 = 1; z3 < u8 - 1; z3++) {
                        if (Math.sqrt(Math.pow((fa[z3] - fa[z2]), 2)) + Math.pow((ga[z3] - ga[z2]), 2) < 3.59) {
                            retry = true;
                            break;
                        }
                    }
                    if (retry)
                        break;
                }
            }
            if (retry) {
                k++;
                continue;
            }
        }
        for (z = 0; z <= fnb(k); z++) {
            if (k - 1 < 0)
                sk = -1;
            else if (k - 1 > 0)
                sk = 1;
            else
                sk = 0;
            aa[fa[z + u8]][ga[z + u8]] = 0.5 + sk * (k - 1.5);
        }
        u6 = 0;
    }
    print("ENTER COORDINATES FOR...\n");
    print("BATTLESHIP\n");
    for (x = 1; x <= 5; x++) {
        str = await input();
        y = parseInt(str);
        z = parseInt(str.substr(str.indexOf(",") + 1));
        ba[y][z] = 3;
    }
    print("CRUISER\n");
    for (x = 1; x <= 3; x++) {
        str = await input();
        y = parseInt(str);
        z = parseInt(str.substr(str.indexOf(",") + 1));
        ba[y][z] = 2;
    }
    print("DESTROYER<A>\n");
    for (x = 1; x <= 2; x++) {
        str = await input();
        y = parseInt(str);
        z = parseInt(str.substr(str.indexOf(",") + 1));
        ba[y][z] = 1;
    }
    print("DESTROYER<B>\n");
    for (x = 1; x <= 2; x++) {
        str = await input();
        y = parseInt(str);
        z = parseInt(str.substr(str.indexOf(",") + 1));
        ba[y][z] = 0.5;
    }
    while (1) {
        print("DO YOU WANT TO START");
        js = await input();
        if (js == "WHERE ARE YOUR SHIPS?") {
            print("BATTLESHIP\n");
            for (z = 1; z <= 5; z++)
                print(" " + fa[z] + " " + ga[z] + "\n");
            print("CRUISER\n");
            print(" " + fa[6] + " " + ga[6] + "\n");
            print(" " + fa[7] + " " + ga[7] + "\n");
            print(" " + fa[8] + " " + ga[8] + "\n");
            print("DESTROYER<A>\n");
            print(" " + fa[9] + " " + ga[9] + "\n");
            print(" " + fa[10] + " " + ga[10] + "\n");
            print("DESTROYER<B>\n");
            print(" " + fa[11] + " " + ga[11] + "\n");
            print(" " + fa[12] + " " + ga[12] + "\n");
        } else {
            break;
        }
    }
    c = 0;
    print("DO YOU WANT TO SEE MY SHOTS");
    ks = await input();
    print("\n");
    if (js != "YES")
        first_time = true;
    else
        first_time = false;
    while (1) {
        if (first_time) {
            first_time = false;
        } else {
            if (js == "YES") {
                c++;
                print("\n");
                print("TURN " + c + "\n");
            }
            a = 0;
            for (w = 0.5; w <= 3; w += 0.5) {
            loop1:
                for (x = 1; x <= 10; x++) {
                    for (y = 1; y <= 10; y++) {
                        if (ba[x][y] == w) {
                            a += Math.floor(w + 0.5);
                            break loop1;
                        }
                    }
                }
            }
            for (w = 1; w <= 7; w++) {
                ca[w] = 0;
                da[w] = 0;
                fa[w] = 0;
                ga[w] = 0;
            }
            p3 = 0;
            for (x = 1; x <= 10; x++) {
                for (y = 1; y <= 10; y++) {
                    if (aa[x][y] <= 10)
                        p3++;
                }
            }
            print("YOU HAVE " + a + " SHOTS.\n");
            if (p3 < a) {
                print("YOU HAVE MORE SHOTS THAN THERE ARE BLANK SQUARES.\n");
                print("YOU HAVE WON.\n");
                return;
            }
            if (a == 0) {
                print("I HAVE WON.\n");
                return;
            }
            for (w = 1; w <= a; w++) {
                while (1) {
                    str = await input();
                    x = parseInt(str);
                    y = parseInt(str.substr(str.indexOf(",") + 1));
                    if (x >= 1 && x <= 10 && y >= 1 && y <= 10) {
                        if (aa[x][y] > 10) {
                            print("YOU SHOT THERE BEFORE ON TURN " + (aa[x][y] - 10) + "\n");
                            continue;
                        }
                        break;
                    }
                    print("ILLEGAL, ENTER AGAIN.\n");
                }
                ca[w] = x;
                da[w] = y;
            }
            for (w = 1; w <= a; w++) {
                if (aa[ca[w]][da[w]] == 3) {
                    print("YOU HIT MY BATTLESHIP.\n");
                } else if (aa[ca[w]][da[w]] == 2) {
                    print("YOU HIT MY CRUISER.\n");
                } else if (aa[ca[w]][da[w]] == 1) {
                    print("YOU HIT MY DESTROYER<A>.\n");
                } else if (aa[ca[w]][da[w]] == 0.5) {
                    print("YOU HIT MY DESTROYER<B>.\n");
                }
                aa[ca[w]][da[w]] = 10 + c;
            }
        }
        a = 0;
        if (js != "YES") {
            c++;
            print("\n");
            print("TURN " + c + "\n");
        }
        a = 0;
        for (w = 0.5; w <= 3; w += 0.5) {
        loop2:
            for (x = 1; x <= 10; x++) {
                for (y = 1; y <= 10; y++) {
                    if (ba[x][y] == w) {
                        a += Math.floor(w + 0.5);
                        break loop2;
                    }
                }
            }
        }
        p3 = 0;
        for (x = 1; x <= 10; x++) {
            for (y = 1; y <= 10; y++) {
                if (aa[x][y] <= 10)
                    p3++;
            }
        }
        print("I HAVE " + a + " SHOTS.\n");
        if (p3 < a) {
            print("I HAVE MORE SHOTS THAN BLANK SQUARES.\n");
            print("I HAVE WON.\n");
            return;
        }
        if (a == 0) {
            print("YOU HAVE WON.\n");
            return;
        }
        for (w = 1; w <= 12; w++) {
            if (ha[w] > 0)
                break;
        }
        if (w <= 12) {
            for (r = 1; r <= 10; r++) {
                ka[r] = [];
                for (s = 1; s <= 10; s++)
                    ka[r][s] = 0;
            }
            for (u = 1; u <= 12; u++) {
                if (ea[u] >= 10)
                    continue;
                for (r = 1; r <= 10; r++) {
                    for (s = 1; s <= 10; s++) {
                        if (ba[r][s] >= 10) {
                            ka[r][s] = -10000000;
                        } else {
                            for (m = sgn(1 - r); m <= sgn(10 - r); m++) {
                                for (n = sgn(1 - s); n <= sgn(10 - s); n++) {
                                    if (n + m + n * m != 0 && ba[r + m][s + n] == ea[u])
                                        ka[r][s] += ea[u] - s * Math.floor(ha[u] + 0.5);
                                }
                            }
                        }
                    }
                }
            }
            for (r = 1; r <= a; r++) {
                fa[r] = r;
                ga[r] = r;
            }
            for (r = 1; r <= 10; r++) {
                for (s = 1; s <= 10; s++) {
                    q9 = 1;
                    for (m = 1; m <= a; m++) {
                        if (ka[fa[m]][ga[m]] < ka[fa[q9]][ga[q9]])
                            q9 = m;
                    }
                    if ((r > a || r != s) && ka[r][s] >= ka[fa[q9]][ga[q9]]) {
                        for (m = 1; m <= a; m++) {
                            if (fa[m] != r) {
                                fa[q9] = r;
                                ga[q9] = s;
                                break;
                            }
                            if (ga[m] == s)
                                break;
                        }
                    }
                }
            }
        } else {
            // RANDOM
            w = 0;
            r3 = 0;
            generate_random();
            r2 = 0;
            while (1) {
                r3++;
                if (r3 > 100) {
                    generate_random();
                    r2 = 0;
                    r3 = 1;
                }
                if (x > 10) {
                    x = 10 - Math.floor(Math.random() * 2.5);
                } else if (x <= 0) {
                    x = 1 + Math.floor(Math.random() * 2.5);
                }
                if (y > 10) {
                    y = 10 - Math.floor(Math.random() * 2.5);
                } else if (y <= 0) {
                    y = 1 + Math.floor(Math.random() * 2.5);
                }
                while (1) {
                    valid = true;
                    if (x < 1 || x > 10 || y < 1 || y > 10 || ba[x][y] > 10) {
                        valid = false;
                    } else {
                        for (q9 = 1; q9 <= w; q9++) {
                            if (fa[q9] == x && ga[q9] == y) {
                                valid = false;
                                break;
                            }
                        }
                        if (q9 > w)
                            w++;
                    }
                    if (valid) {
                        fa[w] = x;
                        ga[w] = y;
                        if (w == a) {
                            finish = true;
                            break;
                        }
                    }
                    if (r2 == 6) {
                        r2 = 0;
                        finish = false;
                        break;
                    }
                    x1 = [1,-1, 1,1,0,-1][r2];
                    y1 = [1, 1,-3,1,2, 1][r2];
                    r2++;
                    x += x1;
                    y += y1;
                }
                if (finish)
                    break;
            }
        }
        if (ks == "YES") {
            for (z5 = 1; z5 <= a; z5++)
                print(" " + fa[z5] + " " + ga[z5] + "\n");
        }
        for (w = 1; w <= a; w++) {
            hit = false;
            if (ba[fa[w]][ga[w]] == 3) {
                print("I HIT YOUR BATTLESHIP.\n");
                hit = true;
            } else if (ba[fa[w]][ga[w]] == 2) {
                print("I HIT YOUR CRUISER.\n");
                hit = true;
            } else if (ba[fa[w]][ga[w]] == 1) {
                print("I HIT YOUR DESTROYER<A>.\n");
                hit = true;
            } else if (ba[fa[w]][ga[w]] == 0.5) {
                print("I HIT YOUR DESTROYER<B>.\n");
                hit = true;
            }
            if (hit) {
                for (q = 1; q <= 12; q++) {
                    if (ea[q] != -1)
                        continue;
                    ea[q] = 10 + c;
                    ha[q] = ba[fa[w]][ga[w]];
                    m3 = 0;
                    for (m2 = 1; m2 <= 12; m2++) {
                        if (ha[m2] == ha[q])
                            m3++;
                    }
                    if (m3 == Math.floor(ha[q] + 0.5) + 1 + Math.floor(Math.floor(ha[q] + 0.5) / 3)) {
                        for (m2 = 1; m2 <= 12; m2++) {
                            if (ha[m2] == ha[q]) {
                                ea[m2] = -1;
                                ha[m2] = -1;
                            }
                        }
                    }
                    break;
                }
                if (q > 12) {
                    print("PROGRAM ABORT:\n");
                    for (q = 1; q <= 12; q++) {
                        print("ea[" + q + "] = " + ea[q] + "\n");
                        print("ha[" + q + "] = " + ha[q] + "\n");
                    }
                    return;
                }
            }
            ba[fa[w]][ga[w]] = 10 + c;
        }
    }
}

main();
