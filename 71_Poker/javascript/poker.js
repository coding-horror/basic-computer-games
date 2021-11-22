// POKER
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
var b;
var c;
var d;
var g;
var i;
var k;
var m;
var n;
var p;
var s;
var u;
var v;
var x;
var z;
var hs;
var is;
var js;
var ks;

function fna(x)
{
    return Math.floor(10 * Math.random());
}

function fnb(x)
{
    return x % 100;
}

function im_busted()
{
    print("I'M BUSTED.  CONGRATULATIONS!\n");
}

// 1740
function deal_card()
{
    while (1) {
        aa[z] = 100 * Math.floor(4 * Math.random()) + Math.floor(100 * Math.random());
        if (Math.floor(aa[z] / 100) > 3)    // Invalid suit
            continue;
        if (aa[z] % 100 > 12) // Invalid number
            continue;
        if (z != 1) {
            for (k = 1; k <= z - 1; k++) {
                if (aa[z] == aa[k])
                    break;
            }
            if (k <= z - 1) // Repeated card
                continue;
            if (z > 10) {
                n = aa[u];
                aa[u] = aa[z];
                aa[z] = n;
            }
        }
        return;
    }
}

// 1850
function show_cards()
{
    for (z = n; z <= n + 4; z++) {
        print(" " + z + "--  ");
        k = fnb(aa[z]);
        show_number();
        print(" OF");
        k = Math.floor(aa[z] / 100);
        show_suit();
        if (z % 2 == 0)
            print("\n");
    }
    print("\n");
}

// 1950
function show_number()
{
    if (k == 9)
        print("JACK");
    if (k == 10)
        print("QUEEN");
    if (k == 11)
        print("KING");
    if (k == 12)
        print("ACE");
    if (k < 9)
        print(" " + (k + 2));
}

// 2070
function show_suit()
{
    if (k == 0)
        print(" CLUBS\t");
    if (k == 1)
        print(" DIAMONDS\t");
    if (k == 2)
        print(" HEARTS\t");
    if (k == 3)
        print(" SPADES\t");
}

// 2170
function evaluate_hand()
{
    u = 0;
    for (z = n; z <= n + 4; z++) {
        ba[z] = fnb(aa[z]);
        if (z != n + 4) {
            if (Math.floor(aa[z] / 100) == Math.floor(aa[z + 1] / 100))
                u++;
        }
    }
    if (u == 4) {
        x = 11111;
        d = aa[n];
        hs = "A FLUS";
        is = "H IN";
        u = 15;
        return;
    }
    for (z = n; z <= n + 3; z++) {
        for (k = z + 1; k <= n + 4; k++) {
            if (ba[z] > ba[k]) {
                x = aa[z];
                aa[z] = aa[k];
                ba[z] = ba[k];
                aa[k] = x;
                ba[k] = aa[k] - 100 * Math.floor(aa[k] / 100);
            }
        }
    }
    x = 0;
    for (z = n; z <= n + 3; z++) {
        if (ba[z] == ba[z + 1]) {
            x = x + 11 * Math.pow(10, z - n);
            d = aa[z];
            if (u < 11) {
                u = 11;
                hs = "A PAIR";
                is = " OF ";
            } else if (u == 11) {
                if (ba[z] == ba[z - 1]) {
                    hs = "THREE";
                    is = " ";
                    u = 13;
                } else {
                    hs = "TWO P";
                    is = "AIR, ";
                    u = 12;
                }
            } else if (u == 12) {
                u = 16;
                hs = "FULL H";
                is = "OUSE, ";
            } else if (ba[z] == ba[z - 1]) {
                u = 17;
                hs = "FOUR";
                is = " ";
            } else {
                u = 16;
                hs = "FULL H";
                is = "OUSE. ";
            }
        }
    }
    if (x == 0) {
        if (ba[n] + 3 == ba[n + 3]) {
            x = 1111;
            u = 10;
        }
        if (ba[n + 1] + 3 == ba[n + 4]) {
            if (u == 10) {
                u = 14;
                hs = "STRAIG";
                is = "HT";
                x = 11111;
                d = aa[n + 4];
                return;
            }
            u = 10;
            x = 11110;
        }
    }
    if (u < 10) {
        d = aa[n + 4];
        hs = "SCHMAL";
        is = "TZ, ";
        u = 9;
        x = 11000;
        i = 6;
        return;
    }
    if (u == 10) {
        if (i == 1)
            i = 6;
        return;
    }
    if (u > 12)
        return;
    if (fnb(d) > 6)
        return;
    i = 6;
}

