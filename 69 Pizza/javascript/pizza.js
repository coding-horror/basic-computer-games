// PIZZA
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

var sa = [, "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P"];
var ma = [, "1","2","3","4"];
var a = [];

// Main program
async function main()
{
    print(tab(33) + "PIZZA\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("PIZZA DELIVERY GAME\n");
    print("\n");
    print("WHAT IS YOUR FIRST NAME");
    ns = await input();
    print("\n");
    print("HI, " + ns + ". IN THIS GAME YOU ARE TO TAKE ORDERS\n");
    print("FOR PIZZAS.  THEN YOU ARE TO TELL A DELIVERY BOY\n");
    print("WHERE TO DELIVER THE ORDERED PIZZAS.\n");
    print("\n");
    print("\n");
    print("MAP OF THE CITY OF HYATTSVILLE\n");
    print("\n");
    print(" -----1-----2-----3-----4-----\n");
    k = 4;
    for (i = 1; i <= 4; i++) {
        print("-\n");
        print("-\n");
        print("-\n");
        print("-\n");
        print(ma[k]);
        s1 = 16 - 4 * i + 1;
        print("     " + sa[s1] + "     " + sa[s1 + 1] + "     " + sa[s1 + 2] + "     ");
        print(sa[s1 + 3] + "     " + ma[k] + "\n");
        k--;
    }
    print("-\n");
    print("-\n");
    print("-\n");
    print("-\n");
    print(" -----1-----2-----3-----4-----\n");
    print("\n");
    print("THE OUTPUT IS A MAP OF THE HOMES WHERE\n");
    print("YOU ARE TO SEND PIZZAS.\n");
    print("\n");
    print("YOUR JOB IS TO GIVE A TRUCK DRIVER\n");
    print("THE LOCATION OR COORDINATES OF THE\n");
    print("HOME ORDERING THE PIZZA.\n");
    print("\n");
    while (1) {
        print("DO YOU NEED MORE DIRECTIONS");
        str = await input();
        if (str == "YES" || str == "NO")
            break;
        print("'YES' OR 'NO' PLEASE, NOW THEN, ");
    }
    if (str == "YES") {
        print("\n");
        print("SOMEBODY WILL ASK FOR A PIZZA TO BE\n");
        print("DELIVERED.  THEN A DELIVERY BOY WILL\n");
        print("ASK YOU FOR THE LOCATION.\n");
        print("     EXAMPLE:\n");
        print("THIS IS J.  PLEASE SEND A PIZZA.\n");
        print("DRIVER TO " + ns + ".  WHERE DOES J LIVE?\n");
        print("YOUR ANSWER WOULD BE 2,3\n");
        print("\n");
        print("UNDERSTAND");
        str = await input();
        if (str != "YES") {
            print("THIS JOB IS DEFINITELY TOO DIFFICULT FOR YOU. THANKS ANYWAY");
            return;
        }
        print("GOOD.  YOU ARE NOW READY TO START TAKING ORDERS.\n");
        print("\n");
        print("GOOD LUCK!!\n");
        print("\n");
    }
    while (1) {
        for (i = 1; i <= 5; i++) {
            s = Math.floor(Math.random() * 16 + 1);
            print("\n");
            print("HELLO " + ns + "'S PIZZA.  THIS IS " + sa[s] + ".\n");
            print("  PLEASE SEND A PIZZA.\n");
            while (1) {
                print("  DRIVER TO " + ns + ":  WHERE DOES " + sa[s] + " LIVE");
                str = await input();
                a[1] = parseInt(str);
                a[2] = parseInt(str.substr(str.indexOf(",") + 1));
                t = a[1] + (a[2] - 1) * 4;
                if (t != s) {
                    print("THIS IS " + sa[t] + ". I DID NOT ORDER A PIZZA.\n");
                    print("I LIVE AT " + a[1] + "," + a[2] + "\n");
                } else {
                    break;
                }
            }
            print("HELLO " + ns + ".  THIS IS " + sa[s] + ", THANKS FOR THE PIZZA.\n");
        }
        print("\n");
        print("DO YOU WANT TO DELIVER MORE PIZZAS");
        str = await input();
        if (str != "YES")
            break;
    }
    print("\n");
    print("O.K. " + ns + ", SEE YOU LATER!\n");
    print("\n");
}

main();
