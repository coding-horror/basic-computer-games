// AWARI
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

print(tab(34) + "AWARI\n");
print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");

n = 0;

b = [0,0,0,0,0,0,0,0,0,0,0,0,0,0];
g = [0,0,0,0,0,0,0,0,0,0,0,0,0,0];
f = [];
for (i = 0; i <= 50; i++) {
    f[i] = 0;
}

function show_number(number)
{
    if (number < 10)
        print("  " + number + " ");
    else
        print(" " + number + " ");
}

function show_board()
{
    var i;
    
    print("\n");
    print("   ");
    for (i = 12; i >= 7; i--)
        show_number(b[i]);
    print("\n");
    i = 13;
    show_number(b[i]);
    print("                       " + b[6] + "\n");
    print("   ");
    for (i = 0; i <= 5; i++)
        show_number(b[i]);
    print("\n");
    print("\n");
}

function do_move()
{
    k = m;
    adjust_board();
    e = 0;
    if (k > 6)
        k -= 7;
    c++;
    if (c < 9)
        f[n] = f[n] * 6 + k
        for (i = 0; i <= 5; i++) {
            if (b[i] != 0) {
                for (i = 7; i <= 12; i++) {
                    if (b[i] != 0) {
                        e = 1;
                        return;
                    }
                }
            }
        }
}

function adjust_board()
{
    p = b[m];
    b[m] = 0;
    while (p >= 1) {
        m++;
        if (m > 13)
            m -= 14;
        b[m]++;
        p--;
    }
    if (b[m] == 1) {
        if (m != 6 && m != 13) {
            if (b[12 - m] != 0) {
                b[h] += b[12 - m] + 1;
                b[m] = 0;
                b[12 - m] = 0;
            }
        }
    }
}

function computer_move()
{
    d = -99;
    h = 13;
    for (i = 0; i<= 13; i++)	// Backup board
        g[i] = b[i];
    for (j = 7; j <= 12; j++) {
        if (b[j] == 0)
            continue;
        q = 0;
        m = j;
        adjust_board();
        for (i = 0; i <= 5; i++) {
            if (b[i] == 0)
                continue;
            l = b[i] + i;
            r = 0;
            while (l > 13) {
                l -= 14;
                r = 1;
            }
            if (b[l] == 0) {
                if (l != 6 && l != 13)
                    r = b[12 - l] + r;
            }
            if (r > q)
                q = r;
        }
        q = b[13] - b[6] - q;
        if (c < 8) {
            k = j;
            if (k > 6)
                k -= 7;
            for (i = 0; i <= n - 1; i++) {
                if (f[n] * 6 + k == Math.floor(f[i] / Math.pow(7 - c, 6) + 0.1))
                    q -= 2;
            }
        }
        for (i = 0; i <= 13; i++)	// Restore board
            b[i] = g[i];
        if (q >= d) {
            a = j;
            d = q;
        }
    }
    m = a;
    print(m - 6);
    do_move();
}

// Main program
async function main()
{
    while (1) {
        print("\n");
        print("\n");
        e = 0;
        for (i = 0; i <= 12; i++)
            b[i] = 3;
        
        c = 0;
        f[n] = 0;
        b[13] = 0;
        b[6] = 0;
        
        while (1) {
            show_board();
            print("YOUR MOVE");
            while (1) {
                m = parseInt(await input());
                if (m < 7) {
                    if (m > 0) {
                        m--;
                        if (b[m] != 0)
                            break;
                    }
                }
                print("ILLEGAL MOVE\n");
                print("AGAIN");
            }
            h = 6;
            do_move();
            show_board();
            if (e == 0)
                break;
            if (m == h) {
                print("AGAIN");
                while (1) {
                    m = parseInt(await input());
                    if (m < 7) {
                        if (m > 0) {
                            m--;
                            if (b[m] != 0)
                                break;
                        }
                    }
                    print("ILLEGAL MOVE\n");
                    print("AGAIN");
                }
                h = 6;
                do_move();
                show_board();
            }
            if (e == 0)
                break;
            print("MY MOVE IS ");
            computer_move();
            if (e == 0)
                break;
            if (m == h) {
                print(",");
                computer_move();
            }
            if (e == 0)
                break;
        }
        print("\n");
        print("GAME OVER\n");
        d = b[6] - b[13];
        if (d < 0)
            print("I WIN BY " + -d + " POINTS\n");
        else if (d == 0) {
            n++;
            print("DRAWN GAME\n");
        } else {
            n++;
            print("YOU WIN BY " + d + " POINTS\n");
        }
    }
}

main();
