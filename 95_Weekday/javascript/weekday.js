// WEEKDAY
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

function fna(arg) {
    return Math.floor(arg / 4);
}

function fnb(arg) {
    return Math.floor(arg / 7);
}

var t = [, 0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5];

var k5;
var k6;
var k7;

function time_spent(f, a8)
{
    k1 = Math.floor(f * a8);
    i5 = Math.floor(k1 / 365);
    k1 -= i5 * 365;
    i6 = Math.floor(k1 / 30);
    i7 = k1 - (i6 * 30);
    k5 -= i5;
    k6 -= i6;
    k7 -= i7;
    if (k7 < 0) {
        k7 += 30;
        k6--;
    }
    if (k6 <= 0) {
        k6 += 12;
        k5--;
    }
    print(i5 + "\t" + i6 + "\t" + i7 + "\n");
}

// Main control section
async function main()
{
    print(tab(32) + "WEEKDAY\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("WEEKDAY IS A COMPUTER DEMONSTRATION THAT\n");
    print("GIVES FACTS ABOUT A DATE OF INTEREST TO YOU.\n");
    print("\n");
    print("ENTER TODAY'S DATE IN THE FORM: 3,24,1979  ");
    str = await input();
    m1 = parseInt(str);
    d1 = parseInt(str.substr(str.indexOf(",") + 1));
    y1 = parseInt(str.substr(str.lastIndexOf(",") + 1));
    // This program determines the day of the week
    //  for a date after 1582
    print("ENTER DAY OF BIRTH (OR OTHER DAY OF INTEREST)");
    str = await input();
    m = parseInt(str);
    d = parseInt(str.substr(str.indexOf(",") + 1));
    y = parseInt(str.substr(str.lastIndexOf(",") + 1));
    print("\n");
    i1 = Math.floor((y - 1500) / 100);
    // Test for date before current calendar.
    if (y - 1582 < 0) {
        print("NOT PREPARED TO GIVE DAY OF WEEK PRIOR TO MDLXXXII.\n");
    } else {
        a = i1 * 5 + (i1 + 3) / 4;
        i2 = Math.floor(a - fnb(a) * 7);
        y2 = Math.floor(y / 100);
        y3 = Math.floor(y - y2 * 100);
        a = y3 / 4 + y3 + d + t[m] + i2;
        b = Math.floor(a - fnb(a) * 7) + 1;
        if (m <= 2) {
            if (y3 != 0) {
                t1 = Math.floor(y - fna(y) * 4);
            } else {
                a = i1 - 1;
                t1 = Math.floor(a - fna(a) * 4);
            }
            if (t1 == 0) {
                if (b == 0)
                    b = 6;
                b--;
            }
        }
        if (b == 0)
            b = 7;
        if ((y1 * 12 + m1) * 31 + d1 < (y * 12 + m) * 31 + d) {
            print(m + "/" + d + "/" + y + " WILL BE A ");
        } else if ((y1 * 12 + m1) * 31 + d1 == (y * 12 + m) * 31 + d) {
            print(m + "/" + d + "/" + y + " IS A ");
        } else {
            print(m + "/" + d + "/" + y + " WAS A ");
        }
        switch (b) {
            case 1: print("SUNDAY.\n"); break;
            case 2: print("MONDAY.\n"); break;
            case 3: print("TUESDAY.\n"); break;
            case 4: print("WEDNESDAY.\n"); break;
            case 5: print("THURSDAY.\n"); break;
            case 6:
                if (d == 13) {
                    print("FRIDAY THE THIRTEENTH---BEWARE!\n");
                } else {
                    print("FRIDAY.\n");
                }
                break;
            case 7: print("SATURDAY.\n"); break;
        }
        if ((y1 * 12 + m1) * 31 + d1 != (y * 12 + m) * 31 + d) {
            i5 = y1 - y;
            print("\n");
            i6 = m1 - m;
            i7 = d1 - d;
            if (i7 < 0) {
                i6--;
                i7 += 30;
            }
            if (i6 < 0) {
                i5--;
                i6 += 12;
            }
            if (i5 >= 0) {
                if (i7 == 0 && i6 == 0)
                    print("***HAPPY BIRTHDAY***\n");
                print("                        \tYEARS\tMONTHS\tDAYS\n");
                print("                        \t-----\t------\t----\n");
                print("YOUR AGE (IF BIRTHDATE) \t" + i5 + "\t" + i6 + "\t" + i7 + "\n");
                a8 = (i5 * 365) + (i6 * 30) + i7 + Math.floor(i6 / 2);
                k5 = i5;
                k6 = i6;
                k7 = i7;
                // Calculate retirement date.
                e = y + 65;
                // Calculate time spent in the following functions.
                print("YOU HAVE SLEPT \t\t\t");
                time_spent(0.35, a8);
                print("YOU HAVE EATEN \t\t\t");
                time_spent(0.17, a8);
                if (k5 <= 3) {
                    print("YOU HAVE PLAYED \t\t\t");
                } else if (k5 <= 9) {
                    print("YOU HAVE PLAYED/STUDIED \t\t");
                } else {
                    print("YOU HAVE WORKED/PLAYED \t\t");
                }
                time_spent(0.23, a8);
                if (k6 == 12) {
                    k5++;
                    k6 = 0;
                }
                print("YOU HAVE RELAXED \t\t" + k5 + "\t" + k6 + "\t" + k7 + "\n");
                print("\n");
                print(tab(16) + "***  YOU MAY RETIRE IN " + e + " ***\n");
                print("\n");
            }
        }
    }
    print("\n");
    print("\n");
    print("\n");
    print("\n");
    print("\n");
}

main();
