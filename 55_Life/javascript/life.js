// LIFE
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

var bs = [];
var a = [];

// Main program
async function main()
{
    print(tab(34) + "LIFE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("ENTER YOUR PATTERN:\n");
    x1 = 1;
    y1 = 1;
    x2 = 24;
    y2 = 70;
    for (c = 1; c <= 24; c++) {
        bs[c] = "";
        a[c] = [];
        for (d = 1; d <= 70; d++)
            a[c][d] = 0;
    }
    c = 1;
    while (1) {
        bs[c] = await input();
        if (bs[c] == "DONE") {
            bs[c] = "";
            break;
        }
        if (bs[c].substr(0, 1) == ".")
            bs[c] = " " + bs[c].substr(1);
        c++;
    }
    c--;
    l = 0;
    for (x = 1; x <= c - 1; x++) {
        if (bs[x].length > l)
            l = bs[x].length;
    }
    x1 = 11 - (c >> 1);
    y1 = 33 - (l >> 1);
    p = 0;
    for (x = 1; x <= c; x++) {
        for (y = 1; y <= bs[x].length; y++) {
            if (bs[x][y - 1] != " ") {
                a[x1 + x][y1 + y] = 1;
                p++;
            }
        }
    }
    print("\n");
    print("\n");
    print("\n");
    i9 = false;
    g = 0;
    while (g < 100) {
        print("GENERATION: " + g + " POPULATION: " + p + " ");
        if (i9)
            print("INVALID!");
        x3 = 24;
        y3 = 70;
        x4 = 1;
        y4 = 1;
        p = 0;
        g++;
        for (x = 1; x <= x1 - 1; x++)
            print("\n");
        for (x = x1; x <= x2; x++) {
            print("\n");
            str = "";
            for (y = y1; y <= y2; y++) {
                if (a[x][y] == 2) {
                    a[x][y] = 0;
                    continue;
                } else if (a[x][y] == 3) {
                    a[x][y] = 1;
                } else if (a[x][y] != 1) {
                    continue;
                }
                while (str.length < y)
                    str += " ";
                str += "*";
                if (x < x3)
                    x3 = x;
                if (x > x4)
                    x4 = x;
                if (y < y3)
                    y3 = y;
                if (y > y4)
                    y4 = y;
            }
            print(str);
        }
        for (x = x2 + 1; x <= 24; x++)
            print("\n");
        x1 = x3;
        x2 = x4;
        y1 = y3;
        y2 = y4;
        if (x1 < 3) {
            x1 = 3;
            i9 = true;
        }
        if (x2 > 22) {
            x2 = 22;
            i9 = true;
        }
        if (y1 < 3) {
            y1 = 3;
            i9 = true;
        }
        if (y2 > 68) {
            y2 = 68;
            i9 = true;
        }
        p = 0;
        for (x = x1 - 1; x <= x2 + 1; x++) {
            for (y = y1 - 1; y <= y2 + 1; y++) {
                c = 0;
                for (i = x - 1; i <= x + 1; i++) {
                    for (j = y - 1; j <= y + 1; j++) {
                        if (a[i][j] == 1 || a[i][j] == 2)
                            c++;
                    }
                }
                if (a[x][y] == 0) {
                    if (c == 3) {
                        a[x][y] = 3;
                        p++;
                    }
                } else {
                    if (c < 3 || c > 4) {
                        a[x][y] = 2;
                    } else {
                        p++;
                    }
                }
            }
        }
        x1--;
        y1--;
        x2++;
        y2++;
    }
}

main();