function get_prompt(question, def)
{
    var str;
    
    str = window.prompt(question, def);
    print(question + "? " + str + "\n");
    return str;
}

function player_low_in_money()
{
    print("\n");
    print("YOU CAN'T BET WITH WHAT YOU HAVEN'T GOT.\n");
    str = "N";
    if (o % 2 != 0) {
        str = get_prompt("WOULD YOU LIKE TO SELL YOUR WATCH", "YES");
        if (str.substr(0, 1) != "N") {
            if (fna(0) < 7) {
                print("I'LL GIVE YOU $75 FOR IT.\n");
                s += 75;
            } else {
                print("THAT'S A PRETTY CRUMMY WATCH - I'LL GIVE YOU $25.\n");
                s += 25;
            }
            o *= 2;
        }
    }
    if (o % 3 == 0 && str.substr(0, 1) == "N") {
        str = get_prompt("WILL YOU PART WITH THAT DIAMOND TIE TACK", "YES");
        if (str.substr(0, 1) != "N") {
            if (fna(0) < 6) {
                print("YOU ARE NOW $100 RICHER.\n");
                s += 100;
            } else {
                print("IT'S PASTE.  $25.\n");
                s += 25;
            }
            o *= 3;
        }
    }
    if (str.substr(0,1) == "N") {
        print("YOUR WAD IS SHOT.  SO LONG, SUCKER!\n");
        return true;
    }
    return false;
}

function computer_low_in_money()
{
    if (c - g - v >= 0)
        return false;
    if (g == 0) {
        v = c;
        return false;
    }
    if (c - g < 0) {
        print("I'LL SEE YOU.\n");
        k = g;
        s = s - g;
        c = c - k;
        p = p + g + k;
        return false;
    }
    js = "N";
    if (o % 2 == 0) {
        js = get_prompt("WOULD YOU LIKE TO BUY BACK YOUR WATCH FOR $50", "YES");
        if (js.substr(0, 1) != "N") {
            c += 50;
            o /= 2;
        }
    }
    if (js.substr(0, 1) == "N" && o % 3 == 0) {
        js = get_prompt("WOULD YOU LIKE TO BUY BACK YOUR TIE TACK FOR $50", "YES");
        if (js.substr(0, 1) != "N") {
            c += 50;
            o /= 3;
        }
    }
    if (js.substr(0, 1) == "N") {
        print("I'M BUSTED.  CONGRATULATIONS!\n");
        return true;
    }
    return false;
}

function ask_for_bet()
{
    var forced;
    
    if (t != Math.floor(t)) {
        if (k != 0 || g != 0 || t != 0.5) {
            print("NO SMALL CHANGE, PLEASE.\n");
            return 0;
        }
        return 1;
    }
    if (s - g - t < 0) {
        if (player_low_in_money())
            return 2;
        return 0;
    }
    if (t == 0) {
        i = 3;
    } else if (g + t < k) {
        print("IF YOU CAN'T SEE MY BET, THEN FOLD.\n");
        return 0;
    } else {
        g += t;
        if (g != k) {
            forced = false;
            if (z != 1) {
                if (g <= 3 * z)
                    forced = true;
            } else {
                if (g <= 5) {
                    if (z < 2) {
                        v = 5;
                        if (g <= 3 * z)
                            forced = true;
                    }
                } else {
                    if (z == 1 || t > 25) {
                        i = 4;
                        print("I FOLD.\n");
                        return 1;
                    }
                }
            }
            if (forced || z == 2) {
                v = g - k + fna(0);
                if (computer_low_in_money())
                    return 2;
                print("I'LL SEE YOU, AND RAISE YOU " + v + "\n");
                k = g + v;
                return 0;
            }
            print("I'LL SEE YOU.\n");
            k = g;
        }
    }
    s -= g;
    c -= k;
    p += g + k;
    return 1;
}

