// HAMMURABI
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

var a;
var s;

function exceeded_grain()
{
    print("HAMURABI: THINK AGAIN.  YOU HAVE ONLY\n");
    print(s + " BUSHELS OF GRAIN.  NOW THEN,\n");
    
}

function exceeded_acres()
{
    print("HAMURABI: THINK AGAIN.  YOU OWN ONLY " + a + " ACRES.  NOW THEN,\n");
}

// Main control section
async function main()
{
    print(tab(32) + "HAMURABI\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("TRY YOUR HAND AT GOVERNING ANCIENT SUMERIA\n");
    print("FOR A TEN-YEAR TERM OF OFFICE.\n");
    print("\n");
    
    d1 = 0;
    p1 = 0;
    z = 0;
    p = 95;
    s = 2800;
    h = 3000;
    e = h - s;
    y = 3;
    a = h / y;
    i = 5;
    q = 1;
    d = 0;
    while (1) {
        print("\n");
        print("\n");
        print("\n");
        print("HAMURABI:  I BEG TO REPORT TO YOU,\n");
        z++;
        print("IN YEAR " + z + ", " + d + " PEOPLE STARVED, " + i + " CAME TO THE CITY,\n");
        p += i;
        if (q <= 0) {
            p = Math.floor(p / 2);
            print("A HORRIBLE PLAGUE STRUCK!  HALF THE PEOPLE DIED.\n");
        }
        print("POPULATION IS NOW " + p + "\n");
        print("THE CITY NOW OWNS " + a + " ACRES.\n");
        print("YOU HARVESTED " + y + " BUSHELS PER ACRE.\n");
        print("THE RATS ATE " + e + " BUSHELS.\n");
        print("YOU NOW HAVE " + s + " BUSHELS IN STORE.\n");
        print("\n");
        if (z == 11) {
            q = 0;
            break;
        }
        c = Math.floor(10 * Math.random());
        y = c + 17;
        print("LAND IS TRADING AT " + y + " BUSHELS PER ACRE.\n");
        while (1) {
            print("HOW MANY ACRES DO YOU WISH TO BUY");
            q = parseInt(await input());
            if (q < 0)
                break;
            if (y * q > s) {
                exceeded_grain();
            } else
                break;
        }
        if (q < 0)
            break;
        if (q != 0) {
            a += q;
            s -= y * q;
            c = 0;
        } else {
            while (1) {
                print("HOW MANY ACRES DO YOU WISH TO SELL");
                q = parseInt(await input());
                if (q < 0)
                    break;
                if (q >= a) {
                    exceeded_acres();
                } else {
                    break;
                }
            }
            if (q < 0)
                break;
            a -= q;
            s += y * q;
            c = 0;
        }
        print("\n");
        while (1) {
            print("HOW MANY BUSHELS DO YOU WISH TO FEED YOUR PEOPLE");
            q = parseInt(await input());
            if (q < 0)
                break;
            if (q > s)  // Trying to use more grain than is in silos?
                exceeded_grain();
            else
                break;
        }
        if (q < 0)
            break;
        s -= q;
        c = 1;
        print("\n");
        while (1) {
            print("HOW MANY ACRES DO YOU WISH TO PLANT WITH SEED");
            d = parseInt(await input());
            if (d != 0) {
                if (d < 0)
                    break;
                if (d > a) {    // Trying to plant more acres than you own?
                    exceeded_acres();
                } else {
                    if (Math.floor(d / 2) > s)  // Enough grain for seed?
                        exceeded_grain();
                    else {
                        if (d >= 10 * p) {
                            print("BUT YOU HAVE ONLY " + p + " PEOPLE TO TEND THE FIELDS!  NOW THEN,\n");
                        } else {
                            break;
                        }
                    }
                }
            }
        }
        if (d < 0) {
            q = -1;
            break;
        }
        s -= Math.floor(d / 2);
        c = Math.floor(Math.random() * 5) + 1;
        // A bountiful harvest!
        if (c % 2 == 0) {
            // Rats are running wild!!
            e = Math.floor(s / c);
        }
        s = s - e + h;
        c = Math.floor(Math.random() * 5) + 1;
        // Let's have some babies
        i = Math.floor(c * (20 * a + s) / p / 100 + 1);
        // How many people had full tummies?
        c = Math.floor(q / 20);
        // Horros, a 15% chance of plague
        q = Math.floor(10 * (2 * Math.random() - 0.3));
        if (p < c) {
            d = 0;
            continue;
        }
        // Starve enough for impeachment?
        d = p - c;
        if (d <= 0.45 * p) {
            p1 = ((z - 1) * p1 + d * 100 / p) / z;
            p = c;
            d1 += d;
            continue;
        }
        print("\n");
        print("YOU STARVED " + d + " PEOPLE IN ONE YEAR!!!\n");
        q = 0;
        p1 = 34;
        p = 1;
        break;
    }
    if (q < 0) {
        print("\n");
        print("HAMURABI:  I CANNOT DO WHAT YOU WISH.\n");
        print("GET YOURSELF ANOTHER STEWARD!!!!!\n");
    } else {
        print("IN YOUR 10-YEAR TERM OF OFFICE, " + p1 + " PERCENT OF THE\n");
        print("POPULATION STARVED PER YEAR ON THE AVERAGE, I.E. A TOTAL OF\n");
        print(d1 + " PEOPLE DIED!!\n");
        l = a / p;
        print("YOU STARTED WITH 10 ACRES PER PERSON AND ENDED WITH\n");
        print(l + " ACRES PER PERSON.\n");
        print("\n");
        if (p1 > 33 || l < 7) {
            print("DUE TO THIS EXTREME MISMANAGEMENT YOU HAVE NOT ONLY\n");
            print("BEEN IMPEACHED AND THROWN OUT OF OFFICE BUT YOU HAVE\n");
            print("ALSO BEEN DECLARED NATIONAL FINK!!!!\n");
        } else if (p1 > 10 || l < 9) {
            print("YOUR HEAVY-HANDED PERFORMANCE SMACKS OF NERO AND IVAN IV.\n");
            print("THE PEOPLE (REMIANING) FIND YOU AN UNPLEASANT RULER, AND,\n");
            print("FRANKLY, HATE YOUR GUTS!!\n");
        } else if (p1 > 3 || l < 10) {
            print("YOUR PERFORMANCE COULD HAVE BEEN SOMEWHAT BETTER, BUT\n");
            print("REALLY WASN'T TOO BAD AT ALL. " + Math.floor(p * 0.8 * Math.random()) + " PEOPLE\n");
            print("WOULD DEARLY LIKE TO SEE YOU ASSASSINATED BUT WE ALL HAVE OUR\n");
            print("TRIVIAL PROBLEMS.\n");
        } else {
            print("A FANTASTIC PERFORMANCE!!!  CHARLEMANGE, DISRAELI, AND\n");
            print("JEFFERSON COMBINED COULD NOT HAVE DONE BETTER!\n");
        }
    }
    print("\n");
    print("SO LONG FOR NOW.\n");
    print("\n");
}

main();
