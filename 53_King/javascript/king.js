// KING
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

function hate_your_guts()
{
    print("\n");
    print("\n");
    print("OVER ONE THIRD OF THE POPULATION HAS DIED SINCE YOU\n");
    print("WERE ELECTED TO OFFICE. THE PEOPLE (REMAINING)\n");
    print("HATE YOUR GUTS.\n");
}

// Main program
async function main()
{
    print(tab(34) + "KING\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("DO YOU WANT INSTRUCTIONS");
    str = await input();
    n5 = 8;
    if (str == "AGAIN") {
        while (1) {
            print("HOW MANY YEARS HAD YOU BEEN IN OFFICE WHEN INTERRUPTED");
            x5 = parseInt(await input());
            if (x5 == 0)
                return;
            if (x5 < 8)
                break;
            print("   COME ON, YOUR TERM IN OFFICE IS ONLY " + n5 + " YEARS.\n");
        }
        print("HOW MUCH DID YOU HAVE IN THE TREASURY");
        a = parseInt(await input());
        if (a < 0)
            return;
        print("HOW MANY COUNTRYMEN");
        b = parseInt(await input());
        if (b < 0)
            return;
        print("HOW MANY WORKERS");
        c = parseInt(await input());
        if (c < 0)
            return;
        while (1) {
            print("HOW MANY SQUARE MILES OF LAND");
            d = parseInt(await input());
            if (d < 0)
                return;
            if (d > 1000 && d <= 2000)
                break;
            print("   COME ON, YOU STARTED WITH 1000 SQ. MILES OF FARM LAND\n");
            print("   AND 10,000 SQ. MILES OF FOREST LAND.\n");
        }
    } else {
        if (str.substr(0, 1) != "N") {
            print("\n");
            print("\n");
            print("\n");
            print("CONGRATULATIONS! YOU'VE JUST BEEN ELECTED PREMIER OF SETATS\n");
            print("DETINU, A SMALL COMMUNIST ISLAND 30 BY 70 MILES LONG. YOUR\n");
            print("JOB IS TO DECIDE UPON THE CONTRY'S BUDGET AND DISTRIBUTE\n");
            print("MONEY TO YOUR COUNTRYMEN FROM THE COMMUNAL TREASURY.\n");
            print("THE MONEY SYSTEM IS RALLODS, AND EACH PERSON NEEDS 100\n");
            print("RALLODS PER YEAR TO SURVIVE. YOUR COUNTRY'S INCOME COMES\n");
            print("FROM FARM PRODUCE AND TOURISTS VISITING YOUR MAGNIFICENT\n");
            print("FORESTS, HUNTING, FISHING, ETC. HALF YOUR LAND IS FARM LAND\n");
            print("WHICH ALSO HAS AN EXCELLENT MINERAL CONTENT AND MAY BE SOLD\n");
            print("TO FOREIGN INDUSTRY (STRIP MINING) WHO IMPORT AND SUPPORT\n");
            print("THEIR OWN WORKERS. CROPS COST BETWEEN 10 AND 15 RALLODS PER\n");
            print("SQUARE MILE TO PLANT.\n");
            print("YOUR GOAL IS TO COMPLETE YOUR " + n5 + " YEAR TERM OF OFFICE.\n");
            print("GOOD LUCK!\n");
        }
        print("\n");
        a = Math.floor(60000 + (1000 * Math.random()) - (1000 * Math.random()));
        b = Math.floor(500 + (10 * Math.random()) - (10 * Math.random()));
        c = 0;
        d = 2000;
        x5 = 0;
    }
    v3 = 0;
    b5 = 0;
    x = false;
    while (1) {
        w = Math.floor(10 * Math.random() + 95);
        print("\n");
        print("YOU NOW HAVE " + a + " RALLODS IN THE TREASURY.\n");
        print(b + " COUNTRYMEN, ");
        v9 = Math.floor(((Math.random() / 2) * 10 + 10));
        if (c != 0)
            print(c + " FOREIGN WORKERS, ");
        print("AND " + Math.floor(d) + " SQ. MILES OF LAND.\n");
        print("THIS YEAR INDUSTRY WILL BUY LAND FOR " + w + " ");
        print("RALLODS PER SQUARE MILE.\n");
        print("LAND CURRENTLY COSTS " + v9 + " RALLODS PER SQUARE MILE TO PLANT.\n");
        print("\n");
        while (1) {
            print("HOW MANY SQUARE MILES DO YOU WISH TO SELL TO INDUSTRY");
            h = parseInt(await input());
            if (h < 0)
                continue;
            if (h <= d - 1000)
                break;
            print("***  THINK AGAIN. YOU ONLY HAVE " + (d - 1000) + " SQUARE MILES OF FARM LAND.\n");
            if (x == false) {
                print("\n");
                print("(FOREIGN INDUSTRY WILL ONLY BUY FARM LAND BECAUSE\n");
                print("FOREST LAND IS UNECONOMICAL TO STRIP MINE DUE TO TREES,\n");
                print("THICKER TOP SOIL, ETC.)\n");
                x = true;
            }
        }
        d = Math.floor(d - h);
        a = Math.floor(a + (h * w));
        while (1) {
            print("HOW MANY RALLODS WILL YOU DISTRIBUTE AMONG YOUR COUNTRYMEN");
            i = parseInt(await input());
            if (i < 0)
                continue;
            if (i < a)
                break;
            if (i == a) {
                j = 0;
                k = 0;
                a = 0;
                break;
            }
            print("   THINK AGAIN. YOU'VE ONLY " + a + " RALLODS IN THE TREASURY\n");
        }
        if (a) {
            a = Math.floor(a - i);
            while (1) {
                print("HOW MANY SQUARE MILES DO YOU WISH TO PLANT");
                j = parseInt(await input());
                if (j < 0)
                    continue;
                if (j <= b * 2) {
                    if (j <= d - 1000) {
                        u1 = Math.floor(j * v9);
                        if (u1 > a) {
                            print("   THINK AGAIN. YOU'VE ONLY " + a + " RALLODS LEFT IN THE TREASURY.\n");
                            continue;
                        } else if (u1 == a) {
                            k = 0;
                            a = 0;
                        }
                        break;
                    }
                    print("   SORRY, BUT YOU'VE ONLY " + (d - 1000) + " SQ. MILES OF FARM LAND.\n");
                    continue;
                }
                print("   SORRY, BUT EACH COUNTRYMAN CAN ONLY PLANT 2 SQ. MILES.\n");
            }
        }
        if (a) {
            a -= u1;
            while (1) {
                print("HOW MANY RALLODS DO YOU WISH TO SPEND ON POLLUTION CONTROL");
                k = parseInt(await input());
                if (k < 0)
                    continue;
                if (k <= a)
                    break;
                print("   THINK AGAIN. YOU ONLY HAVE " + a + " RALLODS REMAINING.\n");
            }
        }
        if (h == 0 && i == 0 && j == 0 && k == 0) {
            print("GOODBYE.\n");
            print("(IF YOU WISH TO CONTINUE THIS GAME AT A LATER DATE, ANSWER\n");
            print("'AGAIN' WHEN ASKED IF YOU WANT INSTRUCTIONS AT THE START\n");
            print("OF THE GAME).\n");
            return;
        }
        print("\n");
        print("\n");
        a = Math.floor(a - k);
        a4 = a;
        if (Math.floor(i / 100 - b) < 0) {
            if (i / 100 < 50) {
                hate_your_guts();
                break;
            }
            print(Math.floor(b - (i / 100)) + " COUNTRYMEN DIED OF STARVATION\n");
        }
        f1 = Math.floor(Math.random() * (2000 - d));
        if (k >= 25)
            f1 = Math.floor(f1 / (k / 25));
        if (f1 > 0)
            print(f1 + " COUNTRYMEN DIED OF CARBON-MONOXIDE AND DUST INHALATION\n");
        funeral = false;
        if (Math.floor((i / 100) - b) >= 0) {
            if (f1 > 0) {
                print("   YOU WERE FORCED TO SPEND " + Math.floor(f1 * 9) + " RALLODS ON ");
                print("FUNERAL EXPENSES.\n");
                b5 = f1;
                a = Math.floor(a - (f1 * 9));
                funeral = true;
            }
        } else {
            print("   YOU WERE FORCED TO SPEND " + Math.floor((f1 + (b - (i / 100))) * 9));
            print(" RALLODS ON FUNERAL EXPENSES.\n");
            b5 = Math.floor(f1 + (b - (i / 100)));
            a = Math.floor(a - ((f1 + (b - (i / 100))) * 9));
            funeral = true;
        }
        if (funeral) {
            if (a < 0) {
                print("   INSUFFICIENT RESERVES TO COVER COST - LAND WAS SOLD\n");
                d = Math.floor(d + (a / w));
                a = 0;
            }
            b = Math.floor(b - b5);
        }
        c1 = 0;
        if (h != 0) {
            c1 = Math.floor(h + (Math.random() * 10) - (Math.random() * 20));
            if (c <= 0)
                c1 += 20;
            print(c1 + " WORKERS CAME TO THE COUNTRY AND ");
        }
        p1 = Math.floor(((i / 100 - b) / 10) + (k / 25) - ((2000 - d) / 50) - (f1 / 2));
        print(Math.abs(p1) + " COUNTRYMEN ");
        if (p1 >= 0)
            print("CAME TO");
        else
            print("LEFT");
        print(" THE ISLAND.\n");
        b = Math.floor(b + p1);
        c = Math.floor(c + c1);
        u2 = Math.floor(((2000 - d) * ((Math.random() + 1.5) / 2)));
        if (c != 0) {
            print("OF " + Math.floor(j) + " SQ. MILES PLANTED,");
        }
        if (j <= u2)
            u2 = j;
        print(" YOU HARVESTED " + Math.floor(j - u2) + " SQ. MILES OF CROPS.\n");
        if (u2 != 0 && t1 < 2) {
            print("   (DUE TO ");
            if (t1 != 0)
                print("INCREASED ");
            print("AIR AND WATER POLLUTION FROM FOREIGN INDUSTRY.)\n");
        }
        q = Math.floor((j - u2) * (w / 2));
        print("MAKING " + q + " RALLODS.\n");
        a = Math.floor(a + q);
        v1 = Math.floor(((b - p1) * 22) + (Math.random() * 500));
        v2 = Math.floor((2000 - d) * 15);
        print(" YOU MADE " + Math.abs(Math.floor(v1 - v2)) + " RALLODS FROM TOURIST TRADE.\n");
        if (v2 != 0 && v1 - v2 < v3) {
            print("   DECREASE BECAUSE ");
            g1 = 10 * Math.random();
            if (g1 <= 2)
                print("FISH POPULATION HAS DWINDLED DUE TO WATER POLLUTION.\n");
            else if (g1 <= 4)
                print("AIR POLLUTION IS KILLING GAME BIRD POPULATION.\n");
            else if (g1 <= 6)
                print("MINERAL BATHS ARE BEING RUINED BY WATER POLLUTION.\n");
            else if (g1 <= 8)
                print("UNPLEASANT SMOG IS DISCOURAGING SUN BATHERS.\n");
            else if (g1 <= 10)
                print("HOTELS ARE LOOKING SHABBY DUE TO SMOG GRIT.\n");
        }
        v3 = Math.floor(a + v3);    // Probable bug from original game
        a = Math.floor(a + v3);
        if (b5 > 200) {
            print("\n");
            print("\n");
            print(b5 + " COUNTRYMEN DIED IN ONE YEAR!!!!!\n");
            print("DUE TO THIS EXTREME MISMANAGEMENT, YOU HAVE NOT ONLY\n");
            print("BEEN IMPEACHED AND THROWN OUT OF OFFICE, BUT YOU\n");
            m6 = Math.floor(Math.random() * 10);
            if (m6 <= 3)
                print("ALSO HAD YOUR LEFT EYE GOUGED OUT!\n");
            else if (m6 <= 6)
                print("HAVE ALSO GAINED A VERY BAD REPUTATION.\n");
            else
                print("HAVE ALSO BEEN DECLARED NATIONAL FINK.\n");
            print("\n");
            print("\n");
            return;
        }
        if (b < 343) {
            hate_your_guts();
            break;
        }
        if (a4 / 100 > 5 && b5 - f1 >= 2) {
            print("\n");
            print("MONEY WAS LEFT OVER IN THE TREASURY WHICH YOU DID\n");
            print("NOT SPEND. AS A RESULT, SOME OF YOUR COUNTRYMEN DIED\n");
            print("OF STARVATION. THE PUBLIC IS ENRAGED AND YOU HAVE\n");
            print("BEEN FORCED TO EITHER RESIGN OR COMMIT SUICIDE.\n");
            print("THE CHOICE IS YOURS.\n");
            print("IF YOU CHOOSE THE LATTER, PLEASE TURN OFF YOUR COMPUTER\n");
            print("BEFORE PROCEEDING.\n");
            print("\n");
            print("\n");
            return;
        }
        if (c > b) {
            print("\n");
            print("\n");
            print("THE NUMBER OF FOREIGN WORKERS HAS EXCEEDED THE NUMBER\n");
            print("OF COUNTRYMEN. AS A MINORITY, THEY HAVE REVOLTED AND\n");
            print("TAKEN OVER THE COUNTRY.\n");
            break;
        }
        if (n5 - 1 == x5) {
            print("\n");
            print("\n");
            print("CONGRATULATIONS!!!!!!!!!!!!!!!!!!\n");
            print("YOU HAVE SUCCESFULLY COMPLETED YOUR " + n5 + " YEAR TERM\n");
            print("OF OFFICE. YOU WERE, OF COURSE, EXTREMELY LUCKY, BUT\n");
            print("NEVERTHELESS, IT'S QUITE AN ACHIEVEMENT. GOODBYE AND GOOD\n");
            print("LUCK - YOU'LL PROBABLY NEED IT IF YOU'RE THE TYPE THAT\n");
            print("PLAYS THIS GAME.\n");
            print("\n");
            print("\n");
            return;
        }
        x5++;
        b5 = 0;
    }
    if (Math.random() <= 0.5) {
        print("YOU HAVE BEEN ASSASSINATED.\n");
    } else {
        print("YOU HAVE BEEN THROWN OUT OF OFFICE AND ARE NOW\n");
        print("RESIDING IN PRISON.\n");
    }
    print("\n");
    print("\n");
}

main();