function check_for_win(type)
{
    if (type == 0 && i == 3 || type == 1) {
        print("\n");
        print("I WIN.\n");
        c += p;
    } else if (type == 0 && i == 4 || type == 2) {
        print("\n");
        print("YOU WIN.\n");
        s += p;
    } else {
        return 0;
    }
    print("NOW I HAVE $" + c + " AND YOU HAVE $" + s + "\n");
    return 1;
}

function show_hand()
{
    print(hs + is);
    if (hs == "A FLUS") {
        k = Math.floor(k / 100);
        print("\n");
        show_suit();
        print("\n");
    } else {
        k = fnb(k);
        show_number();
        if (hs == "SCHMAL" || hs == "STRAIG")
            print(" HIGH\n");
        else
            print("'S\n");
    }
}

// Main program
async function main()
{
    print(tab(33) + "POKER\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("WELCOME TO THE CASINO.  WE EACH HAVE $200.\n");
    print("I WILL OPEN THE BETTING BEFORE THE DRAW; YOU OPEN AFTER.\n");
    print("TO FOLD BET 0; TO CHECK BET .5.\n");
    print("ENOUGH TALK -- LET'S GET DOWN TO BUSINESS.\n");
    print("\n");
    o = 1;
    c = 200;
    s = 200;
    z = 0;
    while (1) {
        p = 0;
        //
        print("\n");
        if (c <= 5) {
            im_busted();
            return;
        }
        print("THE ANTE IS $5, I WILL DEAL:\n");
        print("\n");
        if (s <= 5) {
            if (player_low_in_money())
                return;
        }
        p += 10;
        s -= 5;
        c -= 5;
        for (z = 1; z <= 10; z++)
            deal_card();
        print("YOUR HAND:\n");
        n = 1;
        show_cards();
        n = 6;
        i = 2;
        evaluate_hand();
        print("\n");
        first = true;
        if (i == 6) {
            if (fna(0) > 7) {
                x = 11100;
                i = 7;
                z = 23;
            } else if (fna(0) > 7) {
                x = 11110;
                i = 7;
                z = 23;
            } else if (fna(0) < 2) {
                x = 11111;
                i = 7;
                z = 23;
            } else {
                z = 1;
                k = 0;
                print("I CHECK.\n");
                first = false;
            }
        } else {
            if (u < 13) {
                if (fna(0) < 2) {
                    i = 7;
                    z = 23;
                } else {
                    z = 0;
                    k = 0;
                    print("I CHECK.\n");
                    first = false;
                }
            } else if (u > 16) {
                z = 2;
                if (fna(0) < 1)
                    z = 35;
            } else {
                z = 35;
            }
        }
        if (first) {
            v = z + fna(0);
            g = 0;
            if (computer_low_in_money())
                return;
            print("I'LL OPEN WITH $" + v + "\n");
            k = v;
        }
        g = 0;
        do {
            print("\nWHAT IS YOUR BET");
            t = parseFloat(await input());
            status = ask_for_bet();
        } while (status == 0) ;
        if (status == 2)
            return;
        status = check_for_win(0);
        if (status == 1) {
            while (1) {
                print("DO YOU WISH TO CONTINUE");
                hs = await input();
                if (hs == "YES") {
                    status = 1;
                    break;
                }
                if (hs == "NO") {
                    status = 2;
                    break;
                }
                print("ANSWER YES OR NO, PLEASE.\n");
            }
        }
        if (status == 2)
            return;
        if (status == 1) {
            p = 0;
            continue;
        }
        print("\n");
        print("NOW WE DRAW -- HOW MANY CARDS DO YOU WANT");
        while (1) {
            t = parseInt(await input());
            if (t != 0) {
                z = 10;
                if (t >= 4) {
                    print("YOU CAN'T DRAW MORE THAN THREE CARDS.\n");
                    continue;
                }
                print("WHAT ARE THEIR NUMBERS:\n");
                for (q = 1; q <= t; q++) {
                    u = parseInt(await input());
                    z++;
                    deal_card();
                }
                print("YOUR NEW HAND:\n");
                n = 1;
                show_cards();
            }
            break;
        }
        z = 10 + t;
        for (u = 6; u <= 10; u++) {
            if (Math.floor(x / Math.pow(10, u - 6)) != 10 * Math.floor(x / Math.pow(10, u - 5)))
                break;
            z++;
            deal_card();
        }
        print("\n");
        print("I AM TAKING " + (z - 10 - t) + " CARD");
        if (z != 11 + t) {
            print("S");
        }
        print("\n");
        n = 6;
        v = i;
        i = 1;
        evaluate_hand();
        b = u;
        m = d;
        if (v == 7) {
            z = 28;
        } else if (i == 6) {
            z = 1;
        } else {
            if (u < 13) {
                z = 2;
                if (fna(0) == 6)
                    z = 19;
            } else if (u < 16) {
                z = 19;
                if (fna(0) == 8)
                    z = 11;
            } else {
                z = 2;
            }
        }
        k = 0;
        g = 0;
        do {
            print("\nWHAT IS YOUR BET");
            t = parseFloat(await input());
            status = ask_for_bet();
        } while (status == 0) ;
        if (status == 2)
            return;
        if (t == 0.5) {
            if (v != 7 && i == 6) {
                print("I'LL CHECK\n");
            } else {
                v = z + fna(0);
                if (computer_low_in_money())
                    return;
                print("I'LL BET $" + v + "\n");
                k = v;
                do {
                    print("\nWHAT IS YOUR BET");
                    t = parseFloat(await input());
                    status = ask_for_bet();
                } while (status == 0) ;
                if (status == 2)
                    return;
                status = check_for_win(0);
                if (status == 1) {
                    while (1) {
                        print("DO YOU WISH TO CONTINUE");
                        hs = await input();
                        if (hs == "YES") {
                            status = 1;
                            break;
                        }
                        if (hs == "NO") {
                            status = 2;
                            break;
                        }
                        print("ANSWER YES OR NO, PLEASE.\n");
                    }
                }
                if (status == 2)
                    return;
                if (status == 1) {
                    p = 0;
                    continue;
                }
            }
        } else {
            status = check_for_win(0);
            if (status == 1) {
                while (1) {
                    print("DO YOU WISH TO CONTINUE");
                    hs = await input();
                    if (hs == "YES") {
                        status = 1;
                        break;
                    }
                    if (hs == "NO") {
                        status = 2;
                        break;
                    }
                    print("ANSWER YES OR NO, PLEASE.\n");
                }
            }
            if (status == 2)
                return;
            if (status == 1) {
                p = 0;
                continue;
            }
        }
        print("\n");
        print("NOW WE COMPARE HANDS:\n");
        js = hs;
        ks = is;
        print("MY HAND:\n");
        n = 6;
        show_cards();
        n = 1;
        evaluate_hand();
        print("\n");
        print("YOU HAVE ");
        k = d;
        show_hand();
        hs = js;
        is = ks;
        k = m;
        print("AND I HAVE ");
        show_hand();
        status = 0;
        if (b > u) {
            status = 1;
        } else if (u > b) {
            status = 2;
        } else {
            if (hs != "A FLUS") {
                if (fnb(m) < fnb(d))
                    status = 2;
                else if (fnb(m) > fnb(d))
                    status = 1;
            } else {
                if (fnb(m) > fnb(d))
                    status = 1;
                else if (fnb(d) > fnb(m))
                    status = 2;
            }
            if (status == 0) {
                print("THE HAND IS DRAWN.\n");
                print("ALL $" + p + " REMAINS IN THE POT.\n");
                continue;
            }
        }
        status = check_for_win(status);
        if (status == 1) {
            while (1) {
                print("DO YOU WISH TO CONTINUE");
                hs = await input();
                if (hs == "YES") {
                    status = 1;
                    break;
                }
                if (hs == "NO") {
                    status = 2;
                    break;
                }
                print("ANSWER YES OR NO, PLEASE.\n");
            }
        }
        if (status == 2)
            return;
        if (status == 1) {
            p = 0;
            continue;
        }
    }
}

main();
