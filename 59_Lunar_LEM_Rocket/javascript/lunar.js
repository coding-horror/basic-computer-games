// LUNAR
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

var l;
var t;
var m;
var s;
var k;
var a;
var v;
var i;
var j;
var q;
var g;
var z;
var d;

function formula_set_1()
{
    l = l + s;
    t = t - s;
    m = m - s * k;
    a = i;
    v = j;
}

function formula_set_2()
{
    q = s * k / m;
    j = v + g * s + z * (-q - q * q / 2 - Math.pow(q, 3) / 3 - Math.pow(q, 4) / 4 - Math.pow(q, 5) / 5);
    i = a - g * s * s / 2 - v * s + z * s * (q / 2 + Math.pow(q, 2) / 6 + Math.pow(q, 3) / 12 + Math.pow(q, 4) / 20 + Math.pow(q, 5) / 30);
}

function formula_set_3()
{
    while (s >= 5e-3) {
        d = v + Math.sqrt(v * v + 2 * a * (g - z * k / m));
        s = 2 * a / d;
        formula_set_2();
        formula_set_1();
    }
}

// Main program
async function main()
{
    print(tab(33) + "LUNAR\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS IS A COMPUTER SIMULATION OF AN APOLLO LUNAR\n");
    print("LANDING CAPSULE.\n");
    print("\n");
    print("\n");
    print("THE ON-BOARD COMPUTER HAS FAILED (IT WAS MADE BY\n");
    print("XEROX) SO YOU HAVE TO LAND THE CAPSULE MANUALLY.\n");
    while (1) {
        print("\n");
        print("SET BURN RATE OF RETRO ROCKETS TO ANY VALUE BETWEEN\n");
        print("0 (FREE FALL) AND 200 (MAXIMUM BURN) POUNDS PER SECOND.\n");
        print("SET NEW BURN RATE EVERY 10 SECONDS.\n");
        print("\n");
        print("CAPSULE WEIGHT 32,500 LBS; FUEL WEIGHT 16,500 LBS.\n");
        print("\n");
        print("\n");
        print("\n");
        print("GOOD LUCK\n");
        l = 0;
        print("\n");
        print("SEC\tMI + FT\t\tMPH\tLB FUEL\tBURN RATE\n");
        print("\n");
        a = 120;
        v = 1;
        m = 32500;
        n = 16500;
        g = 1e-3;
        z = 1.8;
        while (1) {
            print(l + "\t" + Math.floor(a) + " + " + Math.floor(5280 * (a - Math.floor(a))) + " \t" + Math.floor(3600 * v * 100) / 100 + "\t" + (m - n) + "\t");
            k = parseFloat(await input());
            t = 10;
            should_exit = false;
            while (1) {
                if (m - n < 1e-3)
                    break;
                if (t < 1e-3)
                    break;
                s = t;
                if (m < n + s * k)
                    s = (m - n) / k;
                formula_set_2();
                if (i <= 0) {
                    formula_set_3();
                    should_exit = true;
                    break;
                }
                if (v > 0) {
                    if (j < 0) {
                        do {
                            w = (1 - m * g / (z * k)) / 2;
                            s = m * v / (z * k * (w + Math.sqrt(w * w + v / z))) + 0.05;
                            formula_set_2();
                            if (i <= 0) {
                                formula_set_3();
                                should_exit = true;
                                break;
                            }
                            formula_set_1();
                            if (j > 0)
                                break;
                        } while (v > 0) ;
                        if (should_exit)
                            break;
                        continue;
                    }
                }
                formula_set_1();
            }
            if (should_exit)
                break;
            if (m - n < 1e-3) {
                print("FUEL OUT AT " + l + " SECOND\n");
                s = (-v * Math.sqrt(v * v + 2 * a * g)) / g;
                v = v + g * s;
                l = l + s;
                break;
            }
        }
        w = 3600 * v;
        print("ON MOON AT " + l + " SECONDS - IMPACT VELOCITY " + w + " MPH\n");
        if (w <= 1.2) {
            print("PERFECT LANDING!\n");
        } else if (w <= 10) {
            print("GOOD LANDING (COULD BE BETTER)\n");
        } else if (w <= 60) {
            print("CRAFT DAMAGE... YOU'RE STRANDED HERE UNTIL A RESCUE\n");
            print("PARTY ARRIVES. HOPE YOU HAVE ENOUGH OXYGEN!\n");
        } else {
            print("SORRY THERE WERE NO SURVIVORS. YOU BLEW IT!\n");
            print("IN FACT, YOU BLASTED A NEW LUNAR CRATER " + (w * 0.227) + " FEET DEEP!\n");
        }
        print("\n");
        print("\n");
        print("\n");
        print("TRY AGAIN??\n");
    }
}

main();
