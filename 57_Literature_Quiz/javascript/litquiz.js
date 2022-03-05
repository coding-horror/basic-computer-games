// LITQUIZ
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
    print(tab(25) + "LITERATURE QUIZ\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    r = 0;
    print("TEST YOUR KNOWLEDGE OF CHILDREN'S LITERATURE.\n");
    print("\n");
    print("THIS IS A MULTIPLE-CHOICE QUIZ.\n");
    print("TYPE A 1, 2, 3, OR 4 AFTER THE QUESTION MARK.\n");
    print("\n");
    print("GOOD LUCK!\n");
    print("\n");
    print("\n");
    print("IN PINOCCHIO, WHAT WAS THE NAME OF THE CAT\n");
    print("1)TIGGER, 2)CICERO, 3)FIGARO, 4)GUIPETTO\n");
    a = parseInt(await input());
    if (a == 3) {
        print("VERY GOOD!  HERE'S ANOTHER.\n");
        r++;
    } else {
        print("SORRY...FIGARO WAS HIS NAME.\n");
    }
    print("\n");
    print("\n");
    print("FROM WHOSE GARDEN DID BUGS BUNNY STEAL THE CARROTS?\n");
    print("1)MR. NIXON'S, 2)ELMER FUDD'S, 3)CLEM JUDD'S, 4)STROMBOLI'S\n");
    a = parseInt(await input());
    if (a == 2) {
        print("PRETTY GOOD!\n");
        r++;
    } else {
        print("TOO BAD...IT WAS ELMER FUDD'S GARDEN.\n");
    }
    print("\n");
    print("\n");
    print("IN THE WIZARD OF OS, DOROTHY'S DOG WAS NAMED\n");
    print("1)CICERO, 2)TRIXIA, 3)KING, 4)TOTO\n");
    a = parseInt(await input());
    if (a == 4) {
        print("YEA!  YOU'RE A REAL LITERATURE GIANT.\n");
        r++;
    } else {
        print("BACK TO THE BOOKS,...TOTO WAS HIS NAME.\n");
    }
    print("\n");
    print("\n");
    print("WHO WAS THE FAIR MAIDEN WHO ATE THE POISON APPLE\n");
    print("1)SLEEPING BEAUTY, 2)CINDERELLA, 3)SNOW WHITE, 4)WENDY\n");
    a = parseInt(await input());
    if (a == 3) {
        print("GOOD MEMORY!\n");
        r++;
    } else {
        print("OH, COME ON NOW...IT WAS SNOW WHITE.\n");
    }
    print("\n");
    print("\n");
    if (r == 4) {
        print("WOW!  THAT'S SUPER!  YOU REALLY KNOW YOUR NURSERY\n");
        print("YOUR NEXT QUIZ WILL BE ON 2ND CENTURY CHINESE\n");
        print("LITERATURE (HA, HA, HA)\n");
    } else if (r < 2) {
        print("UGH.  THAT WAS DEFINITELY NOT TOO SWIFT.  BACK TO\n");
        print("NURSERY SCHOOL FOR YOU, MY FRIEND.\n");
    } else {
        print("NOT BAD, BUT YOU MIGHT SPEND A LITTLE MORE TIME\n");
        print("READING THE NURSERY GREATS.\n");
    }
}

main();
