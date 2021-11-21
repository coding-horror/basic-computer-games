// FUR TRADER
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

var f = [];
var bs = [, "MINK", "BEAVER", "ERMINE", "FOX"];

function reset_stats()
{
    for (var j = 1; j <= 4; j++)
        f[j] = 0;
}

// Main program
async function main()
{
    print(tab(31) + "FUR TRADER\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    first_time = true;
    while (1) {
        if (first_time) {
            print("YOU ARE THE LEADER OF A FRENCH FUR TRADING EXPEDITION IN \n");
            print("1776 LEAVING THE LAKE ONTARIO AREA TO SELL FURS AND GET\n");
            print("SUPPLIES FOR THE NEXT YEAR.  YOU HAVE A CHOICE OF THREE\n");
            print("FORTS AT WHICH YOU MAY TRADE.  THE COST OF SUPPLIES\n");
            print("AND THE AMOUNT YOU RECEIVE FOR YOUR FURS WILL DEPEND\n");
            print("ON THE FORT THAT YOU CHOOSE.\n");
            i = 600;
            print("DO YOU WISH TO TRADE FURS?\n");
            first_time = false;
        }
        print("ANSWER YES OR NO\t");
        str = await input();
        if (str == "NO")
            break;
        print("\n");
        print("YOU HAVE $" + i + " SAVINGS.\n");
        print("AND 190 FURS TO BEGIN THE EXPEDITION.\n");
        e1 = Math.floor((0.15 * Math.random() + 0.95) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
        b1 = Math.floor((0.25 * Math.random() + 1.00) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
        print("\n");
        print("YOUR 190 FURS ARE DISTRIBUTED AMONG THE FOLLOWING\n");
        print("KINDS OF PELTS: MINK, BEAVER, ERMINE AND FOX.\n");
        reset_stats();
        for (j = 1; j <= 4; j++) {
            print("\n");
            print("HOW MANY " + bs[j] + " PELTS DO YOU HAVE\n");
            f[j] = parseInt(await input());
            f[0] = f[1] + f[2] + f[3] + f[4];
            if (f[0] == 190)
                break;
            if (f[0] > 190) {
                print("\n");
                print("YOU MAY NOT HAVE THAT MANY FURS.\n");
                print("DO NOT TRY TO CHEAT.  I CAN ADD.\n");
                print("YOU MUST START AGAIN.\n");
                break;
            }
        }
        if (f[0] > 190) {
            first_time = true;
            continue;
        }
        print("YOU MAY TRADE YOUR FURS AT FORT 1, FORT 2,\n");
        print("OR FORT 3.  FORT 1 IS FORT HOCHELAGA (MONTREAL)\n");
        print("AND IS UNDER THE PROTECTION OF THE FRENCH ARMY.\n");
        print("FORT 2 IS FORT STADACONA (QUEBEC) AND IS UNDER THE\n");
        print("PROTECTION OF THE FRENCH ARMY.  HOWEVER, YOU MUST\n");
        print("MAKE A PORTAGE AND CROSS THE LACHINE RAPIDS.\n");
        print("FORT 3 IS FORT NEW YORK AND IS UNDER DUTCH CONTROL.\n");
        print("YOU MUST CROSS THROUGH IROQUOIS LAND.\n");
        do {
            print("ANSWER 1, 2, OR 3.\n");
            b = parseInt(await input());
            if (b == 1) {
                print("YOU HAVE CHOSEN THE EASIEST ROUTE.  HOWEVER, THE FORT\n");
                print("IS FAR FROM ANY SEAPORT.  THE VALUE\n");
                print("YOU RECEIVE FOR YOUR FURS WILL BE LOW AND THE COST\n");
                print("OF SUPPLIES HIGHER THAN AT FORTS STADACONA OR NEW YORK.\n");
            } else if (b == 2) {
                print("YOU HAVE CHOSEN A HARD ROUTE.  IT IS, IN COMPARSION,\n");
                print("HARDER THAN THE ROUTE TO HOCHELAGA BUT EASIER THAN\n");
                print("THE ROUTE TO NEW YORK.  YOU WILL RECEIVE AN AVERAGE VALUE\n");
                print("FOR YOUR FURS AND THE COST OF YOUR SUPPLIES WILL BE AVERAGE.\n");
            } else {
                print("YOU HAVE CHOSEN THE MOST DIFFICULT ROUTE.  AT\n");
                print("FORT NEW YORK YOU WILL RECEIVE THE HIGHEST VALUE\n");
                print("FOR YOUR FURS.  THE COST OF YOUR SUPPLIES\n");
                print("WILL BE LOWER THAN AT ALL THE OTHER FORTS.\n");
            }
            if (b >= 1 && b <= 3) {
                print("DO YOU WANT TO TRADE AT ANOTHER FORT?\n");
                print("ANSWER YES OR NO\t");
                str = await input();
                if (str == "YES") {
                    b = 0;
                }
            }
        } while (b < 1 || b > 3) ;
        show_beaver = true;
        show_all = true;
        if (b == 1) {
            i -= 160;
            print("\n");
            m1 = Math.floor((0.2 * Math.random() + 0.7) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
            e1 = Math.floor((0.2 * Math.random() + 0.65) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
            b1 = Math.floor((0.2 * Math.random() + 0.75) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
            d1 = Math.floor((0.2 * Math.random() + 0.8) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
            print("SUPPLIES AT FORT HOCHELAGA COST $150.00.\n");
            print("YOUR TRAVEL EXPENSES TO HOCHELAGA WERE $10.00.\n");
        } else if (b == 2) {
            i -= 140;
            print("\n");
            m1 = Math.floor((0.3 * Math.random() + 0.85) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
            e1 = Math.floor((0.15 * Math.random() + 0.8) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
            b1 = Math.floor((0.2 * Math.random() + 0.9) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
            p = Math.floor(10 * Math.random()) + 1;
            if (p <= 2) {
                f[2] = 0;
                print("YOUR BEAVER WERE TOO HEAVY TO CARRY ACROSS\n");
                print("THE PORTAGE.  YOU HAD TO LEAVE THE PELTS, BUT FOUND\n");
                print("THEM STOLEN WHEN YOU RETURNED.\n");
                show_beaver = false;
            } else if (p <= 6) {
                print("YOU ARRIVED SAFELY AT FORT STADACONA.\n");
            } else if (p <= 8) {
                reset_stats();
                print("YOUR CANOE UPSET IN THE LACHINE RAPIDS.  YOU\n");
                print("LOST ALL YOUR FURS.\n");
                show_all = false;
            } else if (p <= 10) {
                f[4] = 0;
                print("YOUR FOX PELTS WERE NOT CURED PROPERLY.\n");
                print("NO ONE WILL BUY THEM.\n");
            }
            print("SUPPLIES AT FORT STADACONA COST $125.00.\n");
            print("YOUR TRAVEL EXPENSES TO STADACONA WERE $15.00.\n");
            
            d1 = Math.floor((0.2 * Math.random() + 0.8) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
        } else if (b == 3) {
            i -= 105;
            print("\n");
            m1 = Math.floor((0.15 * Math.random() + 1.05) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
            d1 = Math.floor((0.25 * Math.random() + 1.1) * Math.pow(10, 2) + 0.5) / Math.pow(10, 2);
            p = Math.floor(10 * Math.random()) + 1;
            if (p <= 2) {
                print("YOU WERE ATTACKED BY A PARTY OF IROQUOIS.\n");
                print("ALL PEOPLE IN YOUR TRADING GROUP WERE\n");
                print("KILLED.  THIS ENDS THE GAME.\n");
                break;
            } else if (p <= 6) {
                print("YOU WERE LUCKY.  YOU ARRIVED SAFELY\n");
                print("AT FORT NEW YORK.\n");
            } else if (p <= 8) {
                reset_stats();
                print("YOU NARROWLY ESCAPED AN IROQUOIS RAIDING PARTY.\n");
                print("HOWEVER, YOU HAD TO LEAVE ALL YOUR FURS BEHIND.\n");
                show_all = false;
            } else if (p <= 10) {
                b1 /= 2;
                m1 /= 2;
                print("YOUR MINK AND BEAVER WERE DAMAGED ON YOUR TRIP.\n");
                print("YOU RECEIVE ONLY HALF THE CURRENT PRICE FOR THESE FURS.\n");
            }
            print("SUPPLIES AT NEW YORK COST $80.00.\n");
            print("YOUR TRAVEL EXPENSES TO NEW YORK WERE $25.00.\n");
        }
        print("\n");
        if (show_all) {
            if (show_beaver)
                print("YOUR BEAVER SOLD FOR $" + b1 * f[2] + " ");
            print("YOUR FOX SOLD FOR $" + d1 * f[4] + "\n");
            print("YOUR ERMINE SOLD FOR $" + e1 * f[3] + " ");
            print("YOUR MINK SOLD FOR $" + m1 * f[1] + "\n");
        }
        i += m1 * f[1] + b1 * f[2] + e1 * f[3] + d1 * f[4];
        print("\n");
        print("YOU NOW HAVE $" + i + " INCLUDING YOUR PREVIOUS SAVINGS\n");
        print("\n");
        print("DO YOU WANT TO TRADE FURS NEXT YEAR?\n");
    }
}

main();
