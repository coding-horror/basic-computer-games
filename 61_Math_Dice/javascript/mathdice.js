// MATH DICE
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

// Main program
async function main()
{
    print(tab(31) + "MATH DICE\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS PROGRAM GENERATES SUCCESSIVE PICTURES OF TWO DICE.\n");
    print("WHEN TWO DICE AND AN EQUAL SIGN FOLLOWED BY A QUESTION\n");
    print("MARK HAVE BEEN PRINTED, TYPE YOUR ANSWER AND THE RETURN KEY.\n"),
    print("TO CONCLUDE THE LESSON, TYPE ZERO AS YOUR ANSWER.\n");
    print("\n");
    print("\n");
    n = 0;
    while (1) {
        n++;
        d = Math.floor(6 * Math.random() + 1);
        print(" ----- \n");
        if (d == 1)
            print("I     I\n");
        else if (d == 2 || d == 3)
            print("I *   I\n");
        else
            print("I * * I\n");
        if (d == 2 || d == 4)
            print("I     I\n");
        else if (d == 6)
            print("I * * I\n");
        else
            print("I  *  I\n");
        if (d == 1)
            print("I     I\n");
        else if (d == 2 || d == 3)
            print("I   * I\n");
        else
            print("I * * I\n");
        print(" ----- \n");
        print("\n");
        if (n != 2) {
            print("   +\n");
            print("\n");
            a = d;
            continue;
        }
        t = d + a;
        print("      =");
        t1 = parseInt(await input());
        if (t1 == 0)
            break;
        if (t1 != t) {
            print("NO, COUNT THE SPOTS AND GIVE ANOTHER ANSWER.\n");
            print("      =");
            t1 = parseInt(await input());
            if (t1 != t) {
                print("NO, THE ANSWER IS " + t + "\n");
            }
        }
        if (t1 == t) {
            print("RIGHT!\n");
        }
        print("\n");
        print("THE DICE ROLL AGAIN...\n");
        print("\n");
        n = 0;
    }
}

main();
