// NAME
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

var str;
var b;

// Main program
async function main()
{
    print(tab(34) + "NAME\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("HELLO.\n");
    print("MY NAME IS CREATIVE COMPUTER.\n");
    print("WHAT'S YOUR NAME (FIRST AND LAST)");
    str = await input();
    l = str.length;
    print("\n");
    print("THANK YOU, ");
    for (i = l; i >= 1; i--)
        print(str[i - 1]);
    print(".\n");
    print("OOPS!  I GUESS I GOT IT BACKWARDS.  A SMART\n");
    print("COMPUTER LIKE ME SHOULDN'T MAKE A MISTAKE LIKE THAT!\n");
    print("\n");
    print("BUT I JUST NOTICED YOUR LETTERS ARE OUT OF ORDER.\n");
    print("LET'S PUT THEM IN ORDER LIKE THIS: ");
    b = [];
    for (i = 1; i <= l; i++)
        b[i - 1] = str.charCodeAt(i - 1);
    b.sort();
    for (i = 1; i <= l; i++)
        print(String.fromCharCode(b[i - 1]));
    print("\n");
    print("\n");
    print("DON'T YOU LIKE THAT BETTER");
    ds = await input();
    if (ds == "YES") {
        print("\n");
        print("I KNEW YOU'D AGREE!!\n");
    } else {
        print("\n");
        print("I'M SORRY YOU DON'T LIKE IT THAT WAY.\n");
    }
    print("\n");
    print("I REALLY ENJOYED MEETING YOU " + str + ".\n");
    print("HAVE A NICE DAY!\n");
}

main();
