// BUZZWORD
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

var a = ["",
         "ABILITY","BASAL","BEHAVIORAL","CHILD-CENTERED",
         "DIFFERENTIATED","DISCOVERY","FLEXIBLE","HETEROGENEOUS",
         "HOMOGENEOUS","MANIPULATIVE","MODULAR","TAVISTOCK",
         "INDIVIDUALIZED","LEARNING","EVALUATIVE","OBJECTIVE",
         "COGNITIVE","ENRICHMENT","SCHEDULING","HUMANISTIC",
         "INTEGRATED","NON-GRADED","TRAINING","VERTICAL AGE",
         "MOTIVATIONAL","CREATIVE","GROUPING","MODIFICATION",
         "ACCOUNTABILITY","PROCESS","CORE CURRICULUM","ALGORITHM",
         "PERFORMANCE","REINFORCEMENT","OPEN CLASSROOM","RESOURCE",
         "STRUCTURE","FACILITY","ENVIRONMENT",
         ];

// Main program
async function main()
{
    print(tab(26) + "BUZZWORD GENERATOR\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("THIS PROGRAM PRINTS HIGHLY ACCEPTABLE PHRASES IN\n");
    print("'EDUCATOR-SPEAK' THAT YOU CAN WORK INTO REPORTS\n");
    print("AND SPEECHES.  WHENEVER A QUESTION MARK IS PRINTED,\n");
    print("TYPE A 'Y' FOR ANOTHER PHRASE OR 'N' TO QUIT.\n");
    print("\n");
    print("\n");
    print("HERE'S THE FIRST PHRASE:\n");
    do {
        print(a[Math.floor(Math.random() * 13 + 1)] + " ");
        print(a[Math.floor(Math.random() * 13 + 14)] + " ");
        print(a[Math.floor(Math.random() * 13 + 27)] + "\n");
        print("\n");
        y = await input();
    } while (y == "Y") ;
    print("COME BACK WHEN YOU NEED HELP WITH ANOTHER REPORT!\n");
}

main();
